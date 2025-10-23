using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Jiezou.Controllers
{
    public class ZhuboJiezouController : BaseLoginController
    {
        /// <summary>
        /// 主播节奏展示
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult Item_zhubo(string c_date, string ting_sn)
        {
            //默认日期前一天
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.ting_sn = ting_sn;

            //统计汇所属厅的达标率情况
            var tableList = new List<ServiceFactory.JiezouZhuboResult>();
            if (!ting_sn.IsNullOrEmpty())
            {
                var ting_arr = ting_sn.Split(',');
                ting_sn = "";
                foreach (var ting in ting_arr)
                {
                    ting_sn += "'" + ting + "',";
                }
                ting_sn = ting_sn.Substring(0, ting_sn.Length - 1);
                string sql = $"select t1.data_time,t2.name,t4.ting_name,t1.step,t1.zb_user_sn,t1.ting_sn FROM jiezou_zhubo_detail t1 left join user_base t2 on t1.zb_user_sn = t2.user_sn left join user_info_tg t4 on t4.ting_sn = t1.ting_sn where t1.ting_sn in ({ting_sn}) and t1.data_time = '{c_date}'";
                tableList = DoMySql.FindListBySql<ServiceFactory.JiezouZhuboResult>(sql);
            }

            if (tableList != null)
            {
                ViewBag.tableList = tableList;
                ViewBag.zts = tableList.Count;
                decimal dbsToday = 0;
                decimal wdbsToday = 0;
                foreach (var item in tableList)
                {
                    if (item.step >= 4) dbsToday++;
                    else wdbsToday++;
                }
                ViewBag.dbsToday = dbsToday;
                ViewBag.wdbsToday = wdbsToday;
                if (tableList.Count == 0)
                {
                    ViewBag.dbl = 0;
                    ViewBag.dblToday = 0;
                }
                else
                {
                    ViewBag.dblToday = Decimal.Round(dbsToday / (decimal)tableList.Count * 100, 2);
                }
            }

            return View();
        }
        /// <summary>
        /// 主播节奏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleDefinition_zhubo()
        {
            var ruleList = DoMySql.FindList<ModelDb.jiezou_zhubo_detail_rule>($"1=1");
            ViewBag.ruleList = ruleList;
            return View();
        }
        /// <summary>
        /// 主播节奏阶段展示
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult Detail(string c_date, string zb_user_sn)
        {
            //默认为近半个月
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-16).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
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

            //获取时间段内每天的节奏阶段
            var jiezouDetailList = DoMySql.FindListBySql<ModelDb.jiezou_zhubo_detail>($"select data_time,step FROM jiezou_zhubo_detail t1 where zb_user_sn = '{zb_user_sn}' and data_time between '{f_date}' and '{t_date}'");
            List<decimal> stepList = new List<decimal>();
            foreach (string date in dateArray)
            {
                decimal step;
                int zbNum;
                int dangwei;
                var jiezoudetail = jiezouDetailList.Find(n => n.data_time.ToDateString("yyyy-MM-dd").Equals(date));
                if (jiezoudetail != null)
                {
                    //添加阶段数据
                    step = (decimal)jiezoudetail.step;
                }
                else
                {
                    step = 0M;
                }
                stepList.Add(step);
            }

            ViewBag.stepList = stepList;
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.username = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(zb_user_sn).username;

            return View();
        }
    }
}