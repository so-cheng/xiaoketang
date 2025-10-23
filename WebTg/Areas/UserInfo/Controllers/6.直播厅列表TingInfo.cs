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

namespace WebProject.Areas.UserInfo.Controllers
{
    public class TingInfoController : BaseLoginController
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public ActionResult List()
        {
            var req = new PageFactory.UserInfo.TgInfoList.DtoReq();

            var pageModel = new PageFactory.UserInfo.TgInfoList().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Del").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Transform").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "DouUser").disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn = '{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        /// <summary>
        /// 编辑厅
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoEdit(int id = 0)
        {
            var req = new PageFactory.UserInfo.TgInfoEdit.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.TgInfoEdit().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 提交厅管背景信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Post(string jjr_name, PageFactory.UserInfo.TgInfoPost.DtoReq req)
        {

            var pageModel = new PageFactory.UserInfo.TgInfoPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 大头号
        /// </summary>
        public ActionResult DouUser(int id)
        {
            ViewBag.user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(id);
            return View();
        }

        /// <summary>
        /// 绑定大头号
        /// </summary>
        /// <param name="id">厅id</param>
        /// <param name="dou_user_type">0=dou_user, 1=dou_user1, 2=dou_user2</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SetDouUser(int id, int dou_user_type)
        {
            var req = new PageFactory.UserInfo.SetDouUserPost.DtoReq();
            req.id = id;
            req.dou_user_type = dou_user_type;
            var pageModel = new PageFactory.UserInfo.SetDouUserPost().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 解绑大头号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dou_user_type">0=dou_user, 1=dou_user1, 2=dou_user2</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelDouUser(int id, int dou_user_type)
        {
            var info = new JsonResultAction();
            try
            {
                var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(id);
                var anchor_id = "";
                switch (dou_user_type)
                {
                    case 0:
                        anchor_id = user_info_tg.dou_UID;
                        break;
                    case 1:
                        anchor_id = user_info_tg.dou_UID1;
                        break;
                    case 2:
                        anchor_id = user_info_tg.dou_UID2;
                        break;
                }
                // 绑定抖音大头号
                var JjrParam = new dyCheckParam()
                {
                    anchor_id = anchor_id
                };
                var setResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/DelTg", JjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                {
                    contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                }).ToJObject();
                if (setResult["code"].ToNullableString().Equals("0"))
                {
                    // 保存数据
                    switch (dou_user_type)
                    {
                        case 0:
                            user_info_tg.dou_user = "[null]";
                            user_info_tg.dou_UID = "[null]";
                            break;
                        case 1:
                            user_info_tg.dou_user1 = "[null]";
                            user_info_tg.dou_UID1 = "[null]";
                            break;
                        case 2:
                            user_info_tg.dou_user2 = "[null]";
                            user_info_tg.dou_UID2 = "[null]";
                            break;
                    }
                    user_info_tg.Update();
                }
                else
                {
                    throw new Exception("解绑失败");
                }
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 调用抖音接口请求参数
        /// </summary>
        public class dyCheckParam : Entity
        {
            /// <summary>
            /// 抖音账号
            /// </summary>
            public object dou_username { get; set; }
            /// <summary>
            /// 抖音作者id
            /// </summary>
            public object anchor_id { get; set; }
            /// <summary>
            /// 经纪人uid
            /// </summary>
            public object broker_id { get; set; }
        }
    }
}