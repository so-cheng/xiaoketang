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
    public class HomeController : BaseLoginController
    {
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
            var zbInfo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);
            var zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{zbInfo.user_sn}'", false);
            var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{zbInfo.user_sn}'", false);
            ViewBag.username = zbInfo.username;
            ViewBag.wechat_username = user_info_zb.wechat_username;

            string img_url = "/Assets/images/default_head.jpg";
            if (!zhubo.img_url.IsNullOrEmpty())
            {
                img_url = zhubo.img_url;
            }
            ViewBag.img_url = img_url;
            return View();
        }

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <returns></returns>
        public ActionResult SetHead()
        {
            var zb = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'",false);

            return View();
        }

        public RedirectToRouteResult SaveHead(string head)
        {
            var zb = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'",false);
            if (zb.IsNullOrEmpty())
            {
                var zbinfo = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{new UserIdentityBag().user_sn}'");
                zb = zbinfo.ToModel<ModelDb.user_info_zhubo>();
                zb.id = 0;
            }
            zb.img_url = head;
            zb.InsertOrUpdate();
            return RedirectToAction("MobileView");
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <returns></returns>
        public ActionResult BindWechat()
        {
            return View();
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