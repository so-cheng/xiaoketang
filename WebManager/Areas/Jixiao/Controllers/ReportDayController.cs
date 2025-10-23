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
    public class ReportDayController : BaseLoginController
    {

        /// <summary>
        /// 绩效列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(string c_date, string yy_user_sn,string tg_user_sn)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'").user_sn;
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }

            ViewBag.c_date = c_date;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.username = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.page = new ModelDbSite.site_page
            {
                title = "首页",
                name = "首页"
            };

            var jixao_today = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"c_date = '{DateTime.Today}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'");

            ViewBag.today_amount = (jixao_today[0].IsNullOrEmpty() ? 0 : jixao_today[0].ToInt());
            ViewBag.today_new_num = (jixao_today[1].IsNullOrEmpty() ? "0" : jixao_today[1]);
            ViewBag.today_contact_num = (jixao_today[2].IsNullOrEmpty() ? "0" : jixao_today[2]);
            ViewBag.today_datou_num = (jixao_today[3].IsNullOrEmpty() ? "0" : jixao_today[3]);

            var jixao_yesterday = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"c_date = '{DateTime.Today.AddDays(-1)}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'");

            ViewBag.yesterday_amount = (jixao_yesterday[0].IsNullOrEmpty() ? 0 : jixao_yesterday[0].ToInt());
            ViewBag.yesterday_new_num = (jixao_yesterday[1].IsNullOrEmpty() ? "0" : jixao_yesterday[1]);
            ViewBag.yesterday_contact_num = (jixao_yesterday[2].IsNullOrEmpty() ? "0" : jixao_yesterday[2]);
            ViewBag.yesterday_datou_num = (jixao_yesterday[3].IsNullOrEmpty() ? "0" : jixao_yesterday[3]);

            var jixao_month = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"c_date >= '{DateTime.Today.ToString("yyyy-MM-01")}' and  c_date < '{DateTime.Today.AddMonths(1).ToString("yyyy-MM-01")}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'");

            ViewBag.month_amount = (jixao_month[0].IsNullOrEmpty() ? 0 : jixao_month[0].ToInt());
            ViewBag.month_new_num = (jixao_month[1].IsNullOrEmpty() ? "0" : jixao_month[1]);
            ViewBag.month_contact_num = (jixao_month[2].IsNullOrEmpty() ? "0" : jixao_month[2]);
            ViewBag.month_datou_num = (jixao_month[3].IsNullOrEmpty() ? "0" : jixao_month[3]);

            return View();

        }

        public ActionResult Datas(string dateRange, string tg_user_sn,string yy_user_sn)
        {
            var Monday = DateTime.Today;
            while (Monday.DayOfWeek != DayOfWeek.Monday)
            {
                Monday = Monday.AddDays(-1);
            }
            if (dateRange.IsNullOrEmpty())
            {
                dateRange = Monday.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status!=9").user_sn;
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }

            ViewBag.c_date_s = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            ViewBag.c_date_e = dateRange.Substring(dateRange.IndexOf("~") + 2);
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.Monday = Monday;
            ViewBag.dateRange = dateRange;
            ViewBag.username = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'").username;
            return View();
        }


        public ActionResult ExportDatas(string dateRange, string tg_user_sn, string yy_user_sn)
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status!=9").user_sn;
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }

            string s_date = dateRange.Substring(0, dateRange.IndexOf("~") - 1);
            string e_date = dateRange.Substring(dateRange.IndexOf("~") + 2);


            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = "数据分析表格.xls";
            string SheetName = "数据分析";

            AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            AppLibrary.WriteExcel.Cells cells = sheet.Cells;
            //第一行表头
            cells.Add(1, 1, "进直播间人数");
            cells.Add(1, 2, "直播时长");
            cells.Add(1, 3, "总观看时长");
            cells.Add(1, 4, "曝光人数");
            cells.Add(1, 5, "曝光次数");
            cells.Add(1, 6, "打赏人数");
            cells.Add(1, 7, "打赏次数");
            cells.Add(1, 8, "新人打赏人数");
            cells.Add(1, 9, "音浪");
            cells.Add(1, 10, "互动人数");
            cells.Add(1, 11, "人均观看时长");

            int sort = 1;

            doc.Send();
            Response.Flush();
            Response.End();

            return Content("");
        }
    }
}