using Services.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;

namespace TaskProject
{
    public partial class ProjectClass
    {
        /// <summary>
        /// 直播间使用记录
        /// </summary>
        /// <returns></returns>
        public string LiveRate()
        {
            //查询所有的
            var liveRoom = DoMySql.FindList<ModelDb.p_liveroom>();
            List<string> lSql = new List<string>();
            foreach (var item in liveRoom)
            {
                ModelDb.p_liveroom_userate p_Liveroom_Userate = new ModelDb.p_liveroom_userate()
                {
                    create_time = DateTime.Now.AddDays(-1),
                    modify_time = DateTime.Now.AddDays(-1),
                    live_room_id = item.id,
                    live_room_name = item.name,
                    tenant_id = 1,
                    zt_user_sn = item.zt_user_sn,
                    status = item.status
                };
                lSql.Add(p_Liveroom_Userate.InsertOrUpdateTran());
            }
            DoMySql.ExecuteSqlTran(lSql);
            return "直播间使用记录";
        }
    }
}
