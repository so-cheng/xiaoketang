using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Domain;
using System.Data;
using static WeiCode.Models.ModelBasic;
namespace Services.Project
{

    public partial class PageFactory
    {

        /// <summary>
        /// 资产管理模块
        /// </summary>
        public partial class Asset
        {
            /// <summary>
            /// 资产盘点
            /// </summary>
            public class AssetStockList
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {

                    var pageModel = new PageList("pagelist");

                    pageModel.listFilter = GetListFilter(req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.listFilter.isExport = true;
                    return pageModel;
                }
                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    return listFilter;


                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "新增盘点",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Stock/Post",
                        }
                    });
                    return buttonGroup;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("name")
                    {
                        text = "盘点单名称",
                        width = "180",
                        minWidth = "180",
                        fix = EmtModel.ListItem.Fixed.left
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "盘点状态",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("stock_end")
                    {
                        mode = EmtModel.ListItem.Mode.进度条,
                        text = "盘点完成情况",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("plan_time")
                    {
                        text = "计划盘点时间",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("real_e_time")
                    {
                        text = "实践完成时间",
                        width = "180",
                        minWidth = "180",
                    });
                    #endregion
                    #region 2.批量操作列
                    #endregion
                    #region 3.操作列按钮
                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        actionEvent = EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "进入盘点",
                        title = "盘点单进度",
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"/Asset/Stock/Detaill",
                            field_paras = "stock_sn",
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith()
                        {
                            field = "status",
                            value = "1,2"
                        }
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                }
                #endregion
                #region ListData
                /// <summary>
                /// 资产盘点表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"1=1";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_stock, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public int tenant_id { get; set; }
                    public string create_time { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_stock
                {
                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.asset_stock.status_enum>();
                        }
                    }
                    public string stock_end
                    {
                        get
                        {
                            var asset_stock_item = DoMySql.FindList<ModelDb.asset_stock_item>($"p_status!='{ModelDb.asset_stock_item.p_status_enum.待盘点.ToInt()}' and stock_sn='{this.stock_sn}'").Count.ToFloat();
                            var asset_stock_item_e = DoMySql.FindList<ModelDb.asset_stock_item>($"stock_sn='{this.stock_sn}'").Count.ToInt();
                            return ((asset_stock_item / asset_stock_item_e * 100) + "%");
                        }
                    }
                    public string plan_time
                    {
                        get
                        {
                            return plan_s_time + "-" + plan_s_time;
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 新增资产盘点信息
            /// </summary>
            public class AssetStockPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new PagePost.EventCsAction
                    {
                        func = PostAction
                    };

                    return pageModel;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var asset_stock = DoMySql.FindEntityById<ModelDb.asset_stock>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new EmtInput("name")
                    {
                        title = "*盘点单名称",
                        defaultValue = asset_stock.name,
                        colLength = 6
                    });
                    var opinion = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>())
                    {
                        opinion.Add(new ModelDoBasic.Option
                        {
                            text = item.name,
                            value = item.user_sn
                        });
                    }
                    formDisplay.formItems.Add(new EmtTimeSelect("plan_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        title = "*计划盘点时间",
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtFieldset("fieldset_ramge")
                    {
                        title = "盘点范围"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("company_ids")
                    {
                        title = "所属公司",
                        options = new DomainBasic.DictionaryApp().GetListForOption("公司名称"),

                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("user_sns")
                    {
                        options = DoMySql.FindListBySql<ModelDoBasic.Option>($"select user_sn as value,name as text from user_base"),
                        title = "使用人",
                        placeholder = "选择使用人",
                    });

                    formDisplay.formItems.Add(new EmtTimeSelect("in_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        title = "入库时间",
                        colLength = 6
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 附加额外参数
                    /// </summary>
                    public int id { get; set; }

                    public int tenant_id { get; set; }
                    /// <summary>
                    /// 所属公司
                    /// </summary>
                    public string company_ids { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sns { get; set; }
                    /// <summary>
                    /// 计划入库时间
                    /// </summary>
                    public string plan_time { get; set; }
                    /// <summary>
                    /// 入库时间
                    /// </summary>
                    public string in_time { get; set; }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 新建资产盘点
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    var dateRangeTime = UtilityStatic.CommonHelper.DateRangeFormat(dtoReqData.in_time, 0);
                    var stock_sn = "PD" + UtilityStatic.CommonHelper.CreateUniqueSn();
                    string where = $"1=1";
                    if (!dtoReqData.in_time.IsNullOrEmpty()) where += $" AND  create_time >= '{dateRangeTime.date_range_s}' AND create_time <= '{dateRangeTime.date_range_e}'";
                    if (!dtoReqData.user_sns.IsNullOrEmpty()) where += $" AND  user_sn in ({dtoReqData.user_sns.Split(',').ToJoin()})";
                    if (!dtoReqData.company_ids.IsNullOrEmpty()) where += $" and company_id in ({dtoReqData.company_ids.Split(',').ToJoin()})";
                    var items = DoMySql.FindList<ModelDb.asset>($"{where}");
                    if (dtoReqData.name.IsNullOrEmpty()) throw new WeicodeException("盘点单名称不可为空！");
                    if (dtoReqData.plan_time.IsNullOrEmpty()) throw new WeicodeException("计划盘点时间不可为空！");
                    if (where.IsNullOrEmpty()) throw new WeicodeException("没有找到资产！");
                    foreach (var item in items)
                    {
                        lSql.Add(new ModelDb.asset_stock_item
                        {
                            asset_sn = item.asset_sn,
                            status = item.status,
                            user_sn = item.user_sn,
                            p_status = ModelDb.asset_stock_item.p_status_enum.待盘点.ToSByte(),
                            stock_sn = stock_sn,
                            company_id = item.company_id
                        }.InsertTran());
                    }
                    dateRangeTime = UtilityStatic.CommonHelper.DateRangeFormat(dtoReqData.plan_time, 0);
                    lSql.Add(new ModelDb.asset_stock
                    {
                        plan_s_time = dateRangeTime.date_range_s.ToDateTime(),
                        plan_e_time = dateRangeTime.date_range_e.ToDateTime(),
                        name = dtoReqData.name,
                        stock_sn = stock_sn,
                    }.InsertTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.asset_stock_item
                {
                    public string in_time { get; set; }
                    public string user_sns { get; set; }
                    public string company_ids { get; set; }
                    public string name { get; set; }
                    public string plan_time { get; set; }
                }
                #endregion
            }

            public class AssetStockContentList
            {
                #region DefaultView
                /// <summary>
                /// 资产派发详细页
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");

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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new EmtHidden("stock_sn")
                    {
                        placeholder = "",
                        defaultValue = req.stock_sn
                    });
                    return listFilter;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "提交盘点",
                        mode = EmtModel.ButtonItem.Mode.请求处理_Ajax调用指定URL,
                        eventAjaxUrl = new EmtModel.ButtonItem.EventAjaxUrl
                        {
                            url = "/Asset/Stock/Submit?stock_sn=" + req.stock_sn,
                        }

                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("asset_sn")
                    {
                        text = "资产编号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("name")
                    {
                        text = "资产名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("spec")
                    {
                        text = "规格型号",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("brand")
                    {
                        text = "品牌",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("p_status_text")
                    {
                        text = "盘点结果",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "资产状态",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "240",
                        minWidth = "240",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "持有人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("end_time")
                    {
                        text = "盘点时间",
                        width = "280",
                        minWidth = "280",
                    });
                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<EmtModel.ButtonItem>
                    {
                        new EmtModel.ButtonItem("")
                        {
                            text = "批量盘点",
                            mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer= new EmtModel.ButtonItem.EventOpenLayer
                            {
                                  url = $"StockChecks",
                             }
                        }
                    }
                    });
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
                    /// <summary>
                    /// 盘点单编号
                    /// </summary>
                    public string stock_sn { get; set; }
                }
                #endregion

                #region ListData
                /// <summary>
                /// 资产盘点明细表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"stock_sn = '{req["stock_sn"]}'";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_stock_item, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 盘点编号
                    /// </summary>
                    public string stock_sn { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_stock_item
                {

                    public ModelDb.user_base user_base = null;
                    public ModelDb.asset asset
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{this.asset_sn}'", false);
                        }
                    }

                    public string user_name
                    {
                        get
                        {
                            user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false);
                            return user_base.name;
                        }
                    }
                    public string p_status_text
                    {
                        get
                        {
                            return p_status.ToEnum<ModelDb.asset_stock_item.p_status_enum>();
                        }
                    }

                    public string company_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("公司名称", company_id);
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.asset_stock_item.status_enum>();
                        }
                    }
                    public string name
                    {
                        get
                        {
                            return asset.name;
                        }
                    }
                    public string spec
                    {
                        get
                        {
                            return asset.spec;
                        }
                    }
                    public string brand
                    {
                        get
                        {
                            return asset.brand;
                        }
                    }
                }
                #endregion
            }
            public class AssetStockChecksPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口
                    };

                    return pageModel;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new EmtSelect("result")
                    {
                        title = "盘点结果",
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.asset_stock_item.p_status_enum>(),
                        colLength = 6
                    });
                    var opinion = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>())
                    {
                        opinion.Add(new ModelDoBasic.Option
                        {
                            text = item.name,
                            value = item.user_sn
                        });
                    }
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("user_sn")
                    {
                        options = DoMySql.FindListBySql<ModelDoBasic.Option>($"select user_sn as value,name as text from user_base"),
                        title = "使用人",
                        placeholder = "选择使用人",
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "盘点备注",
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 附加额外参数
                    /// </summary>
                    public int id { get; set; }
                    public string ids { get; set; }
                    /// <summary>
                    /// 盘点结果
                    /// </summary>
                    public string p_status { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                }
                #endregion
                #region 异步请求处理
                #endregion
            }
        }
    }
}