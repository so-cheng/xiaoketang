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

namespace WebProject.Areas.PCrm.Controllers
{
    /// <summary>
    /// 主播每日上报
    /// </summary>
    public class ReportController : BaseLoginController
    {
        /// <summary>
        /// 每日上报列表
        /// </summary>
        public ActionResult List(PageFactory.UserDayReportSession.DtoReq req)
        {
            req.c_date_range = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            var pageModel = new PageFactory.UserDayReportSession().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 每日上报表单提交
        /// </summary>
        public ActionResult Post(PageFactory.UserDayReportPost.DtoReq req)
        {
            //跳转到新页面
            Response.Redirect("/JixiaoDay/Report/Post");

            req.zb_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.UserDayReportPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus =  EmtModelBase.DisplayStatus.只读;
            }
            pageModel.buttonGroup.buttonItems.Where(x => x.text == "提交记录").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
    }
}