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
        #region 优先分配
        public ActionResult Priority()
        {
            var req = new PageFactory.JoinNew.UnShareList.DtoReq();
            var pageModel = new PageFactory.JoinNew.UnShareList().Get(req);
            pageModel.listFilter.formItems.Find(x => x.name == "mx_sn").disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Edit").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "CausePost").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Lock").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Release").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"(status = '{ModelDb.p_join_new_info.status_enum.等待分配.ToInt()}' or status = '{ModelDb.p_join_new_info.status_enum.中台锁定.ToInt()}') and exists (select 1 from p_join_new_citys_zt where zt_user_sn = '{new UserIdentityBag().user_sn}' and name = SUBSTRING_INDEX(p_join_new_info.city, '市', 1))";
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

        #region 主播名单明细
        /// <summary>
        /// 主播名单明细
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

        #region 主播名单列表
        public ActionResult Search(PageFactory.JoinNew.Search.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.Search().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Edit").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter { attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType { userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地, UserSn = new UserIdentityBag().user_sn } })}";
            return View(pageModel);
        }
        #endregion
    }
}