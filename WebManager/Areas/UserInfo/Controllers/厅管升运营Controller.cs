using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using Services.Project;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TgPromotionYyController : BaseLoginController
    {
        // GET: UserInfo/厅管升运营
        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.TgPromotionYy.DtoReq();
            var pageModel = new PageFactory.UserInfo.TgPromotionYy().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(string user_sn)
        {
            var req = new PageFactory.UserInfo.TgUpgradeYyPost.DtoReq();
            req.tg_user_sn = user_sn;
            var pageModel = new PageFactory.UserInfo.TgUpgradeYyPost().Get(req);
            return View(pageModel);
        }
    }
}