using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.TingZhanService;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 排布
    /// </summary>
    public class PaibuController : BaseLoginController
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.PaibuList.DtoReq();
            var pageModel = new PageFactory.TingZhan.PaibuList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建/编辑厅战
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.TingZhan.PaibuPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.PaibuPost().Get(req);
            return View(pageModel);
        }
    }
}