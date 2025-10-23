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
    /// 资产盘点管理
    /// </summary>
    public class StockController : BaseController
    {
        #region 1.资产盘点
        /// <summary>
        /// 盘点资产明细表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var req = new PageFactory.Asset.AssetStockList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetStockList().Get(req);
            return View(pageModel);
        }
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.Asset.AssetStockPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetStockPost().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 2.资产单进展
        public ActionResult Detaill(string stock_sn)
        {
            var req = new PageFactory.Asset.AssetStockContentList.DtoReq();
            req.stock_sn = stock_sn;
            var pageModel = new PageFactory.Asset.AssetStockContentList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
     
        /// <summary>
        /// 4.2批量入库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cause"></param>
        /// <returns></returns>_sn)
        [HttpGet]
        public ActionResult StockChecks(string ids)
        {
            var req = new PageFactory.Asset.AssetStockChecksPost.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.Asset.AssetStockChecksPost().Get(req);
            return View(pageModel);
        }
        [HttpPost]
        public JsonResult StockChecks(string user_sn,string ids)
        {
            var info = new JsonResultAction(null);
            List<string> lSql = new List<string>();
                if(user_sn.IsNullOrEmpty()) 
                {
                    lSql.Add(new ModelDb.asset_stock_item
                    {
                        status = 0,
                        user_sn = user_sn,
                        p_status = 1
                    }.UpdateTran($"id in ({ids})"));
                }
                else
                {
                    lSql.Add(new ModelDb.asset_stock_item
                    {
                        status = 1,
                        user_sn = user_sn,
                        p_status = 1
                    }.UpdateTran($"id in ({ids})"));
                }
                DoMySql.ExecuteSqlTran(lSql);
            return Json(info);
        }
        public JsonResult Submit(string stock_sn)
        {
            var info = new JsonResultAction(null);
            List<string> lSql = new List<string>();
            var asset_stock_item = DoMySql.FindList<ModelDb.asset_stock_item>($"stock_sn='{stock_sn}'");
                foreach (var item in asset_stock_item)
                {
                    lSql.Add(new ModelDb.asset
                    {
                        status = item.status,
                        user_sn = item.user_sn,
                    }.UpdateTran($"asset_sn='{item.asset_sn}'"));
                lSql.Add(new ModelDb.asset_stock
                {
                    real_e_time = DateTime.Now,
                    status = 1,
                }.UpdateTran($"stock_sn='{item.stock_sn}'")) ;
                if (item.p_status == 0)
                        lSql.Add(new ModelDb.sys_biz_log
                        {
                            modular_function = "盘点",
                            memo = "盘点结果未盘"
                        }.InsertTran());
                    else if (item.status==0)
                    {
                        lSql.Add(new ModelDb.sys_biz_log
                        {
                            modular_function = "盘点",
                            relation_sn = item.asset_sn,
                            memo = "状态变为空闲" 
                        }.InsertTran());
                    }
                    else
                    {
                        lSql.Add(new ModelDb.sys_biz_log
                        {
                            modular_function = "盘点",
                            relation_sn = item.asset_sn,
                            memo = "将使用人变更为" + item.user_sn
                        }.InsertTran());
                    }                 
                }
                lSql.Add(new ModelDb.asset_stock
                {
                    status = 1,
                }.UpdateTran($"stock_sn='{stock_sn}'"));
                
                DoMySql.ExecuteSqlTran(lSql);
            return Json(info);
        }
        #endregion
    }
}