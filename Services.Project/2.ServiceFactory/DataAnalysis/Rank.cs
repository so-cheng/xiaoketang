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
        public partial class DataAnalysis
        {
            #region 主播排名Model
            /// <summary>
            /// 主播平均拉新个数
            /// </summary>
            public class ZbJixiaoNewNumRankModel : UserInfo.Zhubo.ZbBaseInfo
            {
                /// <summary>
                /// 所属厅名
                /// </summary>
                public string ting_name
                {
                    get
                    {
                        return ting_sn.IsNullOrEmpty() ? "" : new UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                /// <summary>
                /// 主播平均拉新个数
                /// </summary>
                public object newNum_average { get; set; }
                /// <summary>
                /// 总拉新
                /// </summary>
                public object newNum_sum { get; set; } = 0;
            }

            /// <summary>
            /// 主播平均二消个数
            /// </summary>
            public class ZbJixiaoNum2RankModel : UserInfo.Zhubo.ZbBaseInfo
            {
                /// <summary>
                /// 所属厅名
                /// </summary>
                public string ting_name
                {
                    get
                    {
                        return ting_sn.IsNullOrEmpty() ? "" : new UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                /// <summary>
                /// 主播平均二消个数
                /// </summary>
                public object num2_average { get; set; }
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 主播平均二消音浪
            /// </summary>
            public class ZbJixiaoAmount2RankModel : UserInfo.Zhubo.ZbBaseInfo
            {
                /// <summary>
                /// 所属厅名
                /// </summary>
                public string ting_name
                {
                    get
                    {
                        return ting_sn.IsNullOrEmpty() ? "" : new UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                /// <summary>
                /// 主播每日平均二消音浪
                /// </summary>
                public object dayAmount2_average { get; set; }
                /// <summary>
                /// 用户平均二消音浪
                /// </summary>
                public object yhAmount2_average { get; set; }
                /// <summary>
                /// 总二消音浪
                /// </summary>
                public object jixiaoAmount_sum { get; set; } = 0;
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 主播平均建联数
            /// </summary>
            public class ZbJixiaoContactNumRankModel : UserInfo.Zhubo.ZbBaseInfo
            {
                /// <summary>
                /// 所属厅名
                /// </summary>
                public string ting_name
                {
                    get
                    {
                        return ting_sn.IsNullOrEmpty() ? "" : new UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                /// <summary>
                /// 主播平均建联数
                /// </summary>
                public object contact_average { get; set; }
                /// <summary>
                /// 总建联
                /// </summary>
                public object contact_sum { get; set; } = 0;
            }
            #endregion

            #region 单厅排名Model
            /// <summary>
            /// 单厅主播平均拉新个数
            /// </summary>
            public class TingJixiaoNewNumRankModel : UserInfo.Ting.TingInfo
            {
                /// <summary>
                /// 所属运营
                /// </summary>
                public object yy_name
                {
                    get; set;
                }
                /// <summary>
                /// 所属厅管
                /// </summary>
                public object tg_name
                {
                    get; set;
                }
                /// <summary>
                /// 主播平均拉新个数
                /// </summary>
                public object newNum_average { get; set; }
                /// <summary>
                /// 总拉新
                /// </summary>
                public object newNum_sum { get; set; } = 0;
            }

            /// <summary>
            /// 单厅主播平均二消个数
            /// </summary>
            public class TingJixiaoNum2RankModel : UserInfo.Ting.TingInfo
            {
                /// <summary>
                /// 所属运营
                /// </summary>
                public object yy_name
                {
                    get; set;
                }
                /// <summary>
                /// 所属厅管
                /// </summary>
                public object tg_name
                {
                    get; set;
                }
                /// <summary>
                /// 主播平均二消个数
                /// </summary>
                public object num2_average { get; set; }
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 单厅主播平均二消音浪
            /// </summary>
            public class TingJixiaoAmount2RankModel : UserInfo.Ting.TingInfo
            {
                /// <summary>
                /// 所属运营
                /// </summary>
                public object yy_name
                {
                    get; set;
                }
                /// <summary>
                /// 所属厅管
                /// </summary>
                public object tg_name
                {
                    get; set;
                }
                /// <summary>
                /// 主播平均二消音浪
                /// </summary>
                public object zbAmount2_average { get; set; }
                /// <summary>
                /// 用户平均二消音浪
                /// </summary>
                public object yhAmount2_average { get; set; }
                /// <summary>
                /// 总二消音浪
                /// </summary>
                public object jixiaoAmount_sum { get; set; } = 0;
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 单厅主播平均建联数
            /// </summary>
            public class TingJixiaoContactNumRankModel : UserInfo.Ting.TingInfo
            {
                /// <summary>
                /// 所属运营
                /// </summary>
                public object yy_name
                {
                    get; set;
                }
                /// <summary>
                /// 所属厅管
                /// </summary>
                public object tg_name
                {
                    get; set;
                }
                /// <summary>
                /// 主播平均建联数
                /// </summary>
                public object contact_average { get; set; }
                /// <summary>
                /// 总建联
                /// </summary>
                public object contact_sum { get; set; } = 0;
            }
            #endregion

            #region 运营排名Model
            /// <summary>
            /// 团队主播平均拉新个数
            /// </summary>
            public class YyJixiaoNewNumRankModel : UserInfo.Yy.YYInfo
            {
                /// <summary>
                /// 主播平均拉新个数
                /// </summary>
                public object newNum_average { get; set; }
                /// <summary>
                /// 总拉新
                /// </summary>
                public object newNum_sum { get; set; } = 0;
            }

            /// <summary>
            /// 团队主播平均二消个数
            /// </summary>
            public class YyJixiaoNum2RankModel : UserInfo.Tg.TgInfo
            {
                /// <summary>
                /// 主播平均二消个数
                /// </summary>
                public object num2_average { get; set; }
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 团队主播平均二消音浪
            /// </summary>
            public class YyJixiaoAmount2RankModel : UserInfo.Tg.TgInfo
            {
                /// <summary>
                /// 主播平均二消音浪
                /// </summary>
                public object zbmount2_average { get; set; }
                /// <summary>
                /// 用户平均二消音浪
                /// </summary>
                public object yhAmount2_average { get; set; }
                /// <summary>
                /// 总二消音浪
                /// </summary>
                public object jixiaoAmount_sum { get; set; } = 0;
                /// <summary>
                /// 总二消个数
                /// </summary>
                public object num2_sum { get; set; } = 0;
            }

            /// <summary>
            /// 团队主播平均建联数
            /// </summary>
            public class YyJixiaoContactNumRankModel : UserInfo.Tg.TgInfo
            {
                /// <summary>
                /// 主播平均建联数
                /// </summary>
                public object contact_average { get; set; }
                /// <summary>
                /// 总建联
                /// </summary>
                public object contact_sum { get; set; } = 0;
            }
            #endregion
        }
    }
}
