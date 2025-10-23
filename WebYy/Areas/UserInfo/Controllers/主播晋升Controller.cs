using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Domain;
using WeiCode.Services;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZbPromotionController : BaseLoginController
    {
        // GET: UserInfo/主播晋升
        /// <summary>
        /// 审批列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyList()
        {
            var req = new PageFactory.UserInfo.ZbPromotionList.DtoReq();
            var pageModel = new PageFactory.UserInfo.ZbPromotionList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_info_promotion_zhubo_apply.yy_user_sn = '{new UserIdentityBag().user_sn}'";
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Create").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="user_sn"></param>
        /// <returns></returns>
        public ActionResult Detail(string apply_sn)
        {
            var req = new PageFactory.UserInfo.ZbPromotionDetail.DtoReq();
            req.apply_sn = apply_sn;
            var pageModel = new PageFactory.UserInfo.ZbPromotionDetail().Get(req);
            return View(pageModel);
        }
    }
}