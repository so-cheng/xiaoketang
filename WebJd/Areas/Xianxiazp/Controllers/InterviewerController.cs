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
            pageModel.listFilter.formItems.Find(x => x.name == "interview_dateRange").disabled = false;
            pageModel.listFilter.formItems.Find(x => x.name == "dateRange").disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"jd_user_sn = '{new UserIdentityBag().user_sn}' and jianli_id > 0";
            pageModel.listDisplay.listItems.Find(x => x.field == "zt_name").disabled = true;
            pageModel.buttonGroup.disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "JianLi").disabled = false;
            return View(pageModel);
        }

        /// <summary>
        /// 编辑面试信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.Xianxiazp.ZT_Edit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Xianxiazp.ZT_Edit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 查看简历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult JianLi(int id)
        {
            var req = new PageFactory.Xianxiazp.ResumeView.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Xianxiazp.ResumeView().Get(req);
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