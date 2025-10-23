using System.Web.Mvc;

namespace WebProject.Areas.Xianxiazp
{
    public class XianxiazpAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Xianxiazp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Xianxiazp_default",
                "Xianxiazp/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}