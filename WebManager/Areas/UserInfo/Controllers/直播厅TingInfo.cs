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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TingInfoController : BaseLoginController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public ActionResult List(PageFactory.UserInfo.TgInfoList.DtoReq req)
        {
            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            //pageModel.listDisplay.listItems.Where(x => x.field == "username").FirstOrDefault().disabled = false;
            //pageModel.buttonGroup.buttonItems.Where(x => x.name == "InfoPost").FirstOrDefault().disabled = false;

            return View(pageModel);
        }

        /// <summary>
        /// 新开厅
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoPost(string jjr_name, string out_para, string tg_user_sn,string dou_user)
        {
            var req = new PageFactory.UserInfo.TgInfoPost.DtoReq();
            req.jjr_name = jjr_name;
            req.out_para = out_para.ToInt();
            req.tg_user_sn = tg_user_sn;
            req.dou_user = dou_user;
            var pageModel = new PageFactory.UserInfo.TgInfoPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑厅
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoEdit(int id = 0)
        {
            var req = new PageFactory.UserInfo.TgInfoEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.TgInfoEdit().Get(req);
            return View(pageModel);
        }


        /// <summary>
        /// 转移所属厅管
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Transform(PageFactory.UserInfo.TransformPost.DtoReq req)
        {
            Response.Write("待开通");Response.End();
            var pageModel = new PageFactory.UserInfo.TransformPost().Get(req);
            return View(pageModel);
        }

        #region 日志
        public ActionResult LogList()
        {
            var req = new PageFactory.UserInfo.TingLogList.DtoReq();
            var pageModel = new PageFactory.UserInfo.TingLogList().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}