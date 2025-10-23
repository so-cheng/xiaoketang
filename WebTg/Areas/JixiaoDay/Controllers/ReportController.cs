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
    public class ReportController : BaseLoginController
    {
        /// <summary>
        /// 主播每日上报列表
        /// </summary>
        public ActionResult List(PageFactory.JixiaoDay.UserDayReportSession.DtoReq req)
        {
            if (req.c_date_range.IsNullOrEmpty())
            {
                req.c_date_range = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
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
            pageModel.formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
            {
                title = "所属主播",
                style = "background-color: transparent;",
                options = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn),
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
            pageModel.formDisplay.formItems.Where(x => x.name == "tips").FirstOrDefault().index = 98;
            pageModel.formDisplay.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().index=99;
            pageModel.adjuncts.Where(x => x.name == "floatlayer").FirstOrDefault().disabled = true;
            pageModel.adjuncts.Where(x => x.name == "floatlayer2").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }
    }
}