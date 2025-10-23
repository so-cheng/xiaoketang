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

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 目标
    /// </summary>
    public class TargetController : BaseLoginController
    {
        /// <summary>
        /// 本期名单/已提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            pageModel.buttonGroup.buttonItems.Find(item => item.name == "post").disabled = true;
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }

        /// <summary>
        /// 厅战参加列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Join(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "amont > 0";
            return View(pageModel);
        }

        /// <summary>
        /// 厅战不参加列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnJoin(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "amont = 0";
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标未提报名单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnList(int id = 0)
        {
            var req = new PageFactory.TingZhan.UnTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UnTargetList().Get(req);
            pageModel.buttonGroup.buttonItems.Find(item => item.name == "post").disabled = true;
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }
    }
}