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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboController : BaseLoginController
    {
        /// <summary>
        /// 新的主播
        /// </summary>
        /// <returns></returns>
        public ActionResult NewList()
        {
            var req = new PageFactory.UserInfo.NewList.DtoReq();
            var pageModel = new PageFactory.UserInfo.NewList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        #region 流失名单
        public ActionResult LossList()
        {
            var req = new PageFactory.UserInfo.LossList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LossList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 日志
        public ActionResult LogList()
        {
            var req = new PageFactory.UserInfo.LogList.DtoReq();
            var pageModel = new PageFactory.UserInfo.LogList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        #endregion

        #region 在职主播

        /// <summary>
        /// 创建账号/开通账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(string tg_user_sn, string ting_sn, int user_info_zhubo_id = 0)
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountPostFromNew.DtoReq();
            req.user_info_zhubo_id = user_info_zhubo_id;
            req.tg_user_sn = tg_user_sn;
            req.ting_sn = ting_sn;
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountPostFromNew().Get(req);

            return View(pageModel);
        }

        /// <summary>
        /// 编辑账号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string user_sn)
        {
            var req = new PageFactory.UserInfo.Zhubo_AccountEdit.DtoReq();
            req.id = new DomainBasic.UserApp().GetInfoByUserSn(user_sn).id;
            var pageModel = new PageFactory.UserInfo.Zhubo_AccountEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑背调
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult InfoEdit(int id)
        {
            var req = new PageFactory.UserInfo.OnJobPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.OnJobPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 主播列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OnJobList()
        {
            var req = new PageFactory.UserInfo.OnJobList.DtoReq();
            var pageModel = new PageFactory.UserInfo.OnJobList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 批量换厅
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult ChangeTing(string ids)
        {
            var req = new PageFactory.UserInfo.ChangeTing.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.UserInfo.ChangeTing().Get(req);
            return View(pageModel);
        }
        #endregion
        /// <summary>
        /// 离职主播
        /// </summary>
        /// <returns></returns>
        public ActionResult ResignList()
        {
            var req = new PageFactory.UserInfo.ResignList.DtoReq();
            var pageModel = new PageFactory.UserInfo.ResignList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"user_sn in (select t_user_sn from user_relation where relation_type_id =1 and f_user_sn = '{new UserIdentityBag().user_sn}')";
            
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