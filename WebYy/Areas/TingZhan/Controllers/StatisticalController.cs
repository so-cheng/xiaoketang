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
using static Services.Project.ServiceFactory;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 统计报表
    /// </summary>
    public class StatisticalController : BaseLoginController
    {
        /// <summary>
        /// 统计报表-数据统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Analysis(int id = 0)
        {
            if (id > 0)
            {
                var targets = DoMySql.FindListBySql<GradeDto>($"SELECT yy_user_sn,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {id} and yy_user_sn = '{new UserIdentityBag().user_sn}' group by yy_user_sn order by id desc");
                ViewBag.targets = targets;
            }
            else
            {
                var p_tingzhan = new TingZhanService().getNewTingzhan();
                if (!p_tingzhan.IsNullOrEmpty())
                {
                    var targets = DoMySql.FindListBySql<GradeDto>($"SELECT yy_user_sn,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} and yy_user_sn = '{new UserIdentityBag().user_sn}' group by yy_user_sn order by id desc");
                    ViewBag.targets = targets;
                }
                else
                {
                    ViewBag.targets = new List<GradeDto>();
                }
            }
            ViewBag.id = id;

            return View();
        }

        /// <summary>
        /// 统计报表-运营团队胜率统计
        /// </summary>
        /// <returns></returns>
        public ActionResult MateRanking(string dateRange)
        {
            var targets = DoMySql.FindListBySql<GradeDto>($"SELECT yy_user_sn,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} group by yy_user_sn order by id desc");
            ViewBag.targets = targets;

            if (dateRange.IsNullOrEmpty())
            {
                var c_date = new TingZhanService().getNewTingzhan().c_date.ToDate().ToString("yyyy-MM-dd");
                dateRange = c_date + " ~ " + c_date;
            }

            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.dateRange = dateRange;

            return View();
        }

        /// <summary>
        /// 统计报表-单厅胜率统计
        /// </summary>
        /// <returns></returns>
        public ActionResult TingMateRanking(string dateRange)
        {
            var targets = DoMySql.FindListBySql<GradeDto>($"SELECT ting_sn,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and yy_user_sn = '{new UserIdentityBag().user_sn}' group by ting_sn order by id desc");
            ViewBag.targets = targets;

            if (dateRange.IsNullOrEmpty())
            {
                var c_date = new TingZhanService().getNewTingzhan().c_date.ToDate().ToString("yyyy-MM-dd");
                dateRange = c_date + " ~ " + c_date;
            }

            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.dateRange = dateRange;

            return View();
        }

        public class GradeDto : ModelDb.p_tingzhan_target
        {
            public object ting_sum { get; set; }
        }
    }
}