using System.Web.Mvc;

namespace WebProject.Areas.PBasic
{
    public class PBasicAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PBasic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PBasic_default",
                "PBasic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}