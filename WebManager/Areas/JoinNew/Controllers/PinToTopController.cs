using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 运营补人申请置顶
    /// </summary>
    public class PinToTopController : BaseLoginController
    {
        /// <summary>
        /// 置顶次数列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JoinNew.PinToTopList.DtoReq();
            var pageModel = new PageFactory.JoinNew.PinToTopList().Get(req);
            return View(pageModel);
        }
    }
}