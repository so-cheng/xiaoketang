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
    public class SystemController : BaseLoginController
    {
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult LogList(PageFactory.System.LogList.DtoReq req)
        {
            var pageModel = new PageFactory.System.LogList().Get(req);
            return View(pageModel);
        }
    }
}