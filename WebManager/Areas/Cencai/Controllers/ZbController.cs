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
using System;

namespace WebProject.Areas.Cencai.Controllers
{
    /// <summary>
    /// 成才主播
    /// </summary>
    public class ZbController : BaseLoginController
    {
        /// <summary>
        /// 主播成才目标list
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string yy_user_sn, string tg_user_sn, string c_date,int round=1)
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'").user_sn;
            }
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.round = round;
            return View();
        }

        /// <summary>
        /// 成才目标添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Cencai.CencaiPost.DtoReq req)
        {
            var pageModel = new PageFactory.Cencai.CencaiPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 成才目标编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(PageFactory.Cencai.CencaiEdit.DtoReq req)
        {
            var pageModel = new PageFactory.Cencai.CencaiEdit().Get(req);
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