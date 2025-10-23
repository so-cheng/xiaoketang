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

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 规则
    /// </summary>
    public class RuleController : BaseLoginController
    {
        /// <summary>
        /// 对战规则列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.RuleList.DtoReq();
            var pageModel = new PageFactory.TingZhan.RuleList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建对战规则
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.TingZhan.RulePost.DtoReq();
            var pageModel = new PageFactory.TingZhan.RulePost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 长期对战规则列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LongList()
        {
            var req = new PageFactory.TingZhan.RuleLongList.DtoReq();
            var pageModel = new PageFactory.TingZhan.RuleLongList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建长期对战规则
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LongPost(int type = 0, int id = 0)
        {
            var req = new PageFactory.TingZhan.RuleLongPost.DtoReq();
            req.type = type;
            req.id = id;
            var pageModel = new PageFactory.TingZhan.RuleLongPost().Get(req);
            return View(pageModel);
        }
    }
}