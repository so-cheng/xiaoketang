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
    /// 跨房活动（暂停）
    /// </summary>
    public class KuafangController : BaseLoginController
    {
        /// <summary>
        /// 厅站数据列表页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult KuafangList(PageFactory.KuafangList.DtoReq req)
        {
            var pageModel = new PageFactory.KuafangList().Get(req);
            //pageModel.buttonGroup.buttonItems.Where(x => x.text == "上报音浪").FirstOrDefault().disabled = true;
            //pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }

        /// <summary>
        /// 上报音浪
        /// </summary>
        [HttpGet]
        public ActionResult KuafangPost(PageFactory.KuafangPost.DtoReq req)
        {
            var pageModel = new PageFactory.KuafangPost().Get(req);
            return View(pageModel);
        }
        public ActionResult Edit(PageFactory.KuafangPost.DtoReq req)
        {
            var pageModel = new PageFactory.KuafangPost().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "c_date").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.只读;
            return View(pageModel);
        }
    }
}