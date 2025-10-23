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
        public class ResumeService
        {
            public List<ZpInfo> GetXianXiaZpinfo(DateTime c_date)
            {
                var where = $"yy_interview_date between '{c_date}' and '{c_date.AddDays(1).AddSeconds(-1)}'";
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $" and jd_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                    case ModelEnum.UserTypeEnum.zter:
                        where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                    case ModelEnum.UserTypeEnum.wxer:
                        where += $" and wx_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                    case ModelEnum.UserTypeEnum.manager:
                        break;
                    default:
                        where += $" and 1!=1";
                        break;
                }

                var zpInfoResult = DoMySql.FindList<ModelDb.p_xianxiazp_info>(where);

                List<ZpInfo> zpinfoData = new List<ZpInfo>();
                foreach (var item in zpInfoResult)
                {
                    zpinfoData.Add(new ZpInfo()
                    {
                        count = 0,
                        c_date = item.c_date,
                        range = GetTimePeriod(item.yy_interview_date.ToDateTime())
                    });
                }
                List<ZpInfo> newzpinfo = new List<ZpInfo>();
                for (int i = 0; i < timePeriods.Length; i++)
                {
                    var zpInfo = new ZpInfo();
                    zpInfo.range = timePeriods[i];
                    zpInfo.count = zpinfoData.Count(t => t.range == zpInfo.range);
                    newzpinfo.Add(zpInfo);
                }
                return newzpinfo;
            }
            public class ZpInfo
            {
                public object c_date { get; set; }
                public object range { get; set; }

                public object count { get; set; }
            }

            string[] timePeriods = { "0-3点", "3-6点", "6-9点", "9-12点", "12-15点", "15-18点", "18-21点", "21-24点" };

            /// <summary>
            /// 日期转换为时间段
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public string GetTimePeriod(DateTime dateTime)
            {
                int periodIndex = dateTime.Hour / 3;

                if (periodIndex >= 0 && periodIndex < timePeriods.Length)
                    return timePeriods[periodIndex];
                else
                    return "未知时间段";
            }
        }
    }

}
