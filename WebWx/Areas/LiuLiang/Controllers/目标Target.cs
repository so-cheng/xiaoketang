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

namespace WebProject.Areas.LiuLiang.Controllers
{
    /// <summary>
    /// 流量月度目标
    /// </summary>
    public class TargetController : BaseLoginController
    {
        /// <summary>
        /// 目标列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.LiuLiang.TargetList.DtoReq();
            var pageModel = new PageFactory.LiuLiang.TargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 目标新增/编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.LiuLiang.TargetPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.LiuLiang.TargetPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 本月完成进度
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishSchedule()
        {
            return View();
        }
    }
}