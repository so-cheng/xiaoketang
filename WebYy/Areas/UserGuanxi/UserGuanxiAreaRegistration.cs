using System.Web.Mvc;

namespace WebProject.Areas.UserGuanxi
{
    public class UserGuanxiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserGuanxi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UserGuanxi_default",
                "UserGuanxi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}