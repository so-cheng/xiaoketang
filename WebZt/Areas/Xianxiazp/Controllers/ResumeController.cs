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
    public class ResumeController : BaseLoginController
    {
        /// <summary>
        /// 可选字段配置
        /// </summary>
        /// <returns></returns>
        public ActionResult FiledConfig()
        {
            var req = new PageFactory.Xianxiazp.FiledConfig.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.FiledConfig().Get(req);
            return View(pageModel);
        }
    }
}