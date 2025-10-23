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
    public class TgController : BaseController
    {
        /// <summary>
        /// 根据抖音获取主播资料
        /// </summary>
        /// <param name="dou_username"></param>
        /// <returns></returns>
        public JsonResult GetInfo(string dou_username)
        {
            var result = new JsonResultAction();
            try
            {
                if (dou_username.IsNullOrEmpty()) throw new WeicodeException("抖音号不能为空");
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/component/anchor/search?scene=0&page=0&size=30&search_val={dou_username}&support_recruit_broker_relation=true", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
                if (data_arr.Count == 0) throw new WeicodeException("抖音号不存在");
                if (Convert.ToString(data_arr[0]["unique_id"]) != dou_username) throw new WeicodeException("抖音号不存在");
                foreach (var item in data_arr)
                {
                    string nickname = Convert.ToString(item["nickname"]);   //抖音昵称
                    string anchor_id = Convert.ToString(item["anchor_id"]);   //anchor_id
                    result.data = new
                    {
                        nickname = nickname,
                        anchor_id = anchor_id
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
        /// 根据经纪人名字获取id
        /// </summary>
        /// <param name="jjr_name"></param>
        /// <returns></returns>
        public JsonResult GetJjrInfo(string jjr_name)
        {
            var result = new JsonResultAction();
            try
            {
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/data/component/agent/search?search_val={jjr_name}&skip_leader=true&all_skip_leader=true&size=100&page=0", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                var data_arr = (JArray)JsonConvert.DeserializeObject(Convert.ToString(obj["data"]["item_list"]));
                if (data_arr.Count == 0) throw new WeicodeException("用户不存在");
                string name = Convert.ToString(data_arr[0]["name"]);   //经纪人名称
                string broker_id = Convert.ToString(data_arr[0]["id"]);   //经纪人id
                result.data = new
                {
                    name = name,
                    broker_id = broker_id
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

        /// <summary>
        /// 校验签约信息
        /// </summary>
        /// <param name="dou_username"></param>
        /// <param name="last_four_number"></param>
        /// <param name="real_name"></param>
        /// <returns></returns>
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
        /// 普通邀约的签约
        /// </summary>
        /// <param name="anchor_id">主播ID</param>
        /// <param name="real_name">主播真实姓名</param>
        /// <param name="last_four_number"></param>
        /// <param name="jjranchor_id">主播手机号后四位</param>
        /// <param name="recruit_source">招募渠道: 1抖音-短视频 2抖音-直播间 3抖音-私信 5熟人介绍 10社交APP-小红书 4招聘软件-Boss直聘 12招聘软件-智联招聘等 11线下合作(街头地推/本地广告/异业合作等) 6街头地推 7本地广告 8 高校/技校  9其他 </param>
        /// <returns>{"_entity":null,"return_reload":null,"code":0,"msg":"操作成功","data":{"invite_id":"27126283","flow_id":"7562474621564472103"}}</returns> 
        public JsonResult InnviteAnchor(string anchor_id, string real_name, string last_four_number, string jjranchor_id, int recruit_source = 1)
        {
            var result = new JsonResultAction();
            try
            {
                //第一步验证能否邀约
                if (anchor_id.IsNullOrEmpty()) throw new WeicodeException("抖音号不能为空");
                if (real_name.IsNullOrEmpty()) throw new WeicodeException("真实姓名不能为空");
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpPost($"https://union.bytedance.com/ark/api/basic/anchor/invite/check_anchor_can_invite_v3", @"{""live_id"":1,""input_anchor_id"":""" + anchor_id + @""",""real_name"":""" + real_name + @""",""last_four_number"":""" + last_four_number + @"""}", new UtilityStatic.HttpHelper.HttpPostReq
                {
                    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad,
                    headers = headers
                }).ToJObject();
                if (obj["status_code"].ToNullableString() == "0")//jie
                {
                    if (obj["data"]["can_invite"].ToBool())//可以注册
                    {
                        string invitepoststring = "{\"live_id\":1,\"input_anchor_id\":\"" + anchor_id + "\",\"real_name\":\"" + real_name + "\",\"telephone\":\"" + last_four_number + "\",\"broker_id\":\"" + jjranchor_id + "\",\"sign_duration\":1,\"anchor_src_live_id\":1,\"invite_type\":1,\"contract_oid\":\"\",\"version\":3,\"special_server_details\":\"[{\\\"clauseKey\\\":\\\"hardware\\\",\\\"clauseName\\\":\\\"直播硬件设备\\\",\\\"faction\\\":{\\\"requiredDetail\\\":1,\\\"unit\\\":1,\\\"concrete\\\":null}},{\\\"clauseKey\\\":\\\"offlineRoom\\\",\\\"clauseName\\\":\\\"线下直播间\\\",\\\"faction\\\":{\\\"requiredDetail\\\":1,\\\"unit\\\":2,\\\"concrete\\\":null}},{\\\"clauseKey\\\":\\\"liveTogether\\\",\\\"clauseName\\\":\\\"经纪人跟播\\\",\\\"faction\\\":{\\\"requiredDetail\\\":1,\\\"unit\\\":2,\\\"concrete\\\":null}},{\\\"clauseKey\\\":\\\"hotSpot\\\",\\\"clauseName\\\":\\\"流量投入（dou+）\\\",\\\"faction\\\":{\\\"requiredDetail\\\":1,\\\"unit\\\":2,\\\"concrete\\\":null}},{\\\"clauseKey\\\":\\\"shortVideo\\\",\\\"clauseName\\\":\\\"短视频运营\\\",\\\"faction\\\":{\\\"requiredDetail\\\":2,\\\"unit\\\":2,\\\"concrete\\\":null}}]\",\"basic_salary_type\":0,\"anchor_duty\":\"[{\\\"clauseKey\\\":\\\"anchorDutyValidDay\\\",\\\"clauseName\\\":\\\"主播开播有效天\\\",\\\"anchor\\\":{\\\"requiredDetail\\\":2,\\\"unit\\\":2,\\\"concrete\\\":\\\"25\\\"}},{\\\"clauseKey\\\":\\\"anchorDutyDuration\\\",\\\"clauseName\\\":\\\"主播开播有效时长\\\",\\\"anchor\\\":{\\\"requiredDetail\\\":1,\\\"unit\\\":2,\\\"concrete\\\":\\\"150\\\"}},{\\\"clauseKey\\\":\\\"anchorDutyVideo\\\",\\\"clauseName\\\":\\\"主播发布短视频\\\",\\\"anchor\\\":{\\\"requiredDetail\\\":3,\\\"unit\\\":2,\\\"concrete\\\":\\\"5\\\"}}]\",\"leave_penalty_type\":0,\"share_percents\":{\"1\":70,\"4\":90,\"15\":0},\"faction_anchor_category_info\":{\"faction_anchor_live_type\":2,\"faction_anchor_live_category_id\":\"4417\"},\"faction_invite_anchor_type\":1,\"leave_faction_rights\":{\"leave_rights_version\":1,\"unconditional_leave_period\":2},\"is_batch_live\":false,\"recruit_source\":" + recruit_source + ",\"recruit_broker_id\":\"" + jjranchor_id + "\",\"relieved_join\":2,\"function_version\":{\"probation_leave_version\":1},\"contain_join_faction_demand\":false,\"is_game_live\":false,\"is_faction_competition_live\":false,\"is_oversea_invite\":false}";
                        var inviteobj = UtilityStatic.HttpHelper.HttpPost($"https://union.bytedance.com/ark/api/uanchor/invite/submit_anchor_invite", invitepoststring, new UtilityStatic.HttpHelper.HttpPostReq
                        {
                            contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad,
                            headers = headers
                        }).ToJObject();
                        if (inviteobj["status_code"].ToNullableString() == "0" && inviteobj["message"].ToNullableString() == "success")//注册成功
                        {
                            result.data = new
                            {
                                invite_id = inviteobj["data"]["invite_id"].ToString(),
                                flow_id = inviteobj["data"]["flow_id"].ToString()
                            };
                        }
                        else
                        {
                            result.code = 1;
                            result.msg = inviteobj["message"].ToNullableString();

                        }
                    }
                    else
                    {
                        result.code = 1;
                        result.msg = obj["data"]["stripe_message"].ToString();

                    }
                }
                else
                {
                    switch (obj["status_code"].ToNullableString())
                    {
                        case "130000":
                            result.code = 1;
                            result.msg = obj["message"].ToNullableString();
                            break;
                        default:
                            result.code = 1;
                            result.msg = obj["msg"].ToNullableString();
                            break;
                    }
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