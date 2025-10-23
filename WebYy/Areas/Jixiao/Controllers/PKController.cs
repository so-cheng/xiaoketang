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
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas.Jixiao.Controllers
{
    /// <summary>
    /// 主播PK（暂停）
    /// </summary>
    public class PKController : BaseLoginController
    {
        public ActionResult Index(C_zbs c_zbs,string dateRange,string users)
        {
            if (dateRange.IsNullOrEmpty())
            {
                dateRange = DateTime.Today.ToString("yyyy-MM-01") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            string zbs = "";
            if (!c_zbs.l_zbs.IsNullOrEmpty())
            {
                foreach (var item in c_zbs.l_zbs.user_sn)
                {
                    zbs += $"'{item}',";
                }
                zbs = zbs.Substring(0, zbs.Length - 1);
            }
            if (!users.IsNullOrEmpty())
            {
                zbs = users;
            }
            ViewBag.zbs = zbs;
            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.dateRange = dateRange;
            return View();
        }
        /// <summary>
        /// 选择pk的主播
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        /*public ActionResult choose()
        {
            return View();
        }*/
        public ActionResult choose(PageFactory.ZbPKPost.DtoReq req)
        {
            var pageModel = new PageFactory.ZbPKPost().Get(req);
            return View(pageModel);
        }
        public class C_zbs
        {
            public L_zbs l_zbs { get; set; }
            public class L_zbs
            {
                public string[] user_sn { get; set; }
            }
        }
    }
}