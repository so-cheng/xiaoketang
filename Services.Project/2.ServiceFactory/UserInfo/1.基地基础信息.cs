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
            public class Jd
            {
                /// <summary>
                /// 根据user_sn获取中台信息
                /// </summary>
                /// <param name="user_sn">运营sn</param>
                /// <returns></returns>
                public ZtInfo GetInfoByUserSn(string user_sn)
                {
                    var yyInfo = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("jder").id}'", false).ToModel<ZtInfo>();
                    if (yyInfo.IsNullOrEmpty())
                    {
                        return new ZtInfo();
                    }
                    return yyInfo;
                }

                /// <summary>
                /// 根据id获取中台信息
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public ZtInfo GetInfoById(int id)
                {
                    var ztInfo = DoMySql.FindEntity<ModelDb.user_base>($"id='{id}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zter").id}'", false).ToModel<ZtInfo>();
                    if (ztInfo.IsNullOrEmpty())
                    {
                        return new ZtInfo();
                    }
                    return ztInfo;
                }


                /// <summary>
                /// 获取下级运营
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public List<ServiceFactory.UserInfo.Yy.YYInfo> ZtGetNextYy(string yy_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.基地邀运营, yy_user_sn).ToModel<List<ModelDb.user_base>>();
                    List<ServiceFactory.UserInfo.Yy.YYInfo> _list = new List<ServiceFactory.UserInfo.Yy.YYInfo>();
                    foreach (var item in list)
                    {
                        var yy = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(item.user_sn);
                        _list.Add(yy);
                    }
                    return _list;
                }

                /// <summary>
                /// 中台获取下级运营Sql
                /// </summary>
                /// <param name="zt_user_sn"></param>
                /// <returns></returns>
                public string ZtGetNextYyForSql(string zt_user_sn)
                {
                    return new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.基地邀运营, zt_user_sn);
                }

                /// <summary>
                /// 中台获取下级运营KV
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                public Dictionary<string, string> ZtGetNextYyForKv(string zt_user_sn)
                {
                    var list = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.基地邀运营, zt_user_sn);
                    return list;
                }

                /// <summary>
                /// 获取中台名下厅管sql
                /// </summary>
                /// <param name="zt_user_sn">中台user_sn</param>
                /// <returns></returns>
                public string ZtGetNextTgForSql(string zt_user_sn)
                {
                    return $"select t1.user_sn from user_base t1 left join user_relation t2 on t1.user_sn = t2.t_user_sn where t1.tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and t1.user_type_id = 10 and t1.status = 0 and t2.f_user_type_id = 12 and t2.f_user_sn in (select t_user_sn from user_relation where f_user_sn = '{zt_user_sn}')";
                }


                #region 统合查询
                /// <summary>
                /// 获取厅的sql语句where
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                private string GetZtBaseInfosForWhere(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id = '{new DomainBasic.UserTypeApp().GetInfoByCode("jder").id}'";

                    switch (ztBaseInfoFilter.status)
                    {
                        case ZtBaseInfoFilter.Status.全部:
                            break;
                        case ZtBaseInfoFilter.Status.正常:
                            where += $" and status = '{ModelDb.user_base.status_enum.正常.ToInt()}'";
                            break;
                        case ZtBaseInfoFilter.Status.禁用:
                            where += $" and status = '{ModelDb.user_base.status_enum.禁用.ToInt()}'";
                            break;
                        case ZtBaseInfoFilter.Status.逻辑删除:
                            where += $" and status = '{ModelDb.user_base.status_enum.逻辑删除.ToInt()}'";
                            break;
                        default:
                            break;
                    }

                    switch (ztBaseInfoFilter.attachUserType.userType)
                    {
                        case ZtBaseInfoFilter.AttachUserType.UserType.中台:
                            where += $" and zt_user_sn = '{ztBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        default:
                            break;
                    }

                    if (!ztBaseInfoFilter.attachWhere.IsNullOrEmpty())
                    {
                        where += $" and {ztBaseInfoFilter.attachWhere}";
                    }
                    return $"(select user_sn from user_base where {where})";
                }
                /// <summary>
                /// 获取厅的sql语句sql
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                public string GetZtBaseInfosForSql(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    string where = GetZtBaseInfosForWhere(ztBaseInfoFilter);
                    return $"(select user_sn from user_base where {where})";
                }
                /// <summary>
                /// 获取单条中台
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                public ZtInfo GetZtBaseInfo(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    string where = GetZtBaseInfosForWhere(ztBaseInfoFilter);

                    return DoMySql.FindList<ModelDb.user_base>(where).ToModel<ZtInfo>();
                }
                /// <summary>
                /// 获取多条中台
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ZtInfo> GetZtBaseInfos(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    string where = GetZtBaseInfosForWhere(ztBaseInfoFilter);

                    return DoMySql.FindList<ModelDb.user_base, ZtInfo>(where);
                }
                /// <summary>
                /// 获取字典格式
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetZtBaseInfosForKv(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    string where = GetZtBaseInfosForWhere(ztBaseInfoFilter);
                    return DoMySql.FindKvList<ModelDb.user_base>(where, "name,user_sn");
                }
                /// <summary>
                /// 获取选项格式
                /// </summary>
                /// <param name="ztBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetZtBaseInfosForOption(ZtBaseInfoFilter ztBaseInfoFilter)
                {
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var item in GetZtBaseInfos(ztBaseInfoFilter))
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
                public class ZtBaseInfoFilter
                {
                    /// <summary>
                    /// 自定义附加where条件
                    /// </summary>
                    public string attachWhere { get; set; }

                    public Status status { get; set; } = ZtBaseInfoFilter.Status.正常;
                    public enum Status
                    {
                        正常,
                        禁用=1,
                        逻辑删除=9,
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
                            中台,
                        }
                        public string UserSn { get; set; }
                    }
                }
                #endregion



                /// <summary>
                /// 运营实体
                /// </summary>
                public class ZtInfo : ServiceFactory.UserInfo.User.user_base
                {

                }
            }
        }

    }
}
