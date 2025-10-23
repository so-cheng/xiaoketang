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
using static WeiCode.Models.ModelBasic;

namespace WebProject.Areas._2.ZbManage.Controllers
{
    /// <summary>
    /// 主播账号
    /// </summary>
    public class AccountController : BaseLoginController
    {
        /// <summary>
        /// 主播列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.UserList.DtoReq req)
        {
            var pageModel = new PageFactory.UserList().Get(req);
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "InfoPost").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "ExcelPost").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "UnDel").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "Post").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
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
            pageModel.formDisplay.formItems.Where(x => x.name == "f_user_sn").FirstOrDefault().isDisplay = false;
            pageModel.formDisplay.formItems.Where(x => x.name == "attach1").FirstOrDefault().displayStatus =  EmtModelBase.DisplayStatus.编辑;
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
            pageModel.formDisplay.formItems.Add(new ModelBasic.EmtHtml("c_html")
            {
                Content = "A类主播：具备1麦带档能力<br>B类主播：具备2麦配合能力<br>C类主播：具备3麦互动能力"
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
            pageModel.listDisplay.listData.attachFilterSql = $"user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
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

        /// <summary>
        /// 主播背景信息列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult InfoList(PageFactory.ZbInfoList.DtoReq req)
        {
            var pageModel = new PageFactory.ZbInfoList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)}";
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
        /// 提交主播背景信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExcelPost(PageFactory.ZbExcelPost.DtoReq req)
        {
            var pageModel = new PageFactory.ZbExcelPost().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 待开账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Wait(PageFactory.WaitUserList.DtoReq req)
        {
            var pageModel = new PageFactory.WaitUserList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 待开账号背调基础信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackTuning(PageFactory.BdPost.DtoReq req)
        {
            var pageModel = new PageFactory.BdPost().Get(req);
            return View(pageModel);
        }
    }
}