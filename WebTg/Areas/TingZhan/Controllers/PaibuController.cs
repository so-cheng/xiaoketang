using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using static Services.Project.ServiceFactory;
using WeiCode.Utility;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 排布
    /// </summary>
    public class PaibuController : BaseLoginController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var p_tingzhan = new TingZhanService().getNewTingzhan();
            var p_tingzhan_mate1 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"tingzhan_id = {p_tingzhan.id} and tg_user_sn1 = '{new UserIdentityBag().user_sn}'", false);
            var p_tingzhan_mate2 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"tingzhan_id = {p_tingzhan.id} and tg_user_sn2 = '{new UserIdentityBag().user_sn}'", false);

            // 获取当前时间
            DateTime now = DateTime.Now;
            // 设置目标时间（20:30）
            DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 20, 30, 0);

            if (p_tingzhan_mate1.IsNullOrEmpty() && p_tingzhan_mate2.IsNullOrEmpty())
            {
                var req = new PageFactory.TingZhan.TargetPost.DtoReq();
                var pageModel = new PageFactory.TingZhan.TargetPost().Get(req);
                pageModel.formDisplay.formItems.Find(item => item.name == "yy_user_sn").isDisplay = false;
                pageModel.formDisplay.formItems.Find(item => item.name == "tg_user_sn").isDisplay = false;
                return View(pageModel);
            }
            else if (now < targetTime)
            {
                var req = new PageFactory.TingZhan.CfContentPost.DtoReq();
                var pageModel = new PageFactory.TingZhan.CfContentPost().Get(req);
                return View(pageModel);
            }
            else
            {
                var req = new PageFactory.TingZhan.ScorePost.DtoReq();
                var pageModel = new PageFactory.TingZhan.ScorePost().Get(req);
                return View(pageModel);
            }
        }
    }
}