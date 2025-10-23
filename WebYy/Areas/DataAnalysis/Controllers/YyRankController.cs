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
    public class YyRankController : BaseLoginController
    {
        // GET: DataAnalysis/YyRank
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
        /// 运营数据-折线图
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="yy_user_sn"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult LineChart(string c_date, string yy_user_sn, string type)
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
            //获取时间段内每天的运营绩效数据
            var jixiaoDayList = DoMySql.FindList<ModelDb.p_jixiao_day>($"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and DATE(c_date) between '{f_date}' and '{t_date}' and yy_user_sn = '{yy_user_sn}' and (new_num!=0 or amount_1!=0 or num_2!=0 or amount_2!=0 or contact_num!=0)");
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
                //获取当天的运营数据
                var yyJixiaoList = jixiaoDayList.Where(n => n.c_date.ToDateString("yyyy-MM-dd").Equals(date)).ToList();
                if (yyJixiaoList.Count > 0)
                {
                    #region 拉新
                    // 取平均值需要去掉最高值和最低值的绩效记录
                    var yyJixiaoNewNumListOrder = yyJixiaoList.OrderBy(p => p.new_num).ToList();// 按拉新排序
                    var middleNewNumZbCount = GetMiddleZbCount(yyJixiaoNewNumListOrder); // 有效主播数（去掉最高拉新和最低拉新的绩效记录）

                    newNum = yyJixiaoList.Sum(x => x.new_num).ToInt(); // 总拉新
                    newNumAvg = middleNewNumZbCount > 0 ? Math.Round(GetMiddleYyJixiaoList(yyJixiaoNewNumListOrder).Sum(c => c.new_num).ToDecimal() / middleNewNumZbCount.ToDecimal(), 2) : 0; // 主播平均拉新个数 = 总拉新（去掉最高拉新和最低拉新的绩效记录） / 有效主播数（去掉最高拉新和最低拉新的绩效记录）
                    #endregion

                    #region 二消个数
                    // 取平均值需要去掉最高值和最低值的绩效记录
                    var yyJixiaoNum2ListOrder = yyJixiaoList.OrderBy(p => p.num_2).ToList();// 按二消个数排序
                    var middleNum2ZbCount = GetMiddleZbCount(yyJixiaoNum2ListOrder); // 有效主播数（去掉最高二消个数和最低二消个数的绩效记录）

                    num2 = yyJixiaoList.Sum(x => x.num_2).ToInt(); // 总二消个数
                    num2Avg = middleNum2ZbCount > 0 ? Math.Round(GetMiddleYyJixiaoList(yyJixiaoNum2ListOrder).Sum(c => c.num_2).ToDecimal() / middleNum2ZbCount.ToDecimal(), 2) : 0; // 主播平均二消个数 = 总二消个数（去掉最高二消个数和最低二消个数的绩效记录） / 有效主播数（去掉最高二消个数和最低二消个数的绩效记录）
                    #endregion

                    #region 二消总音浪
                    // 取平均值需要去掉最高值和最低值的绩效记录
                    var yyJixiaoAmount2ListOrder = yyJixiaoList.OrderBy(p => p.amount_2).ToList();// 按二消总音浪排序
                    var middleAmount2ZbCount = GetMiddleZbCount(yyJixiaoAmount2ListOrder); // 有效主播数（去掉最高二消总音浪和最低二消总音浪的绩效记录）
                    var middleNum2Sum = GetMiddleYyJixiaoList(yyJixiaoAmount2ListOrder).Sum(c => c.num_2); // 二消用户数（去掉最高二消总音浪和最低二消总音浪的绩效记录）
                    var middleAmount2Sum = GetMiddleYyJixiaoList(yyJixiaoAmount2ListOrder).Sum(c => c.amount_2); // 二消总音浪（去掉最高二消总音浪和最低二消总音浪的绩效记录）

                    amount2 = yyJixiaoList.Sum(x => x.amount_2).ToInt(); // 总二消音浪
                    zbAmount2Avg = middleAmount2ZbCount > 0 ? Math.Round(middleAmount2Sum.ToDecimal() / middleAmount2ZbCount.ToDecimal(), 2) : 0; // 主播平均二消音浪 = 二消总音浪（去掉最高二消总音浪和最低二消总音浪的绩效记录） / 有效主播数（去掉最高二消总音浪和最低二消总音浪的绩效记录）
                    yhAmount2Avg = middleNum2Sum > 0 ? Math.Round(middleAmount2Sum.ToDecimal() / middleNum2Sum.ToDecimal(), 2) : 0; // 用户平均二消音浪 = 二消总音浪（去掉最高二消总音浪和最低二消总音浪的绩效记录） / 二消用户数（去掉最高二消总音浪和最低二消总音浪的绩效记录）
                    #endregion

                    #region 建联
                    // 取平均值需要去掉最高值和最低值的绩效记录
                    var yyJixiaoContactNumListOrder = yyJixiaoList.OrderBy(p => p.contact_num).ToList();// 按建联排序
                    var middleContactNumZbCount = GetMiddleZbCount(yyJixiaoContactNumListOrder); // 有效主播数（去掉最高建联和最低建联的绩效记录）

                    contactNum = yyJixiaoList.Sum(x => x.contact_num).ToInt(); // 总建联
                    contactNumAvg = middleContactNumZbCount > 0 ? Math.Round(GetMiddleYyJixiaoList(yyJixiaoContactNumListOrder).Sum(c => c.contact_num).ToDecimal() / middleContactNumZbCount.ToDecimal(), 2) : 0; // 主播平均建联数 = 总建联（去掉最高建联和最低建联的绩效记录） / 有效主播数（去掉最高建联和最低建联的绩效记录）
                    #endregion
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

            ViewBag.username = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).name;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.type = type;

            return View();
        }

        public int GetMiddleZbCount(List<ModelDb.p_jixiao_day> yyJixiaoListOrder)
        {
            var middleYyJixiaoList = GetMiddleYyJixiaoList(yyJixiaoListOrder);// 去掉首尾
            var middleZbList = middleYyJixiaoList.GroupBy(c => c.zb_user_sn); // 按主播分组

            return middleZbList.Count(); // 有效主播数
        }

        public List<ModelDb.p_jixiao_day> GetMiddleYyJixiaoList(List<ModelDb.p_jixiao_day> yyJixiaoListOrder)
        {
            var middleYyJixiaoList = yyJixiaoListOrder.Skip(1).Take(yyJixiaoListOrder.Count - 2).ToList();// 去掉首尾
            return middleYyJixiaoList;
        }

        #region 运营绩效排名
        public ActionResult YyNum2RankTable(string dateRange)
        {
            dateRange = GetDateRange(dateRange);
            return View();
        }
        public ActionResult YyAmount2RankTable(string dateRange, string option)
        {
            dateRange = GetDateRange(dateRange);
            if (option.IsNullOrEmpty())
            {
                option = "1";
            }
            ViewBag.option = option;
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