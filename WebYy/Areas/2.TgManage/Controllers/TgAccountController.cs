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
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas._2.TgManage.Controllers
{
    /// <summary>
    /// 厅管账号
    /// </summary>
    public class TgAccountController : BaseLoginController
    {
        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserList.DtoReq req)
        {
            req.isShowZbInfo = false;
            req.isShowTgInfo = true;
            var pageModel = new PageFactory.UserList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";

            pageModel.buttonGroup.buttonItems.Where(x => x.name == "UnDel").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Post").FirstOrDefault().disabled = false;
            //pageModel.listDisplay.listOperateItems.Where(x => x.name == "InfoPost").FirstOrDefault().disabled = true;

            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                style = "",
                text = "登录",
                name = "login",
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                {
                    url = "fastlogin",
                    field_paras = "user_sn",
                    target = "_bank",
                }
            });
            return View(pageModel);
        }
        /// <summary>
        /// 回收站列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult UnDel(PageFactory.UserUnDel.DtoReq req)
        {
            var pageModel = new PageFactory.UserUnDel().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        public ActionResult FastLogin(string user_sn)
        {
            string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
            var sys_tenant_domain = new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "tger");
            string toUrl = $"http://{sys_tenant_domain.host_domain}/Basic/Auth/AuthorizedLogin?secret={secret}&user_sn={user_sn}";

            Response.Redirect(toUrl);
            ViewBag.secret = secret;
            ViewBag.toUrl = toUrl;
            return View();
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(PageFactory.UserCreate.DtoReq req)
        {
            req.user_type = "tger";
            var pageModel = new PageFactory.UserCreate().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post(PageFactory.UserCreate.DtoReq req)
        {
            req.user_type = "tger";
            var pageModel = new PageFactory.UserCreate().Get(req);
            return View(pageModel);
        }

        [HttpPost]
        public JsonResult Post(ModelDb.user_base req)
        {
            var info = new JsonResultList();
            try
            {
                if (req.name.IsNullOrEmpty()) throw new Exception("昵称不可为空");
                if (req.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                new DomainBasic.UserApp().SetUserInfoByEntity(req);
            }
            catch (Exception e)
            {
                info.code = 1;
                info.msg = e.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 提交厅管背景信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoPost(PageFactory.TgInfoPost.DtoReq req)
        {
            var pageModel = new PageFactory.TgInfoPost().Get(req);
            return View(pageModel);
        }
    }
}