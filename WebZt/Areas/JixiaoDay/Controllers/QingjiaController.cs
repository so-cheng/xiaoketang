using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class QingjiaController : BaseLoginController
    {
        public ActionResult List()
        {
            var req = new PageFactory.JixiaoDay.QingjiaList.DtoReq();
            var pageModel = new PageFactory.JixiaoDay.QingjiaList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zt_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
    }
}