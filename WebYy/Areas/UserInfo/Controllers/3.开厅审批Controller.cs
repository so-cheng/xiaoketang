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

namespace WebProject.Areas.UserInfo.Controllers
{
    /// <summary>
    /// 开厅申请
    /// </summary>
    public class TingApplyController : BaseLoginController
    {

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.UserInfo.TingApplyPost.DtoReq();
            var pageModel = new PageFactory.UserInfo.TingApplyPost().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 申请记录：等待超管审批=1 | 全部记录=2
        /// </summary>
        public ActionResult List(int type = 1)
        {
            var req = new PageFactory.UserInfo.TingApplyList.DtoReq();
            var pageModel = new PageFactory.UserInfo.TingApplyList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $" yy_user_sn = '{new UserIdentityBag().user_sn}' and status = {ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()}";
            return View(pageModel);
        }

        

    }
}