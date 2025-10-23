using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.ModelDbs;
using static WeiCode.Utility.UtilityStatic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class ZhenduanApiDataService
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="beginDate">开始日期</param>
            /// <param name="endDate">结束日期</param>
            /// <param name="anchorID">UID</param>
            /// <returns></returns>
            public decimal[] Get(string beginDate, string endDate, string anchorID)
            {
                decimal[] arr = new decimal[11];
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                string reStr = HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/pugna_component/data/v2/faction/anchor_detail/room_list_with_tag?anchorID={anchorID}&liveID=1&beginDate={beginDate}&endDate={endDate}&page=1&size=50&msToken=MrECf1RitIL3k34iKAiDeCUhn68K76cGGajJO-NwvjiK3C3OEtiVvwPq1DU1SDjPM-CnH1OtyOUpHSwsK7k8c-BX6wewaRc0ZXkACo_Z&X-Bogus=DFSzswVLoWSdU5q5tpm6KM9WX7rE&_signature=_02B4Z6wo000014i97ZwAAIDDCL8X34v6vPOIvekAAIWolorZ-86YP.ZksHpKCYXKaDM2o2LZj1PUkhFrvywIHKkLTZFG5Lr8Ac4Wj-wy4CLZ1IaHLuPN9QbYuARXS2R6l7V7qKqMyz0P8Q9iu3f", headers);
                var obj = (JObject)JsonConvert.DeserializeObject(reStr);
                UtilityStatic.TxtLog.Info(reStr);
                string data_string = Convert.ToString(obj["data"]["data_string"]);
                var obj_data_string = (JObject)JsonConvert.DeserializeObject(data_string);
                var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj_data_string["data"]["series"]));

                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.5845.97 Safari/537.36 Core/1.116.457.400 QQBrowser/13.4.6233.400");
                foreach (var item in data_arr)
                {
                    arr[0] += Convert.ToDecimal(item["watchUV"]);   //进直播间人数
                    arr[1] += Convert.ToDecimal(item["liveDuration"]);   //直播时长
                    arr[2] += Convert.ToDecimal(item["avgWatchDuration"]) * Convert.ToDecimal(item["watchUV"]);   //总观看时长=人均观看时长*进直播间人数
                    arr[3] += Convert.ToDecimal(item["showUV"]);   //曝光人数
                    arr[4] += Convert.ToDecimal(item["showPV"]);   //曝光次数
                    arr[5] += Convert.ToDecimal(item["payUV"]);   //打赏人数
                    arr[6] += Convert.ToDecimal(item["payPV"]);   //打赏次数
                    arr[8] += Convert.ToDecimal(item["fanTicket"]);   //音浪
                    arr[9] += Convert.ToDecimal(item["commentUV"]);   //评论人数(互动人数)
                    arr[10] += Convert.ToDecimal(item["avgWatchDuration"]);   //人均观看时长
                    Thread.Sleep(500);

                    string reStrLx = HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/pugna_component/data/v2/room/detail/new_pay_rank?roomID={item["roomID"]}&offset=1&limit=5", headers);
                    string total = RegexHelper.GetMetaString(reStrLx, @"\\""total\\"":", @"},");
                    arr[7] += Convert.ToDecimal(total);//新人打赏人数
                }
                return arr;
            }

        }
    }    
}
