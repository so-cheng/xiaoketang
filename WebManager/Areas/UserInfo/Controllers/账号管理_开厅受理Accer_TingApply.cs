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

namespace WebProject.Areas.UserInfo.Controllers
{
    /// <summary>
    /// 账号管理_开厅流程
    /// </summary>
    public class Accer_TingApplyController : BaseLoginController
    {
        /// <summary>
        /// 开厅处理
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenHandle(PageFactory.UserInfo.TingApplyList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.TingApplyList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()}";
            return View(pageModel);
        }
    }
}









