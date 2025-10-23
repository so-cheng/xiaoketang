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

        /// <summary>
        /// 给直播间分配主播
        /// </summary>
        /// <returns></returns>
        public ActionResult RepartitionZB(int id = 0, int operatorType = 1)
        {
            ViewBag.liveRoomName = DoMySql.FindEntityById<ModelDb.p_liveroom>(id);
            ViewBag.operatorType = operatorType;
            return View();
        }

        /// <summary>
        /// 给直播间分配/转移主播
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRepartitionZB(int id = 0, int type = 1, int operatorType = 1)
        {
            var req = new PageFactory.LiveRoom.RepartitionZB.DtoReq();
            req.id = id;
            req.type = type;
            req.operatorType = operatorType;
            var pageModel = new PageFactory.LiveRoom.RepartitionZB().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 转移主播
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        public ActionResult TransferZB(int id = 0, int type = 1, int operatorType = 1)
        {
            var req = new PageFactory.LiveRoom.RepartitionZB.DtoReq();
            req.id = id;
            req.type = type;
            req.operatorType = operatorType;
            var pageModel = new PageFactory.LiveRoom.RepartitionZB().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 删除分配绑定的主播
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult DelRepartitionZB(int id = 0, int type = 1)
        {
            var req = new PageFactory.LiveRoom.RepartitionZB.DtoReq();
            var pageModel = new PageFactory.LiveRoom.RepartitionZB().DeleteZbAction(id, type);
            return Json(pageModel);
        }
    }
}