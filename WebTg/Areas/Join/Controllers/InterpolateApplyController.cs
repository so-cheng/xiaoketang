using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    public class InterpolateApplyController : BaseLoginController
    {
        #region 主播名单
        public ActionResult applyList(PageFactory.JoinPush.InterpolateApplyList.DtoReq req)
        {
            var pageModel = new PageFactory.JoinPush.InterpolateApplyList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 内推申请
        public ActionResult interpolateApply(int id = 0)
        {
            var req = new PageFactory.JoinPush.NtPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinPush.NtPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}