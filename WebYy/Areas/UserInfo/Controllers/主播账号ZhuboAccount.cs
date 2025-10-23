using Services.Project;
using System.Linq;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboAccountController : BaseLoginController
    {
        #region 主播名单

        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.OnJobList.DtoReq(); 
            var pageModel = new PageFactory.UserInfo.OnJobList().Get(req);
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Create").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"yy_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountPost.DtoReq();
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string user_sn)
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountEdit.DtoReq();
            req.id = new DomainBasic.UserApp().GetInfoByUserSn(user_sn).id;
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑背调
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult InfoEdit(int id)
        {
            var req = new PageFactory.UserInfo.OnJobPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.OnJobPost().Get(req);
            return View(pageModel);
        }

        public ActionResult FastLogin(string user_sn)
        {
            string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
            var sys_tenant_domain = new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "zber");
            string toUrl = $"http://{sys_tenant_domain.host_domain}/Basic/Auth/AuthorizedLogin?secret={secret}&user_sn={user_sn}";

            Response.Redirect(toUrl);
            ViewBag.secret = secret;
            ViewBag.toUrl = toUrl;
            return View();
        }
        #endregion
    }
}