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
        #region 单厅管粉丝列表
        /// <summary>
        /// 当前厅管粉丝列表
        /// </summary>
        public ActionResult SingleTgFensiList(PageFactory.CrmList.Req req)
        {
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "import").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listItems.Where(x => x.field == "tg_name").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.p_crm_customer.status_enum.正常.ToSByte()} and tg_user_sn in {new ServiceFactory.UserInfo.Tg().GetTreeOptionForSql(new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        #endregion

        #region 流失回收列表
        /// <summary>
        /// 流失回收列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Recover()
        {
            var req = new PageFactory.CrmList.Req();
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.listFilter.disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "recover").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "edit").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "del").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.listData.attachFilterSql = $"status = {ModelDb.p_crm_customer.status_enum.逻辑删除.ToSByte()} and tg_user_sn in {new ServiceFactory.UserInfo.Tg().GetTreeOptionForSql(new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        #endregion

        #region 导入用户
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            var req = new PageFactory.CrmImport.DtoReq();
            var pageModel = new PageFactory.CrmImport().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 编辑粉丝信息
        /// <summary>
        /// 编辑粉丝页面
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

        #region 多厅管粉丝列表
        /// <summary>
        /// 多厅管的粉丝列表
        /// </summary>
        public ActionResult MultiTgFensiList(PageFactory.CrmList.Req req)
        {
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.listItems.Where(x => x.field == "tg_name").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)}";
            return View(pageModel);
        }
        #endregion
    }
}