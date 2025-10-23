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
        /// <returns></returns>
        public ActionResult UnAllocateList(string date_range_s, string date_range_e)
        {
            var req = new PageFactory.JoinNew.Stu_List.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_List().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"id not in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and term in (select term from p_mengxin where date >= '{date_range_s}' and date <= '{date_range_e.ToDate().ToString("yyyy-MM-dd")}') and mx_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        /// <summary>
        /// 实时分配情况
        /// </summary>
        /// <returns></returns>
        public ActionResult AllocateTotal(string month)
        {
            if (month.IsNullOrEmpty())
            {
                month = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.month = month;
            return View();
        }

        #region 考核学员列表
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

        #region 已分配学员列表
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

        #region 未分配学员列表
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
    }
}