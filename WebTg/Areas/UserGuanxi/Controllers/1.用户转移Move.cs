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
        #region 主播转移
        /// <summary>
        /// 转移训练厅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoveToTrainPost()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTrainPost.DtoReq();
            req.tg_user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTrainPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 转移到厅
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveToTingPost()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTingPost.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTingPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 转移申请列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveToTingList()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTingList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTingList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 厅管接收主播
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveToTingApprove()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveToTingApprove.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveToTingApprove().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"t_tg_user_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_change_apply.status_enum.等待对方同意.ToSByte()}'";
            return View(pageModel);
        }

        /// <summary>
        /// 转厅日志
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveLogList()
        {
            var req = new PageFactory.UserGuanxi.Zhubo_MoveLogList.DtoReq();
            var pageModel = new PageFactory.UserGuanxi.Zhubo_MoveLogList().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}