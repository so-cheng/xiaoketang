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
    /// 补人权重
    /// </summary>
    public class WeightController : BaseLoginController
    {
        /// <summary>
        /// 补人权重列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.JoinNew.WeightList.DtoReq();
            var pageModel = new PageFactory.JoinNew.WeightList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅权重新增
        /// </summary>
        /// <returns></returns>
        public ActionResult TingPost()
        {
            var req = new PageFactory.JoinNew.TingWeightPost.DtoReq();
            var pageModel = new PageFactory.JoinNew.TingWeightPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 直播厅选择页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TingSelect()
        {
            var req = new PageFactory.JoinNew.TingSelect.DtoReq();
            var pageModel = new PageFactory.JoinNew.TingSelect().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 团队权重新增
        /// </summary>
        /// <returns></returns>
        public ActionResult YyPost()
        {
            var req = new PageFactory.JoinNew.YyWeightPost.DtoReq();
            var pageModel = new PageFactory.JoinNew.YyWeightPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 运营选择页面
        /// </summary>
        /// <returns></returns>
        public ActionResult YySelect()
        {
            var req = new PageFactory.JoinNew.YySelect.DtoReq();
            var pageModel = new PageFactory.JoinNew.YySelect().Get(req);
            return View(pageModel);
        }
    }
}