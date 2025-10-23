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
    /// <summary>
    /// 开厅申请
    /// </summary>
    public class TgAccountController : BaseLoginController
    {
        /// <summary>
        /// 开厅申请待审核列表
        /// </summary>
        public ActionResult List(PageFactory.UserInfo.Tg_AccountList.DtoReq req)
        {
            req.yy_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.UserInfo.Tg_AccountList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"(user_base.user_sn IN (SELECT t_user_sn FROM user_relation WHERE relation_type_id = 2 AND f_user_sn = '{new UserIdentityBag().user_sn}'))";
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Create").FirstOrDefault().disabled = false;

            return View(pageModel);
        }


        /// <summary>
        /// 创建账号页面
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(PageFactory.UserCreate.DtoReq req)
        {
            req.user_type = "tger";
            var pageModel = new PageFactory.UserCreate().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 编辑账号页面
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
        //开厅申请已审核列表
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