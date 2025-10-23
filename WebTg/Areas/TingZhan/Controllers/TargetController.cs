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
        /// 提报列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.tg_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);

            pageModel.listFilter.formItems.Find(item => item.name == "yy_user_sn").disabled = true;
            pageModel.listFilter.formItems.Find(item => item.name == "tg_user_sn").disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 目标提报
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.TingZhan.TargetPost.DtoReq();
            var pageModel = new PageFactory.TingZhan.TargetPost().Get(req);
            pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
            {
                returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
            };

            pageModel.formDisplay.formItems.Find(item => item.name == "yy_user_sn").isDisplay = false;
            pageModel.formDisplay.formItems.Find(item => item.name == "tg_user_sn").isDisplay = false;
            return View(pageModel);
        }

        /// <summary>
        /// 提报目标编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var req = new PageFactory.TingZhan.Edit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.Edit().Get(req);
            return View(pageModel);
        }
    }
}