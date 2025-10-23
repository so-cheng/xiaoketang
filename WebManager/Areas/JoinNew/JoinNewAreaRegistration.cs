using System.Web.Mvc;

namespace WebProject.Areas.JoinNew
{
    public class JoinNewAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JoinNew";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JoinNew_default",
                "JoinNew/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}