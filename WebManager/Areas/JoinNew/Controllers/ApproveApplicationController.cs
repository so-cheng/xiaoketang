using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ApproveApplicationController : BaseLoginController
    {
        #region 审批List
        public ActionResult ApproveApplicationList()
        {
            var req = new PageFactory.JoinNew.App_ApproveApplyZb.DtoReq();
            var pageModel = new PageFactory.JoinNew.App_ApproveApplyZb().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "create_time").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_sex").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"(status='{ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()}')";
            return View(pageModel);
        }

        /// <summary>
        /// 补人查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveQueryList()
        {
            var req = new PageFactory.JoinNew.App_ApproveApplyZb.DtoReq();
            var pageModel = new PageFactory.JoinNew.App_ApproveApplyZb().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "status").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "create_time").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_sex").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        #endregion

        #region 审批页面
        public ActionResult ApproveApplicationPost(int id)
        {
            var req = new PageFactory.JoinNew.App_ApproveZbPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.App_ApproveZbPost().Get(req);
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

        #region 免审白名单List
        public ActionResult WhiteList()
        {
            var req = new PageFactory.JoinNew.WhiteList.DtoReq();
            var pageModel = new PageFactory.JoinNew.WhiteList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 添加免审白名单
        public ActionResult AddWhite(string yy_user_sn = "")
        {
            var req = new PageFactory.JoinNew.WhitePost.DtoReq();
            req.yy_user_sn = yy_user_sn;
            var pageModel = new PageFactory.JoinNew.WhitePost().Get(req);
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