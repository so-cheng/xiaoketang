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
    public class JiezouController : BaseLoginController
    {

        public ActionResult Item()
        {
            var yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn);
            var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"yy_user_sn='{yy_user_sn}' and status=0 order by create_time desc",false);
            ViewBag.jiezou_sn = jiezou.jiezou_sn;
            ViewBag.yy_user_sn = yy_user_sn;
            if (new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀厅管,new UserIdentityBag().user_sn).Count>0)
            {
                return View("ItemTg");
            }
            return View();
        }

        public ActionResult QNSIndex(string keyword="")
        {
            ViewBag.keyword = keyword;
            return View();
        }
    }
}