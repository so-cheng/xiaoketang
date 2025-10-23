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
    public class MyInfoController : BaseLoginController
    {
        [HttpGet]
        public ActionResult Post()
        {
            var req = new PageFactory.UserInfo.Tg_AccountEdit.DtoReq();
            req.id = new UserIdentityBag().id;
            var pageModel = new PageFactory.UserInfo.Tg_AccountEdit().Get(req);
            return View(pageModel);
        }
    }
}