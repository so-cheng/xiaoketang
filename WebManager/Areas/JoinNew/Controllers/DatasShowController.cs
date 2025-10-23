using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 数据展示
    /// </summary>
    public class DatasShowController : BaseLoginController
    {
        #region 分配概况
        /// <summary>
        /// 分配概况-所有未分
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitOverview()
        {
            return View();
        }

        /// <summary>
        /// 分配概况-当天数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DayGetview(string c_date)
        {
            //默认为今天
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_s = c_date.Substring(0, c_date.IndexOf("~") - 1);
            ViewBag.c_date_e = c_date.Substring(c_date.IndexOf("~") + 2);
            return View();
        }
        #endregion

        #region 团队数据

        /// <summary>
        /// 审批情况统计
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="yy_user_sn"></param>
        /// <returns></returns>
        public ActionResult ApproveSumTable(string dateRange = "")
        {
            ViewBag.dateRange = dateRange;
            return View();
        }

        #endregion
    }
}