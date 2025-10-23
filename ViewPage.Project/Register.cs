using MvcContrib.PortableAreas;
using System.Web.Mvc;

namespace ViewPage.Project
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewPageAreaRegistration : PortableAreaRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "ViewPage";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bus"></param>
        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) 
        {
            context.MapRoute(
                "ViewPage_default",
                "ViewPage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "ViewPage.Project.Controllers" }
            );
            RegisterAreaEmbeddedResources();
        }
    }
}