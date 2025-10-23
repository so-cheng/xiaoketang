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

namespace WebProject.Areas.QianYue.Controllers
{
    /// <summary>
    /// 签约数据
    /// </summary>
    public class DataController : BaseLoginController
    {
        /// <summary>
        /// 签约提报
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.QianYue.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.QianYue.Post().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 提报记录
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.QianYue.List.DtoReq();
            var pageModel = new PageFactory.QianYue.List().Get(req);
            return View(pageModel);
        }
    }
}