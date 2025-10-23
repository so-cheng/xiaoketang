using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class LiveRoomLogService
        {
            /// <summary>
            /// 直播间操作日志
            /// </summary>
            /// <param name="need_id"></param>
            /// <param name="e"></param>
            /// <param name="content"></param>
            public void AddLog(Enum e, string content = "")
            {
                new ModelDb.p_liveroom_log()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    zt_user_sn = new UserIdentityBag().user_sn,
                    content = content,
                    c_type = e.ToSByte(),
                }.Insert();
            }
        }
    }
}
