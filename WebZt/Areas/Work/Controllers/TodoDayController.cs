using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Domain;
using WeiCode.Services;



namespace WebProject.Areas.Work.Controllers
{
    public class TodoDayController : BaseLoginController
    {
        /// <summary>
        /// 工作-待办-明细列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Work.TodoDay.List.DtoReq();
            var pageModel = new PageFactory.Work.TodoDay.List().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zt_sn='{new UserIdentityBag().user_sn}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
            return View(pageModel);
        }


        /// <summary>
        /// 新增/编辑工作-待办明细
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.Work.TodoDay.Post.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Work.TodoDay.Post().Get(req);
            return View(pageModel);
        }
    }
}
    