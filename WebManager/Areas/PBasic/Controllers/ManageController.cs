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
using WeiCode.Modular;

namespace WebProject.Areas.PBasic.Controllers
{
    public class ManageController : BaseLoginController
    {
        /*
         * /// <summary>
        /// 账号列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int user_type_id = 0)
        {
            var userBaseTplReq = new UserBaseTplPara(user_type_id);
            userBaseTplReq.Add("keyword", "");
            var pageModel = new PageFactory.UserBaseTList().Get(userBaseTplReq);
            return View(pageModel);
        }
         */



        /// <summary>
        /// 账号列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult TgList(PageFactory.UserList.DtoReq req)
        {
            req.isShowZbInfo = false;
            var pageModel = new PageFactory.UserList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $" user_base.user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}'";
            //pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "UnDel").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "create").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("CreateTg")
            {
                title = "createtg",
                text = "添加厅管",
                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                {
                    url = @"PostTg"
                }
            });
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "InfoPost").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().eventOpenLayer.url = "PostTg";
            return View(pageModel);
        }

        public ActionResult ZbList(PageFactory.UserList.DtoReq req)
        {
            req.isShowZbInfo = true;
            var pageModel = new PageFactory.UserList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $" user_base.user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zber").id}'";
            //pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            //pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "UnDel").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Where(x => x.name == "create").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("CreateZb")
            {
                title = "createzb",
                text = "添加主播",
                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                {
                    url = @"PostZb"
                }
            });
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Post").FirstOrDefault().eventOpenLayer.url = "PostZb";
            return View(pageModel);
        }


        /// <summary>
        /// 创建厅管账号
        /// </summary>
        /// <returns></returns>
        public ActionResult PostTg(PageFactory.UserCreate.DtoReq req)
        {
            var pageModel = new PageFactory.ManagerCreateTg().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建主播账号
        /// </summary>
        /// <returns></returns>
        public ActionResult PostZb(PageFactory.UserCreate.DtoReq req)
        {
            var pageModel = new PageFactory.ManagerCreateZb().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 请假列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult VacationList(PageFactory.VacationList.DtoReq req)
        {
            var pageModel = new PageFactory.VacationList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "zb_user_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }
    }
}