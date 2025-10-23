using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Areas.Target.Controllers
{
    /// <summary>
    /// 主播目标
    /// </summary>
    public class ZbTargetController : BaseLoginController
    {
        /// <summary>
        /// 月度目标（列表）
        /// </summary>
        /// <returns></returns>
        public ActionResult List(PageFactory.ZbTargetList.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetList().Get(req);
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    field_paras = "id",
                    url = "Edit"
                },
                text = "编辑",
            });
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";

            return View(pageModel);
        }

        /// <summary>
        /// 月度目标（编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(PageFactory.ZbTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 每日目标
        /// </summary>
        /// <returns></returns>
        public ActionResult DayTarget(string date, string tg_user_sn, string zb_user_sn, string type = "amount")
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (zb_user_sn == "undefined" || zb_user_sn.IsNullOrEmpty())
            {
                zb_user_sn = "";
            }
            if (tg_user_sn == "undefined" || tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)[0].user_sn;
            }
            if (type == "undefined" || type.IsNullOrEmpty())
            {
                type = "";
            }
            ViewBag.date = date;
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// 目标进度
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Schedule(PageFactory.ScheduleZbList.DtoReq req)
        {
            var pageModel = new PageFactory.ScheduleZbList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
    }
}