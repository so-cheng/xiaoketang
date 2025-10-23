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
namespace WebProject.Areas.Default.Controllers
{
    public class MainPageController : BaseLoginController
    {
        // GET: Default/MainPage
        public ActionResult Index()
        {
            ViewBag.page = new ModelDbSite.site_page
            {
                title = "首页",
                name = "首页"
            };
            return View();

        }
    }
}