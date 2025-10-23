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
        /// <summary>
        /// 补人数据分析
        /// </summary>
        public class JoinAnalysisService
        {

            #region 获取补人数据分析表格
            /// <summary>
            /// 获取厅管的表格
            /// </summary>
            /// <param name="tg_user_sn"></param>
            /// <returns></returns>
            public AnalysisTable GetTgTable(string tg_user_sn)
            {
                var p_join_needs = DoMySql.FindList<ModelDb.p_join_need>($"tg_user_sn = '{tg_user_sn}' NOT (status = '{ModelDb.p_join_need.status_enum.已取消.ToInt()}' and supplement_count = 0) and status in ({ModelDb.p_join_need.status_enum.已完成.ToInt()},{ModelDb.p_join_need.status_enum.已取消.ToInt()},{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()})");

                AnalysisTable table = new AnalysisTable(tg_user_sn);

                //如果p_join_need表中没有符合条件的补人申请，返回一条空的数据
                if (p_join_needs.FindAll(x => x.tg_user_sn == tg_user_sn).Count > 0)
                {
                    table.TotalZbCount = p_join_needs.Sum(x => x.zb_count).ToInt();
                    table.TotalNoSupplement = p_join_needs.Sum(x => x.zb_count - x.supplement_count).ToInt();
                    table.TotalNoQun = p_join_needs.Sum(x => x.supplement_count - x.inqun_count).ToInt();
                    table.TotalInQun = p_join_needs.Sum(x => x.inqun_count).ToInt();
                    table.TotalLeave = p_join_needs.Sum(x => x.quit_count).ToInt();

                    table.LeaveIn3 = GetLeaveInDaysByTg(tg_user_sn, 3);
                    table.LeaveIn7 = GetLeaveInDaysByTg(tg_user_sn, 7);
                    table.LeaveIn15 = GetLeaveInDaysByTg(tg_user_sn, 15);
                    table.LeaveIn30 = GetLeaveInDaysByTg(tg_user_sn, 30);
                    table.LeaveOver30 = GetLeaveInDaysByTg(tg_user_sn);

                    table.KeepIn3 = GetKeepInDaysByTg(tg_user_sn, 3);
                    table.KeepIn7 = GetKeepInDaysByTg(tg_user_sn, 7);
                    table.KeepIn15 = GetKeepInDaysByTg(tg_user_sn, 15);
                    table.KeepIn30 = GetKeepInDaysByTg(tg_user_sn, 30);
                    table.KeepOver30 = GetKeepInDaysByTg(tg_user_sn);
                }

                
                return table;
            }

            /// <summary>
            /// 获取运营的表格
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public List<AnalysisTable> GetYyTable(string yy_user_sn)
            {
                var tgs = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);

                var PJoinNeedAnalysis = new List<AnalysisTable>();

                var p_join_needs = DoMySql.FindList<ModelDb.p_join_need>($"yy_user_sn = '{yy_user_sn}' and NOT (status = '{ModelDb.p_join_need.status_enum.已取消.ToInt()}' and supplement_count = 0) and status in ({ModelDb.p_join_need.status_enum.已完成.ToInt()},{ModelDb.p_join_need.status_enum.已取消.ToInt()},{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()})");

                foreach (var tg in tgs)
                {
                    var tg_join_needs = p_join_needs.FindAll(x => x.tg_user_sn == tg.user_sn);

                    //如果p_join_need表中没有符合条件的补人申请，添加一条空的数据
                    if (tg_join_needs.Count == 0)
                    {
                        PJoinNeedAnalysis.Add(new AnalysisTable(tg.user_sn));
                        continue;
                    }

                    PJoinNeedAnalysis.Add(new AnalysisTable(tg.user_sn)
                    {
                        TotalZbCount = tg_join_needs.Sum(x => x.zb_count).ToInt(),
                        TotalNoSupplement = tg_join_needs.Sum(x => x.zb_count - x.supplement_count).ToInt(),
                        TotalNoQun = tg_join_needs.Sum(x => x.supplement_count - x.inqun_count).ToInt(),
                        TotalInQun = tg_join_needs.Sum(x => x.inqun_count).ToInt(),
                        TotalLeave = tg_join_needs.Sum(x => x.quit_count).ToInt(),

                        LeaveIn3 = GetLeaveInDaysByTg(tg.user_sn, 3),
                        LeaveIn7 = GetLeaveInDaysByTg(tg.user_sn, 7),
                        LeaveIn15 = GetLeaveInDaysByTg(tg.user_sn, 15),
                        LeaveIn30 = GetLeaveInDaysByTg(tg.user_sn, 30),
                        LeaveOver30 = GetLeaveInDaysByTg(tg.user_sn),

                        KeepIn3 = GetKeepInDaysByTg(tg.user_sn, 3),
                        KeepIn7 = GetKeepInDaysByTg(tg.user_sn, 7),
                        KeepIn15 = GetKeepInDaysByTg(tg.user_sn, 15),
                        KeepIn30 = GetKeepInDaysByTg(tg.user_sn, 30),
                        KeepOver30 = GetKeepInDaysByTg(tg.user_sn),
                    });
                }

                return PJoinNeedAnalysis;
            }


            /// <summary>
            /// 获取管理员的表格
            /// </summary>
            /// <returns></returns>
            public List<AnalysisTable> GetManagerTable()
            {
                var yys = DoMySql.FindList<ModelDb.user_base>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id= '{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and status != '{ModelDb.user_base.status_enum.逻辑删除.ToInt()}'");


                var PJoinNeedAnalysis = new List<AnalysisTable>();

                var p_join_needs = DoMySql.FindList<ModelDb.p_join_need>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and NOT (status = '{ModelDb.p_join_need.status_enum.已取消.ToInt()}' and supplement_count = 0) and status in ({ModelDb.p_join_need.status_enum.已完成.ToInt()},{ModelDb.p_join_need.status_enum.已取消.ToInt()},{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()})");

                foreach (var yy in yys)
                {
                    var yy_join_needs = p_join_needs.FindAll(x => x.yy_user_sn == yy.user_sn);
                    //如果p_join_need表中没有符合条件的补人申请，添加一条空的数据
                    if (yy_join_needs.Count == 0)
                    {
                        PJoinNeedAnalysis.Add(new AnalysisTable(yy.user_sn));
                        continue;
                    }

                    PJoinNeedAnalysis.Add(new AnalysisTable(yy.user_sn)
                    {
                        TotalZbCount = yy_join_needs.Sum(x => x.zb_count).ToInt(),
                        TotalNoSupplement = yy_join_needs.Sum(x => x.zb_count - x.supplement_count).ToInt(),
                        TotalNoQun = yy_join_needs.Sum(x => x.supplement_count - x.inqun_count).ToInt(),
                        TotalInQun = yy_join_needs.Sum(x => x.inqun_count).ToInt(),
                        TotalLeave = yy_join_needs.Sum(x => x.quit_count).ToInt(),

                        LeaveIn3 = GetLeaveInDaysByYy(yy.user_sn, 3),
                        LeaveIn7 = GetLeaveInDaysByYy(yy.user_sn, 7),
                        LeaveIn15 = GetLeaveInDaysByYy(yy.user_sn, 15),
                        LeaveIn30 = GetLeaveInDaysByYy(yy.user_sn, 30),
                        LeaveOver30 = GetLeaveInDaysByYy(yy.user_sn),

                        KeepIn3 = GetKeepInDaysByYy(yy.user_sn, 3),
                        KeepIn7 = GetKeepInDaysByYy(yy.user_sn, 7),
                        KeepIn15 = GetKeepInDaysByYy(yy.user_sn, 15),
                        KeepIn30 = GetKeepInDaysByYy(yy.user_sn, 30),
                        KeepOver30 = GetKeepInDaysByYy(yy.user_sn)

                    });
                }

                return PJoinNeedAnalysis;
            }

            #endregion



            #region 获取流失人数
            /// <summary>
            /// 获取某运营补人申请中，数天内流失的主播总人数
            /// </summary>
            /// <param name="yy_user_sn">所属运营</param>
            /// <param name="days">在days天内流失的总人数,days=0代表30天后流失的总人数</param>
            /// <returns></returns>
            private int GetLeaveInDaysByYy(string yy_user_sn, int days = 0)
            {

                string sql = $"select * from p_join_need inner join user_info_zb on p_join_need.id=user_info_zb.tg_need_id where yy_user_sn = '{yy_user_sn}' and (status={ModelDb.p_join_need.status_enum.已完成.ToInt()} or status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()} or status={ModelDb.p_join_need.status_enum.已取消.ToInt()})";


                if (days > 0)
                {
                    sql += $" and no_share_time <= (user_info_zb.qun_time + INTERVAL {days} DAY) OR leave_date <= (user_info_zb.qun_time + INTERVAL {days} DAY)";
                }
                else
                {
                    //获取30天后流失人数的特殊计算
                    sql += $" and no_share_time > (user_info_zb.qun_time + INTERVAL 30 DAY)  OR leave_date > (user_info_zb.qun_time + INTERVAL 30 DAY)";
                }

                var user_info_zbs = DoMySql.FindListBySql<ModelDb.p_join_need>(sql);
                return user_info_zbs.Count;
            }

            /// <summary>
            /// 获取某厅管补人申请中，数天内流失的主播总人数
            /// </summary>
            /// <param name="tg_user_sn">所属厅管</param>
            /// <param name="days">在days天内流失的总人数,days=0代表30天后流失的总人数</param>
            /// <returns></returns>
            private int GetLeaveInDaysByTg(string tg_user_sn, int days = 0)
            {

                string sql = $"select * from p_join_need inner join user_info_zb on p_join_need.id=user_info_zb.tg_need_id where tg_user_sn = '{tg_user_sn}' and (status={ModelDb.p_join_need.status_enum.已完成.ToInt()} or status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()} or status={ModelDb.p_join_need.status_enum.已取消.ToInt()})";


                if (days > 0)
                {
                    sql += $" and no_share_time <= (user_info_zb.qun_time + INTERVAL {days} DAY) OR leave_date <= (user_info_zb.qun_time + INTERVAL {days} DAY)";
                }
                else
                {
                    //获取30天后流失人数的特殊计算
                    sql += $" and no_share_time > (user_info_zb.qun_time + INTERVAL 30 DAY)  OR leave_date > (user_info_zb.qun_time + INTERVAL 30 DAY)";
                }

                var user_info_zbs = DoMySql.FindListBySql<ModelDb.p_join_need>(sql);
                return user_info_zbs.Count;
            }
            #endregion

            #region 获取留存人数

            /// <summary>
            /// 获取某条厅管补人申请中，数天内留存的主播总人数
            /// </summary>
            /// <param name="yy_user_sn">所属厅管</param>
            /// <param name="days">在days天内留存的总人数,days=0代表30天后留存的总人数</param>
            /// <returns></returns>
            private int GetKeepInDaysByYy(string yy_user_sn, int days = 0)
            {

                string sql = $"select * from p_join_need inner join user_info_zb on p_join_need.id=user_info_zb.tg_need_id where yy_user_sn = '{yy_user_sn}' and (status={ModelDb.p_join_need.status_enum.已完成.ToInt()} or status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()} or status={ModelDb.p_join_need.status_enum.已取消.ToInt()})";


                if (days > 0)
                {
                    sql += $" AND (no_share_time > (user_info_zb.qun_time + INTERVAL {days} DAY) OR no_share_time is null) AND (leave_date > (user_info_zb.qun_time + INTERVAL {days} DAY) OR leave_date is null)";
                }
                else
                {
                    //获取30天后留存人数的特殊计算
                    sql += $" AND no_share_time is null AND leave_date is null";
                }

                var user_info_zbs = DoMySql.FindListBySql<ModelDb.p_join_need>(sql);
                return user_info_zbs.Count;
            }

            /// <summary>
            /// 获取某条厅管补人申请中，数天内留存的主播总人数
            /// </summary>
            /// <param name="tg_user_sn">所属厅管</param>
            /// <param name="days">在days天内留存的总人数,days=0代表30天后留存的总人数</param>
            /// <returns></returns>
            private int GetKeepInDaysByTg(string tg_user_sn, int days = 0)
            {

                string sql = $"select * from p_join_need inner join user_info_zb on p_join_need.id=user_info_zb.tg_need_id where tg_user_sn = '{tg_user_sn}' and (status={ModelDb.p_join_need.status_enum.已完成.ToInt()} or status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()} or status={ModelDb.p_join_need.status_enum.已取消.ToInt()})";


                if (days > 0)
                {
                    sql += $" AND (no_share_time > (user_info_zb.qun_time + INTERVAL {days} DAY) OR no_share_time is null) AND (leave_date > (user_info_zb.qun_time + INTERVAL {days} DAY) OR leave_date is null)";
                }
                else
                {
                    //获取30天后留存人数的特殊计算
                    sql += $" AND no_share_time is null AND leave_date is null";
                }

                var user_info_zbs = DoMySql.FindListBySql<ModelDb.p_join_need>(sql);
                return user_info_zbs.Count;
            }
            #endregion

            /// <summary>
            /// 补人数据分析表
            /// </summary>
            public class AnalysisTable
            {
                /// <summary>
                /// 所属用户sn
                /// </summary>
                public string user_sn { get; set; }

                /// <summary>
                /// 申请主播总数
                /// </summary>
                public int TotalZbCount { get; set; } = 0;

                /// <summary>
                /// 未分配总数
                /// </summary>
                public int TotalNoSupplement { get; set; } = 0;

                /// <summary>
                /// 待拉群总数
                /// </summary>
                public int TotalNoQun { get; set; } = 0;

                /// <summary>
                /// 已分配总数
                /// </summary>
                public int TotalInQun { get; set; } = 0;

                /// <summary>
                /// 流失(对接前)总数
                /// </summary>
                public int TotalLeave { get; set; } = 0;

                /// <summary>
                /// 3天内流失
                /// </summary>
                public int LeaveIn3 { get; set; } = 0;

                /// <summary>
                /// 3天内流失率
                /// </summary>
                public int KeepIn3 { get; set; } = 0;

                /// <summary>
                /// 7天内流失
                /// </summary>
                public int LeaveIn7 { get; set; } = 0;

                /// <summary>
                /// 7天内流失率
                /// </summary>
                public int KeepIn7 { get; set; } = 0;

                /// <summary>
                /// 15天内流失
                /// </summary>
                public int LeaveIn15 { get; set; } = 0;

                /// <summary>
                /// 15天内流失率
                /// </summary>
                public int KeepIn15 { get; set; } = 0;

                /// <summary>
                /// 30天内流失
                /// </summary>
                public int LeaveIn30 { get; set; } = 0;

                /// <summary>
                /// 30天内流失率
                /// </summary>
                public int KeepIn30 { get; set; } = 0;

                /// <summary>
                /// 30天后流失
                /// </summary>
                public int LeaveOver30 { get; set; } = 0;

                /// <summary>
                /// 30天后流失率
                /// </summary>
                public int KeepOver30 { get; set; } = 0;

                public AnalysisTable(string user_sn)
                {
                    this.user_sn = user_sn;
                }

            }

        }

    }
}
