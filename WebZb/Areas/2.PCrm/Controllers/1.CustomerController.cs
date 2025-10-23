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
using WeiCode.Modular;

namespace WebProject.Areas.PCrm.Controllers
{

    /// <summary>
    /// 客户管理
    /// </summary>
    public class CustomerController : BaseLoginController
    {
        /// <summary>
        /// 主播端列表
        /// </summary>
        public ActionResult List(PageFactory.CrmList.Req req)
        {
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn= '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        /*
         * /// <summary>
        /// 登记/编辑用户页面
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.CustomerPost.DtoReq req)
        {
            var pageModel = new PageFactory.CustomerPost().Get(req);
            return View(pageModel);
        }
         */


        /// <summary>
        /// 转移客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Moves(PageFactory.CustomerMoves.DtoReq req)
        {
            var pageModel = new PageFactory.CustomerMoves().Get(req);
            return View(pageModel);
        }
    }
}