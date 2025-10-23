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
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas.UserTable.Controllers
{
    public class GradeController : BaseLoginController
    {
        // GET: UserTable/Grade
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Table()
        {
            return View();
        }
    }
}