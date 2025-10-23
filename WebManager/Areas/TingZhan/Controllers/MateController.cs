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
using static Services.Project.ServiceFactory.TingZhanService;

namespace WebProject.Areas.TingZhan.Controllers
{
    /// <summary>
    /// 对战
    /// </summary>
    public class MateController : BaseLoginController
    {
        /// <summary>
        /// 生成战表页面
        /// </summary>
        /// <param name="ting_sn"></param>
        /// <param name="yy_user_sn"></param>
        /// <returns></returns>
        public ActionResult Index(string ting_sn, string yy_user_sn)
        {
            var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
            if (p_tingzhan.IsNullOrEmpty())
            {
                ViewBag.mates1 = new List<ModelDb.p_tingzhan_mate>();
                ViewBag.mates2 = new List<ModelDb.p_tingzhan_mate>();
            }
            else
            {
                var query = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id}";

                if (!yy_user_sn.IsNullOrEmpty()) query += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                        UserSn = yy_user_sn
                    }
                })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                        UserSn = yy_user_sn
                    }
                })})";
                if (!ting_sn.IsNullOrEmpty()) query += $" and (ting_sn1 = '{ting_sn}' or ting_sn2 = '{ting_sn}')";

                var mates1 = DoMySql.FindList<ModelDb.p_tingzhan_mate>(query + " and amont > 10 order by id");
                var mates2 = DoMySql.FindList<ModelDb.p_tingzhan_mate>(query + " and amont <= 10 order by id");

                ViewBag.mates1 = mates1;
                ViewBag.mates2 = mates2;
            }
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.ting_sn = ting_sn;

            return View();
        }

        /// <summary>
        /// 生成对战表
        /// </summary>
        /// <param name="tingzhan_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Index(int tingzhan_id)
        {
            var info = new JsonResultAction();
            try
            {
                List<string> lSql = new List<string>();

                var mates = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id}");
                if (mates.Count > 0) throw new WeicodeException("对战表已存在");

                // 对手匹配筛选 1.筛选目标相同厅 2.筛选日均相近的厅 3.筛选近三场厅战完成率相近的厅
                int sort = 1;
                var amonts = DoMySql.FindListBySql<ModelDb.p_tingzhan_target>($"SELECT amont FROM p_tingzhan_target where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and amont > 0 group by amont order by amont desc");// 筛选目标相同厅（目标分组）
                foreach (var item in amonts)
                {
                    var tingTargets = new List<TingTarget>();
                    var targets = DoMySql.FindList<ModelDb.p_tingzhan_target>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and amont = {item.amont}");
                    var newTargets = new ServiceFactory.TingZhanService().TargetsOrderByDayAmountAndCompletion(targets, new List<TingMate>());// 筛选日均相近的厅，筛选近三场厅战完成率相近的厅（按日均数据排序和厅战完成率排序）
                    foreach (var target in newTargets)
                    {
                        var no_tgs = new List<string>();
                        foreach (var _item in DoMySql.FindList<ModelDb.p_tingzhan_mate_rule>
                            ($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and rule_type = {ModelDb.p_tingzhan_mate_rule.rule_type_enum.不跟厅打.ToSByte()} and ting_sn1 = '{target.ting_sn}'"))
                        {
                            no_tgs.Add(_item.ting_sn2);
                        }

                        tingTargets.Add(new TingTarget
                        {
                            yy_sn = target.yy_user_sn,
                            name = target.ting_sn,
                            day_amount = target.day_amount.ToDecimal(),
                            no_tgs = no_tgs,
                            yes_ting = DoMySql.FindEntity<ModelDb.p_tingzhan_mate_rule>
                            ($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhan_id} and rule_type = {ModelDb.p_tingzhan_mate_rule.rule_type_enum.跟厅打.ToSByte()} and ting_sn1 = '{target.ting_sn}'", false).ting_sn2,
                            last_ting = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>
                            ($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and ting_sn1 = '{target.ting_sn}' order by id desc", false).ting_sn2
                        });
                    }

                    // 排厅战
                    var list = new ServiceFactory.TingZhanService().PaiItem(tingTargets, new List<TingTargetDui>());
                    foreach (var ting in list)
                    {
                        var p_tingzhan_mate = new ModelDb.p_tingzhan_mate()
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            tingzhan_id = tingzhan_id,
                            tg_user_sn1 = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting.tingTarget1.name).tg_user_sn,
                            ting_sn1 = ting.tingTarget1.name,
                            tg_user_sn2 = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting.tingTarget2.name).tg_user_sn,
                            ting_sn2 = ting.tingTarget2.name,
                            amont = item.amont,
                            sort = sort
                        };

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
        public JsonResult Del(int tingzhan_id)
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
        public JsonResult Replace(string[] tg_ids)
        {
            var info = new JsonResultAction();

            try
            {
                if (tg_ids.IsNullOrEmpty() || tg_ids.Length != 2) throw new WeicodeException("请选择两个厅进行调换");

                var repalce_tg_sn1 = "";
                var repalce_ting_sn1 = "";
                var repalce_tg_sn2 = "";
                var repalce_ting_sn2 = "";

                var mate1 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = '{tg_ids[0].Substring(2)}'");
                var mate2 = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = '{tg_ids[1].Substring(2)}'");

                if (mate1.id.Equals(mate2.id)) throw new WeicodeException("请选择不同对战");

                if (tg_ids[0].Contains("l_"))
                {
                    repalce_tg_sn1 = mate1.tg_user_sn1;
                    repalce_ting_sn1 = mate1.ting_sn1;
                }
                else if (tg_ids[0].Contains("r_"))
                {
                    repalce_tg_sn1 = mate1.tg_user_sn2;
                    repalce_ting_sn1 = mate1.ting_sn2;
                }

                if (tg_ids[1].Contains("l_"))
                {
                    repalce_tg_sn2 = mate2.tg_user_sn1;
                    repalce_ting_sn2 = mate2.ting_sn1;
                }
                else if (tg_ids[1].Contains("r_"))
                {
                    repalce_tg_sn2 = mate2.tg_user_sn2;
                    repalce_ting_sn2 = mate2.ting_sn2;
                }

                List<string> lSql = new List<string>();

                if (tg_ids[0].Contains("l_"))
                {
                    mate1.tg_user_sn1 = repalce_tg_sn2;
                    mate1.ting_sn1 = repalce_ting_sn2;
                }
                else if (tg_ids[0].Contains("r_"))
                {
                    mate1.tg_user_sn2 = repalce_tg_sn2;
                    mate1.ting_sn2 = repalce_ting_sn2;
                }
                lSql.Add(mate1.UpdateTran());

                if (tg_ids[1].Contains("l_"))
                {
                    mate2.tg_user_sn1 = repalce_tg_sn1;
                    mate2.ting_sn1 = repalce_ting_sn1;
                }
                else if (tg_ids[1].Contains("r_"))
                {
                    mate2.tg_user_sn2 = repalce_tg_sn1;
                    mate2.ting_sn2 = repalce_ting_sn1;
                }
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

        public ActionResult ExportToExcel()
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = "对战表格.xls";
            string SheetName = "对战表格";

            AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            AppLibrary.WriteExcel.Cells cells = sheet.Cells;
            //第一行表头
            cells.Add(1, 1, "队伍A运营");
            cells.Add(1, 2, "队伍A厅名");
            cells.Add(1, 3, "队伍A日均音浪");
            cells.Add(1, 4, "队伍B运营");
            cells.Add(1, 5, "队伍B厅名");
            cells.Add(1, 6, "队伍B日均音浪");
            cells.Add(1, 7, "目标音浪");

            var mates = DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {new ServiceFactory.TingZhanService().getNewTingzhan().id} order by id");
            int sort = 1;
            foreach (var mate in mates)
            {
                sort++;
                var tingzhan_target1 = DoMySql.FindEntity<ModelDb.p_tingzhan_target>($"tingzhan_id = {mate.tingzhan_id} and ting_sn = '{mate.ting_sn1}'", false);
                var tingzhan_target2 = DoMySql.FindEntity<ModelDb.p_tingzhan_target>($"tingzhan_id = {mate.tingzhan_id} and ting_sn = '{mate.ting_sn2}'", false);
                cells.Add(sort, 1, new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(tingzhan_target1.yy_user_sn).name);
                cells.Add(sort, 2, new ServiceFactory.UserInfo.Ting().GetTingBySn(mate.ting_sn1).ting_name);
                cells.Add(sort, 3, tingzhan_target1.day_amount);
                cells.Add(sort, 4, new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(tingzhan_target2.yy_user_sn).name);
                cells.Add(sort, 5, new ServiceFactory.UserInfo.Ting().GetTingBySn(mate.ting_sn2).ting_name);
                cells.Add(sort, 6, tingzhan_target2.day_amount);
                cells.Add(sort, 7, mate.amont);
            }

            doc.Send();
            Response.Flush();
            Response.End();

            var info = new JsonResultAction();
            return Json(info);
        }

        /// <summary>
        /// 修改对战厅
        /// </summary>
        /// <param name="type">1 左ting_sn1，2 右ting_sn2</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeTgPost(int type = 0, int id = 0)
        {
            var req = new PageFactory.TingZhan.MateChangeTgPost.DtoReq();
            req.type = type;
            req.id = id;
            var pageModel = new PageFactory.TingZhan.MateChangeTgPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 新增对战关系
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            var req = new PageFactory.TingZhan.AddMatePost.DtoReq();
            var pageModel = new PageFactory.TingZhan.AddMatePost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 删除对战关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelMate(int id)
        {
            var info = new JsonResultAction();
            try
            {
                var p_tingzhan_mate = DoMySql.FindEntityById<ModelDb.p_tingzhan_mate>(id);
                // 删除目标
                new ModelDb.p_tingzhan_target().Delete($"tingzhan_id = {p_tingzhan_mate.tingzhan_id} and (ting_sn = '{p_tingzhan_mate.ting_sn1}' or ting_sn = '{p_tingzhan_mate.ting_sn2}')");

                // 删除对战关系
                new ModelDb.p_tingzhan_mate().Delete($"id = {id}");
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 战绩列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.TingZhan.MateList.DtoReq();
            var pageModel = new PageFactory.TingZhan.MateList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩已提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult PostList(int id = 0)
        {
            var req = new PageFactory.TingZhan.MateTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.MateTargetList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩未提报名单
        /// </summary>
        /// <returns></returns>
        public ActionResult UnPostList(int id = 0)
        {
            var req = new PageFactory.TingZhan.UnMateTargetList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.UnMateTargetList().Get(req);
            return View(pageModel);
        }
    }
}