using System;
using System.Data;
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
        public partial class KuaFang
        {
            public class Common
            {
                /// <summary>
                /// 取当前的跨房
                /// </summary>
                /// <returns></returns>
                public ModelDb.p_kuafang getNewKuaFang()
                {
                    return DoMySql.FindEntity<ModelDb.p_kuafang>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and start_time <= '{DateTime.Now.ToDateString("yyyy-MM-dd HH:mm:ss")}' and end_time >= '{DateTime.Now.ToDateString("yyyy-MM-dd HH:mm:ss")}'", false);
                }
            }
        }
    }
}