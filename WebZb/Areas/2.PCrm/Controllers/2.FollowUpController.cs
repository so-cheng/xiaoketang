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

namespace WebProject.Areas.PCrm.Controllers
{
    public class FollowUpController : BaseLoginController
    {
        /// <summary>
        /// 跟进表单提交
        /// </summary>
        public ActionResult Post(PageFactory.CustomerFollow.DtoReq req)
        {
            var pageModel = new PageFactory.CustomerFollow().Get(req);
            return View(pageModel);
        }



        /// <summary>
        /// 跟进记录
        /// </summary>
        public ActionResult List(PageFactory.FollowUpList.DtoReq req)
        {
            var pageModel = new PageFactory.FollowUpList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
    }
}