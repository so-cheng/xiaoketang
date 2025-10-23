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
    /// 厅管目标
    /// </summary>
    public class TgTargetController : BaseLoginController
    {
        /// <summary>
        /// 月度目标
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.TgTargetPostSingle.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetPostSingle().Get(req);
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

        /// <summary>
        /// 修改目标（列表）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.TgTargetList.DtoReq req)
        {
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.TgTargetList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new ServiceFactory.UserInfo.Tg().GetTreeOptionForSql(new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }

        /// <summary>
        /// 修改目标（编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult TgEdit(PageFactory.TgTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetEdit().Get(req);
            return View(pageModel);
        }
    }
}