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

namespace WebProject.Areas.PBasic.Controllers
{
    public class ManageController : Controller
    {
        /// <summary>
        /// 账号列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int user_type_id = 0)
        {
            var userBaseTplReq = new UserBaseTplPara(user_type_id);
            userBaseTplReq.Add("keyword", "");
            var pageModel = new PageFactory.UserBaseTList().Get(userBaseTplReq);
            return View(pageModel);
        }

    }
}