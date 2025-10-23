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
namespace WebProject.Areas._3.TgsManage.Controllers
{
    /// <summary>
    /// 每日上报
    /// </summary>
    public class ReportController : BaseLoginController
    {
        /// <summary>
        /// 绩效列表
        /// </summary>
        public ActionResult List(PageFactory.UserDayReportSession.DtoReq req)
        {
            if (req.c_date_range.IsNullOrEmpty())
            {
                req.c_date_range = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            req.relation_type =ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.UserDayReportSession().Get(req);

            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
    }
}