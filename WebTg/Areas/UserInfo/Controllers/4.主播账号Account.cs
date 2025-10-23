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
    /// 主播账号
    /// </summary>
    public class AccountController : BaseLoginController
    {

        /// <summary>
        /// 账号列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountList()
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountList.DtoReq();
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn in ({new ServiceFactory.UserInfo.Tg().TgGetNextZbForSql(new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }

        /// <summary>
        /// 编辑账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountEdit().Get(req);
            return View(pageModel);
        }
        

        /// <summary>
        /// 批量换厅
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult ChangeTing(string ids)
        {
            var req = new PageFactory.UserInfo.ChangeTing.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.UserInfo.ChangeTing().Get(req);
            return View(pageModel);
        }
    }
}