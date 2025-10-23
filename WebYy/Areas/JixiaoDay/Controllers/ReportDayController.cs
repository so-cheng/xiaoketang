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

namespace WebProject.Areas.JixiaoDay.Controllers
{
    /// <summary>
    /// 绩效
    /// </summary>
    public class ReportDayController : BaseLoginController
    {
        public ActionResult Index()
        {
            ViewBag.page = new ModelDbSite.site_page
            {
                title = "首页",
                name = "首页"
            };

            var jixao_today = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)} and c_date = '{DateTime.Today}'");

            ViewBag.today_amount = (jixao_today[0].IsNullOrEmpty() ? 0 : jixao_today[0].ToInt());
            ViewBag.today_new_num = (jixao_today[1].IsNullOrEmpty() ? "0" : jixao_today[1]);
            ViewBag.today_contact_num = (jixao_today[2].IsNullOrEmpty() ? "0" : jixao_today[2]);
            ViewBag.today_datou_num = (jixao_today[3].IsNullOrEmpty() ? "0" : jixao_today[3]);

            var jixao_yesterday = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)} and c_date = '{DateTime.Today.AddDays(-1)}'");

            ViewBag.yesterday_amount = (jixao_yesterday[0].IsNullOrEmpty() ? 0 : jixao_yesterday[0].ToInt());
            ViewBag.yesterday_new_num = (jixao_yesterday[1].IsNullOrEmpty() ? "0" : jixao_yesterday[1]);
            ViewBag.yesterday_contact_num = (jixao_yesterday[2].IsNullOrEmpty() ? "0" : jixao_yesterday[2]);
            ViewBag.yesterday_datou_num = (jixao_yesterday[3].IsNullOrEmpty() ? "0" : jixao_yesterday[3]);

            var jixao_month = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)} and c_date >= '{DateTime.Today.ToString("yyyy-MM-01")}' and  c_date < '{DateTime.Today.AddMonths(1).ToString("yyyy-MM-01")}'");

            ViewBag.month_amount = (jixao_month[0].IsNullOrEmpty() ? 0 : jixao_month[0].ToInt());
            ViewBag.month_new_num = (jixao_month[1].IsNullOrEmpty() ? "0" : jixao_month[1]);
            ViewBag.month_contact_num = (jixao_month[2].IsNullOrEmpty() ? "0" : jixao_month[2]);
            ViewBag.month_datou_num = (jixao_month[3].IsNullOrEmpty() ? "0" : jixao_month[3]);

            var A_level = DoMySql.FindField<ModelDb.crm_grade_log>("count(id)", $"n_grade_id= '{3}' and create_time>'{DateTime.Today}' and create_time<'{DateTime.Today.AddDays(1)}'");
            ViewBag.A_level = (A_level[0].IsNullOrEmpty() ? "0" : A_level[0]);

            var else_level = DoMySql.FindField<ModelDb.crm_grade_log>("count(id)", $"n_grade_id!= '{3}' and create_time>'{DateTime.Today}' and create_time<'{DateTime.Today.AddDays(1)}'");
            ViewBag.else_level = (else_level[0].IsNullOrEmpty() ? "0" : else_level[0]);

            return View();
        }

        public ActionResult List(string c_date, string tg_user_sn,string t_yy_user_sn)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }
            if (t_yy_user_sn.IsNullOrEmpty())
            {
                t_yy_user_sn = "";
            }

            ViewBag.c_date = c_date;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.username = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.t_yy_user_sn = t_yy_user_sn;

            if (UtilityStatic.ClientHelper.IsMobileRequest())
            {
                return View("MobileDataList"); 
            }
            else
            {
                return View();
            }
        }

        public ActionResult Datas(string dateRange, string tg_user_sn)
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
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = DoMySql.FindListBySql<ModelDb.user_relation>(new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn));
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].t_user_sn;
                }
            }

            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.Monday = Monday;
            ViewBag.dateRange = dateRange;
            ViewBag.username = new ServiceFactory.UserInfo.Ting().GetTingBySn(tg_user_sn).ting_name;
            return View();
        }

        /// <summary>
        /// 团队数据
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult TeamDatas(string dateRange)
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

            ViewBag.dateRange = dateRange;
            return View();
        }
        /// <summary>
        /// 快捷设置
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult FastOpinion()
        {
            return View();
        }

        /// <summary>
        /// 上报列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult DataList(PageFactory.JixiaoDay.UserDayReportSession.DtoReq req)
        {
            var pageModel = new PageFactory.JixiaoDay.UserDayReportSession().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }


        /// <summary>
        /// 时间段数据表格
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult DataOnDateRangeList(string dateRange, string zb_user_sn)
        {
            if (dateRange.IsNullOrEmpty())
            {
                dateRange = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (zb_user_sn.IsNullOrEmpty())
            {
                return Content("请选择主播");
            }
            var Range = UtilityStatic.CommonHelper.DateRangeFormat(dateRange);
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.c_date_s = Range.date_range_s;
            ViewBag.c_date_e = Range.date_range_e;
            return View();
        }

        /// <summary>
        /// 每日上报表单提交
        /// </summary>
        public ActionResult Post(PageFactory.JixiaoDay.UserDayReportPost.DtoReq req)
        {
            
            var pageModel = new PageFactory.JixiaoDay.UserDayReportPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            }
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }
    }
}