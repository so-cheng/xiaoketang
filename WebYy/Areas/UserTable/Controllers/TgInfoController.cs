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

namespace WebProject.Areas.UserTable.Controllers
{
    public class TgInfoController : BaseLoginController
    {
        // GET: UserTable/TgInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 厅管信息完整度排名
        public ActionResult RankList(ServiceFactory.UserTable.TgInfoRankModel req)
        {

            return View();
        }
        public ActionResult List(PageFactory.UserTable.TgInfoRank.DtoReq req)
        {
            req.isShowZbInfo = false;
            req.isShowTgInfo = true;
            var yy_user_sn = new UserIdentityBag().user_sn;
            if (!req.yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = req.yy_user_sn;
            }
            var pageModel = new PageFactory.UserTable.TgInfoRank().Get(req);
            pageModel.buttonGroup.disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "Del").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "login").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("password_text")
            {
                text = "密码",
                width = "120",
                minWidth = "120",
            });
            pageModel.listDisplay.listData.attachFilterSql = $" user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn)} and ((user_base.attach3 not LIKE '%☆%☆%☆%' OR user_base.attach3 is null) or user_base.password='e10adc3949ba59abbe56e057f20f883e')";

            return View(pageModel);
        }
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post(PageFactory.UserTable.UserCreate.DtoReq req)
        {
            req.user_type = "tger";
            var pageModel = new PageFactory.UserTable.UserCreate().Get(req);
            return View(pageModel);
        }
        #endregion
    }


}