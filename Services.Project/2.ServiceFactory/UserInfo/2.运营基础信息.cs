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
using static Services.Project.ServiceFactory.UserInfo.Zhubo;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {
            public class Yy
            {
                /// <summary>
                /// 根据user_sn获取运营信息
                /// </summary>
                /// <param name="user_sn">运营sn</param>
                /// <returns></returns>
                public YYInfo GetInfoByUserSn(string user_sn)
                {
                    var yyInfo = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", false).ToModel<YYInfo>();
                    if (yyInfo.IsNullOrEmpty())
                    {
                        return new YYInfo();
                    }
                    return yyInfo;
                }

                /// <summary>
                /// 根据id获取运营信息
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public YYInfo GetInfoById(int id)
                {
                    var yyInfo = DoMySql.FindEntity<ModelDb.user_base>($"id='{id}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", false).ToModel<YYInfo>();
                    if (yyInfo.IsNullOrEmpty())
                    {
                        return new YYInfo();
                    }
                    return yyInfo;
                }

                /// <summary>
                /// 获取运营名下某天的请假主播
                /// </summary>
                /// <param name="yy_user_sn">运营sn</param>
                /// <param name="date">指定日期</param>
                /// <returns></returns>
                public List<ModelDb.user_base> GetZbVacation(string yy_user_sn, DateTime date)
                {
                    var tg_list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);

                    var list = new List<ModelDb.user_base>();
                    foreach (var tg in tg_list)
                    {
                        list.AddRange(DoMySql.FindList<ModelDb.user_base>($"user_sn in (select zb_user_sn from p_jixiao_vacation where c_date = '{date.ToString("yyyy-MM-dd")}' and user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg.user_sn)}) and status = '{ModelDb.user_base.status_enum.正常}'"));
                    }
                    return list;
                }

                /// <summary>
                /// 运营获取下级厅管
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<ServiceFactory.UserInfo.Tg.TgInfo> YyGetNextTg(string yy_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn).ToModel<List<ModelDb.user_base>>();
                    List<ServiceFactory.UserInfo.Tg.TgInfo> _list = new List<Tg.TgInfo>();
                    foreach (var item in list)
                    {
                        var tg = new ServiceFactory.UserInfo.Tg().GetTgInfoByUserBase(item);
                        _list.Add(tg);
                    }
                    return _list;
                }

                /// <summary>
                /// 运营获取下级厅管Sql
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public string YyGetNextTgForSql(string yy_user_sn)
                {
                    return new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                }

                /// <summary>
                /// 运营获取下级厅管KV
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public Dictionary<string, string> YyGetNextTgForKv(string yy_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                    return list;
                }

                /// <summary>
                /// 获取所有的运营KV
                /// </summary>
                /// <returns></returns>
                public Dictionary<string, string> GetAllYyForKv()
                {
                    var Dic = new Dictionary<string, string>();

                    var list = GetAllYyForList();
                    foreach (var item in list)
                    {
                        Dic.Add(item.username, item.user_sn);
                    }
                    return Dic;
                }

                /// <summary>
                /// 获取所有的运营信息
                /// </summary>
                /// <returns></returns>
                public List<ModelDbBasic.user_base> GetAllYyForList()
                {
                    return new DomainBasic.UserApp().GetInfosByEntityWhere(new DomainBasic.UserApp.EntityWhere
                    {
                        userTypeEnum = ModelEnum.UserTypeEnum.yyer
                    });
                }

                /// <summary>
                /// 获取关联运营kv
                /// </summary>
                /// <returns></returns>
                public Dictionary<string, string> GetWithYyForKv(string f_yy_user_sn)
                {
                    var Dic = new Dictionary<string, string>();

                    var list = DoMySql.FindList<ModelDb.user_info_yy_with>($"f_yy_user_sn='{f_yy_user_sn}'");
                    var yy = GetInfoByUserSn(f_yy_user_sn);
                    Dic.Add(yy.username,yy.user_sn);
                    foreach (var item in list)
                    {
                        foreach (var t_yy in item.t_yy_user_sns.Split(','))
                        {
                            Dic.Add(new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(t_yy).username,t_yy);
                        }
                    }
                    return Dic;
                }






                #region 统合查询
                /// <summary>
                /// 获取运营的sql语句where
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                private string GetBaseInfosForWhere(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'";

                    switch (yyBaseInfoFilter.status)
                    {
                        case YyBaseInfoFilter.Status.全部:
                            break;
                        case YyBaseInfoFilter.Status.正常:
                            where += $" and status = '{ModelDb.user_base.status_enum.正常.ToInt()}'";
                            break;
                        case YyBaseInfoFilter.Status.禁用:
                            where += $" and status = '{ModelDb.user_base.status_enum.禁用.ToInt()}'";
                            break;
                        case YyBaseInfoFilter.Status.逻辑删除:
                            where += $" and status = '{ModelDb.user_base.status_enum.逻辑删除.ToInt()}'";
                            break;
                        default:
                            break;
                    }

                    switch (yyBaseInfoFilter.attachUserType.userType)
                    {
                        case YyBaseInfoFilter.AttachUserType.UserType.基地:
                            where += $" and user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.基地邀运营, yyBaseInfoFilter.attachUserType.UserSn)}";
                            break;
                        case YyBaseInfoFilter.AttachUserType.UserType.运营:
                            where += $" and user_sn = '{yyBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        default:
                            break;
                    }

                    if (!yyBaseInfoFilter.attachWhere.IsNullOrEmpty())
                    {
                        where += $" and ({yyBaseInfoFilter.attachWhere})";
                    }

                    return where;
                }

                /// <summary>
                /// 获取运营的sql语句
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                public string GetYyBaseInfosForSql(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(yyBaseInfoFilter);
                    return $"(select user_sn from user_base where {where})";
                }

                /// <summary>
                /// 获取单条运营基本信息
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                public YyBaseInfo GetBaseInfo(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(yyBaseInfoFilter);
                    return DoMySql.FindEntity<ModelDb.user_base>(where).ToModel<YyBaseInfo>();
                }
                /// <summary>
                /// 获取多条运营基本信息
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                public List<YyBaseInfo> GetBaseInfos(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(yyBaseInfoFilter);
                    return DoMySql.FindList<ModelDb.user_base, YyBaseInfo>(where);
                }
                
                /// <summary>
                /// 获取运营基本信息字典格式
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetBaseInfosForKv(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(yyBaseInfoFilter);
                    return DoMySql.FindKvList<ModelDb.user_base>(where, "name,user_sn");
                }
                /// <summary>
                /// 获取运营基本信息选项格式
                /// </summary>
                /// <param name="yyBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetBaseInfosForOption(YyBaseInfoFilter yyBaseInfoFilter)
                {
                    var options = new List<ModelDoBasic.Option>();

                    foreach (var item in GetBaseInfos(yyBaseInfoFilter))
                    {
                        options.Add(new ModelDoBasic.Option()
                        {
                            text = item.name,
                            value = item.user_sn,
                        });
                    }

                    return options;
                }
                /// <summary>
                /// 过滤条件
                /// </summary>
                public class YyBaseInfoFilter
                {
                    /// <summary>
                    /// 自定义附加where条件
                    /// </summary>
                    public string attachWhere { get; set; }

                    public Status status { get; set; } = YyBaseInfoFilter.Status.正常;
                    public enum Status
                    {
                        正常,
                        禁用 = 1,
                        逻辑删除 = 9,
                        全部,
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
                            运营
                        }
                        public string UserSn { get; set; }
                    }
                }
                #endregion



                /// <summary>
                /// 获取运营下所有主播
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<ServiceFactory.UserInfo.Zhubo.ZbBaseInfo> YyGetZbInfo(string yy_user_sn)
                {

                    var tgList = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn).ToModel<List<ServiceFactory.UserInfo.Tg.TgBaseInfo>>();

                    List<ServiceFactory.UserInfo.Zhubo.ZbBaseInfo> zbList = new List<Zhubo.ZbBaseInfo>();
                    foreach (var tg in tgList)
                    {
                        zbList.AddRange(new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg.user_sn).ToModel<List<ServiceFactory.UserInfo.Zhubo.ZbBaseInfo>>());
                    }

                    return zbList;
                }

                /// <summary>
                /// 删除运营（将运营状态改为逻辑删除）
                /// </summary>
                /// <param name="yy_user_sn"></param>
                public void DeleteYy(string yy_user_sn)
                {
                    var user_base = new DomainBasic.UserApp().GetInfoByUserSn(yy_user_sn);
                    var lSql = new List<string>();
                    user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                    lSql.Add(user_base.UpdateTran());

                    MysqlHelper.ExecuteSqlTran(lSql);
                }

                /// <summary>
                /// 运营实体
                /// </summary>
                public class YYInfo : ServiceFactory.UserInfo.User.user_base
                {
                    public string zt_user_sn
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.基地邀运营, user_sn);
                        }
                    }

                    public string img_url
                    {
                        get
                        {
                            return attach1;
                        }
                    }
                }


                /// <summary>
                /// 运营实体
                /// </summary>
                public class YyBaseInfo : ServiceFactory.UserInfo.User.user_base
                {

                }
            }
        }
    }
}
