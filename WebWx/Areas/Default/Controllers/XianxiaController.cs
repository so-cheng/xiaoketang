using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Areas.Default.Controllers
{
    public class XianxiaController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.XianxiaList.DtoReq req)
        {
            var pageModel = new PageFactory.XianxiaList().Get(req);
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.XianxiaPost.DtoReq req)
        {
            var pageModel = new PageFactory.XianxiaPost().Get(req);
            return View(pageModel);
        }
    }
}

