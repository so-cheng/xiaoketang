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

namespace WebProject.Areas.DataAnalysis.Controllers
{
    public class TgRankController : BaseLoginController
    {
        // GET: DataAnalysis/TgRank
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
        /// 单厅数据-折线图
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult LineChart(string c_date, string ting_sn, string type)
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

            List<int> newNumList = new List<int>(); // 总拉新
            List<decimal> newNumAvgList = new List<decimal>(); // 主播平均拉新个数

            List<int> num2List = new List<int>(); // 总二消个数
            List<decimal> num2AvgList = new List<decimal>(); // 主播平均二消个数

            List<int> amount2List = new List<int>(); // 总二消音浪
            List<decimal> zbAmount2AvgList = new List<decimal>(); // 主播平均二消音浪
            List<decimal> yhAmount2AvgList = new List<decimal>(); // 用户平均二消音浪

            List<int> contactNumList = new List<int>(); // 总建联
            List<decimal> contactNumAvgList = new List<decimal>(); // 主播平均建联数
            //获取时间段内每天的单厅绩效数据
            var jixiaoDayList = DoMySql.FindList<ModelDb.p_jixiao_day>($"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and DATE(c_date) between '{f_date}' and '{t_date}' and ting_sn = '{ting_sn}' and (new_num!=0 or amount_1!=0 or num_2!=0 or amount_2!=0 or contact_num!=0)");
            foreach (string date in dateArray)
            {
                int newNum = 0;
                decimal newNumAvg = 0;

                int num2 = 0;
                decimal num2Avg = 0;

                int amount2 = 0;
                decimal zbAmount2Avg = 0;
                decimal yhAmount2Avg = 0;

                int contactNum = 0;
                decimal contactNumAvg = 0;
                //获取当天的单厅数据
                var tingJixiaoList = jixiaoDayList.Where(n => n.c_date.ToDateString("yyyy-MM-dd").Equals(date)).ToList();
                if (tingJixiaoList.Count > 0)
                {
                    var zbList = tingJixiaoList.GroupBy(c => c.zb_user_sn); // 按主播分组
                    var zbCount = zbList.Count(); // 有效主播数

                    newNum = tingJixiaoList.Sum(x => x.new_num).ToInt(); // 总拉新
                    newNumAvg = zbCount > 0 ? Math.Round(newNum.ToDecimal() / zbCount.ToDecimal(), 2) : 0; // 主播平均拉新个数 = 总拉新 / 有效主播数

                    num2 = tingJixiaoList.Sum(x => x.num_2).ToInt(); // 总二消个数
                    num2Avg = zbCount > 0 ? Math.Round(num2.ToDecimal() / zbCount.ToDecimal(), 2) : 0; // 主播平均二消个数 = 总二消个数 / 有效主播数

                    amount2 = tingJixiaoList.Sum(x => x.amount_2).ToInt(); // 总二消音浪
                    zbAmount2Avg = zbCount > 0 ? Math.Round(amount2.ToDecimal() / zbCount.ToDecimal(), 2) : 0; // 主播平均二消音浪 = 二消总音浪 / 有效主播数
                    yhAmount2Avg = num2 > 0 ? Math.Round(amount2.ToDecimal() / num2.ToDecimal(), 2) : 0; // 用户平均二消音浪 = 二消总音浪 / 二消用户数

                    contactNum = tingJixiaoList.Sum(x => x.contact_num).ToInt(); // 总建联
                    contactNumAvg = zbCount > 0 ? Math.Round(contactNum.ToDecimal() / zbCount.ToDecimal(), 2) : 0; // 主播平均建联数 = 二消总音浪 / 有效主播数
                }

                newNumList.Add(newNum);
                newNumAvgList.Add(newNumAvg);

                num2List.Add(num2);
                num2AvgList.Add(num2Avg);

                amount2List.Add(amount2);
                zbAmount2AvgList.Add(zbAmount2Avg);
                yhAmount2AvgList.Add(yhAmount2Avg);

                contactNumList.Add(contactNum);
                contactNumAvgList.Add(contactNumAvg);
            }

            ViewBag.newNumList = newNumList;
            ViewBag.newNumAvgList = newNumAvgList;

            ViewBag.num2List = num2List;
            ViewBag.num2AvgList = num2AvgList;

            ViewBag.amount2List = amount2List;
            ViewBag.zbAmount2AvgList = zbAmount2AvgList;
            ViewBag.yhAmount2AvgList = yhAmount2AvgList;

            ViewBag.contactNumList = contactNumList;
            ViewBag.contactNumAvgList = contactNumAvgList;

            ViewBag.username = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
            ViewBag.ting_sn = ting_sn;
            ViewBag.type = type;

            return View();
        }

        #region 厅管绩效排名

        public ActionResult TgNum2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult TgAmount2RankTable(string dateRange, string option)
        {
            dateRange = GetDateRange(dateRange);
            if (option.IsNullOrEmpty())
            {
                option = "1";
            }
            ViewBag.option = option;
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
    }
}