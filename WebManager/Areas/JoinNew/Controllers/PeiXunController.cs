using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 培训数据
    /// </summary>
    public class PeiXunController : BaseLoginController
    {
        /// <summary>
        /// 培训数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.JoinNew.PeixunList.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.PeixunList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        /// <summary>
        /// 培训数据详情（可编辑）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Post(PageFactory.JoinNew.PeixunPost.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.PeixunPost().Get(req);
            return View(pageModel);
        }

        public ActionResult Index(string month, string mx_sn = "", string dateRange = "")
        {
            if (month.IsNullOrEmpty())
            {
                month = DateTime.Today.ToString("yyyy-MM");
            }
            var date = new UtilityStatic.CommonHelper.DateRange();
            if (!dateRange.IsNullOrEmpty() && dateRange.Contains("~"))
            {
                date = UtilityStatic.CommonHelper.DateRangeFormat(dateRange, 0);
            }
            ViewBag.month = month;
            ViewBag.mx_sn = mx_sn;

            ViewBag.isRange = !dateRange.IsNullOrEmpty();

            ViewBag.c_date_s = date.date_range_s;
            ViewBag.c_date_e = date.date_range_e;
            ViewBag.dateRange = dateRange;
            return View();
        }

        public double getRate(double? A, double? B)
        {
            if (B == null || B == 0)
            {
                return 0;
            }
            else
            {
                return Math.Round((A * 100 / B).ToDouble(), 0);
            }
        }
    }
}