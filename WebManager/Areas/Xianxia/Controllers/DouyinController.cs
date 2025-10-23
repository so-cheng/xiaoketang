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

namespace WebProject.Areas.Xianxia.Controllers
{
    /// <summary>
    /// 抖音每日数据
    /// </summary>
    public class DouyinController : BaseLoginController
    {
        /// <summary>
        /// 抖音列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(PageFactory.DouyinList.DtoReq req)
        {
            var pageModel = new PageFactory.DouyinList().Get(req);
            pageModel.buttonGroup.buttonItems[0].disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 新增抖音每日数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.DouyinPost.DtoReq req)
        {
            var pageModel = new PageFactory.DouyinPost().Get(req);
            return View(pageModel);
        }
    }
}