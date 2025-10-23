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

namespace WebProject.Areas.Cencai.Controllers
{
    /// <summary>
    /// 成才主播PK
    /// </summary>
    public class PKController : BaseLoginController
    {
        #region PK主页
        /// <summary>
        /// 编辑PK
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(PageFactory.Cencai.CencaiPKPost.DtoReq req)
        {
            req.u_type = (int)ModelDb.p_cencai_pk.u_type_enum.超管;
            req.user_sn = new UserIdentityBag().user_sn;
            var pageModel = new PageFactory.Cencai.CencaiPKPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 成才PKlist
        /// </summary>
        /// <returns></returns>
        public ActionResult List(PageFactory.Cencai.CencaiPKList.DtoReq req)
        {
            var pageModel = new PageFactory.Cencai.CencaiPKList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region PK子页面
        /// <summary>
        /// 成才Itemlist
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemList(PageFactory.Cencai.CencaiPKItemList.DtoReq req)
        {
            var pageModel = new PageFactory.Cencai.CencaiPKItemList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑item
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemPost(PageFactory.Cencai.CencaiPKItemPost.DtoReq req)
        {
            var pageModel = new PageFactory.Cencai.CencaiPKItemPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// PK数据列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PKDataList(string pk_sn, string yy_user_sn, string tg_user_sn, string c_date)
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'").user_sn;
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }

            ViewBag.pk_sn = pk_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.c_date = c_date;
            return View();
        }
        #endregion
    }
}