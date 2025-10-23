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
    /// 惩罚
    /// </summary>
    public class PunishmentController : BaseLoginController
    {
        /// <summary>
        /// 惩罚内容列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.CfContentList.DtoReq();
            var pageModel = new PageFactory.TingZhan.CfContentList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 惩罚内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.TingZhan.CfContentPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.CfContentPost().Get(req);
            return View(pageModel);
        }
    }
}