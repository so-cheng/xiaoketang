using System.Web.Mvc;

namespace WebProject.Areas.DataAnalysis
{
    public class DataAnalysisAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataAnalysis";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataAnalysis_default",
                "DataAnalysis/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}