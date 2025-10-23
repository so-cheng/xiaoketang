using Services.Project;
using System.Collections.Generic;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserRelationManage.Controllers
{
    public class UserRelationController : BaseLoginController
    {
        #region 转移厅管与主播
        /// <summary>
        /// 转移厅管
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MovePost(int type_id = 1)//厅管邀主播
        {
            var req = new PageFactory.YYMovePost.DtoReq();
            req.type_id = type_id;
            var pageModel = new PageFactory.YYMovePost().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 转移记录
        /// <summary>
        /// 转移厅管记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveList(int type_id)
        {
            var req = new PageFactory.YYMoveList.DtoReq();
            req.type_id = type_id;
            var pageModel = new PageFactory.YYMoveList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 解绑关系
        /// <summary>
        /// 解绑关系
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveUserRelationShip()
        {
            var req = new PageFactory.YYRemoveList.DtoReq();
            var pageModel = new PageFactory.YYRemoveList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 用户关系绑定
        /// <summary>
        /// 用户关系解绑
        /// </summary>
        /// <returns></returns>
        public ActionResult BindUserRelationShip(int type_id = 2)//运营邀厅管
        {
            var req = new PageFactory.YYBindList.DtoReq();
            req.type_id = type_id;
            var pageModel = new PageFactory.YYBindList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 通用
        /// <summary>
        /// 选择user_sn下属的用户页面
        /// <returns></returns>
        public ActionResult Select(string user_sn, int type_id, int isolated = 0)
        {
            var req = new PageFactory.YYSelect.DtoReq();
            req.isolated = isolated;//1:孤立用户，0:不是孤立用户
            req.type_id = type_id;
            req.user_sn = user_sn;
            var pageModel = new PageFactory.YYSelect().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}