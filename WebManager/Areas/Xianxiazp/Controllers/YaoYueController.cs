using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 邀约
    /// </summary>
    public class YaoYueController : BaseLoginController
    {
        /// <summary>
        /// 邀约明细
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Xianxiazp.YaoYueList.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.YaoYueList().Get(req);
            pageModel.buttonGroup.disabled = true;
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }

        /// <summary>
        /// 邀约数据
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="organize_id"></param>
        /// <returns></returns>
        public ActionResult Statistical(string c_date, int organize_id = 0)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (organize_id == 0)
            {
                organize_id = DoMySql.FindEntity<ModelDb.sys_organize>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and parent_id = (select id from sys_organize where user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and name = '线下') and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by sort,id").id;
            }

            ViewBag.c_date = c_date;
            ViewBag.organize_id = organize_id;

            return View();
        }
    }
}