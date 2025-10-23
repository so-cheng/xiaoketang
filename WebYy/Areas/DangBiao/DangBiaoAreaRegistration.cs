using System.Web.Mvc;

namespace WebProject.Areas.DangBiao
{
    public class DangBiaoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DangBiao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DangBiao_default",
                "DangBiao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}