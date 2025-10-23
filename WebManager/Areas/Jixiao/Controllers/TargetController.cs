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
    /// <summary>
    /// 目标管理
    /// </summary>
    public class TargetController : BaseLoginController
    {




        #region 日目标

        /// <summary>
        /// 主播日目标列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DayTarget(string date, string yy_user_sn, string tg_user_sn, string zb_user_sn, string type = "amount")
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (zb_user_sn == "undefined")
            {
                zb_user_sn = "";
            }
            if (tg_user_sn == "undefined")
            {
                tg_user_sn = "";
            }
            if (yy_user_sn == "undefined")
            {
                yy_user_sn = "";
            }
            if (type == "undefined")
            {
                type = "";
            }
            ViewBag.date = date;
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.type = type;
            return View();
        }


        /// <summary>
        /// 厅管日目标列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TGDayTarget(string date, string tg_user_sn,string yy_user_sn, string type = "amount")
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (yy_user_sn == "undefined")
            {
                yy_user_sn = "";
            }
            if (tg_user_sn == "undefined")
            {
                tg_user_sn = "";
            }
            if (type == "undefined")
            {
                type = "";
            }
            ViewBag.date = date;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.type = type;
            return View();
        }

        #endregion
        public ActionResult List(PageFactory.ZbTargetList.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetList().Get(req);
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    field_paras = "id",
                    url = "ZbEdit"
                },
                text = "编辑",
            });
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
        /// <summary>
        /// 修改绩效目标
        /// </summary>
        /// <returns></returns>
        public ActionResult ZbEdit(PageFactory.ZbTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 绩效目标列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TgList(PageFactory.TgTargetList.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetList().Get(req);
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    field_paras = "id",
                    url = "TgEdit"
                },
                text = "编辑",
            });
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }

        public ActionResult TgEdit(PageFactory.TgTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetEdit().Get(req);
            return View(pageModel);
        }

        public ActionResult Index()
        {
            var p_jixiao_target = DoMySql.FindField<ModelDb.p_jixiao_target>("sum(amount),sum(new_num)", $"yearmonth = '{DateTime.Today.ToString("yyyy-MM")}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'");

            ViewBag.amount = (p_jixiao_target[0].IsNullOrEmpty() ? 0 : p_jixiao_target[0].ToInt());
            ViewBag.new_num = (p_jixiao_target[1].IsNullOrEmpty() ? "0" : p_jixiao_target[1]);

            var jixao_month = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount),sum(new_num),sum(contact_num),sum(datou_num)", $"c_date >= '{DateTime.Today.ToString("yyyy-MM-01")}' and  c_date < '{DateTime.Today.AddMonths(1).ToString("yyyy-MM-01")}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'");

            ViewBag.month_amount = (jixao_month[0].IsNullOrEmpty() ? 0 : jixao_month[0].ToInt());
            ViewBag.month_new_num = (jixao_month[1].IsNullOrEmpty() ? "0" : jixao_month[1]);

            int new_num = (p_jixiao_target[1].IsNullOrEmpty() ? 0 : p_jixiao_target[1].ToInt());
            int month_new_num = (jixao_month[1].IsNullOrEmpty() ? 0 : jixao_month[1].ToInt());
            if (new_num == 0)
            {
                ViewBag.new_num_per = "0%";
            }
            else
            {
                ViewBag.new_num_per = (100 * month_new_num.ToDouble() / new_num).ToFixed(2) + "%";
            }
            int amount = (p_jixiao_target[0].IsNullOrEmpty() ? 0 : p_jixiao_target[0].ToInt());
            int month_amount = (jixao_month[0].IsNullOrEmpty() ? 0 : jixao_month[0].ToInt());
            if (amount == 0)
            {
                ViewBag.amount_per = "0%";
            }
            else
            {
                ViewBag.amount_per = (100 * month_amount.ToDouble() / amount).ToFixed(2) + "%";
            }

            var list = DoMySql.FindListBySql<p_jixiao_day>($"select c_date,sum(amount) as sum_amount,sum(new_num) as sum_new_num,sum(contact_num) as sum_contact_num,sum(datou_num) as sum_datou_num " +
        $"from p_jixiao_day " +
        $"where c_date>='{DateTime.Today.ToString("yyyy-MM-01")}' and c_date<'{DateTime.Today.AddMonths(1).ToString("yyyy-MM-01")}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' " +
        $"group by c_date order by c_date");

            string[] dateArray = new string[list.Count];
            string[] amountArray = new string[list.Count];
            string[] newNumArray = new string[list.Count];
            string[] contactNumArray = new string[list.Count];
            string[] datouNumArray = new string[list.Count];
            int i = 0;
            foreach (var item in list)
            {
                dateArray[i] = item.c_date.ToDate().ToString("MM-dd");
                amountArray[i] = item.sum_amount.ToString();
                newNumArray[i] = item.sum_new_num.ToString();
                contactNumArray[i] = item.sum_contact_num.ToString();
                datouNumArray[i] = item.sum_datou_num.ToString();
                i++;
            }
            ViewBag.dateArray = dateArray;
            ViewBag.amountArray = amountArray;
            ViewBag.newNumArray = newNumArray;
            ViewBag.contactNumArray = contactNumArray;
            ViewBag.datouNumArray = datouNumArray;
            return View();
        }

        public class p_jixiao_day : ModelDb.p_jixiao_day
        {
            public string sum_amount { get; set; }
            public string sum_new_num { get; set; }
            public string sum_contact_num { get; set; }
            public string sum_datou_num { get; set; }
        }
    }
}