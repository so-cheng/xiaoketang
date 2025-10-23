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
            var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn = '{new UserIdentityBag().user_sn}'",false);
            if (user_info_zhubo.anchor_id.IsNullOrEmpty()&& user_info_zhubo.dou_username.IsNullOrEmpty())
            {
                Response.Redirect("SetInfo");
            }

            ViewBag.wechat_username = "";

            var findsumsql = $"select SUM(income) as sumincome from doudata_day_zb where YEAR(c_date)=YEAR(CURDATE()) and MONTH(c_date)=MONTH(CURDATE()) and zb_user_sn='{new UserIdentityBag().user_sn}'";
            var sumdata = DoMySql.FindObjectsBySql(findsumsql);

            var findgroupsql = $"select SUM(income) as sumincome,DATE_FORMAT(c_date,'%Y-%m-%d') as c_date from doudata_day_zb where c_date > DATE_ADD(NOW(),INTERVAL -3 DAY) and zb_user_sn='{new UserIdentityBag().user_sn}' group by c_date,income";
            var datas = DoMySql.FindObjectsBySql(findgroupsql);
            var beforeday = "0";
            var yesterday = "0";
            var today = "0";
            var sum = sumdata[0]["sumincome"].ToString();
            foreach (var item in datas)
            {
                if (item["c_date"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    today = item["sumincome"].ToString();
                }

                if (item["c_date"].ToString() == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                {
                    yesterday = item["sumincome"].ToString();
                }

                if (item["c_date"].ToString() == DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"))
                {
                    beforeday = item["sumincome"].ToString();
                }
            }

            ViewBag.ToDay = today;
            ViewBag.YesterDay = yesterday;
            ViewBag.BeforeDay = beforeday;
            ViewBag.Sum = sum;
            return View();
        }

        public ActionResult SetInfo()
        {
            var req = new PageFactory.UserInfo.ZbMobileInfoPost.DtoReq();
            var pageModel = new PageFactory.UserInfo.ZbMobileInfoPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <returns></returns>
        public ActionResult BindWechat()
        {
            return View();
        }

        public RedirectToRouteResult BindWechatAction()
        {
            var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{new UserIdentityBag().user_sn}'", false);
            var wechat = new PlatformSdk.WeixinMP();

            user_info_zb.wechat_username = wechat.GetOpenId();
            user_info_zb.Update();

            return RedirectToAction("MobileView");
        }

    }
}