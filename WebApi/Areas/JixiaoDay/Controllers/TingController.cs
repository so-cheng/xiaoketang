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
    public class TingController : BaseController
    {
        /// <summary>
        /// 获取单厅数据
        /// </summary>
        /// <param name="dou_UID">抖音UID</param>
        /// <param name="s_date">开始日期</param>
        /// <param name="e_date">结束日期</param>
        /// <returns></returns>
        public JsonResult GetCoreData(string dou_UID, string s_date = "", string e_date = "")
        {
            var result = new JsonResultAction();
            try
            {
                //string anchorID = DoMySql.FindEntity<ModelDb.user_info_tg>($"dou_user = '{dou_username}'", false).dou_UID;
                if (s_date.IsNullOrEmpty()) s_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                if (e_date.IsNullOrEmpty()) e_date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                var headers = new Dictionary<string, string>();
                headers.Add("cookie", UtilityStatic.ConfigHelper.GetConfigString("Cookie"));

                var obj = UtilityStatic.HttpHelper.HttpGet($"https://union.bytedance.com/ark/api/uanchor/pugna_component/data/anchor/portrait/core_day_stats/c80ehg3c77uelq3kif90?anchorID={dou_UID}&beginDate={s_date}&endDate={e_date}&orderType=desc&orderField=date&page=1&size=10", headers).ToJObject();
                //UtilityStatic.TxtLog.Info(reStr);
                string data_string = Convert.ToString(obj["data"]["data_string"]);
                var dataObject = (JObject)JsonConvert.DeserializeObject(data_string);

                string score = Convert.ToString(dataObject["data"][0]["score"]);   //抖音昵称
                result.data = new
                {
                    score = score
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