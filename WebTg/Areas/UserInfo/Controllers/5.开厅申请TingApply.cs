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
    public class TingApplyController : BaseLoginController
    {
        /// <summary>
        /// 开厅申请列表
        /// </summary>
        public ActionResult List(PageFactory.UserInfo.TingApplyList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.TingApplyList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Del").disabled = false;
            return View(pageModel);
        }

        /// <summary>
        /// 提交开厅申请
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(PageFactory.UserInfo.TingApplyPost.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.TingApplyPost().Get(req);
            return View(pageModel);
        }
    }
}