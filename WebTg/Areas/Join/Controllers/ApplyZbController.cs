using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    public class ApplyZbController : BaseLoginController
    {
        #region 申请主播
        [HttpGet]
        public ActionResult ApplyZbPost(PageFactory.Join.ApplyZbPost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ApplyZbPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 申请记录
        public ActionResult ApplyZbList()
        {
            var req = new PageFactory.Join.ApplyZbList.DtoReq();
            var pageModel = new PageFactory.Join.ApplyZbList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 主播名单
        public ActionResult ZbList(int id = 0)
        {
            var req = new PageFactory.Join.ZbList.DtoReq();
            req.tg_need_id = id;
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 退回主播
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackPost(PageFactory.Join.ZbBackPost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ZbBackPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 主播汇总
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ZbTotalList()
        {
            var req = new PageFactory.Join.ZbList.DtoReq();
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            pageModel.listDisplay.listItems.Where(x => x.field == "qun_time").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "month").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "status").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "CausePost").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
        public ActionResult CausePost(PageFactory.Join.ZbCausePost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ZbCausePost().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 补人表单详情
        public ActionResult ZbDetails(int id = 0)
        {
            var req = new PageFactory.Join.ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.ZbDetails().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}