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
        public partial class UserTable
        {
            public class TingZbInfoModel : ServiceFactory.UserInfo.Ting.TingInfo
            {
                public int zbCount { get; set; }
                public int zbACount { get; set; }
                public int zbBCount { get; set; }
                public int zbCCount { get; set; }
                public int zbJZCount { get; set; }
                public int zbQZCount { get; set; }

            }
            public class TgZbInfoModel : ServiceFactory.UserInfo.Tg.TgInfo
            {
                public int zbCount { get; set; }
                public int zbACount { get; set; }
                public int zbBCount { get; set; }
                public int zbCCount { get; set; }
                public int zbJZCount { get; set; }
                public int zbQZCount { get; set; }

            }
            public class YyZbInfoModel : ServiceFactory.UserInfo.Yy.YYInfo
            {
                public int zbCount { get; set; }
                public int zbACount { get; set; }
                public int zbBCount { get; set; }
                public int zbCCount { get; set; }
                public int zbJZCount { get; set; }
                public int zbQZCount { get; set; }

            }
        }
    }
    
}
