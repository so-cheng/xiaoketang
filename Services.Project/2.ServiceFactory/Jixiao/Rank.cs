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
        public partial class Jixiao
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

            public class ZbJixiaoRankModel : ServiceFactory.UserInfo.Zhubo.ZbBaseInfo
            {
                public object zbCount { get; set; } = 0;
                public string tg_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(this.tg_user_sn).username;
                    }
                }
                /// <summary>
                /// 平均数
                /// </summary>
                public object average { get; set; }
                public object yhAmount2_average { get; set; }
                public object jixiaoSum { get; set; } = 0;
                /// <summary>
                /// 排名
                /// </summary>
                public int rank { get; set; }

            }
            public class TgJixiaoRankModel : ServiceFactory.UserInfo.Tg.TgInfo
            {
                public object yy_name
                {
                    get; set;
                }
                public object zbCount { get; set; } = 0;
                /// <summary>
                /// 平均数
                /// </summary>
                public object average { get; set; }
                public object jixiaoSum { get; set; } = 0;
                /// <summary>
                /// 排名
                /// </summary>
                public int rank { get; set; }

            }
            public class YyJixiaoRankModel : ServiceFactory.UserInfo.Yy.YYInfo
            {

                public object zbCount
                {
                    get; set;
                }
                /// <summary>
                /// 平均数
                /// </summary>
                public object average { get; set; }
                public object jixiaoSum { get; set; } = 0;
                /// <summary>
                /// 排名
                /// </summary>
                public int rank { get; set; }

            }

            //public class YyJixiaoRankModel : ServiceFactory.UserInfo.Yy.YYInfo
            //{

            //}
        }
        
    }
    
}
