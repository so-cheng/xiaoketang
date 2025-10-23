using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Xuexi.Controllers
{
    public class PageController : BaseLoginController
    {
        // GET: Xuexi/Page
        public ActionResult Index(int id)
        {
            ViewBag.id = id;
            
            return View();
        }
    }
}