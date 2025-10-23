using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Project;
using WeiCode.Services;

namespace WebProject.Areas.Join.Controllers
{
    /// <summary>
    /// 厅管申请转厅功能
    /// </summary>
    public class ChangeApplyController : BaseLoginController
    {
        #region 主播名单
        public ActionResult zbList(PageFactory.JoinChange.UserList.DtoReq req)
        {
            var pageModel = new PageFactory.JoinChange.UserList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 转厅申请
        public ActionResult changeApply(PageFactory.JoinChange.ChangePost.DtoReq req)
        {
            var pageModel = new PageFactory.JoinChange.ChangePost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}