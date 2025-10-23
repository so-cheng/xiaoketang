using Services.Project;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboController : BaseLoginController
    {
        #region 新的主播
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult NewList()
        {
            var req = new PageFactory.UserInfo.NewList.DtoReq();
            var pageModel = new PageFactory.UserInfo.NewList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 流失名单
        public ActionResult LossList()
        {
            var req = new PageFactory.UserInfo.LossList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LossList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 日志
        public ActionResult LogList()
        {
            var req = new PageFactory.UserInfo.LogList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LogList().Get(req);
            //pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 主播账号
        public ActionResult AccountList()
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountList.DtoReq();
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountList().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}