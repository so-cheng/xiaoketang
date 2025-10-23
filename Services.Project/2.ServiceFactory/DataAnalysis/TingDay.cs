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
        public class TingDay
        {
            #region 每日核心-实时Model
            /// <summary>
            /// 抖音数据-厅-每日核心-实时
            /// </summary>
            public class TingDayNowModel
            {
                public Nullable<DateTime> c_date { get; set; } 
                public string ting_name { get; set; } = "";
                public decimal? income { get; set; } = 0M;
                public decimal? ting_target { get; set; } = 0M;
                public decimal? remain_target
                {
                    get { return ((ting_target - income) > 0 ? (ting_target - income) : 0M); }
                }
            }
            #endregion
        }
    }
}