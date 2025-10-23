using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ApplyZbController : BaseLoginController
    {
        #region 申请主播
        [HttpGet]
        public ActionResult ApplyZbPost(int id = 0)
        {
            var req = new PageFactory.JoinNew.App_YyApplyZbPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.App_YyApplyZbPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 申请日志
        /// <summary>
        /// 申请日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Log(string apply_sn = "")
        {
            var req = new PageFactory.JoinNew.ApplyLog.DtoReq();
            req.apply_sn = apply_sn;
            var pageModel = new PageFactory.JoinNew.ApplyLog().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}