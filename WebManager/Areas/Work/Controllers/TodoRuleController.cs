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

namespace WebProject.Areas.Work.Controllers
{
    public class TodoRuleController : BaseLoginController
    {
        public ActionResult List(int status = 0)
        {
            var req = new PageFactory.Work.TodoRule.List.DtoReq();
            req.status = status;
            var pageModel = new PageFactory.Work.TodoRule.List().Get(req);
            return View(pageModel);
        }
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.Work.TodoRule.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Work.TodoRule.Post().Get(req);
            return View(pageModel);
        }    
    }
}