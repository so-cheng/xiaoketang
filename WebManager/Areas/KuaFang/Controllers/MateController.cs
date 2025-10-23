using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using WeiCode.Utility;

namespace WebProject.Areas.KuaFang.Controllers
{
    public class MateController : BaseLoginController
    {
        /// <summary>
        /// 匹配列表
        /// </summary>
        /// <param name="amont_s"></param>
        /// <param name="amont_e"></param>
        /// <param name="ting_name"></param>
        /// <returns></returns>
        public ActionResult List(string amont_s, string amont_e, string ting_name)
        {
            if (!amont_s.IsValidNumeric())
            {
                amont_s = null;
            }
            if (!amont_e.IsValidNumeric())
            {
                amont_e = null;
            }

            var where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {new ServiceFactory.KuaFang.Common().getNewKuaFang().id} and (ting_sn2 is null or ting_sn2 = '')";

            if (!amont_s.IsNullOrEmpty())
            {
                where += $" and amont >= {amont_s}";
            }
            if (!amont_e.IsNullOrEmpty())
            {
                where += $" and amont <= {amont_e}";
            }
            if (!ting_name.IsNullOrEmpty())
            {
                where += $" and ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter { attachWhere = $"ting_name like '%{ting_name}%'" })}";
            }

            var list = DoMySql.FindList<ModelDb.p_kuafang_mate>(where);
            ViewBag.mate_list = list;
            ViewBag.amont_s = amont_s;
            ViewBag.amont_e = amont_e;
            ViewBag.ting_name = ting_name;

            return View();
        }

        /// <summary>
        /// 对战列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MateList()
        {
            var req = new PageFactory.KuaFangMate.List.DtoReq();
            var pageModel = new PageFactory.KuaFangMate.List().Get(req);
            return View(pageModel);
        }
    }
}