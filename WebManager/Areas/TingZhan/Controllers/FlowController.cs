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

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 流水
    /// </summary>
    public class FlowController : BaseLoginController
    {
        /// <summary>
        /// 厅站数据列表页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.TingZhan.TingZhanList.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhan.TingZhanList().Get(req);

            return View(pageModel);
        }

        /// <summary>
        /// 上报音浪
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.TingZhan.TingZhanPost.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhan.TingZhanPost().Get(req);
            return View(pageModel);
        }

        public ActionResult Edit(PageFactory.TingZhan.TingZhanPost.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhan.TingZhanPost().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            return View(pageModel);
        }
    }
}