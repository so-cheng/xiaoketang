using System.Web.Mvc;

namespace WebProject.Areas.UserTable
{
    /// <summary>
    /// 创建人：李俊杰，创建日期：2025-06-21
    /// </summary>
    public class UserTableAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserTable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserTable_default",
                "UserTable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}