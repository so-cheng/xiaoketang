using Services.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;

namespace TaskProject
{
    /// <summary>
    /// 类名(固定命名，不能修改)
    /// </summary>
    public partial class ProjectClass
    {
        /// <summary>
        /// 生成每天工作待办明细
        /// </summary>
        /// <returns></returns>
        public string SetWorkTodoDay()
        {
            string c_date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            foreach (var item in DoMySql.FindList<ModelDb.p_work_todo_rule>($"s_time <= '{c_date}' and e_time >= '{c_date}'"))
            {
                if (!item.c_rule.Equals("每天"))
                {
                    if (!item.c_rule.Contains(":")) continue;
                    var c_rule = item.c_rule.Substring(0, item.c_rule.IndexOf(":"));
                    switch (c_rule)
                    {
                        case "每周":
                            CultureInfo cultureInfo = new CultureInfo("zh-CN");
                            string week = c_date.ToDate().ToString("dddd", cultureInfo);

                            if (!item.c_rule.Contains(week))
                            {
                                continue;
                            }
                            break;
                        case "每月":
                            var day = $",{c_date.ToDate().Day},";

                            var days = item.c_rule.Substring(item.c_rule.IndexOf(":") + 1);
                            if (!$",{days},".Contains(day))
                            {
                                // 判断月最后一天
                                if (!item.c_rule.Contains("最后一天"))
                                {
                                    continue;
                                }
                                else
                                {
                                    // 获取月最后一天
                                    var last_day = DateTime.Now.ToString("yyyy-MM-01").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                    if (c_date != last_day)
                                    {
                                        continue;
                                    }
                                }
                            }
                            break;
                    }
                }

                //所属中台发起的待办规则
                if (!string.IsNullOrEmpty(item.zt_sn))
                {
                    foreach (var yy in new ServiceFactory.UserInfo.Zt().ZtGetNextYy(item.zt_sn))
                    {
                        new ModelDb.p_work_todo
                        {
                            zt_sn = item.zt_sn,
                            yy_sn = yy.user_sn,
                            tenant_id = item.tenant_id,
                            create_time = c_date.ToDate(),
                            rule_id = item.id,
                            content = item.content,
                            sort = item.sort
                        }.Insert();
                    }
                }
                //所属运营发起的待办规则
                if (!string.IsNullOrEmpty(item.yy_sn))
                {
                    foreach (var tg in new ServiceFactory.UserInfo.Tg().TgGetNextTg(item.yy_sn))
                    {
                        new ModelDb.p_work_todo
                        {

                            yy_sn = tg.user_sn,
                            tg_sn = tg.user_sn,
                            tenant_id = item.tenant_id,
                            create_time = c_date.ToDate(),
                            rule_id = item.id,
                            content = item.content,
                            sort = item.sort
                        }.Insert();
                    }
                }
            }
            return "生成每天工作待办明细";
        }

        /// <summary>
        /// 补人申请单超过7天未完成设置自动完成
        /// </summary>
        /// <returns></returns>
        public string SetJoinApplyFinish()
        {
            foreach (var p_join_apply in DoMySql.FindList<p_join_apply>($"status = {p_join_apply.status_enum.等待外宣补人.ToSByte()} and exists (select 1 from p_join_apply_log where apply_sn = p_join_apply.apply_sn and c_type = {p_join_apply_log.c_type_enum.公会审批.ToSByte()} and create_time < '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}')"))
            //foreach (var p_join_apply in DoMySql.FindList<p_join_apply>($"status = {p_join_apply.status_enum.等待外宣补人.ToSByte()} and create_time < '{DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}'"))
            {
                // 更新申请档位明细已完成
                new p_join_apply_item()
                {
                    status = p_join_apply_item.status_enum.已完成.ToSByte()
                }.Update($"apply_sn = '{p_join_apply.apply_sn}'");

                // 更新补人申请表已完成
                p_join_apply.status = p_join_apply.status_enum.已完成.ToSByte();
                p_join_apply.Update();

                // 添加日志
                new ServiceFactory.JoinNew().AddApplyLog(p_join_apply.apply_sn, p_join_apply_log.c_type_enum.完成, "超过7天未完成自动更新完成");

                try
                {
                    // 给厅管推送公众号消息
                    new ServiceFactory.Sdk.WeixinSendMsg().WorkOrderCancel(p_join_apply.tg_user_sn, "", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger").host_domain}/Home/MobileView", new ServiceFactory.Sdk.WeixinSendMsg.WorkOrderCancelInfo
                    {
                        name = $"{new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn).ting_name}的补人申请超过7天未完成",
                        cancel_time = DateTime.Now,
                        number = p_join_apply.apply_sn
                    });
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return "补人申请单超过7天未完成设置自动完成";
        }

        /// <summary>
        /// 计算补人申请直播厅7天补人率和15天留人率（用于补人处理显示和排序）
        /// </summary>
        /// <returns></returns>
        public string SetJoinApplyRate()
        {
            foreach (var item in new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()))
            {
                /*
                 * 7天补人率指10天前-3天前补人成功的主播比例
                 * 15天留人率指20天前-5天前补进来（拉群），在5天内开账号的主播比例
                 */

                // 7天提交补人
                var apply_zb_count = DoMySql.FindField<p_join_apply>("sum(zb_count)", $"ting_sn = '{item.ting_sn}' and status <= {p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0].ToInt();
                // 7天实际补人
                var real_zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"exists (select 1 from p_join_apply where id = p_join_new_info.tg_need_id and ting_sn = '{item.ting_sn}' and status <= {p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {p_join_new_info.status_enum.等待培训.ToSByte()} or status = {p_join_new_info.status_enum.补人完成.ToSByte()})")[0].ToInt();
                // 7天补人率（7天实际补人 / 7天提交补人）
                var join_rate = apply_zb_count > 0 ? Math.Round(real_zb_count.ToDecimal() / apply_zb_count.ToDecimal() * 100, 2) : 0;

                // 15天补进来（拉群）
                var zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"ting_sn = '{item.ting_sn}' and exists (select 1 from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)")[0].ToInt();

                // 15天留人（拉群5天内开通账号入职）
                var stay_zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"ting_sn = '{item.ting_sn}' and exists (select 1 from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id) and status = {p_join_new_info.status_enum.补人完成.ToSByte()} and exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0].ToInt();

                // 15天留人率（15天留人 / 15天补进来）
                var stay_rate = zb_count > 0 ? Math.Round(stay_zb_count.ToDecimal() / zb_count.ToDecimal() * 100, 2) : 0;

                // 更新数据
                item.join_rate = join_rate;
                item.stay_rate = stay_rate;
                item.ToModel<user_info_tg>().Update();

                new p_join_apply()
                {
                    join_rate = join_rate,
                    stay_rate = stay_rate
                }.Update($"ting_sn = '{item.ting_sn}' and status <= {p_join_apply.status_enum.等待外宣补人.ToSByte()}");
            }
            return "计算补人申请直播厅7天补人率和15天留人率";
        }

        /// <summary>
        /// 计算运营团队7天补人率和15天留人率
        /// </summary>
        /// <returns></returns>
        public string SetYyJoinRate()
        {
            foreach (var item in new ServiceFactory.UserInfo.Yy().GetBaseInfos(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()))
            {
                /*
                 * 7天补人率指10天前-3天前补人成功的主播比例
                 * 15天留人率指20天前-5天前补进来（拉群），在5天内开账号的主播比例
                 */

                // 7天提交补人
                var apply_zb_count = DoMySql.FindField<p_join_apply>("sum(zb_count)", $"yy_user_sn = '{item.user_sn}' and status <= {p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0].ToInt();
                // 7天实际补人
                var real_zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"exists (select 1 from p_join_apply where id = p_join_new_info.tg_need_id and yy_user_sn = '{item.user_sn}' and status <= {p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {p_join_new_info.status_enum.等待培训.ToSByte()} or status = {p_join_new_info.status_enum.补人完成.ToSByte()})")[0].ToInt();
                // 7天补人率（7天实际补人 / 7天提交补人）
                var join_rate = apply_zb_count > 0 ? Math.Round(real_zb_count.ToDecimal() / apply_zb_count.ToDecimal() * 100, 2) : 0;

                // 15天补进来（拉群）
                var zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"yy_user_sn = '{item.user_sn}' and exists (select 1 from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)")[0].ToInt();

                // 15天留人（拉群5天内开通账号入职）
                var stay_zb_count = DoMySql.FindField<p_join_new_info>("count(1)", $"yy_user_sn = '{item.user_sn}' and exists (select 1 from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id) and status = {p_join_new_info.status_enum.补人完成.ToSByte()} and exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0].ToInt();

                // 15天留人率（15天留人 / 15天补进来）
                var stay_rate = zb_count > 0 ? Math.Round(stay_zb_count.ToDecimal() / zb_count.ToDecimal() * 100, 2) : 0;

                // 更新数据
                item.attach2 = join_rate.ToString();
                item.attach3 = stay_rate.ToString();
                item.ToModel<user_base>().Update();
            }
            return "计算运营团队7天补人率和15天留人率";
        }

        /// <summary>
        /// 重置补人置顶次数
        /// </summary>
        /// <returns></returns>
        public string SetJoinPinToTopTimes()
        {
            // 获取当前日期的天数，仅每月1号执行
            if (DateTime.Now.Day != 1)
            {
                return "";
            }

            // 重置（运营每月默认5次）
            new user_info_yunying()
            {
                join_pintotop_times = 5
            }.Update("1=1");
            return "重置补人置顶次数";
        }

        /// <summary>
        /// 厅管开通账号提醒（补人拉群后5天内未开通账号发送厅管提醒）
        /// </summary>
        /// <returns></returns>
        public string CreateZhuboMsg()
        {
            foreach (var item in new ServiceFactory.UserInfo.Tg().GetBaseInfos(new ServiceFactory.UserInfo.Tg.TgBaseInfoFilter()))
            {
                try
                {
                    // 存在5天前拉群并且没有完成培训或流失的主播，发送提醒
                    var peixun = DoMySql.FindList<p_join_new_info>($"tg_user_sn = '{item.user_sn}' and status = {p_join_new_info.status_enum.等待培训.ToSByte()} and id in (select user_info_zb_id from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time < '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}')");
                    if (peixun.Count > 0)
                    {
                        // 给厅管推送公众号消息
                        new ServiceFactory.Sdk.WeixinSendMsg().WorkOrderCancel(item.user_sn, "", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger").host_domain}/JoinNew/Training/WaitTraining", new ServiceFactory.Sdk.WeixinSendMsg.WorkOrderCancelInfo
                        {
                            name = "主播超过5天未完成培训请确认是否流失",
                            cancel_time = DateTime.Now,
                            number = "0"
                        });
                    }

                    // 存在5天前拉群并且培训后没有开通账号的主播，发送提醒
                    var ruzhi = DoMySql.FindList<p_join_new_info>($"tg_user_sn = '{item.user_sn}' and status = {p_join_new_info.status_enum.补人完成.ToSByte()} and id in (select user_info_zb_id from p_join_new_info_log where c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time < '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}') and exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and status = {user_info_zhubo.status_enum.待开账号.ToSByte()})");
                    if (ruzhi.Count > 0)
                    {
                        // 给厅管推送公众号消息
                        new ServiceFactory.Sdk.WeixinSendMsg().WorkOrderCancel(item.user_sn, "", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger").host_domain}/UserInfo/Zhubo/NewList", new ServiceFactory.Sdk.WeixinSendMsg.WorkOrderCancelInfo
                        {
                            name = "主播超过5天未开通账号请确认是否流失",
                            cancel_time = DateTime.Now,
                            number = "0"
                        });
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return "厅管开通账号提醒";
        }

        /// <summary>
        /// 更新补人团队数据统计的数据
        /// </summary>
        /// <returns></returns>
        public string JoinDataStatistics()
        {
            foreach (var item in new ServiceFactory.UserInfo.Yy().GetBaseInfos(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()))
            {
                var yy_user_sn = item.user_sn;

                // 更新10天内的数据
                string feild_sum = "apply_date, sum(zb_sum) zb_sum, sum(unsupplement_sum) unsupplement_sum, sum(uninqun_sum) uninqun_sum, sum(inqun_sum) inqun_sum, sum(b_allocation_loss) b_allocation_loss, sum(b_traning_loss) b_traning_loss, sum(a_traning_loss) a_traning_loss, sum(male_zb_sum) male_zb_sum, sum(female_zb_sum) female_zb_sum, sum(male_training_sum) male_training_sum, sum(female_training_sum) female_training_sum, sum(male_supplement_sum) male_supplement_sum, sum(female_supplement_sum) female_supplement_sum, sum(male_inqun_sum) male_inqun_sum, sum(female_inqun_sum) female_inqun_sum";
                string feild = "yy_user_sn, b.id, DATE_FORMAT(a.create_time, '%Y-%m-%d') apply_date,";
                // 申请数（提交补人申请的主播）
                feild += "b.zb_count as zb_sum,";
                // 未分配（提交补人申请未分配的主播）
                feild += "b.unsupplement_count as unsupplement_sum,";
                // 待拉群（补人待拉群状态的主播）
                feild += "b.put_count as uninqun_sum,";
                // 已拉群（做过拉群操作的主播）
                feild += $"(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn AND id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and tg_dangwei = b.id) as inqun_sum,";
                // 分配前流失数（拉群之前流失的主播）
                feild += $"(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn AND id not in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and tg_dangwei = b.id and status = {p_join_new_info.status_enum.逻辑删除.ToSByte()}) as b_allocation_loss,";

                // 分配后未培训流失数（拉群之后没有完成培训流失的主播）
                feild += $"(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn AND id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and tg_dangwei = b.id and status = {p_join_new_info.status_enum.逻辑删除.ToSByte()}) as b_traning_loss,";
                // 分配后已培训流失数（完成培训后流失的主播）
                feild += $"(select count(1) FROM user_info_zhubo WHERE yy_user_sn = a.yy_user_sn and sources_name = '外宣补人完成培训' and (status = {user_info_zhubo.status_enum.已离职.ToSByte()} or status = {user_info_zhubo.status_enum.逻辑删除.ToSByte()}) and tg_dangwei = b.id) as a_traning_loss,";

                // 申请数男生（提交补人申请未分配的主播）
                feild += "if(tg_sex = '男',b.zb_count,0) as male_zb_sum,";
                // 申请数女生（提交补人申请未分配的主播）
                feild += "if(tg_sex = '女',b.zb_count,0) as female_zb_sum,";

                // 培训名单总数男生（全部主播）
                feild += "(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn and zb_sex = '男' and tg_dangwei = b.id) as male_training_sum,";
                // 培训名单总数女生（全部主播）
                feild += "(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn and zb_sex = '女' and tg_dangwei = b.id) as female_training_sum,";

                // 已分配总数男生（提交补人申请已分配的主播）
                feild += "if(tg_sex = '男',b.zb_count - b.unsupplement_count,0) as male_supplement_sum,";
                // 已分配总数女生（提交补人申请已分配的主播）
                feild += "if(tg_sex = '女',b.zb_count - b.unsupplement_count,0) as female_supplement_sum,";

                // 已拉群总数男生（做过拉群操作的主播）
                feild += $"(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn AND id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and tg_dangwei = b.id and zb_sex = '男') as male_inqun_sum,";
                // 已拉群总数女生（做过拉群操作的主播）
                feild += $"(select count(1) from p_join_new_info where yy_user_sn = a.yy_user_sn AND id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {p_join_new_info_log.c_type_enum.拉群.ToSByte()}) and tg_dangwei = b.id and zb_sex = '女') as female_inqun_sum";

                var list = DoMySql.FindListBySql<ServiceFactory.JoinNew.v_p_join_new>($"SELECT {feild_sum} FROM (SELECT {feild} FROM p_join_apply a,p_join_apply_item b where a.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and a.status <= {p_join_apply.status_enum.已完成.ToSByte()} and a.apply_sn = b.apply_sn and a.create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and yy_user_sn = '{yy_user_sn}') t group by apply_date");
                foreach (var data in list)
                {
                    new p_join_new_data_statistics_yy()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        yy_user_sn = yy_user_sn,
                        yy_name = new DomainBasic.UserApp().GetInfoByUserSn(yy_user_sn).username,
                        apply_date = data.apply_date,
                        zb_sum = data.zb_sum.ToInt(),
                        unsupplement_sum = data.unsupplement_sum.ToInt(),
                        uninqun_sum = data.uninqun_sum.ToInt(),
                        inqun_sum = data.inqun_sum.ToInt(),
                        b_allocation_loss = data.b_allocation_loss.ToInt(),
                        b_traning_loss = data.b_traning_loss.ToInt(),
                        a_traning_loss = data.a_traning_loss.ToInt(),
                        male_zb_sum = data.male_zb_sum.ToInt(),
                        female_zb_sum = data.female_zb_sum.ToInt(),
                        male_training_sum = data.male_training_sum.ToInt(),
                        female_training_sum = data.female_training_sum.ToInt(),
                        male_supplement_sum = data.male_supplement_sum.ToInt(),
                        female_supplement_sum = data.female_supplement_sum.ToInt(),
                        male_inqun_sum = data.male_inqun_sum.ToInt(),
                        female_inqun_sum = data.female_inqun_sum.ToInt()
                    }.InsertOrUpdate($"yy_user_sn = '{yy_user_sn}' and apply_date = '{data.apply_date}'");
                }
            }

            // 日志
            new DomainBasic.SystemBizLogApp().Write("定时任务", sys_biz_log.log_type_enum.产品模块.ToSByte(), "", $"数据最后更新时间:{DateTime.Now}", "补人团队数据");
            return "补人团队数据统计";
        }
    }
}
