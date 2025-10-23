using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using Modular.Functions;

using DataBase.Project;
using Services.Project;
using DataBase.Models;

namespace WebProject.Areas.User.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: User/Default
        public ActionResult Index()
        {
            string json = new ModularMethodsFunc.DataQuery().Get(0, "sys_modular_menu");

            ModelDb.test test = new ModelDb.test();
            test.user_type = ModelDb.test.user_type_enum.代理商;
            test.user_sn = "aa";
            test.Insert();

            ViewBag.json = json;
            return View();
        }

        /// <summary>
        ///  普通商品创建订单
        /// </summary>
        public JsonResult CreateUser(UserBasicCreateProject.DtoReq req)
        {
            JsonResultInfo result = new JsonResultInfo();

            var res = new UserBasicCreateProject.DtoRes();
            var log = new UserBasicCreateProject.DtoLog();
            //执行PageController请求处理
            var service = new UserBasicCreateProject.Service<UserBasicCreateProject.DtoReq, UserBasicCreateProject.DtoRes, UserBasicCreateProject.DtoLog>();
            result.data = service.Execute(req, res, log);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}