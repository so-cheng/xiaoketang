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
    public class TingAndZbController : BaseController
    {
        /// <summary>
        /// 今日开播
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetTodayOpen(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {

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
            var headers = new Dictionary<string, string>();
            headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

            var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/anchor_list/list?vertical_type=0&sort_tag=5&is_increasing=false&anchor_list_filter=%7B%22support_render_new_anchor%22%3Atrue%2C%22support_render_growth%22%3Atrue%2C%22support_render_recruit_broker%22%3Atrue%2C%22list_default_enum%22%3A2%7D&page={page}&size=50", headers).ToJObject();
            //UtilityStatic.TxtLog.Info(reStr);
            var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
            if (data_arr.Count == 0) throw new WeicodeException("用户不存在");
            foreach (var item in data_arr)
            {

            }
            GetTodayOpenFunc(page++);
        }

        /// <summary>
        /// 获取每日核心数据
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
    }
}