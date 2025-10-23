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
    public class Accer_TingController : BaseLoginController
    {

        /// <summary>
        /// 流水无归属
        /// </summary>
        /// <returns></returns>
        public ActionResult NoBindTing()
        {
            var req = new PageFactory.DouData.NoBindTingList.DtoReq();
            var pageModel = new PageFactory.DouData.NoBindTingList().Get(req);
            return View(pageModel);
        }
    }
}