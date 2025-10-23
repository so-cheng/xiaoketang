using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    /// <summary>
    /// 萌新分配
    /// </summary>
    public class ShareController : BaseLoginController
    {
        #region 分享二维码
        public ActionResult Page()
        {
            return View();
        }
        #endregion

        #region 萌新记录
        public ActionResult List(PageFactory.Join.MxList.DtoReq req)
        {
            req.orderby = " order by create_time desc";
            var pageModel = new PageFactory.Join.MxList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "isQuited").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "isShared").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "is_qun").FirstOrDefault().disabled = false;

            pageModel.listDisplay.isHideOperate = true;

            pageModel.buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
            {
                text = "已流失",
                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                {
                    url = $"ZBQuited"
                }
            });
            pageModel.buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
            {
                text = "已分配",
                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                {
                    url = $"ZBShared"
                }
            });
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 培训数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult PeixunList(PageFactory.Xianxiabiao.PeixunList.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunList().Get(req);
            pageModel.buttonGroup.buttonItems.Clear();
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.Xianxiabiao.PeixunPost.DtoReq req)
        {
            var pageModel = new PageFactory.Xianxiabiao.PeixunPost().Get(req);
            return View(pageModel);
        }
        #endregion


        #region 拉群
        /// <summary>
        /// 等待拉群
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult WaitQun(PageFactory.Join.WaitQun.DtoReq req)
        {
            var pageModel = new PageFactory.Join.WaitQun().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "group").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $" tg_user_sn != '' and is_qun = 0 and zb_level != 'C' and zb_level != 'D'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 已拉群
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult IsQun(PageFactory.Join.WaitQun.DtoReq req)
        {
            var pageModel = new PageFactory.Join.WaitQun().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "group").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Clear();

            pageModel.listDisplay.listData.attachFilterSql = $" is_qun = '1'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        #endregion

        #region 入库
        public ActionResult WaitPut(PageFactory.Join.WaitPut.DtoReq req)
        {
            var pageModel = new PageFactory.Join.WaitPut().Get(req);
            pageModel.listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("supplement_time") 
            { 
                index=1,
                text="分配时间",
                width="200",
            });
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "group").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $" tg_user_sn != '' and is_qun = 0 and zb_level != 'C' and zb_level != 'D' and put_time is null";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 标记错误
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult Mark(PageFactory.Join.MarkPost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.MarkPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 暂未分配
        public ActionResult NoShare(PageFactory.Join.MxList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.MxList().Get(req);

            pageModel.listDisplay.isHideOperate = true;

            pageModel.listDisplay.listData.attachFilterSql = $"(mx_sn != '' and tg_user_sn = '' and (zb_level='A' or zb_level='B'))";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 用户分级
        public ActionResult WaitShare(PageFactory.Join.MxList.DtoReq req)
        {
            req.orderby = "order by create_time desc";
            var pageModel = new PageFactory.Join.MxList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.isHideOperate = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "level").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"(tg_user_sn = '' AND zb_level = '-')";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        public ActionResult FastLevel(PageFactory.Join.FastLevel.DtoReq req)
        {
            var pageModel = new PageFactory.Join.FastLevel().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 主播分配情况
        public ActionResult ZBQuited(PageFactory.Join.MxList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.MxList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            pageModel.listDisplay.listData.attachFilterSql = $"( (zb_level = 'C' or zb_level = 'D'))";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        public ActionResult ZBShared(PageFactory.Join.MxList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.MxList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            pageModel.listDisplay.listData.attachFilterSql = $"( (zb_level != 'C' or zb_level != 'D') and zb_level != '-' and tg_user_sn !='')";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion
    }
}