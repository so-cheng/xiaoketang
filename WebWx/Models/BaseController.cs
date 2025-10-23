using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using Common.Core;
using DataBase.Core;

namespace Services.Project
{
    [BaseException]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //判断action是否有CheckLogin特性
            try
            {
                checkGetTimes();
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
                Response.End();
            }
        }

        /// <summary>
        /// 返回JsonResult
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="contentEncoding">内容编码</param>
        /// <param name="behavior">行为</param>
        /// <returns>JsonReuslt</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                FormateStr = "yyyy-MM-dd HH:mm:ss"
            };
        }

        /// <summary>
        /// 返回JsonResult.24
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult MyJson(object data, JsonRequestBehavior behavior, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                FormateStr = format
            };
        }

        /// <summary>
        /// 返回JsonResult42     
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <returns>Json</returns>
        protected JsonResult MyJson(object data, string format)
        {
            return new CustomJsonResult
            {
                Data = data,
                FormateStr = format
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected ViewResult View(PageModelBase model)
        {
            return View(model.pageCshtmlPath, model);
        }

        protected void checkGetTimes()
        {
        }
    }

    /// <summary>
      /// 自定义Json视图
      /// </summary>
    public class CustomJsonResult : JsonResult
    {
        /// <summary>
            /// 格式化字符串
            /// </summary>
        public string FormateStr
        {
            get;
            set;
        }

        /// <summary>
            /// 重写执行视图
            /// </summary>
            /// <param name="context">上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string jsonString = jss.Serialize(Data);
                string p = @"\\/Date\((\d+)\)\\/";
                MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
                Regex reg = new Regex(p);
                jsonString = reg.Replace(jsonString, matchEvaluator);

                response.Write(jsonString);
            }
        }

        /// <summary> 
        /// 将Json序列化的时间由/Date(1294499956278)转为字符串 .
        /// </summary> 
        /// <param name="m">正则匹配</param>
        /// <returns>格式化后的字符串</returns>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(FormateStr);
            return result;
        }
    }
}