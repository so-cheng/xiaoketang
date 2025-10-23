using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 邀约
    /// </summary>
    public class YaoYueController : BaseLoginController
    {
        /// <summary>
        /// 邀约列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Xianxiazp.YaoYueList.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.YaoYueList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"p_xianxiazp_info.wx_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 邀约新增/编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Xianxiazp.YaoYuePost.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiazp.YaoYuePost().Get(req);
            return View(pageModel);
        }
    }
}