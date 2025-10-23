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


namespace WebProject.Areas.Task.Controllers
{
    public class SaolianController : BaseLoginController
    {
        /// <summary>
        /// 扫脸任务列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Task.FaceList.DtoReq();
            var pageModel = new PageFactory.Task.FaceList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new ServiceFactory.UserInfo.Tg().GetTreeOptionForSql(new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }

        /// <summary>
        /// 各厅扫脸数据明细
        /// </summary>
        /// <returns></returns>
        public ActionResult TingDetail()
        {
            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
            var p_renwu_saolian_tg = DoMySql.FindEntity<ModelDb.p_renwu_saolian_tg>($"c_month = '{DateTime.Today.ToString("yyyy-MM")}' and tg_user_sn = '{new UserIdentityBag().user_sn}'",false);
            if (p_renwu_saolian_tg.IsNullOrEmpty())
            {
                p_renwu_saolian_tg.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                p_renwu_saolian_tg.c_month = DateTime.Today.ToString("yyyy-MM");
                p_renwu_saolian_tg.tg_user_sn = new UserIdentityBag().user_sn;
            }
            p_renwu_saolian_tg.yy_user_sn = tg.yy_sn;
            p_renwu_saolian_tg.zt_user_sn = tg.zt_sn;
            p_renwu_saolian_tg.last_time = DateTime.Now;
            p_renwu_saolian_tg.InsertOrUpdate();
            return View();
        }
    }
}