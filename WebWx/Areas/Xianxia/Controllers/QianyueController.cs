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
    public class QianyueController : BaseLoginController
    {
        public ActionResult List(PageFactory.XianxiaList.DtoReq req)
        {
            var pageModel = new PageFactory.XianxiaList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"wx_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.XianxiaPost.DtoReq req)
        {
            var pageModel = new PageFactory.XianxiaPost().Get(req);
            return View(pageModel);
        }
    }
}