using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class ZbController : BaseController
    {
        /// <summary>
        /// 根据抖音号获取抖音昵称
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetInfo(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/component/anchor/search?scene=0&page=0&size=30&search_val={dou_username}&support_recruit_broker_relation=true", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
                if(data_arr.Count == 0) throw new WeicodeException("用户不存在");
                foreach (var item in data_arr)
                {
                    string nickname = Convert.ToString(item["nickname"]);   //抖音昵称
                    result.data = new
                    {
                        nickname = nickname
                    };
                }

            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetCoreData(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/pugna_component/data/v2/union/data_center/anchor_list?beginDate={DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}&endDate={DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}&page=1&size=100&orderField=totalFanTicket&sortType=desc&fields=totalFanTicket&fields=liveFanTicketWithoutGuest&fields=activityFanTicketWithoutGuest&fields=propertyFanTicketWithoutGuest&fields=subscribeFanTicket&fields=specialFanTicket&fields=isSingleThan1hLiveValidDay&fields=liveDuration1d&fields=single25minValidLiveDuration1d&fields=feedAcuRaw&fields=feedWatchDuration1d&fields=perMinuteUcnt1d&fields=starGuardEarnScore&fields=statGuardEnterAccountEarnScore&fields=starGuardUcnt&fields=anchorInfo&msToken=_eKtQsUf_BoOEskHXZGXQv7VPfdb3NNqrR4nj-joaYYpZW1_3uBzgu4Mcnf8dHTfi3WDhfRIENRHMxJTF7LqYisfS2aQ46x-kphymq_Y&X-Bogus=DFSzswVuAZRdUf3fCSf5Ee9WX7rQ", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
                if (data_arr.Count == 0) throw new WeicodeException("用户不存在");
                foreach (var item in data_arr)
                {
                    string nickname = Convert.ToString(item["nickname"]);   //抖音昵称
                    result.data = new
                    {
                        nickname = nickname
                    };
                }

            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        #region 今日开播

        /// <summary>
        /// 今日开播
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTodayOpen()
        {
            var result = new JsonResultAction();
            try
            {
                GetTodayOpenFunc(1);
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        private void GetTodayOpenFunc(int page)
        {
            string s_date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string e_date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var headers = new Dictionary<string, string>();
            headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

            var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/pugna_component/data/v2/union/data_center/anchor_list?beginDate={s_date}&endDate={e_date}&anchorType=guest&page={page}&size=100&orderField=totalFanTicket&sortType=desc&fields=totalFanTicket&fields=liveFanTicketWithoutGuest&fields=activityFanTicketWithoutGuest&fields=propertyFanTicketWithoutGuest&fields=subscribeFanTicket&fields=specialFanTicket&fields=isSingleThan1hLiveValidDay&fields=liveDuration1d&fields=single25minValidLiveDuration1d&fields=feedAcuRaw&fields=feedWatchDuration1d&fields=perMinuteUcnt1d&fields=starGuardEarnScore&fields=statGuardEnterAccountEarnScore&fields=starGuardUcnt&fields=voiceChatNowandererFanTicket&fields=voiceChatLinkDuration1d&fields=anchorInfo", headers).ToJObject();
            //UtilityStatic.TxtLog.Info(reStr);
            var data_arr = Convert.ToString(obj["data"]["data_string"]).ToJObject();
            var series = Convert.ToString(data_arr["data"]["series"]).ToJArray();
            if (series.Count == 0)
            {
                return; //最后一页
            }
            foreach (var item in series)
            {
                string anchorID = Convert.ToString(item["anchorID"]);

            }
            Thread.Sleep(1000);
            GetTodayOpenFunc(page++);
        }
        #endregion
    }
}