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


namespace WebProject.Areas.JixiaoDay.Controllers
{
    /// <summary>
    /// 厅收益上交
    /// </summary>
    public class ShangjiaoController : BaseLoginController
    {
        /// <summary>
        /// 厅收益上交列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JixiaoDay.ShangjiaoList.DtoReq();
            var pageModel = new PageFactory.JixiaoDay.ShangjiaoList().Get(req);
            return View(pageModel);
        }
    }
}