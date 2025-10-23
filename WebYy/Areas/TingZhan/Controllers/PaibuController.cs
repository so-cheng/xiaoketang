using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using static Services.Project.ServiceFactory;
using WeiCode.Utility;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 排布
    /// </summary>
    public class PaibuController : BaseLoginController
    {
        /// <summary>
        /// 活动列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.PaibuList.DtoReq();
            var pageModel = new PageFactory.TingZhan.PaibuList().Get(req);

            pageModel.buttonGroup.buttonItems.Find(item => item.name == "post").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(item => item.name == "Post").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(item => item.name == "DayPost").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(item => item.name == "GradeList").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(item => item.name == "Score").disabled = true;
            return View(pageModel);
        }
    }
}