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
namespace WebProject.Areas.Target.Controllers
{
    /// <summary>
    /// 厅管目标
    /// </summary>
    public class TgTargetController : BaseLoginController
    {

        /// <summary>
        /// 月度目标（列表）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult List(PageFactory.TgTargetList.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetList().Get(req);
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    field_paras = "id",
                    url = "Edit"
                },
                text = "编辑",
            });
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_user_sn").FirstOrDefault().disabled = false;
            return View(pageModel);
        }

        /// <summary>
        /// 月度目标（编辑）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Edit(PageFactory.TgTargetEdit.DtoReq req)
        {
            var pageModel = new PageFactory.TgTargetEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 每日目标
        /// </summary>
        /// <returns></returns>
        public ActionResult DayTarget(string date, string tg_user_sn, string yy_user_sn, string type = "amount")
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (yy_user_sn == "undefined")
            {
                yy_user_sn = "";
            }
            if (tg_user_sn == "undefined")
            {
                tg_user_sn = "";
            }
            if (type == "undefined")
            {
                type = "";
            }
            ViewBag.date = date;
            ViewBag.tg_user_sn = tg_user_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// 完成进度
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishSchedule(string date)
        {
            if (date.IsNullOrEmpty())
            {
                date = DateTime.Today.ToString("yyyy-MM");
            }
            ViewBag.date = date;

            return View();
        }

        /// <summary>
        /// 未提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult NotReported()
        {
            return View();
        }
    }
}