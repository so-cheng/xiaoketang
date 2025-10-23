using Services.Project;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    public class ApproveApplicationController : Controller
    {
        #region 审批List
        public ActionResult ApproveApplicationList()
        {
            var req = new PageFactory.Join.YYApproveApplyZb.DtoReq();
            var pageModel = new PageFactory.Join.YYApproveApplyZb().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 审批页面
        public ActionResult ApproveApplicationPost(int id)
        {
            var req = new PageFactory.Join.YYApproveZbPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.YYApproveZbPost().Get(req);
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

        #region 数据分析
        public ActionResult ApproveAnalyse(PageFactory.Join.YyApproveAnalyse.DtoReq req)
        {
            var pageModel = new PageFactory.Join.YyApproveAnalyse().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 流失明细
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QuitList(PageFactory.Join.YyQuitList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.YyQuitList().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}