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

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 目标
    /// </summary>
    public class TargetController : BaseLoginController
    {
        /// <summary>
        /// 上传日均数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DayPost(int id)
        {
            var req = new PageFactory.TingZhan.DayPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.DayPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.TingZhan.TargetPost.DtoReq();
            var pageModel = new PageFactory.TingZhan.TargetPost().Get(req);
            pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
            {
                returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
            };
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var req = new PageFactory.TingZhan.Edit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.Edit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅战参加列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Join(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "amont > 0";
            return View(pageModel);
        }

        /// <summary>
        /// 厅战不参加列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnJoin(int id = 0)
        {
            var req = new PageFactory.TingZhan.TargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.TargetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "amont = 0";
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标未提报名单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UnList(int id = 0)
        {
            var req = new PageFactory.TingZhan.UnTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UnTargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 对战匹配
        /// </summary>
        /// <returns></returns>
        public ActionResult GradeList(int id = 0)
        {
            if (id > 0)
            {
                var targets = DoMySql.FindListBySql<GradeDto>($"SELECT amont,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {id} and amont > 0 group by amont order by amont desc");
                ViewBag.targets = targets;
            }
            else
            {
                var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                if (!p_tingzhan.IsNullOrEmpty())
                {
                    var targets = DoMySql.FindListBySql<GradeDto>($"SELECT amont,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} and amont > 0 group by amont order by amont desc");
                    ViewBag.targets = targets;
                }
                else
                {
                    ViewBag.targets = new List<GradeDto>();
                }
            }
            ViewBag.id = id;

            return View();
        }

        public class GradeDto : ModelDb.p_tingzhan_target
        {
            public object ting_sum { get; set; }
        }
    }
}