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
            public class Zb_NewZhubo
            {

                /// <summary>
                /// 创建新主播
                /// </summary>
                /// <returns></returns>
                public void CreateNewZhubo(string sources_name, string sources_memo, ZhuboInfoDto zhuboInfo)
                {
                    if (sources_name.IsNullOrEmpty()) throw new WeicodeException("主播来源名称不能为空");
                    if (sources_memo.IsNullOrEmpty()) throw new WeicodeException("主播来源备注信息不能为空");
                    string user_info_zb_sn = UtilityStatic.CommonHelper.CreateUniqueSn();

                    //新主播数据插入数据库
                    zhuboInfo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    var user_info_zhubo = zhuboInfo.ToModel<ModelDb.user_info_zhubo>();
                    user_info_zhubo.user_info_zb_sn = user_info_zb_sn;
                    user_info_zhubo.sources_name = sources_name;
                    user_info_zhubo.wechat_nickname = zhuboInfo.wechat_nickname;
                    user_info_zhubo.dou_username = zhuboInfo.dou_username;
                    user_info_zhubo.dou_nickname = zhuboInfo.dou_nickname;
                    user_info_zhubo.sources_memo = sources_memo + "(/UserInfo/主播/新的主播.CreateNewZhubo.38)";
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.待开账号.ToSByte();

                    List<string> lSql = new List<string>();
                    lSql.Add(user_info_zhubo.InsertTran());

                    lSql.Add(new Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.新的主播,
                        "新增了一个新的主播",
                        user_info_zhubo));
                    DoMySql.ExecuteSqlTran(lSql);
                }

                public class ZhuboInfoDto:ModelDb.user_info_zhubo
                {
                }
            }
        }
    }
}
