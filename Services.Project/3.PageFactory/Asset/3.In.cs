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
            /// 资产入库明细管理
            /// </summary>
            public class AssetInList
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
                    listFilter.formItems.Add(new EmtTimeSelect("create_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "选择时间",
                    });
                    listFilter.formItems.Add(new EmtSelect("op_type")
                    {
                        placeholder = "请选择操作类型",
                        width = "140",
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.asset_in.op_type_enum>()
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("in_sn")
                    {
                        text = "入库单号",
                        width = "280",
                        minWidth = "280",
                        fix = EmtModel.ListItem.Fixed.left
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("op_type_text")
                    {
                        text = "操作类型",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("num")
                    {
                        text = "入库数量",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("end_num")
                    {
                        text = "已入库数量",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "提交时间",
                        width = "180",
                        minWidth = "180",
                    });
                    #endregion
                    #region 2.批量操作列
                    #endregion
                    #region 3.操作列
                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        actionEvent = EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "查看详情",
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"Order_Detaill",
                            field_paras = "in_sn"
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
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
                }
                #endregion
                #region ListData
                public string FindOrganize(string id, string s)
                {
                    foreach (var item in DoMySql.FindList<ModelDb.asset_category>($"parent_id ='{id}'"))
                    {
                        s += item.id + ",";
                        if (!DoMySql.FindEntity<ModelDb.asset_category>($"parent_id ='{item.id}'", false).IsNullOrEmpty())
                        {
                            return FindOrganize(item.id.ToString(), s);
                        }
                    }
                    return s + id;
                }
                /// <summary>
                /// 资产入库表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["create_time"].ToString(), 0);
                    string where = $"1=1";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    if (!req["op_type"].ToNullableString().IsNullOrEmpty()) where += $" AND (op_type ='{req["op_type"]}')";
                    if ((!req["id"].ToNullableString().IsNullOrEmpty()) && (!req["id"].Equals("0"))) where += $" AND category_id in ({FindOrganize(req["id"].ToString(), "")})";
                    if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += " AND  create_time > '" + dateRange.date_range_s + "' AND create_time < '" + dateRange.date_range_e + "'";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_in, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    public string id { get; set; }
                    public string create_time { get; set; }
                    /// <summary>
                    /// 操作类型
                    /// </summary>
                    public string op_type { get; set; }
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_in
                {
                    public string op_type_text
                    {
                        get
                        {
                            return op_type.ToEnum<ModelDb.asset_in.op_type_enum>();
                        }
                    }
                    public string end_num
                    {
                        get
                        {
                            return DoMySql.FindList<ModelDb.asset_in_item>($"in_sn = '{this.in_sn}' and status='{ModelDb.asset_in_item.status_enum.入库成功.ToInt()}' ").Count.ToString();
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 资产入库明细页面
            /// </summary>
            public class AssetDetailList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("category_id")
                    {
                        placeholder = "资产类别",
                        defaultValue = req.category_id
                    });
                    listFilter.formItems.Add(new EmtHidden("in_sn")
                    {
                        placeholder = "入库编号",
                        defaultValue = req.in_sn
                    });
                    listFilter.formItems.Add(new EmtTimeSelect("create_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "选择时间",
                    });
                    listFilter.formItems.Add(new EmtSelect("status")
                    {
                        placeholder = "入库状态",
                        width = "100",
                        //defaultValue = ""
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.asset_in_item.status_enum>()
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("name")
                    {
                        text = "资产名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("asset_sn")
                    {
                        text = "资产编号",
                        width = "280",
                        minWidth = "280",
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
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "入库状态",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "持有人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("cause")
                    {
                        text = "事由",
                        width = "240",
                        minWidth = "240",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("overdue_time")
                    {
                        text = "逾期时间",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("plan_time")
                    {
                        text = "约定入库时间",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("in_time")
                    {
                        text = "实际入库时间",
                        width = "180",
                        minWidth = "180",
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
                            text = "批量入库",
                            mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer=new EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = $"Checks"
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
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
                    public string category_id { get; set; }
                    public string parent_id { get; set; }
                }
                #endregion
                #region ListData

                /// <summary>
                /// 资产入库明细表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"1=1";
                    var dateRange = UtilityStatic.CommonHelper.DateTimeRangeFormat(req["create_time"].ToString(), 0);
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += " AND  create_time > '" + dateRange.date_range_s + "' AND create_time < '" + dateRange.date_range_e + "'";
                    if (!req["status"].ToNullableString().IsNullOrEmpty()) where += $" AND (status ='{req["status"]}')";
                    if (!req["in_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (in_sn ='{req["in_sn"]}')";
                    var list = new DomainBasic.DataQueryApp().FindAllChildsByParentId<ModelDb.asset_category>(req["category_id"].ToInt());
                    string ids = "";
                    foreach (var item in list)
                    {
                        ids += item.id + ",";
                    }
                    ids = ids.Substring(0, ids.Length - 1);
                    if (req["category_id"].ToInt() != 0)
                    {
                        if (!req["category_id"].ToNullableString().IsNullOrEmpty()) where += $" AND asset_sn in (select asset_sn from asset where category_id in ('{ids}'))";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " ORDER BY id"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_in_item, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    public string create_time { get; set; }
                    /// <summary>
                    /// 入库状态
                    /// </summary>
                    public string status { get; set; }
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
                    public string id { get; set; }
                    public string category_id { get; set; }
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_in_item
                {
                    public ModelDb.asset asset
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{this.asset_sn}'", false);
                        }
                    }
                    public string brand
                    {
                        get
                        {
                            return asset.brand;
                        }
                    }
                    public string spec
                    {
                        get
                        {

                            return asset.spec;
                        }
                    }
                    public string name
                    {
                        get
                        {
                            return asset.name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.asset_in_item.status_enum>();
                        }
                    }
                    public string user_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false).name;
                        }
                    }
                    public string overdue_time
                    {
                        get
                        {
                            return UtilityStatic.DateHelper.DateDiff(plan_time, DateTime.Now);
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 批量审核待入库资产的“实际入库”情况（库管）
            /// </summary>
            public class AssetInChecksPost
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
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = ChecksAction,

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
                    formDisplay.formItems.Add(new EmtSelect("status")
                    {
                        title = "入库状态",
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.asset_in_item.status_enum>(),
                        defaultValue = "1",
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "失败事由",
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
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 批量审核待入库资产的“实际入库”情况（库管）
                /// </summary>
                /// <param name="req">入库明细记录ids, 审核结果，拒绝原因</param>
                /// <returns></returns>
                public JsonResultAction ChecksAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    //1.更新入库明细记录状态
                    lSql.Add(new ModelDb.asset_in_item
                    {
                        status = dtoReqData.status,
                        in_time = DateTime.Now,
                        cause = dtoReqData.cause
                    }.UpdateTran($"id in ({dtoReqData.ids})"));

                    switch (dtoReqData.status)
                    {
                        case 1:
                            //2.更新关联资产的状态
                            lSql.Add(new ModelDb.asset
                            {
                                status = ModelDb.asset.status_enum.空闲.ToSByte()
                            }.UpdateTran($"asset_sn IN (SELECT asset_sn FROM asset_in_item WHERE id IN ({dtoReqData.ids}))"));
                            break;
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.asset_in_item
                {
                    /// <summary>
                    /// 入库明细记录ids
                    /// </summary>
                    public string ids { get; set; }
                }
                #endregion
            }
        }
    }

}