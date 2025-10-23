using Services.Project;
using System.Collections.Generic;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserGuanxi.Controllers
{
    public class MoveController : BaseLoginController
    {
        #region 转厅申请抖音后台操作状态展示
        public ActionResult changeShowList(PageFactory.UserGuanxi.ChangeList.DtoReq req)
        {
            var pageModel = new PageFactory.UserGuanxi.ChangeList().Get(req);
            return View(pageModel);
        }
        #endregion


        public ActionResult YyMoveList()
        {
            var req = new PageFactory.UserGuanxi.Yy_RelationList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Yy_RelationList().Get(req);
            return View(pageModel);
        }
        public ActionResult YyMovePost()
        {
            var req = new PageFactory.UserGuanxi.Yy_RelationPost.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Yy_RelationPost().Get(req);
            return View(pageModel);
        }
    }
}