using System;
using System.Collections.Generic;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using DataBase.Project;
using Services.Project;
using Services.Project;
using DataBase.Models;

namespace WebProject.Areas.Config.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenusController : BaseLoginController
    {
        #region 菜单管理
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(int parentId = 0)
        {
            List<ModelDb.sys_modular> sys_modular = DoMySql.FindList<ModelDb.sys_modular>($"1 = 1");
            ViewBag.sys_modular = sys_modular;

            ViewBag.parentId = parentId;
            return View();
        }

        [HttpPost]
        public JsonResult Add(ModelDb.sys_modular_menu menu)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (menu.name.IsNullOrEmpty()) throw new Exception("请输入菜单名称");
                if (menu.url.IsNullOrEmpty()) throw new Exception("请输入链接地址");

                menu.Insert();
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
            List<ModelDb.sys_modular> sys_modular = DoMySql.FindList<ModelDb.sys_modular>($"1 = 1");
            ViewBag.sys_modular = sys_modular;

            ModelDb.sys_modular_menu menu = DoMySql.FindEntity<ModelDb.sys_modular_menu>($"id = {id}");
            ViewBag.menu = menu;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.sys_modular_menu menu)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<string> sql = new List<string>();
                if (menu.name.IsNullOrEmpty()) throw new Exception("请输入菜单名称");
                if (menu.url.IsNullOrEmpty()) throw new Exception("请输入链接地址");

                sql.Add(menu.Update(true));

                if (menu.modular_id > 0)
                {
                    sql.Add($"UPDATE sys_modular_menu_funtion SET modular_id = {menu.modular_id} WHERE menu_id = {menu.id}");
                }

                MysqlHelper.ExecuteSqlTran(sql);
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
                List<ModelDb.sys_modular_menu> list = DoMySql.FindList<ModelDb.sys_modular_menu>($"parent_id = {id}");
                if (list.Count > 0)
                {
                    throw new Exception("存在子菜单不可删除，请先删除子菜单");
                }

                List<string> sql = new List<string>();

                ModelDb.sys_modular_menu menu = DoMySql.FindEntity<ModelDb.sys_modular_menu>($"id = {id}");

                sql.Add($"DELETE FROM sys_modular_menu_funtion WHERE menu_id = '{id}'");
                sql.Add(menu.Delete(true));

                MysqlHelper.ExecuteSqlTran(sql);
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="swapId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SwapSort(int currentId, int swapId)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                ModelDb.sys_modular_menu current = DoMySql.FindEntity<ModelDb.sys_modular_menu>($"id = '{currentId}'");
                ModelDb.sys_modular_menu swap = DoMySql.FindEntity<ModelDb.sys_modular_menu>($"id = '{swapId}'");

                int? sort = current.sort;
                current.sort = swap.sort;
                swap.sort = sort;

                current.Update();
                swap.Update();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        public JsonResult GetMenusRoot(int? roleId, int parentId = 0)
        {
            ViewModel.Menus menus = new ViewModel.Menus();
            try
            {
                List<ViewModel.Data> datas = new List<ViewModel.Data>();
                List<ModelDb.sys_modular_menu> _menus = DoMySql.FindList<ModelDb.sys_modular_menu>($"parent_id = {parentId} ORDER BY sort");
                foreach (var item in _menus)
                {
                    ModelDb.sys_modular sys_modular = DoMySql.FindEntity<ModelDb.sys_modular>($"id = {item.modular_id}");
                    ModelDb.sys_role__menu role_Menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{item.id}' AND role_id = '{roleId}'");
                    datas.Add(new ViewModel.Data
                    {
                        id = item.id,
                        url = item.url,
                        icon = item.icon,
                        modular_name = sys_modular.name,
                        name = item.name,
                        perms = "",
                        checkArr = role_Menu.id > 0 ? "1" : "0",
                        @checked = role_Menu.id > 0 ? "checked" : "",
                        children = GetMenusChild(roleId, item.id),
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

        public List<ViewModel.Childen> GetMenusChild(int? roleId, int parentId = 0)
        {
            List<ViewModel.Childen> childens = new List<ViewModel.Childen>();
            List<ModelDb.sys_modular_menu> _s_menus = DoMySql.FindList<ModelDb.sys_modular_menu>($"parent_id = '{parentId}' ORDER BY sort");
            foreach (var stem in _s_menus)
            {
                ModelDb.sys_role__menu _role_Menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{stem.id}' AND role_id = '{roleId}'");

                childens.Add(new ViewModel.Childen
                {
                    id = stem.id.ToString(),
                    url = stem.url,
                    icon = stem.icon,
                    name = stem.name,
                    perms = "",
                    checkArr = _role_Menu.id > 0 ? "1" : "0",
                    @checked = _role_Menu.id > 0 ? "checked" : "",
                    children = GetMenusChild(roleId, stem.id),
                    parentId = stem.parent_id.ToInt()
                });
            }

            return childens;
        }
        #endregion
    }
}