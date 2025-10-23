
using System;
using System.Collections.Generic;
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
    /// 菜单功能
    /// </summary>
    public class MenuFuntionController : BaseLoginController
    {
        public ActionResult List(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public JsonResult GetData(int menu_id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<ModelDb.sys_modular_menu_funtion> list = DoMySql.FindList<ModelDb.sys_modular_menu_funtion>($"menu_id = {menu_id}");

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
        /// 添加菜单功能
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int menu_id)
        {
            ViewBag.menu_id = menu_id;
            return View();
        }

        [HttpPost]
        public JsonResult Add(ModelDb.sys_modular_menu_funtion sys_modular_menu_funtion)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_modular_menu_funtion.name.IsNullOrEmpty()) throw new Exception("请输入功能名称");
                if (sys_modular_menu_funtion.introduce.IsNullOrEmpty()) throw new Exception("请输入功能介绍");

                ModelDb.sys_modular_menu sys_modular_menu = DoMySql.FindEntity<ModelDb.sys_modular_menu>($"id = {sys_modular_menu_funtion.menu_id}");
                sys_modular_menu_funtion.modular_id = sys_modular_menu.modular_id;

                sys_modular_menu_funtion.Insert();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 编辑菜单功能
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            ModelDb.sys_modular_menu_funtion sys_modular_menu_funtion = DoMySql.FindEntity<ModelDb.sys_modular_menu_funtion>($"id = {id}");
            ViewBag.menu_funtion = sys_modular_menu_funtion;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.sys_modular_menu_funtion sys_modular_menu_funtion)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_modular_menu_funtion.name.IsNullOrEmpty()) throw new Exception("请输入功能名称");
                if (sys_modular_menu_funtion.introduce.IsNullOrEmpty()) throw new Exception("请输入功能介绍");

                sys_modular_menu_funtion.Update();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 删除菜单功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                ModelDb.sys_modular_menu_funtion sys_modular_menu_funtion = DoMySql.FindEntity<ModelDb.sys_modular_menu_funtion>($"id = {id}");
                sys_modular_menu_funtion.Delete();
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