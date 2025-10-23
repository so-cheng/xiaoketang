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
    /// <summary>
    /// 用户等级变更
    /// </summary>
    public class GradeLogController : BaseLoginController
    {
        /// <summary>
        /// 用户等级列表
        /// </summary>
        public ActionResult List(PageFactory.GradeLogList.DtoReq req)
        {
            var pageModel = new PageFactory.GradeLogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
    }
}