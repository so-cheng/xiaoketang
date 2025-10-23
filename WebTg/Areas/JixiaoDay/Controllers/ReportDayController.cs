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
    public class ReportDayController : BaseLoginController
    {
        /// <summary>
        /// 主播每日上报列表
        /// </summary>
        public ActionResult List(PageFactory.JixiaoDay.UserDayReportSession.DtoReq req)
        {
            if (req.c_date_range.IsNullOrEmpty())
            {
                req.c_date_range = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀主播;
            var pageModel = new PageFactory.JixiaoDay.UserDayReportSession().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} or tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }



        /// <summary>
        /// 每日上报表单提交
        /// </summary>
        public ActionResult Post(PageFactory.JixiaoDay.UserDayReportPost.DtoReq req)
        {
            req.tg_user_sn = new UserIdentityBag().user_sn;
            
            var pageModel = new PageFactory.JixiaoDay.UserDayReportPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            }
            pageModel.formDisplay.formItems.Remove(pageModel.formDisplay.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault());
            pageModel.formDisplay.formItems.Remove(pageModel.formDisplay.formItems.Where(x => x.name == "zb").FirstOrDefault());

            var p_jixiao_day_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.id);

            pageModel.formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
            {
                title = "所属直播间",
                style = "background-color: transparent;",
                options = DoMySql.FindKvList<ModelDb.user_info_tg>($"tg_user_sn='{new UserIdentityBag().user_sn}'", "ting_name,ting_sn"),
                defaultValue = p_jixiao_day_session.ting_sn,
                eventJsChange = new EmtFormBase.EventJsChange
                {
                    eventCsAction = new ModelBase.EventJsBase.EventCsAction
                    {
                        attachPara = new Dictionary<string, object>
                        {
                            {"ting_sn","<%=page_post.ting_sn.value%>"}
                        },
                        func = GetZbAction,
                        resCallJs = $"{new ModelBasic.EmtSelect.Js("post.zb_user_sn").options(@"JSON.parse(res.data)")};"
                    }
                }
            });

            pageModel.formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
            {
                title = "所属主播",
                style = "background-color: transparent;",
                options = new ServiceFactory.UserInfo.Tg().TgGetNextZbForKv(new UserIdentityBag().user_sn),
                defaultValue = req.zb_user_sn,
                eventJsChange = new EmtFormBase.EventJsChange
                {
                    eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                    {
                        attachPara = new Dictionary<string, object>
                            {
                                { "c_date","<%=page_post.c_date.value%>"},
                                { "zb_user_sn","<%=page_post.zb_user_sn.value%>"}
                            },
                        func = new PageFactory.JixiaoDay.UserDayReportPost().GetZbSessionInfo,
                        resCallJs = $"page_post.amount.setPlaceholder('本日累计:'+res.data.amount);" +
                                            $"page_post.amount_1.setPlaceholder('本日累计:'+res.data.amount_1);" +
                                            $"page_post.num_2.setPlaceholder('本日累计:'+res.data.num_2);" +
                                            $"page_post.amount_2.setPlaceholder('本日累计:'+res.data.amount_2);" +
                                            //$"page_post.old_amount.setPlaceholder('本日累计:'+res.data.old_amount);" +
                                            $"page_post.hdpk_amount.setPlaceholder('本日累计:'+res.data.hdpk_amount);" +
                                            $"page_post.hx_num.setPlaceholder('本日累计:'+res.data.hx_num);" +
                                            $"page_post.hx_amount.setPlaceholder('本日累计:'+res.data.hx_amount);" +
                                            $"page_post.new_num.setPlaceholder('本日累计:'+res.data.new_num);" +
                                            $"page_post.contact_num.setPlaceholder('本日累计:'+res.data.contact_num);" +
                                            $"page_post.datou_num.setPlaceholder('本日累计:'+res.data.datou_num);"
                    }
                }
            });
            pageModel.formDisplay.formItems.Where(x => x.name == "show_tips").FirstOrDefault().index = 97;
            pageModel.formDisplay.formItems.Where(x => x.name == "ting_sn").FirstOrDefault().index = 98;
            pageModel.formDisplay.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().index=99;
            pageModel.adjuncts.Where(x => x.name == "floatlayer2").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        /// <summary>
        /// 每日上报快捷提报
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult FastPost(PageFactory.JixiaoDay.UserDayReportFastPost.DtoReq req)
        {
            req.zb_user_sn = new UserIdentityBag().user_sn;
            req.tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn);
            var pageModel = new PageFactory.JixiaoDay.UserDayReportFastPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            }
            pageModel.buttonGroup.buttonItems.Where(x => x.text == "提交记录").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
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
                var time = item.text.Substring(0, item.text.IndexOf(":"));
                if (DateTime.Now.ToString("HH").ToInt() < time.ToInt())
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

        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult DataList(string c_date, string tg_user_sn)
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
            ViewBag.username = new ServiceFactory.UserInfo.Ting().GetTingBySn(tg_user_sn).ting_name;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            if (!UtilityStatic.ClientHelper.IsMobileRequest())
            {
                return View();
            }
            else
            {
                return View("MobileDataList");
            }
            
        }


        /// <summary>
        /// 团队数据
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult TeamDatas(string dateRange)
        {
            //没有下级厅管的厅管不能查看
            if (new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn).Count == 0)
            {
                return Content("当前账号无法查看团队流水");
            }
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
                tg_user_sn = new UserIdentityBag().user_sn;
            }

            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
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
        public JsonResult SetVacation(string zb_user_sn, string date)
        {
            var info = new JsonResultAction();
            try
            {
                var p_jixiao_vacation = new ModelDb.p_jixiao_vacation
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    zb_user_sn = zb_user_sn,
                    tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, zb_user_sn),
                    c_date = date.ToDate(),
                    cause = "请假"
                };
                if (DoMySql.FindEntity<ModelDb.p_jixiao_vacation>($"zb_user_sn='{zb_user_sn}' and c_date='{date}'", false).IsNullOrEmpty())
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

        public JsonResultAction GetZbAction(JsonRequestAction req)
        {
            var result = new JsonResultAction();
            var ting_sn = req.GetPara("ting_sn");
            result.data = new ServiceFactory.UserInfo.Tg().TgGetNextZbForKv(new UserIdentityBag().user_sn).ToJson();
            return result;
        }
    }
}