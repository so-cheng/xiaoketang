using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class Join
        {
            public class dyCheckParam : Entity
            {
                /// <summary>
                /// 抖音账号
                /// </summary>
                public object dou_username { get; set; }
                /// <summary>
                /// 手机后四位
                /// </summary>
                public object last_four_number { get; set; }
                /// <summary>
                /// 真实姓名
                /// </summary>
                public object real_name { get; set; }

            }
        }
    }
}
