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
    /// 规则
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 对战规则列表
            /// <summary>
            /// 
            /// </summary>
            public class RuleList
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
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
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
                        text = "创建对战规则",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"Post",
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
                    var listDisplay = new ModelBasic.CtlListDisplay(req);
                    listDisplay.operateWidth = "360";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("rule_type_txt")
                    {
                        text = "规则类型",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn1_name")
                    {
                        text = "发起厅",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn2_name")
                    {
                        text = "对方厅",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    #region 批量操作列
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
                        }
                    }
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        },
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {

                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and tingzhan_id = {new ServiceFactory.TingZhanService().getNewTingzhan().id}";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_mate_rule, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {

                    public string c_date { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_tingzhan_mate_rule
                {
                    public string rule_type_txt
                    {
                        get
                        {
                            return ((rule_type_enum)rule_type).ToString();
                        }
                    }
                    public string ting_sn1_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                        }
                    }
                    public string ting_sn2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                        }
                    }
                    public ModelDb.p_tingzhan p_tingzhan
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_tingzhan>($"id = {tingzhan_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #region 删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_tingzhan_mate_rule_item = req.data_json.ToModel<ModelDb.p_tingzhan_mate_rule>();
                    lSql.Add(p_tingzhan_mate_rule_item.DeleteTran($"id in ({p_tingzhan_mate_rule_item.id})"));
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
                    var p_tingzhan_mate_rule = new ModelDb.p_tingzhan_mate_rule();
                    lSql.Add(p_tingzhan_mate_rule.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_tingzhan_mate_rule
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }
            #endregion

            #region 创建对战规则
            /// <summary>
            /// 创建页面
            /// </summary>
            public class RulePost
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
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    return pageModel;
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
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("rule_type")
                    {
                        title = "规则类型",
                        options = new Dictionary<string, string> {
                        {"跟厅打",ModelDb.p_tingzhan_mate_rule.rule_type_enum.跟厅打.ToSByte().ToString() },
                        {"不跟厅打",ModelDb.p_tingzhan_mate_rule.rule_type_enum.不跟厅打.ToSByte().ToString() },
                    }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn1").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn1")
                    {
                        title = "厅管账号",
                        options = new Dictionary<string, string> { },
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "tg_user_sn1","<%=page.tg_user_sn1.value%>"}
                                },
                                func = GetTings,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn1").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn1")
                    {
                        title = "发起厅",
                        options = new Dictionary<string, string> { }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("ting_sn2s")
                    {
                        title = "对方厅",
                        options = DoMySql.FindKvList<ModelDb.user_info_tg>($"tenant_id={new DomainBasic.TenantApp().GetInfo().id}", "ting_name,ting_sn")
                    });
                    #endregion
                    return formDisplay;
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
                    result.data = new ServiceFactory.RelationService().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 获取厅筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTings(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(req["tg_user_sn1"].ToNullableString()).ToJson();
                    return result;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_tingzhan_mate_rule = req.data_json.ToModel<DtoReqData>();
                    // 取最近的厅战
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                    if (p_tingzhan.IsNullOrEmpty()) throw new Exception("厅战不存在");
                    if (p_tingzhan_mate_rule.rule_type.IsNullOrEmpty()) throw new Exception("请选择规则类型");
                    if (p_tingzhan_mate_rule.tg_user_sn1.IsNullOrEmpty()) throw new Exception("请选择厅管账号");
                    if (p_tingzhan_mate_rule.ting_sn1.IsNullOrEmpty()) throw new Exception("请选择发起厅");
                    if (p_tingzhan_mate_rule.ting_sn2s.IsNullOrEmpty()) throw new Exception("请选择对手厅");

                    List<string> lSql = new List<string>();
                    foreach (var ting_sn2 in p_tingzhan_mate_rule.ting_sn2s.Split(','))
                    {
                        lSql.Add(new ModelDb.p_tingzhan_mate_rule
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            tingzhan_id = p_tingzhan.id,
                            rule_type = p_tingzhan_mate_rule.rule_type,
                            tg_user_sn1 = p_tingzhan_mate_rule.tg_user_sn1,
                            ting_sn1 = p_tingzhan_mate_rule.ting_sn1,
                            tg_user_sn2 = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).tg_user_sn,
                            ting_sn2 = ting_sn2
                        }.InsertTran());
                    }

                    DoMySql.ExecuteSqlTran(lSql);

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_tingzhan_mate_rule
                {
                    public string ting_sn2s { get; set; }
                }

                #endregion
            }
            #endregion

            #region 长期对战规则列表
            /// <summary>
            /// 
            /// </summary>
            public class RuleLongList
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
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
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
                        text = "厅不跟厅打",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"LongPost?type=" + ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟厅打.ToSByte().ToInt(),
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "厅不跟运营打",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"LongPost?type=" + ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟运营打.ToSByte().ToInt(),
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "运营不跟运营打",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"LongPost?type=" + ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.运营不跟运营打.ToSByte().ToInt(),
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
                    var listDisplay = new ModelBasic.CtlListDisplay(req);
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("rulelong_type_txt")
                    {
                        text = "规则类型",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn1_name")
                    {
                        text = "发起方",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn2_name")
                    {
                        text = "对方",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_txt")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "160",
                        sort = true
                    });
                    #region 批量操作列
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
                        }
                    }
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "LongPost",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "LongPost"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        },
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {

                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_mate_rulelong, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {

                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_tingzhan_mate_rulelong
                {
                    public string rulelong_type_txt
                    {
                        get
                        {
                            return ((rulelong_type_enum)rulelong_type).ToString();
                        }
                    }
                    public string user_sn1_name
                    {
                        get
                        {
                            switch (rulelong_type)
                            {
                                case (sbyte?)rulelong_type_enum.运营不跟运营打:
                                    return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_sn1).username;
                                default:
                                    // 厅
                                    return new ServiceFactory.UserInfo.Ting().GetTingBySn(user_sn1).ting_name;
                            }
                        }
                    }
                    public string user_sn2_name
                    {
                        get
                        {
                            switch (rulelong_type)
                            {
                                case (sbyte?)rulelong_type_enum.厅不跟厅打:
                                    return new ServiceFactory.UserInfo.Ting().GetTingBySn(user_sn2).ting_name;
                                default:
                                    // 运营
                                    return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_sn2).username;
                            }
                        }
                    }
                    public string status_txt
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #region 删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_tingzhan_mate_rulelong_item = req.data_json.ToModel<ModelDb.p_tingzhan_mate_rulelong>();
                    lSql.Add(p_tingzhan_mate_rulelong_item.DeleteTran($"id in ({p_tingzhan_mate_rulelong_item.id})"));
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
                    var p_tingzhan_mate_rulelong = new ModelDb.p_tingzhan_mate_rulelong();
                    lSql.Add(p_tingzhan_mate_rulelong.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_tingzhan_mate_rulelong
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }
            #endregion

            #region 创建长期对战规则
            /// <summary>
            /// 创建页面
            /// </summary>
            public class RuleLongPost
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
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    return pageModel;
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
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    var p_tingzhan_mate_rulelong = DoMySql.FindEntity<ModelDb.p_tingzhan_mate_rulelong>($"id = {req.id}", false);
                    if (!p_tingzhan_mate_rulelong.IsNullOrEmpty()) req.type = p_tingzhan_mate_rulelong.rulelong_type.ToInt();
                    var options1 = new Dictionary<string, string> { };
                    var options2 = new Dictionary<string, string> { };
                    switch (req.type)
                    {
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟厅打:
                            options1 = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter());
                            options2 = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter());
                            break;
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.厅不跟运营打:
                            options1 = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter());
                            options2 = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                        case (int)ModelDb.p_tingzhan_mate_rulelong.rulelong_type_enum.运营不跟运营打:
                            options1 = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            options2 = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                    }

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("rulelong_type")
                    {
                        defaultValue = req.type.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("user_sn1")
                    {
                        title = "发起方",
                        options = options1,
                        defaultValue = p_tingzhan_mate_rulelong.user_sn1
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("user_sn2")
                    {
                        title = "对方",
                        options = options2,
                        defaultValue = p_tingzhan_mate_rulelong.user_sn2
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("status")
                    {
                        title = "状态",
                        options = new Dictionary<string, string>
                    {
                        {"启用","1" },
                        {"禁用","0" },
                    },
                        defaultValue = p_tingzhan_mate_rulelong.status.IsNullOrEmpty() ? "1" : p_tingzhan_mate_rulelong.status.ToString()
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public int type { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_tingzhan_mate_rulelong = req.data_json.ToModel<ModelDb.p_tingzhan_mate_rulelong>();
                    if (p_tingzhan_mate_rulelong.user_sn1.IsNullOrEmpty()) throw new Exception("请选择发起方");
                    if (p_tingzhan_mate_rulelong.user_sn2.IsNullOrEmpty()) throw new Exception("请选择对方");

                    p_tingzhan_mate_rulelong.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    p_tingzhan_mate_rulelong.InsertOrUpdate();

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_tingzhan_mate_rulelong
                {

                }

                #endregion
            }
            #endregion
        }
    }
}
