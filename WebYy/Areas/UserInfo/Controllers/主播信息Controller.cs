using Services.Project;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboInfoController : BaseLoginController
    {
        #region 主播名单

        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountList.DtoReq(); 
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in ({new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }
        #endregion
    }
}