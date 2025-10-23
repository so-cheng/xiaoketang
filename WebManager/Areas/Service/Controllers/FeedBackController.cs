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

namespace WebProject.Areas.Service.Controllers
{
    public class FeedBackController : BaseLoginController
    {
        /// <summary>
        /// 主播反馈记录
        /// </summary>
        public ActionResult List(PageFactory.FeedBackList.DtoReq req)
        {
            var pageModel = new PageFactory.FeedBackList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().disabled = true;

            //更新对象容器时间
            new DomainBasic.ObjecContainertApp.TenantAndUserTypeData().Set("lastTimeOfFeedback", DateTime.Now.ToString());
            return View(pageModel);
        }



        /// <summary>
        /// 提醒有主播反馈
        /// </summary>
        public JsonResult NotifyMessage()
        {
            var info = new JsonResultList();
            //1.查询容器时间
            DateTime dateTime = new DomainBasic.ObjecContainertApp.TenantAndUserTypeData().Get("lastTimeOfFeedback").ToDateTime();
            if (dateTime.IsNullOrEmpty()) dateTime = DateTime.Now;
            //2.查询主播最新反馈创建的时间
            var count = DomainBasicStatic.DoMySql.FindList<ModelDb.p_service_feedback>($"create_time > '{dateTime}'").Count;
            //3.提醒有新的消息
            if (count > 0)
            {
                info.msg = "有新的主播反馈，请前往查看!";
            }
            else
            {
                info.msg = "";
            }

            //转介绍申请消息提示
            var pushListCount = DoMySql.FindListBySql<ModelDb.p_join_push_apply>($"select id from p_join_push_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = 0").Count();
            if (pushListCount > 0 && new UserIdentityBag().cur_role_id == 34)
            {
                info.msg = "有新的转介绍申请，请前往查看!共:" + pushListCount + "条!";
            }

            return Json(info);
        }
    }
}