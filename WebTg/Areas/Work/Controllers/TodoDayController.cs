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

namespace WebProject.Areas.Work.Controllers
{
    public class TodoDayController : BaseLoginController
    {
        /// <summary>
        /// 工作—今日待办记录
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {           
            return View();
        }
        /// <summary>
        /// 待办
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitTodo()
        {
           ViewBag.tg_sn = new UserIdentityBag().user_sn;
            ViewBag.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
            return View();
        }
        /// <summary>
        /// 删除待办
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            var info = new JsonResultAction();
            try
            {
                // 先查询待办明细记录
                var daiban = DoMySql.FindEntity<ModelDb.p_work_todo>($"id = {id}", true);

                // 检查记录是否存在
                if (daiban == null) throw new WeicodeException("待办明细记录不存在");
                // 更新记录
                int count =  daiban.Delete();
                if (count>0)
                {
                    info.msg = "删除成功！";
                }
                else
                {
                    info.code = 1;
                    info.msg = "删除失败！";
                }
                
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }

            return Json(info);
        }

        /// <summary>
        /// 工作—历史待办记录
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryList()
        {        
            return View();
        }

        /// <summary>
        /// 完成工作—今日待办
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Complete(int id)
        {
            var info = new JsonResultAction();
            try
            {
                // 先查询待办明细记录
                var daiban = DoMySql.FindEntity<ModelDb.p_work_todo>($"id = {id}", true);
                DateTime now = DateTime.Now;
                if (now >daiban.s_date_time  && now < daiban.e_date_time )
                {
                    // 检查记录是否存在
                    if (daiban == null) throw new WeicodeException("待办明细记录不存在");

                    // 检查状态
                    if (daiban.status != (sbyte?)ModelDb.p_work_todo.status_enum.未完成)
                    {
                        info.code = 1;
                        info.msg = "当前状态无法提交";
                        return Json(info);
                    }

                    // 修改状态
                    daiban.status = (sbyte?)ModelDb.p_work_todo.status_enum.已完成;
                    daiban.f_date_time = DateTime.Now;
                    // 更新记录
                    daiban.Update();

                    info.msg = "提交完成";
                }else
                {
                    info.code = 1;
                    info.msg = "当前时间不在待办时间内";
                }

               
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }

            return Json(info);
        }


        /// <summary>
        /// 历史未完成工作-待办
        /// </summary>
        /// <returns></returns>
        public ActionResult list_add()
        {
            return View();
        }

        /// <summary>
        /// 新增/编辑今日待办明细
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(ModelDb.p_work_todo p_work_todo_day)
        {
            var info = new JsonResultAction();           
            // 公共字段赋值
            p_work_todo_day.tg_sn = new UserIdentityBag().user_sn;
            p_work_todo_day.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
            var yy_user = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn);
            p_work_todo_day.yy_sn = yy_user.yy_sn;
            if (p_work_todo_day.id > 0)
            {
                p_work_todo_day.Update();                
            }
            else {
              long a =  p_work_todo_day.Insert();

                info.data =  DoMySql.FindEntity<ModelDb.p_work_todo>($"id = {a}", true);
            }
            return Json(info);
        }
    }
}