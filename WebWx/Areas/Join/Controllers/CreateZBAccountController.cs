using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

namespace WebProject.Areas.Join.Controllers
{
    public class CreateZBAccountController : BaseLoginController
    {
        #region 显示厅管申请列表
        public ActionResult TGApplicationList(int completeStatus = 0)//0:"未完成"; 1:"已完成"
        {
            var req = new PageFactory.Join.TGApplyZbList.DtoReq();
            req.completeStatus = completeStatus;
            var pageModel = new PageFactory.Join.TGApplyZbList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 创建主播页面
        public ActionResult CreateZbPage()
        {
            var req = new PageFactory.Join.CreateZBPage.DtoReq();
            var pageModel = new PageFactory.Join.CreateZBPage().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 主播名单
        public ActionResult ZbList(int id = 0)
        {
            var req = new PageFactory.Join.ZbList.DtoReq();
            req.tg_need_id = id;
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 补人表单详情
        public ActionResult ZbDetails(int id = 0)
        {
            var req = new PageFactory.Join.ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.ZbDetails().Get(req);
            return View(pageModel);
        }
        #endregion

    }
}