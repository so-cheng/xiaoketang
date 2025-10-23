using Services.Project;
using System.Linq;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas._2.ZbManage.Controllers
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

        public ActionResult ZbList(PageFactory.Join.ZbList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            if (req.isTotalInfo)
            {
                pageModel.listDisplay.listItems.Where(x => x.field == "qun_time").FirstOrDefault().disabled = false;
                pageModel.listFilter.formItems.Where(x => x.name == "month").FirstOrDefault().disabled = false;
                pageModel.listFilter.formItems.Where(x => x.name == "status").FirstOrDefault().disabled = false;
                pageModel.listDisplay.listOperateItems.Where(x => x.name == "CausePost").FirstOrDefault().disabled = false;
            }

            return View(pageModel);
        }

        /// <summary>
        /// 退回主播背调
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackPost(PageFactory.Join.ZbBackPost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ZbBackPost().Get(req);
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

        #region 主动领取

        public ActionResult ZhuDongLingQu(int id = 0)
        {
            var req = new PageFactory.Join.ZhuDongLingQu.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.ZhuDongLingQu().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 主动领取_补人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dangwei"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public ActionResult Zdlq_ChooseZbPost(int id = 0, int dangwei = 0, string tg_user_sn = "")
        {
            var req = new PageFactory.Join.Zdlq_ChooseZbList.DtoReq();
            req.tg_dangwei = dangwei;
            req.tg_need_id = id;
            req.tg_user_sn = tg_user_sn;
            var pageModel = new PageFactory.Join.Zdlq_ChooseZbList().Get(req);
            pageModel.listDisplay.listItems.Where(x => x.field == "zb_level_text").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listItems.Where(x => x.field == "wechat_username").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listItems.Where(x => x.field == "dou_username").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listItems.Where(x => x.field == "note").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        #endregion
    }
}