using System.Web.Mvc;

namespace WebProject.Areas.QianYue
{
    public class QianYueAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QianYue";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QianYue_default",
                "QianYue/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}