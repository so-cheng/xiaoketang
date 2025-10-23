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

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 对方信息
    /// </summary>
    public class UserInfoController : BaseLoginController
    {
        /// <summary>
        /// 对方信息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.UserInfoList.DtoReq();
            var pageModel = new PageFactory.TingZhan.UserInfoList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 对方信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            var req = new PageFactory.TingZhan.UserInfoView.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UserInfoView().Get(req);
            return View(pageModel);
        }
    }
}