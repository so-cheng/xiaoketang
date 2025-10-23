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
using WeiCode.Utility;

namespace WebProject.Areas.QianYue.Controllers
{
    /// <summary>
    /// 提成规则（档位）
    /// </summary>
    public class CommissionRuleController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.QianYue.CommissionRuleList.DtoReq();
            var pageModel = new PageFactory.QianYue.CommissionRuleList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新增/编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.QianYue.CommissionRulePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.QianYue.CommissionRulePost().Get(req);
            return View(pageModel);
        }
    }
}