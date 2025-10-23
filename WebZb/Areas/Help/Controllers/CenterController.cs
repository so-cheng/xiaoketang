using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using WeiCode.Utility;

namespace WebProject.Areas.Help.Controllers
{
    public class CenterController : BaseController
    {
        /// <summary>
        /// 展示页
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string code = "")
        {
            var help = DoMySql.FindEntity<ModelDb.help>($"category_code = '{code}'", false);
            ViewBag.help = help;
            return View();
        }
    }
}