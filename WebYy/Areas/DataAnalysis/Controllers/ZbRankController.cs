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
    public class ZbRankController : BaseLoginController
    {
        // GET: DataAnalysis/ZbRank
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
        /// 主播数据-折线图
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="zb_user_sn"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult LineChart(string c_date, string zb_user_sn, string type)
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
            List<int> num2List = new List<int>(); // 总二消个数
            List<int> amount2List = new List<int>(); // 总二消音浪
            List<int> contactNumList = new List<int>(); // 总建联
            //获取时间段内每天的主播绩效数据
            var jixiaoDayList = DoMySql.FindList<ModelDb.p_jixiao_day>($"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and DATE(c_date) between '{f_date}' and '{t_date}' and zb_user_sn = '{zb_user_sn}' and (new_num!=0 or amount_1!=0 or num_2!=0 or amount_2!=0 or contact_num!=0)");
            foreach (string date in dateArray)
            {
                int newNum = 0;
                int num2 = 0;
                int amount2 = 0;
                int contactNum = 0;
                //获取当天的主播数据
                var zbJixiao = jixiaoDayList.Find(n => n.c_date.ToDateString("yyyy-MM-dd").Equals(date));
                if (zbJixiao != null)
                {
                    newNum = zbJixiao.new_num.ToInt(); // 总拉新
                    num2 = zbJixiao.num_2.ToInt(); // 总二消个数
                    amount2 = zbJixiao.amount_2.ToInt(); // 总二消音浪
                    contactNum = zbJixiao.contact_num.ToInt(); // 总建联
                }

                newNumList.Add(newNum);
                num2List.Add(num2);
                amount2List.Add(amount2);
                contactNumList.Add(contactNum);
            }

            ViewBag.newNumList = newNumList;
            ViewBag.num2List = num2List;
            ViewBag.amount2List = amount2List;
            ViewBag.contactNumList = contactNumList;
            ViewBag.username = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(zb_user_sn).name;
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.type = type;

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
    }
}