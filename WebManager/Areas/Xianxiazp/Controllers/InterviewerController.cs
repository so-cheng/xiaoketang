using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 面试
    /// </summary>
    public class InterviewerController : BaseLoginController
    {
        /// <summary>
        /// 到场数据
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

        /// <summary>
        /// 邀约面试时间段统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ZPStatistics(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.date = nowDate;
                date = nowDate;
            }
            else
            {
                ViewBag.date = date;
            }
            var result = new ServiceFactory.ResumeService().GetXianXiaZpinfo(date.ToDateTime());
            ViewBag.List = result;
            return View();
        }
    }
}