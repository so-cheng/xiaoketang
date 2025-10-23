using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Xianxiazp.Controllers
{
    /// <summary>
    /// 面试
    /// </summary>
    public class InterviewerController : BaseLoginController
    {
        /// <summary>
        /// 面试列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Xianxiazp.YaoYueList.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.YaoYueList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"p_xianxiazp_info.wx_user_sn = '{new UserIdentityBag().user_sn}' and jianli_id > 0";
            pageModel.buttonGroup.buttonItems.Find(x => x.name == "post").text = "简历";
            pageModel.buttonGroup.buttonItems.Find(x => x.name == "post").eventOpenLayer.url = "PostSelect";
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Cancel").disabled = false;
            return View(pageModel);
        }

        /// <summary>
        /// 简历表
        /// </summary>
        /// <returns></returns>
        public ActionResult PostSelect()
        {
            var req = new PageFactory.Xianxiazp.ResumeList.DtoReq();
            var pageModel = new PageFactory.Xianxiazp.ResumeList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"wx_user_sn = '{new UserIdentityBag().user_sn}' and id not in (select jianli_id from p_xianxiazp_info)";
            return View(pageModel);
        }

        /// <summary>
        /// 编辑简历表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var req = new PageFactory.Xianxiazp.ResumeEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Xianxiazp.ResumeEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 登记表与简历关联
        /// </summary>
        /// <param name="jianli_id"></param>
        /// <returns></returns>
        public ActionResult CheckIn(int jianli_id)
        {
            var req = new PageFactory.Xianxiazp.ResumeCheckInPost.DtoReq();
            req.jianli_id = jianli_id;
            var pageModel = new PageFactory.Xianxiazp.ResumeCheckInPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 编辑面试信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.Xianxiazp.WX_Edit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Xianxiazp.WX_Edit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 邀约面试时间段统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ZPStatistics(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.date = nowDate;
                date = nowDate;
            }
            else
            {
                ViewBag.date = date;
            }
            var result = new ServiceFactory.ResumeService().GetXianXiaZpinfo(date.ToDateTime());
            ViewBag.List = result;
            return View();
        }
    }
}