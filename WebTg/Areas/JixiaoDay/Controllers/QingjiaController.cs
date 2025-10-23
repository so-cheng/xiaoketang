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


namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class QingjiaController : BaseLoginController
    {
        /// <summary>
        /// 请假列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JixiaoDay.QingjiaList.DtoReq();
            var pageModel = new PageFactory.JixiaoDay.QingjiaList().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Pass").disabled = false;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Reject").disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
    }
}