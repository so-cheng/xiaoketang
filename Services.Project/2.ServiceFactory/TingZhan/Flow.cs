using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.ModelDbs;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class TingZhanFlow
        {
            /// <summary>
            /// 根据运营获取各阶段厅战流水
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <param name="month">2025-10</param>
            /// <returns></returns>
            public List<TingZhanFlowModel> GetTingDataByYysn(string yy_user_sn, string month)
            {
                DateTime c_date = month.ToDateString("yyyy-MM-01").ToDate();//月第一天         
                DateTime lastDay = c_date.AddMonths(1).AddDays(-1); // 月最后一天
                var list = new List<TingZhanFlowModel>();
                foreach (var item in new UserInfo.Ting().GetBaseInfos(new UserInfo.Ting.TgBaseInfoFilter
                {
                    attachUserType = new UserInfo.Ting.TgBaseInfoFilter.AttachUserType
                    {
                        userType = UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                        UserSn = yy_user_sn
                    }
                }))
                {
                    var ting_sn = item.ting_sn;
                    decimal tenday_income_1 = DoMySql.FindField<ModelDb.doudata_round_ting>("SUM(fanTicket)", $"ting_sn = '{ting_sn}' and c_date BETWEEN '{c_date}' AND '{c_date.AddDays(9)}' and c_date in (select c_date from p_tingzhan where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and DATE_FORMAT(c_date, '%Y-%m') = '{month}')")[0].ToDecimal() / 10000;//第一阶段音浪
                    decimal tenday_income_2 = DoMySql.FindField<ModelDb.doudata_round_ting>("SUM(fanTicket)", $"ting_sn = '{ting_sn}' and c_date BETWEEN '{c_date.AddDays(10)}' AND '{c_date.AddDays(19)}' and c_date in (select c_date from p_tingzhan where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and DATE_FORMAT(c_date, '%Y-%m') = '{month}')")[0].ToDecimal() / 10000;//第二阶段音浪
                    decimal tenday_income_3 = DoMySql.FindField<ModelDb.doudata_round_ting>("SUM(fanTicket)", $"ting_sn = '{ting_sn}' and c_date BETWEEN '{c_date.AddDays(20)}' AND '{lastDay}' and c_date in (select c_date from p_tingzhan where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and DATE_FORMAT(c_date, '%Y-%m') = '{month}')")[0].ToDecimal() / 10000;//第三阶段音浪

                    list.Add(new TingZhanFlowModel
                    {
                        ting_sn = ting_sn,
                        tenday_income_1 = tenday_income_1,
                        tenday_income_2 = tenday_income_2,
                        tenday_income_3 = tenday_income_3
                    });
                }

                return list;
            }

            public class TingZhanFlowModel
            {
                /// <summary>
                /// 厅
                /// </summary>
                public string ting_sn { get; set; }
                /// <summary>
                /// 第一阶段音浪
                /// </summary>
                public decimal tenday_income_1 { get; set; }
                /// <summary>
                /// 第二阶段音浪
                /// </summary>
                public decimal tenday_income_2 { get; set; }
                /// <summary>
                /// 第三阶段音浪
                /// </summary>
                public decimal tenday_income_3 { get; set; }
            }
        }
    }
}