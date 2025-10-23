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

namespace WebProject.Areas.PCrm.Controllers
{
    public class PCrmController : BaseLoginController
    {
        #region 粉丝列表
        /// <summary>
        /// 粉丝列表页
        /// </summary>
        public ActionResult CrmList(PageFactory.CrmList.Req req)
        {
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.listDisplay.listItems.Where(x => x.field == "tg_name").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems[0].disabled = true;
            pageModel.buttonGroup.buttonItems[1].disabled = true;
            pageModel.buttonGroup.buttonItems[2].disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.isHideOperate = true;
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.p_crm_customer.status_enum.正常.ToSByte()}";
            return View(pageModel);
        }
        /// <summary>
        /// 登记/编辑粉丝页面
        /// </summary>
        [HttpGet]
        public ActionResult CrmPost(int id = 0)
        {
            var req = new PageFactory.CrmPost.Req();
            req.p_Crm_Customer = new ModelDb.p_crm_customer();
            req.p_Crm_Customer.id = id;
            var pageModel = new PageFactory.CrmPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 类型设置
        
        /// <summary>
        /// 类型列表
        /// </summary>
        public ActionResult TypeList(PageFactory.TypeList.DtoReq req)
        {
            var pageModel = new PageFactory.TypeList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新建类型
        /// </summary>
        public ActionResult TypePost(PageFactory.TypePost.DtoReq req)
        {
            var pageModel = new PageFactory.TypePost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 等级管理
        /// <summary>
        /// 等级列表页
        /// </summary>
        public ActionResult GradeList(PageFactory.TypeGrade.DtoReq req)
        {
            var pageModel = new PageFactory.TypeGrade().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 新建等级
        /// </summary>
        [HttpGet]
        public ActionResult CradeCreate(PageFactory.TypeGradeCreate.DtoReq req)
        {
            var pageModel = new PageFactory.TypeGradeCreate().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}