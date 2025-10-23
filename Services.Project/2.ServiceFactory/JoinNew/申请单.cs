using Newtonsoft.Json.Linq;
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
            /// 补人申请操作日志
            /// </summary>
            /// <param name="need_id"></param>
            /// <param name="e"></param>
            /// <param name="content"></param>
            public void AddApplyLog(string apply_sn, Enum e, string content = "")
            {
                var p_join_apply = DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'", false);
                if (content.IsNullOrEmpty())
                {
                    content = $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'{e.ToString()}了'{p_join_apply.tg_username}'在'{p_join_apply.create_time}'提交的申请";
                }
                new ModelDb.p_join_apply_log()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    apply_sn = apply_sn,
                    user_sn = new UserIdentityBag().user_sn,
                    content = content,
                    user_type_id = new DomainBasic.UserTypeApp().GetInfo().id,
                    c_type = e.ToSByte(),
                }.Insert();
            }

            /// <summary>
            /// 取消申请
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction CancelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_join_apply = DoMySql.FindEntityById<ModelDb.p_join_apply>(req.GetPara()["id"].ToNullableString().ToInt());

                //如果存在等待拉群的主播，禁止取消
                if (!DoMySql.FindEntity<ModelDb.p_join_new_info>($"tg_need_id='{p_join_apply.id}'and status = '{ModelDb.p_join_new_info.status_enum.等待拉群.ToSByte()}'", false).IsNullOrEmpty())
                {
                    throw new Exception("还有未完成拉群的主播，禁止取消");
                }

                p_join_apply.status = ModelDb.p_join_apply.status_enum.已取消.ToSByte();
                p_join_apply.Update();
                AddApplyLog(p_join_apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.取消);
                return result;
            }

            /// <summary>
            /// 外宣补人操作
            /// </summary>
            /// <param name="zbList"></param>
            /// <param name="ting_sn"></param>
            /// <param name="apply_id"></param>
            /// <param name="apply_item_id"></param>
            /// <returns></returns>
            public JsonResultAction SupplementAction(List<ModelDb.p_join_new_info> zbList, string ting_sn, int apply_id, int apply_item_id)
            {
                var result = new JsonResultAction();

                var tingInfo = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn);
                List<string> lSql = new List<string>();
                foreach (var item in zbList)
                {
                    if (item.status != ModelDb.p_join_new_info.status_enum.等待分配.ToSByte() && item.status != ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte()) continue;

                    item.old_tg_user_sn = item.ting_sn;
                    item.tg_user_sn = tingInfo.tg_user_sn;
                    item.ting_sn = ting_sn;
                    item.yy_user_sn = tingInfo.yy_user_sn;
                    item.tg_need_id = apply_id;
                    item.tg_dangwei = apply_item_id;
                    /////
                    var dyParam = new dyCheckParam()
                    {
                        anchor_id = item.anchor_id
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetBrokerByAnchorId", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    //记录分配日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.分配, item.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了补人分配");

                    if (dyCheckResult["code"].ToNullableString().Equals("0"))//接口正常
                    {
                        var obj = JObject.Parse(dyCheckResult["data"].ToNullableString());
                        //经纪人是外宣审核字样的抖音号可以自动进行经纪人设置
                        if (obj["agent_name"].ToNullableString().Contains("外宣审核-") || obj["agent_name"].ToNullableString().Contains("经纪人-") || obj["agent_name"].ToNullableString().Contains("外宣-") || obj["agent_name"].ToNullableString().Contains("经纪人—") || obj["agent_name"].ToNullableString().Contains("审核-"))
                        {
                            //自动进行经纪人UID设置
                            //var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn);
                            //var JjrParam = new dyCheckParam()
                            //{
                            //    anchor_id = item.anchor_id,
                            //    broker_id = ting.jjr_uid,
                            //};
                            //var setInfo = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/SetTg", JjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                            //{
                            //    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                            //}).ToJObject();
                            //if (setInfo["code"].ToNullableString().Equals("0"))//自动设置成功直接进入待拉群状态
                            //{
                            //    // 日志
                            //    item.status = ModelDb.p_join_new_info.status_enum.等待拉群.ToSByte();
                            //    //记录自动入库成功日志
                            //    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.入库, item.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了补人分配，自动入库成功，运营经纪人:{ting.jjr_name},uid:{ting.jjr_uid}");
                            //}
                            //else //自动设置失败进入等待入库手动入库操作
                            //{
                            // 日志
                            item.status = ModelDb.p_join_new_info.status_enum.等待入库.ToSByte();
                            //记录自动入库失败日志
                            new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.重新入库, item.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了补人分配，自动入库失败需手动重新入库");
                            //}
                        }
                        else//已有经纪人则转至萌新处提交是否需要转厅的申请
                        {
                            item.status = ModelDb.p_join_new_info.status_enum.有对接厅.ToSByte();
                            item.old_tg_username = obj["agent_name"].ToNullableString();
                            //记录自动入库失败日志
                            new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.入库失败, item.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了补人分配，自动入库失败有对接厅");
                        }
                    }
                    else //接口异常直接进入等待入库待手动入库
                    {
                        item.status = ModelDb.p_join_new_info.status_enum.等待入库.ToSByte();
                        //记录自动入库失败日志
                        new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.重新入库, item.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了补人分配，自动入库失败需手动重新入库");
                    }

                    item.is_fast = ModelDb.user_info_zb.is_fast_enum.不加急.ToSByte();
                    lSql.Add(item.UpdateTran());
                }
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }

            /// <summary>
            /// 补人申请单明细
            /// </summary>
            public class applyItemDetails : ModelDb.p_join_apply_item
            {
                /// <summary>
                /// 厅管性别
                /// </summary>
                public string tg_sex { get; set; }
                /// <summary>
                /// 管理
                /// </summary>
                public string manager { get; set; }
                /// <summary>
                /// 厅管用户编号
                /// </summary>
                public string ting_sn { get; set; }
                /// <summary>
                /// 申请原因
                /// </summary>
                public string apply_cause { get; set; }
                /// <summary>
                /// 申请单状态
                /// </summary>
                public int apply_status { get; set; }
                /// <summary>
                /// 申请单状态
                /// </summary>
                public int apply_id { get; set; }
            }

            /// <summary>
            /// 调用抖音接口请求参数
            /// </summary>
            public class dyCheckParam : Entity
            {
                /// <summary>
                /// 抖音账号
                /// </summary>
                public object dou_username { get; set; }
                /// <summary>
                /// 抖音作者id
                /// </summary>
                public object anchor_id { get; set; }
                /// <summary>
                /// 经纪人uid
                /// </summary>
                public object broker_id { get; set; }
                /// <summary>
                /// 手机后四位
                /// </summary>
                public object last_four_number { get; set; }
                /// <summary>
                /// 真实姓名
                /// </summary>
                public object real_name { get; set; }
            }

            /// <summary>
            /// 检查主播能否申请补人
            /// </summary>
            /// <returns></returns>
            public List<ModelDb.p_join_apply_item> CheckAccessApplyZb(List<ModelDb.p_join_apply_item> applyItemList, ModelDb.p_join_apply p_join_apply)
            {
                foreach (var item in applyItemList)
                {
                    if (item.dangwei.IsNullOrEmpty())
                    {
                        throw new Exception("档位不可为空");
                    }
                    if (item.zb_count.ToInt() <= 0)
                    {
                        throw new Exception($"{item.dangwei}档申请人数必须大于0");
                    }
                    //校验是否允许厅管申请该档位
                    var compare_p_join_apply = DoMySql.FindListBySql<ModelDb.p_join_apply_item>($"select t2.* from p_join_apply t1 left join p_join_apply_item t2 on t1.apply_sn = t2.apply_sn where t1.ting_sn = '{p_join_apply.ting_sn}' and t1.id != '{p_join_apply.id}' and t1.status != '{ModelDb.p_join_apply.status_enum.已完成.ToInt()}' and t1.status != '{ModelDb.p_join_apply.status_enum.已拒绝.ToInt()}' and t1.status != '{ModelDb.p_join_apply.status_enum.已取消.ToInt()}'");
                    if (!compare_p_join_apply.IsNullOrEmpty())
                    {
                        foreach (var detail in compare_p_join_apply)
                        {
                            if (detail.dangwei == item.dangwei && detail.status != ModelDb.p_join_apply_item.status_enum.已完成.ToSByte())
                            {
                                throw new Exception($"档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}已有补人申请，请勿重复提交同档位的申请");
                            }
                        }
                    }

                    var total_count = 0;
                    #region 计算厅档位拥有主播数
                    //总数 = 现有主播数 + 待开账号数 + 在补主播数 + 申请主播数
                    var zhubo_count = new UserInfo.Zhubo().GetBaseInfos(new UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        status = UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                        attachUserType = new UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = p_join_apply.ting_sn
                        },
                        attachWhere = $"sessions like '%{item.dangwei}%'"
                    }).Count;
                    var new_zhubo_count = new UserInfo.Zhubo().GetBaseInfos(new UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        status = UserInfo.Zhubo.ZbBaseInfoFilter.Status.待开账号,
                        attachUserType = new UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = p_join_apply.ting_sn
                        },
                        attachWhere = $"sessions like '%{item.dangwei}%'"
                    }).Count;
                    var apply_zb_count = GetApplySessionZbCountNew(p_join_apply.ting_sn, item.dangwei);

                    total_count = zhubo_count + new_zhubo_count + apply_zb_count + item.zb_count.ToInt();
                    #endregion
                    //大于5人为超额,禁止补人
                    if (total_count > 5)
                    {
                        //throw new Exception($"档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}当前{total_count}人（已有主播{zhubo_count}+在补人数{new_zhubo_count + apply_zb_count}+申请人数{item.zb_count.ToInt()}），已超5人上限（离职主播请即时删除）");
                    }
                }
                return applyItemList;
            }

            /// <summary>
            /// 获取厅当前在补主播数
            /// </summary>
            /// <param name="ting_sn"></param>
            /// <returns></returns>
            public int GetApplyZbCountNew(string ting_sn)
            {
                // 提交申请中的未分配主播数+补人中的主播数
                var apply_zb_count = DoMySql.FindField<ModelDb.p_join_apply_item>("sum(unsupplement_count)", $"apply_sn in (select apply_sn from p_join_apply where ting_sn = '{ting_sn}' and status <= {ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()})")[0].ToInt();
                return apply_zb_count + DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()} and status != {ModelDb.p_join_new_info.status_enum.补人完成.ToInt()}")[0].ToInt();
            }

            /// <summary>
            /// 获取厅档位当前在补主播数
            /// </summary>
            /// <param name="ting_sn"></param>
            /// <param name="dangwei"></param>
            /// <returns></returns>
            public int GetApplySessionZbCountNew(string ting_sn, string dangwei)
            {
                // 提交申请中的未分配主播数+补人中的主播数
                var apply_zb_count = DoMySql.FindField<ModelDb.p_join_apply_item>("sum(unsupplement_count)", $"apply_sn in (select apply_sn from p_join_apply where ting_sn = '{ting_sn}' and dangwei = '{dangwei}' and status <= {ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()})")[0].ToInt();
                return apply_zb_count + DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and sessions like '%{dangwei}%' and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()} and status != {ModelDb.p_join_new_info.status_enum.补人完成.ToInt()}")[0].ToInt();
            }

            /// <summary>
            /// 获取运营待审批申请单总数
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public int GetYyApproveCount(string yy_user_sn)
            {
                return DoMySql.FindList<ModelDb.p_join_apply>($"yy_user_sn = '{yy_user_sn}' and status = '{ModelDb.p_join_apply.status_enum.等待运营审批.ToInt()}'").Count;
            }

            /// <summary>
            /// 获取超管待审批申请单总数
            /// </summary>
            /// <returns></returns>
            public int GetManagerApproveCount()
            {
                return DoMySql.FindList<ModelDb.p_join_apply>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = '{ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()}'").Count;
            }

            /// <summary>
            /// 根据厅获取档位审批数
            /// </summary>
            /// <returns></returns>
            public int GetDangweiApproveCountByTingSn(string ting_sn, string dangwei)
            {
                return DoMySql.FindListBySql<ModelDb.p_join_apply_item>($"SELECT t2.* FROM p_join_apply t1 left join p_join_apply_item t2 on t1.apply_sn = t2.apply_sn WHERE t1.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} AND (t1.STATUS = {ModelDb.p_join_apply.status_enum.等待运营审批.ToSByte()} OR t1.STATUS = {ModelDb.p_join_apply.status_enum.等待公会审批.ToSByte()}) AND t1.ting_sn = '{ting_sn}' and t2.dangwei = '{dangwei}'").Count;
            }

            /// <summary>
            /// 根据厅获取档位申请数
            /// </summary>
            /// <returns></returns>
            public int GetDangweiApplyCountByTingSn(string ting_sn, string dangwei)
            {
                return DoMySql.FindListBySql<ModelDb.p_join_apply_item>($"SELECT t2.* FROM p_join_apply t1 left join p_join_apply_item t2 on t1.apply_sn = t2.apply_sn WHERE t1.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} AND t1.STATUS = {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()} and t2.STATUS = {ModelDb.p_join_apply_item.status_enum.未完成.ToSByte()} AND t1.ting_sn = '{ting_sn}' and t2.dangwei = '{dangwei}'").Count;
            }

            /// <summary>
            /// 根据性别和档位获取申请未分配总人数
            /// </summary>
            /// <param name="zb_sex">男女厅</param>
            /// <param name="dangwei">档位</param>
            /// <returns></returns>
            public int GetApplyZbCountByDangwei(string zb_sex, string dangwei)
            {
                string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.p_join_apply_item.status_enum.未完成.ToSByte()} and exists (select 1 from p_join_apply where apply_sn = p_join_apply_item.apply_sn and status <= {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()} and tg_sex = '{zb_sex}')";
                if (!dangwei.IsNullOrEmpty()) where += $" and dangwei = '{dangwei}'";
                return DoMySql.FindField<ModelDb.p_join_apply_item>("sum(unsupplement_count)", where)[0].ToInt();
            }

            /// <summary>
            /// 快捷更改权重值
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction FastEditWeight(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();

                var p_join_new_weight = DoMySql.FindEntity<ModelDb.p_join_new_weight>($"id='{req["id"].ToNullableString()}'", false);

                if (!p_join_new_weight.IsNullOrEmpty())
                {
                    p_join_new_weight.SetValue(req["name"].ToNullableString(), req["value"].ToInt());
                    p_join_new_weight.Update();
                }

                // 更新补人申请表权重值
                UpdateWeight();
                return result;
            }

            /// <summary>
            /// 更新补人申请表权重值（定义权重值1000表示申请单置顶，置顶后不更新权重）
            /// </summary>
            public void UpdateWeight()
            {
                // 根据设置的权重更新申请表权重
                foreach (var p_join_new_weight in DoMySql.FindList<ModelDb.p_join_new_weight>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by w_type"))
                {
                    switch (p_join_new_weight.w_type)
                    {
                        case (sbyte)ModelDb.p_join_new_weight.w_type_enum.运营:
                            new ModelDb.p_join_apply
                            {
                                weight = p_join_new_weight.weight
                            }.Update($"yy_user_sn = '{p_join_new_weight.yy_user_sn}' and weight != 1000");
                            break;
                        case (sbyte)ModelDb.p_join_new_weight.w_type_enum.厅:
                            new ModelDb.p_join_apply
                            {
                                weight = p_join_new_weight.weight
                            }.Update($"ting_sn = '{p_join_new_weight.ting_sn}' and weight != 1000");
                            break;
                    }
                }

                // 未设置权重更新申请表权重为0
                new ModelDb.p_join_apply
                {
                    weight = 0
                }.Update($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and not exists (select 1 from p_join_new_weight where yy_user_sn = p_join_apply.yy_user_sn) and not exists (select 1 from p_join_new_weight where ting_sn = p_join_apply.ting_sn) and weight != 1000");
            }
        }
    }
}
