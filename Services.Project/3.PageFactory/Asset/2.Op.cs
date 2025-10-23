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
            /// 资产派发记录
            /// </summary>
            public class AssetAcceptList

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
                    listFilter.formItems.Add(new EmtInput("name")
                    {
                        placeholder = "资产名称",
                        defaultValue = ""
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
                        text = "新增派发",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Op/AcceptPost",
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("out_sn")
                    {
                        text = "派发单号",
                        width = "280",
                        minWidth = "280",
                        fix = EmtModel.ListItem.Fixed.left
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "提交时间",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "领用人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("accept_num")
                    {
                        text = "派发数量",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("end_num")
                    {
                        text = "已领取数量",
                        width = "100",
                        minWidth = "100",
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
                            url = $"AcceptContentList",
                            field_paras = "out_sn"
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
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"op_type='{ModelDb.asset_out.op_type_enum.派发出库.ToInt()}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_out, ItemDataModel>(filter, reqJson);
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
                    /// <summary>
                    /// 资产状态
                    /// </summary>
                    public string status { get; set; }
                    public int tenant_id { get; set; }
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_out
                {
                    public string user_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false).name;
                        }
                    }
                    public string accept_num
                    {
                        get
                        {
                            return DoMySql.FindList<ModelDb.asset_out_item>($"out_sn = '{this.out_sn}'").Count.ToString();
                        }
                    }
                    public string end_num
                    {
                        get
                        {
                            return DoMySql.FindList<ModelDb.asset_out_item>($"out_sn = '{this.out_sn}' and status='{ModelDb.asset_out_item.status_enum.出库成功.ToInt()}'").Count.ToString();
                        }
                    }
                    /// <summary>
                    /// 
                    /// </summary>
                }
                #endregion
            }
            /// <summary>
            /// 新建派发记录
            /// </summary>
            public class AssetAcceptPost
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
                    /*pageModel.buttonGroup = new EmtButtonGroup("")
                    {
                        buttonItems = new List<EmtModel.ButtonItem>
                        {
                        }
                    };*/
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
                    formDisplay.formItems.Add(new EmtHidden("tenant_id")
                    {
                        defaultValue = req.tenant_id.ToNullableString()
                    });

                    var option = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>())
                    {
                        option.Add(new ModelDoBasic.Option
                        {
                            text = item.name,
                            value = item.user_sn
                        });
                    }
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn")
                    {
                        title = "*领用人",
                        placeholder = "选择领用人",
                        options = option,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new EmtTimeSelect("plan_time")
                    {
                        title = "*约定领取时间",
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_asset")
                    {
                        title = "资产信息",
                        selectUrl = "/Asset/Op/Select?status=0",
                        buttonText = "选择资产",

                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                     {
                         new ModelBasic.EmtDataSelect.ColItem("category")
                         {
                              title = "资产类别",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              title = "资产名称"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("spec")
                         {
                              title = "规格型号",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("brand")
                         {
                              title = "品牌",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("company_text")
                         {
                              title = "所属公司",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("asset_sn")
                         {
                              title = "资产编号",
                         },
                     }
                    });
                    #endregion
                    return formDisplay;
                }
                public class ItemUser : ModelDbBasic.user_base
                {
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 附加额外参数
                    /// </summary>
                    public int id { get; set; }
                    public int tenant_id { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }

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
                    if (dtoReqData.user_sn.IsNullOrEmpty()) throw new WeicodeException("领用人不可为空！");
                    if (dtoReqData.plan_time.IsNullOrEmpty()) throw new WeicodeException("时间不可为空！");
                    var out_sn = "GH" + UtilityStatic.CommonHelper.CreateUniqueSn();
                    int count = 0;
                    if (dtoReqData.l_asset == null) throw new WeicodeException("请选择资产！");
                    foreach (var item in dtoReqData.l_asset)
                    {
                        count++;
                        lSql.Add(new ModelDb.asset
                        {
                            status = 1,
                            user_sn = dtoReqData.user_sn
                        }.UpdateTran($"asset_sn='{item.asset_sn}'"));
                        lSql.Add(new ModelDb.asset_out_item
                        {
                            user_sn = dtoReqData.user_sn,
                            out_sn = out_sn,
                            asset_sn = item.asset_sn,
                            plan_time = dtoReqData.plan_time.ToDate()
                        }.InsertTran());
                    }
                    lSql.Add(new ModelDb.asset_out
                    {
                        num = count,
                        out_sn = out_sn,
                        create_time = DateTime.Now,
                        op_type = 1,
                        user_sn = dtoReqData.user_sn
                    }.InsertTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 约定出库时间
                    /// </summary>
                    public string plan_time { get; set; }
                    public List<accept> l_asset { get; set; }

                }
                public class accept
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 规格型号
                    /// </summary>
                    public string spec { get; set; }
                    /// <summary>
                    /// 品牌
                    /// </summary>
                    public string brand { get; set; }
                    /// <summary>
                    /// 所属公司
                    /// </summary>
                    public string company_id { get; set; }
                    /// <summary>
                    /// 资产编号
                    /// </summary>
                    public string asset_sn { get; set; }
                    public int category_id { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 资产派发详细列表
            /// </summary>
            public class AssetAcceptContentList
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
                    listFilter.formItems.Add(new EmtInput("out_sn")
                    {
                        placeholder = "",
                        defaultValue = req.out_sn
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
                        width = "140",
                        minWidth = "140",
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
                        width = "140",
                        minWidth = "140",
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
                        width = "240",
                        minWidth = "240",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "领用人",
                        width = "100",
                        minWidth = "100",
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
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 出库编号
                    /// </summary>
                    public string out_sn { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// 资产出库明细表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"out_sn = '{req["out_sn"]}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_out_item, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 出库编号
                    /// </summary>
                    public string out_sn { get; set; }
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_out_item
                {

                    public ModelDb.asset asset
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{this.asset_sn}'");
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
                    public string company_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("公司名称", asset.company_id);
                        }
                    }

                    public string user_name
                    {
                        get
                        {
                            var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false);
                            return user_base.name;
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 资产归还记录
            /// </summary>
            public class AssetBackList
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
                    var listFilter = new CtlListFilter(req);
                    listFilter.formItems.Add(new EmtInput("name")
                    {
                        placeholder = "资产名称",
                        defaultValue = ""
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
                        text = "新增退库",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Op/BackPost",
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("in_sn")
                    {
                        text = "退库单号",
                        width = "280",
                        minWidth = "280",
                        fix = EmtModel.ListItem.Fixed.left
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
                            url = $"BackContentList",
                            field_paras = "in_sn"
                        }
                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : PageList.Req
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"op_type='{ModelDb.asset_in.op_type_enum.退库入库.ToInt()}'";
                    var req = reqJson.GetPara();
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
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
                    /// <summary>
                    /// 资产类别
                    /// </summary>
                    public int op_type { get; set; }
                    public int tenant_id { get; set; }


                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_in
                {

                }
                #endregion
            }
            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class AssetBackPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.eventCsAction = new PagePost.EventCsAction
                    {
                        func = PostAction
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    /*pageModel.buttonGroup = new EmtButtonGroup("")
                    {
                        buttonItems = new List<EmtModel.ButtonItem>
                        {
                        }
                    };*/
                    return pageModel;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay; ;

                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    var opinion = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>())
                    {
                        var count = DoMySql.FindList<ModelDb.asset>($"user_sn = '{item.user_sn}'").Count.ToString();
                        opinion.Add(new ModelDoBasic.Option
                        {
                            text = item.name + "(" + count + ")",
                            value = item.user_sn
                        });
                    }
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn")
                    {
                        title = "*持有人",
                        placeholder = "选择持有人",
                        options = opinion,
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtTimeSelect("plan_time")
                    {
                        title = "*约定归还时间",
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtDataSelect("l_asset")
                    {
                        title = "资产信息",
                        selectUrl = "/Asset/Op/Select?user_sn=<%=page.user_sn.value%>&status=1",
                        buttonText = "选择资产",
                        buttonAddOneText = null,

                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                     {
                         new ModelBasic.EmtDataSelect.ColItem("category")
                         {
                              title = "资产类别",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              title = "资产名称"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("spec")
                         {
                              title = "规格型号",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("brand")
                         {
                              title = "品牌",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("company_text")
                         {
                              title = "所属公司",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("asset_sn")
                         {
                              title = "资产编号",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("user_name")
                         {
                              title = "持有人",
                         }
                     }
                    });

                    #endregion
                    return formDisplay;
                }
                public ModelBasic.EmtInput.AutoComplete.Res GetData(ModelBasic.EmtInput.AutoComplete.Req req)
                {
                    var info = new ModelBasic.EmtInput.AutoComplete.Res();
                    info.data = DoMySql.FindList<ModelDbBasic.user_base, ItemUser>(new DoMySql.Filter
                    {
                        fields = "name as username",
                        where = $"name like '%{req.keyword}%' and user_type_id = '{new DomainBasic.UserTypeApp().GetInfoByCode("user").id}' and tenant_id ='{new DomainBasic.TenantApp().GetInfo().id}'"
                    });
                    return info;
                }
                public class ItemUser : ModelDbBasic.user_base
                {
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 附加额外参数
                    /// </summary>
                    public int id { get; set; }
                    public int tenant_id { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 约定出库时间
                    /// </summary>
                    public string plan_time { get; set; }
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
                    if (dtoReqData.user_sn.IsNullOrEmpty()) throw new WeicodeException("持有人不可为空！");
                    if (dtoReqData.plan_time.IsNullOrEmpty()) throw new WeicodeException("时间不可为空！");
                    var in_sn = "PF" + UtilityStatic.CommonHelper.CreateUniqueSn();
                    int count = 0;
                    if (dtoReqData.l_asset == null) throw new WeicodeException("请选择资产！");
                    foreach (var item in dtoReqData.l_asset)
                    {

                        lSql.Add(new ModelDb.asset_in_item
                        {
                            user_sn = dtoReqData.user_sn,
                            in_sn = in_sn,
                            asset_sn = item.asset_sn,
                            plan_time = dtoReqData.plan_time.ToDate(),
                        }.InsertTran());
                        count++;
                    }
                    lSql.Add(new ModelDb.asset_in
                    {
                        in_sn = in_sn,
                        num = count,
                        create_time = DateTime.Now,
                        op_type = 2
                    }.InsertTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 约定出库时间
                    /// </summary>
                    public string plan_time { get; set; }
                    public List<asset> l_asset { get; set; }

                }
                public class asset
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 规格型号
                    /// </summary>
                    public string spec { get; set; }
                    /// <summary>
                    /// 品牌
                    /// </summary>
                    public string brand { get; set; }
                    /// <summary>
                    /// 所属公司
                    /// </summary>
                    public string company_id { get; set; }
                    /// <summary>
                    /// 资产编号
                    /// </summary>
                    public string asset_sn { get; set; }
                    public int category_id { get; set; }
                }
                #endregion
            }
            /// <summary>
            /// 资产退库详细列表
            /// </summary>
            public class AssetBackContentList
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
                    listFilter.formItems.Add(new EmtInput("in_sn")
                    {
                        placeholder = "",
                        defaultValue = req.in_sn
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "持有人",
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
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
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
                    string where = $"in_sn = '{req["in_sn"]}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_in_item, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : PageList.ListData.Req
                {
                    /// <summary>
                    /// 入库编号
                    /// </summary>
                    public string in_sn { get; set; }
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_in_item
                {

                    public ModelDb.user_base user_base = null;
                    public ModelDb.asset asset
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{this.asset_sn}'");
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
                    public string company_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("公司名称", asset.company_id);
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
                }
                #endregion
            }
            /// <summary>
            /// 资产转移
            /// </summary>
            public class AssetMoveList
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
                    listFilter.formItems.Add(new EmtInput("name")
                    {
                        placeholder = "资产名称",
                        defaultValue = ""
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
                        text = "转移登记",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Asset/Op/MovePost",
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
                    listDisplay.operateWidth = "180";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("move_sn")
                    {
                        text = "转移单号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_before_name")
                    {
                        text = "原领用人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_after_name")
                    {
                        text = "现领用人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ac_date")
                    {
                        text = "转移日期",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("cause")
                    {
                        text = "转移事由",
                        width = "200",
                        minWidth = "180",
                    });
                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "编辑",
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"MovePost",
                            field_paras = "id"
                        }
                    });
                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        actionEvent = EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "查看详情",
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"MoveContentList",
                            field_paras = "move_sn"
                        }
                    });
                    #region 操作列按钮

                    /*
                    listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem
                    {
                        actionEvent = EmtModel.ListOperateItem.ActionEvent.确认对话框,
                        style = "",
                        text = "删除",
                        actionUrl = $"Del",
                        actionParameters = "id"
                    });*/

                    return listDisplay;
                    #endregion
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
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {

                    var req = reqJson.GetPara();
                    string where = $"1=1";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };

                    return new CtlListDisplay.ListData().getList<ModelDb.asset_move, ItemDataModel>(filter, reqJson);
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
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_move
                {
                    public ModelDb.user_base user_base = null;
                    public string user_before_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn_before}'", false).name;
                        }
                    }
                    public string user_after_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn_after}'", false).name; ;
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 新建资产转移
            /// </summary>
            public class AssetMovePost
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
                    var asset_move = DoMySql.FindEntity<ModelDb.asset_move>($"id = '{req.id}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    var user = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>())
                    {

                        var count = DoMySql.FindList<ModelDb.asset>($"user_sn = '{item.user_sn}'").Count.ToString();
                        user.Add(new ModelDoBasic.Option
                        {
                            text = item.name + "(" + count + ")",
                            value = item.user_sn
                        });
                    }
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_before")
                    {
                        title = "选择前转移人",
                        options = user,
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_after")
                    {
                        title = "选择现转移人",
                        options = user,
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtTimeSelect("ac_date")
                    {
                        title = "转移日期",
                        defaultValue = asset_move.ac_date.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "转移事由",
                        defaultValue = asset_move.tenant_id.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                    {
                        title = "资产信息",
                        selectUrl = "/Asset/Op/Select?user_sn=<%=page.user_sn_before.value%>&status=1",
                        buttonText = "选择资产",
                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                     {
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              edit = "text",
                              title = "资产名称"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("spec")
                         {
                              edit = "text",
                              title = "规格型号",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("brand")
                         {
                              edit = "text",
                              title = "品牌",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("company_id")
                         {
                              edit = "text",
                              title = "公司id",
                         },
                         new ModelBasic.EmtDataSelect.ColItem("asset_sn")
                         {
                              edit = "text",
                              title = "资产编号",
                         },
                     }
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
                }
                #endregion
                #region 异步请求处理
                #endregion
            }
            /// <summary>
            /// 资产转移详细列表
            /// </summary>
            public class AssetMoveContentList
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
                    listFilter.formItems.Add(new EmtInput("move_sn")
                    {
                        placeholder = "",
                        defaultValue = req.move_sn
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("move_sn")
                    {
                        text = "转移编号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("name")
                    {
                        text = "资产名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("asset_id")
                    {
                        text = "资产id",
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
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "领用人",
                        width = "100",
                        minWidth = "100",
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
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 转移编号
                    /// </summary>
                    public string move_sn { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// 资产转移明细表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"move_sn = '{req["move_sn"]}'";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset_move_item, ItemDataModel>(filter, reqJson);
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
                    /// <summary>
                    /// 转移编号
                    /// </summary>
                    public string move_sn { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset_move_item
                {
                    public ModelDb.user_base user_base = null;
                    public ModelDb.asset asset
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset>($"asset_sn = '{this.asset_sn}'");
                        }
                    }
                    public string name
                    {
                        get
                        {
                            return asset.name;
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
                    public string user_name
                    {
                        get
                        {
                            user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false);
                            return asset.name;
                        }
                    }
                    public string company_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("公司名称", asset.company_id);
                        }
                    }
                }
                #endregion
            }
            /// <summary>
            /// 选择资产表单
            /// </summary>
            public class AssetSelect
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
                    pageModel.dataSelect.selectEvent.cbSelected = ModelBasic.EmtDataSelect.reloadListByData();
                    return pageModel;
                }
                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter(req);
                    listFilter.formItems.Add(new EmtHidden("user_sn")
                    {
                        placeholder = "领用人",
                        defaultValue = req.user_sn
                    });
                    listFilter.formItems.Add(new EmtInput("name")
                    {
                        placeholder = "资产名称",
                        defaultValue = ""
                    });
                    listFilter.formItems.Add(new EmtHidden("status")
                    {
                        placeholder = "资产状态",
                        defaultValue = req.status
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
                    var listDisplay = new CtlListDisplay(req);
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("category")
                    {
                        text = "资产类别",
                        width = "100",
                        minWidth = "100",
                    });
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
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("company_text")
                    {
                        text = "所属公司",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("brand")
                    {
                        text = "品牌",
                        width = "120",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("price")
                    {
                        text = "资产价值",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "使用人",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("on_date_text")
                    {
                        text = "登记日期",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "资产状态",
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
                public class DtoReq : PageList.Req
                {
                    /// <summary>
                    /// 资产名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 资产状态
                    /// </summary>
                    public string status { get; set; }
                    public int id { get; set; }
                }
                #endregion
                #region ListData

                /// <summary>
                /// 资产表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {

                    var req = reqJson.GetPara();
                    string where = $"1=1";
                    if (!req["name"].ToNullableString().IsNullOrEmpty()) where += $" AND (name like '%{req["name"]}%')";
                    if (!req["status"].ToNullableString().IsNullOrEmpty()) where += $" AND (status ='{req["status"]}')";
                    if (!req["user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (user_sn ='{req["user_sn"]}')";
                    //name查询条件
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.asset, ItemDataModel>(filter, reqJson);
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
                    /// <summary>
                    /// 资产状态
                    /// </summary>
                    public string status { get; set; }
                    /// <summary>
                    /// 使用人
                    /// </summary>
                    public string user_sn { get; set; }
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.asset
                {
                    public ModelDb.user_base user_Base = null;
                    public string on_date_text
                    {
                        get
                        {
                            return this.on_date.ToDate().ToString("D");
                        }
                    }
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
                    public string category
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.asset_category>($"id = '{this.category_id}'", false).name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.asset.status_enum>();
                        }
                    }
                    public string company_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("公司名称", company_id);
                        }
                    }
                }
                #endregion
            }
        }
    }


}