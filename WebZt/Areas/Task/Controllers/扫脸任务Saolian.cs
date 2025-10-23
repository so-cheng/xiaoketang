using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Task.Controllers
{
    /// <summary>
    /// 扫脸
    /// </summary>
    public class SaolianController : BaseLoginController
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
        /// 扫脸未达标
        /// </summary>
        /// <returns></returns>
        public ActionResult NoBelow(PageFactory.Task.FaceList_TG.DtoReq req)
        {

            var pageModel = new PageFactory.Task.FaceList_TG().Get(req);
            return View(pageModel);
        }
    }
}