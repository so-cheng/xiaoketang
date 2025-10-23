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
    public class DataController : BaseController
    {
        #region 1.资产列表
        /// <summary>
        /// 1.1资产列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var req = new PageFactory.Asset.AssetList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "is_deleted=0";
            pageModel.listDisplay.listBatchItems[0].buttonItems[0].disabled = false;
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }

        /// <summary>
        /// 1.2登记/编辑资产页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Post(int id = 0)
        {
            var req = new PageFactory.Asset.AssetPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetPost().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 1.3资产操作详情表
        /// </summary>
        /// <param name="out_sn"></param>
        /// <returns></returns>
        public ActionResult ContentList(string asset_sn)
        {
            var req = new PageFactory.Asset.AssetContentList.DtoReq();
            req.asset_sn = asset_sn;
            var pageModel = new PageFactory.Asset.AssetContentList().Get(req);
            return View(pageModel);
        }
        public ActionResult Change(int id = 0)
        {
            var req = new PageFactory.Asset.AssetPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Asset.AssetPost().Get(req);
            pageModel.formDisplay.formItems[9].isDisplay = false;
            return View(pageModel);
        }
        /// <summary>
        /// 通过excel表单导入资产信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Inserts()
        {
            var req = new PageFactory.Asset.AssetInserts.DtoReq();
            var pageModel = new PageFactory.Asset.AssetInserts().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 2.回收站
        /// <summary>
        /// 回收资产
        /// </summary>
        /// <returns></returns>
        public ActionResult Recycle()
        {
            var req = new PageFactory.Asset.AssetList.DtoReq();
            var pageModel = new PageFactory.Asset.AssetList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = "is_deleted=1";
            pageModel.buttonGroup.disabled = true;
            pageModel.listDisplay.listOperateItems[0].disabled = true;
            pageModel.listDisplay.listBatchItems[0].buttonItems[1].disabled = false;
            pageModel.listDisplay.isOpenCheckBox = true;
            return View(pageModel);
        }
        #endregion
        #region 3.资产分类
        /// <summary>
        /// 2.1资产分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CategoryList(int parent_id = 0)
        {
            var req = new PageFactory.Asset.AssetCategoryList.DtoReq();
            req.parent_id = parent_id;
            var pageModel = new PageFactory.Asset.AssetCategoryList().Get(req);
            return View(pageModel);
        }
        /// <summary>
        /// 2.2新增/编辑资产信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CategoryPost(int id = 0, int parent_id = 0)
        {
            var req = new PageFactory.Asset.AssetCategoryPost.DtoReq();
            req.id = id;
            req.parent_id = parent_id;
            var pageModel = new PageFactory.Asset.AssetCategoryPost().Get(req);
            return View(pageModel);
        }
        #endregion
        #region 通用
        #endregion
    }
}