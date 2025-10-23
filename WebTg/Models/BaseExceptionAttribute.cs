using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Services.Project
{
    /// <summary>
    /// 异常捕获
    /// </summary>
    public class BaseExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Common.Core.TxtLog.Error(filterContext.Exception.Message);
            //推送到微信消息

            base.OnException(filterContext);
        }
    }
}