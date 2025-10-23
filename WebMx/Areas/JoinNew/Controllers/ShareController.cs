using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ShareController : BaseLoginController
    {
        #region 分享二维码、资料收集
        public ActionResult Page()
        {
            return View();
        }
        #endregion

        #region 用户分级
        public ActionResult WaitShare()
        {
            var req = new PageFactory.JoinNew.Stu_MxList.DtoReq();
            req.orderby = "create_time desc";
            var pageModel = new PageFactory.JoinNew.Stu_MxList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "level").FirstOrDefault().disabled = false;//批量分级开启
            pageModel.listDisplay.listData.attachFilterSql = $" mx_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.等待分级.ToInt()}'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 快速分级
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult FastLevel(string ids)
        {
            var req = new PageFactory.JoinNew.FastLevel.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.JoinNew.FastLevel().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 流失操作页面
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

        #region 等待拉群
        /// <summary>
        /// 等待拉群
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult WaitQun()
        {
            var req = new PageFactory.JoinNew.WaitQun.DtoReq();
            var pageModel = new PageFactory.JoinNew.WaitQun().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "group").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"mx_sn = '{new UserIdentityBag().user_sn}' AND tg_user_sn != '' and status = '{ModelDb.p_join_new_info.status_enum.等待拉群.ToInt()}'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 已拉群
        /// <summary>
        /// 已拉群
        /// </summary>
        /// <returns></returns>
        public ActionResult Qun()
        {
            var req = new PageFactory.JoinNew.Qun.DtoReq();
            var pageModel = new PageFactory.JoinNew.Qun().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 已经流失
        /// <summary>
        /// 已经流失
        /// </summary>
        /// <returns></returns>
        public ActionResult ZBQuited()
        {
            var req = new PageFactory.JoinNew.Stu_MxList.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_MxList().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Edit").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "CausePost").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()} and mx_sn = '{new UserIdentityBag().user_sn}'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 查询归属
        public ActionResult Seach()
        {
            var req = new PageFactory.JoinNew.Stu_SearchList.DtoReq();
            req.orderby = " order by create_time desc";
            var pageModel = new PageFactory.JoinNew.Stu_SearchList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        #endregion

        #region 暂未分配
        public ActionResult UnShare()
        {
            var req = new PageFactory.JoinNew.UnShareList.DtoReq();
            var pageModel = new PageFactory.JoinNew.UnShareList().Get(req);
            //pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listData.attachFilterSql = $"(mx_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.等待分配.ToInt()}')";
            return View(pageModel);
        }
        #endregion

        #region 分厅:选择直播厅档位明细
        /// <summary>
        /// 分厅:选择直播厅档位明细
        /// </summary>
        /// <param name="dateRange"></param>
        /// <param name="p_join_new_info_id"></param>
        /// <returns></returns>
        public ActionResult WxChooseJoinApplyPost(string dateRange, int p_join_new_info_id)
        {
            var req = new PageFactory.JoinNew.WX_ChooseJoinApplyList.DtoReq();
            req.dateRange = dateRange;
            req.p_join_new_info_id = p_join_new_info_id;
            var pageModel = new PageFactory.JoinNew.WX_ChooseJoinApplyList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 主播名单
        /// <summary>
        /// 主播名单
        /// </summary>
        /// <param name="apply_item_id"></param>
        /// <param name="para_status">列表点击人数弹出页面筛选状态</param>
        /// <returns></returns>
        public ActionResult ZbList(int apply_item_id, string para_status = "")
        {
            var req = new PageFactory.JoinNew.Stu_ZbList.DtoReq();
            req.apply_item_id = apply_item_id;
            req.para_status = para_status;
            var p_join_apply_item = DoMySql.FindEntityById<ModelDb.p_join_apply_item>(req.apply_item_id);
            var p_join_apply = DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{p_join_apply_item.apply_sn}'");
            req.tg_need_id = p_join_apply.id;
            var pageModel = new PageFactory.JoinNew.Stu_ZbList().Get(req);
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "BatchPost").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $" tg_user_sn='{p_join_apply.tg_user_sn}' and tg_dangwei = '{p_join_apply_item.id}'";
            if (!para_status.IsNullOrEmpty())
            {
                pageModel.listFilter.formItems.Find(x => x.name == "status").disabled = true;
            }
            return View(pageModel);
        }
        #endregion

        #region 暂不分配
        public ActionResult NoShare()
        {
            var req = new PageFactory.JoinNew.UndistributedList.DtoReq();
            var pageModel = new PageFactory.JoinNew.UndistributedList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"(mx_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.暂不分配.ToInt()}')";
            return View(pageModel);
        }
        #endregion

        #region 改抖音号
        /// <summary>
        /// 待修改抖音号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult ChangeDyAccountList()
        {
            var req = new PageFactory.JoinNew.ChangeDyAccountList.DtoReq();
            var pageModel = new PageFactory.JoinNew.ChangeDyAccountList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listData.attachFilterSql = $"(mx_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.改抖音号.ToInt()}')";
            return View(pageModel);
        }
        /// <summary>
        /// 修改抖音号操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult ChangeDyAction(string id)
        {
            var req = new PageFactory.JoinNew.ChangeDyAction.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.ChangeDyAction().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 有对接厅
        /// <summary>
        /// 萌新端已有对接厅列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult HavingTing()
        {
            var req = new PageFactory.JoinNew.HavingTing.DtoReq();
            var pageModel = new PageFactory.JoinNew.HavingTing().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listData.attachFilterSql = $"(mx_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_new_info.status_enum.有对接厅.ToInt()}')";
            return View(pageModel);
        }
        /// <summary>
        /// 萌新端抖音经纪人已有对接厅转厅操作
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult ChangeTingAction(string id)
        {
            var req = new PageFactory.JoinNew.ChangeTingAction.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.ChangeTingAction().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 等待退回
        /// <summary>
        /// 等待退回
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitBack()
        {
            var req = new PageFactory.JoinNew.WaitBack.DtoReq();
            var pageModel = new PageFactory.JoinNew.WaitBack().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"mx_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 退回审批同意：重新分配
        /// <summary>
        /// 退回审批同意：重新分配
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetPost(int id)
        {
            var req = new PageFactory.JoinNew.ResetPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.ResetPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 退回审批拒绝：取消退回
        /// <summary>
        /// 退回审批拒绝：取消退回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BackCancelPost(int id)
        {
            var req = new PageFactory.JoinNew.BackCancelPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.BackCancelPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 退回主播
        /// <summary>
        /// 退回主播
        /// </summary>
        /// <returns></returns>
        public ActionResult Back()
        {
            var req = new PageFactory.JoinNew.Back.DtoReq();
            var pageModel = new PageFactory.JoinNew.Back().Get(req);
            pageModel.listFilter.formItems.Find(x => x.name == "mx_sn").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"mx_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 分配日志
        /// <summary>
        /// 分配日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Log(int id = 0)
        {
            var req = new PageFactory.JoinNew.ShareLog.DtoReq();
            var pageModel = new PageFactory.JoinNew.ShareLog().Get(req);
            if (id > 0)
            {
                pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id = {id}";
            }
            else
            {
                // 当前萌新下的日志
                pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id in (select id from p_join_new_info where mx_sn = '{new UserIdentityBag().user_sn}')";
            }
            return View(pageModel);
        }
        #endregion

        #region 申请日志
        /// <summary>
        /// 申请日志
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyLog(string apply_sn = "")
        {
            var req = new PageFactory.JoinNew.ApplyLog.DtoReq();
            req.apply_sn = apply_sn;
            var pageModel = new PageFactory.JoinNew.ApplyLog().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 查询归属
        public ActionResult Search(PageFactory.JoinNew.Search.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.Search().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}