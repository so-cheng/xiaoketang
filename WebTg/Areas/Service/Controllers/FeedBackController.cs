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
using WeiCode.Modular;

namespace WebProject.Areas.Service.Controllers
{
    public class FeedBackController : BaseLoginController
    {
        /// <summary>
        /// 主播反馈记录
        /// </summary>
        public ActionResult List(PageFactory.FeedBackList.DtoReq req)
        {
            var pageModel = new PageFactory.FeedBackList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
    }
}