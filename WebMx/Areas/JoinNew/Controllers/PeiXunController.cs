using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class PeiXunController : BaseLoginController
    {
        /// <summary>
        /// 培训数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.JoinNew.PeixunList.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.PeixunList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 培训数据新增/编辑   
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.JoinNew.PeixunPost.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.PeixunPost().Get(req);
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