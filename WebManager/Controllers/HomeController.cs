using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using Services.Project;
using WeiCode.ModelDbs;
using WeiCode.Domain;


namespace WebProject.Controllers
{
    [BaseLogin]
    public class HomeController : BaseController
    {

        /// <summary>
        /// 移动端视图
        /// </summary>
        /// <returns></returns>
        public ActionResult MobileView()
        {
            var yyInfo = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(new UserIdentityBag().user_sn);
            ViewBag.username = yyInfo.username;
            ViewBag.wechat_username = "";
            return View();
        }

        public ContentResult Index()
        {
            Response.Redirect("/Basic/HomePage/Index");
            return Content("");
        }

        public ActionResult MIndex()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 微信openid登记
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


        public ContentResult Send()
        {
            //new PlatformSdk.WeixinMP().SendTemplateMessage("GNLsQPuQDHNGjSw9qfPfJcKh3K9jpeDWLkrj7UV3cbU", new DomainBasic.UserApp().GetInfoByUserSn("20210504154936061-1809088913").attach4, "有补人申请需要审核", "http://manager.beta.doupeixun.cn/Waixuan/ApproveApplication/ApproveApplicationList", @"{""thing1"":""张三""}");

            new ServiceFactory.Sdk.WeixinSendMsg().Approve("20240916205535390-151589563", "有补人申请需要审核", "http://manager.beta.doupeixun.cn/Waixuan/ApproveApplication/ApproveApplicationList", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
            {
                person = "李四",
                post_time = DateTime.Now.AddDays(-1)
            });
            return Content("");
        }

        public ContentResult JiezouMouth()
        {
            new TaskProject.ProjectClass().JiezouMouth();
            return Content("");
        }
        
    }
}