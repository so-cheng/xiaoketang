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

        public partial class Join
        {
            public class MengxinZbService
            {

                /// <summary>
                /// 快捷更改厅站
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction FastEditUserInfoZb(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    //声明
                    string feild_name = req["name"].ToNullableString();    //user_info_zb 的属性名
                    string feild_value = req["value"].ToString();           //user_info_zb 的属性值

                    string cause = "";
                    var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{req["id"].ToNullableString()}'", false);
                    try
                    {
                        cause = req["cause"].ToString();
                        if (!req["cause_text"].IsNullOrEmpty())
                        {
                            cause = req["cause_text"].ToString();
                        }
                    }
                    catch
                    {

                    }
                    if (!user_info_zb.IsNullOrEmpty())
                    {
                        if (req["name"].ToString() == "zb_level")
                        {
                            ChangeLevel(user_info_zb, req["value"].ToString(), cause);
                        }
                        else
                        {
                            user_info_zb.SetValue(req["name"].ToNullableString(), req["value"].ToString());
                            user_info_zb.Update();
                        }
                    }
                    return result;
                }

                public void ChangeLevel(ModelDb.user_info_zb user_info_zb, string level, string cause = "")
                {
                    var lSql = new List<string>();
                    int tg_need_id = user_info_zb.tg_need_id.ToInt();
                    var p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{user_info_zb.tg_need_id}'", false);
                    if (level == "A" || level == "B")
                    {
                        user_info_zb.no_share = "";
                        user_info_zb.status = ModelDb.user_info_zb.status_enum.正常.ToInt().ToSByte();
                        user_info_zb.quit_status = ModelDb.user_info_zb.quit_status_enum.未退回.ToSByte();
                    }

                    if (level == "D")
                    {
                        user_info_zb.no_share = "流失原因:" + cause;
                        user_info_zb.status = ModelDb.user_info_zb.status_enum.已流失.ToInt().ToSByte();
                        //流失后，解除原有的厅管关系，记录历史厅管
                        if (!user_info_zb.tg_user_sn.IsNullOrEmpty())
                        {
                            user_info_zb.old_tg_user_sn = user_info_zb.tg_user_sn;
                            user_info_zb.old_tg_username = new ServiceFactory.TgInfoService().GetTgInfo(user_info_zb.tg_user_sn).username;
                            user_info_zb.is_qun = ModelDb.user_info_zb.is_qun_enum.未拉群.ToSByte();
                            user_info_zb.tg_user_sn = "";
                            user_info_zb.tg_need_id = 0;
                        }
                    }
                    if (level.IsNullOrEmpty())
                    {
                        level = "-";
                    }
                    user_info_zb.zb_level = level;

                    //记录首次主播分级时间
                    if (user_info_zb.zb_level_time.IsNullOrEmpty())
                    {
                        user_info_zb.zb_level_time = DateTime.Now;
                    }
                    user_info_zb.Update();
                    //如果已分配厅管，重置厅管补人申请信息
                    if (tg_need_id > 0)
                    {
                        p_join_need = ResetPJoinNeedForEntity(p_join_need);
                        p_join_need.Update();
                    }

                    new ServiceFactory.Join.MengxinSortService().SetZbSort(user_info_zb.id).Update();
                }
                /// <summary>
                /// 补人分配
                /// </summary>
                /// <param name="user_info_zb">被补人的萌新主播</param>
                /// <param name="tg_need_id">厅管补人申请id</param>
                public void SupplementZb(int user_info_zb_id, int tg_need_id,int tg_dangwei)
                {
                    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(user_info_zb_id, false);
                    var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(tg_need_id);
                    var tginfo = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_join_need.tg_user_sn);

                    user_info_zb.tg_user_sn = tginfo.user_sn;
                    user_info_zb.yy_user_sn = tginfo.yy_sn;
                    user_info_zb.tg_need_id = tg_need_id;
                    user_info_zb.tg_dangwei = tg_dangwei;
                    user_info_zb.supplement_time = DateTime.Now;
                    user_info_zb.is_change = ModelDb.user_info_zb.is_change_enum.不换.ToSByte();
                    user_info_zb.is_fast = ModelDb.user_info_zb.is_fast_enum.不加急.ToSByte();
                    user_info_zb.Update();

                    p_join_need=ResetPJoinNeedForEntity(p_join_need);
                    p_join_need.Update();
                }

                public bool UpdateUserInfoZb(int p_join_new_info_id)
                {
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(p_join_new_info_id);

                    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(p_join_new_info_id);

                    user_info_zb.supplement_time = DateTime.Now;
                    user_info_zb.tg_user_sn = p_join_new_info.tg_user_sn;
                    user_info_zb.yy_user_sn = p_join_new_info.yy_user_sn;
                    user_info_zb.tg_need_id = p_join_new_info.tg_need_id;
                    user_info_zb.tg_dangwei = p_join_new_info.tg_dangwei;
                    user_info_zb.tg_need_id = 0;
                    user_info_zb.tg_dangwei = 0;
                    return user_info_zb.Update() > 0;
                }

                #region 主播流失，退回，退群操作
                /// <summary>
                /// 退群
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                public ModelDb.user_info_zb QuitQunZb(int user_info_zb_id)
                {
                    var lSql = new List<string>();
                    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(user_info_zb_id);

                    if (user_info_zb.tg_need_id == 0)
                    {
                        throw new Exception("无所属厅管，禁止退群");
                    }
                    user_info_zb.is_qun = 0;

                    var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(user_info_zb.tg_need_id);
                    p_join_need = QuitZb(p_join_need, user_info_zb);

                    lSql.Add(user_info_zb.UpdateTran());
                    lSql.Add(CaculateApproveInfo(p_join_need).UpdateTran());

                    MysqlHelper.ExecuteSqlTran(lSql);
                    return user_info_zb;
                }

                /// <summary>
                /// 主播流失
                /// </summary>
                /// <returns></returns>
                private ModelDb.p_join_need QuitZb(ModelDb.p_join_need p_join_need, ModelDb.user_info_zb user_info_zb)
                {
                    if (user_info_zb.id == 0)
                    {
                        throw new Exception("主播已不存在");
                    }
                    if (user_info_zb.tg_user_sn != p_join_need.tg_user_sn)
                    {
                        throw new Exception("该主播已不属于当前提报申请");
                    }

                    var detail = p_join_need.apply_details.ToModel<List<ApplyItem>>();
                    foreach (var item in detail)
                    {
                        if (item.dangwei == user_info_zb.tg_dangwei.ToString())
                        {
                            item.recruited_count = (item.recruited_count.ToInt() - 1).ToString();
                            item.quite_count = (item.quite_count.ToInt() + 1).ToString();
                        }
                        if (item.recruited_count.ToInt() < 0) { throw new Exception("主播已分配数异常"); }
                        if (item.quite_count.ToInt() < 0) { throw new Exception("主播流失数异常"); }
                    }
                    p_join_need.apply_details = detail.ToJson();

                    //user_info_zb.status = ModelDb.user_info_zb.status_enum.已流失.ToSByte();
                    user_info_zb.supplement_status = ModelDb.user_info_zb.supplement_status_enum.未分配.ToSByte();
                    user_info_zb.old_tg_user_sn = user_info_zb.tg_user_sn;
                    user_info_zb.tg_user_sn = "";
                    user_info_zb.tg_need_id = 0;

                    return p_join_need;
                }
                #endregion

                /// <summary>
                /// 获取补人申请表明细数据
                /// </summary>
                /// <param name="p_join_need_id"></param>
                /// <returns></returns>
                public List<ApplyItem> GetApplyDetailForEntity(int p_join_need_id)
                {
                    var p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{p_join_need_id}'", false);
                    if (p_join_need.id == 0)
                    {
                        throw new Exception("补人申请表不存在");
                    }
                    return GetApplyDetailForEntity(p_join_need);
                }

                public List<ApplyItem> GetApplyDetailForEntity(ModelDb.p_join_need p_join_need)
                {
                    return p_join_need.apply_details.ToModel<List<ApplyItem>>();
                }

                /// <summary>
                /// 重置更新厅管补人申请的状态
                /// </summary>
                public ModelDb.p_join_need ResetApproveInfo(int p_join_need_id)
                {
                    var p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{p_join_need_id}'", false);
                    return CaculateApproveInfo(p_join_need);
                }

                /// <summary>
                /// 计算申请信息
                /// </summary>
                /// <param name="p_join_need"></param>
                /// <returns></returns>
                public ModelDb.p_join_need CaculateApproveInfo(ModelDb.p_join_need p_join_need)
                {
                    if (p_join_need.IsNullOrEmpty())
                    {
                        return p_join_need;
                    }

                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyItem>>();

                    p_join_need.zb_count = apply_details.Sum(x => x.count.ToInt());
                    p_join_need.supplement_count = apply_details.Sum(x => x.recruited_count.ToInt());
                    p_join_need.inqun_count = apply_details.Sum(x => x.inqun_count.ToInt());
                    p_join_need.quit_count = apply_details.Sum(x => x.quite_count.ToInt());

                    p_join_need.finish_zb_count = p_join_need.inqun_count;

                    //if (p_join_need.supplement_count>p_join_need.zb_count) { throw new Exception("已补人数已超过申请总人数");}

                    //if (p_join_need.inqun_count > p_join_need.zb_count) { throw new Exception("拉群人数已超过申请总人数"); }

                    //if (p_join_need.finish_zb_count > p_join_need.zb_count) { throw new Exception("已完成人数已超过申请总人数"); }

                    if (p_join_need.inqun_count >= p_join_need.zb_count)
                    {
                        p_join_need.status = ModelDb.p_join_need.status_enum.已完成.ToInt();
                        p_join_need.complete_status = ModelDb.p_join_need.complete_status_enum.已完成.ToInt();
                    }
                    return p_join_need;
                }

                /// <summary>
                /// 重置厅管申请信息
                /// </summary>
                /// <param name="p_join_need_id"></param>
                /// <returns></returns>
                public ModelDb.p_join_need ResetPJoinNeedForEntity(ModelDb.p_join_need p_join_need)
                {
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyItem>>();
                    foreach (var apply in apply_details)
                    {
                        var list = DoMySql.FindList<ModelDb.user_info_zb>($"(tg_user_sn='{p_join_need.tg_user_sn}' or old_tg_user_sn='{p_join_need.tg_user_sn}')  and tg_dangwei='{apply.dangwei}'");
                        apply.recruited_count = list.FindAll(x => x.tg_user_sn == p_join_need.tg_user_sn && x.tg_need_id == p_join_need.id && (x.zb_level == "A" || x.zb_level == "B")).Count.ToString();
                        apply.quite_count = list.FindAll(x => x.old_tg_user_sn == p_join_need.tg_user_sn).Count.ToString();
                        apply.inqun_count = list.FindAll(x => x.is_qun == 1 && (x.zb_level == "A" || x.zb_level == "B") && x.tg_need_id == p_join_need.id && x.tg_user_sn == p_join_need.tg_user_sn).Count.ToString();
                        apply.is_complete = apply.count == apply.inqun_count ? "1" : "0";
                    }
                    p_join_need.apply_details = apply_details.ToJson();
                    return CaculateApproveInfo(p_join_need);
                }

                public ModelDb.p_join_need ResetPJoinNeedForEntity(int p_join_need_id)
                {
                    var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(p_join_need_id);
                    return ResetPJoinNeedForEntity(p_join_need);
                }

                /// <summary>
                /// 主播分配
                /// </summary>
                /// <param name="p_join_need"></param>
                /// <param name="user_info_zb"></param>
                /// <returns></returns>
                private ModelDb.p_join_need SupplementZb(ModelDb.p_join_need p_join_need, ModelDb.user_info_zb user_info_zb)
                {
                    if (user_info_zb.id == 0)
                    {
                        throw new Exception("主播已不存在");
                    }
                    var detail = p_join_need.apply_details.ToModel<List<ApplyItem>>();
                    foreach (var item in detail)
                    {
                        if (item.dangwei == user_info_zb.tg_dangwei.ToString())
                        {
                            item.recruited_count = (item.recruited_count.ToInt() + 1).ToString();

                            if (user_info_zb.tg_user_sn == p_join_need.tg_user_sn)
                            {
                                item.quite_count = (item.quite_count.ToInt() - 1).ToString();
                            }
                        }
                        if (item.recruited_count.ToInt() > item.count.ToInt()) { throw new Exception("补人总数超出已申请数"); }
                        if (item.quite_count.ToInt() < 0) { throw new Exception("主播流失数异常"); }
                    }
                    p_join_need.apply_details = detail.ToJson();
                    user_info_zb.status = ModelDb.user_info_zb.status_enum.正常.ToSByte();
                    user_info_zb.supplement_status = ModelDb.user_info_zb.supplement_status_enum.已分配.ToSByte();
                    return p_join_need;
                }

                /// <summary>
                /// 检查主播能否申请补人
                /// </summary>
                /// <returns></returns>
                public ModelDb.p_join_need CheckAccessApplyZb(ModelDb.p_join_need p_join_need)
                {
                    foreach (var item in p_join_need.apply_details.ToModel<List<ApplyItem>>())
                    {
                        if (item.dangwei.IsNullOrEmpty())
                        {
                            throw new Exception("档位不可为空");
                        }
                        if (item.count.ToInt() <= 0)
                        {
                            throw new Exception($"{item.dangwei}档申请人数必须大于0");
                        }
                        //校验是否允许厅管申请该档位

                        var compare_p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"tg_user_sn='{p_join_need.tg_user_sn}' and apply_details like '%\"dangwei\":\"{item.dangwei}\"%' and id != '{p_join_need.id}' and status!='{ModelDb.p_join_need.status_enum.已完成.ToInt()}' and status!='{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}' and status!='{ModelDb.p_join_need.status_enum.已取消.ToInt()}'", false);
                        if (!compare_p_join_need.IsNullOrEmpty())
                        {
                            foreach (var detail in compare_p_join_need.apply_details.ToModel<List<ApplyItem>>())
                            {
                                if (detail.dangwei == item.dangwei && detail.is_complete != "1")
                                {
                                    throw new Exception($"档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}已被申请,请勿重复提交同档位的申请");
                                }
                            }
                        }
                    }
                    return p_join_need;
                }


                /// <summary>
                /// 申请补人明细
                /// </summary>
                public class ApplyItem
                {
                    /// <summary>
                    /// 所属档位
                    /// </summary>
                    public string dangwei { get; set; }

                    /// <summary>
                    /// 申请人数
                    /// </summary>
                    public string count { get; set; }

                    /// <summary>
                    /// 已被分配人数
                    /// </summary>
                    public string recruited_count { get; set; }
                    /// <summary>
                    /// 流失人数
                    /// </summary>
                    public string quite_count { get; set; }
                    /// <summary>
                    /// 拉群人数
                    /// </summary>
                    public string inqun_count { get; set; }
                    /// <summary>
                    /// 是否完成此档申请
                    /// </summary>
                    public string is_complete { get; set; }
                }

                /// <summary>
                /// 用于统计所有团队的补人情况
                /// </summary>
                public class p_join_need : Entity
                {
                    public string tg_user_sn { get; set; }
                    public string yy_user_sn { get; set; }
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
                    /// 流失总数
                    /// </summary>
                    public string quit_sum { get; set; }

                    public string male_zb_sum { get; set; }
                    public string female_zb_sum { get; set; }

                    public string male_supplement_sum { get; set; }
                    public string female_supplement_sum { get; set; }

                    public string male_inqun_sum { get; set; }
                    public string female_inqun_sum { get; set; }
                }

            }
        }
        
    }
}
