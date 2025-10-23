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
namespace WebProject.Areas._2.TgManage.Controllers
{
    /// <summary>
    /// 绩效目标
    /// </summary>
    public class TargetController : BaseLoginController
    {
        /// <summary>
        /// 厅完成进度
        /// </summary>
        /// <returns></returns>
        public ActionResult TingFinishSchedule(string date)
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.date = date;

            return View();
        }



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




        #endregion
    }
}