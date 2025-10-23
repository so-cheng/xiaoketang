using System.Web.Mvc;

namespace WebProject.Areas.Jixiao
{
    public class JixiaoAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 绩效上报
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Jixiao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Jixiao_default",
                "Jixiao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}