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
namespace WebProject.Areas.Jixiao.Controllers
{
    /// <summary>
    /// 每日绩效
    /// </summary>
    public class UploadController : BaseLoginController
    {
        /// <summary>
        /// 绩效列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserDayReportSession.DtoReq req)
        {
            var pageModel = new PageFactory.UserDayReportSession().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
    }
}