using Services.Project;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboLogController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            string sql = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForSql(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
            {
                attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                {
                    userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.运营,
                    UserSn = new UserIdentityBag().user_sn
                }
            });
            var req = new PageFactory.UserInfo.LogList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"(user_info_zb_sn in ({sql}))";
            return View(pageModel);
        }
    }
}