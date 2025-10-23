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
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas._2.TgManage.Controllers
{
    /// <summary>
    /// 每日上报
    /// </summary>
    public class ReportController : BaseLoginController
    {
        /// <summary>
        /// 上报列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserDayReportSession.DtoReq req)
        {
            var pageModel = new PageFactory.UserDayReportSession().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";
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
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        public ActionResult SetType(PageFactory.YYSetType.DtoReq req)
        {
            var pageModel = new PageFactory.YYSetType().Get(req);
            if (req.id <= 0 && req.is_show == 1)
            {
                pageModel.buttonGroup.buttonItems.Where(x => x.name == "detail").FirstOrDefault().disabled = false;
            }
            return View(pageModel);
        }

        public ActionResult SetTypeList(PageFactory.YYSetTypeList.DtoReq req)
        {
            var pageModel = new PageFactory.YYSetTypeList().Get(req);
            return View(pageModel);
        }

        #region 主播请假
        public ActionResult VacationList(PageFactory.VacationList.DtoReq req)
        {
            var pageModel = new PageFactory.VacationList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        #endregion
    }
}