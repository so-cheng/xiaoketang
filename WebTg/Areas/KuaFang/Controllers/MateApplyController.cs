using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using WeiCode.Utility;

namespace WebProject.Areas.KuaFang.Controllers
{
    public class MateApplyController : BaseLoginController
    {
        /// <summary>
        /// 报名信息（我的）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyList()
        {
            var req = new PageFactory.KuaFangMateApply.MyList.DtoReq();
            var pageModel = new PageFactory.KuaFangMateApply.MyList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 跨房报名
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {id}");
            ViewBag.p_kuafang_mate = p_kuafang_mate;
            return View();
        }

        /// <summary>
        /// 跨房报名
        /// </summary>
        /// <param name="mate_id"></param>
        /// <param name="ting_sn"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Commit(int mate_id, string ting_sn)
        {
            var info = new JsonResultAction();
            // 用于推送微信公众号
            var user_sn = "";
            var url = "";
            bool send = false;
            try
            {
                var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {mate_id}", false);
                if (p_kuafang_mate.IsNullOrEmpty()) throw new Exception("跨房不存在");

                if (ting_sn.IsNullOrEmpty()) throw new Exception("请选择直播厅");

                // 判断自己不可报名
                if (p_kuafang_mate.ting_sn1.Equals(ting_sn)) throw new Exception("不可报名自己的跨房");

                // 判断当前厅是否已填报跨房，不可报名
                var mate_post_exists = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang_mate.kuafang_id} and ting_sn1 = '{ting_sn}'", false);
                if (!mate_post_exists.IsNullOrEmpty()) throw new Exception("直播厅已有发起的跨房，不可参加其他跨房");

                // 判断是否已参加跨房，不可报名
                var mate_exists = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang_mate.kuafang_id} and ting_sn2 = '{ting_sn}'", false);
                if (!mate_exists.IsNullOrEmpty()) throw new Exception("直播厅已参加跨房，不可重复参加");

                // 判断当前跨房活动是否已匹配成功，不可报名
                if (!p_kuafang_mate.ting_sn2.IsNullOrEmpty()) throw new Exception("此跨房已配对，请选择其他跨房");

                // 无需确认直接匹配
                var lSql = new List<string>();

                // 更新跨房活动信息
                p_kuafang_mate.tg_user_sn2 = new UserIdentityBag().user_sn;
                p_kuafang_mate.ting_sn2 = ting_sn;
                lSql.Add(p_kuafang_mate.UpdateTran());

                var p_kuafang_mate_apply = new ModelDb.p_kuafang_mate_apply()
                {
                    tenant_id = p_kuafang_mate.tenant_id,
                    kuafang_id = p_kuafang_mate.kuafang_id,
                    kuafang_mate_id = mate_id,
                    tg_user_sn = new UserIdentityBag().user_sn,
                    ting_sn = ting_sn,
                    status = ModelDb.p_kuafang_mate_apply.status_enum.已确认.ToSByte()
                };

                lSql.Add(p_kuafang_mate_apply.InsertTran());

                DoMySql.ExecuteSqlTran(lSql);

                info.msg = "已报名成功，请查看对手信息";

                // 推送公众号
                user_sn = p_kuafang_mate.tg_user_sn1;
                url = $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger").host_domain}/KuaFang/Mate/UserInfo";
                send = true;
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            finally
            {
                if (send)
                {
                    // 推送公众号
                    new ServiceFactory.Sdk.WeixinSendMsg().Apply(user_sn, "", url, new ServiceFactory.Sdk.WeixinSendMsg.ApplyInfo
                    {
                        theme = "跨房活动",
                        post_time = DateTime.Now,
                        person = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).username
                    });
                }
            }
            return Json(info);
        }
    }
}