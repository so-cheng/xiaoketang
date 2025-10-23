using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class XinRenPandingController : BaseLoginController
    {
        /// <summary>
        /// 主播规则列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.XinRenPanding.List.DtoReq();
            var pageModel = new PageFactory.UserInfo.XinRenPanding.List().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 创建/编辑规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.UserInfo.XinRenPanding.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.XinRenPanding.Post().Get(req);
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
            pageModel.listDisplay.listData.attachFilterSql = $"modular_function = '主播规则'";
            return View(pageModel);
        }
    }
}