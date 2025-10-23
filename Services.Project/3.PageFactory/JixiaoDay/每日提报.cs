using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    /// <summary>
    /// 绩效上报
    /// </summary>
    public partial class PageFactory
    {
        public partial class JixiaoDay
        {
            /// <summary>
            /// 绩效列表
            /// </summary>
            public class UserDayReport
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public ModelBasic.PageList Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageList("PageList");

                    pageModel.listFilter = GetListFilter(req);

                    pageModel.listDisplay = GetListDisplay(req);
                    //pageModel.buttonGroup = GetButtonGroup(req);
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        disabled = true,
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "厅管账号",
                        disabled = true,
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(req.relation_type, new UserIdentityBag().user_sn)}", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                    {
                        placeholder = "主播账号",
                        options = new Dictionary<string, string>(),
                        disabled = true
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        placeholder = "绩效发生日期",
                        defaultValue = req.c_date_range
                    });
                    return listFilter;
                }

                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new Dictionary<string, string>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString())}"))
                    {
                        option.Add(item.username, item.user_sn);
                    }
                    result.data = option.ToJson();
                    return result;
                }

                /// <summary>
                /// 获取厅管筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    /*
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "上报绩效",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Crm/Customer/ReportPost",
                        }
                    });
                    */
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "160";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "绩效发生日期",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                    {
                        text = "主播账号",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                    {
                        text = "音浪值",
                        width = "100",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                    {
                        text = "拉新数",
                        width = "100",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num")
                    {
                        text = "建联数",
                        width = "100",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("datou_num")
                    {
                        text = "误刷大头",
                        width = "100",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_text")
                    {
                        text = "时间段",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("question_demo")
                    {
                        text = "今日问题",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("summary_demo")
                    {
                        text = "今日总结",
                        width = "280",
                        minWidth = "280"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("review_demo")
                    {
                        text = "反思",
                        width = "280",
                        minWidth = "280"
                    });
                    #region 操作列按钮


                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "edit",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        disabled = true,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/PCrm/Report/Post",
                            field_paras = "c_date,session"
                        },
                        text = "编辑"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                    public string keyword { get; set; }
                    public string c_date_range { get; set; }
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
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{new UserIdentityBag().user_sn}'");

                    string where = "1=1";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                    }

                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                    }

                    if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                    }

                    if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                        where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                    }



                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by c_date desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_session, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string keyword { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_jixiao_day_session
                {
                    public string c_date_text
                    {
                        get
                        {
                            return this.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string zb_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                        }
                    }
                    public string session_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("场次", this.session.ToString());
                        }
                    }
                }
                #endregion
            }


            /// <summary>
            /// 按分档显示绩效列表
            /// </summary>
            public class UserDayReportSession
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public ModelBasic.PageList Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageList("PageList");
                    pageModel.listFilter = GetListFilter(req);
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.buttonGroup = GetButtonGroup(req);
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        disabled = true,
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                    {
                        placeholder = "厅管账号",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });

                    var option_zb = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn))
                    {
                        option_zb.Add(item.username, item.user_sn);
                    }
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                    {
                        placeholder = "主播账号",
                        options = option_zb,
                        disabled = true
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range", true)
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        placeholder = "绩效发生日期",
                        defaultValue = req.c_date_range
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("is_rest")
                    {
                        placeholder = "是否请假",
                        width = "100px",
                        options= new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = req.c_date_range
                    });
                    return listFilter;
                }

                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString()))
                    {
                        option.Add(item.name, item.user_sn);
                    }
                    result.data = option.ToJson();
                    return result;
                }

                /// <summary>
                /// 获取厅管筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");

                    //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("reportday")
                    //{
                    //    text = "数据提交",
                    //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //    {
                    //        url = "Post",
                    //    },
                    //    disabled=true
                    //});

                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "160";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "绩效发生日期",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                    {
                        text = "主播账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_option")
                    {
                        text = "时间段",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_1")
                    {
                        text = "首消音浪",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2")
                    {
                        text = "二消个数",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2")
                    {
                        text = "二消音浪",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hx_num")
                    {
                        text = "回消人数",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hx_amount")
                    {
                        text = "回消音浪",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hdpk_amount")
                    {
                        text = "活动PK音浪",
                        width = "110",
                        minWidth = "110",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                    {
                        text = "拉新数",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num")
                    {
                        text = "建联数",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("datou_num")
                    {
                        text = "误刷大头",
                        width = "90",
                        minWidth = "90",
                        summaryReq = new Pagination.SummaryReq
                        {
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hanhuo_num_total")
                    {
                        text = "喊活数量",
                        width = "90",
                        minWidth = "90",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("xinfu_num_total")
                    {
                        text = "新付数量",
                        width = "90",
                        minWidth = "90",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("is_rest_text")
                    {
                        text = "是否请假",
                        width = "90",
                        minWidth = "90",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("op_name")
                    {
                        text = "提交人",
                        width = "180",
                        minWidth = "180",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "提交时间",
                        width = "180",
                        minWidth = "180",

                    });
                    /*
                     listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("summary_demo")
                    {
                        text = "今日总结",
                        width = "280",
                        minWidth = "280"
                    });
                     */

                    #region 操作列按钮


                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "edit",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Post",
                            field_paras = "id,zb_user_sn"
                        },
                        text = "编辑"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        disabled = true,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = DelAction
                        },
                        text = "删除"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                    public string keyword { get; set; }
                    public string c_date_range { get; set; }
                }


                #region 回调cs函数
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_jixiao_day_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.GetPara()["id"].ToNullableString().ToInt());

                    if (p_jixiao_day_session.c_date <= DateTime.Today.AddDays(-2))
                    {
                        result.code = 1;
                        result.msg = "删除失败,不能删除提交超过3天的数据";
                        return result;
                    }

                    if (p_jixiao_day_session.Delete() > 0)
                    {
                        var sum = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(amount_1),sum(num_2),sum(amount_2),sum(old_amount),sum(amount),sum(contact_num),sum(datou_num),sum(new_num),sum(hdpk_amount),sum(hx_num),sum(hx_amount)", $"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                        new ModelDb.p_jixiao_day
                        {
                            amount_1 = sum[0].ToInt(),
                            num_2 = sum[1].ToInt(),
                            amount_2 = sum[2].ToInt(),
                            old_amount = sum[3].ToInt(),
                            amount = sum[4].ToInt(),
                            contact_num = sum[5].ToInt(),
                            datou_num = sum[6].ToInt(),
                            new_num = sum[7].ToInt(),
                            hdpk_amount = sum[8].ToInt(),
                            hx_num = sum[9].ToInt(),
                            hx_amount = sum[10].ToInt(),
                            session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                        }.Update($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                    }

                    new DomainBasic.SystemBizLogApp().Write("数据提报", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"删除提报:(id:{p_jixiao_day_session.id},绩效日期:{p_jixiao_day_session.c_date.ToDate().ToString("yyyy年MM月dd日")},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", p_jixiao_day_session.session.ToString())},所属主播;{new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).username})");
                    return result;
                }
                #endregion


                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = "1=1";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                    }

                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                    }

                    if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                    }

                    if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                        where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                    }
                    if (!req["is_rest"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and is_rest = '{req["is_rest"].ToNullableString()}'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by c_date desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_session, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string keyword { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_jixiao_day_session
                {
                    public string op_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(op_user_sn).username;
                        }
                    }
                    public string is_rest_text
                    {
                        get
                        {
                            return is_rest == 0 ? "否" : "是";
                        }
                    }
                    public string session_option
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("场次", this.session.ToString());
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return this.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string zb_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                        }
                    }
                    public string hanhuo_num_total
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{this.tg_user_sn}' and c_date='{this.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{this.session}'", false).hanhuo_num.ToString();
                        }
                    }
                    public string xinfu_num_total
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{this.tg_user_sn}' and c_date='{this.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{this.session}'", false).xinfu_num.ToString();
                        }
                    }
                }
                #endregion
            }


            /// <summary>
            /// 每日上报绩效数据
            /// </summary>
            public class UserDayReportPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.style = @"background-image:url('/Assets/images/qgxkt_m.jpg');background-size: cover;background-position: center; background-repeat: no-repeat;";
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    pageModel.submitButton.eventJsClick.eventJavascript.code = @"if((page_post.new_num.value==='0'||page_post.new_num.value==='')&&
                                      (page_post.amount.value==='0'||page_post.amount.value==='')&&
                                      (page_post.num_2.value==='0'||page_post.num_2.value==='')&&
                                      (page_post.amount_2.value==='0'||page_post.amount_2.value==='')&&
                                      (page_post.contact_num.value==='0'||page_post.contact_num.value==='')&&
                                      (page_post.datou_num.value==='0'||page_post.datou_num.value==='')){
                                        page.floatlayer3.show();
                                        return false;
                                   }else{
                                    }";

                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer2")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            left = 100
                        },
                        emtModelBase = new EmtHtml("CreateUser")
                        {
                            Content = @"<a href=""/pcrm/pcrm/crmpost"" style=""color:#3399FF;"">新建用户</a>",
                        }
                    });

                    pageModel.adjuncts.Add(new ModelBasic.AdjFloatLayer("floatlayer3")
                    {
                        height = "100px",
                        style = " border:5px solid #F0F0F0; background-color:#FDFDFD; padding:10px;",
                        position = ModelBasic.AdjFloatLayer.Position.固定定位,
                        positionRelative = new ModelBasic.AdjFloatLayer.PositionRelative
                        {
                            className = "post_submit_post",
                            offset_x = 20,
                            offset_y = 20
                        },
                        positionFixed = new ModelBasic.AdjFloatLayer.PositionFixed
                        {
                            display = ModelBasic.AdjFloatLayer.Display.none,
                            bottom = 40,
                            left = 10
                        },
                        emtModelBase = new ModelBasic.EmtGrid("Confirm")
                        {
                            items = new List<ModelBasic.EmtGrid.Item>
                        {
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=12,
                                emtModelBase=new ModelBasic.EmtLabel("tip")
                                {
                                    width="50px",
                                    defaultValue="当前是否为挂麦?",
                                    displayStatus= EmtModelBase.DisplayStatus.只读
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("true")
                                {
                                    width="50px",
                                    defaultValue="是",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code = "page_post.is_guamai.set('1')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("false")
                                {
                                    width="50px",
                                    defaultValue="否",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code="page_post.is_guamai.set('0')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtButton("cancle")
                                {
                                    width="200px",
                                    defaultValue="取消",
                                    eventJsChange=new EmtFormBase.EventJsChange
                                    {
                                        eventJavascript=new EventJavascript
                                        {
                                            code="page.floatlayer3.hide();"
                                        }
                                    }
                                }
                            }
                        }
                        }
                    });

                    return pageModel;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("bg");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b1")
                    {
                        text = "提交记录",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/JixiaoDay/Report/List",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b2")
                    {
                        text = "首页",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                        eventToUrl = new EmtModel.ButtonItem.EventToUrl
                        {
                            url = "/Default/MainPage/Index",
                            target = "top"
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b3")
                    {
                        text = "新增用户",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                        eventToUrl = new EmtModel.ButtonItem.EventToUrl
                        {
                            url = "/PCrm/PCrm/CrmPost",
                            target = "top"
                        }
                    });

                    return buttonGroup;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_jixiao_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.id, false);
                    var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{p_jixiao_session.zb_user_sn}' and c_date='{p_jixiao_session.c_date.ToDate().ToString("yyyy-MM-dd")}'", false);
                    
                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{req.tg_user_sn}' and c_date='{p_jixiao_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_session.session}'", false);
                    if (p_jixiao_day.IsNullOrEmpty())
                    {
                        p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{new UserIdentityBag().user_sn}' and c_date='{DateTime.Today.ToString("yyyy-MM-dd")}'", false);
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("is_guamai")
                    {
                        defaultValue = "0"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtButton("show_tips")
                    {
                        defaultValue = "查看提报规则",
                        eventJsChange=new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript()
                            {
                                code = "confirm(`二消定义：99拉新用户在本档内第二次及多次的消费音浪值（如果二消用户送了全麦，拉新主播的拉新个数+1，二消个数+1。其余主播拉新个数不增加，二消个数不增加，音浪值归属到老用户）\r\n \r\n喊活、新付报量规则：系统是以最后一个提报喊活新付数据的主播为准（例：主播A首先提报了3喊活、3新付，主播B随后提报了2喊活、2新付那这个档上实际喊活新付以主播B提交的数据为准）喊活、新付每个档最好由一个人统计提报喊活新付总数，其他人不填`);"
                            }  
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTextarea("zb")
                    {
                        title = "所属主播",
                        style = "background-color: transparent;",
                        defaultValue = p_jixiao_session.zb_user_sn.IsNullOrEmpty() ? new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).username : new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_session.zb_user_sn).username,
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("zb_user_sn")
                    {
                        title = "主播user_sn",
                        defaultValue = req.zb_user_sn
                    });
                    
                    var defaultDate = p_jixiao_session.c_date;
                    if (defaultDate.IsNullOrEmpty())
                    {
                        defaultDate = DateTime.Today;
                    }

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "绩效日期",
                        style = "background-color: transparent;",
                        defaultValue = defaultDate.ToDate().ToString("yyyy-MM-dd"),
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "c_date","<%=page_post.c_date.value%>"},
                                    { "zb_user_sn","<%=page_post.zb_user_sn.value%>"}
                                },
                                func = GetZbSessionInfo,
                                resCallJs = $"page_post.amount.setPlaceholder('本日累计:'+res.data.amount);" +
                                            $"page_post.amount_1.setPlaceholder('本日累计:'+res.data.amount_1);" +
                                            $"page_post.num_2.setPlaceholder('本日累计:'+res.data.num_2);" +
                                            $"page_post.amount_2.setPlaceholder('本日累计:'+res.data.amount_2);" +
                                            //$"page_post.old_amount.setPlaceholder('本日累计:'+res.data.old_amount);" +
                                            $"page_post.hdpk_amount.setPlaceholder('本日累计:'+res.data.hdpk_amount);" +
                                            $"page_post.hx_num.setPlaceholder('本日累计:'+res.data.hx_num);" +
                                            $"page_post.hx_amount.setPlaceholder('本日累计:'+res.data.hx_amount);" +
                                            $"page_post.new_num.setPlaceholder('本日累计:'+res.data.new_num);" +
                                            $"page_post.contact_num.setPlaceholder('本日累计:'+res.data.contact_num);" +
                                            $"page_post.datou_num.setPlaceholder('本日累计:'+res.data.datou_num);" +
                                            $"page_post.hanhuo_num.set(res.data.hanhuo_num);" +
                                            $"page_post.xinfu_num.set(res.data.xinfu_num);"
                            }
                        }
                    });
                    /*formDisplay.formItems.Add(new ModelBasic.EmtSelect("is_leader")
                    {
                        title = "我是组长",
                        colLength = 6,
                        options = new Dictionary<string, string>
                        {
                            { "是","1"},
                            { "否","0"}
                        },
                        defaultValue=p_jixiao_session.is_leader.ToString()
                    });*/

                    var tgsn= "";
                    if (new DomainBasic.UserTypeApp().GetInfoByCode("tger").id == new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        tgsn = new UserIdentityBag().user_sn;
                    }
                    else
                    {
                        tgsn= new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn);
                    }
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("session")
                    {
                        title = "时间段",
                        style = "background-color: transparent;",
                        colLength = 12,
                        options = new DomainBasic.DictionaryApp().GetListForOption("场次"),
                        defaultValue = p_jixiao_session.session.ToNullableString(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "c_date","<%=page_post.c_date.value%>"},
                                { "zb_user_sn","<%=page_post.zb_user_sn.value%>"},
                                { "tg_user_sn",tgsn},
                                { "session","<%=page_post.session.value%>"}
                            },
                                func = GetZbSessionInfo,
                                resCallJs = $"page_post.amount.setPlaceholder('本日累计:'+res.data.amount);" +
                                            $"page_post.amount_1.setPlaceholder('本日累计:'+res.data.amount_1);" +
                                            $"page_post.num_2.setPlaceholder('本日累计:'+res.data.num_2);" +
                                            $"page_post.amount_2.setPlaceholder('本日累计:'+res.data.amount_2);" +
                                            //$"page_post.old_amount.setPlaceholder('本日累计:'+res.data.old_amount);" +
                                            $"page_post.hdpk_amount.setPlaceholder('本日累计:'+res.data.hdpk_amount);" +
                                            $"page_post.hx_num.setPlaceholder('本日累计:'+res.data.hx_num);" +
                                            $"page_post.hx_amount.setPlaceholder('本日累计:'+res.data.hx_amount);" +
                                            $"page_post.new_num.setPlaceholder('本日累计:'+res.data.new_num);" +
                                            $"page_post.contact_num.setPlaceholder('本日累计:'+res.data.contact_num);" +
                                            $"page_post.datou_num.setPlaceholder('本日累计:'+res.data.datou_num);" +
                                            $"page_post.hanhuo_num.set(res.data.hanhuo_num);"+
                                            $"page_post.xinfu_num.set(res.data.xinfu_num);"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("is_rest")
                    {
                        title = "是否请假",
                        style = "background-color: transparent;",
                        options = new Dictionary<string, string>
                    {
                        {"是","1" },
                        {"否","0" }
                    },
                        defaultValue = p_jixiao_session.is_rest.ToNullableString().IsNullOrEmpty() ? "0" : p_jixiao_session.is_rest.ToString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "基本信息",
                        style = "background-color: transparent;",
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("hanhuo_num")
                    {
                        title = "喊活人数",
                        style = "background-color: transparent;",
                        colLength = 6,
                        defaultValue = p_jixiao_day_session_total.hanhuo_num.ToString().IsNullOrEmpty() ? "0" : p_jixiao_day_session_total.hanhuo_num.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("xinfu_num")
                    {
                        title = "新付费人数",
                        style = "background-color: transparent;",
                        colLength = 6,
                        defaultValue = p_jixiao_day_session_total.xinfu_num.ToString().IsNullOrEmpty() ? "0" : p_jixiao_day_session_total.xinfu_num.ToString(),

                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("new_num")
                    {
                        title = "拉新数",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.new_num,
                        colLength = 6,
                        defaultValue = p_jixiao_session.new_num.ToNullableString()
                    });
                    string code = @"if(page_post.amount_1.value===''){page_post.amount_1.value = parseFloat(0);}
                                if(page_post.amount_2.value===''){page_post.amount_2.value = parseFloat(0);}
                                if(page_post.hx_amount.value===''){page_post.hx_amount.value = parseFloat(0);}
                                if(page_post.hdpk_amount.value===''){page_post.hdpk_amount.value = parseFloat(0);}
                                page_post.amount.set(parseFloat(page_post.amount_1.value)+parseFloat(page_post.amount_2.value)+parseFloat(page_post.hx_amount.value)+parseFloat(page_post.hdpk_amount.value));";
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_1")
                    {
                        title = "首消音浪",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.amount_1,
                        colLength = 6,
                        defaultValue = p_jixiao_session.amount_1.ToNullableString(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = code
                            }
                        }

                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("num_2")
                    {
                        title = "二消个数",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.num_2,
                        colLength = 6,
                        defaultValue = p_jixiao_session.num_2.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2")
                    {
                        title = "二消音浪",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.amount_2,
                        colLength = 6,
                        defaultValue = p_jixiao_session.amount_2.ToNullableString(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = code
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("hx_num")
                    {
                        title = "老用户人数",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.hx_num,
                        colLength = 6,
                        defaultValue = p_jixiao_session.hx_num.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("hx_amount")
                    {
                        title = "老用户音浪",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.hx_amount,
                        colLength = 6,
                        defaultValue = p_jixiao_session.hx_amount.ToNullableString(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = code
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("hdpk_amount")
                    {
                        title = "活动PK音浪",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.hdpk_amount,
                        colLength = 6,
                        defaultValue = p_jixiao_session.hdpk_amount.ToNullableString(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = code
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("contact_num")
                    {
                        title = "建联数",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.contact_num,
                        colLength = 6,
                        defaultValue = p_jixiao_session.contact_num.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("datou_num")
                    {
                        title = "误刷大头",
                        style = "background-color: transparent;",
                        colLength = 6,
                        placeholder = "本日累计:" + p_jixiao_day.datou_num,
                        defaultValue = p_jixiao_session.datou_num.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("amount")
                    {
                        title = "总音浪",
                        style = "background-color: transparent;",
                        placeholder = "本日累计:" + p_jixiao_day.amount,
                        colLength = 6,
                        defaultValue = p_jixiao_session.amount.ToString().IsNullOrEmpty() ? "0" : p_jixiao_session.amount.ToString(),
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    
                    /*
                     formDisplay.formItems.Add(new ModelBasic.EmtTextarea("summary_demo")
                    {
                        title = "今日总结",
                        mode = ModelBasic.EmtTextarea.Mode.TextArea,
                        defaultValue = p_jixiao_day.summary_demo,
                        height = 100
                    });
                     */

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("question_demo")
                    {
                        title = "今日问题",

                        //mode = ModelBasic.EmtTextarea.Mode.TextArea,
                        defaultValue = p_jixiao_day.question_demo,
                        //height=100
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("review_demo")
                    {
                        title = "反思",
                        //mode = ModelBasic.EmtTextarea.Mode.TextArea,
                        defaultValue = p_jixiao_day.review_demo,
                        //height = 100
                    });

                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public int id { get; set; } = 0;
                    public string zb_user_sn { get; set; }
                    public string tg_user_sn { get; set; }
                }

                /// <summary>
                /// 获取主播提交记录
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZbSessionInfo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();

                    var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"c_date='{req["c_date"].ToNullableString()}' and zb_user_sn='{req["zb_user_sn"].ToNullableString()}'", false);
                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and c_date='{req["c_date"].ToNullableString()}' and session='{req["session"].ToNullableString()}'", false);
                    result.data = new
                    {
                        amount_1 = (p_jixiao_day.amount_1.IsNullOrEmpty() ? 0 : p_jixiao_day.amount_1),
                        num_2 = (p_jixiao_day.num_2.IsNullOrEmpty() ? 0 : p_jixiao_day.num_2),
                        amount_2 = (p_jixiao_day.amount_2.IsNullOrEmpty() ? 0 : p_jixiao_day.amount_2),
                        old_amount = (p_jixiao_day.old_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.old_amount),
                        hdpk_amount = (p_jixiao_day.hdpk_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.hdpk_amount),
                        hx_amount = (p_jixiao_day.hx_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.hx_amount),
                        hx_num = (p_jixiao_day.hx_num.IsNullOrEmpty() ? 0 : p_jixiao_day.hx_num),
                        new_num = (p_jixiao_day.new_num.IsNullOrEmpty() ? 0 : p_jixiao_day.new_num),
                        contact_num = (p_jixiao_day.contact_num.IsNullOrEmpty() ? 0 : p_jixiao_day.contact_num),
                        datou_num = (p_jixiao_day.datou_num.IsNullOrEmpty() ? 0 : p_jixiao_day.datou_num),
                        summary_demo = (p_jixiao_day.summary_demo.IsNullOrEmpty() ? "" : p_jixiao_day.summary_demo),
                        hanhuo_num = (p_jixiao_day_session_total.hanhuo_num.IsNullOrEmpty() ? 0 : p_jixiao_day_session_total.hanhuo_num),
                        xinfu_num = (p_jixiao_day_session_total.xinfu_num.IsNullOrEmpty() ? 0 : p_jixiao_day_session_total.xinfu_num)
                    };
                    return result;
                }

                #endregion
                #region 异步请求处理
                /// <summary>
                /// 新增每日上报
                /// </summary>
                /// <param name="req">1.主播提交每日数据;2.主播修改每日数据;3.厅管修改下级主播每日数据</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_jixiao_day_session= req.GetPara().ToModel<ModelDb.p_jixiao_day_session>();

                    int? hanhuo_num = req.GetPara("hanhuo_num").ToInt();
                    int? xinfu_num = req.GetPara("xinfu_num").ToInt();

                    CheckMethod(p_jixiao_day_session);

                    var zb = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(p_jixiao_day_session.zb_user_sn);

                    if (zb.ting_sn.IsNullOrEmpty())
                    {
                        throw new Exception("提交失败,没有查到主播所在厅的信息,请重试" + p_jixiao_day_session.zb_user_sn);
                    }

                    p_jixiao_day_session.tg_user_sn = zb.tg_user_sn;
                    p_jixiao_day_session.yy_user_sn = zb.yy_user_sn;
                    p_jixiao_day_session.ting_sn = zb.ting_sn;
                    //如果是新增状态
                    if (p_jixiao_day_session.id <= 0)
                    {
                        p_jixiao_day_session.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id != new DomainBasic.UserTypeApp().GetInfoByCode("zber").id && new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id != new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                        {
                            throw new Exception("当前用户类型禁止提交");
                        }

                        if (p_jixiao_day_session.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择日期");
                    }
                    //如果是编辑状态
                    else
                    {
                        var entity = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(p_jixiao_day_session.id);
                        //校验数据
                        if (entity.id <= 0)
                        {
                            throw new Exception("此提报已经不存在,不能修改");
                        }
                        switch (new ServiceFactory.UserInfo().GetUserType())
                        {
                            case ModelEnum.UserTypeEnum.zber:
                                if (entity.zb_user_sn != new UserIdentityBag().user_sn) throw new WeicodeException("无修改权限");
                                break;
                            case ModelEnum.UserTypeEnum.tger:
                                if (new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, entity.zb_user_sn) != new UserIdentityBag().user_sn)
                                {
                                    throw new WeicodeException("无修改权限");
                                }
                                break;
                            default:
                                break;
                        }
                        if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("zber").id && !DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"zb_user_sn = '{new UserIdentityBag().user_sn}' and c_date = '{p_jixiao_day_session.c_date}' and session = '{p_jixiao_day_session.session}' and create_time != modify_time", false).IsNullOrEmpty())
                        {
                            throw new Exception($"已修改过1次,禁止多次修改");
                        }
                        var e = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{entity.c_date}' and session='{p_jixiao_day_session.session}' and zb_user_sn = '{p_jixiao_day_session.zb_user_sn}' and id != '{p_jixiao_day_session.id}'", false);
                        if (e.id > 0)
                        {
                            throw new Exception($"主播:{new DomainBasic.UserApp().GetInfoByUserSn(entity.zb_user_sn).username},日期:{entity.c_date},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", entity.session.ToString())}提交的数据已经存在");
                        }
                        p_jixiao_day_session.c_date = entity.c_date;

                        
                    }
                    p_jixiao_day_session.amount = p_jixiao_day_session.amount_1 + p_jixiao_day_session.amount_2 + p_jixiao_day_session.hx_amount+p_jixiao_day_session.hdpk_amount;

                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{p_jixiao_day_session.tg_user_sn}' and c_date='{p_jixiao_day_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_day_session.session}'", false);

                    //如果喊活与新付接收到的数值是0，则还保持之前填的值（hanhuo_num = null表示不修改）
                    if (!p_jixiao_day_session_total.hanhuo_num.IsNullOrEmpty() && hanhuo_num == 0) hanhuo_num = null;
                    if (!p_jixiao_day_session_total.xinfu_num.IsNullOrEmpty() && xinfu_num == 0) xinfu_num = null;
                    var old_jixiao = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(p_jixiao_day_session.id, false);
                    if (p_jixiao_day_session.InsertOrUpdate() > 0)
                    {
                        var sum = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(amount_1),sum(num_2),sum(amount_2),sum(old_amount),sum(amount),sum(contact_num),sum(datou_num),sum(new_num),sum(hdpk_amount),sum(hx_num),sum(hx_amount)", $"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                        
                        //新增状态
                        if (p_jixiao_day_session.id <= 0)
                        {
                            var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(p_jixiao_day_session.zb_user_sn);
                            new ModelDb.p_jixiao_day
                            {
                                c_date = p_jixiao_day_session.c_date,
                                tenant_id = p_jixiao_day_session.tenant_id,
                                zb_user_sn = p_jixiao_day_session.zb_user_sn,
                                tg_user_sn = p_jixiao_day_session.tg_user_sn,
                                ting_sn=zb.ting_sn,
                                yy_user_sn= p_jixiao_day_session.yy_user_sn,
                                amount_1 = sum[0].ToInt(),
                                num_2 = sum[1].ToInt(),
                                amount_2 = sum[2].ToInt(),
                                old_amount = sum[3].ToInt(),
                                amount = sum[4].ToInt(),
                                contact_num = sum[5].ToInt(),
                                datou_num = sum[6].ToInt(),
                                new_num = sum[7].ToInt(),
                                hdpk_amount = sum[8].ToInt(),
                                hx_num = sum[9].ToInt(),
                                hx_amount = sum[10].ToInt(),
                                job = zhubo.full_or_part,
                                is_newer = zhubo.is_newer.ToSByte(),
                                session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            }.InsertOrUpdate($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");

                            new DomainBasic.SystemBizLogApp().Write("数据提报", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"提交每日提报:(绩效日期:{p_jixiao_day_session.c_date.ToDate().ToString("yyyy年MM月dd日")},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", p_jixiao_day_session.session.ToString())},所属主播;{new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).username})");
                        }
                        //编辑状态
                        else
                        {
                            
                            new ModelDb.p_jixiao_day
                            {
                                ting_sn= zb.ting_sn,
                                amount_1 = sum[0].ToInt(),
                                num_2 = sum[1].ToInt(),
                                amount_2 = sum[2].ToInt(),
                                old_amount = sum[3].ToInt(),
                                amount = sum[4].ToInt(),
                                contact_num = sum[5].ToInt(),
                                datou_num = sum[6].ToInt(),
                                new_num = sum[7].ToInt(),
                                hdpk_amount = sum[8].ToInt(),
                                hx_num = sum[9].ToInt(),
                                hx_amount = sum[10].ToInt(),
                                session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            }.Update($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                            string content = "";
                            if (old_jixiao.session != p_jixiao_day_session.session) { content += $"原档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", old_jixiao.session.ToString())}修改后:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", p_jixiao_day_session.session.ToString())},"; }
                            if (old_jixiao.hanhuo_num1 != p_jixiao_day_session.hanhuo_num1) 
                            { 
                                content += $"原喊活人数:{old_jixiao.hanhuo_num1}修改后:{p_jixiao_day_session.hanhuo_num1},"; 
                            }
                            if (old_jixiao.new_pay_num1 != p_jixiao_day_session.new_pay_num1) 
                            { 
                                content += $"原新付费人数:{old_jixiao.new_pay_num1}修改后:{p_jixiao_day_session.new_pay_num1},"; 
                            }
                            if (old_jixiao.new_num != p_jixiao_day_session.new_num) 
                            { 
                                content += $"原拉新:{old_jixiao.new_num}修改后:{p_jixiao_day_session.new_num},"; 
                            }
                            if (old_jixiao.amount_1 != p_jixiao_day_session.amount_1) 
                            { 
                                content += $"原首消音浪:{old_jixiao.amount_1}修改后:{p_jixiao_day_session.amount_1},"; 
                            }
                            if (old_jixiao.num_2 != p_jixiao_day_session.num_2) 
                            { 
                                content += $"原二消个数:{old_jixiao.num_2}修改后:{p_jixiao_day_session.num_2},"; 
                            }
                            if (old_jixiao.amount_2 != p_jixiao_day_session.amount_2) 
                            { 
                                content += $"原二消音浪:{old_jixiao.amount_2}修改后:{p_jixiao_day_session.amount_2},"; 
                            }
                            if (old_jixiao.hx_amount != p_jixiao_day_session.hx_amount) 
                            { 
                                content += $"原老用户音浪:{old_jixiao.hx_amount}修改后:{p_jixiao_day_session.hx_amount},"; 
                            }
                            if (old_jixiao.contact_num != p_jixiao_day_session.contact_num) 
                            { 
                                content += $"原建联数:{old_jixiao.contact_num}修改后:{p_jixiao_day_session.contact_num},"; 
                            }
                            if (old_jixiao.hdpk_amount != p_jixiao_day_session.hdpk_amount) 
                            { 
                                content += $"原活动PK音浪:{old_jixiao.hdpk_amount}修改后:{p_jixiao_day_session.hdpk_amount},"; 
                            }
                            if (old_jixiao.datou_num != p_jixiao_day_session.datou_num) 
                            { 
                                content += $"原大头数:{old_jixiao.datou_num}修改后:{p_jixiao_day_session.datou_num},"; 
                            }
                            
                            new ModelDb.sys_biz_log()
                            {
                                modular_function = "数据提报",
                                log_type = ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(),
                                user_sn = new UserIdentityBag().user_sn,
                                user_type_id = new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id,
                                memo = $"修改提报数据(id:{old_jixiao.id},绩效日期:{old_jixiao.c_date.ToDate().ToString("yyyy年MM月dd日")},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", old_jixiao.session.ToString())},所属主播:{new DomainBasic.UserApp().GetInfoByUserSn(old_jixiao.zb_user_sn).username})",
                                info = content,
                            }.Insert();
                        }

                        //场次-喊活数据
                        new ModelDb.p_jixiao_day_session_total
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            tg_user_sn = p_jixiao_day_session.tg_user_sn,
                            ting_sn=zb.ting_sn,
                            c_date = p_jixiao_day_session.c_date,
                            session = p_jixiao_day_session.session,
                            hanhuo_num = hanhuo_num,
                            xinfu_num = xinfu_num
                        }.InsertOrUpdate($"tg_user_sn='{p_jixiao_day_session.tg_user_sn}' and c_date='{p_jixiao_day_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_day_session.session}'");
                    }
                    else
                    {
                        throw new WeicodeException("提交失败");
                    }
                    return result;
                }

                /// <summary>
                /// 提交基础校验
                /// </summary>
                /// <param name="p_jixiao_day_session"></param>
                private void CheckMethod(ModelDb.p_jixiao_day_session p_jixiao_day_session)
                {
                    if (new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).IsNullOrEmpty())
                    {
                        throw new Exception("主播不存在或未选择主播");
                    }
                    if (new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("主播账号已被删除，无法提交");
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("当前账号已被删除，无法提交");
                    if (p_jixiao_day_session.c_date > DateTime.Today) throw new WeicodeException("所选日期不能超过今日");
                    if (p_jixiao_day_session.session.IsNullOrEmpty()) throw new WeicodeException("请选择时间段");

                    var session = new DomainBasic.DictionaryApp().GetKeyFromValue("场次", p_jixiao_day_session.session.ToString());
                    var session_last_time = session.Substring(session.IndexOf("-") + 1);
                    session_last_time = session_last_time.Substring(0, session_last_time.IndexOf(":"));
                    var p_session = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and session='{p_jixiao_day_session.session}' and id != '{p_jixiao_day_session.id}'", false);
                    if (!p_session.IsNullOrEmpty()) throw new WeicodeException($"该时间段:{new DomainBasic.DictionaryApp().GetKeyFromValue("场次", p_session.session.ToString())}已提交过,提交人:{new DomainBasic.UserApp().GetInfoByUserSn(p_session.zb_user_sn).name},所属厅管:{new DomainBasic.UserApp().GetInfoByUserSn(p_session.tg_user_sn).name},所属厅:{new ServiceFactory.UserInfo.Ting().GetTingBySn(p_session.ting_sn).ting_name},提交时间:{p_session.create_time}");

                    if (DateTime.Now.ToString("HH").ToInt() < session_last_time.ToInt() && p_jixiao_day_session.c_date == DateTime.Today && DateTime.Now < DateTime.Today.AddDays(1).AddMinutes(-10)) throw new Exception("所选场次时间还未结束，当前无法提交");
                    if (p_jixiao_day_session.hanhuo_num1 == null) p_jixiao_day_session.hanhuo_num1 = 0;
                    if (p_jixiao_day_session.new_pay_num1 == null) p_jixiao_day_session.new_pay_num1 = 0;
                    if (p_jixiao_day_session.new_num == null) p_jixiao_day_session.new_num = 0;
                    if (p_jixiao_day_session.amount_1 == null) p_jixiao_day_session.amount_1 = 0;
                    if (p_jixiao_day_session.num_2 == null) p_jixiao_day_session.num_2 = 0;
                    if (p_jixiao_day_session.amount_2 == null) p_jixiao_day_session.amount_2 = 0;
                    if (p_jixiao_day_session.contact_num == null) p_jixiao_day_session.contact_num = 0;
                    if (p_jixiao_day_session.hx_num == null) p_jixiao_day_session.hx_num = 0;
                    if (p_jixiao_day_session.hx_amount == null) p_jixiao_day_session.hx_amount = 0;
                    if (p_jixiao_day_session.hdpk_amount == null) p_jixiao_day_session.hdpk_amount = 0;
                    if (p_jixiao_day_session.datou_num == null) p_jixiao_day_session.datou_num = 0;

                    if (p_jixiao_day_session.new_num > 30) { throw new Exception("拉新数不能超过30"); }
                    if (p_jixiao_day_session.contact_num > 30) { throw new Exception("建联数不能超过30"); }
                    if (p_jixiao_day_session.num_2 > 30) { throw new Exception("二消个数不能超过30"); }
                    if (p_jixiao_day_session.hx_num > 30) { throw new Exception(" 回消人数不能超过30"); }
                    p_jixiao_day_session.new_num.ToInt("拉新数量不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.amount_1.ToInt("首消音浪值不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.num_2.ToInt("二消个数不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hx_num.ToInt("老用户人数不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hx_amount.ToInt("老用户音浪不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hdpk_amount.ToInt("活动PK音浪不可小于0", ConvertExt.IntType.非负整数);
                    if (p_jixiao_day_session.num_2 > p_jixiao_day_session.new_num) { throw new Exception("二消个数必须小于等于拉新数"); }
                    p_jixiao_day_session.amount_2.ToInt("二消音浪值不可小于0", ConvertExt.IntType.非负整数);
                    //p_jixiao_day_session.old_amount=p_jixiao_day_session.old_amount.ToInt("老客户音浪值不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.contact_num.ToInt("建联数不可小于0", ConvertExt.IntType.非负整数);
                }

                /// <summary>
                /// 额外请求参数
                /// </summary>
                public class ReqDto
                {
                    /// <summary>
                    /// 今日总结
                    /// </summary>
                    public string summary_demo { get; set; }
                }
                #endregion

            }

            public class UserDayReportFastPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.style = @"background-image:url('/Assets/images/qgxkt_m.jpg');background-size: cover;background-position: center; background-repeat: no-repeat;";
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };


                    /*
                     pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            left = 30
                        },
                        emtModelBase = new EmtHtml("feedback")
                        {
                            Content = @"<a href=""/Service/FeedBack/Post"" style=""color:#3399FF;"">匿名反馈</a>",
                        }
                    });
                     */

                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer2")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            left = 100
                        },
                        emtModelBase = new EmtHtml("CreateUser")
                        {
                            Content = @"<a href=""/pcrm/pcrm/crmpost"" style=""color:#3399FF;"">新建用户</a>",
                        }
                    });

                    pageModel.adjuncts.Add(new ModelBasic.AdjFloatLayer("floatlayer3")
                    {
                        height = "100px",
                        style = " border:5px solid #F0F0F0; background-color:#FDFDFD; padding:10px;",
                        position = ModelBasic.AdjFloatLayer.Position.固定定位,
                        positionRelative = new ModelBasic.AdjFloatLayer.PositionRelative
                        {
                            className = "post_submit_post",
                            offset_x = 20,
                            offset_y = 20
                        },
                        positionFixed = new ModelBasic.AdjFloatLayer.PositionFixed
                        {
                            display = ModelBasic.AdjFloatLayer.Display.none,
                            bottom = 40,
                            left = 10
                        },
                        emtModelBase = new ModelBasic.EmtGrid("Confirm")
                        {
                            items = new List<ModelBasic.EmtGrid.Item>
                        {
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=12,
                                emtModelBase=new ModelBasic.EmtLabel("tip")
                                {
                                    width="50px",
                                    defaultValue="当前是否为挂麦?",
                                    displayStatus= EmtModelBase.DisplayStatus.只读
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("true")
                                {
                                    width="50px",
                                    defaultValue="是",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code = "page_post.is_guamai.set('1')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("false")
                                {
                                    width="50px",
                                    defaultValue="否",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code="page_post.is_guamai.set('0')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtButton("cancle")
                                {
                                    width="200px",
                                    defaultValue="取消",
                                    eventJsChange=new EmtFormBase.EventJsChange
                                    {
                                        eventJavascript=new EventJavascript
                                        {
                                            code="page.floatlayer3.hide();"
                                        }
                                    }
                                }
                            }
                        }
                        }
                    });

                    return pageModel;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("bg");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b1")
                    {
                        text = "提交记录",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/JixiaoDay/Report/List",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b2")
                    {
                        text = "首页",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                        eventToUrl = new EmtModel.ButtonItem.EventToUrl
                        {
                            url = "/Default/MainPage/Index",
                            target = "top"
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b3")
                    {
                        text = "新增用户",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                        eventToUrl = new EmtModel.ButtonItem.EventToUrl
                        {
                            url = "/PCrm/PCrm/CrmPost",
                            target = "top"
                        }
                    });

                    return buttonGroup;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_jixiao_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.id, false);
                    var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{p_jixiao_session.zb_user_sn}' and c_date='{p_jixiao_session.c_date.ToDate().ToString("yyyy-MM-dd")}'", false);

                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{req.tg_user_sn}' and c_date='{p_jixiao_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_session.session}'", false);
                    if (p_jixiao_day.IsNullOrEmpty())
                    {
                        p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{new UserIdentityBag().user_sn}' and c_date='{DateTime.Today.ToString("yyyy-MM-dd")}'", false);
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("is_guamai")
                    {
                        defaultValue = "0"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtButton("show_tips")
                    {
                        defaultValue = "查看提报规则",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript()
                            {
                                code = "confirm(`二消定义：99拉新用户在本档内第二次及多次的消费音浪值（如果二消用户送了全麦，拉新主播的拉新个数+1，二消个数+1。其余主播拉新个数不增加，二消个数不增加，音浪值归属到老用户）\r\n \r\n喊活、新付报量规则：系统是以最后一个提报喊活新付数据的主播为准（例：主播A首先提报了3喊活、3新付，主播B随后提报了2喊活、2新付那这个档上实际喊活新付以主播B提交的数据为准）喊活、新付每个档最好由一个人统计提报喊活新付总数，其他人不填`);"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTextarea("zb")
                    {
                        title = "所属主播",
                        style = "background-color: transparent;",
                        defaultValue = p_jixiao_session.zb_user_sn.IsNullOrEmpty() ? new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).username : new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_session.zb_user_sn).username,
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("zb_user_sn")
                    {
                        title = "主播user_sn",
                        defaultValue = req.zb_user_sn
                    });


                    var colItems = new List<ModelBasic.EmtHandExcelRead.ColItem>();
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "上档人数",
                        width = "220",
                        edit = "true"
                    });
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "喊活人数",
                        width = "220",
                        edit = "true"
                    });
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "新付费人数",
                        width = "220",
                        edit = "true"
                    });
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "拉新总数",
                        width = "220",
                        edit = "true"
                    });
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "建联人数",
                        width = "220",
                        edit = "true"
                    });
                    colItems.Add(new ModelBasic.EmtHandExcelRead.ColItem("amount1")
                    {
                        title = "总音浪",
                        width = "220",
                        edit = "true"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHandExcelRead("l_pack")
                    {
                        title = "上传",
                        titleSelect = "选择表格",
                        titleCopy = "手动快捷上报",
                        titleDown = "表格上报模版下载",
                        colItems = colItems
                    });

                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public int id { get; set; } = 0;
                    public string zb_user_sn { get; set; }
                    public string tg_user_sn { get; set; }
                }

                /// <summary>
                /// 获取主播提交记录
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZbSessionInfo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();

                    var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"c_date='{req["c_date"].ToNullableString()}' and zb_user_sn='{req["zb_user_sn"].ToNullableString()}'", false);
                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and c_date='{req["c_date"].ToNullableString()}' and session='{req["session"].ToNullableString()}'", false);
                    result.data = new
                    {
                        amount_1 = (p_jixiao_day.amount_1.IsNullOrEmpty() ? 0 : p_jixiao_day.amount_1),
                        num_2 = (p_jixiao_day.num_2.IsNullOrEmpty() ? 0 : p_jixiao_day.num_2),
                        amount_2 = (p_jixiao_day.amount_2.IsNullOrEmpty() ? 0 : p_jixiao_day.amount_2),
                        old_amount = (p_jixiao_day.old_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.old_amount),
                        hdpk_amount = (p_jixiao_day.hdpk_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.hdpk_amount),
                        hx_amount = (p_jixiao_day.hx_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.hx_amount),
                        hx_num = (p_jixiao_day.hx_num.IsNullOrEmpty() ? 0 : p_jixiao_day.hx_num),
                        new_num = (p_jixiao_day.new_num.IsNullOrEmpty() ? 0 : p_jixiao_day.new_num),
                        contact_num = (p_jixiao_day.contact_num.IsNullOrEmpty() ? 0 : p_jixiao_day.contact_num),
                        datou_num = (p_jixiao_day.datou_num.IsNullOrEmpty() ? 0 : p_jixiao_day.datou_num),
                        summary_demo = (p_jixiao_day.summary_demo.IsNullOrEmpty() ? "" : p_jixiao_day.summary_demo),
                        hanhuo_num = (p_jixiao_day_session_total.hanhuo_num.IsNullOrEmpty() ? 0 : p_jixiao_day_session_total.hanhuo_num),
                        xinfu_num = (p_jixiao_day_session_total.xinfu_num.IsNullOrEmpty() ? 0 : p_jixiao_day_session_total.xinfu_num)
                    };
                    return result;
                }

                #endregion
                #region 异步请求处理
                /// <summary>
                /// 新增每日上报
                /// </summary>
                /// <param name="req">1.主播提交每日数据;2.主播修改每日数据;3.厅管修改下级主播每日数据</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_jixiao_day_session = req.GetPara().ToModel<ModelDb.p_jixiao_day_session>();

                    int? hanhuo_num = req.GetPara("hanhuo_num").ToInt();
                    int? xinfu_num = req.GetPara("xinfu_num").ToInt();

                    CheckMethod(p_jixiao_day_session);

                    var zb = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(p_jixiao_day_session.zb_user_sn);

                    if (zb.ting_sn.IsNullOrEmpty())
                    {
                        throw new Exception("提交失败,没有查到主播所在厅的信息,请重试" + p_jixiao_day_session.zb_user_sn);
                    }

                    p_jixiao_day_session.tg_user_sn = zb.tg_user_sn;
                    p_jixiao_day_session.yy_user_sn = zb.yy_user_sn;
                    p_jixiao_day_session.ting_sn = zb.ting_sn;
                    //如果是新增状态
                    if (p_jixiao_day_session.id <= 0)
                    {
                        p_jixiao_day_session.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id != new DomainBasic.UserTypeApp().GetInfoByCode("zber").id && new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id != new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                        {
                            throw new Exception("当前用户类型禁止提交");
                        }

                        if (p_jixiao_day_session.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择日期");
                    }
                    //如果是编辑状态
                    else
                    {
                        var entity = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(p_jixiao_day_session.id);
                        //校验数据
                        if (entity.id <= 0)
                        {
                            throw new Exception("此提报已经不存在,不能修改");
                        }
                        switch (new ServiceFactory.UserInfo().GetUserType())
                        {
                            case ModelEnum.UserTypeEnum.zber:
                                if (entity.zb_user_sn != new UserIdentityBag().user_sn) throw new WeicodeException("无修改权限");
                                break;
                            case ModelEnum.UserTypeEnum.tger:
                                if (new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, entity.zb_user_sn) != new UserIdentityBag().user_sn)
                                {
                                    throw new WeicodeException("无修改权限");
                                }
                                break;
                            default:
                                break;
                        }
                        if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("zber").id && !DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"zb_user_sn = '{new UserIdentityBag().user_sn}' and c_date = '{p_jixiao_day_session.c_date}' and session = '{p_jixiao_day_session.session}' and create_time != modify_time", false).IsNullOrEmpty())
                        {
                            throw new Exception($"已修改过1次,禁止多次修改");
                        }
                        var e = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{entity.c_date}' and session='{p_jixiao_day_session.session}' and zb_user_sn = '{p_jixiao_day_session.zb_user_sn}' and id != '{p_jixiao_day_session.id}'", false);
                        if (e.id > 0)
                        {
                            throw new Exception($"主播:{new DomainBasic.UserApp().GetInfoByUserSn(entity.zb_user_sn).username},日期:{entity.c_date},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", entity.session.ToString())}提交的数据已经存在");
                        }
                        p_jixiao_day_session.c_date = entity.c_date;


                    }
                    p_jixiao_day_session.amount = p_jixiao_day_session.amount_1 + p_jixiao_day_session.amount_2 + p_jixiao_day_session.hx_amount + p_jixiao_day_session.hdpk_amount;

                    var p_jixiao_day_session_total = DoMySql.FindEntity<ModelDb.p_jixiao_day_session_total>($"tg_user_sn='{p_jixiao_day_session.tg_user_sn}' and c_date='{p_jixiao_day_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_day_session.session}'", false);

                    //如果喊活与新付接收到的数值是0，则还保持之前填的值（hanhuo_num = null表示不修改）
                    if (!p_jixiao_day_session_total.hanhuo_num.IsNullOrEmpty() && hanhuo_num == 0) hanhuo_num = null;
                    if (!p_jixiao_day_session_total.xinfu_num.IsNullOrEmpty() && xinfu_num == 0) xinfu_num = null;
                    var old_jixiao = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(p_jixiao_day_session.id, false);
                    if (p_jixiao_day_session.InsertOrUpdate() > 0)
                    {
                        var sum = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(amount_1),sum(num_2),sum(amount_2),sum(old_amount),sum(amount),sum(contact_num),sum(datou_num),sum(new_num),sum(hdpk_amount),sum(hx_num),sum(hx_amount)", $"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");

                        //新增状态
                        if (p_jixiao_day_session.id <= 0)
                        {
                            var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(p_jixiao_day_session.zb_user_sn);
                            new ModelDb.p_jixiao_day
                            {
                                c_date = p_jixiao_day_session.c_date,
                                tenant_id = p_jixiao_day_session.tenant_id,
                                zb_user_sn = p_jixiao_day_session.zb_user_sn,
                                tg_user_sn = p_jixiao_day_session.tg_user_sn,
                                ting_sn = zb.ting_sn,
                                yy_user_sn = p_jixiao_day_session.yy_user_sn,
                                amount_1 = sum[0].ToInt(),
                                num_2 = sum[1].ToInt(),
                                amount_2 = sum[2].ToInt(),
                                old_amount = sum[3].ToInt(),
                                amount = sum[4].ToInt(),
                                contact_num = sum[5].ToInt(),
                                datou_num = sum[6].ToInt(),
                                new_num = sum[7].ToInt(),
                                hdpk_amount = sum[8].ToInt(),
                                hx_num = sum[9].ToInt(),
                                hx_amount = sum[10].ToInt(),
                                job = zhubo.full_or_part,
                                is_newer = zhubo.is_newer.ToSByte(),
                                session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            }.InsertOrUpdate($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");

                            new DomainBasic.SystemBizLogApp().Write("数据提报", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"提交每日提报:(绩效日期:{p_jixiao_day_session.c_date.ToDate().ToString("yyyy年MM月dd日")},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", p_jixiao_day_session.session.ToString())},所属主播;{new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).username})");
                        }
                        //编辑状态
                        else
                        {

                            new ModelDb.p_jixiao_day
                            {
                                ting_sn = zb.ting_sn,
                                amount_1 = sum[0].ToInt(),
                                num_2 = sum[1].ToInt(),
                                amount_2 = sum[2].ToInt(),
                                old_amount = sum[3].ToInt(),
                                amount = sum[4].ToInt(),
                                contact_num = sum[5].ToInt(),
                                datou_num = sum[6].ToInt(),
                                new_num = sum[7].ToInt(),
                                hdpk_amount = sum[8].ToInt(),
                                hx_num = sum[9].ToInt(),
                                hx_amount = sum[10].ToInt(),
                                session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            }.Update($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                            string content = "";
                            if (old_jixiao.session != p_jixiao_day_session.session) { content += $"原档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", old_jixiao.session.ToString())}修改后:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", p_jixiao_day_session.session.ToString())},"; }
                            if (old_jixiao.hanhuo_num1 != p_jixiao_day_session.hanhuo_num1)
                            {
                                content += $"原喊活人数:{old_jixiao.hanhuo_num1}修改后:{p_jixiao_day_session.hanhuo_num1},";
                            }
                            if (old_jixiao.new_pay_num1 != p_jixiao_day_session.new_pay_num1)
                            {
                                content += $"原新付费人数:{old_jixiao.new_pay_num1}修改后:{p_jixiao_day_session.new_pay_num1},";
                            }
                            if (old_jixiao.new_num != p_jixiao_day_session.new_num)
                            {
                                content += $"原拉新:{old_jixiao.new_num}修改后:{p_jixiao_day_session.new_num},";
                            }
                            if (old_jixiao.amount_1 != p_jixiao_day_session.amount_1)
                            {
                                content += $"原首消音浪:{old_jixiao.amount_1}修改后:{p_jixiao_day_session.amount_1},";
                            }
                            if (old_jixiao.num_2 != p_jixiao_day_session.num_2)
                            {
                                content += $"原二消个数:{old_jixiao.num_2}修改后:{p_jixiao_day_session.num_2},";
                            }
                            if (old_jixiao.amount_2 != p_jixiao_day_session.amount_2)
                            {
                                content += $"原二消音浪:{old_jixiao.amount_2}修改后:{p_jixiao_day_session.amount_2},";
                            }
                            if (old_jixiao.hx_amount != p_jixiao_day_session.hx_amount)
                            {
                                content += $"原老用户音浪:{old_jixiao.hx_amount}修改后:{p_jixiao_day_session.hx_amount},";
                            }
                            if (old_jixiao.contact_num != p_jixiao_day_session.contact_num)
                            {
                                content += $"原建联数:{old_jixiao.contact_num}修改后:{p_jixiao_day_session.contact_num},";
                            }
                            if (old_jixiao.hdpk_amount != p_jixiao_day_session.hdpk_amount)
                            {
                                content += $"原活动PK音浪:{old_jixiao.hdpk_amount}修改后:{p_jixiao_day_session.hdpk_amount},";
                            }
                            if (old_jixiao.datou_num != p_jixiao_day_session.datou_num)
                            {
                                content += $"原大头数:{old_jixiao.datou_num}修改后:{p_jixiao_day_session.datou_num},";
                            }

                            new ModelDb.sys_biz_log()
                            {
                                modular_function = "数据提报",
                                log_type = ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(),
                                user_sn = new UserIdentityBag().user_sn,
                                user_type_id = new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id,
                                memo = $"修改提报数据(id:{old_jixiao.id},绩效日期:{old_jixiao.c_date.ToDate().ToString("yyyy年MM月dd日")},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", old_jixiao.session.ToString())},所属主播:{new DomainBasic.UserApp().GetInfoByUserSn(old_jixiao.zb_user_sn).username})",
                                info = content,
                            }.Insert();
                        }

                        //场次-喊活数据
                        new ModelDb.p_jixiao_day_session_total
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            tg_user_sn = p_jixiao_day_session.tg_user_sn,
                            ting_sn = zb.ting_sn,
                            c_date = p_jixiao_day_session.c_date,
                            session = p_jixiao_day_session.session,
                            hanhuo_num = hanhuo_num,
                            xinfu_num = xinfu_num
                        }.InsertOrUpdate($"tg_user_sn='{p_jixiao_day_session.tg_user_sn}' and c_date='{p_jixiao_day_session.c_date.ToDate().ToString("yyyy-MM-dd")}' and session='{p_jixiao_day_session.session}'");
                    }
                    else
                    {
                        throw new WeicodeException("提交失败");
                    }
                    return result;
                }

                /// <summary>
                /// 提交基础校验
                /// </summary>
                /// <param name="p_jixiao_day_session"></param>
                private void CheckMethod(ModelDb.p_jixiao_day_session p_jixiao_day_session)
                {
                    if (new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).IsNullOrEmpty())
                    {
                        throw new Exception("主播不存在或未选择主播");
                    }
                    if (new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("主播账号已被删除，无法提交");
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("当前账号已被删除，无法提交");
                    if (p_jixiao_day_session.c_date > DateTime.Today) throw new WeicodeException("所选日期不能超过今日");
                    if (p_jixiao_day_session.session.IsNullOrEmpty()) throw new WeicodeException("请选择时间段");

                    var session = new DomainBasic.DictionaryApp().GetKeyFromValue("场次", p_jixiao_day_session.session.ToString());
                    var session_last_time = session.Substring(session.IndexOf("-") + 1);
                    session_last_time = session_last_time.Substring(0, session_last_time.IndexOf(":"));
                    var p_session = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and session='{p_jixiao_day_session.session}' and id != '{p_jixiao_day_session.id}'", false);
                    if (!p_session.IsNullOrEmpty()) throw new WeicodeException($"该时间段:{new DomainBasic.DictionaryApp().GetKeyFromValue("场次", p_session.session.ToString())}已提交过,提交人:{new DomainBasic.UserApp().GetInfoByUserSn(p_session.zb_user_sn).name},所属厅管:{new DomainBasic.UserApp().GetInfoByUserSn(p_session.tg_user_sn).name},所属厅:{new ServiceFactory.UserInfo.Ting().GetTingBySn(p_session.ting_sn).ting_name},提交时间:{p_session.create_time}");

                    if (DateTime.Now.ToString("HH").ToInt() < session_last_time.ToInt() && p_jixiao_day_session.c_date == DateTime.Today && DateTime.Now < DateTime.Today.AddDays(1).AddMinutes(-10)) throw new Exception("所选场次时间还未结束，当前无法提交");
                    if (p_jixiao_day_session.hanhuo_num1 == null) p_jixiao_day_session.hanhuo_num1 = 0;
                    if (p_jixiao_day_session.new_pay_num1 == null) p_jixiao_day_session.new_pay_num1 = 0;
                    if (p_jixiao_day_session.new_num == null) p_jixiao_day_session.new_num = 0;
                    if (p_jixiao_day_session.amount_1 == null) p_jixiao_day_session.amount_1 = 0;
                    if (p_jixiao_day_session.num_2 == null) p_jixiao_day_session.num_2 = 0;
                    if (p_jixiao_day_session.amount_2 == null) p_jixiao_day_session.amount_2 = 0;
                    if (p_jixiao_day_session.contact_num == null) p_jixiao_day_session.contact_num = 0;
                    if (p_jixiao_day_session.hx_num == null) p_jixiao_day_session.hx_num = 0;
                    if (p_jixiao_day_session.hx_amount == null) p_jixiao_day_session.hx_amount = 0;
                    if (p_jixiao_day_session.hdpk_amount == null) p_jixiao_day_session.hdpk_amount = 0;
                    if (p_jixiao_day_session.datou_num == null) p_jixiao_day_session.datou_num = 0;

                    if (p_jixiao_day_session.new_num > 30) { throw new Exception("拉新数不能超过30"); }
                    if (p_jixiao_day_session.contact_num > 30) { throw new Exception("建联数不能超过30"); }
                    if (p_jixiao_day_session.num_2 > 30) { throw new Exception("二消个数不能超过30"); }
                    if (p_jixiao_day_session.hx_num > 30) { throw new Exception(" 回消人数不能超过30"); }
                    p_jixiao_day_session.new_num.ToInt("拉新数量不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.amount_1.ToInt("首消音浪值不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.num_2.ToInt("二消个数不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hx_num.ToInt("老用户人数不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hx_amount.ToInt("老用户音浪不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.hdpk_amount.ToInt("活动PK音浪不可小于0", ConvertExt.IntType.非负整数);
                    if (p_jixiao_day_session.num_2 > p_jixiao_day_session.new_num) { throw new Exception("二消个数必须小于等于拉新数"); }
                    p_jixiao_day_session.amount_2.ToInt("二消音浪值不可小于0", ConvertExt.IntType.非负整数);
                    //p_jixiao_day_session.old_amount=p_jixiao_day_session.old_amount.ToInt("老客户音浪值不可小于0", ConvertExt.IntType.非负整数);
                    p_jixiao_day_session.contact_num.ToInt("建联数不可小于0", ConvertExt.IntType.非负整数);
                }

                /// <summary>
                /// 额外请求参数
                /// </summary>
                public class ReqDto
                {
                    /// <summary>
                    /// 今日总结
                    /// </summary>
                    public string summary_demo { get; set; }
                }
                #endregion

            }

        }
        

    }
}
