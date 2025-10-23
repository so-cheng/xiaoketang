using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.PBasic.Controllers
{
    public class ConfigController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCookie()
        {
            var result = new JsonResultAction();
            try
            {
                result.data = UtilityStatic.ConfigHelper.GetConfigString("Cookie");
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }
    }
}