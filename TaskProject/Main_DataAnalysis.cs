using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// <summary>
    /// 类名(固定命名，不能修改)
    /// </summary>
    public partial class ProjectClass
    {
        /// <summary>
        /// 根据厅场次和观众统计厅每日核心数据指标
        /// </summary>
        /// <returns></returns>
        public string IndicatorDay()
        {
            DateTime date = DateTime.Today.AddDays(-1);
            string c_date = date.ToString("yyyy-MM-dd");
            string date3 = date.AddDays(-2).ToString("yyyy-MM-dd");// 3天内
            string date5 = date.AddDays(-4).ToString("yyyy-MM-dd");
            string date7 = date.AddDays(-6).ToString("yyyy-MM-dd");
            string date10 = date.AddDays(-9).ToString("yyyy-MM-dd");
            string date15 = date.AddDays(-14).ToString("yyyy-MM-dd");

            foreach (var item in new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()))
            {
                #region 1. 查询当前厅的基础数据
                var arr_doudata_round_ting = DoMySql.FindField<doudata_round_ting>("SUM(liveSeconds), SUM(watchUV), SUM(commentUV), SUM(showUV), SUM(fanTicket), AVG(live_rate)", $"ting_sn = '{item.ting_sn}' AND c_date = '{c_date}'");
                // 开播时长
                var liveSeconds = arr_doudata_round_ting[0];
                // 访客数
                var watchUV = arr_doudata_round_ting[1];
                // 互动人数
                var commentUV = arr_doudata_round_ting[2];
                // 曝光人数
                var showUV = arr_doudata_round_ting[3];
                // 总音浪
                var fanTicket = arr_doudata_round_ting[4];
                // 直播推荐比例
                var live_rate = arr_doudata_round_ting[5];

                // 直播时长（小时）
                var liveHour = Math.Round(liveSeconds.ToDecimal() / 3600, 2);
                // 每小时访客数（访客数 / 直播时长）
                var hourly_visitors = liveHour > 0 ? Math.Round(watchUV.ToDecimal() / liveHour) : 0;
                // 进入率（访客数 / 曝光人数）
                var entry_rate = showUV.ToDecimal() > 0 ? Math.Round(watchUV.ToDecimal() / showUV.ToDecimal() * 100, 2) : 0;

                // 付费用户总数（当天）
                var paying_users_Count = DoMySql.FindList<doudata_round_ting_guest>($"ting_sn = '{item.ting_sn}' AND c_date = '{c_date}'").Count;
                #endregion

                /* 
                 * A类 累计点赞送礼值 > 999
                 * B类 累计点赞送礼值 > 399，<= 999
                 * C类 累计点赞送礼值 > 99，<= 399
                 * 新付 累计点赞送礼值 > 0，<= 99
                 */

                #region 2. 厅当天升为A/B/C类用户数
                var aTyeCount = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift999_date = '{c_date}'").Count;
                var bTyeCount = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift399_date = '{c_date}'").Count - aTyeCount;
                var cTyeCount = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift99_date = '{c_date}'").Count - aTyeCount - bTyeCount;
                var new_paying_users = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift_date = '{c_date}'").Count - aTyeCount - bTyeCount - cTyeCount;
                #endregion

                #region 3. 近5/10/15天升为A类用户数       
                var aType5Day = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift999_date BETWEEN '{date5}' AND '{c_date}'").Count;
                var aType10Day = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift999_date BETWEEN '{date10}' AND '{c_date}'").Count;
                var aType15Day = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift999_date BETWEEN '{date15}' AND '{c_date}'").Count;
                #endregion

                #region 4. 3/5/7/15天内厅老用户数（近15天陪伴次数 > 0）
                var last_day = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
                var oldUser3Day = DoMySql.FindList<doudata_round_ting_guest>($"ting_sn = '{item.ting_sn}' and companyCnt > 0 AND c_date = '{c_date}' and exists (select 1 from doudata_ting_guest where userID = doudata_round_ting_guest.userID and likeGift_date >= '{date3}')").Count;

                var oldUser5Day = DoMySql.FindList<doudata_round_ting_guest>($"ting_sn = '{item.ting_sn}' and companyCnt > 0 AND c_date = '{c_date}' and exists (select 1 from doudata_ting_guest where userID = doudata_round_ting_guest.userID and likeGift_date >= '{date5}')").Count;

                var oldUser7Day = DoMySql.FindList<doudata_round_ting_guest>($"ting_sn = '{item.ting_sn}' and companyCnt > 0 AND c_date = '{c_date}' and exists (select 1 from doudata_ting_guest where userID = doudata_round_ting_guest.userID and likeGift_date >= '{date7}')").Count;

                var oldUser15Day = DoMySql.FindList<doudata_round_ting_guest>($"ting_sn = '{item.ting_sn}' and companyCnt > 0 AND c_date = '{c_date}' and exists (select 1 from doudata_ting_guest where userID = doudata_round_ting_guest.userID and likeGift_date >= '{date15}')").Count;
                #endregion

                // 5天内升A类的用户数
                var typeA5DayCount = DoMySql.FindList<doudata_ting_guest>($"ting_sn = '{item.ting_sn}' AND likeGift_date BETWEEN '{date5}' AND '{c_date}' AND likeGift999_date BETWEEN '{date5}' AND '{c_date}'").Count;

                // 插入数据
                new dataanalysis_coredata_ting
                {
                    tenant_id = item.tenant_id,
                    yy_user_sn = item.yy_user_sn,
                    tg_user_sn = item.tg_user_sn,
                    ting_sn = item.ting_sn,
                    dou_UID = item.dou_UID,
                    c_date = date,
                    zb_count = new ServiceFactory.JixiaoDay.JixiaoService().GetZbReportNumByTingSn(item.ting_sn, date),
                    gear = new ServiceFactory.JixiaoDay.JixiaoService().GetDangNumByTingSn(item.ting_sn, date),
                    liveHour = liveHour,
                    visitor = watchUV.ToInt(),
                    hourly_visitors = hourly_visitors.ToInt(),
                    live_rate = live_rate.ToDecimal(),
                    entry_rate = entry_rate,
                    interactive_user_count = commentUV.ToInt(),
                    paying_user_total = paying_users_Count,
                    new_paying_user_count = new_paying_users,
                    avg_second_consumption = new ServiceFactory.JixiaoDay.JixiaoService().GetAmount2AVG(item.ting_sn, date).ToInt(),
                    user_count_type_a = aTyeCount,
                    user_count_type_b = bTyeCount,
                    user_count_type_c = cTyeCount,
                    user_count_type_a_last_5d = aType5Day,
                    user_count_type_a_last_10d = aType10Day,
                    user_count_type_a_last_15d = aType15Day,
                    old_user_count_last_3d = oldUser3Day,
                    old_user_count_last_5d = oldUser5Day,
                    old_user_count_last_7d = oldUser7Day,
                    old_user_count_last_15d = oldUser15Day,
                    upgraded_to_a_count_last_5d = typeA5DayCount,
                    totalFanTickets = fanTicket.ToInt(),
                }.InsertOrUpdate($"ting_sn = '{item.ting_sn}' AND c_date = '{date}'");

                Thread.Sleep(1000);
            }

            // 日志
            new DomainBasic.SystemBizLogApp().Write("定时任务", sys_biz_log.log_type_enum.产品模块.ToSByte(), "", $"数据最后更新时间:{DateTime.Now}", "核心数据");
            return "根据厅场次和观众统计厅每日核心数据指标";
        }
    }
}
