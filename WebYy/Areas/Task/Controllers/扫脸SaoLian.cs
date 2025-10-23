using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Task.Controllers
{
    public class SaoLianController : BaseLoginController
    {
        /// <summary>
        /// 扫脸任务统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskList(PageFactory.Task.FaceList.DtoReq req)
        {

            var pageModel = new PageFactory.Task.FaceList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅管最后核查时间
        /// </summary>
        /// <returns></returns>
        public ActionResult NoBelow(PageFactory.Task.FaceList_TG.DtoReq req)
        {
            var pageModel = new PageFactory.Task.FaceList_TG().Get(req);
            return View(pageModel);
        }
    }
}