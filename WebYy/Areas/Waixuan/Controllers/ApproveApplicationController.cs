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

namespace WebProject.Areas.Waixuan.Controllers
{
    public class ApproveApplicationController : BaseLoginController
    {

        /// <summary>
        /// 待开账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult WaitCreate(int id = 0)
        {
            var req = new PageFactory.Join.ZbList.DtoReq();
            req.tg_need_id = id;
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '' AND tg_user_sn in {new ServiceFactory.YyService().YyGetNextTgForSql(new UserIdentityBag().user_sn)} AND qun_time < '{DateTime.Now.AddDays(-3)}'";
            pageModel.listDisplay.listItems.Where(x => x.field == "qun_time").FirstOrDefault().disabled = false;

            return View(pageModel);
        }
    }
}