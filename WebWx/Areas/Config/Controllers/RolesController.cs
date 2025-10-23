using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using DataBase.Project;
using Services.Project;
using DataBase.Models;

namespace WebProject.Areas.Config.Controllers
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class RolesController : BaseLoginController
    {
        //static readonly ConcurrentDictionary<int, int> keys = new ConcurrentDictionary<int, int>();

        #region 角色权限
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Post(int? id)
        {
            ModelDb.sys_role role = DoMySql.FindEntity<ModelDb.sys_role>($"id = '{id}'");
            ViewBag.role = role;
            return View();
        }

        [HttpPost]
        public JsonResult Post(ModelDb.sys_role role)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (role.name.IsNullOrEmpty()) throw new Exception("请输入角色名称");
                if (role.memo.IsNullOrEmpty()) throw new Exception("请输入备注信息");

                if (role.id > 0)
                {
                    ModelDb.sys_role entity = DoMySql.FindEntity<ModelDb.sys_role>($"name = '{role.name}' AND id <> '{role.id}'");
                    if (entity.id > 0) throw new Exception("当前角色已存在");
                }
                else
                {
                    ModelDb.sys_role entity = DoMySql.FindEntity<ModelDb.sys_role>($"name = '{role.name}'");
                    if (entity.id > 0) throw new Exception("当前角色已存在");
                }

                role.InsertOrUpdate();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<string> sql = new List<string>();

                ModelDb.sys_role role = DoMySql.FindEntity<ModelDb.sys_role>($"id = '{id}'");
                if (role.id == 0) throw new Exception("该角色不存在或已被删除");

                sql.Add($"DELETE FROM sys_role__menu WHERE role_id = '{id}'");
                sql.Add(role.Delete(true));

                MysqlHelper.ExecuteSqlTran(sql);
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
                //if (roleId != null && roleId > 0)
                //{
                //    if (!keys.TryAdd((int)roleId, Thread.CurrentThread.ManagedThreadId))
                //    {
                //        if (keys[(int)roleId] != Thread.CurrentThread.ManagedThreadId)
                //            throw new Exception("锁定编辑，过会尝试");
                //    }
                //}
                //else
                //    keys.Clear();

                List<ViewModel.Data> datas = new List<ViewModel.Data>();
                List<ModelDb.sys_modular_menu> _menus = DoMySql.FindList<ModelDb.sys_modular_menu>($"parent_id = {parentId} ORDER BY sort");
                foreach (var item in _menus)
                {
                    ModelDb.sys_role__menu role_Menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{item.id}' AND role_id = '{roleId}'");
                    datas.Add(new ViewModel.Data
                    {
                        id = item.id,
                        url = item.url,
                        icon = item.icon,
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
            if (_s_menus.Any())
            {
                foreach (var stem in _s_menus)
                {
                    ModelDb.sys_role__menu _role_Menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{stem.id}' AND role_id = '{roleId}'");

                    childens.Add(new ViewModel.Childen
                    {
                        id = stem.id.ToString(),
                        url = stem.url,
                        icon = stem.icon,
                        type = "1",
                        name = stem.name,
                        perms = "",
                        checkArr = _role_Menu.id > 0 ? "1" : "0",
                        @checked = _role_Menu.id > 0 ? "checked" : "",
                        children = GetMenusChild(roleId, stem.id),
                        parentId = stem.parent_id.ToInt()
                    }); ;
                }
            }
            else
            {
                List<ModelDb.sys_modular_menu_funtion> _s_menu_funtions = DoMySql.FindList<ModelDb.sys_modular_menu_funtion>($"menu_id = '{parentId}'");
                foreach (var stem in _s_menu_funtions)
                {
                    ModelDb.sys_role__menu _role_Menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{stem.menu_id}' AND concat(',',function_ids,',') LIKE '%,{stem.id},%' AND role_id = '{roleId}'");

                    childens.Add(new ViewModel.Childen
                    {
                        id = "func-" + stem.id,
                        iconClass = "dtree-icon-caidan_xunzhang",
                        type = "2",
                        name = stem.name,
                        perms = "",
                        checkArr = _role_Menu.id > 0 ? "1" : "0",
                        @checked = _role_Menu.id > 0 ? "checked" : "",
                        children = null,
                        parentId = stem.menu_id.ToInt()
                    });
                }
            }

            return childens;
        }

        public JsonResult GetRolesRoot()
        {
            ViewModel.Roles roles = new ViewModel.Roles();
            try
            {
                List<ViewModel.sys_role_data> data = new List<ViewModel.sys_role_data>();
                List<ModelDb.sys_role> list = DoMySql.FindList<ModelDb.sys_role>("1 = 1");
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        ViewModel.sys_role_data row = new ViewModel.sys_role_data();
                        row.id = item.id;
                        row.name = item.name;
                        row.memo = item.memo;
                        row.data_view = item.data_view;
                        row.data_view_name = ((ModelDb.sys_role.data_view_enum)item.data_view).ToString();
                        row.create_time = item.create_time;
                        row.modify_time = item.modify_time;

                        data.Add(row);
                    }
                }

                roles.code = 0;
                roles.msg = "success";
                roles.data = data;
            }
            catch (Exception ex)
            {
                roles.code = 1;
                roles.msg = ex.Message;
            }
            return Json(roles, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ChangeAuthor(string[] arrMenuId, string[] arrMenuFuncId, string roleId)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                List<string> SQLStringList = new List<string>();
                if (roleId.IsNullOrEmpty()) throw new Exception("请先选择角色");
                List<ModelDb.sys_role__menu> role_menus = DoMySql.FindList<ModelDb.sys_role__menu>($"role_id = '{roleId}'");
                string[] arrRoleMenuId = new string[role_menus.Count];
                for (int i = 0; i < role_menus.Count; i++)
                {
                    arrRoleMenuId[i] = role_menus[i].menu_id.ToString();
                }
                // 删除
                if (null != arrMenuId)
                {
                    var del = arrRoleMenuId.Except(arrMenuId);
                    foreach (var i in del)
                    {
                        ModelDb.sys_role__menu _role_menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{i.ToInt()}' AND role_id = '{roleId}'");
                        SQLStringList.Add(_role_menu.Delete(true));
                    }
                    // 新增
                    var add = arrMenuId.Except(arrRoleMenuId);
                    foreach (var i in add)
                    {
                        SQLStringList.Add(new ModelDb.sys_role__menu
                        {
                            menu_id = i.ToInt(),
                            role_id = roleId.ToInt()
                        }.Insert(true));
                    }
                }
                else
                {
                    throw new Exception("请至少保留一个页面");
                }

                MysqlHelper.ExecuteSqlTran(SQLStringList);

                // 菜单功能
                MysqlHelper.ExecuteSql($"UPDATE sys_role__menu SET function_ids = NULL WHERE role_id = {roleId}");
                if (null != arrMenuFuncId)
                {
                    foreach (var func in arrMenuFuncId)
                    {
                        ModelDb.sys_modular_menu_funtion sys_modular_menu_funtion = DoMySql.FindEntity<ModelDb.sys_modular_menu_funtion>($"id = '{func.ToInt()}'");
                        if (null != sys_modular_menu_funtion)
                        {
                            ModelDb.sys_role__menu _role_menu = DoMySql.FindEntity<ModelDb.sys_role__menu>($"menu_id = '{sys_modular_menu_funtion.menu_id}' AND role_id = '{roleId}'");
                            if (_role_menu.id > 0)
                            {
                                if (_role_menu.function_ids.IsNullOrEmpty())
                                {
                                    _role_menu.function_ids = func;
                                }
                                else
                                {
                                    _role_menu.function_ids += "," + func;
                                }

                                _role_menu.Update();
                            }
                        }
                    }
                }
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