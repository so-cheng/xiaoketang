using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    /// <summary>
    /// 表格
    /// </summary>
    public class TablesController : BaseLoginController
    {
        /// <summary>
        /// 数据分析
        /// </summary>
        /// <param name="yy_user_sn"></param>
        /// <returns></returns>
        public ActionResult Analysis(string yy_user_sn="")
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = new UserIdentityBag().user_sn;
            }
            var PJoinNeedAnalysis = new ServiceFactory.JoinAnalysisService().GetYyTable(yy_user_sn);
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.PJoinNeedAnalysis = PJoinNeedAnalysis;
            return View();
        }

        public ActionResult ApproveApplicationList(PageFactory.Join.YYApproveApplyZb.DtoReq req)
        {
            var pageModel = new PageFactory.Join.YYApproveApplyZb().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.listData.attachFilterSql = $" NOT (status = '{ModelDb.p_join_need.status_enum.已取消.ToInt()}' and supplement_count = 0) and status in ({ModelDb.p_join_need.status_enum.已完成.ToInt()},{ModelDb.p_join_need.status_enum.已取消.ToInt()},{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()})";
            return View(pageModel);
        }

        /// <summary>
        /// 流失主播名单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QuitZbList(PageFactory.Join.QuitedZbList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.QuitedZbList().Get(req);
            return View(pageModel);
        }

    }
}