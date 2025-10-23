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

namespace WebProject.Areas.Jixiao.Controllers
{
    /// <summary>
    /// 节奏跟进
    /// </summary>
    public class JiezouController : BaseLoginController
    {
        #region 节奏
        public ActionResult List(PageFactory.JiezouList.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"yy_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }
        public ActionResult Post(PageFactory.JiezouPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouPost().Get(req);
            return View(pageModel);
        }
        public ActionResult Item(string jiezou_sn)
        {
            if (jiezou_sn.IsNullOrEmpty())
            {
                var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"yy_user_sn='{new UserIdentityBag().user_sn}' order by id desc", false);

                jiezou_sn = jiezou.jiezou_sn;
            }
            ViewBag.jiezou_sn = jiezou_sn;
            ViewBag.yy_user_sn = new UserIdentityBag().user_sn;
            return View();
        }
        public JsonResult ReSetStep()
        {
            var info = new JsonResultAction();
            var list = new List<string>();
            string yy_user_sn = new UserIdentityBag().user_sn;
            try
            {
                foreach (ModelDbBasic.user_base item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管,yy_user_sn))
                {
                    var jiezou_item = DoMySql.FindEntity<ModelDb.jiezou_item>($"tg_user_sn='{item.user_sn}' and status=0", false);
                    if (jiezou_item.IsNullOrEmpty())
                    {
                        jiezou_item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        jiezou_item.yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, item.user_sn);
                        jiezou_item.tg_user_sn = item.user_sn;
                        jiezou_item.status = 0;
                    }
                    jiezou_item.start_date= UtilityStatic.CommonHelper.GetDbNullTime();
                    jiezou_item.step = 0;
                    CleanModel(jiezou_item);
                    list.Add(jiezou_item.InsertOrUpdateTran($"tg_user_sn='{item.user_sn}' and status=0"));
                }
                MysqlHelper.ExecuteSqlTran(list);
            }
            catch (Exception e)
            {
                info.msg = e.Message;
                info.code = 1;
            }
            return Json(info);
        }
        public JsonResult LastStep(string tg_user_sn, string jiezou_sn)
        {
            var info = new JsonResultAction();

            try
            {
                var jiezou_item = DoMySql.FindEntity<ModelDb.jiezou_item>($"tg_user_sn='{tg_user_sn}' and jiezou_sn='{jiezou_sn}' and status=0", false);
                switch (jiezou_item.step)
                {
                    case 0:
                        throw new Exception("已达到第一步");
                    case (decimal)0.5:
                        jiezou_item.step = 0;
                        jiezou_item.progress_1 = 0;
                        jiezou_item.time_plan_1 = UtilityStatic.CommonHelper.GetDbNullTime();
                        break;
                    case 1:
                        jiezou_item.step = (decimal)0.5;
                        jiezou_item.progress_2 = 0;
                        jiezou_item.time_plan_2 = UtilityStatic.CommonHelper.GetDbNullTime();
                        jiezou_item.progress_1 = ModelDb.jiezou_item.progress_3_enum.进行中.ToSByte();
                        jiezou_item.time_plan_1 = DateTime.Today;
                        break;
                    case 2:
                        jiezou_item.step = 1;
                        jiezou_item.progress_3 = 0;
                        jiezou_item.time_plan_3 = UtilityStatic.CommonHelper.GetDbNullTime();
                        jiezou_item.progress_2 = ModelDb.jiezou_item.progress_2_enum.进行中.ToSByte();
                        jiezou_item.time_plan_2 = DateTime.Today;
                        break;
                    case 3:
                        jiezou_item.step = 2;
                        jiezou_item.progress_4 = 0;
                        jiezou_item.time_plan_4 = UtilityStatic.CommonHelper.GetDbNullTime();
                        jiezou_item.progress_3 = ModelDb.jiezou_item.progress_3_enum.进行中.ToSByte();
                        jiezou_item.time_plan_3 = DateTime.Today;
                        break;
                    case 4:
                        jiezou_item.step = 3;
                        jiezou_item.progress_5 = 0;
                        jiezou_item.time_plan_5 = UtilityStatic.CommonHelper.GetDbNullTime();
                        jiezou_item.progress_4 = ModelDb.jiezou_item.progress_4_enum.进行中.ToSByte();
                        jiezou_item.time_plan_4 = DateTime.Today;
                        break;
                    case 5:
                        jiezou_item.step = 4;
                        jiezou_item.progress_5 = ModelDb.jiezou_item.progress_4_enum.进行中.ToSByte();
                        jiezou_item.time_plan_5 = DateTime.Today;
                        break;
                    default:
                        jiezou_item.step = (decimal)0.5;
                        CleanModel(jiezou_item);

                        jiezou_item.progress_1 = ModelDb.jiezou_item.progress_1_enum.进行中.ToSByte();

                        jiezou_item.time_plan_1 = DateTime.Today;
                        break;
                }

                jiezou_item.Update();
            }
            catch (Exception e)
            {
                info.msg = e.Message;
                info.code = 1;
            }


            return Json(info);
        }
        public JsonResult NextStep(string tg_user_sn,string jiezou_sn)
        {
            var info = new JsonResultAction();

            try
            {
                var jiezou_item = DoMySql.FindEntity<ModelDb.jiezou_item>($"tg_user_sn='{tg_user_sn}' and jiezou_sn='{jiezou_sn}' and status=0",false);

                switch (jiezou_item.step)
                {
                    case (decimal)0.5:
                        jiezou_item.step = 1;
                        jiezou_item.progress_1 = ModelDb.jiezou_item.progress_1_enum.已完成.ToSByte();
                        jiezou_item.progress_2 = ModelDb.jiezou_item.progress_2_enum.进行中.ToSByte(); 
                        jiezou_item.time_plan_1 = DateTime.Today;
                        break;
                    case 1:
                        jiezou_item.step = 2;
                        jiezou_item.progress_2 = ModelDb.jiezou_item.progress_2_enum.已完成.ToSByte();
                        jiezou_item.progress_3 = ModelDb.jiezou_item.progress_3_enum.进行中.ToSByte();
                        jiezou_item.time_plan_2 = DateTime.Today;
                        break;
                    case 2:
                        jiezou_item.step = 3;
                        jiezou_item.progress_3 = ModelDb.jiezou_item.progress_3_enum.已完成.ToSByte();
                        jiezou_item.progress_4 = ModelDb.jiezou_item.progress_4_enum.进行中.ToSByte();
                        jiezou_item.time_plan_3 = DateTime.Today;
                        break;
                    case 3:
                        jiezou_item.step = 4;
                        jiezou_item.progress_4 = ModelDb.jiezou_item.progress_4_enum.已完成.ToSByte();
                        jiezou_item.progress_5 = ModelDb.jiezou_item.progress_5_enum.进行中.ToSByte();
                        jiezou_item.time_plan_4 = DateTime.Today;
                        break;
                    case 4:
                        jiezou_item.step = 5;
                        jiezou_item.progress_5 = ModelDb.jiezou_item.progress_4_enum.已完成.ToSByte();
                        jiezou_item.time_plan_5 = DateTime.Today;
                        break;
                    case 5:
                        throw new Exception("已达到最后一步");
                    default:
                        jiezou_item.step = (decimal)0.5;
                        CleanModel(jiezou_item);

                        jiezou_item.progress_1 = ModelDb.jiezou_item.progress_1_enum.进行中.ToSByte();

                        jiezou_item.time_plan_1 = DateTime.Today;
                        break;
                }

                jiezou_item.Update();
            }
            catch (Exception e)
            {
                info.msg = e.Message;
                info.code = 1;
            }


            return Json(info);
        }
        private void CleanModel(ModelDb.jiezou_item jiezou_item)
        {
            jiezou_item.progress_1 = 0;
            jiezou_item.progress_2 = 0;
            jiezou_item.progress_3 = 0;
            jiezou_item.progress_4 = 0;
            jiezou_item.progress_5 = 0;

            jiezou_item.time_plan_1 = UtilityStatic.CommonHelper.GetDbNullTime();
            jiezou_item.time_plan_2 = UtilityStatic.CommonHelper.GetDbNullTime();
            jiezou_item.time_plan_3 = UtilityStatic.CommonHelper.GetDbNullTime();
            jiezou_item.time_plan_4 = UtilityStatic.CommonHelper.GetDbNullTime();
            jiezou_item.time_plan_5 = UtilityStatic.CommonHelper.GetDbNullTime();
        }
        #endregion
        #region 问题梳理
        /// <summary>
        /// 问题明细表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QList(PageFactory.JiezouQList.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
            return View(pageModel);
        }

        /// <summary>
        /// 新增问题提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QAdd(PageFactory.JiezouQAdd.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQAdd().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 修改问题提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult QPost(PageFactory.JiezouQPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouQPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 解决方案提交页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult SPost(PageFactory.JiezouSPost.DtoReq req)
        {
            var pageModel = new PageFactory.JiezouSPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 问题梳理统计表格
        /// </summary>
        /// <param name="jiezou_sn"></param>
        /// <returns></returns>
        public ActionResult QNSIndex(string keyword="")
        {
            ViewBag.keyword = keyword;
            return View();
        }
        #endregion
    }
}