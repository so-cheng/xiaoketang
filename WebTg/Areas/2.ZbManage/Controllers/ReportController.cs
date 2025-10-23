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
namespace WebProject.Areas._2.ZbManage.Controllers
{
    /// <summary>
    /// 每日上报
    /// </summary>
    public class ReportController : BaseLoginController
    {

        /// <summary>
        /// 每日上报列表
        /// </summary>
        public ActionResult List(PageFactory.UserDayReportSession.DtoReq req)
        {
            if (req.c_date_range.IsNullOrEmpty())
            {
                req.c_date_range = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀主播;
            var pageModel = new PageFactory.UserDayReportSession().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} or tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }

        /// <summary>
        /// 每日上报表单提交
        /// </summary>
        public ActionResult Post(PageFactory.UserDayReportPost.DtoReq req)
        {
            var pageModel = new PageFactory.UserDayReportPost().Get(req);
            if (req.id > 0)
            {
                pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            }
            pageModel.adjuncts.Where(x => x.name == "floatlayer").FirstOrDefault().disabled = true;
            pageModel.adjuncts.Where(x => x.name == "floatlayer2").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        #region 设置直播类型
        public ActionResult SetType(PageFactory.SetType.DtoReq req)
        {
            var pageModel = new PageFactory.SetType().Get(req);
            if (req.id <= 0 && req.is_show==1)
            {
                pageModel.buttonGroup.buttonItems.Where(x => x.name == "detail").FirstOrDefault().disabled = false;
            }
            return View(pageModel);
        }

        public ActionResult SetTypeList(PageFactory.SetTypeList.DtoReq req)
        {
            var pageModel = new PageFactory.SetTypeList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 设置主播请假
        public ActionResult VacationPost(PageFactory.VacationPost.DtoReq req)
        {
            var pageModel = new PageFactory.VacationPost().Get(req);
            return View(pageModel);
        }
        public ActionResult VacationList(PageFactory.VacationList.DtoReq req)
        {
            var pageModel = new PageFactory.VacationList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        #endregion
    }
}