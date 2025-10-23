using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.ModelDbs;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        
        public partial class UserTable
        {
            public class TgInfoRankModel : ModelDb.user_base
            {
                public double rank { get; set; }
                /// <summary>
                /// 总厅管数
                /// </summary>
                public int allCount { get; set; } = 0;
                /// <summary>
                /// 完整信息厅管数
                /// </summary>
                public int completeCount { get; set; } = 0;
                /// <summary>
                /// 缺失信息厅管数
                /// </summary>
                public int lackCount { get; set; } = 0;
            }
        }
    }
    
}
