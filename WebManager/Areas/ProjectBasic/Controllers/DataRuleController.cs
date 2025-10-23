using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.Services;
using Newtonsoft.Json;
using static WeiCode.ModelDbs.ModelDb;

namespace WebProject.Areas.ProjectBasic.Controllers
{
    public class DataRuleController : BaseLoginController
    {
        //厅核心数据规则 
        public ActionResult List(PageFactory.ProjectBasic.DataRule.DtoReq req)
        {
            var pageModel = new PageFactory.ProjectBasic.DataRule.List().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int type_id, string field_key)
        {
            var req = new PageFactory.ProjectBasic.DataRule.DtoReq();
            req.type_id = type_id;
            req.field_key = field_key;

            ViewBag.type_id = type_id.ToString();
            ViewBag.field_key = field_key.ToString();

            return View();
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public ActionResult AddOrEditRule(projectbasic_data_rule rule)
        {
            var info = new JsonResultAction();
            ServiceFactory.ProjectBasic.DataRule dataRule = new ServiceFactory.ProjectBasic.DataRule();
            dataRule.AddOrEditRule(rule);
            return Json(info);
        }
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteRule(int id)
        {
            var info = new JsonResultAction();
            ServiceFactory.ProjectBasic.DataRule dataRule = new ServiceFactory.ProjectBasic.DataRule();
            dataRule.DeleteRule(id);
            return Json(info);
        }
    }
}