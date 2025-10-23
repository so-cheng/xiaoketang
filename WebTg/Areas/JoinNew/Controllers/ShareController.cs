using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ShareController : BaseLoginController
    {
        #region 分配日志
        /// <summary>
        /// 分配日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Log(int id = 0)
        {
            var req = new PageFactory.JoinNew.ShareLog.DtoReq();
            var pageModel = new PageFactory.JoinNew.ShareLog().Get(req);
            if (id > 0)
            {
                pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id = {id}";
            }
            else
            {
                // 当前萌新下的日志
                pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id in (select id from p_join_new_info where mx_sn = '{new UserIdentityBag().user_sn}')";
            }
            return View(pageModel);
        }
        #endregion
    }
}