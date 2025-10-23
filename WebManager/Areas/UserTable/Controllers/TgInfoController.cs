using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Areas.UserTable.Controllers
{
    /// <summary>
    /// 厅管信息完整度排名，创建日期:2025-06-11
    /// </summary>
    public class TgInfoController : BaseLoginController
    {
        // GET: TgManage/Rank
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RankList(ServiceFactory.UserTable.TgInfoRankModel req)
        {

            return View();
        }

        public ActionResult List(string yy_user_sn)
        {
            var req = new PageFactory.UserInfo.TgInfoList.DtoReq();
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = new UserIdentityBag().user_sn;
            }
            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $" yy_user_sn = '{yy_user_sn}' and (dou_user = '' or dou_user is null or manager_wx = '' or manager_wx is null or dou_UID = '' or dou_UID is null or tg_sex = '' or tg_sex is null)";
            return View(pageModel);
        }

        /// <summary>
        /// 编辑厅
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoEdit(int id = 0)
        {
            var req = new PageFactory.UserInfo.TgInfoEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.TgInfoEdit().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post(PageFactory.UserTable.UserCreate.DtoReq req)
        {
            req.user_type = "tger";
            var pageModel = new PageFactory.UserTable.UserCreate().Get(req);
            return View(pageModel);
        }
    }
}