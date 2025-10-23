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

namespace WebProject.Areas.Default.Controllers
{
    /// <summary>
    /// 用户等级变更记录
    /// </summary>
    public class GradeLogController : BaseLoginController
    {
        /// <summary>
        /// 用户等级列表
        /// </summary>
        public ActionResult List(PageFactory.GradeLogList.DtoReq req)
        {
            var pageModel = new PageFactory.GradeLogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"n_grade_id = '{req.n_grade_id}'";
            return View(pageModel);
        }

        public ActionResult NonList(PageFactory.GradeLogList.DtoReq req)
        {
            var pageModel = new PageFactory.GradeLogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"n_grade_id != '{req.n_grade_id}'";
            return View(pageModel);
        }
    }
}