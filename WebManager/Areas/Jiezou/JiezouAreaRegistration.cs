using System.Web.Mvc;

namespace WebProject.Areas.Jiezou
{
    public class JiezouAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Jiezou";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Jiezou_default",
                "Jiezou/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}