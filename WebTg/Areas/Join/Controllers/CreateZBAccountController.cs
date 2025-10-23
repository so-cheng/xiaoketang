using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.Join.Controllers
{
    /// <summary>
    /// 萌新主播
    /// </summary>
    public class CreateZBAccountController : BaseLoginController
    {
        #region 显示厅管申请列表
        public ActionResult TGApplicationList(PageFactory.Join.TGApplyZbList.DtoReq req)//completeStatus 0:"未完成"; 1:"已完成", approve_status等待外宣补人=2;已完成=3
        {
            var pageModel = new PageFactory.Join.TGApplyZbList().Get(req);
            pageModel.buttonGroup.buttonItems[0].disabled = true;
            pageModel.listDisplay.listData.attachFilterSql = $"(status >= {ModelDb.p_join_need.status_enum.等待公会审批.ToInt()})";
            //pageModel.listDisplay.listOperateItems[0].disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 创建主播页面
        public ActionResult CreateZbPage()
        {
            var req = new PageFactory.Join.CreateZBPage.DtoReq();
            var pageModel = new PageFactory.Join.CreateZBPage().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 主播名单
        public ActionResult ZbList(int id = 0)
        {
            var req = new PageFactory.Join.ZbList.DtoReq();
            req.tg_need_id = id;
            var pageModel = new PageFactory.Join.ZbList().Get(req);
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "BatchPost").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 已流失主播名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult QuitedZbList(PageFactory.Join.QuitedZbList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.QuitedZbList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"zb_level='D'";
            return View(pageModel);
        }

        /// <summary>
        /// 退回主播
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public ActionResult BackPost(PageFactory.Join.ZbBackPost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.ZbBackPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 补人表单详情
        public ActionResult ZbDetails(int id = 0)
        {
            var req = new PageFactory.Join.WX_ZG_ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.WX_ZG_ZbDetails().Get(req);

            return View(pageModel);
        }
        #endregion

        #region 补人:选择萌新表单
        public ActionResult MX_ChooseZbPost(int id = 0, int dangwei = 0, string tg_user_sn = "")
        {
            var req = new PageFactory.Join.WX_ChooseZbList.DtoReq();
            req.tg_dangwei = dangwei;
            req.tg_need_id = id;
            req.tg_user_sn = tg_user_sn;
            var pageModel = new PageFactory.Join.WX_ChooseZbList().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 总数据
        public ActionResult ApproveApplicationList()
        {
            var req = new PageFactory.Join.ApproveApplyZb.DtoReq();
            var pageModel = new PageFactory.Join.ApproveApplyZb().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "status").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "create_time").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "tg_sex").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;

            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    url = $"Detail",
                    field_paras = "id"
                },
                text = "详情"
            });
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    url = $"QuitList",
                    field_paras = "tg_user_sn"
                },
                text = "流失名单",
            });
            return View(pageModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id = 0)
        {
            var req = new PageFactory.Join.ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.ZbDetails().Get(req);
            return View(pageModel);
        }

        public ActionResult QuitList(PageFactory.Join.YyQuitList.DtoReq req)
        {
            var pageModel = new PageFactory.Join.YyQuitList().Get(req);
            return View(pageModel);
        }

        #endregion

        #region 已补人员名单
        public ActionResult FinishList(int id = 0)
        {
            var req = new PageFactory.Join.ZbDetails.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.Join.ZbDetails().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 未分配名单
        public ActionResult Wait(PageFactory.Join.MxList.DtoReq req)
        {
            //加急的排序排前面
            req.orderby = $@"ORDER BY is_fast DESC,create_time DESC";
            var pageModel = new PageFactory.Join.MxList().Get(req);
            try
            {
                pageModel.listDisplay.listItems.Where(x => x.text == "主播分级").FirstOrDefault().mode = ModelBasic.EmtModel.ListItem.Mode.文本;
            }
            catch
            {

            }

            pageModel.listDisplay.listData.attachFilterSql = $"(tg_user_sn = '' and mx_sn!='' and zb_level != 'C' and zb_level != 'D' )";
            pageModel.listDisplay.listItems.Where(x => x.field == "zb_sex").FirstOrDefault().index = 201;
            pageModel.listDisplay.listItems.Where(x => x.field == "sessions_name").FirstOrDefault().index = 202;
            pageModel.listDisplay.listItems.Where(x => x.field == "age").FirstOrDefault().index = 203;
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }

        /// <summary>
        /// 所有未分
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitOverview()
        {
            return View();
        }

        /// <summary>
        /// 单日收到
        /// </summary>
        /// <returns></returns>
        public ActionResult DayGetview(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 单日已分
        /// </summary>
        /// <returns></returns>
        public ActionResult DaySetview(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 当日示分
        /// </summary>
        /// <returns></returns>
        public ActionResult DayNosetview(string c_date)
        {
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;
            ViewBag.c_date_early = c_date.ToDate().AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.c_date_late = c_date.ToDate().AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        #endregion

        #region 免审白名单
        public ActionResult WhiteList()
        {
            var req = new PageFactory.Join.WhiteList.DtoReq();
            var pageModel = new PageFactory.Join.WhiteList().Get(req);
            return View(pageModel);
        }

        public ActionResult AddWhite(PageFactory.Join.WhitePost.DtoReq req)
        {
            var pageModel = new PageFactory.Join.WhitePost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}