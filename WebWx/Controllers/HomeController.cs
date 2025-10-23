using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.Services;

using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Controllers
{
    [BaseLogin]
    public class HomeController : BaseController
    {
        public ContentResult Index()
        {
            Response.Redirect("/Basic/HomePage/Index");
            return Content("");
        }

        public ActionResult Main()
        {
            ViewBag.id = new UserIdentityBag().id;
            ViewBag.username = new UserIdentityBag().username;
            ViewBag.name = new UserIdentityBag().name;
            var manager_base = DoMySql.FindEntity<ModelDbBasic.user_base>($"id = {new UserIdentityBag().id}", false);
            ViewBag.manager_base = manager_base;

            ViewBag.manager_Menus = DoMySql.FindList<ModelDbBasic.sys_modular_menu>($"parent_id = 0 AND user_type_id = '{new DomainBasic.TenantDomainApp().GetInfo().user_type_id}' AND id in (SELECT menu_id FROM sys_role__menu WHERE role_id = {new UserIdentityBag().cur_role_id}) ORDER BY sort, id desc");
            return View();
        }
    }
}