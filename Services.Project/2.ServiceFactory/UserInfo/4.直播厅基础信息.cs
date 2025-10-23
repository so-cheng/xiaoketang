using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.ModelDbs;
using static Services.Project.ServiceFactory.UserInfo.Yy;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {
            public class Ting
            {

                #region 根据厅管sn获取名下厅id键值对
                /// <summary>
                /// 根据厅管sn获取名下厅id键值对
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetTingidsKvByTgsn(string user_sn)
                {
                    var list = DoMySql.FindList<ModelDb.user_info_tg, TingInfo>($"tg_user_sn = '{user_sn}'");
                    var option = new Dictionary<string, string>();
                    foreach (var item in list)
                    {
                        option.Add(item.ting_name, item.id.ToString());
                    }
                    return option;
                }

                /// <summary>
                /// 根据user_sn获取直播厅下拉多选选项（获取厅id）
                /// </summary>
                /// <param name="user_sn">user_sn</param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetTingidsOptions(string user_sn)
                {
                    var option = new List<ModelDoBasic.Option>();
                    var tgs = new Tg().GetTingidTreeOption(user_sn);
                    if (tgs.IsNullOrEmpty())
                    {
                        return option;
                    }
                    foreach (var tg in tgs)
                    {
                        option.Add(new ModelDoBasic.Option()
                        {
                            text = tg.text,
                            value = tg.value,
                            disabled = tg.disabled
                        });
                    }
                    return option;
                }

                #endregion

                /// <summary>
                /// 根据user_sn获取直播厅下拉多选选项
                /// </summary>
                /// <param name="user_sn">user_sn</param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetTingsOptions(string user_sn)
                {
                    var option = new List<ModelDoBasic.Option>();
                    var tgs = new Tg().GetTreeOption(user_sn);
                    if (tgs.IsNullOrEmpty())
                    {
                        return option;
                    }
                    foreach (var tg in tgs)
                    {
                        option.Add(new ModelDoBasic.Option()
                        {
                            text = tg.text,
                            value = tg.value,
                            disabled = tg.disabled
                        });
                    }
                    return option;
                }

                /// <summary>
                /// 根据厅sn查询厅信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public TingInfo GetTingBySn(string ting_sn)
                {
                    return DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{ting_sn}'").ToModel<TingInfo>();
                }

                /// <summary>
                /// 根据自定义sql条件查询厅列表
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public List<TingInfo> GetTingsByWhere(string where)
                {
                    return DoMySql.FindList<ModelDb.user_info_tg, TingInfo>($"{where}");
                }

                /// <summary>
                /// 根据厅管sn获取名下厅列表
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public List<TingInfo> GetTingsByTgsn(string user_sn)
                {
                    return DoMySql.FindList<ModelDb.user_info_tg, TingInfo>($"tg_user_sn = '{user_sn}'");
                }

                /// <summary>
                /// 根据厅管sn获取名下厅键值对
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetTingsKvByTgsn(string user_sn)
                {
                    var list = DoMySql.FindList<ModelDb.user_info_tg, TingInfo>($"tg_user_sn = '{user_sn}'");
                    var option = new Dictionary<string, string>();
                    foreach (var item in list)
                    {
                        option.Add(item.ting_name, item.ting_sn);
                    }
                    return option;
                }

                /// <summary>
                /// 
                /// </summary>
                public class TingInfo : ModelDb.user_info_tg
                {

                }

                /// <summary>
                /// 
                /// </summary>
                public class TingFullInfo : TingInfo
                {
                    public string yy_name { get; set; }
                    public string tg_name { get; set; }
                }

                /// <summary>
                /// 修改厅名
                /// </summary>
                /// <param name="ting_sn"></param>
                /// <param name="new_ting_name"></param>
                public void UpdateTingName(string ting_sn, string new_ting_name)
                {
                    var user_info_tg = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{ting_sn}'");
                    user_info_tg.ting_name = new_ting_name;

                    user_info_tg.Update();

                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("用户信息", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), "", $"修改厅名：{new_ting_name},{ting_sn}");
                }

                /// <summary>
                /// 关厅
                /// </summary>
                /// <param name="ting_sn"></param>
                public void CloseTing(string ting_sn)
                {
                    //判断厅是否存在
                    var user_info_tg = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{ting_sn}'");
                    if (user_info_tg == null)
                    {
                        throw new Exception("厅不存在！");
                    }
                    //判断厅下时候存在主播
                    var zhubo = DoMySql.FindList<ModelDb.user_info_zhubo>($"ting_sn = '{ting_sn}' AND status = '{ModelDb.user_info_zhubo.status_enum.正常.ToSByte()}'");

                    if (zhubo.Count > 0)
                    {
                        throw new Exception("厅下存在主播，请转移主播后再关闭直播厅！");
                    }
                    //修改状态
                    user_info_tg.status = ModelDb.user_info_tg.status_enum.逻辑删除.ToSByte();
                    //user_info_tg.Update();
                    //写入日志
                    List<string> lsql = new List<string>();
                    lsql.Add(user_info_tg.UpdateTran());
                    lsql.Add(AddTingLog(ModelDb.user_info_ting_log.c_type_enum.关厅, $"关闭直播厅：{user_info_tg.ting_name},所属厅管：{new DomainBasic.UserApp().GetInfoByUserSn(user_info_tg.tg_user_sn).username}", user_info_tg));
                    DoMySql.ExecuteSqlTran(lsql);
                }

                #region 统合查询
                /// <summary>
                /// 获取厅的sql语句where
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                private string GetBaseInfosForWhere(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'";

                    switch (tingBaseInfoFilter.status)
                    {
                        case TgBaseInfoFilter.Status.全部:
                            break;
                        case TgBaseInfoFilter.Status.正常:
                            where += $" and status = '{ModelDb.user_info_tg.status_enum.正常.ToInt()}'";
                            break;
                        default:
                            break;
                    }

                    switch (tingBaseInfoFilter.attachUserType.userType)
                    {
                        case TgBaseInfoFilter.AttachUserType.UserType.基地:

                            where += $" and zt_user_sn = '{tingBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case TgBaseInfoFilter.AttachUserType.UserType.运营:
                            where += $" and yy_user_sn = '{tingBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case TgBaseInfoFilter.AttachUserType.UserType.厅管:
                            where += $" and tg_user_sn = '{tingBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case TgBaseInfoFilter.AttachUserType.UserType.厅:
                            where += $" and ting_sn = '{tingBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        default:
                            break;
                    }

                    if (!tingBaseInfoFilter.attachWhere.IsNullOrEmpty())
                    {
                        where += $" and ({tingBaseInfoFilter.attachWhere})";
                    }

                    return where;
                }

                /// <summary>
                /// 获取ting_sn的sql语句
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public string GetBaseInfosForSql(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);
                    return $"(select ting_sn from user_info_tg where {where})";
                }

                /// <summary>
                /// 获取多条厅基础信息
                /// </summary>
                /// <param name="tingBaseInfoFilter"></param>
                /// <returns></returns>
                public List<TingInfo> GetBaseInfos(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);

                    return DoMySql.FindList<ModelDb.user_info_tg, TingInfo>(where);
                }

                /// <summary>
                /// 获取字典格式厅信息
                /// </summary>
                /// <param name="tingBaseInfoFilter"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetBaseInfosForKv(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);
                    return DoMySql.FindKvList<ModelDb.user_info_tg>(where, "ting_name,ting_sn");
                }

                /// <summary>
                /// 获取多条厅基础信息
                /// </summary>
                /// <param name="tingBaseInfoFilter"></param>
                /// <returns></returns>
                public List<TingFullInfo> GetBaseFullInfos(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);

                    return DoMySql.FindListBySql<TingFullInfo>($"select *,(select name from user_base where user_sn = user_info_tg.yy_user_sn and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}) yy_name,(select name from user_base where user_sn = user_info_tg.tg_user_sn and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}) tg_name from user_info_tg where {where}");
                }

                /// <summary>
                /// 获取选项模式厅信息
                /// </summary>
                /// <param name="tgBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetBaseInfosForOption(TgBaseInfoFilter tgBaseInfoFilter)
                {
                    var options = new List<ModelDoBasic.Option>();

                    foreach (var item in GetBaseInfos(tgBaseInfoFilter))
                    {
                        options.Add(new ModelDoBasic.Option()
                        {
                            text = item.ting_name,
                            value = item.ting_sn,
                        });
                    }

                    return options;
                }
                /// <summary>
                /// 过滤条件
                /// </summary>
                public class TgBaseInfoFilter
                {
                    /// <summary>
                    /// 自定义附加where条件
                    /// </summary>
                    public string attachWhere { get; set; }

                    public Status status { get; set; } = TgBaseInfoFilter.Status.正常;
                    public enum Status
                    {
                        全部,
                        正常
                    }

                    /// <summary>
                    /// 根据上级用户类型筛选
                    /// </summary>
                    public AttachUserType attachUserType { get; set; } = new AttachUserType();
                    public class AttachUserType
                    {
                        public UserType userType { get; set; } = UserType.全部;
                        public enum UserType
                        {
                            全部,
                            基地,
                            运营,
                            厅管,
                            厅,
                        }
                        public string UserSn { get; set; }
                    }
                }
                #endregion


                /// <summary>
                /// 重置直播厅名下主播的用户关系数据
                /// </summary>
                /// <param name="ting_sn"></param>
                public void ResetZhuboRelationByTingSn(string ting_sn)
                {
                    var user_info_tg = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{ting_sn}'");
                    var l_user_info_zhubo = DoMySql.FindList<ModelDb.user_info_zhubo>($"ting_sn = '{ting_sn}'");
                    var lSql = new List<string>();
                    foreach (var zhubo in l_user_info_zhubo)
                    {
                        if (user_info_tg.tg_user_sn != zhubo.tg_user_sn)
                        {
                            lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, zhubo.tg_user_sn, zhubo.user_sn, $"换厅操作后解绑原主播关系,ting_sn:{ting_sn}，user_sn:{zhubo.user_sn}"));

                            lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_info_tg.tg_user_sn, zhubo.user_sn, $"换厅操作后绑定新主播关系,ting_sn:{ting_sn}，user_sn:{zhubo.user_sn}"));

                            zhubo.tg_user_sn = user_info_tg.tg_user_sn;
                            zhubo.yy_user_sn = user_info_tg.yy_user_sn;

                            lSql.Add(zhubo.UpdateTran());
                        }

                    }
                    MysqlHelper.ExecuteSqlTran(lSql);
                }

                #region 日志
                /// <summary>
                /// 添加直播厅日志
                /// </summary>
                /// <param name="c_type">操作类型</param>
                /// <param name="content">日志内容</param>
                /// <param name="ting_sn"></param>
                /// <returns></returns>
                public string AddTingLog(ModelDb.user_info_ting_log.c_type_enum c_type, string content, string ting_sn)
                {
                    var user_info_tg = DoMySql.FindEntity<ModelDb.user_info_tg>($"user_info_zb_sn = '{ting_sn}'");
                    return AddTingLog(c_type, content, user_info_tg);
                }

                /// <summary>
                /// 添加直播厅日志
                /// </summary>
                /// <param name="c_type">操作类型</param>
                /// <param name="content">日志内容</param>
                /// <param name="user_info_tg">厅实体</param>
                /// <returns></returns>
                public string AddTingLog(ModelDb.user_info_ting_log.c_type_enum c_type, string content, ModelDb.user_info_tg user_info_tg)
                {
                    return new ModelDb.user_info_ting_log()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        c_type = c_type.ToSByte(),
                        ting_status = user_info_tg.status.ToSByte(),
                        user_info_ting_sn = user_info_tg.ting_sn,
                        content = content,
                        user_type_id = new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id,
                        user_sn = new UserIdentityBag().user_sn,
                    }.InsertTran();
                }
                #endregion
            }
        }
    }
}
