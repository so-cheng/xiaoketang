using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Support.Controllers
{
    public class BindController : BaseLoginController
    {
        #region 绑定微信二维码
        public ActionResult Page()
        {
            return View();
        }
        #endregion
    }
}