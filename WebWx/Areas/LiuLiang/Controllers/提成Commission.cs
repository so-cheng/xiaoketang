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
        /// 绩效确认页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var p_liuliang_commission = DoMySql.FindEntity<ModelDb.p_liuliang_commission>($"yearmonth = '{DateTime.Today.AddMonths(-1).ToString("yyyy-MM")}' and wx_user_sn = '{new UserIdentityBag().user_sn}'", false);

            if (p_liuliang_commission.IsNullOrEmpty())
            {
                // 未确认
                return View("Total");
            }
            else
            {
                // 已确认
                var req = new PageFactory.LiuLiang.CommissionView.DtoReq();
                var pageModel = new PageFactory.LiuLiang.CommissionView().Get(req);
                return View(pageModel);
            }
        }

        /// <summary>
        /// 绩效确认提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Post()
        {
            var info = new JsonResultAction();
            try
            {
                var yearmonth = DateTime.Today.AddMonths(-1).ToString("yyyy-MM");
                // 校验
                var p_liuliang_commission = DoMySql.FindEntity<ModelDb.p_liuliang_commission>($"wx_user_sn = '{new UserIdentityBag().user_sn}' and yearmonth = '{yearmonth}'", false);
                if (!p_liuliang_commission.IsNullOrEmpty()) throw new WeicodeException("请勿重复提交");

                // 计算提成
                var num = DoMySql.FindField<ModelDb.p_liuliang_info>("COALESCE(SUM(num), 0)", $"wx_user_sn = '{new UserIdentityBag().user_sn}' and DATE_FORMAT(c_date, '%Y-%m') = '{yearmonth}'")[0].ToInt();
                var amount = DoMySql.FindEntity<ModelDb.p_liuliang_commission_rule>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and COALESCE(num_s, -999999) <= {num} and COALESCE(num_e, 999999) >= {num}", false).amount;
                if (amount.IsNullOrEmpty()) throw new WeicodeException("未设置提成规则");

                // 生成提成数据
                new ModelDb.p_liuliang_commission()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    wx_user_sn = new UserIdentityBag().user_sn,
                    wx_name = new UserIdentityBag().username,
                    yearmonth = yearmonth,
                    num = num,
                    status = ModelDb.p_liuliang_commission.status_enum.已确认.ToSByte(),
                    amount = amount * num
                }.Insert();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }
    }
}