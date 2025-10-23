using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using WeiCode.Utility;

namespace WebProject.Areas.KuaFang.Controllers
{
    public class MateController : BaseLoginController
    {
        /// <summary>
        /// 对战列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MateList()
        {
            var req = new PageFactory.KuaFangMate.List.DtoReq();
            var pageModel = new PageFactory.KuaFangMate.List().Get(req);
            return View(pageModel);
        }
    }
}