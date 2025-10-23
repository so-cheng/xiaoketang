using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class VacationController : BaseLoginController
    {
        #region 主播请假
        public ActionResult VacationPost(PageFactory.JixiaoDay.ZbVacationPost.DtoReq req)
        {
            var user_info_zb = DomainBasicStatic.DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn = '{new UserIdentityBag().user_sn}'", false);
            req.id = user_info_zb.id;
            var pageModel = new PageFactory.JixiaoDay.ZbVacationPost().Get(req);

            return View(pageModel);
        }

        public ActionResult VacationList(PageFactory.JixiaoDay.VacationList.DtoReq req)
        {
            var pageModel = new PageFactory.JixiaoDay.VacationList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listItems.Where(x => x.field == "zb_user_sn_text").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn ={ new UserIdentityBag().user_sn}";
            return View(pageModel);
        }
        #endregion
    }
}