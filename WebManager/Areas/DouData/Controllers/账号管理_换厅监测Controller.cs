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

namespace WebProject.Areas.DouData.Controllers
{
    public class Accer_ListenTingController : BaseLoginController
    {
        /// <summary>
        /// 换厅监测
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult ChangeListen(PageFactory.DouData.TingChangeList.DtoReq req)
        {
            var pageModel = new PageFactory.DouData.TingChangeList().Get(req);
            return View(pageModel);
        }
    }
}