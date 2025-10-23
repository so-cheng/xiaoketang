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

namespace WebProject.Areas._2.ZbManage.Controllers
{
    /// <summary>
    /// 客户
    /// </summary>
    public class UserController : BaseLoginController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public ActionResult List(PageFactory.CrmList.Req req)
        {
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
    }
}