using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class TrainingController : BaseLoginController
    {
        #region 学员培训
        /// <summary>
        /// 厅管端待培训学院列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult WaitTraining()
        {
            var req = new PageFactory.JoinNew.WaitTraining.DtoReq();
            var pageModel = new PageFactory.JoinNew.WaitTraining().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listData.attachFilterSql = $"(tg_user_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.等待培训.ToInt()}')";
            return View(pageModel);
        }
        #endregion

        #region 退回
        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackPost(string ids, int tg_need_id)
        {
            var req = new PageFactory.JoinNew.Stu_BackPost.DtoReq();
            req.ids = ids;
            req.tg_need_id = tg_need_id;
            var pageModel = new PageFactory.JoinNew.Stu_BackPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 流失
        /// <summary>
        /// 流失
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult CausePost(string id)
        {
            var req = new PageFactory.JoinNew.Stu_CausePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.Stu_CausePost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 分配日志
        /// <summary>
        /// 分配日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Log(int id)
        {
            var req = new PageFactory.JoinNew.ShareLog.DtoReq();
            var pageModel = new PageFactory.JoinNew.ShareLog().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id = {id}";
            return View(pageModel);
        }
        #endregion
    }
}