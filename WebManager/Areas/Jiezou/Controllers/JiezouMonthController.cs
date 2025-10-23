using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskProject;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;

namespace WebProject.Areas.Jiezou.Controllers
{
    public class JiezouMonthController : BaseLoginController
    {
        /// <summary>
        /// 节奏阶段展示
        /// </summary>
        /// <param name="jiezou_sn"></param>    
        public ActionResult Item(string jiezou_sn, string yy_user_sn = "", string is_standard = "")
        {
            ViewBag.jiezou_sn = jiezou_sn;
            //仅在yy_user_sn为空时设置默认值，否则使用传入的参数
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = new ServiceFactory.UserInfo.Yy().GetAllYyForKv().First().Value;
            }
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.is_standard = is_standard;
            return View();
        }

        /// <summary>
        /// 查看历史节奏
        /// </summary>    
        /// <returns></returns>
        public ActionResult Detail(string jiezou_sn, string ting_sn, string c_date)
        {
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.ting_sn = ting_sn;
            ViewBag.c_date = c_date;
            return View();
        }
    }
}