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

namespace WebProject.Areas.PCrm.Controllers
{

    /// <summary>
    /// 用户列表
    /// </summary>
    public class TypeController : BaseLoginController
    {
        #region 用户列表
        /// <summary>
        /// 类型列表页
        /// </summary>
        public ActionResult List(PageFactory.TypeList.DtoReq req)
        {
            var pageModel = new PageFactory.TypeList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 新建类型
        /// </summary>
        [HttpGet]
        public ActionResult Post(PageFactory.TypePost.DtoReq req)
        {
            var pageModel = new PageFactory.TypePost().Get(req);
            return View(pageModel);
        }
        
        #endregion
    }
}