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

namespace WebProject.Areas._3.TgsManage.Controllers
{
    public class TargetController : BaseLoginController
    {
        /// <summary>
        /// 绩效目标列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.ZbTargetList.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }

        /// <summary>
        /// 提交绩效目标
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.ZbTargetPost.DtoReq req)
        {
            var pageModel = new PageFactory.ZbTargetPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 目标完成进度
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Schedule(PageFactory.ScheduleZbList.DtoReq req)
        {
            var pageModel = new PageFactory.ScheduleZbList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"(tg_user_sn ='{new UserIdentityBag().user_sn}' or tg_user_sn in{new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }

        #region 月度目标
        /// <summary>
        /// 绩效目标列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TgList(PageFactory.TgTargetList.DtoReq req)
        {
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.TgTargetList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"(tg_user_sn ='{new UserIdentityBag().user_sn}' or tg_user_sn in{new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }

        /// <summary>
        /// 提交绩效目标
        /// </summary>
        /// <returns></returns>
        public ActionResult TgPost(PageFactory.TgTargetPostSingle.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetPostSingle().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 修改绩效目标
        /// </summary>
        /// <returns></returns>
        public ActionResult TgEdit(PageFactory.TgTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetEdit().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 日目标
        /// <summary>
        /// 日目标列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DayTarget(string date, string tg_user_sn, string zb_user_sn, string type = "amount")
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (zb_user_sn == "undefined")
            {
                zb_user_sn = "";
            }
            if (tg_user_sn == "undefined")
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            if (type == "undefined")
            {
                type = "";
            }
            ViewBag.date = date;
            ViewBag.zb_user_sn = zb_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.type = type;
            return View();
        }




        #endregion
    }
}