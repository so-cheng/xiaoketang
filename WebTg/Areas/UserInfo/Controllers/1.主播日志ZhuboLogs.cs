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
    public class ZhuboLogsController : BaseLoginController
    {
        #region 日志
        public ActionResult LogList()
        {
            var req = new PageFactory.UserInfo.LogList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion
    }
}