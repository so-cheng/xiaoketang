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
        public class JixiaoService
        {

            /// <summary>
            /// 获取厅管名下所有主播每日目标
            /// </summary>
            /// <returns></returns>
            public DataTable GetStandardData(string tg_user_sn,DateTime month)
            {
                string sql = $@"SELECT
	                                p_jixiao_standard_item.zb_user_sn,
	                                DATE_FORMAT(p_jixiao_standard.s_date, '%Y-%m-%d') as s_date,
	                                DATE_FORMAT(p_jixiao_standard.e_date, '%Y-%m-%d') as e_date,
	                                p_jixiao_standard_item.amount 
                                FROM
	                                `p_jixiao_standard`,
	                                p_jixiao_standard_item 
                                WHERE
	                                p_jixiao_standard.st_sn = p_jixiao_standard_item.st_sn 
	                                AND p_jixiao_standard.tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' 
	                                AND tg_user_sn = '{tg_user_sn}' 
	                                AND s_date <= '{month.ToString("yyyy-MM-01")}' AND e_date >= '{month.ToString("yyyy-MM-01").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}' 
                                ORDER BY
	                                p_jixiao_standard.s_date";
                UtilityStatic.TxtLog.Info(sql);
                DataTable dt = MysqlHelper.GetDataTable(sql);
                return dt;
            }

            /// <summary>
            /// 获取厅管名下所有主播当月每日数据
            /// </summary>
            /// <returns></returns>
            public DataTable GetJixiaoData(string tg_user_sn, DateTime month)
            {
                string sql = $@"SELECT
	                                zb_user_sn,
	                                DATE_FORMAT(c_date, '%Y-%m-%d') as c_date,
	                                amount,new_num,contact_num,num_2 
                                FROM
	                                p_jixiao_day 
                                WHERE
	                                tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' 
	                                and c_date>='{month.ToString("yyyy-MM-01")}'
	                                and c_date<='{month.ToString("yyyy-MM-01").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'
	                                AND tg_user_sn = '{tg_user_sn}' ORDER BY c_date";
                UtilityStatic.TxtLog.Info(sql);
                DataTable dt = MysqlHelper.GetDataTable(sql);
                return dt;
            }

            /// <summary>
            /// 获取厅管当月每日数据
            /// </summary>
            /// <returns></returns>
            public DataTable GetTgJixiaoData(string yy_user_sn, DateTime month)
            {
                string sql = $@"SELECT
	                                tg_user_sn,
	                                DATE_FORMAT(c_date, '%Y-%m-%d') as c_date,
	                                amount,new_num,contact_num,num_2 
                                FROM
	                                p_jixiao_day 
                                WHERE
	                                tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' 
	                                and c_date>='{month.ToString("yyyy-MM-01")}'
	                                and c_date<='{month.ToString("yyyy-MM-01").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'
	                                AND tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管,yy_user_sn)} ORDER BY c_date";
                UtilityStatic.TxtLog.Info(sql);
                DataTable dt = MysqlHelper.GetDataTable(sql);
                return dt;
            }

        }
        public class JixiaoTable : ModelDb.p_jixiao_day
        {
            public object user_sn { get; set; }
            public object amount_sum { get; set; }
            public object new_num_sum { get; set; }
            public object contact_num_sum { get; set; }
            public object datou_num_sum { get; set; }
            public object amount_1_sum { get; set; }
            public object num_2_sum { get; set; }
            public object amount_2_sum { get; set; }
            public object old_amount_sum { get; set; }
            public object new_num_avg { get; set; }
            public object session_count_avg { get; set; }
            public object zb_num { get; set; }
        }
    }    
}
