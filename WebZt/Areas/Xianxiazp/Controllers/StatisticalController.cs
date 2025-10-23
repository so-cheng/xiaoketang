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

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    public class StatisticalController : BaseLoginController
    {
        /// <summary>
        /// 月度统计
        /// </summary>
        /// <param name="c_date"></param>
        /// <returns></returns>
        public ActionResult Total(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM");
            }

            ViewBag.c_date = c_date;
            return View();
        }
    }
}