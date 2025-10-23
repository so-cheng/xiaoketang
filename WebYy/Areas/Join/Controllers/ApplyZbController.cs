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
    public class ApplyZbController : BaseLoginController
    {
        #region 申请主播
        [HttpGet]
        public ActionResult ApplyZbPost()
        {
            var req = new PageFactory.Join.YyApplyZbPost.DtoReq();
            var pageModel = new PageFactory.Join.YyApplyZbPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}