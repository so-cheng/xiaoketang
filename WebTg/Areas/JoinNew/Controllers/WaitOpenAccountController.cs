
using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Models;
using WeiCode.Services;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class WaitOpenAccountController : BaseLoginController
    {
        #region 待开账号
        /// <summary>
        /// 待开账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Wait()
        {
            var req = new PageFactory.JoinNew.WaitOpenAccountList.DtoReq();
            var pageModel = new PageFactory.JoinNew.WaitOpenAccountList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 流失操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult CausePost(string id)
        {
            var req = new PageFactory.JoinNew.CausePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.CausePost().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 创建主播账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(int user_info_zb_id)
        {
            var req = new PageFactory.JoinNew.UserCreate.DtoReq();
            req.user_info_zb_id = user_info_zb_id;
            req.user_type = "zber";
            var pageModel = new PageFactory.JoinNew.UserCreate().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 待开账号背调基础信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackTuning(int id)
        {
            var req = new PageFactory.JoinNew.BdPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.BdPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}