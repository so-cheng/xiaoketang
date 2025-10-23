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
    /// 战绩
    /// </summary>
    public class ScoreController : BaseLoginController
    {
        /// <summary>
        /// 战绩提报列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.ScorePostList.DtoReq();
            var pageModel = new PageFactory.TingZhan.ScorePostList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩提报
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.TingZhan.ScorePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.ScorePost().Get(req);
            return View(pageModel);
        }
    }
}