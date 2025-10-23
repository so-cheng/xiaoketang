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
        public class TingZhanService
        {
            /// <summary>
            /// // 取最近的厅战
            /// </summary>
            /// <returns></returns>
            public ModelDb.p_tingzhan getNewTingzhan()
            {
                return DoMySql.FindEntity<ModelDb.p_tingzhan>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by create_time desc", false);
            }

            /// <summary>
            /// 取厅目标
            /// </summary>
            /// <param name="p_tingzhan_id"></param>
            /// <param name="ting_sn"></param>
            /// <returns></returns>
            public ModelDb.p_tingzhan_target getCurrentTarget(int p_tingzhan_id, string ting_sn)
            {
                return DoMySql.FindEntity<ModelDb.p_tingzhan_target>($"tingzhan_id = {p_tingzhan_id} and ting_sn = '{ting_sn}'", false);
            }

            /// <summary>
            /// 添加厅目标
            /// </summary>
            /// <param name="p_tingzhan_id"></param>
            /// <param name="ting_sn"></param>
            /// <param name="amont"></param>
            /// <param name="lSql"></param>
            /// <returns></returns>
            public List<string> AddTargetForSql(int? p_tingzhan_id, string ting_sn, decimal? amont, List<string> lSql)
            {
                var ting_sn_target = DoMySql.FindEntity<ModelDb.p_tingzhan_target>($"tingzhan_id = {p_tingzhan_id} and ting_sn = '{ting_sn}'", false);
                if (ting_sn_target.IsNullOrEmpty())
                {
                    // 获取厅信息
                    var ting = new UserInfo.Ting().GetTingBySn(ting_sn);
                    ting_sn_target = new ModelDb.p_tingzhan_target
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        tingzhan_id = p_tingzhan_id,
                        yy_user_sn = ting.yy_user_sn,
                        tg_user_sn = ting.tg_user_sn,
                        ting_sn = ting_sn,
                        amont = amont
                    };

                    lSql.Add(ting_sn_target.InsertTran());
                }

                return lSql;
            }

            /// <summary>
            /// 更新对战表目标音浪
            /// </summary>
            /// <param name="reqJson"></param>
            public JsonResultAction EditMate(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id={req["id"]}", false);
                p_tingzhan_mate.amont = req["amont"].ToDecimal();

                p_tingzhan_mate.Update();

                return result;
            }

            /// <summary>
            /// 长期规则生效
            /// </summary>
            public void SetRulelong()
            {
                var list = DoMySql.FindList<ModelDb.p_tingzhan_mate_rulelong>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.p_tingzhan_mate_rulelong.status_enum.启用.ToSByte().ToInt()}");
                List<string> lSql = new List<string>();
                foreach (var item in list)
                {
                    switch (item.rulelong_type)
                    {
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟厅打:
                            lSql.Add(new ModelDb.p_tingzhan_mate_rule
                            {
                                tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                                tingzhan_id = getNewTingzhan().id,
                                rule_type = ModelDb.p_tingzhan_mate_rule.rule_type_enum.不跟厅打.ToSByte(),
                                tg_user_sn1 = new UserInfo.Ting().GetTingBySn(item.user_sn1).tg_user_sn,
                                ting_sn1 = item.user_sn1,
                                tg_user_sn2 = new UserInfo.Ting().GetTingBySn(item.user_sn2).tg_user_sn,
                                ting_sn2 = item.user_sn2,
                            }.InsertTran());
                            break;
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟运营打:
                            var tings = new UserInfo.Ting().GetBaseInfos(new UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = item.user_sn2,
                                }
                            });
                            foreach (var ting in tings)
                            {
                                lSql.Add(new ModelDb.p_tingzhan_mate_rule
                                {
                                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                                    tingzhan_id = getNewTingzhan().id,
                                    rule_type = ModelDb.p_tingzhan_mate_rule.rule_type_enum.不跟厅打.ToSByte(),
                                    tg_user_sn1 = new UserInfo.Ting().GetTingBySn(item.user_sn1).tg_user_sn,
                                    ting_sn1 = item.user_sn1,
                                    tg_user_sn2 = new UserInfo.Ting().GetTingBySn(ting.ting_sn).tg_user_sn,
                                    ting_sn2 = ting.ting_sn
                                }.InsertTran());
                            }
                            break;
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.运营不跟运营打:
                            var tings1 = new UserInfo.Ting().GetBaseInfos(new UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = item.user_sn1,
                                }
                            });
                            var tings2 = new UserInfo.Ting().GetBaseInfos(new UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = item.user_sn2,
                                }
                            });
                            foreach (var ting1 in tings1)
                            {
                                foreach (var ting2 in tings2)
                                {
                                    lSql.Add(new ModelDb.p_tingzhan_mate_rule
                                    {
                                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                                        tingzhan_id = getNewTingzhan().id,
                                        rule_type = ModelDb.p_tingzhan_mate_rule.rule_type_enum.不跟厅打.ToSByte(),
                                        tg_user_sn1 = new UserInfo.Ting().GetTingBySn(ting1.ting_sn).tg_user_sn,
                                        ting_sn1 = ting1.ting_sn,
                                        tg_user_sn2 = new UserInfo.Ting().GetTingBySn(ting2.ting_sn).tg_user_sn,
                                        ting_sn2 = ting2.ting_sn
                                    }.InsertTran());
                                }
                            }
                            break;
                    }
                }
                DoMySql.ExecuteSqlTran(lSql);
            }

            /// <summary>
            /// 按日均音浪和近三场厅战完成率排序
            /// </summary>
            /// <param name="p_tingzhan_targets"></param>
            /// <returns></returns>
            public List<TingMate> TargetsOrderByDayAmountAndCompletion(List<ModelDb.p_tingzhan_target> p_tingzhan_targets, List<TingMate> newList)
            {
                // 取近三场厅战的目标和战绩
                var target = DoMySql.FindList<ModelDb.p_tingzhan_target>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id in (SELECT id FROM (SELECT id FROM p_tingzhan where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by create_time desc LIMIT 1,3) AS subquery)");
                var mate = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id in (SELECT id FROM (SELECT id FROM p_tingzhan where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by create_time desc LIMIT 1,3) AS subquery)");
                foreach (var p_tingzhan_target in p_tingzhan_targets)
                {
                    // 厅目标
                    var tingTargetList = target.Where(x => x.ting_sn == p_tingzhan_target.ting_sn).ToList();
                    // 厅左战绩
                    var tingMate1List = mate.Where(x => x.ting_sn1 == p_tingzhan_target.ting_sn).ToList();
                    // 厅右战绩
                    var tingMate2List = mate.Where(x => x.ting_sn2 == p_tingzhan_target.ting_sn).ToList();

                    // 厅总战绩
                    var scoreSum = decimal.Add(tingMate1List.Sum(x => x.score_1).ToDecimal(), tingMate2List.Sum(x => x.score_2).ToDecimal());
                    // 厅总目标
                    var amontSum = tingTargetList.Sum(x => x.amont);

                    newList.Add(new TingMate()
                    {
                        ting_sn = p_tingzhan_target.ting_sn,
                        yy_user_sn = p_tingzhan_target.yy_user_sn,
                        day_amount = p_tingzhan_target.day_amount,
                        completion = amontSum > 0 ? scoreSum / amontSum.ToDecimal() : 0 // 厅完成率 = 厅总战绩 / 厅总目标
                    });
                }

                return newList.OrderByDescending(x => x.day_amount).ThenByDescending(x => x.completion).ToList();
            }

            public class TingMate : ModelDb.p_tingzhan_target
            {
                /// <summary>
                /// 完成率
                /// </summary>
                public decimal completion { get; set; }
            }

            /// <summary>
            /// 快捷更改厅站
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction FastEdit(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var p_jixiao_tingzhan = DoMySql.FindEntity<ModelDb.p_jixiao_tingzhan>($"id='{req["id"].ToNullableString()}'", false);
                if (!p_jixiao_tingzhan.IsNullOrEmpty())
                {
                    p_jixiao_tingzhan.SetValue(req["name"].ToNullableString(), req["value"].ToDecimal());
                    p_jixiao_tingzhan.Update();
                }



                return result;
            }


            public JsonResultAction EPT(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 排厅战
            /// </summary>
            /// <param name="tingTargets">排之前的厅</param>
            /// <param name="tingTargetDuis">排之后的对战表</param>
            /// <returns></returns>
            public List<TingTargetDui> PaiItem(List<TingTarget> tingTargets, List<TingTargetDui> tingTargetDuis)
            {
                var tingTarget1 = PaiItemBj(tingTargets[0], tingTargets);
                tingTargetDuis.Add(new TingTargetDui
                {
                    tingTarget1 = tingTargets[0],
                    tingTarget2 = tingTarget1
                });
                tingTargets.Remove(tingTargets[0]);
                tingTargets.Remove(tingTarget1);

                if (tingTargets.Count > 0) return PaiItem(tingTargets, tingTargetDuis);

                return tingTargetDuis;
            }

            /// <summary>
            /// 排厅战-规则
            /// </summary>
            /// <param name="tingTarget">当前排厅</param>
            /// <param name="tingTargets">候选厅</param>
            /// <returns></returns>
            public TingTarget PaiItemBj(TingTarget tingCur, List<TingTarget> tingTargets)
            {
                var _tingTarget = new TingTarget(); //对手厅
                foreach (var item in tingTargets)
                {
                    if (tingCur.yes_ting == item.name) return item; //指定打的厅

                    if (tingCur.yy_sn == item.yy_sn)
                    {
                        _tingTarget = item;
                        continue;  //相同团队
                    }

                    if (tingCur.last_ting == item.name)
                    {
                        _tingTarget = item;
                        continue;//上次打过
                    }

                    if (tingCur.no_tgs.IndexOf(item.name) >= 0)
                    {
                        _tingTarget = item;
                        continue;//指定不打的厅
                    }
                    return item;
                }
                return _tingTarget;
            }

            public class TingTarget
            {
                /// <summary>
                /// 厅名
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 目标
                /// </summary>
                public decimal target { get; set; }

                /// <summary>
                /// 所属运营sn
                /// </summary>
                public string yy_sn { get; set; }

                /// <summary>
                /// 日均
                /// </summary>
                public decimal day_amount { get; set; }
                /// <summary>
                /// 上次对家
                /// </summary>
                public string last_ting { get; set; }
                /// <summary>
                /// 指定不打的厅
                /// </summary>
                public List<string> no_tgs { get; set; } = new List<string>();
                /// <summary>
                /// 指定跟哪个厅打
                /// </summary>
                public string yes_ting { get; set; }
            }

            public class TingTargetDui
            {
                public TingTarget tingTarget1 { get; set; }
                public TingTarget tingTarget2 { get; set; }
            }
        }
    }

}
