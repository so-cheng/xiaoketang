using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Join.Controllers
{
    public class DatasController : BaseLoginController
    {
        /// <summary>
        /// 补人数据汇总
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApplySum(PageFactory.Join.ApplySum.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ApplySum().Get(req);
            return View(pageModel);
        }
    }
}