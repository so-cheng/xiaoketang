using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ApplyZbController : BaseLoginController
    {
        #region 申请主播

        [HttpGet]
        public ActionResult ApplyZbPost(string ting_sn, int id = 0)
        {
            var req = new PageFactory.JoinNew.App_ZbPost.DtoReq();
            req.id = id;
            req.ting_sn = ting_sn;
            var pageModel = new PageFactory.JoinNew.App_ZbPost().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 申请记录

        public ActionResult ApplyZbList()
        {
            var req = new PageFactory.JoinNew.App_ZbList.DtoReq();
            var pageModel = new PageFactory.JoinNew.App_ZbList().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 主播已补名单

        /// <summary>
        /// 主播明细
        /// </summary>
        /// <param name="tg_need_id"></param>
        /// <param name="apply_item_id"></param>
        /// <param name="isTotalInfo"></param>
        /// <param name="para_status">列表点击人数弹出页面筛选状态</param>
        /// <returns></returns>
        public ActionResult ZbList(int tg_need_id, int apply_item_id = 0, bool isTotalInfo = true, string para_status = "")
        {
            var req = new PageFactory.JoinNew.Stu_ZbList.DtoReq();
            req.tg_need_id = tg_need_id;
            req.apply_item_id = apply_item_id;
            req.isTotalInfo = isTotalInfo;
            req.para_status = para_status;
            var pageModel = new PageFactory.JoinNew.Stu_ZbList().Get(req);
            if (req.isTotalInfo)
            {
                pageModel.listDisplay.listItems.Where(x => x.field == "qun_time").FirstOrDefault().disabled = false;
                pageModel.listFilter.formItems.Where(x => x.name == "month").FirstOrDefault().disabled = false;
                pageModel.listFilter.formItems.Where(x => x.name == "status").FirstOrDefault().disabled = false;
            }
            if (!para_status.IsNullOrEmpty())
            {
                pageModel.listFilter.formItems.Find(x => x.name == "status").disabled = true;
            }

            return View(pageModel);
        }

        /// <summary>
        /// 修改主播背调
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BatchPost(int id)
        {
            var req = new PageFactory.JoinNew.Stu_UserBatchPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.Stu_UserBatchPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 退回主播背调
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

        /// <summary>
        /// 流失原因
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

        #region 补人表单详情

        public ActionResult ZbDetails(int id = 0)
        {
            var req = new PageFactory.JoinNew.App_ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.App_ZbDetails().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 申请日志
        /// <summary>
        /// 申请日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Log(string apply_sn = "")
        {
            var req = new PageFactory.JoinNew.ApplyLog.DtoReq();
            req.apply_sn = apply_sn;
            var pageModel = new PageFactory.JoinNew.ApplyLog().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}