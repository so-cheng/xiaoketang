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
    public partial class PageFactory
    {
        /// <summary>
        /// 资产管理模块
        /// </summary>
        public partial class Asset
        {


            /// <summary>
            /// 资产明细列表
            /// </summary>
            public class AssetList
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
                    pageModel.listFilter.isExport = true;
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("category_id")
                    {
                        placeholder = "资产类别",
                        defaultValue = req.category_id
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        placeholder = "资产名称",
                        defaultValue = ""
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "选择状态",
                        width = "100",
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.asset.status_enum>(),
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "登记(单个)资产",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Data/Post",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "导入(多)个资产",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Data/Inserts",
                        }
                    });
                    return buttonGroup;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("category")
                    {
                        text = "资产类别",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "资产名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("asset_sn")
                    {
                        text = "资产编号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("spec")
                    {
                        text = "规格型号",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("brand")
                    {
                        text = "品牌",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("price")
                    {
                        text = "资产价值",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("cj_name")
                    {
                        text = "采购人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "使用人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("on_date")
                    {
                        text = "登记日期",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "资产状态",
                        width = "140",
                        minWidth = "140",
                    });
                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "批量操作",

                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量删除",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = DeletesAction,
                             },
                            disabled = true
                        },
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量恢复",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = DelAction,
                             },
                            disabled = true
                        }
                    }
                    });
                    #endregion
                    #region 3.操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "编辑",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/Asset/Data/Change",
                            field_paras = "id"
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "查看详情",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/Asset/Data/ContentList",
                            field_paras = "asset_sn"
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
                    public string id { get; set; }

                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    /// <summary>
                    /// 资产编号
                    /// </summary>
                    public string asset_sn { get; set; }
                    public string parent_id { get; set; }
                    public string category_id { get; set; } = "0";
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
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    if (!req["status"].ToNullableString().IsNullOrEmpty()) where += $" AND (status ='{req["status"]}')";
                    var list = new DomainBasic.DataQueryApp().FindAllChildsByParentId<ModelDb.asset_category>(req["category_id"].ToInt());
                    string ids = "";

                    foreach (var item in list)
                    {
                        ids += item.id + ",";
                    }
                    ids = ids.Substring(0, ids.Length - 1);
                    if (req["category_id"].ToInt() != 0)
                    {
                        if (!req["category_id"].ToNullableString().IsNullOrEmpty()) where += $" AND (category_id in ('{ids}'))";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.asset, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public string id { get; set; }
                    public string status { get; set; }
                    public string parent_id { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset
                {

                    public string cj_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.cj_user_sn}'", false).name;
                        }
                    }
                    public string user_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false).name;
                        }
                    }

                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.asset.status_enum>();
                        }
                    }
                  
                    public string category
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset_category>($"id = '{this.category_id}'", false).name;
                        }
                    }
                }
                #endregion
                #region 恢复操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var asset = dtoReqData.ToModel<ModelDb.asset>();
                    asset.is_deleted = 0;
                    lSql.Add(asset.UpdateTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                #endregion
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var asset = new ModelDb.asset();
                    foreach (var item in DoMySql.FindList<ModelDb.asset>($"id in ('{dtoReqData.ids}')"))
                    {
                        if (item.status != ModelDb.asset.status_enum.空闲.ToInt()) throw new WeicodeException("只有空闲的资产可以删除，请重新选择！");

                    }
                    asset.is_deleted = 1;
                    lSql.Add(asset.UpdateTran($"id in ('{dtoReqData.ids}')"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.asset
                {
                    public string ids { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 新增资产信息
            /// </summary>
            public class AssetPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
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
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var asset = DoMySql.FindEntityById<ModelDb.asset>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("")
                    {
                        Content = @"<div class=""layui-body"" style=""width:100%"" >
                                        <ul class=""layui-body-title"">
                                            <p >用于现存的资产直接创建的情况，同时创建入库单，如果现存资产有使用人，当前资产则为在用状态</p>
                                        </ul>
                                    </div>"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    var opinion = new List<ModelDoBasic.Option>();
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.Dtree("category_id")
                    {
                        bindData = new DomainBasic.DataQueryApp().GetTree(0, "asset_category", null, $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id}"),
                        title = "*资产类别",
                        defaultValue = asset.category_id.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "*资产名称",
                        defaultValue = asset.name,
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("spec")
                    {
                        title = "规格型号",
                        defaultValue = asset.spec,
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("asset_sn")
                    {
                        title = "*资产编号",
                        defaultValue = "ZC" + UtilityStatic.CommonHelper.CreateUniqueSn(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("brand")
                    {
                        title = "品牌",
                        defaultValue = asset.brand,
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("cj_user_sn")
                    {
                        options = DoMySql.FindListBySql<ModelDoBasic.Option>($"select user_sn as value,name as text from user_base"),
                        title = "采购人",
                        placeholder = "选择采购人",
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("status")
                    {
                        title = "*资产状态",
                        options = new List<ModelDoBasic.Option>
                    {
                         new ModelDoBasic.Option
                         {
                             value ="0",
                             text = "已有资产-空闲在库"
                         },
                         new ModelDoBasic.Option
                         {
                             value="1" ,
                             text = "已有资产-正在使用中",
                             displayNames = "user_sn"
                         },
                         new ModelDoBasic.Option
                         {
                             value= "2",
                             text ="新采购-待入库",
                             displayNames = "plan_time"
                         },
                    },
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("on_date")
                    {
                        title = "*登记时间",
                        defaultValue = asset.on_date.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("plan_time")
                    {
                        title = "*约定入库时间",
                        colLength = 6,
                        isDisplay = false
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("price")
                    {
                        title = "资产价值",
                        defaultValue = asset.price.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("user_sn")
                    {
                        options = DoMySql.FindListBySql<ModelDoBasic.Option>($"select user_sn as value,name as text from user_base"),
                        title = "*使用人",
                        placeholder = "选择使用人",
                        isDisplay = false
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("num")
                    {
                        title = "登记数量",
                        defaultValue = "1",
                        colLength = 6
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public int id { get; set; }
                    public string plan_time { get; set; }
                    public string asset_sn { get; set; }
                }
                #endregion
                #region 新建资产
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    var asset = dtoReqData.ToModel<ModelDb.asset>();
                    var getdata = new DomainBasic.TenantApp().GetInfo().id;
                    var in_sn = "RK" + UtilityStatic.CommonHelper.CreateUniqueSn();
                    int num = dtoReqData.num;
                    if (asset.name.IsNullOrEmpty()) throw new WeicodeException("资产名称不可为空！");
                    if (asset.asset_sn.IsNullOrEmpty()) throw new WeicodeException("资产编号不可为空！");
                    if (asset.category_id.ToString().IsNullOrEmpty()) throw new WeicodeException("资产类别不可为空！");
                    if (dtoReqData.id < 1)
                    {
                        if (asset.status.ToString().IsNullOrEmpty()) throw new WeicodeException("资产状态不可为空！");
                    }
                    if (asset.status == ModelDb.asset.status_enum.在用.ToInt() && asset.user_sn.IsNullOrEmpty()) throw new WeicodeException("使用人不可为空！");
                    if (asset.status == ModelDb.asset.status_enum.待入.ToInt() && dtoReqData.plan_time == null) throw new WeicodeException("约定时间不可为空！");
                    if (asset.on_date == null) throw new WeicodeException("登记时间不可为空！");
                    if (dtoReqData.plan_time <= asset.on_date) throw new WeicodeException("登记时间不能晚于约定入库时间！");
                    if (asset.asset_sn.IsNullOrEmpty()) throw new WeicodeException("资产编号不可为空！");
                    if (asset.price.ToDouble() <= 0) throw new WeicodeException("资产价值必须为正数！");
                    if (dtoReqData.num.ToInt() <= 0) throw new WeicodeException("登记数量必须为正整数！");
                    if (asset.status == ModelDb.asset.status_enum.空闲.ToInt() && !asset.user_sn.IsNullOrEmpty()) throw new WeicodeException("使用人不可填写！");

                    for (int i = 0; i < num; i++)
                    {
                        string refresh = asset.asset_sn;

                        if ((!DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{dtoReqData.asset_sn}'", false).IsNullOrEmpty())) throw new WeicodeException("资产编号:" + dtoReqData.asset_sn + "已存在！");
                        if (num > 1)
                        {

                            asset.asset_sn += "-" + i;
                        }
                        asset.tenant_id = getdata;
                        lSql.Add(asset.InsertOrUpdateTran());
                        if (asset.status == ModelDb.asset.status_enum.待入.ToInt())
                        {
                            lSql.Add(new ModelDb.asset_in_item
                            {
                                asset_sn = asset.asset_sn,
                                plan_time = dtoReqData.plan_time,
                                in_sn = in_sn,
                            }.InsertTran());
                            asset.asset_sn = refresh;
                        }
                    }
                    if (asset.status == ModelDb.asset.status_enum.待入.ToInt())
                    {
                        lSql.Add(new ModelDb.asset_in
                        {
                            num = num,
                            in_sn = in_sn,
                            tenant_id = getdata,
                            op_type = 1
                        }.InsertTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.asset
                {
                    public DateTime? plan_time { get; set; }
                    public int num { get; set; }
                    public int id { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 资产派发详细列表
            /// </summary>
            public class AssetContentList
            {
                #region DefaultView
                /// <summary>
                /// 资产派发详细页
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("asset_sn")
                    {
                        defaultValue = req.asset_sn
                    });
                    return listFilter;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("modular_function")
                    {
                        text = "模块功能名称",
                        width = "140",
                        minWidth = "140",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("memo")
                    {
                        text = "操作日志信息",
                        width = "140",
                        minWidth = "140",
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
                    /// <summary>
                    /// 资产编号
                    /// </summary>
                    public string asset_sn { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// 日志表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"relation_sn = '{req["asset_sn"]}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.sys_biz_log, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string asset_sn { get; set; }
                    public string name { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.sys_biz_log
                {


                }
                #endregion
            }
            /// <summary>
            /// 导入资产信息
            /// </summary>
            public class AssetInserts
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    return pageModel;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtExcelRead("l_asset")
                    {
                        title = "批量导入资产",
                        colItems = new List<ModelBasic.EmtExcelRead.ColItem>
                    {
                        new ModelBasic.EmtExcelRead.ColItem("category_name")
                        {
                         title = "资产类别",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("name")
                        {
                         title = "资产名称",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("brand")
                        {
                         title = "品牌",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("spec")
                        {
                         title = "规格型号",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("price")
                        {
                         title = "资产价值",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("company_txt")
                        {
                         title = "所属公司",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("status_txt")
                        {
                         title = "资产状态",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_name")
                        {
                         title = "使用人",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("cj_user_name")
                        {
                         title = "采购人",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("on_date")
                        {
                         title = "登记日期",
                         edit = "text",
                        },
                     },
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 附加额外参数
                    /// </summary>
                    public FormData formData { get; set; } = new FormData();
                    public class FormData
                    {
                        public int id { get; set; }
                    }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 导入多个资产
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    int sort = 0;
                    foreach (var item in dtoReqData.l_asset)
                    {
                        sort++;
                        if (item.name.IsNullOrEmpty()) throw new WeicodeException("资产名称:" + item.name + "不存在;(第" + sort + "行)");
                        if (item.on_date.IsNullOrEmpty()) throw new WeicodeException("登记日期:" + item.on_date + "不存在;(第" + sort + "行)");
                        var company_id = new DomainBasic.DictionaryApp().GetValueFromKey("公司名称", item.company_txt);
                        if (company_id.IsNullOrEmpty()) throw new WeicodeException("公司名称:" + item.company_txt + "不存在;(第" + sort + "行)");
                        var asset_sn = "ZC" + UtilityStatic.CommonHelper.CreateUniqueSn();
                        var asset_category = DoMySql.FindEntity<ModelDb.asset_category>($"name = '{item.category_name}'", false);
                        if (asset_category.IsNullOrEmpty()) throw new WeicodeException("资产类别:" + item.category_id + "不存在;(第" + sort + "行)");
                        var user_name = DoMySql.FindEntity<ModelDb.user_base>($"name = '{item.user_name}'", false);
                        var cj_user_name = DoMySql.FindEntity<ModelDb.user_base>($"name = '{item.cj_user_name}'", false);
                        switch (item.status_txt)
                        {
                            case "在用":
                                if (user_name.IsNullOrEmpty()) throw new WeicodeException("资产为在用状态时，使用人不能为空");
                                if (user_name.user_sn.IsNullOrEmpty()) throw new WeicodeException("使用人:" + item.user_name + "不存在;(第" + sort + "行)");
                                item.status = ModelDb.asset.status_enum.在用.ToSByte();
                                break;
                            case "空闲":
                                if (!(user_name.IsNullOrEmpty())) throw new WeicodeException("资产为空闲状态时，使用人必须为空");
                                item.status = ModelDb.asset.status_enum.空闲.ToSByte();
                                break;
                            default:
                                throw new WeicodeException("请选择空闲或者在用");
                        }

                        var asset = item.ToModel<ModelDb.asset>();
                        asset.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        asset.asset_sn = asset_sn;
                        asset.user_sn = user_name.user_sn;
                        asset.cj_user_sn = cj_user_name.user_sn;
                        asset.category_id = asset_category.id;
                        lSql.Add(asset.InsertTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.asset
                {
                    public List<asset> l_asset { get; set; }
                    public string on_date { get; set; }

                }
                public class asset : ModelDb.asset
                {
                    public string category_name { get; set; }
                    public string user_name { get; set; }
                    public string cj_user_name { get; set; }
                    public string status_txt { get; set; }
                    public string company_txt { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 资产分类明细页面
            /// </summary>
            public class AssetCategoryList
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
                    pageModel.listFilter.isExport = true;
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

                    listFilter.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        placeholder = "类别名称",
                        defaultValue = ""
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtHidden("parent_id")
                    {
                        placeholder = "父级id",
                        defaultValue = req.parent_id.ToNullableString()
                    });


                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");

                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "添加类别",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {

                            url = "/Asset/Data/CategoryPost?parent_id=" + req.parent_id,
                        }
                    });
                    return buttonGroup;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
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
                        text = "类别名称",
                        width = "180",
                        minWidth = "180",
                        fix = ModelBasic.EmtModel.ListItem.Fixed.left
                    });
                    #endregion
                    #region 2.批量操作列
                    #endregion
                    #region 3.操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "下级选项",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"CategoryList",
                            field_paras = "parent_id=id"
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "编辑",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"CategoryPost",
                            field_paras = "id"
                        }

                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id"
                        }

                    });
                    /*

                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        actionEvent = EmtModel.ListOperateItem.ActionEvent.确认对话框,
                        style = "",
                        text = "删除",
                        actionUrl = $"Del",
                        actionParameters = "id"
                    });*/
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public int parent_id { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"parent_id = '{req["parent_id"]}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.asset_category, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public string user_sn { get; set; }
                    public int tenant_id { get; set; }
                    public int parent_id { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_category
                {
                }
                #endregion
                #region 删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    var Asset = DoMySql.FindEntity<ModelDb.asset_category>($"id='{dtoReqData.id}'");
                    Asset.Delete();
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string id { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 新增/编辑资产分类页面
            /// </summary>
            public class AssetCategoryPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
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
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var jushu_goods_anchor = DoMySql.FindEntity<ModelDb.asset_category>($"id = '{req.id}'", false);

                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("parent_id")
                    {
                        defaultValue = req.parent_id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("tenant_id")
                    {
                        defaultValue = new DomainBasic.UserApp().GetCurrentRole().tenant_id.ToString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "类别名称",
                        defaultValue = jushu_goods_anchor.name
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
                    public int parent_id { get; set; }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 新增分类
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    var asset_category = dtoReqData.ToModel<ModelDb.asset_category>();
                    asset_category.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    asset_category.InsertOrUpdate();
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.asset_category
                {

                }
                #endregion
            }
        }
    }
}