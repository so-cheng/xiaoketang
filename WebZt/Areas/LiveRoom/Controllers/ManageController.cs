using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;
namespace WebProject.Areas.LiveRoom.Controllers
{
    /// <summary>
    /// 直播间管理
    /// </summary>
    public class ManageController : BaseLoginController
    {
        /// <summary>
        /// 直播间列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var result = new JsonResultAction();
            try
            {
                var tradeProductInfos = new List<PageFactory.LiveRoom.LiveRoomList.ItemDataModel>();

                string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'";
                //中台看自己的,管理端看全部
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                    case ModelEnum.UserTypeEnum.manager:
                        break;
                    default:
                        where += $" and 1!=1";
                        break;
                }
                List<ModelDb.p_liveroom> qrlist = DoMySql.FindList<ModelDb.p_liveroom>(where);
                foreach (var item in qrlist)
                {
                    tradeProductInfos.Add(new Services.Project.PageFactory.LiveRoom.LiveRoomList.ItemDataModel(item));
                }
                result.data = tradeProductInfos;
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }

        /// <summary>
        /// 直播间列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


    }
}