using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.ModelDbs;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class JoinChangeTable : ModelDb.user_info_zb
        {
            /// <summary>
            /// 待开账号:微信账号
            /// </summary>
            public object username { get; set; }
            /// <summary>
            /// 辨识数据来源(待开账号/主播列表)
            /// </summary>
            public object flag { get; set; }
        }

        public class JoinChangeService 
        {
            /// <summary>
            /// 通过唯一键获取主播名称(微信账号)
            /// </summary>
            /// <param name="user_info_zb_sn"></param>
            /// <returns></returns>
            public List<JoinChangeTable> GetUsernameByUniqueKey(string user_info_zb_sn)
            {
                return DoMySql.FindListBySql<ServiceFactory.JoinChangeTable>($"select case when t1.wechat_username is null then '' else t1.wechat_username end as username,t1.user_sn,t1.dou_username from user_info_zb t1 where t1.user_info_zb_sn = '{user_info_zb_sn}'");
            }

        }

            
    }
}
