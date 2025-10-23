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
    public class InterpolateApplyController : BaseLoginController
    {
        #region 内推记录
        /// <summary>
        /// 内推记录
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyList()
        {
            var req = new PageFactory.JoinNewPush.InterpolateApplyList.DtoReq();
            var pageModel = new PageFactory.JoinNewPush.InterpolateApplyList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 内推申请
        /// <summary>
        /// 内推申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult InterpolateApply(int id = 0)
        {
            var req = new PageFactory.JoinNewPush.ZtNtPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNewPush.ZtNtPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}