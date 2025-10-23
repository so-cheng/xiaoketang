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
    public class JianliController : BaseLoginController
    {
        // GET: Xianxiazp/Jianli
        public ActionResult FiledConfig()
        {
            var req = new PageFactory.Xianxiazp.FiledConfig.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.FiledConfig().Get(req);
            return View(pageModel);
        }
    }
}