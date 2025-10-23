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
            req.yy_user_sn = new UserIdentityBag().user_sn;
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);

            pageModel.listFilter.formItems.Find(item => item.name == "yy_user_sn").disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标未提报名单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnList(int id = 0)
        {
            var req = new PageFactory.TingZhan.UnTargetList.DtoReq();
            req.yy_user_sn = new UserIdentityBag().user_sn;
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UnTargetList().Get(req);

            pageModel.listFilter.formItems.Find(item => item.name == "yy_user_sn").disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标新增页面
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
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标编辑页面
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