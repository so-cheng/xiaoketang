using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Jiezou.Controllers
{
    public class ZhuboJiezouController : BaseLoginController
    {
        /// <summary>
        /// 主播节奏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleChange()
        {
            var req = new PageFactory.Zhubojiezou.DetailZhuboRuleList.DtoReq();
            var pageModel = new PageFactory.Zhubojiezou.DetailZhuboRuleList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 编辑主播节奏规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.Zhubojiezou.DetailZhuboRulePost.DtoReq();
             req.id = id;
            var pageModel = new PageFactory.Zhubojiezou.DetailZhuboRulePost().Get(req);
            return View(pageModel);
        }
    }
}