using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Jiezou.Controllers
{
    public class JiezouDayController : BaseLoginController
    {
        /// <summary>
        /// 厅管节奏展示
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult Item(string c_date, string ting_sn)
        {
            if (ting_sn.IsNullOrEmpty())
            {
                ting_sn = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn).First().Value;
            }
            ViewBag.ting_sn = ting_sn;
            ViewBag.c_date = c_date;
            return View();
        }
        /// <summary>
        /// 厅管节奏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleDefinition()
        {
            var ruleList = DoMySql.FindList<ModelDb.jiezou_detail_rule>($"type = '{ModelDb.jiezou_detail_rule.type_enum.日节奏统计规则.ToSByte()}'");
            ViewBag.ruleList = ruleList;
            return View();
        }
        /// <summary>
        /// 厅管节奏阶段展示
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult Detail(string c_date, string ting_sn)
        {
            ViewBag.c_date = c_date;
            ViewBag.ting_sn = ting_sn;
            return View();
        }
    }
}