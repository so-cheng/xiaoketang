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

namespace WebProject.Areas.Jixiao.Controllers
{
    public class JiezouController : BaseLoginController
    {
        #region 节奏
        public ActionResult List(PageFactory.JiezouList.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouList().Get(req);
            //pageModel.listDisplay.listData.attachFilterSql = $"yy_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.JiezouPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouPost().Get(req);
            return View(pageModel);
        }

        public ActionResult Total()
        {
            return View();
        }

        public ActionResult TgDetail(string step="0.5",string jiezou_sn="")
        {
            ViewBag.step = step.ToDecimal();
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.yy_user_sn = DoMySql.FindEntity<ModelDb.jiezou>($"jiezou_sn='{jiezou_sn}'").yy_user_sn;
            return View();
        }
        public ActionResult TgMissing(string jiezou_sn)
        {
            ViewBag.jiezou_sn = jiezou_sn;
            return View();
        }

        public ActionResult Item(string yy_user_sn,string jiezou_sn)
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id = '{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and status = 0").user_sn;
            }
            if (jiezou_sn.IsNullOrEmpty())
            {
                jiezou_sn = DoMySql.FindEntity<ModelDb.jiezou>($"yy_user_sn='{yy_user_sn}' order by id desc",false).jiezou_sn;
            }
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            return View();
        }
        #endregion

        #region 问题梳理
        /// <summary>
        /// 问题明细表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QList(PageFactory.JiezouQList.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
            return View(pageModel);
        }

        /// <summary>
        /// 新增问题提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QAdd(PageFactory.JiezouQAdd.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQAdd().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 修改问题提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QPost(PageFactory.JiezouQPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 解决方案提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult SPost(PageFactory.JiezouSPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouSPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 问题梳理统计表格
        /// </summary>
        /// <param name="jiezou_sn"></param>
        /// <returns></returns>
        public ActionResult QNSIndex(string keyword="")
        {
            ViewBag.keyword = keyword;
            return View();
        }
        #endregion
    }
}