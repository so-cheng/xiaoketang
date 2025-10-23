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
        public class TingZhan
        {
            public class MateRankModel : ModelDb.user_base
            {
                /// <summary>
                /// 运营团队
                /// </summary>
                public string yy_name { get; set; } = "";
                /// <summary>
                /// 厅名
                /// </summary>
                public string ting_name { get; set; } = "";
                /// <summary>
                /// 对战厅数
                /// </summary>
                public int tingsCount { get; set; } = 0;
                /// <summary>
                /// 胜率
                /// </summary>
                public decimal rate { get; set; } = 0;
            }
        }
    }
}
