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
using WeiCode.Modular;

namespace WebProject.Areas.PCrm.Controllers
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class InfoController : BaseLoginController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var customer_list = DoMySql.FindListBySql<CustomerDto>($"select * from p_crm_customer where zb_user_sn = '{new UserIdentityBag().user_sn}' and status = {ModelDb.p_crm_customer.status_enum.正常.ToSByte()}");
            // 将首字母和头文字添加到用户List
            foreach (var customer in customer_list)
            {
                var initial = customer.nick.Substring(0, 1); // 取第一个字
                var initial_pinyin = char.Parse(UtilityStatic.PinYin.GetFirstLetter(initial.ToUpper())); // 首字母
                if (!char.IsLetter(initial_pinyin)) initial_pinyin = '#';

                customer.initial = initial;
                customer.initial_pinyin = initial_pinyin;
            }

            var list = new List<PinyinDto>();
            // 按首字母分组排序
            var list_group = customer_list.OrderBy(p => p.initial_pinyin).GroupBy(p => p.initial_pinyin);
            foreach (var item in list_group)
            {
                list.Add(new PinyinDto()
                {
                    initial_pinyin = item.Key,
                    customers = item.ToList()
                });
            }
            ViewBag.list = list;

            return View();
        }

        /// <summary>
        /// 流失回收站
        /// </summary>
        /// <returns></returns>
        public ActionResult LossList()
        {
            var customer_list = DoMySql.FindListBySql<CustomerDto>($"select * from p_crm_customer where zb_user_sn = '{new UserIdentityBag().user_sn}' and status = {ModelDb.p_crm_customer.status_enum.逻辑删除.ToSByte()}");
            // 将头文字添加到用户List
            foreach (var customer in customer_list)
            {
                customer.initial = customer.nick.Substring(0, 1); // 取第一个字
            }

            // 总用户数
            var count = DoMySql.FindList<ModelDb.p_crm_customer>($"zb_user_sn = '{new UserIdentityBag().user_sn}'").Count;

            ViewBag.list = customer_list;
            ViewBag.rate = count > 0 ? Math.Round(customer_list.Count.ToDecimal() / count.ToDecimal() * 100, 2) : 0; // 流失率

            return View();
        }

        public class PinyinDto
        {
            /// <summary>
            /// 首字母
            /// </summary>
            public char initial_pinyin { get; set; }
            /// <summary>
            /// 用户
            /// </summary>
            public List<CustomerDto> customers { get; set; }
        }

        public class CustomerDto : ModelDb.p_crm_customer
        {
            /// <summary>
            /// 首字母
            /// </summary>
            public char initial_pinyin { get; set; }
            /// <summary>
            /// 头文字
            /// </summary>
            public string initial { get; set; }
        }

        /// <summary>
        /// 用户名片
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            var customer = DoMySql.FindEntity<ModelDb.p_crm_customer>($"id = {id}", false);
            ViewBag.customer = customer;

            return View();
        }

        /// <summary>
        /// 流失用户名片
        /// </summary>
        /// <returns></returns>
        public ActionResult LossDetail(int id)
        {
            var customer = DoMySql.FindEntity<ModelDb.p_crm_customer>($"id = {id}", false);
            ViewBag.customer = customer;

            return View();
        }

        /// <summary>
        /// 用户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            var customer = DoMySql.FindEntity<ModelDb.p_crm_customer>($"id = {id}", false);
            ViewBag.customer = customer;

            return View();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            var info = new JsonResultAction();
            try
            {
                var p_crm_customer = new ModelDb.p_crm_customer()
                {
                    status = ModelDb.p_crm_customer.status_enum.逻辑删除.ToSByte()
                };
                p_crm_customer.Update($"id = {id}");
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 恢复用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Restore(int id)
        {
            var info = new JsonResultAction();
            try
            {
                var p_crm_customer = new ModelDb.p_crm_customer()
                {
                    status = ModelDb.p_crm_customer.status_enum.正常.ToSByte()
                };
                p_crm_customer.Update($"id = {id}");
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