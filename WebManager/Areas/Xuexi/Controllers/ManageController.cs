using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Xuexi.Controllers
{
    public class ManageController : BaseLoginController
    {

        public ActionResult List()
        {
            var req = new PageFactory.XuexiList.DtoReq();
            var pageModel = new PageFactory.XuexiList().Get(req);
            return View(pageModel);
        }

        public ActionResult Post()
        {
            var req = new PageFactory.XuexiPost.DtoReq();
            var pageModel = new PageFactory.XuexiPost().Get(req);
            return View(pageModel);
        }


        public ActionResult CategoryList()
        {
            var req = new PageFactory.XuexiCategoryList.DtoReq();
            var pageModel = new PageFactory.XuexiCategoryList().Get(req);
            return View(pageModel);
        }

        public ActionResult CategoryPost(PageFactory.XuexiCategoryPost.DtoReq req)
        {
            var pageModel = new PageFactory.XuexiCategoryPost().Get(req);
            return View(pageModel);
        }
    }
}