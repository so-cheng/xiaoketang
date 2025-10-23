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

namespace WebProject.Areas.Service.Controllers
{
    public class FeedBackController : BaseLoginController
    {
        /// <summary>
        /// 主播反馈记录
        /// </summary>
        public ActionResult List(PageFactory.FeedBackList.DtoReq req)
        {
            var pageModel = new PageFactory.FeedBackList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn= '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }





        /// <summary>
        /// 主播提交反馈页面
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.FeedBackPost.DtoReq req)
        {
            var pageModel = new PageFactory.FeedBackPost().Get(req);
            return View(pageModel);
        }



        public ActionResult ConfidePost(PageFactory.ConfidePost.DtoReq req)
        {
            var pageModel = new PageFactory.ConfidePost().Get(req);
            return View(pageModel);
        }


        public ActionResult RecommendPost(PageFactory.RecommendPost.DtoReq req)
        {
            var pageModel = new PageFactory.RecommendPost().Get(req);
            return View(pageModel);
        }
    }
}