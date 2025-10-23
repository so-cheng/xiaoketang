﻿using System.Web.Mvc;

namespace WebProject.Areas.UserInfo
{
    public class UserInfoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserInfo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserInfo_default",
                "UserInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}