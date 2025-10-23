using System.Web.Mvc;

namespace WebProject.Areas.TingZhan
{
    /// <summary>
    /// 厅战活动
    /// </summary>
    public class TingZhanAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TingZhan";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TingZhan_default",
                "TingZhan/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}