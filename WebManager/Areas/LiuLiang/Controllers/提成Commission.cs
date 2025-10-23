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
    /// 提成
    /// </summary>
    public class CommissionController : BaseLoginController
    {
        /// <summary>
        /// 绩效确认情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Situation()
        {
            var req = new PageFactory.LiuLiang.SituationView.DtoReq();
            var pageModel = new PageFactory.LiuLiang.SituationView().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 已确认名单
        /// </summary>
        /// <returns></returns>
        public ActionResult Confirmed()
        {
            var req = new PageFactory.LiuLiang.Confirmed.DtoReq();
            var pageModel = new PageFactory.LiuLiang.Confirmed().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 未确认名单
        /// </summary>
        /// <returns></returns>
        public ActionResult UnConfirm()
        {
            var req = new PageFactory.LiuLiang.UnConfirm.DtoReq();
            var pageModel = new PageFactory.LiuLiang.UnConfirm().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 提成汇总
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="organize_id"></param>
        /// <returns></returns>
        public ActionResult Total(string c_date, int organize_id = 0)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddMonths(-1).ToString("yyyy-MM");
            }
            if (organize_id == 0)
            {
                organize_id = DoMySql.FindEntity<ModelDb.sys_organize>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and parent_id = (select id from sys_organize where user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and name = '线上') and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by sort,id").id;
            }

            ViewBag.c_date = c_date;
            ViewBag.organize_id = organize_id;

            return View();
        }
    }
}