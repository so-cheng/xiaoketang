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

namespace WebProject.Areas.Default.Controllers
{
    public class MainPageController : BaseLoginController
    {
        // GET: Default/MainPage
        public ActionResult Index()
        {
            if (UtilityStatic.ClientHelper.IsMobileRequest())
            {
                return Redirect("/Home/MobileView");
            }
            ViewBag.page = new ModelDbSite.site_page
            {
                title = "首页",
                name = "首页"
            };
            
            var list = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{new UserIdentityBag().user_sn}' and c_date>='{DateTime.Today.ToString("yyyy-MM-01")}' order by c_date");

            string[] dateArray = new string[list.Count];
            string[] amountArray = new string[list.Count];
            string[] newNumArray = new string[list.Count];
            string[] contactNumArray = new string[list.Count];
            string[] datouNumArray = new string[list.Count];
            int i = 0;
            foreach (var item in list)
            {
                dateArray[i] = item.c_date.ToDate().ToString("MM-dd");
                amountArray[i] = item.amount.ToNullableString().IsNullOrEmpty()?"0": item.amount.ToNullableString();
                newNumArray[i] = item.new_num.ToNullableString().IsNullOrEmpty() ? "0" : item.new_num.ToNullableString();
                contactNumArray[i] = item.contact_num.ToNullableString().IsNullOrEmpty() ? "0" : item.contact_num.ToNullableString();
                datouNumArray[i] = item.datou_num.ToNullableString().IsNullOrEmpty() ? "0" : item.datou_num.ToNullableString();
                i++;
            }
            ViewBag.dateArray = dateArray;
            ViewBag.amountArray = amountArray;
            ViewBag.newNumArray = newNumArray;
            ViewBag.contactNumArray = contactNumArray;
            ViewBag.datouNumArray = datouNumArray;
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetData(string date_range)
        {
            var info = new JsonResultAction();

            if (date_range.IsNullOrEmpty())
            {
                date_range = DateTime.Today.ToDate().ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToDate().ToString("yyyy-MM-dd");
            }
            var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(date_range);
            var p_jixiao_day = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $" zb_user_sn='{new UserIdentityBag().user_sn}' and c_date>='{dateRange.date_range_s}' and c_date<='{dateRange.date_range_e}'");

            info.data = new
            {
                amount= p_jixiao_day[0].IsNullOrEmpty()? "0":p_jixiao_day[0],
                new_num = p_jixiao_day[1].IsNullOrEmpty() ? "0" : p_jixiao_day[1],
                contact_num = p_jixiao_day[2].IsNullOrEmpty() ? "0" : p_jixiao_day[2],
                datou_num = p_jixiao_day[3].IsNullOrEmpty() ? "0" : p_jixiao_day[3],
            };
            return Json(info);
        }
    }
}