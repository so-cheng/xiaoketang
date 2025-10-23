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
    public class CreateZBAccountController : BaseLoginController
    {
        #region 加急处理主播名单
        /// <summary>
        /// 加急处理主播名单
        /// </summary>
        /// <returns></returns>
        public ActionResult FastZbList()
        {
            var req = new PageFactory.JoinNew.WX_FastZbList.DtoReq();
            var pageModel = new PageFactory.JoinNew.WX_FastZbList().Get(req);
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

        #region 显示厅管申请列表
        public ActionResult TGApplicationList(string dateRange, int completeStatus = -1)
        {
            var req = new PageFactory.JoinNew.TGApplyZbList.DtoReq();
            req.dateRange = dateRange;
            req.completeStatus = completeStatus;
            var pageModel = new PageFactory.JoinNew.TGApplyZbList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 补人:选择萌新表单
        public ActionResult MxChooseZbPost(int completeStatus, int apply_item_id = 0)
        {
            var req = new PageFactory.JoinNew.WX_ChooseZbList.DtoReq();
            var applyItemDetail = DoMySql.FindEntityBySql<ServiceFactory.JoinNew.applyItemDetails>($"select t1.*,t2.tg_sex,t2.manager,t2.ting_sn,t2.apply_cause,t2.status as apply_status,t2.id as apply_id from p_join_apply_item t1 left join p_join_apply t2 on t1.apply_sn = t2.apply_sn where t1.id = '{apply_item_id}'");
            req.apply_item_id = apply_item_id;
            req.apply_id = applyItemDetail.apply_id;
            req.ting_sn = applyItemDetail.ting_sn;
            var pageModel = new PageFactory.JoinNew.WX_ChooseZbList().Get(req);

            // 优先分配只展示优先地区的人
            if (completeStatus == 10)
            {
                pageModel.listDisplay.listData.attachFilterSql = "p_join_new_citys.priority is not null";
            }
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

        /// <summary>
        /// 退回主播
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
    }
}