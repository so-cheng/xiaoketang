using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;


namespace WebProject.Areas.PUserBasic.Controllers
{
    public class UserOrganizationController : BaseLoginController
    {
        // GET: PUserBasic/UserOrganization
        public ActionResult Index()
        {
            return View();
        }
    }
}