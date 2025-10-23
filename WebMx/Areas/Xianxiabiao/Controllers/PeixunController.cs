using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Xianxiabiao.Controllers
{
    public class PeixunController : BaseLoginController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.Xianxiabiao.PeixunList.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        ///         
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.Xianxiabiao.PeixunPost.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunPost().Get(req);
            return View(pageModel);
        }
        public ActionResult Index(string month)
        {
            if (month.IsNullOrEmpty())
            {
                month = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.month = month;
            return View();
        }
    }
}