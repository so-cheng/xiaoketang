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
    /// 绩效-厅战
    /// </summary>
    public class TingZhanController : BaseLoginController
    {
        /// <summary>
        /// 厅站数据列表页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TingZhanList(PageFactory.TingZhanList.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhanList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 上报音浪
        /// </summary>
        [HttpGet]
        public ActionResult TingZhanPost(PageFactory.TingZhanPost.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhanPost().Get(req);
            return View(pageModel);
        }
        public ActionResult Edit(PageFactory.TingZhanPost.DtoReq req)
        {
            var pageModel = new PageFactory.TingZhanPost().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            return View(pageModel);
        }
    }

}