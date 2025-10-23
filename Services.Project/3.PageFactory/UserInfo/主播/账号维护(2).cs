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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            /// <summary>
            /// 账号列表
            /// </summary>
            public class Zhubo_AccountList
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("PageList");
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
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
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.ting_sn.value%>"}
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
                    var listDisplay = new ModelBasic.CtlListDisplay(req);
                    listDisplay.operateWidth = "350";
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.isOpenNumbers = true;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "用户名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "用户昵称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "所属厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "所属厅管",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "所属运营",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "电话号码",
                        width = "120",
                        minWidth = "120"
                    });

                    #region 批量操作列

                    #endregion

                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Edit",
                            field_paras = "id"
                        },
                        style = "",
                        text = "编辑",
                        name = "Post"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "InfoEdit",
                            field_paras = "id"
                        },
                        style = "",
                        text = "背调信息",
                        name = "Post"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "离职",
                        title = "提示说明",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DeletesAction,
                            field_paras = "id"
                        }
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public FilterForm filterForm { get; set; } = new FilterForm();

                    /// <summary>
                    /// 筛选参数（自定义）
                    /// </summary>
                    public class FilterForm
                    {
                        /// <summary>
                        /// 关键词
                        /// </summary>
                        public string keyword { get; set; }
                    }
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
                    string where = $"status = '{ModelDb.user_base.status_enum.正常.ToSByte()}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zber").id}'";

                    //查询条件
                    string UserSql = $"";
                    var ZbBaseInfoFilter = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter();
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        UserSql = $@" and user_sn in ({new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForSql(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.运营,
                                UserSn = reqJson.GetPara("yy_user_sn")
                            }
                        })})";
                    }
                    if (!reqJson.GetPara("ting_sn").IsNullOrEmpty())
                    {
                        UserSql = $@" and user_sn in ({new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForSql(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                UserSn = reqJson.GetPara("ting_sn")
                            }
                        })})";
                    }
                    if (!reqJson.GetPara("zb_user_sn").IsNullOrEmpty())
                    {
                        UserSql = $" and user_sn = '{reqJson.GetPara("zb_user_sn")}'";
                    }
                    where += UserSql;

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_base
                {
                    public ServiceFactory.UserInfo.Zhubo.ZbBaseInfo zhuboinfo
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(this.user_sn);
                        }
                    }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string ting_name
                    {
                        get
                        {
                            try
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(zhuboinfo.ting_sn).ting_name;
                            }
                            catch
                            {
                                return "";
                            }
                        }
                    }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string tg_name
                    {
                        get
                        {
                            try
                            {
                                return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(zhuboinfo.tg_user_sn).username;
                            }
                            catch
                            {
                                return "";
                            }

                        }
                    }
                    /// <summary>
                    /// 
                    /// </summary>
                    public string yy_name
                    {
                        get
                        {
                            try
                            {
                                return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(zhuboinfo.yy_user_sn).username;
                            }
                            catch
                            {
                                return "";
                            }

                        }
                    }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 批量删除资产
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var user_base = new ModelDb.user_base();
                    user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                    lSql.Add(user_base.UpdateTran($"id = ({dtoReqData.id})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
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
                #endregion
            }


            public class Zhubo_InfoList
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
                    listFilter.isExport = true;
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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "100px",
                        placeholder = "昵称"
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
                    var buttonGroup = new ModelBasic.EmtButtonGroup("create");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("ExcelPost")
                    {
                        title = "ExcelPost",
                        text = "背调信息导入",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "ExcelPost"
                        },
                        disabled = true
                    });

                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Post")
                    {
                        title = "Post",
                        text = "创建账号",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Create"
                        },
                    });

                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("UnDel")
                    {
                        title = "UnDel",
                        text = "恢复账号",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "UnDel"
                        },
                        disabled = true
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
                    listDisplay.operateWidth = "350";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "昵称",
                        width = "120",
                        minWidth = "120"
                    });
                    if (req.isShowZbInfo)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zhiwu")
                        {
                            text = "职务",
                            width = "120",
                            minWidth = "120"
                        });
                    }
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "120",
                        minWidth = "120"
                    });

                    if (req.isShowTgInfo)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_text")
                        {
                            text = "所属运营",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                        {
                            text = "所属厅管",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                        {
                            text = "厅管名",
                            width = "120",
                            minWidth = "120"
                        });

                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username_text")
                        {
                            mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                            text = "抖音号",
                            width = "120",
                            minWidth = "120",
                            fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                            {
                                width = "200px",
                                fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"dou_username", "dou_username_text" },
                                {"wechat_username", "wechat_username_text" },
                                {"tg_sex", "tg_sex_text" },
                                {"UID", "UID_text" },
                            },
                                emtModelBase = new ModelBasic.EmtGrid("grid")
                                {
                                    items = new List<ModelBasic.EmtGrid.Item>
                                {
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=12,
                                        emtModelBase = new ModelBasic.EmtInput($"dou_username")
                                        {
                                            width = "120",
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("submit_dou")
                                        {
                                            defaultValue = "提交",
                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                                {
                                                    attachPara=new Dictionary<string, object>
                                                    {
                                                        {"id","<%=page.focus.attr('data-id')%>" },
                                                        {"Index","0"},
                                                        {"Value","<%=page.dou_username.value%>"},
                                                    },
                                                    resCallJs="page.focus.text(page.dou_username.value);$('.floatlayer_div').hide();",
                                                    func=new ServiceFactory.UserService().TgInfoEdit
                                                }
                                            }
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("cancel_dou")
                                        {
                                            defaultValue = "取消",

                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventJavascript=new EventJavascript
                                                {
                                                    code="$('.floatlayer_div').hide();"
                                                }
                                            }
                                        }
                                    }
                                }
                                },
                                eventJsShow = new EventJsBasic
                                {
                                    eventJavascript = new EventJavascript
                                    {
                                        code = "page.dou_username.set(page.focus.attr('data-dou_username'));"
                                    }
                                }
                            }
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("UID_text")
                        {
                            mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                            text = "UID",
                            width = "120",
                            minWidth = "120",
                            fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                            {
                                width = "200px",
                                fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"dou_username", "dou_username_text" },
                                {"wechat_username", "wechat_username_text" },
                                {"tg_sex", "tg_sex_text" },
                                {"UID", "UID_text" },
                            },
                                emtModelBase = new ModelBasic.EmtGrid("grid")
                                {
                                    items = new List<ModelBasic.EmtGrid.Item>
                                {
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=12,
                                        emtModelBase = new ModelBasic.EmtInput($"UID")
                                        {
                                            width = "120",
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("submit_uid")
                                        {
                                            defaultValue = "提交",
                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                                {
                                                    attachPara=new Dictionary<string, object>
                                                    {
                                                        {"id","<%=page.focus.attr('data-id')%>" },
                                                        {"Index","3"},
                                                        {"Value","<%=page.UID.value%>"},
                                                    },
                                                    resCallJs="page.focus.text(page.UID.value);$('.floatlayer_div').hide();",
                                                    func=new ServiceFactory.UserService().TgInfoEdit
                                                }
                                            }
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("cancel_uid")
                                        {
                                            defaultValue = "取消",

                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventJavascript=new EventJavascript
                                                {
                                                    code="$('.floatlayer_div').hide();"
                                                }
                                            }
                                        }
                                    }
                                }
                                },
                                eventJsShow = new EventJsBasic
                                {
                                    eventJavascript = new EventJavascript
                                    {
                                        code = "page.UID.set(page.focus.attr('data-UID'));"
                                    }
                                }
                            }
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username_text")
                        {
                            mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                            text = "微信号",
                            width = "120",
                            minWidth = "120",
                            fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                            {
                                width = "200px",
                                fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"dou_username", "dou_username_text" },
                                {"wechat_username", "wechat_username_text" },
                                {"tg_sex", "tg_sex_text" },
                                {"UID", "UID_text" },
                            },
                                emtModelBase = new ModelBasic.EmtGrid("grid")
                                {
                                    items = new List<ModelBasic.EmtGrid.Item>
                                {
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=12,
                                        emtModelBase = new ModelBasic.EmtInput($"wechat_username")
                                        {
                                            width = "120",
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("submit_we")
                                        {
                                            defaultValue = "提交",
                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                                {
                                                    attachPara=new Dictionary<string, object>
                                                    {
                                                        {"id","<%=page.focus.attr('data-id')%>" },
                                                        {"Index","2"},
                                                        {"Value","<%=page.wechat_username.value%>"},
                                                    },
                                                    resCallJs="page.focus.text(page.wechat_username.value);$('.floatlayer_div').hide();",
                                                    func=new ServiceFactory.UserService().TgInfoEdit
                                                }
                                            }
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("cancel_we")
                                        {
                                            defaultValue = "取消",

                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventJavascript=new EventJavascript
                                                {
                                                    code="$('.floatlayer_div').hide();"
                                                }
                                            }
                                        }
                                    }
                                }
                                },
                                eventJsShow = new EventJsBasic
                                {
                                    eventJavascript = new EventJavascript
                                    {
                                        code = "page.wechat_username.set(page.focus.attr('data-wechat_username'));"
                                    }
                                }
                            }
                        });

                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_sex_text")
                        {
                            mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                            text = "男女厅",
                            width = "120",
                            minWidth = "120",
                            fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                            {
                                width = "200px",
                                fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"dou_username", "dou_username_text" },
                                {"wechat_username", "wechat_username_text" },
                                {"tg_sex", "tg_sex_text" },
                                {"UID", "UID_text" },
                            },
                                emtModelBase = new ModelBasic.EmtGrid("grid")
                                {
                                    items = new List<ModelBasic.EmtGrid.Item>
                                {
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=12,
                                        emtModelBase = new ModelBasic.EmtSelect($"tg_sex")
                                        {
                                            options=new Dictionary<string, string>
                                            {
                                                { "男","男"},
                                                { "女","女"},
                                            },
                                            width = "120",
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("submit_sex")
                                        {
                                            defaultValue = "提交",
                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                                {
                                                    attachPara=new Dictionary<string, object>
                                                    {
                                                        {"id","<%=page.focus.attr('data-id')%>" },
                                                        {"Index","1"},
                                                        {"Value","<%=page.tg_sex.value%>"},
                                                    },
                                                    resCallJs="page.focus.text(page.tg_sex.value);$('.floatlayer_div').hide();",
                                                    func=new ServiceFactory.UserService().TgInfoEdit
                                                }
                                            }
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtButton("cancel_sex")
                                        {
                                            defaultValue = "取消",

                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventJavascript=new EventJavascript
                                                {
                                                    code="$('.floatlayer_div').hide();"
                                                }
                                            }
                                        }
                                    }
                                }
                                },
                                eventJsShow = new EventJsBasic
                                {
                                    eventJavascript = new EventJavascript
                                    {
                                        code = "page.tg_sex.set(page.focus.attr('data-tg_sex'));"
                                    }
                                }
                            }
                        });
                    }

                    if (req.isShowZbInfo)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("app_id")
                        {
                            text = "直播账号id",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                        {
                            text = "年龄",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("marriage")
                        {
                            text = "是否已婚",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("child")
                        {
                            text = "有无孩子",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sound_card")
                        {
                            text = "声卡",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                        {
                            text = "兼职全职",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address")
                        {
                            text = "地区",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("experience")
                        {
                            text = "直播经验",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                        {
                            text = "现实工作",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("revenue")
                        {
                            text = "目标收入",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions")
                        {
                            text = "接档时间",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("birthday")
                        {
                            text = "生日",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("star_sign_text")
                        {
                            text = "星座",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("talent")
                        {
                            text = "才艺",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("way")
                        {
                            text = "招聘渠道",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("devices_num")
                        {
                            text = "设备数量",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mbti_text")
                        {
                            text = "mbti人格",
                            width = "120",
                            minWidth = "120"
                        });
                    }

                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Post",
                            field_paras = "id"
                        },
                        style = "",
                        text = "编辑",
                        name = "Post"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "InfoPost",
                            field_paras = "id"
                        },
                        style = "",
                        text = "背调",
                        name = "InfoPost",
                        disabled = true
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "离职",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DeletesAction,
                            field_paras = "id"
                        }
                    });
                    string user_sn = "";
                    string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "登录",
                        name = "login",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            url = "/TgManage/ZbAccount/fastlogin",
                            field_paras = "user_sn",
                            target = "_bank",
                        }
                    });

                    #endregion 操作列按钮

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public string yy_user_sn { get; set; }
                    public bool isShowZbInfo { get; set; } = true;
                    public bool isShowTgInfo { get; set; } = false;
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"user_base.status = '{ModelDb.user_base.status_enum.正常.ToSByte()}' and user_base.tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                    //查询条件
                    if (!dtoReqListData.keyword.IsNullOrEmpty()) where += $" AND (user_base.name like '%{dtoReqListData.keyword}%' OR user_base.username like '%{dtoReqListData.keyword}%')";
                    if (!dtoReqListData.tg_user_sn.IsNullOrEmpty() && dtoReqListData.zb_user_sn.IsNullOrEmpty()) where += $" AND user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, dtoReqListData.tg_user_sn)}";
                    if (!dtoReqListData.zb_user_sn.IsNullOrEmpty()) where += $" AND user_base.user_sn = '{dtoReqListData.zb_user_sn}'";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by user_base.id desc ",
                        on = "user_base.user_sn=user_info_zhubo.user_sn"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
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

                    public string tg_user_sn { get; set; }
                    public string zb_user_sn { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string UID_text
                    {
                        get
                        {
                            return UID;
                        }
                    }

                    public string yy_user_sn_text
                    {
                        get
                        {
                            string yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.user_sn);
                            return new DomainBasic.UserApp().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }

                    public string tg_user_sn_text
                    {
                        get
                        {
                            string tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, this.user_sn);
                            return new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn).name;
                        }
                    }

                    public string zhiwu
                    {
                        get
                        {
                            var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_sn);
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", zhubo.position);
                        }
                    }

                    public string dou_username_text
                    {
                        get
                        {
                            return getAttach3(0);
                        }
                    }

                    public string tg_sex_text
                    {
                        get
                        {
                            return getAttach3(1);
                        }
                    }

                    public string wechat_username_text
                    {
                        get
                        {
                            return getAttach3(2);
                        }
                    }

                    public string UID
                    {
                        get
                        {
                            return getAttach3(3);
                        }
                    }

                    public string getAttach3(int index = 0)
                    {
                        if (attach3.IsNullOrEmpty())
                        {
                            return "-";
                        }
                        try
                        {
                            var s = attach3.Split('☆');
                            return s[index];
                        }
                        catch
                        {
                            return "-";
                        }
                    }

                    public string app_id { get; set; }
                    public string age { get; set; }
                    public string marriage { get; set; }
                    public string child { get; set; }
                    public string sound_card { get; set; }
                    public string full_or_part { get; set; }
                    public string address { get; set; }
                    public string experience { get; set; }
                    public string job { get; set; }
                    public string revenue { get; set; }
                    public string sessions { get; set; }
                    public string birthday { get; set; }
                    public string star_sign { get; set; }

                    public string star_sign_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("星座", this.star_sign);
                        }
                    }

                    public string talent { get; set; }
                    public string way { get; set; }
                    public string devices_num { get; set; }
                    public string mbti { get; set; }

                    public string mbti_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", this.mbti);
                        }
                    }
                }

                #endregion ListData

                #region 异步请求处理

                /// <summary>
                /// 批量删除资产
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    var user_base = new DomainBasic.UserApp().GetInfoById(dtoReqData.id.ToInt());
                    new ServiceFactory.UserInfo.Zhubo().DeleteZb(user_base.user_sn);
                    string sql = new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(
                        ModelDb.user_info_zhubo_log.c_type_enum.离职,
                        $"运营'{new UserIdentityBag().username}'操作离职，主播:'{user_base.username}'",
                        new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_base.user_sn));
                    MysqlHelper.ExecuteSqlTran(new List<string>() { sql });
                    return result;
                }

                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
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
                #endregion 异步请求处理
            }

            /// <summary>
            /// 开通主播账号_1:根据新的主播
            /// </summary>
            public class Zhubo_AccountPostFromNew
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                    var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id, false);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_info_zhubo_id")
                    {
                        defaultValue = req.user_info_zhubo_id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    //如果是厅管
                    if (new DomainBasic.UserTypeApp().GetInfo().sys_code == ModelEnum.UserTypeEnum.tger.ToString())
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("ting_sn")
                        {
                            defaultValue = req.ting_sn
                        });
                    }
                    else
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                        {
                            title = "所属厅管",
                            options = new ServiceFactory.RelationService().GetTreeOption(new UserIdentityBag().user_sn),
                            isRequired = true,
                            defaultValue = req.tg_user_sn,
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                {
                                    {"tg_user_sn","<%=page_post.tg_user_sn.value%>"}
                                },
                                    func = GetTing,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("post.ting_sn").options(@"JSON.parse(res.data)")};"
                                }
                            }
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                        {
                            title = "所属厅",
                            options = new List<ModelDoBasic.Option>(),
                            isRequired = true,
                            defaultValue = req.ting_sn
                        });
                    }

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "登录账号",
                        placeholder = "设置登录账号",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "主播昵称",
                        placeholder = "设置主播昵称",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("full_or_part")
                    {
                        title = "兼职/全职",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("position")
                    {
                        title = "职务",
                        options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                    });


                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                    {
                        title = "设置密码",
                        Content = "123456"
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }

                    /// <summary>
                    /// 厅管
                    /// </summary>
                    public string tg_user_sn { get; set; }

                    /// <summary>
                    /// 直播厅
                    /// </summary>
                    public string ting_sn { get; set; }
                    /// <summary>
                    /// 主播信息id
                    /// </summary>
                    public int user_info_zhubo_id { get; set; }


                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_base = req.data_json.ToModel<ServiceFactory.UserInfo.User.user_base>();
                    string tg_user_sn = req.GetPara("tg_user_sn");
                    string yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn;

                    if (DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("user_info_zhubo_id").ToInt()).status != ModelDb.user_info_zhubo.status_enum.待开账号.ToSByte())
                    {
                        throw new Exception($"禁止重复提交");
                    }
                    //如果是厅管创建，则只能是自己名下
                    if (new DomainBasic.UserTypeApp().GetInfo().sys_code == ModelEnum.UserTypeEnum.tger.ToString())
                    {
                        tg_user_sn = new UserIdentityBag().user_sn;
                    }

                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                    user_base.password = "123456";
                    user_base.user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("zber").id.ToSByte();

                    List<string> lSql = new List<string>();
                    //调用创建用户user_base接口
                    lSql.AddRange(new ServiceFactory.UserInfo.User().ManagerPostZb(user_base, tg_user_sn));




                    //新增一条user_info_zb数据
                    var user_info_zb = new ModelDb.user_info_zb()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        yy_user_sn = yy_user_sn,
                        zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).zt_user_sn,
                        tg_user_sn = tg_user_sn,
                        user_sn = user_base.user_sn,
                        zbsr_color = user_base.attach2,
                        name = user_base.name,
                        full_or_part = user_base.attach1,
                        position = user_base.attach2,
                        user_info_zb_sn = user_base.user_sn,
                        ting_sn = req.GetPara("ting_sn"),
                        note = $"user_info_zhubo.id = {req.GetPara("user_info_zhubo_id")}"
                    };

                    //lSql.Add(user_info_zb.InsertTran());

                    //新增一条user_info_zhubo数据
                    var user_info_zhubo = user_info_zb.ToModel<ModelDb.user_info_zhubo>();
                    user_info_zhubo.user_name = user_base.name;
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                    user_info_zhubo.sources_memo = $"昵称:{user_base.name}，/UserInfo/主播/账号维护";
                    lSql.Add(user_info_zhubo.InsertOrUpdateTran($"id = '{req.GetPara("user_info_zhubo_id")}'"));
                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.入职,
                        $"厅管开通账号;url:/Userinfo/Zhubo/Create;昵称:{user_info_zhubo.user_name}",
                        user_info_zhubo));
                    DoMySql.ExecuteSqlTran(lSql);

                    result.data = new
                    {
                        user_sn = user_base.user_sn
                    };
                    return result;
                }

                public JsonResultAction GetTing(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var list = new ServiceFactory.UserInfo.Ting().GetTingsByTgsn(req["tg_user_sn"].ToNullableString());
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var item in list)
                    {
                        options.Add(new ModelDoBasic.Option
                        {
                            text = item.ting_name,
                            value = item.ting_sn,
                        });

                    }
                    result.data = options.ToJson();
                    return result;
                }
                #endregion
            }

            /// <summary>
            /// 开通主播账号_2:直接开通
            /// </summary>
            public class Zhubo_AccountPost
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

                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                    {
                        title = "所属厅管",
                        options = new ServiceFactory.RelationService().GetTreeOption(new UserIdentityBag().user_sn),
                        isRequired = true,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    {"tg_user_sn","<%=page_post.tg_user_sn.value%>"}
                                },
                                func = GetTing,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.ting_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                    {
                        title = "所属厅",
                        options = new List<ModelDoBasic.Option>(),
                        isRequired = true,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "登录账号",
                        placeholder = "设置登录账号",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "主播昵称",
                        placeholder = "设置主播昵称",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("full_or_part")
                    {
                        title = "兼职/全职",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("position")
                    {
                        title = "职务",
                        options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                    {
                        title = "设置密码",
                        Content = "123456"
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_base = req.data_json.ToModel<ServiceFactory.UserInfo.User.user_base>();
                    string tg_user_sn = req.GetPara("tg_user_sn");

                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                    user_base.password = "123456";
                    user_base.user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("zber").id.ToSByte();

                    List<string> lSql = new List<string>();
                    //调用创建用户user_base接口
                    lSql.AddRange(new ServiceFactory.UserInfo.User().ManagerPostZb(user_base, tg_user_sn));

                    //新增一条user_info_zb数据
                    var user_info_zb = new ModelDb.user_info_zb()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        yy_user_sn = new UserIdentityBag().user_sn,
                        zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(new UserIdentityBag().user_sn).zt_user_sn,
                        tg_user_sn = tg_user_sn,
                        user_sn = user_base.user_sn,
                        name = user_base.name,
                        full_or_part = req.GetPara("full_or_part"),
                        position = req.GetPara("position"),
                        user_info_zb_sn = UtilityStatic.CommonHelper.CreateUniqueSn(),
                        ting_sn = req.GetPara("ting_sn")
                    };
                    if (!new ServiceFactory.UserInfo.User().ManagerPostUserInfoZb(user_info_zb))
                    {
                        throw new WeicodeException("创建失败");
                    }

                    //新增一条user_info_zhubo数据
                    var user_info_zhubo = user_info_zb.ToModel<ModelDb.user_info_zhubo>();
                    user_info_zhubo.user_name = user_base.name;
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                    user_info_zhubo.sources_memo = $"昵称:{user_base.name}，/UserInfo/主播/账号维护";
                    lSql.Add(user_info_zhubo.InsertTran());
                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.入职,
                        $"运营创建主播;url:/Userinfo/ZhuboAccount/Create;昵称:{user_info_zhubo.user_name}",
                        user_info_zhubo));
                    DoMySql.ExecuteSqlTran(lSql);

                    result.data = new
                    {
                        user_sn = user_base.user_sn
                    };
                    return result;
                }

                public JsonResultAction GetTing(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var list = new ServiceFactory.UserInfo.Ting().GetTingsByTgsn(req["tg_user_sn"].ToNullableString());
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var item in list)
                    {
                        options.Add(new ModelDoBasic.Option
                        {
                            text = item.ting_name,
                            value = item.ting_sn,
                        });

                    }
                    result.data = options.ToJson();
                    return result;
                }
                #endregion
            }


            /// <summary>
            /// 账号编辑页面:user_base修改
            /// </summary>
            public class Zhubo_AccountEdit
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
                    var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "登录账号",
                        placeholder = "登录账号",
                        isRequired = true,
                        defaultValue = user_base.username
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("name")
                    {
                        defaultValue = user_base.name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                    {
                        title = "设置密码",
                        placeholder = "不填代表不修改密码"
                    });
                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string user_sn { get; set; }
                    public string relation_type { get; set; } = "";
                    public string user_type { get; set; }
                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_base = req.data_json.ToModel<ServiceFactory.UserInfo.User.user_base>();

                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (!user_base.password.IsNullOrEmpty())
                    {
                        user_base.password = UtilityStatic.Md5.getMd5(user_base.password);
                    }
                    user_base.Update();

                    result.data = new
                    {
                        user_sn = user_base.user_sn
                    };
                    return result;
                }

                public JsonResultAction GetTing(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var list = new ServiceFactory.UserInfo.Ting().GetTingsByTgsn(req["tg_user_sn"].ToNullableString());
                    var options = new List<ModelDoBasic.Option>();
                    foreach (var item in list)
                    {

                        options.Add(new ModelDoBasic.Option
                        {
                            text = item.ting_name,
                            value = item.ting_sn,
                        });

                    }
                    result.data = options.ToJson();
                    return result;
                }
                #endregion
            }


        }
    }
}
