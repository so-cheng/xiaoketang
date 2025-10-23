using System.Web.Mvc;

namespace WebProject.Areas.JixiaoDay
{
    public class JixiaoDayAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 绩效上报
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "JixiaoDay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "JixiaoDay_default",
                "JixiaoDay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}