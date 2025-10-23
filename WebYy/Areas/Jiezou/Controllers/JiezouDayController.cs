using Services.Project;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;

namespace WebProject.Areas.Jiezou.Controllers
{
    /// <summary>
    /// 节奏跟进
    /// </summary>
    public class JiezouDayController : BaseLoginController
    {
        /// <summary>
        /// 节奏展示
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="yy_user_sn"></param>
        /// <returns></returns>
        public ActionResult Item(string c_date, string yy_user_sn)
        {
            //仅在yy_user_sn为空时设置默认值，否则使用传入的参数
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = new ServiceFactory.UserInfo.Yy().GetAllYyForKv().First().Value;
            }
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.c_date = c_date;
            return View();
        }

        /// <summary>
        /// 阶段规则说明
        /// </summary>
        /// <returns></returns>
        public ActionResult RuleDefinition()
        {
            var ruleList = DoMySql.FindList<ModelDb.jiezou_detail_rule>($"type = '{ModelDb.jiezou_detail_rule.type_enum.日节奏统计规则.ToSByte()}'");
            ViewBag.ruleList = ruleList;
            return View();
        }
        
        /// <summary>
        /// 历史节奏
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult Detail(string c_date, string ting_sn)
        {
            ViewBag.c_date = c_date;
            ViewBag.ting_sn = ting_sn;
            return View();
        }

        /// <summary>
        /// 阶段规则信息
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult RuleChange(PageFactory.Jiezou.DetailRuleList.DtoReq req)
        {
            var pageModel = new PageFactory.Jiezou.DetailRuleList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑阶段规则
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Jiezou.DetailRulePost.DtoReq req)
        {
            var pageModel = new PageFactory.Jiezou.DetailRulePost().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 活动日列表
        /// </summary>
        /// <returns></returns>
        public ActionResult HuodonList()
        {
            var req = new PageFactory.Jiezou.HuodonList.DtoReq();
            var pageModel = new PageFactory.Jiezou.HuodonList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建/编辑活动日
        /// </summary>
        /// <returns></returns>      
        public ActionResult HuodonPost(int id = 0)
        {
            var req = new PageFactory.Jiezou.HuodonPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Jiezou.HuodonPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 历史节奏
        /// </summary>
        /// <param name="c_date"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        public ActionResult TingStandardChart(string c_date)
        {
            ViewBag.c_date = c_date;
            return View();
        }
    }
}