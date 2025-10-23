using Newtonsoft.Json.Linq;
using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    public class StatisticalController : BaseController
    {
        /// <summary>
        /// 数据分析
        /// </summary>
        /// <returns></returns>
        public ActionResult DataAnalysis()
        {
            return View();
        }

        /// <summary>
        /// 补人概况
        /// </summary>
        /// <param name="c_date"></param>
        /// <returns></returns>
        public ActionResult DataView(string c_date)
        {
            ViewBag.c_date = c_date;
            return View();
        }

        /// <summary>
        /// 折线图-公会补人率
        /// </summary>
        /// <param name="c_date"></param>
        /// <returns></returns>
        public ActionResult LineChart(string c_date)
        {
            //默认为近7天
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd");
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

            List<int> applyList = new List<int>(); // 提交人数
            List<int> joinList = new List<int>(); // 已补人数
            List<decimal> joinRateList = new List<decimal>(); // 补人率
            //获取时间段内每天的补人率
            foreach (string date in dateArray)
            {
                //获取当天的补人率
                var zb_count_sum = DoMySql.FindField<ModelDb.p_join_apply>("sum(zb_count)", $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{date.ToDate().ToString("yyyy-MM-dd")}' and create_time <= '{date.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'")[0].ToInt();
                var real_zb_count_sum = DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{date.ToDate().ToString("yyyy-MM-dd")}' and create_time <= '{date.ToDate().AddDays(1).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0].ToInt();
                var rate = zb_count_sum > 0 ? Math.Round((real_zb_count_sum.ToDecimal() / zb_count_sum.ToDecimal() * 100), 2).ToDecimal() : 0;

                applyList.Add(zb_count_sum);
                joinList.Add(real_zb_count_sum);
                joinRateList.Add(rate);
            }

            ViewBag.applyList = applyList;
            ViewBag.joinList = joinList;
            ViewBag.joinRateList = joinRateList;

            return View();
        }

        /// <summary>
        /// 萌新排名
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult MxRank(string month = "")
        {
            if (string.IsNullOrEmpty(month))
            {
                month = DateTime.Now.ToString("yyyy-MM");
            }
            ViewBag.month = month;
            return View();
        }

        /// <summary>
        /// 未分配明细
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public ActionResult UnAllocate(string dateRange)
        {
            if (string.IsNullOrEmpty(dateRange))
            {
                dateRange += DateTime.Today.ToString("yyyy-MM-01") + " ~";
                dateRange += " " + DateTime.Today.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.dateRange = dateRange;
            return View();
        }

        #region 未分配明细学员列表
        /// <summary>
        /// 未分配明细学员列表
        /// </summary>
        /// <param name="date_range_s"></param>
        /// <param name="date_range_e"></param>
        /// <param name="mx_sn"></param>
        /// <returns></returns>
        public ActionResult UnAllocateList(string date_range_s, string date_range_e, string mx_sn = "")
        {
            var req = new PageFactory.JoinNew.Stu_List.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_List().Get(req);
            string where = $"id not in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and term in (select term from p_mengxin where date >= '{date_range_s}' and date <= '{date_range_e.ToDate().ToString("yyyy-MM-dd")}')";
            if (!mx_sn.IsNullOrEmpty()) where += $" and mx_sn = '{mx_sn}'";
            pageModel.listDisplay.listData.attachFilterSql = where;
            return View(pageModel);
        }
        #endregion

        /// <summary>
        /// 实时分配情况
        /// </summary>
        /// <returns></returns>
        public ActionResult AllocateTotal(string month, string mx_sn)
        {
            if (month.IsNullOrEmpty())
            {
                month = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.month = month;
            ViewBag.mx_sn = mx_sn;
            return View();
        }

        #region 期数考核学员列表
        /// <summary>
        /// 考核学员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReviewStuList(string term)
        {
            var req = new PageFactory.JoinNew.Stu_List.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_List().Get(req);
            pageModel.listFilter.formItems.Find(x => x.name == "term").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"term = '{term}'";
            return View(pageModel);
        }
        #endregion

        #region 期数已分配学员列表
        /// <summary>
        /// 已分配学员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AllocatedStuList(string term)
        {
            var req = new PageFactory.JoinNew.Stu_List.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_List().Get(req);
            pageModel.listFilter.formItems.Find(x => x.name == "term").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"term = '{term}' and exists (select 1 from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = p_join_new_info.id)";
            return View(pageModel);
        }
        #endregion

        #region 期数未分配学员列表
        /// <summary>
        /// 未分配学员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAllocateStuList(string term)
        {
            var req = new PageFactory.JoinNew.Stu_List.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_List().Get(req);
            pageModel.listFilter.formItems.Find(x => x.name == "term").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"term = '{term}' and not exists (select 1 from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = p_join_new_info.id)";
            return View(pageModel);
        }
        #endregion

        /// <summary>
        /// 补人数据（厅每日入库数量统计）
        /// </summary>
        /// <param name="c_date"></param>
        /// <returns></returns>
        public ActionResult TingJoinData(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.c_date = c_date;
            return View();
        }
    }
}