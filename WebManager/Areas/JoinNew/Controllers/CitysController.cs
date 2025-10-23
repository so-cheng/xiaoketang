using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 中台地区
    /// </summary>
    public class CitysController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JoinNew.AllZtCityList.DtoReq();
            var pageModel = new PageFactory.JoinNew.AllZtCityList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.JoinNew.AllZtCityPost.DtoReq();
            var pageModel = new PageFactory.JoinNew.AllZtCityPost().Get(req);
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