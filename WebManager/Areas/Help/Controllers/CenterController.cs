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

namespace WebProject.Areas.Help.Controllers
{
    public class CenterController : BaseLoginController
    {
        /// <summary>
        /// 帮助列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Help.List.DtoReq();
            var pageModel = new PageFactory.Help.List().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建/编辑帮助
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.Help.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Help.Post().Get(req);
            return View(pageModel);
        }
    }
}