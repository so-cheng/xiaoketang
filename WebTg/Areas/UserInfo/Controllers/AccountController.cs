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
    /// 主播、厅管账号
    /// </summary>
    public class AccountController : BaseLoginController
    {
        #region 主播
        /// <summary>
        /// 主播列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserInfo.UserList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.UserList().Get(req);
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "InfoPost").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "ExcelPost").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "UnDel").FirstOrDefault().disabled = false;
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listBatchItems.Find(x => x.title == "ChangeTing").disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }


        /// <summary>
        /// 回收站列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult UnDel(PageFactory.UserInfo.UserUnDel.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.UserUnDel().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn, "user_base.status = 9")}";
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

        #region 厅管
        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TgList(PageFactory.UserInfo.UserList.DtoReq req)
        {
            req.isShowZbInfo = false;
            var pageModel = new PageFactory.UserInfo.UserList().Get(req);
            pageModel.buttonGroup.buttonItems.Where(x => x.title == "create").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }

        public ActionResult TgFastLogin(string user_sn)
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