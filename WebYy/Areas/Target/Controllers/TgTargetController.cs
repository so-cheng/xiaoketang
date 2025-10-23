using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using static WeiCode.ModelDbs.ModelDb;

namespace WebProject.Areas.Target.Controllers
{
    /// <summary>
    /// 厅管目标
    /// </summary>
    public class TgTargetController : BaseLoginController
    {
        public ActionResult IncomeMonth(string date)
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.date = date;
            return View();
        }

        public ActionResult GrowthMonth(string date)
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.date = date;
            return View();
        }

        public ActionResult GrowthYearMonth()
        {
            return View();
        }
    }
}