using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using DataBase.Project;
using Services.Project;
using DataBase.Models;

namespace WebProject.Areas.Config.Controllers
{
    /// <summary>
    /// 模块
    /// </summary>
    public class ModularController : BaseLoginController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetData()
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<ModelDb.sys_modular> list = DoMySql.FindList<ModelDb.sys_modular>("1 = 1");

                info.code = 0;
                info.msg = "success";
                info.data = list;
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(ModelDb.sys_modular sys_modular)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_modular.name.IsNullOrEmpty()) throw new Exception("请输入模块名称");
                if (sys_modular.tb_prefix.IsNullOrEmpty()) throw new Exception("请输入表前缀");

                sys_modular.Insert();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 编辑模块
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            ModelDb.sys_modular sys_modular = DoMySql.FindEntity<ModelDb.sys_modular>($"id = {id}");
            ViewBag.modular = sys_modular;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.sys_modular sys_modular)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_modular.name.IsNullOrEmpty()) throw new Exception("请输入模块名称");
                if (sys_modular.tb_prefix.IsNullOrEmpty()) throw new Exception("请输入表前缀");

                sys_modular.Update();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                ModelDb.sys_modular sys_modular = DoMySql.FindEntity<ModelDb.sys_modular>($"id = {id}");
                sys_modular.Delete();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }
    }
}