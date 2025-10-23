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
            public class All
            {
                //// <summary>
                /// 获取所有的运营信息user_base
                /// </summary>
                /// <returns></returns>
                public List<ModelDb.user_base> GetAllYyForList()
                {
                    return GetAllListByCode("yyer");
                }

                //// <summary>
                /// 获取所有的厅管信息user_base
                /// </summary>
                /// <returns></returns>
                public List<ModelDb.user_base> GetAllTgForList()
                {
                    return GetAllListByCode("tger");
                }

                //// <summary>
                /// 获取所有的主播信息user_base
                /// </summary>
                /// <returns></returns>
                public List<ModelDb.user_base> GetAllZbForList()
                {
                    return GetAllListByCode("zber");
                }

                /// <summary>
                /// 根据角色获取所有用户
                /// </summary>
                /// <param name="code"></param>
                /// <returns></returns>
                public List<ModelDb.user_base> GetAllListByCode(string code)
                {
                    var list = DoMySql.FindList<ModelDb.user_base>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode(code).id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()}");
                    return list;
                }
            }
        }
    }
}