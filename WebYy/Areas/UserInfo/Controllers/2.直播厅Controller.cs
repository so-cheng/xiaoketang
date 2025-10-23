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
    /// <summary>
    /// 开厅申请
    /// </summary>
    public class TingController : BaseLoginController
    {
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.TgInfoList.DtoReq();
            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Del").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Transform").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "DouUser").disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }


    }
}