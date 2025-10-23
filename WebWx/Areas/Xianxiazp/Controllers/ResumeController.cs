using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 简历
    /// </summary>
    public class ResumeController : BaseController
    {
        /// <summary>
        /// 分享二维码、简历登记
        /// </summary>
        /// <returns></returns>
        public ActionResult Page()
        {
            return View();
        }

        /// <summary>
        /// 简历登记
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(string wx_user_sn)
        {
            var req = new PageFactory.Xianxiazp.ResumePost.DtoReq();
            req.wx_user_sn = wx_user_sn;
            var pageModel = new PageFactory.Xianxiazp.ResumePost().Get(req);
            return View(pageModel);
        }
    }
}