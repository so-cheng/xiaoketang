using Services.Project;
using System.Collections.Generic;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.PUserBasic.Controllers
{
    public class UserRelationController : BaseLoginController
    {
        #region 更新上级用户关系
        /// <summary>
        /// 更新上级用户关系
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateUserRelation(int type_id = 2)//运营邀厅管
        {
            var req = new PageFactory.UpdateSuperiorUserRelation.DtoReq();
            req.type_id = type_id;
            var pageModel = new PageFactory.UpdateSuperiorUserRelation().Get(req);
            return View(pageModel);
        }
        #endregion

    }
}