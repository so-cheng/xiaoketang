using System.Web.Mvc;

namespace WebProject.Areas.Xianxiabiao
{
    public class XianxiabiaoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Xianxiabiao";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Xianxiabiao_default",
                "Xianxiabiao/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}