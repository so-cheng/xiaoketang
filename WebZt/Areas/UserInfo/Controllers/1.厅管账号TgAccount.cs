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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TgAccountController : BaseLoginController
    {
        #region 账号列表页面
        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.Tg_AccountList.DtoReq();
            var pageModel = new PageFactory.UserInfo.Tg_AccountList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建账号页面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Eidt(int id = 0)
        {
            var req = new PageFactory.UserInfo.Tg_AccountEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.Tg_AccountEdit().Get(req);
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
        #endregion
    }
}