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
    /// 用户跟进
    /// </summary>
    public class FollowController : BaseLoginController
    {
        /// <summary>
        /// 跟进信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var A_level = DoMySql.FindField<ModelDb.crm_grade_log>("count(id)", $"n_grade_id= '{3}' and create_time>'{DateTime.Today}' and create_time<'{DateTime.Today.AddDays(1)}'");
            ViewBag.A_level = (A_level[0].IsNullOrEmpty() ? "0" : A_level[0]);

            var else_level = DoMySql.FindField<ModelDb.crm_grade_log>("count(id)", $"n_grade_id!= '{3}' and create_time>'{DateTime.Today}' and create_time<'{DateTime.Today.AddDays(1)}'");
            ViewBag.else_level = (else_level[0].IsNullOrEmpty() ? "0" : else_level[0]);
            return View();
        }
    }
}