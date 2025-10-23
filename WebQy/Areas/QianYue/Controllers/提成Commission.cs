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

namespace WebProject.Areas.QianYue.Controllers
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
            var p_qianyue_commission = DoMySql.FindEntity<ModelDb.p_qianyue_commission>($"yearmonth = '{DateTime.Today.AddMonths(-1).ToString("yyyy-MM")}' and qy_user_sn = '{new UserIdentityBag().user_sn}'", false);

            if (p_qianyue_commission.IsNullOrEmpty())
            {
                // 未确认
                return View("Total");
            }
            else
            {
                // 已确认
                var req = new PageFactory.QianYue.CommissionView.DtoReq();
                var pageModel = new PageFactory.QianYue.CommissionView().Get(req);
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
                var p_qianyue_commission = DoMySql.FindEntity<ModelDb.p_qianyue_commission>($"qy_user_sn = '{new UserIdentityBag().user_sn}' and yearmonth = '{yearmonth}'", false);
                if (!p_qianyue_commission.IsNullOrEmpty()) throw new WeicodeException("请勿重复提交");

                // 计算提成
                var p_qianyue_info = DoMySql.FindField<ModelDb.p_qianyue_info>("COALESCE(SUM(wechat_num), 0),COALESCE(SUM(f_num), 0)", $"qy_user_sn = '{new UserIdentityBag().user_sn}' and DATE_FORMAT(c_date, '%Y-%m') = '{yearmonth}'");
                var wechat_num = p_qianyue_info[0].ToInt();// 添加微信
                var f_num = p_qianyue_info[1].ToInt();// 签约人数
                var qianyue_rate = wechat_num > 0 ? Math.Round(f_num.ToDecimal() / wechat_num.ToDecimal() * 100, 2) : 0;// 签约率
                var amount = DoMySql.FindEntity<ModelDb.p_qianyue_commission_rule>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and COALESCE(rate_s, -999999) <= {qianyue_rate} and COALESCE(rate_e, 999999) > {qianyue_rate}", false).amount;
                if (amount.IsNullOrEmpty()) throw new WeicodeException("未设置提成规则");

                // 生成提成数据
                new ModelDb.p_qianyue_commission()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    qy_user_sn = new UserIdentityBag().user_sn,
                    qy_name = new UserIdentityBag().username,
                    yearmonth = yearmonth,
                    wechat_num = wechat_num,
                    f_num = f_num,
                    qianyue_rate = qianyue_rate,
                    status = ModelDb.p_liuliang_commission.status_enum.已确认.ToSByte(),
                    amount = amount * f_num
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