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
using static Services.Project.ServiceFactory.TingZhanService;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 对战
    /// </summary>
    public class MateController : BaseLoginController
    {
        /// <summary>
        /// 战绩列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.MateList.DtoReq();
            var pageModel = new PageFactory.TingZhan.MateList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩已提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult PostList(int id = 0)
        {
            var req = new PageFactory.TingZhan.MateTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.MateTargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩未提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult UnPostList(int id = 0)
        {
            var req = new PageFactory.TingZhan.UnMateTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UnMateTargetList().Get(req);
            return View(pageModel);
        }
    }
}