using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.TingZhanService;

namespace WebProject.Areas.TingZhan.Controllers
{
    public class PaibuController : BaseLoginController
    {
        /// <summary>
        /// 厅战排布
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.PaibuList.DtoReq();
            var pageModel = new PageFactory.PaibuList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 上传日均数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DayPost(int id)
        {
            var req = new PageFactory.DayPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.DayPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 创建/编辑厅战期数
        /// </summary>
        /// <returns></returns>
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.PaibuPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.PaibuPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TargetList()
        {
            var req = new PageFactory.TargetList.DtoReq();
            var pageModel = new PageFactory.TargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 厅战目标新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TargetPost()
        {
            var req = new PageFactory.TargetPost.DtoReq();
            var pageModel = new PageFactory.TargetPost().Get(req);
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
        public ActionResult TargetListPost(int id)
        {
            var req = new PageFactory.TargetListPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TargetListPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 目标分档
        /// </summary>
        /// <returns></returns>
        public ActionResult GradeList()
        {
            var p_tingzhan = new PageFactory.PaibuService().getNewTingzhan();
            if (!p_tingzhan.IsNullOrEmpty())
            {
                var targets = DoMySql.FindListBySql<GradeDto>($"SELECT amont,count(1) ting_sum FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} and amont > 0 group by amont order by amont desc");
                ViewBag.targets = targets;
            }
            else
            {
                ViewBag.targets = new List<GradeDto>();
            }
            return View();
        }

        public class GradeDto : ModelDb.p_tingzhan_target
        {
            public object ting_sum { get; set; }
        }

        /// <summary>
        /// 战绩汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult ScoreTotal()
        {
            var p_tingzhan = new PageFactory.PaibuService().getNewTingzhan();
            if (!p_tingzhan.IsNullOrEmpty())
            {
                var targets = DoMySql.FindListBySql<GradeDto>($"SELECT amont,count(1) ting_sum FROM p_tingzhan_mate where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} group by amont order by amont desc");
                ViewBag.targets = targets;
            }
            else
            {
                ViewBag.targets = new List<GradeDto>();
            }
            return View();
        }

        public ActionResult Mate()
        {
            var p_tingzhan = new PageFactory.PaibuService().getNewTingzhan();
            if (p_tingzhan.IsNullOrEmpty())
            {
                ViewBag.mates1 = new List<ModelDb.p_tingzhan_mate>();
                ViewBag.mates2 = new List<ModelDb.p_tingzhan_mate>();
            }
            else
            {
                var mates1 = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} and amont > 10 order by id");
                var mates2 = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id} and amont <= 10 order by id");

                ViewBag.mates1 = mates1;
                ViewBag.mates2 = mates2;
            }

            return View();
        }

        /// <summary>
        /// 生成对战表
        /// </summary>
        /// <param name="tingzhan_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Mate(int tingzhan_id)
        {
            var info = new JsonResultAction();
            try
            {
                List<string> lSql = new List<string>();

                var mates = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id}");
                if (mates.Count > 0) throw new WeicodeException("对战表已存在");

                int sort = 1;
                var amonts = DoMySql.FindListBySql<ModelDb.p_tingzhan_target>($"SELECT amont FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and amont > 0 group by amont order by amont desc");
                foreach (var item in amonts)
                {
                    var tingTargets = new List<TingTarget>();
                    var targets = DoMySql.FindList<ModelDb.p_tingzhan_target>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and amont = {item.amont}");
                    foreach (var target in targets)
                    {
                        tingTargets.Add(new TingTarget
                        {
                            yy_sn = target.yy_user_sn,
                            name = target.tg_user_sn,
                            day_amount = target.day_amount.ToDecimal()
                        });
                    }
                    var list = new ServiceFactory.TingZhanService().PaiItem(tingTargets, new List<TingTargetDui>());
                    foreach (var ting in list)
                    {
                        var p_tingzhan_mate = new ModelDb.p_tingzhan_mate();
                        p_tingzhan_mate.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_tingzhan_mate.tingzhan_id = tingzhan_id;
                        p_tingzhan_mate.tg_user_sn1 = ting.tingTarget1.name;
                        p_tingzhan_mate.tg_user_sn2 = ting.tingTarget2.name;
                        p_tingzhan_mate.amont = item.amont;
                        p_tingzhan_mate.sort = sort;

                        lSql.Add(p_tingzhan_mate.InsertTran());
                        sort++;
                    }
                }
                DoMySql.ExecuteSqlTran(lSql);
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 删除对战表
        /// </summary>
        /// <param name="tingzhan_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelMate(int tingzhan_id)
        {
            var info = new JsonResultAction();
            try
            {
                var p_tingzhan_mate = new ModelDb.p_tingzhan_mate();
                p_tingzhan_mate.Delete($"tingzhan_id = {tingzhan_id}");
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 对战厅互换
        /// </summary>
        /// <param name="tg_ids"></param>
        /// <returns></returns>
        public JsonResult MateReplace(string[] tg_ids)
        {
            var info = new JsonResultAction();

            try
            {
                if (tg_ids.IsNullOrEmpty() || tg_ids.Length != 2) throw new WeicodeException("请选择两个厅进行调换");

                var repalce_tg_sn1 = "";
                var repalce_tg_sn2 = "";

                var mate1 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = '{tg_ids[0].Substring(2)}'");
                var mate2 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = '{tg_ids[1].Substring(2)}'");

                if (mate1.id.Equals(mate2.id)) throw new WeicodeException("请选择不同对战");

                if (tg_ids[0].Contains("l_")) repalce_tg_sn1 = mate1.tg_user_sn1;
                if (tg_ids[0].Contains("r_")) repalce_tg_sn1 = mate1.tg_user_sn2;

                if (tg_ids[1].Contains("l_")) repalce_tg_sn2 = mate2.tg_user_sn1;
                if (tg_ids[1].Contains("r_")) repalce_tg_sn2 = mate2.tg_user_sn2;

                List<string> lSql = new List<string>();
                if (tg_ids[0].Contains("l_")) mate1.tg_user_sn1 = repalce_tg_sn2;
                if (tg_ids[0].Contains("r_")) mate1.tg_user_sn2 = repalce_tg_sn2;
                lSql.Add(mate1.UpdateTran());

                if (tg_ids[1].Contains("l_")) mate2.tg_user_sn1 = repalce_tg_sn1;
                if (tg_ids[1].Contains("r_")) mate2.tg_user_sn2 = repalce_tg_sn1;
                lSql.Add(mate2.UpdateTran());

                DoMySql.ExecuteSqlTran(lSql);
            }
            catch (Exception e)
            {
                info.msg = e.Message;
                info.code = 1;
            }

            return Json(info);
        }
    }
}