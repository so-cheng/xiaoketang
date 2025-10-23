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

namespace WebProject.Areas.Xianxia.Controllers
{
    public class DouyinController : BaseLoginController
    {
        public ActionResult List(PageFactory.DouyinList.DtoReq req)
        {
            var pageModel = new PageFactory.DouyinList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"wx_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.DouyinPost.DtoReq req)
        {
            var pageModel = new PageFactory.DouyinPost().Get(req);
            return View(pageModel);
        }
    }
}