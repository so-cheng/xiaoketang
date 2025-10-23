using System.Web.Mvc;

namespace WebProject.Areas.PUserBasic
{
    public class PUserBasicAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PUserBasic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PUserBasic_default",
                "PUserBasic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}