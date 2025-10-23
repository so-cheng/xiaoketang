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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class AccountController : BaseLoginController
    {
        public ActionResult List(PageFactory.UserInfo.AccountList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.AccountList().Get(req);
            return View(pageModel);
        }

        public ActionResult FastLogin(string user_sn)
        {
            string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
            var sys_tenant_domain = new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger");
            string toUrl = $"http://{sys_tenant_domain.host_domain}/Basic/Auth/AuthorizedLogin?secret={secret}&user_sn={user_sn}";

            Response.Redirect(toUrl);
            ViewBag.secret = secret;
            ViewBag.toUrl = toUrl;
            return View();
        }
    }
}