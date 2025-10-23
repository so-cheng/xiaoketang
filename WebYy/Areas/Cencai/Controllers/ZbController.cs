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

namespace WebProject.Areas.Cencai.Controllers
{
    /// <summary>
    /// 主播成才
    /// </summary>
    public class ZbController : BaseLoginController
    {
        /// <summary>
        /// 主播成才目标list
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string tg_user_sn, string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.tg_user_sn = tg_user_sn;
            return View();
        }

        /// <summary>
        /// 成才目标添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Cencai.CencaiPost.DtoReq req)
        {
            req.yy_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.Cencai.CencaiPost().Get(req);
            pageModel.formDisplay.formItems.Find(p => p.name == "yy_user_sn").isDisplay = false;
            return View(pageModel);
        }

        /// <summary>
        /// 成才目标编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            var req = new PageFactory.Cencai.CencaiEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Cencai.CencaiEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新轮次成才目标编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult NewEdit(int id = 0, int round = 1)
        {
            var req = new PageFactory.Cencai.CencaiNewRoundEdit.DtoReq();
            req.id = id;
            req.round = round;
            var pageModel = new PageFactory.Cencai.CencaiNewRoundEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 成才目标删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ReMove(int id)
        {
            var result = new JsonResultAction();
            try
            {
                new ModelDb.p_cencai
                {

                }.Delete($"id = '{id}'");
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        /// <summary>
        /// 配置显示字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public ActionResult Setting(PageFactory.Cencai.SettingPost.DtoReq req)
        {
            req.user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.Cencai.SettingPost().Get(req);
            return View(pageModel);
        }
    }
}