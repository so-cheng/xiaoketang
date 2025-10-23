using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;
using WeiCode.ModelDbs;
using Services.Project;
using WeiCode.Domain;
namespace WebProject.Areas.LiveRoom.Controllers
{
    public class RoomInfoController : Controller
    {
        /// <summary>
        /// 直播间查看详情(无需登录)
        /// </summary>
        /// <param name="liveroomid">直播间id</param>
        /// <returns></returns>
        public ActionResult Detail(int liveRoomId)
        {
            var liveRoom = DoMySql.FindEntity<ModelDb.p_liveroom>($" id={liveRoomId}");
            List<ModelDb.user_info_zhubo> list = new List<ModelDb.user_info_zhubo>();
            ViewBag.RoomName = liveRoom.name;
            if (!string.IsNullOrEmpty(liveRoom.zb_user_sn1))
            {
                var zb1 = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(liveRoom.zb_user_sn1);
                list.Add(zb1);
            }
            if (!string.IsNullOrEmpty(liveRoom.zb_user_sn2))
            {
                var zb2 = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(liveRoom.zb_user_sn2);
                list.Add(zb2);
            }
            return View(list);
        }
    }
}