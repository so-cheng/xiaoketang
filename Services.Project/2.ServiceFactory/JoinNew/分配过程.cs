using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// 补人流程操作日志
            /// </summary>
            /// <param name="e">操作</param>
            /// <param name="user_info_zb_id">主播id</param>
            /// <param name="content">操作说明</param>
            /// <param name="last_e">操作前状态</param>
            /// <param name="content">备注</param>
            /// <param name="external">是否外部操作，外部操作无登录用户休息</param>
            public void AddJoinNewLog(Enum e, int user_info_zb_id, Enum last_e, string content, bool external = false)
            {
                var p_join_new_info_log = new ModelDb.p_join_new_info_log()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    c_type = e.ToSByte(),
                    last_status = last_e.ToSByte(),
                    user_info_zb_id = user_info_zb_id,
                    content = content,
                };
                if (!external)
                {
                    p_join_new_info_log.user_type_id = new DomainBasic.UserTypeApp().GetInfo().id;
                    p_join_new_info_log.user_sn = new UserIdentityBag().user_sn;
                }

                p_join_new_info_log.Insert();
            }

            /// <summary>
            /// 快捷更改厅站
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction FastEditUserInfoZb(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();

                var p_join_new_info = DoMySql.FindEntity<ModelDb.p_join_new_info>($"id='{req["id"].ToNullableString()}'", false);

                if (!p_join_new_info.IsNullOrEmpty())
                {
                    if (req["name"].ToString() == "zb_level")
                    {
                        ChangeLevel(p_join_new_info, req["value"].ToString(), req["l_status"].IsNullOrEmpty() ? "" : req["l_status"].ToString());
                    }
                    else
                    {
                        p_join_new_info.SetValue(req["name"].ToNullableString(), req["value"].ToString());
                        p_join_new_info.Update();
                    }
                }
                return result;
            }
            public void ChangeLevel(ModelDb.p_join_new_info p_join_new_info, string level, string status)
            {
                if (ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString().Equals(status) || ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString().Equals(status))
                {
                    if (ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString().Equals(status))
                    {
                        p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待分配.ToSByte();
                    }
                    else if (ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString().Equals(status))
                    {
                        p_join_new_info.status = ModelDb.p_join_new_info.status_enum.暂不分配.ToSByte();
                    }
                    p_join_new_info.old_tg_user_sn = p_join_new_info.ting_sn;
                    p_join_new_info.ting_sn = "[null]";
                    p_join_new_info.tg_user_sn = "[null]";
                    p_join_new_info.yy_user_sn = "[null]";
                    p_join_new_info.tg_need_id = 0;
                    p_join_new_info.tg_dangwei = 0;
                    p_join_new_info.no_share = "";
                }

                if (level.IsNullOrEmpty())
                {
                    level = "-";
                }
                p_join_new_info.zb_level = level;
                if (p_join_new_info.zb_level_time.IsNullOrEmpty())
                {
                    p_join_new_info.zb_level_time = DateTime.Now;
                }

                p_join_new_info.Update();

                //记录首次主播分级时间
                new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.分级, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待分级, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播分级");
                //如果已分配厅管，重置厅管补人申请信息
                //if (tg_need_id > 0)
                //{
                //    p_join_apply = ResetPJoinNeedForEntity(p_join_apply);
                //    p_join_apply.Update();
                //}

                //new ServiceFactory.Join.MengxinSortService().SetZbSort(p_join_new_info.id).Update();
            }

            /// <summary>
            /// 计算计算申请档位明细人数
            /// </summary>
            /// <param name="dangwei_id"></param>
            public void JisuanCount(int? dangwei_id)
            {
                if (dangwei_id > 0)
                {
                    var p_join_apply_item = DoMySql.FindEntityById<ModelDb.p_join_apply_item>(dangwei_id);
                    var p_join_new_infos = DoMySql.FindList<ModelDb.p_join_new_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_dangwei = {dangwei_id}");

                    var recruited_count = 0;// 待入库（已分配人数）
                    var put_count = 0;// 待拉群（已入库人数）
                    var finish_zb_count = 0;// 待培训（已拉群人数）
                    var training_zb_count = 0;// 已完成（已培训人数）
                    var quit_count = 0;// 流失人数
                    foreach (var p_join_new_info in p_join_new_infos)
                    {
                        switch (p_join_new_info.status)
                        {
                            case (sbyte)ModelDb.p_join_new_info.status_enum.等待入库:
                                recruited_count++;
                                break;
                            case (sbyte)ModelDb.p_join_new_info.status_enum.等待拉群:
                                put_count++;
                                break;
                            case (sbyte)ModelDb.p_join_new_info.status_enum.等待培训:
                                finish_zb_count++;
                                break;
                            case (sbyte)ModelDb.p_join_new_info.status_enum.补人完成:
                                training_zb_count++;
                                break;
                            case (sbyte)ModelDb.p_join_new_info.status_enum.逻辑删除:
                                quit_count++;
                                break;
                        }
                    }

                    p_join_apply_item.recruited_count = recruited_count;
                    p_join_apply_item.put_count = put_count;
                    p_join_apply_item.finish_zb_count = finish_zb_count;
                    p_join_apply_item.training_zb_count = training_zb_count;
                    p_join_apply_item.quit_count = quit_count;
                    p_join_apply_item.unsupplement_count = p_join_apply_item.zb_count - (p_join_new_infos.Count - quit_count);
                    p_join_apply_item.other_count = p_join_apply_item.zb_count - p_join_apply_item.unsupplement_count - recruited_count - put_count - finish_zb_count - training_zb_count;
                    if (training_zb_count == p_join_apply_item.zb_count)
                    {
                        p_join_apply_item.status = ModelDb.p_join_apply_item.status_enum.已完成.ToSByte();
                    }

                    p_join_apply_item.Update();

                    // 更新补人申请完成状态
                    UpdateApplyFinish(p_join_apply_item.apply_sn);
                }
            }

            /// <summary>
            /// 更新补人申请完成状态
            /// </summary>
            /// <param name="apply_sn"></param>
            public void UpdateApplyFinish(string apply_sn)
            {
                var p_join_apply = DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'");
                if (p_join_apply.status == ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte())
                {
                    var is_update = true;
                    var p_join_apply_items = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{apply_sn}'");
                    foreach (var p_join_apply_item in p_join_apply_items)
                    {
                        if (p_join_apply_item.status != ModelDb.p_join_apply_item.status_enum.已完成.ToSByte())
                        {
                            is_update = false;
                            break;
                        }
                    }
                    if (is_update)
                    {
                        p_join_apply.status = ModelDb.p_join_apply.status_enum.已完成.ToSByte();
                        p_join_apply.Update();
                    }
                }
            }

            /// <summary>
            /// 根据拉群期数和萌新老师获取已分配人数
            /// </summary>
            /// <param name="term">期数</param
            /// <param name="mx_sn">萌新sn</param>
            /// <returns></returns>
            public int GetJobNumByTermAndMx(string term, string mx_sn)
            {
                return DoMySql.FindList<ModelDb.p_join_new_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and term = '{term}' and mx_sn = '{mx_sn}' and exists (select 1 from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = p_join_new_info.id)").Count;
            }

            /// <summary>
            /// 根据拉群期数和萌新老师获取未分配人数
            /// </summary>
            /// <param name="term">期数</param
            /// <param name="mx_sn">萌新sn</param>
            /// <returns></returns>
            public int GetNoJobNumByTermAndMx(string term, string mx_sn)
            {
                return DoMySql.FindList<ModelDb.p_join_new_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and term = '{term}' and mx_sn = '{mx_sn}' and not exists (select 1 from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = p_join_new_info.id)").Count;
            }

            /// <summary>
            /// 根据性别和档位获取未分配人数
            /// </summary>
            /// <param name="zb_sex">男女厅</param>
            /// <param name="dangwei">档位</param>
            /// <returns></returns>
            public int GetUnShareZbCountByDangwei(string zb_sex, string dangwei)
            {
                string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.p_join_new_info.status_enum.等待分配.ToSByte()} and zb_sex = '{zb_sex}'";
                if (!dangwei.IsNullOrEmpty()) where += $" and CONCAT(',',sessions, ',') like '%,{dangwei},%'";
                return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", where)[0].ToInt();
            }

            /// <summary>
            /// 根据厅申请补人档位获取已分配人数
            /// </summary>
            /// <param name="zb_sex">男女厅</param>
            /// <param name="dangwei">档位</param>
            /// <returns></returns>
            public int GetShareZbCountByApplyDangwei(string zb_sex, string dangwei)
            {
                string where = $"SELECT t1.id FROM p_join_apply t1 left join p_join_apply_item t2 on t1.apply_sn = t2.apply_sn WHERE t1.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} AND t1.STATUS = {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()} and tg_sex = '{zb_sex}'";
                if (!dangwei.IsNullOrEmpty()) where += $" and t2.dangwei = '{dangwei}'";
                return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte()} and tg_dangwei in ({where})")[0].ToInt();
            }
        }
    }
}
