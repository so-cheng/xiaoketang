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
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas.UserTable.Controllers
{
    public class GradeController : BaseLoginController
    {
        // GET: UserTable/Grade
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Table()
        {
            return View();
        }
        public ActionResult LineChart(string c_date)
        {
            //默认为近半个月
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-16).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;

            //解析开始结束时间
            var date_s = c_date.Split('~');
            DateTime f_date = date_s[0].Trim().ToDate();
            DateTime t_date = date_s[1].Trim().ToDate();

            //补全时间段内日期
            List<string> dateArray = new List<string>();
            for (DateTime date = f_date; date <= t_date; date = date.AddDays(1))
            {
                dateArray.Add(date.ToString("yyyy-MM-dd"));
            }
            ViewBag.dateArray = dateArray;

            //获取时间段内每天的各等级分类人数
            var jiezouDetailList = DoMySql.FindListBySql<ModelDb.jiezou_detail>($"select data_time,step FROM jiezou_detail t1 where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and data_time between '{f_date}' and '{t_date}'");
            List<decimal> stepList = new List<decimal>();
            foreach (string date in dateArray)
            {
                decimal step = 0M;
                var jiezoudetail = jiezouDetailList.Find(n => n.data_time.ToDateString("yyyy-MM-dd").Equals(date));
                if (jiezoudetail != null)
                {
                    step = jiezoudetail.step.ToDecimal();
                }

                stepList.Add(step);
            }
            ViewBag.stepList = stepList;

            return View();
        }
    }
}