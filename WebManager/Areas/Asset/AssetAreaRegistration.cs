using System.Web.Mvc;

namespace WebProject.Areas.Asset
{
    public class AssetAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 资产管理
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Asset";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Asset_default",
                "Asset/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}