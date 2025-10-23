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
    /// <summary>
    /// 直播间维护
    /// </summary>
    public class ConfigController : BaseLoginController
    {

        /// <summary>
        /// 直播间列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var req = new PageFactory.LiveRoom.LiveRoomList.DtoReq();
            var pageModel = new PageFactory.LiveRoom.LiveRoomList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 直播间修改
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomPost(int id = 0)
        {
            var req = new PageFactory.LiveRoom.LiveRoomPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.LiveRoom.LiveRoomPost().Get(req);
            return View(pageModel);
        }



        /// <summary>
        /// 区域类别列表
        /// </summary>
        public ActionResult AreaCate()
        {
            var req = new PageFactory.LiveRoom.LiveRoomCateList.DtoReq();
            var pageModel = new PageFactory.LiveRoom.LiveRoomCateList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 添加区域类别
        /// </summary>
        /// <returns></returns>
        public ActionResult AreaCatePost(int id = 0)
        {
            var req = new PageFactory.LiveRoom.LiveRoomCatePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.LiveRoom.LiveRoomCatePost().Get(req);
            return View(pageModel);
        }

    }
}