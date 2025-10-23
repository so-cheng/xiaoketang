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
        /// 厅每日核心数据
        /// </summary>
        /// <param name="req"></param>
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

        /// <summary>
        /// 厅每日核心数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult ItemM(string c_time, string ting_sn)
        {
            if (c_time.IsNullOrEmpty())
            {
                c_time = "yesterday";
            }
            ViewBag.c_time = c_time;
            ViewBag.ting_sn = ting_sn;
            return View();
        }


        /// <summary>
        /// 厅每日目标完成情况
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TingIncomeDayTarget(string date)
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.date = date; 
            return View();
        }

    }
}