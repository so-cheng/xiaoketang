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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TingInfoController : BaseLoginController
    {
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.TgInfoList.DtoReq();

            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"yy_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);

        }
        public ActionResult CloseList()
        {
            var req = new PageFactory.UserInfo.CloseList.DtoReq();
            var pageModel = new PageFactory.UserInfo.CloseList().Get(req);
            return View(pageModel);
        }
        [HttpGet]
        public ActionResult InfoEdit(int id = 0)
        {
            var req = new PageFactory.UserInfo.TgInfoEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.TgInfoEdit().Get(req);
            return View(pageModel);
        }
    }
}