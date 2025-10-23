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
    public class OutController : BaseController
    {
        #region 1.出库单
        /// <summary>
        /// 1.1出库资产明细列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Order()
        {
            var req = new PageFactory.Asset.AssetOutList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetOutList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 1.2出库资产详情表
        /// </summary>
        /// <returns></returns>
        public ActionResult Order_Detaill(string out_sn)
        {
            var req = new PageFactory.Asset.AssetOutDetailList.DtoReq();
            req.out_sn = out_sn;
            var pageModel = new PageFactory.Asset.AssetOutDetailList().Get(req);
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.leftPartial[0].disabled = true;
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        #endregion
        #region 2.出库明细
        /// <summary>
        /// 2.1出库资产详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detaill()
        {
            var req = new PageFactory.Asset.AssetOutDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetOutDetailList().Get(req);
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        /// <summary>
        /// 3.1资产出库审核
        /// </summary>
        /// <returns></returns>

        #endregion
        #region 3.逾期未入
        /// <summary>
        /// 3.1逾期未入名单
        /// </summary>
        /// <returns></returns>
        public ActionResult OverdueList()
        {
            var req = new PageFactory.Asset.AssetOutDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetOutDetailList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"plan_time < '{DateTime.Now}' and status=0";
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listFilter.formItems[3].disabled = true;
            return View(pageModel);
        }
        #endregion
        #region 4.待出处理
        public ActionResult WaitDetaill()
        {
            var req = new PageFactory.Asset.AssetOutDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetOutDetailList().Get(req);
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.listData.attachFilterSql = "status=0";
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
        #endregion
        #region 5.逾期处理
        public ActionResult WaitOverdueList()
        {
            var req = new PageFactory.Asset.AssetOutDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetOutDetailList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"plan_time < '{DateTime.Now}' and status=0";
            pageModel.listFilter.formItems[3].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
        #endregion
        #region 通用
        /// <summary>
        /// 3.2批量出库审批
        /// </summary>
        /// <returns></returns>
        public ActionResult OutChecks(string ids = "")
        {
            var req = new PageFactory.Asset.AssetOutChecksPost.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.Asset.AssetOutChecksPost().Get(req);
            return View(pageModel);
        }

        [HttpPost]
        public JsonResult OutChecks(string cause,int status = 0, string ids = "")
        {
            var info = new JsonResultAction(null);
            List<string> lSql = new List<string>();
                lSql.Add(new ModelDb.asset_out_item { 
                    status=status.ToSByte(),
                    out_time = DateTime.Now,
                    cause =cause
                }.UpdateTran( $"id in ({ids})"));
                DoMySql.ExecuteSqlTran(lSql);
            return Json(info);
        }
        #endregion
    }
}