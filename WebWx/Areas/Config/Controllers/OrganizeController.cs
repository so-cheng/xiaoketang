
using System;
using System.Collections.Generic;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using DataBase.Models;

using DataBase.Project;
using Services.Project;


namespace WebProject.Areas.Config.Controllers
{
    /// <summary>
    /// 组织部门
    /// </summary>
    public class OrganizeController : BaseLoginController
    {
        #region 部门管理
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 添加子部门
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int parentId = 0)
        {
            ViewBag.parentId = parentId;
            return View();
        }

        [HttpPost]
        public JsonResult Add(ModelDb.sys_organize sys_organize)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_organize.name.IsNullOrEmpty()) throw new Exception("请输入部门名称");

                sys_organize.Insert();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            ModelDb.sys_organize sys_organize = DoMySql.FindEntity<ModelDb.sys_organize>($"id = {id}");
            ViewBag.sys_organize = sys_organize;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.sys_organize sys_organize)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_organize.name.IsNullOrEmpty()) throw new Exception("请输入部门名称");

                sys_organize.Update();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<ModelDb.sys_organize> list = DoMySql.FindList<ModelDb.sys_organize>($"parent_id = {id} ORDER BY id");
                if (list.Count > 0)
                {
                    throw new Exception("存在子部门不可删除，请先删除子部门");
                }

                ModelDb.sys_organize sys_organize = DoMySql.FindEntity<ModelDb.sys_organize>($"id = {id}");
                sys_organize.Delete();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        public JsonResult GetRoot(int parentId = 0)
        {
            ViewModel.Menus menus = new ViewModel.Menus();
            try
            {
                List<ViewModel.Data> datas = new List<ViewModel.Data>();
                List<ModelDb.sys_organize> _sys_organizes = DoMySql.FindList<ModelDb.sys_organize>($"parent_id = {parentId} ORDER BY id");
                foreach (var item in _sys_organizes)
                {
                    ModelDb.sys_organize i_sys_organize = DoMySql.FindEntity<ModelDb.sys_organize>($"parent_id = '{item.id}'");
                    datas.Add(new ViewModel.Data
                    {
                        id = item.id,
                        name = item.name,
                        perms = "",
                        checkArr = i_sys_organize.id > 0 ? "1" : "0",
                        @checked = i_sys_organize.id > 0 ? "checked" : "",
                        children = GetChildren(item.id),
                        parentId = item.parent_id.ToInt(),
                    });
                }

                menus.code = 0;
                menus.msg = "success";
                menus.data = datas;
            }
            catch (Exception ex)
            {
                menus.code = 1;
                menus.msg = ex.Message;
            }
            return Json(menus, JsonRequestBehavior.AllowGet);
        }

        public List<ViewModel.Childen> GetChildren(int parentId = 0)
        {
            List<ViewModel.Childen> childens = new List<ViewModel.Childen>();
            List<ModelDb.sys_organize> _sys_organizes = DoMySql.FindList<ModelDb.sys_organize>($"parent_id = '{parentId}' ORDER BY id");
            foreach (var stem in _sys_organizes)
            {
                ModelDb.sys_organize i_sys_organize = DoMySql.FindEntity<ModelDb.sys_organize>($"parent_id = '{stem.id}'");

                childens.Add(new ViewModel.Childen
                {
                    id = stem.id.ToString(),
                    name = stem.name,
                    perms = "",
                    checkArr = i_sys_organize.id > 0 ? "1" : "0",
                    @checked = i_sys_organize.id > 0 ? "checked" : "",
                    children = GetChildren(stem.id),
                    parentId = stem.parent_id.ToInt()
                });
            }

            return childens;
        }
        #endregion
    }
}