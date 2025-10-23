using Services.Project;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;


namespace WebProject.Areas.Jiezou.Controllers
{
    public class JiezouMonthController : BaseLoginController
    {
        /// <summary>
        /// 节奏阶段展示
        /// </summary>
        /// <param name="jiezou_sn"></param>
        /// <returns></returns>
        public ActionResult Item(string jiezou_sn, string yy_user_sn,string ting_sn="", string is_standard = "")
        {
            if (yy_user_sn.IsNullOrEmpty()) yy_user_sn = new UserIdentityBag().user_sn;
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.is_standard = is_standard;
            ViewBag.ting_sn = ting_sn;
            return View();
        }

        /// <summary>
        /// 查看历史节奏
        /// </summary>    
        /// <returns></returns>
        public ActionResult Detail(string jiezou_sn, string tg_user_sn, string c_date)
        {
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.c_date = c_date;
            return View();
        }
    }
}