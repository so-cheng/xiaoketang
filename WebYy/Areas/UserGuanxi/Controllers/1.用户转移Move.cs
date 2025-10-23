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
        #region 主播转移训练厅
        /// <summary>
        /// 转移训练厅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoveToTrainPost()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTrainPost.DtoReq();
            req.yy_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTrainPost().Get(req);
            return View(pageModel);
        }



        #endregion

        #region 主播转移到普通厅
        /// <summary>
        /// 转移到厅管
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MovePost()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MovePost.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MovePost().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 转移记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveList()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveLogList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveLogList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }

        /// <summary>
        /// 选择user_sn下属的用户页面
        /// <returns></returns>
        public ActionResult ZhuboSelect(string user_sn, int type_id=1, int isolated = 0)
        {
            var req = new PageFactory.UserGuanxi.Zhubo_Select.DtoReq();
            req.isolated = isolated;//1:孤立用户，0:不是孤立用户
            req.type_id = type_id;
            req.user_sn = user_sn;
            var pageModel = new PageFactory.UserGuanxi.Zhubo_Select().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 审批厅管转移新主播申请
        /// <summary>
        /// 转移申请列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveToTingList()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTingList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTingList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listFilter.formItems.Clear();
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in ({new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(new UserIdentityBag().user_sn)}) and status = '{ModelDb.p_join_change_apply.status_enum.等待运营审批.ToSByte()}'";
            return View(pageModel);
        }
        #endregion

        #region 厅管跨团队转移
        public ActionResult TgMoveOtherYyPost()
        {
            var req = new PageFactory.UserGuanxi.Tg_MoveOtherYyPost.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Tg_MoveOtherYyPost().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 厅管团队内转移
        public ActionResult TgMovePost()
        {
            var req = new PageFactory.UserGuanxi.Tg_MovePost.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Tg_MovePost().Get(req);
            return View(pageModel);
        }
        #endregion

        /// <summary>
        /// 转移记录
        /// </summary>
        /// <returns></returns>
        public ActionResult TgMoveList()
        {
            var req = new PageFactory.UserGuanxi.Tg_MoveList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Tg_MoveList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            return View(pageModel);
        }

        public ActionResult TgSelect(string yy_sn)
        {
            var req = new PageFactory.UserGuanxi.Tg_Select.DtoReq();
            req.yy_sn = yy_sn;
            var pageModel = new PageFactory.UserGuanxi.Tg_Select().Get(req);
            return View(pageModel);
        }
    }
}