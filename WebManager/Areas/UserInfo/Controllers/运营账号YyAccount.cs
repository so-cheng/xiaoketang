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
using static WeiCode.Utility.UtilityStatic;
namespace WebProject.Areas.UserInfo.Controllers
{
    public class YyAccountController : BaseLoginController
    {
        /// <summary>
        /// 关联团队
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult With(PageFactory.UserInfo.WithList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.WithList().Get(req);
            return View(pageModel);
        }

        public ActionResult WithPost(PageFactory.UserInfo.WithPost.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.WithPost().Get(req);
            return View(pageModel);
        }
    }
}