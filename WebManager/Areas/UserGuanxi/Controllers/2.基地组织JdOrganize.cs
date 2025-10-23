using Services.Project;
using System.Collections.Generic;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserGuanxi.Controllers
{
    public class JdOrganizeController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.UserGuanxi.JdOrganizeList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.JdOrganizeList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 设置组织部门
        /// </summary>
        /// <param name="user_sn"></param>
        /// <returns></returns>
        public ActionResult SetOrganize(string user_sn)
        {
            var req = new PageFactory.UserGuanxi.SetOrganize.DtoReq();
            req.user_sn = user_sn;
            var pageModel = new PageFactory.UserGuanxi.SetOrganize().Get(req);
            return View(pageModel);
        }
    }
}