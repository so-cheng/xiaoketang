using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.Services;

using WeiCode.ModelDbs;
using Services.Project;
using WeiCode.Models;

namespace WebProject.Controllers
{
    public class HomeController : BaseLoginController
    {
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
        public ContentResult Index()
        {
            Response.Redirect(new DomainBasic.UserTypeApp().GetDefaultPage());
            return Content("");
        }
        /// <summary>
        /// 移动端视图
        /// </summary>
        /// <returns></returns>
        public ActionResult MobileView()
        {
            var tgInfo = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn);
            ViewBag.username = tgInfo.username;
            ViewBag.wechat_username = "";
            return View();
        }

        /// <summary>
        /// 手机端设置基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SetInfo()
        {
            var req = new PageFactory.UserInfo.Tg_AccountEdit.DtoReq();
            req.id = new UserIdentityBag().id;
            var pageModel = new PageFactory.UserInfo.Tg_AccountEdit().Get(req);
            pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
            {
                returnType = ModelBasic.PagePost.PostedReturn.ReturnType.当前窗口跳转URL,
                returnUrl = "/Home/MobileView",
            };
            return View(pageModel);
        }

        /// <summary>
        /// 厅管微信openid登记
        /// </summary>
        /// <returns></returns>
        public ActionResult WechatRegist()
        {
            string open_id = new PlatformSdk.WeixinMP().GetOpenId();
            new ModelDb.user_base()
            {
                attach4 = open_id,
            }.Update($"user_sn='{new UserIdentityBag().user_sn}'");
            return Content("绑定成功");
        }


        public JsonResult GetSystemMessage()
        {
            var notice = DoMySql.FindList<ModelDb.sys_notice>($"user_sn = '{new UserIdentityBag().user_sn}' and is_read = '{ModelDb.sys_notice.is_read_enum.未读.ToSByte()}'");
            return Json(new
            {
                code = 0,
                msg = "success",
                data = notice.Count,
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category_id">消息类型id</param>
        /// <returns></returns>
        public ActionResult ListHtml(int category_id = 1)
        {
            var req = new PageFactory.ListHtml1.DtoReq();
            req.category_id = category_id;
            var pageModel = new PageFactory.ListHtml1().Get(req);
            return View(pageModel);
        }

        public ContentResult ReadNotice(int notice_id)
        {/*
            var notice = new ServiceFactory.NoticeService().ReadNotice(notice_id);
            Response.Redirect(notice.link_url);*/
            return Content("");
        }

    }
}