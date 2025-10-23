using System.Web.Mvc;

namespace WebProject.Areas.PCrm
{
    public class CrmAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PCrm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PCrm_default",
                "PCrm/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}