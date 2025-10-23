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
    public class InController : BaseController
    {
        #region 1.入库单
        /// <summary>
        /// 1.1入库资产明细记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Order()
        {
            var req = new PageFactory.Asset.AssetInList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetInList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 1.2入库资产明细表
        /// </summary>
        /// <returns></returns>
        public ActionResult Order_Detaill(string in_sn)
        {
            var req = new PageFactory.Asset.AssetDetailList.DtoReq();
            req.in_sn = in_sn;
            var pageModel = new PageFactory.Asset.AssetDetailList().Get(req);
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.leftPartial[0].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        #endregion
        #region 2.入库明细
        /// <summary>
        /// 2.31库资产明细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detaill()
        {
            var req = new PageFactory.Asset.AssetDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetDetailList().Get(req);
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            return View(pageModel);
        }
        #endregion
        #region 3.逾期未入
        /// <summary>
        /// 3.1逾期未入名单
        /// </summary>
        /// <returns></returns>
        public ActionResult OverdueList()
        {
            var req = new PageFactory.Asset.AssetDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetDetailList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"plan_time < '{DateTime.Now}'and status=0";
            pageModel.listDisplay.listBatchItems[0].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listFilter.formItems[3].disabled = true;
            return View(pageModel);
        }
        #endregion
        #region 4.待入处理
        /// <summary>
        /// 2.31库资产明细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitDetaill()
        {
            var req = new PageFactory.Asset.AssetDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetDetailList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "status=0";
            pageModel.listDisplay.listItemsDisableds = "overdue_time";
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
        #endregion
        #region 5.逾期处理
        /// <summary>
        /// 3.1逾期未入名单
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitOverdueList()
        {
            var req = new PageFactory.Asset.AssetDetailList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetDetailList().Get(req); 
            pageModel.listDisplay.listData.attachFilterSql = $"plan_time < '{DateTime.Now}' and status=0";
            pageModel.listFilter.formItems[3].disabled = true;
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
        #endregion
        #region 通用
        /// <summary>
        /// 4.2 批量审核待入库资产的“实际入库”情况（库管）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cause"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Checks(string ids = "")
        {
            var req = new PageFactory.Asset.AssetInChecksPost.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.Asset.AssetInChecksPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}