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

namespace WebProject.Areas.DataAnalysis.Controllers
{
    public class TingCoreDataController : BaseLoginController
    {
        /// <summary>
        /// 厅每日核心数据指标
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        /// <returns></returns>
        public ActionResult Item(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }

            ViewBag.c_date = c_date;
            return View();
        }
    }
}