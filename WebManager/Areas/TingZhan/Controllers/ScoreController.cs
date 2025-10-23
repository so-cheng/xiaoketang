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
    /// 战绩
    /// </summary>
    public class ScoreController : BaseLoginController
    {
        /// <summary>
        /// 对战数据
        /// </summary>
        /// <param name="ting_sn"></param>
        /// <param name="yy_user_sn"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Total(string ting_sn, string yy_user_sn, int id = 0)
        {
            if (id > 0)
            {
                var query = $"SELECT amont,count(1) ting_sum FROM p_tingzhan_mate where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {id}";

                if (!yy_user_sn.IsNullOrEmpty()) query += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                        UserSn = yy_user_sn
                    } })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                            UserSn = yy_user_sn
                        }
                    })})";
                if (!ting_sn.IsNullOrEmpty()) query += $" and (ting_sn1 = '{ting_sn}' or ting_sn2 = '{ting_sn}')";

                var targets = DoMySql.FindListBySql<GradeDto>(query + " group by amont order by amont desc");
                ViewBag.targets = targets;
            }
            else
            {
                var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                if (!p_tingzhan.IsNullOrEmpty())
                {
                    var query = $"SELECT amont,count(1) ting_sum FROM p_tingzhan_mate where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {p_tingzhan.id}";

                    if (!yy_user_sn.IsNullOrEmpty()) query += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                            UserSn = yy_user_sn
                        }
                    })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                            UserSn = yy_user_sn
                        }
                    })})";
                    if (!ting_sn.IsNullOrEmpty()) query += $" and (ting_sn1 = '{ting_sn}' or ting_sn2 = '{ting_sn}')";

                    var targets = DoMySql.FindListBySql<GradeDto>(query + " group by amont order by amont desc");
                    ViewBag.targets = targets;
                }
                else
                {
                    ViewBag.targets = new List<GradeDto>();
                }
            }
            ViewBag.id = id;
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.ting_sn = ting_sn;

            return View();
        }

        public class GradeDto : ModelDb.p_tingzhan_target
        {
            public object ting_sum { get; set; }
        }

        /// <summary>
        /// 历史战绩
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryList(int id)
        {
            var req = new PageFactory.TingZhan.HistoryScoreList.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.HistoryScoreList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 战绩提报
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Post(int id)
        {
            var req = new PageFactory.TingZhan.ScorePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.TingZhan.ScorePost().Get(req);
            return View(pageModel);
        }
    }
}