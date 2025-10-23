using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ApproveApplicationController : BaseLoginController
    {
        #region 审批List
        public ActionResult ApproveApplicationList(string tg_user_sn, string dateRange)
        {
            var req = new PageFactory.JoinNew.App_YYApproveApplyZb.DtoReq();
            req.tg_user_sn = tg_user_sn;
            req.dateRange = dateRange;
            var pageModel = new PageFactory.JoinNew.App_YYApproveApplyZb().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listFilter.formItems.Find(x => x.name == "status").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.p_join_apply.status_enum.等待运营审批.ToSByte()}";
            return View(pageModel);
        }
        #endregion

        #region 审批页面
        public ActionResult ApproveApplicationPost(int id)
        {
            var req = new PageFactory.JoinNew.App_YYApproveZbPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.App_YYApproveZbPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 主播名单
        /// <summary>
        /// 主播名单
        /// </summary>
        /// <param name="apply_sn"></param>
        /// <param name="para_status">列表点击人数弹出页面筛选状态</param>
        /// <returns></returns>
        public ActionResult ZbList(string apply_sn, string para_status = "")
        {
            var req = new PageFactory.JoinNew.Stu_ZbList.DtoReq();
            req.para_status = para_status;
            var p_join_apply = DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'");
            req.tg_need_id = p_join_apply.id;
            var pageModel = new PageFactory.JoinNew.Stu_ZbList().Get(req);
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "BatchPost").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn='{p_join_apply.tg_user_sn}' and tg_need_id = '{p_join_apply.id}'";
            if (!para_status.IsNullOrEmpty())
            {
                pageModel.listFilter.formItems.Find(x => x.name == "status").disabled = true;
            }
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

        #region 补人查询
        public ActionResult QueryList()
        {
            var req = new PageFactory.JoinNew.App_YYApproveApplyZb.DtoReq();
            var pageModel = new PageFactory.JoinNew.App_YYApproveApplyZb().Get(req);
            pageModel.listDisplay.listItems.Find(x => x.field == "zhubo_text").disabled = true;
            pageModel.listDisplay.listItems.Find(x => x.field == "apply_sn").disabled = false;
            pageModel.listDisplay.listItems.Find(x => x.field == "top_text").disabled = false;
            return View(pageModel);
        }
        #endregion
    }
}