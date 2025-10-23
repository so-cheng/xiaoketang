using System.Web.Mvc;

namespace WebProject.Areas.KuaFang
{
    public class KuaFangAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KuaFang";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KuaFang_default",
                "KuaFang/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}