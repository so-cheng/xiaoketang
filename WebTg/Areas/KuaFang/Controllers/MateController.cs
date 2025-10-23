using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using WeiCode.ModelDbs;
using WeiCode.DataBase;
using WeiCode.Utility;

namespace WebProject.Areas.KuaFang.Controllers
{
    public class MateController : BaseLoginController
    {
        /// <summary>
        /// 匹配列表
        /// </summary>
        /// <param name="amont_s"></param>
        /// <param name="amont_e"></param>
        /// <param name="ting_name"></param>
        /// <returns></returns>
        public ActionResult List(string amont_s, string amont_e, string ting_name)
        {
            if (!amont_s.IsValidNumeric())
            {
                amont_s = null;
            }
            if (!amont_e.IsValidNumeric())
            {
                amont_e = null;
            }

            // 显示跨团队和自己团队内的跨房
            var where = $@"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {new ServiceFactory.KuaFang.Common().getNewKuaFang().id} and (ting_sn2 is null or ting_sn2 = '') and (is_open = {ModelDb.p_kuafang_mate.is_open_enum.是.ToSByte()} or (is_open = {ModelDb.p_kuafang_mate.is_open_enum.否.ToSByte()} and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
            {
                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                {
                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                    UserSn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).yy_sn
                }
            })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
            {
                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                {
                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                    UserSn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).yy_sn
                }
            })})))";

            if (!amont_s.IsNullOrEmpty())
            {
                where += $" and amont >= {amont_s}";
            }
            if (!amont_e.IsNullOrEmpty())
            {
                where += $" and amont <= {amont_e}";
            }
            if (!ting_name.IsNullOrEmpty())
            {
                where += $" and ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter { attachWhere = $"ting_name like '%{ting_name}%'" })}";
            }

            var list = DoMySql.FindList<ModelDb.p_kuafang_mate>(where);
            ViewBag.mate_list = list;
            ViewBag.amont_s = amont_s;
            ViewBag.amont_e = amont_e;
            ViewBag.ting_name = ting_name;

            return View();
        }

        /// <summary>
        /// 提报发起
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            var req = new PageFactory.KuaFangMate.MatePost.DtoReq();
            var pageModel = new PageFactory.KuaFangMate.MatePost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 填报信息（我的）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyList()
        {
            var req = new PageFactory.KuaFangMate.MyList.DtoReq();
            var pageModel = new PageFactory.KuaFangMate.MyList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 对方信息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfoList()
        {
            var req = new PageFactory.KuaFangMate.UserInfoList.DtoReq();
            var pageModel = new PageFactory.KuaFangMate.UserInfoList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 对方信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserInfo(int id)
        {
            var req = new PageFactory.KuaFangMate.UserInfoView.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.KuaFangMate.UserInfoView().Get(req);
            return View(pageModel);
        }
    }
}