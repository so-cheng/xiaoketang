using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    public class TablesController : BaseLoginController
    {
        /// <summary>
        /// 数据分析
        /// </summary>
        /// <returns></returns>
        public ActionResult Analysis(string tg_user_sn="")
        {
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            var PJoinNeedAnalysis = new ServiceFactory.JoinAnalysisService().GetTgTable(tg_user_sn);
            
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.PJoinNeedAnalysis = PJoinNeedAnalysis;
            return View();
        }



    }
}