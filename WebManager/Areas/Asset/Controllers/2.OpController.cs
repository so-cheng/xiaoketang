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

namespace WebProject.Areas.Asset.Controllers
{
    /// <summary>
    /// 资产管理
    /// </summary>
    public class OpController : BaseController
    {
        #region 1.派发记录
        /// <summary>
        /// 1.1已发派资产记录
        /// </summary>
        /// <returns></returns>
        public ActionResult AcceptList()
        {
            var req = new PageFactory.Asset.AssetAcceptList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetAcceptList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 1.2发派资产提交表单
        /// </summary>
        /// <param name="id">资产id</param>
        /// <param name="asset_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AcceptPost(int id = 0)
        {
            var req = new PageFactory.Asset.AssetAcceptPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetAcceptPost().Get(req);
            return View(pageModel);
        }
       
        /// <summary>
        /// 1.3派发详情表
        /// </summary>
        /// <param name="out_sn"></param>
        /// <returns></returns>
        public ActionResult AcceptContentList(string out_sn)
        {
            var req = new PageFactory.Asset.AssetAcceptContentList.DtoReq();
            req.out_sn = out_sn;
            var pageModel = new PageFactory.Asset.AssetAcceptContentList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 1.4删除派发资产
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult acceptDel(int id = 0)
        {
            var info = new JsonResultAction(null);
            var Asset = DoMySql.FindEntity<ModelDb.asset_in>($"id=' {id}'", true);
            Asset.Delete();
            return Json(info);
        }
        #endregion
        #region 2.归还记录

        /// <summary>
        /// 2.1资产归还记录
        /// </summary>
        /// <returns></returns>
        public ActionResult BackList()
        {
            var req = new PageFactory.Asset.AssetBackList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetBackList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 2.2发派资产提交表单
        /// </summary>
        /// <param name="id">资产id</param>
        /// <param name="asset_id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BackPost(int id = 0)
        {
            var req = new PageFactory.Asset.AssetBackPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetBackPost().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 2.3资产退库详细表
        /// </summary>
        /// <param name="in_sn"></param>
        /// <returns></returns>
        public ActionResult BackContentList(string in_sn)
        {
            var req = new PageFactory.Asset.AssetBackContentList.DtoReq();
            req.in_sn = in_sn;
            var pageModel = new PageFactory.Asset.AssetBackContentList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 3.转移记录
        /// <summary>
        /// 3.1资产转移提交表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MovePost(int id = 0,int asset_id= 0)
        {
            var req = new PageFactory.Asset.AssetMovePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetMovePost().Get(req);
            return View(pageModel);
        }
        [HttpPost]
        public JsonResult MovePost(string user_sn_after, string user_sn_before, string ac_date,string cause, List<l_move> l_move)
        {
            List<string> lSql = new List<string>();
            var info = new JsonResultAction(null);
                if (user_sn_before.IsNullOrEmpty()) throw new WeicodeException("原持有人不可为空！");
                if (user_sn_after.IsNullOrEmpty()) throw new WeicodeException("现持有人不可为空！");
                if (ac_date.IsNullOrEmpty()) throw new WeicodeException("时间不可为空！");
                var move_sn = "ZY" + UtilityStatic.CommonHelper.CreateUniqueSn();
                if (l_move is null) throw new WeicodeException("请选择资产！");
                foreach (var item in l_move)
                {
                    lSql.Add(new ModelDb.asset_move_item
                    {
                        user_sn = user_sn_after,
                        move_sn = move_sn,
                        asset_sn = item.asset_sn,
                        cause=cause
                    }.InsertTran());
                    lSql.Add(new ModelDb.asset
                    {
                        user_sn = user_sn_after,
                    }.UpdateTran($"asset_sn='{item.asset_sn}'"));
                }
                lSql.Add(new ModelDb.asset_move
                {
                    move_sn = move_sn,
                    create_time = DateTime.Now,
                    ac_date = ac_date.ToDate(),
                    cause = cause,
                    user_sn_after = user_sn_after,
                    user_sn_before = user_sn_before
                }.InsertTran());
                DoMySql.ExecuteSqlTran(lSql);
            return Json(info);
        }
        public class l_move
        {
            public string name { get; set; }
            public string user_sn { get; set; }
            public string spec { get; set; }
            public string brand { get; set; }
            public string company_id { get; set; }
            public string asset_sn { get; set; }
            public int category_id { get; set; }
        }
        /// <summary>
        /// 3.2资产转移记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveList()
        {
            var req = new PageFactory.Asset.AssetMoveList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetMoveList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 3.3资产转移记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MoveContentList(string move_sn)
        {
            var req = new PageFactory.Asset.AssetMoveContentList.DtoReq();
            req.move_sn = move_sn;
            var pageModel = new PageFactory.Asset.AssetMoveContentList().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 通用
        /// <summary>
        /// 资产选择页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Select(string user_sn,string status)
        {
            var req = new PageFactory.Asset.AssetSelect.DtoReq();
            req.user_sn = user_sn;
            req.status = status;
            var pageModel = new PageFactory.Asset.AssetSelect().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "is_deleted=0";
            return View(pageModel);
        }
        #endregion
    }
}