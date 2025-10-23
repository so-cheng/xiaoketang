using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {
            public class Relation
            {
                /// <summary>
                /// 获取账号名下厅管树形结构选项
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                [Obsolete("该方法已废弃，将在下一版本移除，使用ServiceFactory.UserInfo.Tg().GetTreeOption方法", true)]
                public List<ModelDoBasic.Option> GetTreeOption(string user_sn)
                {
                    var option = new List<ModelDoBasic.Option>();
                    var tgs = GetDirectNextTgs(user_sn);
                    if (tgs.IsNullOrEmpty())
                    {
                        return option;
                    }
                    foreach (var tg in tgs)
                    {
                        option.Add(new ModelDoBasic.Option()
                        {
                            text = tg.username,
                            value = tg.user_sn
                        });
                        option.AddRange(new DomainUserBasic.UserRelationApp().GetNextAllUsersForOption(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg.user_sn, "····"));
                    }
                    return option;
                }

                [Obsolete("该方法已废弃，将在下一版本移除，使用ServiceFactory.UserInfo.Tg().GetTreeOptionDic方法", true)]
                public Dictionary<string, string> GetTreeOptionDic(string user_sn)
                {
                    var option = new Dictionary<string, string>();
                    foreach (var item in GetTreeOption(user_sn))
                    {
                        option.Add(item.text, item.value);
                    }
                    return option;
                }

                /// <summary>
                /// 获取用户直接下级厅管
                /// </summary>
                /// <param name="yy_user_sn"></param>
                /// <returns></returns>
                [Obsolete("该方法已废弃，将在下一版本移除，使用ServiceFactory.UserInfo.Tg().GetDirectNextTgs方法", true)]
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
            }
        }
    }
    
    
}
