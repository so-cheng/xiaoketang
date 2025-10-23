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
        public class TgService
        {

            /// <summary>
            /// 获取账号名下厅管树形结构选项
            /// </summary>
            /// <param name="user_sn"></param>
            /// <returns></returns>
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


            public TgInfo GetTgInfo(string user_sn)
            {
                var tgInfo = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}'",false).ToModel<TgInfo>();
                if (tgInfo.IsNullOrEmpty()) 
                {
                    return new TgInfo();
                }
                try
                {
                    var info = tgInfo.attach3.Split('☆');
                    tgInfo.dou_username = info[0];
                    tgInfo.tg_sex = info[1];
                    tgInfo.wechat_username = info[2];
                    tgInfo.UID = info[3];
                }
                catch
                {

                }
                return tgInfo;
            }

            public class TgInfo : ModelDb.user_base
            {
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
            public List<ModelDbBasic.user_base> TgGetNextZb(string tg_user_sn)
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn);
                return list;
            }
        }
    }    
}
