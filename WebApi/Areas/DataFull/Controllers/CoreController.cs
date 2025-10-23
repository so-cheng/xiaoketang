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

namespace WebProject.Areas.DataFull.Controllers
{
    public class CoreController : BaseController
    {
        /// <summary>
        /// 今日总数据
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetTotal(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/pugna_component/data/v2/faction/workbench/monitor_core_card_old?dateType=4", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var obj_data_string = (JObject)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["data_string"]));
                var data_arr = obj_data_string["data"]["series"][0];
                string income = Convert.ToString(data_arr["income"]);                   //今日总音浪
                string liveAnchorCnt = Convert.ToString(data_arr["liveAnchorCnt"]);     //今日开播人数
                string newAnchor = Convert.ToString(data_arr["newAnchor"]);             //今日新入驻主播人数

                Thread.Sleep(1000);

                var obj1 = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/pugna_component/data/v2/faction/workbench/core_card_common?dateType=4&scene=voice_live", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var obj_data_string1 = (JObject)JsonConvert.DeserializeObject(Convert.ToString(obj1["data"]["data_string"]));
                var data_arr1 = obj_data_string1["data"]["series"][0];
                string linkGuestCnt = Convert.ToString(data_arr1["linkGuestCnt"]);           //今日连线嘉宾数
                string guestLiveIncome = Convert.ToString(data_arr1["guestLiveIncome"]);     //今日嘉宾个播音浪
                string guestLiveUcnt = Convert.ToString(data_arr1["guestLiveUcnt"]);         //今日嘉宾个播数

                result.data = new
                {
                    income = income,
                    liveAnchorCnt = liveAnchorCnt,
                    newAnchor = newAnchor,
                    linkGuestCnt = linkGuestCnt,
                    guestLiveIncome = guestLiveIncome,
                    guestLiveUcnt = guestLiveUcnt
                };
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