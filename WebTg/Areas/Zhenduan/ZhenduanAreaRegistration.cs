using System.Web.Mvc;

namespace WebProject.Areas.Zhenduan
{
    public class ZhenduanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Zhenduan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Zhenduan_default",
                "Zhenduan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}