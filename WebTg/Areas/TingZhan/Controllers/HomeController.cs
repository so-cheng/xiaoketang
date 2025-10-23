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
    public class HomeController : BaseLoginController
    {
        /// <summary>
        /// 手机端菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult MobileView()
        {
            return View();
        }
    }
}