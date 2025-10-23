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

namespace WebProject.Areas.Task.Controllers
{
    public class FaceController : BaseLoginController
    {
        /// <summary>
        /// 扫脸任务列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Task.FaceList.DtoReq();
            var pageModel = new PageFactory.Task.FaceList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "ting_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
    }
}