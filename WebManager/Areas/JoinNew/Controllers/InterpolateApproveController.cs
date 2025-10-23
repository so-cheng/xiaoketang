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
            return View(pageModel);
        }
        #endregion

        #region 内推退回操作
        public ActionResult RollbackApply(int id)
        {
            var req = new PageFactory.JoinNewPush.RollBackPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNewPush.RollBackPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}