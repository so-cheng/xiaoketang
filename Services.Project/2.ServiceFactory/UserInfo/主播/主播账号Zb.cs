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

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public class Zb11111
            {
                /// <summary>
                /// 获取主播信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public ZbInfo GetZbInfo(string user_sn)
                {
                    var zbInfo = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zber").id}'", false).ToModel<ZbInfo>();
                    if (zbInfo.IsNullOrEmpty())
                    {
                        return new ZbInfo();
                    }
                    return zbInfo;
                }

                /// <summary>
                /// 获取主播信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public ZbInfo GetZbInfoById(int id)
                {
                    var zbInfo = DoMySql.FindEntity<ModelDb.user_base>($"id='{id}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zber").id}'", false).ToModel<ZbInfo>();
                    if (zbInfo.IsNullOrEmpty())
                    {
                        return new ZbInfo();
                    }
                    return zbInfo;
                }

                #region 统合查询
                public List<ZbBaseInfo> GetZbBaseInfos(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string where = "1=1";
                    return DoMySql.FindList<ModelDb.user_info_zhubo,ZbBaseInfo>(where);
                }
                public string GetZbBaseInfosForSql(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'";
                    if (!zbBaseInfoFilter.status.IsNullOrEmpty())
                    {
                        where += $" and status = '{zbBaseInfoFilter.status}'";
                    }
                    return $"(select user_sn from user_base where {where})";
                }
                public Dictionary<string,string> GetZbBaseInfosForKv(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (var item in GetZbBaseInfos(zbBaseInfoFilter))
                    {
                        dictionary.Add(item.username,item.user_sn);
                    }
                    return dictionary;
                }
                public List<ModelDoBasic.Option> GetZbBaseInfosForOption(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var item in GetZbBaseInfos(zbBaseInfoFilter))
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
                public class ZbBaseInfoFilter
                {
                    /// <summary>
                    /// 运营下的主播
                    /// </summary>
                    public sbyte status { get; set; }
                }
                #endregion

                /// <summary>
                /// 主播详细信息实体类
                /// </summary>
                public class ZbInfo : ModelDb.user_base
                {
                }

                /// <summary>
                /// 主播基础信息实体类
                /// </summary>
                public class ZbBaseInfo : ModelDb.user_base
                {
                }
            }
        }
    }
}
