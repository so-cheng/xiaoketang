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
using static WeiCode.ModelDbs.ModelDb;
using Newtonsoft.Json;
using WebProject.Areas.DataAnalysis.Data;

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
        public ActionResult Item(string c_date, string yy_user_sn)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;

            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = new ServiceFactory.UserInfo.Yy().GetAllYyForKv().First().Value;
            }
            List<Rule> rules = new List<Rule>();
            dataanalysis_coredata_ting_rule rule = DoMySql.FindList<dataanalysis_coredata_ting_rule>().FirstOrDefault();
            if (rule != null)
            {
                string[] c_rule = rule.c_rule.Split(';');
                foreach (var item in c_rule)
                {
                    string[] c_rulekey = item.Split(',');
                    Rule d = new Rule();
                    d.zd = c_rulekey[0];
                    d.fw = c_rulekey[1];
                    d.value = c_rulekey[2];
                    d.color = c_rulekey[3];
                    rules.Add(d);
                }
            }

            ViewBag.c_rule = JsonConvert.SerializeObject(rules);
            ViewBag.yy_user_sn = yy_user_sn;
            return View();
        }
        /// <summary>
        /// 曲线图
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="yy_user_sn"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult LineChart(string c_date, string tg_user_sn, string type)
        {

            //默认为近7天
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;

            //解析开始结束时间
            var date_s = c_date.Split('~');
            DateTime f_date = date_s[0].Trim().ToDate();
            DateTime t_date = date_s[1].Trim().ToDate();

            //补全时间段内日期
            List<string> dateArray = new List<string>();
            for (DateTime date = f_date; date <= t_date; date = date.AddDays(1))
            {
                dateArray.Add(date.ToString("yyyy-MM-dd"));
            }
            ViewBag.dateArray = dateArray;

            List<int?> zb_count = new List<int?>(); // 总人数
            List<int?> gear = new List<int?>(); // 档位数
            List<decimal?> liveHour = new List<decimal?>(); // 直播时长（时）
            List<int?> visitor = new List<int?>(); // 访客数
            List<int?> hourly_visitors = new List<int?>(); // 每小时访客数
            List<int?> interactive_user_count = new List<int?>(); // 互动人数
            List<int?> paying_user_total = new List<int?>(); // 付费用户总数
            List<int?> new_paying_user_count = new List<int?>(); // 新付费用户数
            List<int?> avg_second_consumption = new List<int?>(); // 平均二消值
            List<int?> user_count_type_a = new List<int?>(); // A 类用户数
            List<int?> user_count_type_b = new List<int?>(); // B 类用户数
            List<int?> user_count_type_c = new List<int?>(); // C 类用户数
            List<int?> user_count_type_a_last_5d = new List<int?>(); // 近 5 天 A 类用户数
            List<int?> user_count_type_a_last_10d = new List<int?>(); // 近 10 天 A 类用户数
            List<int?> user_count_type_a_last_15 = new List<int?>(); // 近 15 天 A 类用户数
            List<int?> old_user_count_last_3d = new List<int?>(); // 3 天内老用户数
            List<int?> old_user_count_last_5d = new List<int?>(); // 5 天内老用户数
            List<int?> old_user_count_last_7d = new List<int?>(); // 7 天内老用户数
            List<int?> old_user_count_last_15d = new List<int?>(); // 15 天内老用户数
            List<int?> totalFanTickets = new List<int?>(); // 总音浪


            //获取时间段内每天的数据
            var where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and DATE(c_date) between '{f_date}' and '{t_date}' ";
            if (!tg_user_sn.IsNullOrEmpty())
            {
                where += $" and tg_user_sn = '{tg_user_sn}'";
            }
            var dataanalysis_coredata_tings = DoMySql.FindList<ModelDb.dataanalysis_coredata_ting>(where);

            //赋值每天的数据
            foreach (var date in dateArray)
            {
                //当天数据
                var todaydata = dataanalysis_coredata_tings.Where(n => n.c_date.ToDateString("yyyy-MM-dd").Equals(date));
                if (todaydata.Count() > 0)
                {
                    // 总人数
                    zb_count.Add(todaydata.Sum(a => a.zb_count));
                    // 档位数
                    gear.Add(todaydata.Sum(a => a.gear));
                    // 直播时长（时）
                    liveHour.Add(todaydata.Sum(a => a.liveHour));
                    //访客数
                    visitor.Add(todaydata.Sum(a => a.visitor));
                    // 每小时访客数
                    hourly_visitors.Add(todaydata.Sum(a => a.hourly_visitors));
                    //互动人数
                    interactive_user_count.Add(todaydata.Sum(a => a.interactive_user_count));
                    //付费用户总数
                    paying_user_total.Add(todaydata.Sum(a => a.paying_user_total));
                    //新付费用户数
                    new_paying_user_count.Add(todaydata.Sum(a => a.new_paying_user_count));
                    //平均二消值
                    avg_second_consumption.Add(todaydata.Sum(a => a.avg_second_consumption));
                    //A 类用户数
                    user_count_type_a.Add(todaydata.Sum(a => a.user_count_type_a));
                    //B 类用户数
                    user_count_type_b.Add(todaydata.Sum(a => a.user_count_type_b));
                    //C 类用户数
                    user_count_type_c.Add(todaydata.Sum(a => a.user_count_type_c));
                    // 3 天内老用户数
                    old_user_count_last_3d.Add(todaydata.Sum(a => a.old_user_count_last_3d));
                    // 5 天内老用户数
                    old_user_count_last_5d.Add(todaydata.Sum(a => a.old_user_count_last_5d));
                    // 7 天内老用户数
                    old_user_count_last_7d.Add(todaydata.Sum(a => a.old_user_count_last_7d));
                    // 15 天内老用户数
                    old_user_count_last_15d.Add(todaydata.Sum(a => a.old_user_count_last_15d));
                    // 总音浪
                    totalFanTickets.Add(todaydata.Sum(a => a.totalFanTickets));

                }
            }

            ViewBag.zb_count = zb_count;
            ViewBag.gear = gear;
            ViewBag.liveHour = liveHour;
            ViewBag.visitor = visitor;
            ViewBag.hourly_visitors = hourly_visitors;
            ViewBag.interactive_user_count = interactive_user_count;
            ViewBag.paying_user_total = paying_user_total;
            ViewBag.new_paying_user_count = new_paying_user_count;
            ViewBag.avg_second_consumption = avg_second_consumption;
            ViewBag.user_count_type_a = user_count_type_a;
            ViewBag.user_count_type_b = user_count_type_b;
            ViewBag.user_count_type_c = user_count_type_c;
            ViewBag.old_user_count_last_3d = old_user_count_last_3d;
            ViewBag.old_user_count_last_5d = old_user_count_last_5d;
            ViewBag.old_user_count_last_7d = old_user_count_last_7d;
            ViewBag.old_user_count_last_15d = old_user_count_last_15d;
            ViewBag.totalFanTickets = totalFanTickets;

            ViewBag.username = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(tg_user_sn).name;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.type = type;
            return View();
        }
    }
}