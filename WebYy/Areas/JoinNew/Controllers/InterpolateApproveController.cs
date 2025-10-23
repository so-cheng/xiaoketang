using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class InterpolatApproveController : BaseLoginController
    {
        #region 内推申请列表
        public ActionResult ApplyList()
        {
            var req = new PageFactory.JoinNewPush.InterpolateApproveList.DtoReq();
            var pageModel = new PageFactory.JoinNewPush.InterpolateApproveList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }
        #endregion
    }
}