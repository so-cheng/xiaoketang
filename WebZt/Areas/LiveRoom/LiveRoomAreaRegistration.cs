

using System.Web.Mvc;

namespace WebProject.Areas.LiveRoom
{
    public class LiveRoomAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LiveRoom";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "LiveRoom_default",
                "LiveRoom/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}