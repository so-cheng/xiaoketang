using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// 用于统计所有团队的补人情况
            /// </summary>
            public class v_p_join_new : Entity
            {
                public string yy_name { get; set; }
                /// <summary>
                /// 补人申请日期
                /// </summary>
                public string apply_date { get; set; }
                /// <summary>
                /// 总申请人数
                /// </summary>
                public string zb_sum { get; set; }
                /// <summary>
                /// 未分配
                /// </summary>
                public string unsupplement_sum { get; set; }
                /// <summary>
                /// 待拉群
                /// </summary>
                public string uninqun_sum { get; set; }
                /// <summary>
                /// 已拉群
                /// </summary>
                public string inqun_sum { get; set; }
                /// <summary>
                /// 分配前流失
                /// </summary>
                public string b_allocation_loss { get; set; }
                /// <summary>
                /// 分配后培训前流失
                /// </summary>
                public string b_traning_loss { get; set; }
                /// <summary>
                /// 分配后培训后流失总数
                /// </summary>
                public string a_traning_loss { get; set; }
                /// <summary>
                /// 申请数男
                /// </summary>
                public string male_zb_sum { get; set; }
                /// <summary>
                /// 申请数女
                /// </summary>
                public string female_zb_sum { get; set; }
                /// <summary>
                /// 培训数男
                /// </summary>
                public string male_training_sum { get; set; }
                /// <summary>
                /// 培训数女
                /// </summary>
                public string female_training_sum { get; set; }
                /// <summary>
                /// 已分配数男
                /// </summary>
                public string male_supplement_sum { get; set; }
                /// <summary>
                /// 已分配数女
                /// </summary>
                public string female_supplement_sum { get; set; }
                /// <summary>
                /// 已拉群数男
                /// </summary>
                public string male_inqun_sum { get; set; }
                /// <summary>
                /// 已拉群数女
                /// </summary>
                public string female_inqun_sum { get; set; }
            }

            /// <summary>
            /// 获取萌新月总数据
            /// </summary>
            /// <param name="month">示例：2025-08</param>
            /// <returns></returns>
            public List<MxRank> GetMxPeiXun(string month)
            {
                var mx_rank_list = new List<MxRank>();
                foreach (var mx in DoMySql.FindList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}"))
                {
                    var p_mengxin = DoMySql.FindList<ModelDb.p_mengxin>($"user_sn = '{mx.user_sn}' and date >= '{month.ToDate().ToString("yyyy-MM-01").ToDate().AddDays(-3).ToString("yyyy-MM-dd")}' and date < '{month.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-3).ToString("yyyy-MM-dd")}'");

                    // 拉群人数
                    var qun = p_mengxin.Sum(x => x.group_num);
                    if (qun > 0)
                    {
                        // 推出人数
                        var tui = p_mengxin.Sum(x => x.first_employ + x.resit_num);
                        // 流失人数
                        var quit = p_mengxin.Sum(x => x.ignore_num).ToInt() + p_mengxin.Sum(x => x.no_exam_num).ToInt() + p_mengxin.Sum(x => x.group_num - x.before_class_2 + x.leave_group_3).ToInt();
                        // 总未分配
                        var unallocate = p_mengxin.Sum(x => x.no_job_num);
                        mx_rank_list.Add(new MxRank()
                        {
                            mx_name = mx.username,
                            qun = qun,
                            tui = tui,
                            // 推出率 = 推出人数 / 拉群人数
                            tui_rate = getRate(tui, qun),
                            quit = quit,
                            // 流失率 = 流失人数 / 拉群人数
                            quit_rate = getRate(quit, qun),
                            unallocate = unallocate,
                            // 未分配率 = 总未分配 / 拉群人数
                            unallocate_rate = getRate(unallocate, qun)
                        });
                    }
                }

                return mx_rank_list;
            }
            public class MxRank : Entity
            {
                /// <summary>
                /// 萌新
                /// </summary>
                public string mx_name { get; set; }
                /// <summary>
                /// 拉群人数
                /// </summary>
                public int? qun { get; set; }
                /// <summary>
                /// 推出人数
                /// </summary>
                public int? tui { get; set; }
                /// <summary>
                /// 推出率
                /// </summary>
                public decimal tui_rate { get; set; }
                /// <summary>
                /// 流失人数
                /// </summary>
                public int? quit { get; set; }
                /// <summary>
                /// 流失率
                /// </summary>
                public decimal quit_rate { get; set; }
                /// <summary>
                /// 总未分配
                /// </summary>
                public int? unallocate { get; set; }
                /// <summary>
                /// 未分配率
                /// </summary>
                public decimal unallocate_rate { get; set; }
            }
            public decimal getRate(decimal? A, decimal? B)
            {
                if (B == null || B == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round((A * 100 / B).ToDecimal(), 2);
                }
            }
        }
    }
}
