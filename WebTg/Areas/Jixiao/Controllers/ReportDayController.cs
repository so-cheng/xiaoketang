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

namespace WebProject.Areas.Jixiao.Controllers
{
    public class ReportDayController : BaseLoginController
    {
        // GET: Jixiao/ReportDay
        public ActionResult Index()
        {
            ViewBag.page = new ModelDbSite.site_page
            {
                title = "首页",
                name = "首页"
            };

            int session = 0;
            foreach (var item in new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.场次))
            {
                var time = item.text.Substring(0,item.text.IndexOf(":"));
                if (DateTime.Now.ToString("HH").ToInt()<time.ToInt())
                {
                    session = item.value.ToInt() - 1;
                    break;
                }
            }
            var jixiao_today = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(new_num),sum(num_2),sum(contact_num),sum(amount_1),sum(amount_2),sum(amount),sum(old_amount)", $"tg_user_sn = '{new UserIdentityBag().user_sn}' and c_date = '{DateTime.Today}' and session <'{session}'");
            ViewBag.today_new_num = (jixiao_today[0].IsNullOrEmpty() ? "0" : jixiao_today[0]);
            ViewBag.today_num_2 = (jixiao_today[1].IsNullOrEmpty() ? "0" : jixiao_today[1]);
            ViewBag.today_contact_num = (jixiao_today[2].IsNullOrEmpty() ? "0" : jixiao_today[2]);
            ViewBag.today_amount_1 = (jixiao_today[3].IsNullOrEmpty() ? "0" : jixiao_today[3]);
            ViewBag.today_amount_2 = (jixiao_today[4].IsNullOrEmpty() ? "0" : jixiao_today[4]);
            ViewBag.today_amount = (jixiao_today[5].IsNullOrEmpty() ? "0" : jixiao_today[5]);
            ViewBag.today_old_amount = (jixiao_today[6].IsNullOrEmpty() ? "0" : jixiao_today[6]);

            var jixiao_yesterday = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(new_num),sum(num_2),sum(contact_num),sum(amount_1),sum(amount_2),sum(amount),sum(old_amount)", $"tg_user_sn = '{new UserIdentityBag().user_sn}' and c_date = '{DateTime.Today.AddDays(-1)}' and session <'{session}'");

            ViewBag.yesterday_new_num = (jixiao_yesterday[0].IsNullOrEmpty() ? "0" : jixiao_yesterday[0]);
            ViewBag.yesterday_num_2 = (jixiao_yesterday[1].IsNullOrEmpty() ? "0" : jixiao_yesterday[1]);
            ViewBag.yesterday_contact_num = (jixiao_yesterday[2].IsNullOrEmpty() ? "0" : jixiao_yesterday[2]);
            ViewBag.yesterday_amount_1 = (jixiao_yesterday[3].IsNullOrEmpty() ? "0" : jixiao_yesterday[3]);
            ViewBag.yesterday_amount_2 = (jixiao_yesterday[4].IsNullOrEmpty() ? "0" : jixiao_yesterday[4]);
            ViewBag.yesterday_amount = (jixiao_yesterday[5].IsNullOrEmpty() ? "0" : jixiao_yesterday[5]);
            ViewBag.yesterday_old_amount = (jixiao_yesterday[6].IsNullOrEmpty() ? "0" : jixiao_yesterday[6]);

            var jixiao_week = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(new_num),sum(num_2),sum(contact_num),sum(amount_1),sum(amount_2),sum(amount),sum(old_amount)", $"tg_user_sn = '{new UserIdentityBag().user_sn}' and c_date = '{DateTime.Today.AddDays(-7)}' and session <'{session}'");

            ViewBag.week_new_num = (jixiao_week[0].IsNullOrEmpty() ? "0" : jixiao_week[0]);
            ViewBag.week_num_2 = (jixiao_week[1].IsNullOrEmpty() ? "0" : jixiao_week[1]);
            ViewBag.week_contact_num = (jixiao_week[2].IsNullOrEmpty() ? "0" : jixiao_week[2]);
            ViewBag.week_amount_1 = (jixiao_week[3].IsNullOrEmpty() ? "0" : jixiao_week[3]);
            ViewBag.week_amount_2 = (jixiao_week[4].IsNullOrEmpty() ? "0" : jixiao_week[4]);
            ViewBag.week_amount = (jixiao_week[5].IsNullOrEmpty() ? "0" : jixiao_week[5]);
            ViewBag.week_old_amount = (jixiao_week[6].IsNullOrEmpty() ? "0" : jixiao_week[6]);
            return View();
        }

        /// <summary>
        /// 未提交绩效人员名单
        /// </summary>
        /// <returns></returns>
        public ActionResult UnReport(string zhubo)
        {
            ViewBag.zhubo = zhubo;
            return View();
        }

        public ActionResult List(string c_date, string tg_user_sn)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }

            ViewBag.c_date = c_date;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.username = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'").username;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            return View();
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
                dateRange = Monday.ToString("yyyy-MM-dd")+" ~ "+ DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            
            ViewBag.c_date_s = dateRange.Substring(0,dateRange.IndexOf("~")-1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2); 
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.Monday = Monday;
            ViewBag.username = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'").username;
            return View();
        }

        


        public ActionResult LargeScreen()
        {
            return View();
        }

        /// <summary>
        /// 快捷设置主播请假
        /// </summary>
        /// <returns></returns>
        public JsonResult SetVacation(string zb_user_sn,string date)
        {
            var info = new JsonResultAction();
            try
            {
                var p_jixiao_vacation = new ModelDb.p_jixiao_vacation
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    zb_user_sn=zb_user_sn,
                    tg_user_sn=new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播,zb_user_sn),
                    c_date=date.ToDate(),
                    cause="请假"
                };
                if (DoMySql.FindEntity<ModelDb.p_jixiao_vacation>($"zb_user_sn='{zb_user_sn}' and c_date='{date}'",false).IsNullOrEmpty())
                {
                    p_jixiao_vacation.Insert();
                }
            }
            catch (Exception e)
            {
                info.code = 1;
                info.msg = e.Message;
            }

            return Json(info);
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
    }
}