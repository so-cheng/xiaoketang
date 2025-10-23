using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class XinRenPandingController : BaseLoginController
    {
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.UserInfo.XinRenPanding.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.XinRenPanding.Post().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.隐藏;
            return View(pageModel);
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult LogList(PageFactory.System.LogList.DtoReq req)
        {
            var pageModel = new PageFactory.System.LogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"modular_function = '主播规则' AND user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
    }
}