using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
namespace WebProject.Areas.PUserBasic.Controllers
{
    /// <summary>
    /// 主播招新
    /// </summary>
    public class NewcomerController : BaseLoginController
    {

        #region 萌新数据
        
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult PeixunList(PageFactory.Xianxiabiao.PeixunList.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        /// <summary>
        /// 培训数据提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Xianxiabiao.PeixunPost.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}