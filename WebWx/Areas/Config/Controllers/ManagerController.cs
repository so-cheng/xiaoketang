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
using Services.Project;

namespace WebProject.Areas.Config.Controllers
{
    /// <summary>
    /// 帐户
    /// </summary>
    public class ManagerController : BaseLoginController
    {
        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetData()
        {
            ViewModel.Manager info = new ViewModel.Manager();
            try
            {
                List<ViewModel.sys_manager_data> datas = new List<ViewModel.sys_manager_data>();
                List<ModelDb.sys_manager> list = DoMySql.FindList<ModelDb.sys_manager>("1 = 1");
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        ViewModel.sys_manager_data sys_manager_data = new ViewModel.sys_manager_data();
                        string organize_name = "";
                        string role_name = "";
                        ModelDb.sys_organize sys_organize = DoMySql.FindEntity<ModelDb.sys_organize>($"id = {item.organize_id}");
                        organize_name = sys_organize.name;
                        ModelDb.sys_role sys_role = DoMySql.FindEntity<ModelDb.sys_role>($"id = {item.role_id}");
                        role_name = sys_role.name;
                        datas.Add(new ViewModel.sys_manager_data
                        {
                            id = item.id,
                            username = item.username,
                            name = item.name,
                            organize_name = organize_name,
                            role_name = role_name,
                            status_name = ((ModelDb.sys_manager.status_enum)item.status).ToString(),
                        });
                    }
                }

                info.code = 0;
                info.msg = "success";
                info.data = datas;
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加帐户
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            List<ModelDb.sys_organize> sys_organize = DoMySql.FindList<ModelDb.sys_organize>("1 = 1");
            List<ModelDb.sys_role> sys_role = DoMySql.FindList<ModelDb.sys_role>("1 = 1");

            ViewBag.sys_organize = sys_organize;
            ViewBag.sys_role = sys_role;
            return View();
        }

        [HttpPost]
        public JsonResult Add(ModelDb.sys_manager sys_manager)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_manager.name.IsNullOrEmpty()) throw new Exception("请输入用户名");
                if (sys_manager.password.IsNullOrEmpty()) throw new Exception("请输入密码");
                if (sys_manager.username.IsNullOrEmpty()) throw new Exception("请输入账号");
                if (sys_manager.organize_id.IsNullOrEmpty()) throw new Exception("请选择所属部门");
                if (sys_manager.role_id.IsNullOrEmpty()) throw new Exception("请选择角色");

                ModelDb.sys_manager entity = DoMySql.FindEntity<ModelDb.sys_manager>($"username = '{sys_manager.username}'");
                if (entity.id > 0) throw new Exception("当前账号已存在");

                sys_manager.password = Md5.getMd5(sys_manager.password);

                sys_manager.Insert();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 编辑帐户
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            ModelDb.sys_manager sys_manager = DoMySql.FindEntity<ModelDb.sys_manager>($"id = {id}");
            ViewBag.sys_manager = sys_manager;

            List<ModelDb.sys_organize> sys_organize = DoMySql.FindList<ModelDb.sys_organize>("1 = 1");
            List<ModelDb.sys_role> sys_role = DoMySql.FindList<ModelDb.sys_role>("1 = 1");

            ViewBag.sys_organize = sys_organize;
            ViewBag.sys_role = sys_role;
            return View();
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.sys_manager sys_manager)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                if (sys_manager.name.IsNullOrEmpty()) throw new Exception("请输入用户名");
                if (sys_manager.username.IsNullOrEmpty()) throw new Exception("请输入账号");
                if (sys_manager.organize_id.IsNullOrEmpty()) throw new Exception("请选择所属部门");
                if (sys_manager.role_id.IsNullOrEmpty()) throw new Exception("请选择角色");

                ModelDb.sys_manager entity = DoMySql.FindEntity<ModelDb.sys_manager>($"username = '{sys_manager.username}' AND id <> {sys_manager.id}");
                if (entity.id > 0) throw new Exception("当前账号已存在");

                if (!sys_manager.password.IsNullOrEmpty())
                {
                    sys_manager.password = Md5.getMd5(sys_manager.password);
                }

                sys_manager.Update();
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 删除帐户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id)
        {
            JsonResultInfo info = new JsonResultInfo();
            try
            {
                var sys_manager = DoMySql.FindEntity<ModelDb.sys_manager>($"id = {id}");
                sys_manager.Delete();
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