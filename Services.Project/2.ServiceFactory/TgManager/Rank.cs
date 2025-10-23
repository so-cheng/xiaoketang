using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using System.Linq;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class TgManager
        {
            public class YyRankModel : ModelDb.user_base
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
