using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas.Jixiao.Controllers
{
    public class RankController : BaseLoginController
    {
        // GET: Jixiao/Rank
        public ActionResult Index()
        {
            return View();
        }
        private string GetDateRange(string dateRange)
        {
            var Monday = DateTime.Today;
            while (Monday.DayOfWeek != DayOfWeek.Monday)
            {
                Monday = Monday.AddDays(-1);
            }
            if (dateRange.IsNullOrEmpty())
            {
                dateRange = Monday.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.Monday = Monday;
            ViewBag.dateRange = dateRange;
            return dateRange;
        }
        /// <summary>
        /// 规则、备注
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleDefinition()
        {
            return View();
        }
        public ActionResult TingRuleDefinition()
        {
            return View();
        }
        public ActionResult YyRuleDefinition()
        {
            return View();
        }

        #region 主播绩效排名
        public ActionResult ZbNum2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult ZbAmount2RankTable(string dateRange, string option)
        {
            dateRange = GetDateRange(dateRange);
            if (option.IsNullOrEmpty())
            {
                option = "1";
            }
            ViewBag.option = option;
            return View();
        }
        public ActionResult ZbContactNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult ZbNewNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        #endregion
        #region 厅管绩效排名

        public ActionResult TgNum2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult TgAmount2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult TgContactNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult TgNewNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        #endregion

        #region 运营绩效排名
        public ActionResult YyNum2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult YyAmount2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult YyContactNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult YyNewNumRankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        #endregion
    }
}