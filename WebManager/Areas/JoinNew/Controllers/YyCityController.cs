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
    /// 运营地区-补人城市优先级
    /// </summary>
    public class YyCityController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JoinNew.YyCityList.DtoReq();
            var pageModel = new PageFactory.JoinNew.YyCityList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.JoinNew.YyCityPost.DtoReq();
            var pageModel = new PageFactory.JoinNew.YyCityPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 选择城市
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCity()
        {
            return View();
        }
    }
}