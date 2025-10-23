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
            public class Tg
            {

                #region 统合查询
                /// <summary>
                /// 获取厅的sql语句where
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                private string GetBaseInfosForWhere(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id = '{ModelEnum.UserTypeEnum.tger.ToSByte()}'";

                    switch (tingBaseInfoFilter.status)
                    {
                        case TgBaseInfoFilter.Status.全部:
                            break;
                        case TgBaseInfoFilter.Status.正常:
                            where += $" and status = '{ModelDb.user_info_tg.status_enum.正常.ToInt()}'";
                            break;
                        case TgBaseInfoFilter.Status.逻辑删除:
                            where += $" and status = '{ModelDb.user_info_tg.status_enum.逻辑删除.ToInt()}'";
                            break;
                        default:
                            break;
                    }

                    switch (tingBaseInfoFilter.attachUserType.userType)
                    {
                        case TgBaseInfoFilter.AttachUserType.UserType.基地:
                        case TgBaseInfoFilter.AttachUserType.UserType.运营:
                        case TgBaseInfoFilter.AttachUserType.UserType.厅管:
                            where += $" and attach5 like '%{tingBaseInfoFilter.attachUserType.UserSn}%'";
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
                /// 获取user_sn的sql语句
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public string GetBaseInfosForSql(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    new ServiceFactory.UserInfo.Tg().GetBaseInfos(new ServiceFactory.UserInfo.Tg.TgBaseInfoFilter()
                    {
                        attachUserType = new TgBaseInfoFilter.AttachUserType()
                        {
                            userType = TgBaseInfoFilter.AttachUserType.UserType.基地,
                            UserSn = ""
                        },
                        attachWhere = ""
                    });
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);
                    return $"(select user_sn from user_base where {where})";
                }

                /// <summary>
                /// 获取多条厅基础信息
                /// </summary>
                /// <param name="tingBaseInfoFilter"></param>
                /// <returns></returns>
                public List<TgInfo> GetBaseInfos(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);

                    return DoMySql.FindList<ModelDb.user_base, TgInfo>(where);
                }

                /// <summary>
                /// 获取字典格式厅信息
                /// </summary>
                /// <param name="tingBaseInfoFilter"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetBaseInfosForKv(TgBaseInfoFilter tingBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(tingBaseInfoFilter);
                    return DoMySql.FindKvList<ModelDb.user_base>(where, "username,user_sn");
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
                            text = item.username,
                            value = item.user_sn,
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
                        正常 = 0,
                        逻辑删除 = 9,
                        全部 = 10
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
                            基地 = 17,
                            运营 = 12,
                            厅管 = 10,
                        }
                        public string UserSn { get; set; }
                    }
                }
                #endregion

                #region 获取账号名下厅管树形结构选项（获取厅id）
                /// <summary>
                /// 获取账号名下厅管树形结构选项（获取厅id）
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetTingidTreeOption(string user_sn)
                {
                    var options = new List<ModelDoBasic.Option>();
                    var tgs = GetDirectNextTgs(user_sn);
                    if (tgs.IsNullOrEmpty()) return options;
                    foreach (var tg in tgs)
                    {
                        options.Add(new ModelDoBasic.Option()
                        {
                            text = tg.name,
                            value = tg.user_sn,
                            disabled = true
                        });

                        //直属厅
                        var tings = new Ting().GetTingidsKvByTgsn(tg.user_sn);
                        foreach (var item in tings)
                        {
                            options.Add(new ModelDoBasic.Option()
                            {
                                text = "♫ " + item.Key,
                                value = item.Value
                            });
                        }

                        var tgslist = new DomainUserBasic.UserRelationApp()
                            .GetNextAllUsersForOption(
                            ModelEnum.UserRelationTypeEnum.厅管邀厅管,
                            tg.user_sn,
                            "····",
                            GetTingids, true);
                        foreach (var item in tgslist)
                        {
                            options.Add(item);
                        }
                    }
                    return options;
                }
                private List<ModelDoBasic.Option> GetTingids(string user_sn, string level_str)
                {
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var ting in DoMySql.FindList<ModelDb.user_info_tg>($"tg_user_sn = '{user_sn}'"))
                    {
                        options.Add(new ModelDoBasic.Option
                        {
                            text = level_str + @"♫ " + ting.ting_name,
                            value = ting.id.ToString(),
                        });
                    }
                    return options;
                }

                #endregion

                /// <summary>
                /// 获取账号名下厅管树形结构选项
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetTreeOption(string user_sn)
                {
                    var options = new List<ModelDoBasic.Option>();
                    var tgs = GetDirectNextTgs(user_sn);
                    if (tgs.IsNullOrEmpty()) return options;
                    foreach (var tg in tgs)
                    {
                        options.Add(new ModelDoBasic.Option()
                        {
                            text = tg.name,
                            value = "",
                            disabled = true
                        });

                        //直属厅
                        var tings = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(tg.user_sn);
                        foreach (var item in tings)
                        {
                            options.Add(new ModelDoBasic.Option()
                            {
                                text = "♫ " + item.Key,
                                value = item.Value
                            });
                        }

                        var tgslist = new DomainUserBasic.UserRelationApp()
                            .GetNextAllUsersForOption(
                            ModelEnum.UserRelationTypeEnum.厅管邀厅管,
                            tg.user_sn,
                            "····",
                            GetTings, true);
                        foreach (var item in tgslist)
                        {
                            options.Add(item);
                        }
                    }
                    return options;
                }
                private List<ModelDoBasic.Option> GetTings(string user_sn, string level_str)
                {
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var ting in DoMySql.FindList<ModelDb.user_info_tg>($"tg_user_sn = '{user_sn}'"))
                    {
                        options.Add(new ModelDoBasic.Option
                        {
                            text = level_str + @"♫ " + ting.ting_name,
                            value = ting.ting_sn,
                        });
                    }
                    return options;
                }

                /// <summary>
                /// 获取账号名下厅管树形结构返回Sql语句
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public string GetTreeOptionForSql(string user_sn)
                {
                    var options = "";
                    var tgs = GetDirectNextTgs(user_sn);
                    if (tgs.IsNullOrEmpty()) return "";
                    foreach (var tg in tgs)
                    {
                        options += $"'{tg.user_sn}',";

                        var tgslist = new DomainUserBasic.UserRelationApp()
                            .GetNextAllUsersForOption(
                            ModelEnum.UserRelationTypeEnum.厅管邀厅管,
                            tg.user_sn,
                            "····");
                        foreach (var item in tgslist)
                        {
                            options += $"'{item.value}',";
                        }
                    }
                    return $"({options.TrimEnd(',')})";
                }

                public Dictionary<string, string> GetTreeOptionDic(string user_sn)
                {
                    var option = new Dictionary<string, string>();
                    foreach (var item in GetTreeOption(user_sn))
                    {
                        if (option.ContainsKey(item.text)) continue;
                        option.Add(item.text, item.value);
                    }
                    return option;
                }

                /// <summary>
                /// 获取用户直接下级厅管
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                private List<ModelDbBasic.user_base> GetDirectNextTgs(string user_sn)
                {
                    var resultUsers = new List<ModelDbBasic.user_base>();
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{user_sn}'", false);
                    if (user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        resultUsers.Add(new DomainBasic.UserApp().GetInfoByUserSn(user_sn));
                        return resultUsers;
                    }
                    if (user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id)
                    {
                        //1.获取运营的所有厅管
                        var allTgs = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, user_sn);
                        //2.找到直接上级只有一个且上级是该运营的厅管
                        foreach (var tg in allTgs)
                        {
                            //2.1 获取厅管的直接上级厅管
                            string parentTg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg.user_sn);
                            //2.2 判断是否为运营
                            if (parentTg_user_sn.IsNullOrEmpty())
                            {
                                resultUsers.Add(tg);
                            }
                        }
                        return resultUsers;
                    }
                    return null;
                }

                /// <summary>
                /// 根据user_sn获取厅管信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public TgInfo GetTgInfoByUsersn(string user_sn)
                {
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}'", false);
                    return GetInfoByEntity(user_base);
                }

                /// <summary>
                /// 根据id获取厅管信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public TgInfo GetInfoById(int id)
                {
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"id='{id}' and user_type_id= '{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}'", false);
                    return GetInfoByEntity(user_base);
                }

                /// <summary>
                /// 根据user_base获取厅管信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public TgInfo GetTgInfoByUserBase(ModelDb.user_base user_base)
                {
                    return GetInfoByEntity(user_base);
                }

                /// <summary>
                /// 根据user_base实体查询TgInfo类
                /// </summary>
                /// <param name="user_Base"></param>
                /// <returns></returns>
                private TgInfo GetInfoByEntity(ModelDbBasic.user_base user_base)
                {
                    TgInfo tgInfo = user_base.ToModel<TgInfo>();
                    if (tgInfo.IsNullOrEmpty())
                    {
                        return new TgInfo();
                    }

                    var info = (tgInfo.attach3 + "☆☆☆☆☆").Split('☆');
                    tgInfo.dou_username = info[0];
                    tgInfo.tg_sex = info[1];
                    tgInfo.wechat_username = info[2];
                    tgInfo.UID = info[3];
                    tgInfo.weixin_qrcode = info[4];
                    return tgInfo;
                }

                /// <summary>
                /// 厅管获取下级厅管
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<ModelDbBasic.user_base> TgGetNextTg(string tg_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg_user_sn);
                    return list;
                }

                /// <summary>
                /// 厅管获取下级主播
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<ServiceFactory.UserInfo.Zhubo.ZbBaseInfo> TgGetNextZb(string tg_user_sn)
                {
                    var _list = DoMySql.FindList<ModelDb.user_info_zhubo, ServiceFactory.UserInfo.Zhubo.ZbBaseInfo>($"ting_sn = '{tg_user_sn}' AND status = '{ModelDb.user_info_zhubo.status_enum.正常.ToSByte()}'");
                    return _list;
                }

                /// <summary>
                /// 获取厅管离职率
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public int GetLeaveRate(string tg_user_sn)
                {
                    int leave = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn, DomainUserBasic.UserRelationApp.GetNextUsersType.仅删除).Count;

                    int all = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn, DomainUserBasic.UserRelationApp.GetNextUsersType.正常和删除).Count;

                    return all <= 0 ? 0 : (100 * leave / all);
                }

                /// <summary>
                /// 获取厅管当前申请主播总数(补人申请重构后废弃)
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public int GetApplyZbCount(string tg_user_sn)
                {
                    var list = DoMySql.FindList<ModelDb.p_join_need>($"tg_user_sn = '{tg_user_sn}' and status != '{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}' and status != '{ModelDb.p_join_need.status_enum.已完成.ToInt()}' and status != '{ModelDb.p_join_need.status_enum.已取消.ToInt()}'");
                    int count = 0;
                    foreach (var item in list)
                    {
                        count += item.zb_count.ToInt();
                    }
                    return count;
                }

                /// <summary>
                /// 获取厅管当前申请主播总数
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public int GetApplyZbCountNew(string tg_user_sn)
                {
                    var list = DoMySql.FindList<ModelDb.p_join_apply>($"tg_user_sn = '{tg_user_sn}' and status != '{ModelDb.p_join_apply.status_enum.已拒绝.ToInt()}' and status != '{ModelDb.p_join_apply.status_enum.已完成.ToInt()}' and status != '{ModelDb.p_join_apply.status_enum.已取消.ToInt()}'");
                    int count = 0;
                    foreach (var item in list)
                    {
                        count += item.zb_count.ToInt();
                    }
                    return count;
                }

                /// <summary>
                /// 厅管获取下级直属厅管SQL
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public string TgGetNextTgForSql(string tg_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg_user_sn);
                    return list;
                }

                /// <summary>
                /// 厅管获取下级所有厅管SQL
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public string TgGetNextAllTgForSql(string tg_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg_user_sn);
                    return list;
                }

                /// <summary>
                /// 厅管获取下级所有直属主播SQL
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <returns></returns>
                public string TgGetNextZbForSql(string tg_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn);
                    return list;
                }

                /// <summary>
                /// 厅管获取下级主播KV
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public Dictionary<string, string> TgGetNextZbForKv(string tg_user_sn)
                {
                    return new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn);
                }

                /// <summary>
                /// 获取所有当前所有在职厅管的sn
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<TgBaseInfo> GetAllTg()
                {
                    return DoMySql.FindListBySql<TgBaseInfo>($"select user_sn from user_base where tenant_id = 1 and user_type_id = 10 and status = 0");
                }

                /// <summary>
                /// 删除厅管(设置状态为逻辑删除)
                /// </summary>
                /// <param name="tg_user_sn"></param>
                public void DeleteTg(string tg_user_sn)
                {
                    var user_base = new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn);
                    var lSql = new List<string>();
                    if (user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id && new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn).Count > 0)
                    {
                        throw new Exception("禁止删除名下有主播的厅管");
                    }
                    user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                    lSql.Add(user_base.UpdateTran());

                    MysqlHelper.ExecuteSqlTran(lSql);

                }
                /// <summary>
                /// 厅管离职
                /// </summary>
                /// <param name="tg_user_sn"></param>
                public void TgResign(string tg_user_sn)
                {
                    var user_tg = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{tg_user_sn}'");
                    //判断账户信息
                    if (user_tg == null)
                    {
                        throw new Exception("当前厅管账号不存在！");
                    }

                    if (new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn).Count > 0)
                    {
                        throw new Exception("当前厅下存在直播厅，请转移直播厅！");
                    }
                    //修改厅管状态为离职
                    user_tg.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                    user_tg.Update();
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.用户模块.ToSByte(), new UserIdentityBag().user_sn, $"厅管离职：{new ServiceFactory.UserInfo.Tg().GetInfoById(user_tg.id).name},{user_tg.user_sn}");
                }


                #region 厅管升运营
                /// <summary>
                /// 厅管晋升运营
                /// </summary>
                /// <param name="tg_user_sn"></param>
                public void TgPromotionYy(string tg_user_sn, string tg_username, string password)
                {

                    var tg_userinfo = DoMySql.FindEntity<ModelDb.user_info_tingguan>($"tg_user_sn = '{tg_user_sn}'");
                    var tg_userbase = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{tg_user_sn}'");
                    //查询当前厅管是否存在
                    if (tg_userinfo == null && tg_userbase == null)
                    {
                        throw new Exception("当前厅管不存在");
                    }
                    List<string> lSql = new List<string>();
                    //创建运营账号
                    string user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    ModelDbBasic.user_base user_Base = new ModelDbBasic.user_base()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        username = tg_username,
                        mobile = tg_userbase.mobile,
                        password = password,
                        user_sn = user_sn,
                        name = tg_username,
                        user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id.ToSByte()
                    };
                    lSql.Add(new DomainBasic.UserApp().Create(user_Base, true));
                    //创建运营信息
                    string yy_user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    ModelDb.user_info_yunying yunying_info = new ModelDb.user_info_yunying()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        yy_user_sn = yy_user_sn,
                    };
                    lSql.Add(yunying_info.InsertTran());
                    //修改当前厅管所属运营
                    var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"t_user_sn = '{tg_user_sn}'");
                    if (user_relation != null)
                    {
                        user_relation.f_user_sn = yy_user_sn;
                        lSql.Add(user_relation.UpdateTran());
                    }
                    tg_userinfo.yy_user_sn = yy_user_sn;
                    lSql.Add(tg_userinfo.UpdateTran());
                    //查询厅管下所有直播厅和主播修改厅和主播的yy_user_sn
                    var tinglist = DoMySql.FindList<ModelDb.user_info_tg>($"tg_user_sn = '{tg_user_sn}'");
                    var zhubolist = DoMySql.FindList<ModelDb.user_info_zhubo>($"tg_user_sn = '{tg_user_sn}'");
                    //修改直播厅所属运营
                    foreach (var item in tinglist)
                    {
                        item.yy_user_sn = yy_user_sn;
                        lSql.Add(item.UpdateTran());
                    }
                    //修改主播所属运营
                    foreach (var item in zhubolist)
                    {
                        item.yy_user_sn = yy_user_sn;
                        lSql.Add(item.UpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                }


                #endregion


                /// <summary>
                /// 厅管详细实体信息
                /// </summary>
                public class TgInfo : ModelDb.user_base
                {
                    /// <summary>
                    /// 所属运营sn
                    /// </summary>
                    public string yy_sn
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, user_sn);
                        }
                    }
                    public string zt_sn
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.基地邀运营, yy_sn);
                        }
                    }
                    /// <summary>
                    /// 上级厅管(可选)
                    /// </summary>
                    public string f_user_sn
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, user_sn);
                        }
                    }
                    public string img_url
                    {
                        get
                        {
                            return attach1;
                        }
                    }

                    /// <summary>
                    /// 抖音大头号
                    /// </summary>
                    public string dou_username { get; set; }
                    /// <summary>
                    /// 男女厅
                    /// </summary>
                    public string tg_sex { get; set; }
                    /// <summary>
                    /// 管理微信号
                    /// </summary>
                    public string wechat_username { get; set; }
                    /// <summary>
                    /// UID
                    /// </summary>
                    public string UID { get; set; }

                    /// <summary>
                    /// 目前在开档
                    /// </summary>
                    public string dangwei { get; set; }

                    /// <summary>
                    /// 微信二维码
                    /// </summary>
                    public string weixin_qrcode { get; set; }
                }

                /// <summary>
                /// 厅管基础实体信息
                /// </summary>
                public class TgBaseInfo : Entity
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string username { get; set; }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                }
            }
        }
    }

}
