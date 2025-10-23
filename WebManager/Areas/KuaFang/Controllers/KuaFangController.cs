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
using WeiCode.Utility;

namespace WebProject.Areas.KuaFang.Controllers
{
    public class KuaFangController : BaseLoginController
    {
        /// <summary>
        /// 跨房列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.KuaFang.List.DtoReq();
            var pageModel = new PageFactory.KuaFang.List().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建/编辑跨房
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.KuaFang.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.KuaFang.Post().Get(req);
            return View(pageModel);
        }
    }
}