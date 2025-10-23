using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TingController : BaseLoginController
    {
        /// <summary>
        /// 直播厅列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OnJobList()
        {
           
            var req = new PageFactory.UserInfo.TgInfoList.DtoReq();
            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zt_user_sn = '{new UserIdentityBag().user_sn}'";
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Del").disabled = false;
            return View(pageModel);
            
        }
        /// <summary>
        /// 直播厅恢复
        /// </summary>
        /// <returns></returns>
        public ActionResult CloseList()
        {
            var req = new PageFactory.UserInfo.CloseList.DtoReq();
            var pageModel = new PageFactory.UserInfo.CloseList().Get(req);
            return View(pageModel);
        }

        [HttpGet]
        public ActionResult InfoEdit(int id = 0)
        {
            var req = new PageFactory.UserInfo.TgInfoEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.TgInfoEdit().Get(req);
            return View(pageModel);
        }
    }
}