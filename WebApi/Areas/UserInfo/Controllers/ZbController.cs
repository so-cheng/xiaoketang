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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZbController : BaseController
    {
        /// <summary>
        /// 根据抖音号获取主播资料
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetInfo(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                var user = DoMySql.FindList<ModelDb.user_base>($"user_name like '%众创未来%'");
                if (user.Count == 0)
                {

                    if (dou_username.IsNullOrEmpty()) throw new WeicodeException("抖音号不能为空");
                    var headers = new Dictionary<string, string>();
                    headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                    string reStr = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/component/anchor/search?scene=0&page=0&size=30&search_val={dou_username}&support_recruit_broker_relation=true", headers);
                    var obj = (JObject)JsonConvert.DeserializeObject(reStr);
                    //UtilityStatic.TxtLog.Info(reStr);
                    var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
                    if (data_arr.Count == 0) throw new WeicodeException("抖音号不存在");
                    if (Convert.ToString(data_arr[0]["unique_id"]) != dou_username) throw new WeicodeException("抖音号不存在");
                    string nickname = Convert.ToString(data_arr[0]["nickname"]);   //抖音昵称
                    string anchor_id = Convert.ToString(data_arr[0]["anchor_id"]);   //anchor_id

                    result.data = new
                    {
                        nickname = nickname,
                        anchor_id = anchor_id
                    };
                }
                else
                {
                    result.data = new
                    {
                        nickname = "",
                        anchor_id = ""
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
        /// 根据主播anchor_id获取当前运营经纪人
        /// </summary>
        /// <param name="anchor_id"></param>
        /// <returns></returns>
        public JsonResult GetBrokerByAnchorId(string anchor_id)
        {
            var result = new JsonResultAction();
            try
            {
                var user = DoMySql.FindList<ModelDb.user_base>($"user_name like '%众创未来%'");
                if (user.Count == 0)
                {
                    if (anchor_id.IsNullOrEmpty()) throw new WeicodeException("主播anchor_id不能为空");
                    var headers = new Dictionary<string, string>();
                    headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                    var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/anchor/detail_v2/get_anchor_detail?anchor_id={anchor_id}", new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        headers = headers
                    }).ToJObject();
                    //UtilityStatic.TxtLog.Info(reStr);
                    result.data = new
                    {
                        agent_name = obj["data"]["signing_info"]["agent_name"].ToNullableString(),
                        agent_id = obj["data"]["signing_info"]["agent_id"].ToNullableString(),
                    };
                }
                else
                {
                    result.data = new
                    {
                        agent_name = "",
                        agent_id = "",
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
        /// 分配运营经纪人
        /// </summary>
        /// <param name="anchor_id">主播anchor_id</param>
        /// <param name="broker_id">运营经纪人</param>
        /// <returns></returns>
        public JsonResult SetTg(string anchor_id, string broker_id)
        {
            var result = new JsonResultAction();
            try
            {
                if (anchor_id.IsNullOrEmpty()) throw new WeicodeException("主播anchor_id不能为空");
                if (broker_id.IsNullOrEmpty()) throw new WeicodeException("运营经纪人不能为空");
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpPost($"https://union.bytedance.com/ark/api/uanchor/anchor/union_anchor_operation/update_anchor_broker", @"{""anchor_id"":""" + anchor_id + @""",""broker_id"":""" + broker_id + @"""}", new UtilityStatic.HttpHelper.HttpPostReq
                {
                    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad,
                    headers = headers
                }).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
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
        /// 解绑运营经纪人
        /// </summary>
        /// <param name="anchor_id">主播anchor_id</param>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult DelTg(string anchor_id)
        {
            var result = new JsonResultAction();
            try
            {
                if (anchor_id.IsNullOrEmpty()) throw new WeicodeException("主播anchor_id不能为空");
                string broker_id = "332528391040899";   //经纪人-光谷

                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpPost($"https://union.bytedance.com/ark/api/uanchor/anchor/union_anchor_operation/update_anchor_broker", @"{""anchor_id"":""" + anchor_id + @""",""broker_id"":""" + broker_id + @"""}", new UtilityStatic.HttpHelper.HttpPostReq
                {
                    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad,
                    headers = headers
                }).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        public JsonResult CheckUserInfo(string dou_username, string last_four_number, string real_name)
        {
            var result = new JsonResultAction();
            try
            {
                if (dou_username.IsNullOrEmpty()) throw new WeicodeException("抖音号不能为空");
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpPost($"https://union.bytedance.com/ark/api/basic/anchor/invite/check_anchor_can_invite_v3", @"{""live_id"":1,""input_anchor_id"":""" + dou_username + @""",""real_name"":""" + real_name + @""",""last_four_number"":""" + last_four_number + @"""}", new UtilityStatic.HttpHelper.HttpPostReq
                {
                    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad,
                    headers = headers
                }).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                switch (obj["status_code"].ToNullableString())
                {
                    case "130000":
                        result.code = 1;
                        result.msg = obj["message"].ToNullableString();
                        break;
                    case "0":
                        if (obj["data"]["block_type"].ToNullableString() != "0")
                        {
                            result.code = 1;
                            result.msg = obj["data"]["stripe_message"].ToNullableString();
                        }
                        break;
                    default:
                        result.code = 1;
                        result.msg = obj["msg"].ToNullableString();
                        break;
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
        /// 每个主播的扫脸天数和有效时长
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetQrDays(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                GetQrDaysPage(1);
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        private void GetQrDaysPage(int page)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

            var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/energise/union_task/get_anchor_task_list?status=1&page=13&size=50&task_type=124&sub_task_type=0&stage_id=-1&order_type=2&rank_type_str=valid_day", headers).ToJObject();
            //UtilityStatic.TxtLog.Info(reStr);
            var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["anchor_task_list"]));

            if (data_arr.Count == 0) throw new WeicodeException("用户不存在");
            foreach (var item in data_arr)
            {
                var columnInfo_lists = (JArray)JsonConvert.DeserializeObject(Convert.ToString(item["columnInfo_list"]));
                foreach (var _item in columnInfo_lists)
                {
                    if (Convert.ToString(_item["column"]) == "4")    //本月有效时长
                    {
                        var finish_value = (JArray)JsonConvert.DeserializeObject(Convert.ToString(_item["condition_info_list"]));
                        string valid_minutes = Convert.ToString(finish_value[0]["finish_value"]);
                    }

                    if (Convert.ToString(_item["column"]) == "8")   //本月有效天数
                    {
                        var finish_value = (JArray)JsonConvert.DeserializeObject(Convert.ToString(_item["condition_info_list"]));
                        string valid_days = Convert.ToString(finish_value[0]["finish_value"]);
                    }
                }
            }
        }
    }
}