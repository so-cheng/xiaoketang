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
    /// 主播账号
    /// </summary>
    public class ZbAccountController : BaseLoginController
    {
        /// <summary>
        /// 主播列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserList.DtoReq req)
        {
            var pageModel = new PageFactory.UserList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in(select t_user_sn from user_relation where relation_type_id='1' and f_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)})";
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "keyword").FirstOrDefault().disabled = true;


            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Post").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Post").FirstOrDefault().eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
            {
                url = "/UserInfo/ZhuboAccount/Create"
            };

            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().disabled = true;

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
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in(select t_user_sn from user_relation where relation_type_id='1' and f_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }
        /// <summary>
        /// 提交主播背景信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoPost(PageFactory.ZbInfoPost.DtoReq req)
        {
            var pageModel = new PageFactory.ZbInfoPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(PageFactory.UserCreate.DtoReq req)
        {
            req.user_type = "zber";
            var pageModel = new PageFactory.UserCreate().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "f_user_sn").FirstOrDefault().isDisplay = true;
            pageModel.formDisplay.formItems.Where(x => x.name == "attach1").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.编辑;
            pageModel.formDisplay.formItems.Where(x => x.name == "attach2").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.编辑;
            return View(pageModel);
        }

        /// <summary>
        /// 编辑账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post(PageFactory.UserCreate.DtoReq req)
        {
            var pageModel = new PageFactory.UserCreate().Get(req);
            pageModel.formDisplay.formItems.Where(x => x.name == "f_user_sn").FirstOrDefault().isDisplay = false;
            pageModel.formDisplay.formItems.Where(x => x.name == "attach1").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.编辑;
            pageModel.formDisplay.formItems.Where(x => x.name == "attach2").FirstOrDefault().displayStatus = EmtModelBase.DisplayStatus.编辑;
            return View(pageModel);
        }


        public ActionResult FastLogin(string user_sn)
        {
            string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
            var sys_tenant_domain = new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "zber");
            string toUrl = $"http://{sys_tenant_domain.host_domain}/Basic/Auth/AuthorizedLogin?secret={secret}&user_sn={user_sn}";

            Response.Redirect(toUrl);
            ViewBag.secret = secret;
            ViewBag.toUrl = toUrl;
            return View();
        }
    }
}