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
    /// 主播每日上报
    /// </summary>
    public class ReportController : BaseLoginController
    {
        /// <summary>
        /// 每日上报列表
        /// </summary>
        public ActionResult List(PageFactory.JixiaoDay.UserDayReportSession.DtoReq req)
        {
            req.c_date_range = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            var pageModel = new PageFactory.JixiaoDay.UserDayReportSession().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 每日上报表单提交
        /// </summary>
        public ActionResult Post(PageFactory.JixiaoDay.UserDayReportPost.DtoReq req)
        {
            req.zb_user_sn = new UserIdentityBag().user_sn;
            req.tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn);
            var pageModel = new PageFactory.JixiaoDay.UserDayReportPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            }
            pageModel.buttonGroup.buttonItems.Where(x => x.text == "提交记录").FirstOrDefault().disabled = false;
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
    }
}