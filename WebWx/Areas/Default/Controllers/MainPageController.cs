using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebProject.Areas.Default.Controllers
{
    public class MainPageController : Controller
    {
        // GET: Default/MainPage
        public ActionResult Index()
        {
            return View();
        }
    }
}