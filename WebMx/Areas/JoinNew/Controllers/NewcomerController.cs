using Newtonsoft.Json.Linq;
using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    /// <summary>
    /// 资料收集（萌新老师通过专属二维码收集候选人资料）
    /// </summary>
    public class NewcomerController : BaseController
    {
        #region 提交表单
        public ActionResult Post(string mx_sn, int id = 0)
        {
            var req = new PageFactory.JoinNew.Stu_MxPost.DtoReq();
            req.id = id;
            req.mx_sn = mx_sn;
            //req.openid = "1";
            req.openid = new PlatformSdk.WeixinMP().GetOpenId();
            if (req.id <= 0 && !req.openid.IsNullOrEmpty())
            {
                var p_join_new_info = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_new_info>($"openid = '{req.openid}'", false);
                req.id = p_join_new_info.id;
            }
            var pageModel = new PageFactory.JoinNew.Stu_MxPost().Get(req);
            return View(pageModel);
        }

        [HttpPost]
        public JsonResult Post(ModelDb.p_join_new_info p_join_new_info)
        {
            var info = new JsonResultAction();
            try
            {
                if (p_join_new_info.id > 0)
                {
                    throw new Exception("已提交，如需修改请联系萌新老师");
                }
                if (!DoMySql.FindEntity<ModelDb.p_join_new_info>($"id = {p_join_new_info.id}", false).ting_sn.IsNullOrEmpty())
                {
                    throw new Exception("已分配直播厅,禁止修改");
                }
                if (p_join_new_info.dou_username.IsNullOrEmpty()) throw new WeicodeException("请填写抖音账号");
                if (p_join_new_info.dou_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写抖音昵称");
                if (p_join_new_info.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请填写微信账号");
                if (p_join_new_info.wechat_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写微信昵称");
                if (p_join_new_info.mx_sn.IsNullOrEmpty()) throw new WeicodeException("缺少萌新老师绑定关系，请联系管理员查看");
                if (!DoMySql.FindEntity<ModelDb.p_join_new_info>($"wechat_username = '{p_join_new_info.wechat_username}' and id != {p_join_new_info.id}", false).IsNullOrEmpty())
                {
                    throw new WeicodeException("微信账号已存在");
                }
                if (p_join_new_info.zb_sex.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写性别");
                }
                if (p_join_new_info.age <= 0 || p_join_new_info.age.ToString().IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写年龄");
                }
                if (p_join_new_info.job.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写现实工作");
                }
                if (p_join_new_info.address.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写所在省市");
                }
                if (p_join_new_info.sessions.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写接档时间");
                }
                if (p_join_new_info.full_or_part.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写兼职/全职");
                }

                if (p_join_new_info.full_or_part == "兼职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 1)
                    {
                        throw new Exception("兼职主播请选择1个接档时间");
                    }
                }
                if (p_join_new_info.full_or_part == "全职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 2)
                    {
                        throw new Exception("全职主播请选择2个接档时间");
                    }
                }

                p_join_new_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_join_new_info.term.IsNullOrEmpty())
                {
                    var p_mengxin = DoMySql.FindEntity<ModelDb.p_mengxin>($"user_sn = '{p_join_new_info.mx_sn}' order by date desc", false);
                    p_join_new_info.term = p_mengxin.term.IsNullOrEmpty() ? "-" : p_mengxin.term;
                }
                if (p_join_new_info.id == 0)
                {
                    p_join_new_info.zb_level = "-";
                }
                if (p_join_new_info.p_join_new_info_sn.IsNullOrEmpty())
                {
                    p_join_new_info.p_join_new_info_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                }
                p_join_new_info.province = p_join_new_info.address.Split(',')[0];
                p_join_new_info.city = p_join_new_info.address.Split(',')[1];
                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待分级.ToSByte();
                List<string> lSql = new List<string>();
                lSql.Add(p_join_new_info.InsertOrUpdateTran($"id = '{p_join_new_info.id}'"));
                DoMySql.ExecuteSqlTran(lSql);

                new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.资料收集, DoMySql.FindEntity<ModelDb.p_join_new_info>($"openid = '{p_join_new_info.openid}'").id, ModelDb.p_join_new_info_log.last_status_enum.无, $"新人'{p_join_new_info.dou_username}'在'{DateTime.Now}'提交了资料", true);
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }
        #endregion

        #region 编辑表单
        public ActionResult Edit(int id, string mx_sn)
        {
            var req = new PageFactory.JoinNew.Stu_MxPost.DtoReq();
            req.id = id;
            req.mx_sn = mx_sn;
            var pageModel = new PageFactory.JoinNew.Stu_MxPost().Get(req);
            pageModel.formDisplay.formItems.Find(x => x.name == "term").displayStatus = WeiCode.Models.EmtModelBase.DisplayStatus.编辑;
            return View(pageModel);
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.p_join_new_info p_join_new_info)
        {
            var info = new JsonResultAction();
            List<string> lSql = new List<string>();

            try
            {
                if (!DoMySql.FindEntity<ModelDb.p_join_new_info>($"id = {p_join_new_info.id}", false).ting_sn.IsNullOrEmpty())
                {
                    throw new Exception("已分配直播厅,禁止修改");
                }
                if (p_join_new_info.dou_username.IsNullOrEmpty()) throw new WeicodeException("请填写抖音账号");
                if (p_join_new_info.dou_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写抖音昵称");
                if (p_join_new_info.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请填写微信账号");
                if (p_join_new_info.wechat_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写微信昵称");
                if (p_join_new_info.mx_sn.IsNullOrEmpty()) throw new WeicodeException("缺少萌新老师绑定关系，请联系管理员查看");
                if (!DoMySql.FindEntity<ModelDb.p_join_new_info>($"wechat_username = '{p_join_new_info.wechat_username}' and id != {p_join_new_info.id}", false).IsNullOrEmpty())
                {
                    throw new WeicodeException("微信账号已存在");
                }
                if (p_join_new_info.zb_sex.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写性别");
                }
                if (p_join_new_info.age <= 0 || p_join_new_info.age.ToString().IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写年龄");
                }
                if (p_join_new_info.job.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写现实工作");
                }
                if (p_join_new_info.address.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写所在省市");
                }
                if (p_join_new_info.sessions.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写接档时间");
                }
                if (p_join_new_info.full_or_part.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写兼职/全职");
                }
                if (p_join_new_info.full_or_part == "兼职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 1)
                    {
                        throw new Exception("兼职主播请选择1个接档时间");
                    }
                }
                if (p_join_new_info.full_or_part == "全职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 2)
                    {
                        throw new Exception("全职主播请选择2个接档时间");
                    }
                }

                p_join_new_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_join_new_info.term.IsNullOrEmpty())
                {
                    var p_mengxin = DoMySql.FindEntity<ModelDb.p_mengxin>($"user_sn = '{new UserIdentityBag().user_sn}' order by date desc", false);
                    p_join_new_info.term = p_mengxin.term.IsNullOrEmpty() ? "-" : p_mengxin.term;
                }
                if (p_join_new_info.zb_level.IsNullOrEmpty())
                {
                    p_join_new_info.zb_level = "-";
                }
                p_join_new_info.province = p_join_new_info.address.Split(',')[0];
                p_join_new_info.city = p_join_new_info.address.Split(',')[1];
                lSql.Add(p_join_new_info.UpdateTran());
                DoMySql.ExecuteSqlTran(lSql);
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }
        #endregion
    }
}