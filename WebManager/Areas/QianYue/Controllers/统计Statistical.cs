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

namespace WebProject.Areas.QianYue.Controllers
{
    /// <summary>
    /// 签约统计
    /// </summary>
    public class StatisticalController : BaseLoginController
    {
        /// <summary>
        /// 数据总表
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="organize_id"></param>
        /// <returns></returns>
        public ActionResult Total(string c_date, int organize_id = 0)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM");
            }
            if (organize_id == 0)
            {
                organize_id = DoMySql.FindEntity<ModelDb.sys_organize>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and parent_id = 0 and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by sort,id").id;
            }

            ViewBag.c_date = c_date;
            ViewBag.organize_id = organize_id;

            return View();
        }
    }
}