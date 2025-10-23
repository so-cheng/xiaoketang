using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;
using WeiCode.ModelDbs;
namespace WebProject.Areas.Waixuan.Controllers
{
    public class DatasController : BaseLoginController
    {
        /// <summary>
        /// 审批情况统计
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="yy_user_sn"></param>
        /// <returns></returns>
        public ActionResult ApproveSumTable(string dateRange = "")
        {
            ViewBag.dateRange = dateRange;
            return View();
        }
    }
}