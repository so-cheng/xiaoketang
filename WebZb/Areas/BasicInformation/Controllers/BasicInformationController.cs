using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using Services.Project;

namespace WebProject.Areas.BasicInformation.Controllers
{
    public class BasicInformationController : BaseLoginController
    {
        /// <summary>
        /// 更换接档号
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplaceDouUserName()
        {
            var req = new PageFactory.UserInfo.ReplaceDouUserName.DtoReq();
            var pageModel = new PageFactory.UserInfo.ReplaceDouUserName().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 绑定运营经纪人
        /// </summary>
        /// <returns></returns>
        public ActionResult Bindingjjr()
        {
            var req = new PageFactory.UserInfo.Bindingjjr.DtoReq();
            var pageModel = new PageFactory.UserInfo.Bindingjjr().Get(req);
            return View(pageModel);
        }
    }
}