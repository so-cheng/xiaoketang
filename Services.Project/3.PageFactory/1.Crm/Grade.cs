using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;


namespace Services.Project
{

    /// <summary>
    /// 等级管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 等级变更记录
        /// </summary>
        public class GradeLogList
        {
            #region DefaultView
            /// <summary>
            /// 获取页面数据模型
            /// </summary>
            /// <returns></returns>
            public ModelBasic.PageList Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PageList("pagelist");
                pageModel.listFilter = GetListFilter(req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.listDisplay = GetListDisplay(req);
                pageModel.listFilter.isExport = false;
                return pageModel;
            }
            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new ModelBasic.CtlListFilter();
                listFilter.formItems.Add(new ModelBasic.EmtHidden("n_grade_id")
                {
                    placeholder = "变更客户等级",
                    defaultValue = req.n_grade_id.ToString()
                });
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                {
                    placeholder = "时间范围",
                    defaultValue=DateTime.Today.ToString("yyyy-MM-dd")+" ~ "+ DateTime.Today.ToString("yyyy-MM-dd")

                });
                return listFilter;
            }
            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                return buttonGroup;
            }
            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 1.显示列
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                {
                    text = "用户昵称",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("o_grade")
                {
                    text = "旧等级",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("n_grade")
                {
                    text = "新等级",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                {
                    text = "变更原因备注",
                    width = "280",
                    minWidth = "280",
                });
                #endregion
                #region 2.批量操作列
                #endregion
                #region 3.操作列
                #endregion
                return listDisplay;
            }
            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                public int n_grade_id { get; set; }
            }
            #endregion
            #region ListData

            /// <summary>
            /// 资产表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = $"1=1";
                
                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <= '" + dateRange.date_range_e + "'";
                }
                if (req["o_grade_id"].ToInt() != 0) where += $" AND (o_grade_id ='{req["o_grade_id"]}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.crm_grade_log, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.ListData.Req
            {

            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.crm_grade_log
            {
                public string name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.p_crm_customer>($"crm_sn = '{this.crm_sn}'", false).nick;
                    }
                }
                public string o_grade
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.crm_grade>($"id = '{this.o_grade_id}'", false).name;
                    }
                }
                public string n_grade
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.crm_grade>($"id = '{this.n_grade_id}'", false).name;
                    }
                }
            }
            #endregion
            #region 异步请求处理

            #endregion
        }
    }
}