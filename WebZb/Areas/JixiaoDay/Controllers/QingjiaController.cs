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


namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class QingjiaController : BaseLoginController
    {
        /// <summary>
        /// 请假列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var qingjia = DoMySql.FindList<ModelDb.p_jixiao_qingjia>($"zb_user_sn = '{new UserIdentityBag().user_sn}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' order by id desc");
            ViewBag.qingjia = qingjia;
            return View();
        }

        /// <summary>
        /// 创建请假
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.JixiaoDay.QingJiaPost.DtoReq();
            req.id = id;
            
            var pageModel = new PageFactory.JixiaoDay.QingJiaPost().Get(req);
            pageModel.postedReturn.returnType = ModelBasic.PagePost.PostedReturn.ReturnType.当前窗口跳转URL;
            pageModel.postedReturn.returnUrl = "/JixiaoDay/QingJia/List";
            return View(pageModel);
        }

        /// <summary>
        /// 取消请假单
        /// </summary>
        /// <param name="tingzhan_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Cancel(int id)
        {
            var info = new JsonResultAction();
            try
            {
                // 先查询请假记录
                var qingjia = DoMySql.FindEntity<ModelDb.p_jixiao_qingjia>($"id = {id}", true);

                // 检查记录是否存在
                if (qingjia == null) throw new WeicodeException("请假记录不存在");

                // 检查状态
                if (qingjia.status != (sbyte?)ModelDb.p_jixiao_qingjia.status_enum.等待审批)
                {
                    info.code = 1;
                    info.msg = "当前状态无法取消";
                    return Json(info);
                }

                // 修改状态
                qingjia.status = (sbyte?)ModelDb.p_jixiao_qingjia.status_enum.已取消;

                // 更新记录
                qingjia.Update();

                info.msg = "取消成功";
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