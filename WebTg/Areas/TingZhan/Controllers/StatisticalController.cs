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

namespace WebProject.Areas.TingZhan.Controllers
{
   /// <summary>
   /// 统计报表
   /// </summary>
    public class StatisticalController : BaseLoginController
    {
        /// <summary>
        /// 胜率排行
        /// </summary>
        /// <returns></returns>
        public ActionResult MateRanking(string dateRange)
        {
            var targets = DoMySql.FindListBySql<GradeDto>($"SELECT ting_sn,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and yy_user_sn = '{new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).yy_sn}' group by ting_sn order by id desc");
            ViewBag.targets = targets;

            if (dateRange.IsNullOrEmpty())
            {
                var c_date = new ServiceFactory.TingZhanService().getNewTingzhan().c_date.ToDate().ToString("yyyy-MM-dd");
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