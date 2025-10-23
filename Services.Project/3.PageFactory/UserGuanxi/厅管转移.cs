using System;
using System.Collections.Generic;
using System.Linq;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Modular;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserGuanxi
        {

            #region 厅管账号团队内转移
            /// <summary>
            /// 厅管账号团队内转移
            /// </summary>
            public class Tg_MovePost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    //设置tab页
                    var pageModel = new PagePost("");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"type_id", req.type_id}
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new EmtButtonGroup("");

                    buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("查看转移记录")
                    {
                        text = "转移记录",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"TgMoveList?type_id={req.type_id}",
                        }
                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取下拉数据
                    //判断当前关系类型
                    if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                    //获取当前运营的所有下属厅管
                    List<ModelDoBasic.Option> options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn);

                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelectFull("yy_sn_before")
                    {
                        title = "当前团队",
                        options = options,
                        defaultValue = new UserIdentityBag().user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventComponent = new EmtDataSelect.Js("l_move").clear()
                        }
                    }); ;
                    formDisplay.formItems.Add(new EmtSelectFull("yy_sn_after")
                    {
                        title = "目标团队",
                        options = options,
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "转移事由",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                    {
                        title = "转移账号",
                        selectUrl = $"TgSelect?yy_sn=<%=page.yy_sn_before.value%>",
                        buttonText = "选择需要转移的厅管账号",
                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                        {
                             new ModelBasic.EmtDataSelect.ColItem("username")
                             {
                                  edit = "text",
                                  title = "用户名"
                             },
                             new ModelBasic.EmtDataSelect.ColItem("name")
                             {
                                  edit = "text",
                                  title = "备注"
                             },
                             new ModelBasic.EmtDataSelect.ColItem("mobile")
                             {
                                  edit = "text",
                                  title = "手机号码",
                             },
                        }
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 类型id：1=主播;2=;3=厅管关系
                    /// </summary>
                    public int type_id { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 转移用户
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    if (req.GetPara("yy_sn_before").IsNullOrEmpty()) throw new WeicodeException("选择当前团队不能为空!");
                    if (req.GetPara("yy_sn_after").IsNullOrEmpty()) throw new WeicodeException("选择目标团队不能为空!");
                    var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                    if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");

                    //2.转移到目标用户
                    var _ = new DomainUserBasic.UserRelationApp().MoveNextUsersToUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, req.GetPara("yy_sn_before"), moveItems, req.GetPara("yy_sn_after"), req.GetPara("cause"));
                    foreach (var item in moveItems)
                    {
                        new DomainBasic.SystemBizLogApp().Write("厅管转移", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"厅管{item.username}转移至{new DomainBasic.UserApp().GetInfoByUserSn(req.GetPara("yy_sn_after")).username}");
                    }
                    
                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion

            #region 厅管跨团队转移
            /// <summary>
            /// 厅管跨团队转移
            /// </summary>
            public class Tg_MoveOtherYyPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    //设置tab页
                    var pageModel = new PagePost("");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"type_id", req.type_id}
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new EmtButtonGroup("");

                    buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("查看转移记录")
                    {
                        text = "转移记录",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"TgMoveList?type_id={req.type_id}",
                        }
                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取下拉数据
                    //判断当前关系类型
                    if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                    //获取当前运营的所有下属厅管
                    List<ModelDoBasic.Option> options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForOption(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter());

                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelectFull("yy_sn_before")
                    {
                        title = "当前团队",
                        options = options,
                        defaultValue = new UserIdentityBag().user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventComponent = new EmtDataSelect.Js("l_move").clear()
                        }
                    }); ;
                    formDisplay.formItems.Add(new EmtSelectFull("yy_sn_after")
                    {
                        title = "目标团队",
                        options = options,
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "转移事由",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                    {
                        title = "转移账号",
                        selectUrl = $"TgSelect?yy_sn=<%=page.yy_sn_before.value%>",
                        buttonText = "选择需要转移的厅管账号",
                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                        {
                             new ModelBasic.EmtDataSelect.ColItem("username")
                             {
                                  edit = "text",
                                  title = "用户名"
                             },
                             new ModelBasic.EmtDataSelect.ColItem("name")
                             {
                                  edit = "text",
                                  title = "备注"
                             },
                             new ModelBasic.EmtDataSelect.ColItem("mobile")
                             {
                                  edit = "text",
                                  title = "手机号码",
                             },
                        }
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 类型id：1=主播;2=;3=厅管关系
                    /// </summary>
                    public int type_id { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 转移用户
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    if (req.GetPara("yy_sn_before").IsNullOrEmpty()) throw new WeicodeException("选择当前团队不能为空!");
                    if (req.GetPara("yy_sn_after").IsNullOrEmpty()) throw new WeicodeException("选择目标团队不能为空!");
                    var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                    if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");

                    //2.转移到目标用户
                    var _ = new DomainUserBasic.UserRelationApp().MoveNextUsersToUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, req.GetPara("yy_sn_before"), moveItems, req.GetPara("yy_sn_after"), req.GetPara("cause"));

                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion

            #region 选择账号
            /// <summary>
            /// 厅管列表
            /// </summary>
            public class Tg_Select
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
                    listFilter.formItems.Add(new EmtInput("username")
                    {
                        placeholder = "用户名称",
                        defaultValue = ""
                    });
                    listFilter.formItems.Add(new EmtHidden("isolated")
                    {
                        placeholder = "是否孤立",
                        defaultValue = req.isolated.ToString()
                    });
                    return listFilter;
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
                        funcGetListData = GetListData,
                        attachPara = new Dictionary<string, object>
                        {
                            {"yy_sn", req.yy_sn}
                        }
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "用户名称",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("name")
                    {
                        text = "备注",
                        width = "120",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_type_name")
                    {
                        text = "用户类型",
                        width = "120",
                        minWidth = "180",
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
                    /// 是否孤立 1:是，0:否
                    /// </summary>
                    public int isolated { get; set; }
                    /// <summary>
                    /// 关系类型id
                    /// </summary>
                    public int type_id { get; set; }

                    /// <summary>
                    /// user_sn
                    /// </summary>
                    public string yy_sn { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// 用户表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = "";
                    string yy_sn = reqJson.GetPara("yy_sn");
                    if (!reqJson.GetPara("username").IsNullOrEmpty()) where += $"username like '%{reqJson.GetPara("username")}%'";

                    return new ModularUserBasic.UserRelationApp().GetNextUsers<ItemDataModel>(reqJson, ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_sn,
                        new DoMySql.Filter
                        {
                            where = where
                        }
                    );
                }
                ///// <summary>
                ///// 数据项模型
                ///// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string user_type_name
                    {
                        get
                        {
                            return DoMySql.FindEntityById<ModelDb.user_type>(this.user_type_id).name;
                        }
                    }
                }
                #endregion
            }
            #endregion

            /// <summary>
            /// 厅管转移记录列表
            /// </summary>
            public class Tg_MoveList
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
                    listFilter.formItems.Add(new EmtInput("username")
                    {
                        placeholder = "用户名称",
                        defaultValue = ""
                    });

                    return listFilter;
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
                        funcGetListData = GetListData,
                        attachPara = new Dictionary<string, object>
                    {
                        {"type_id",req.type_id}
                    }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "转移单号",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_names")
                    {
                        text = "用户名",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_before_name")
                    {
                        text = "原上级用户",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_after_name")
                    {
                        text = "现上级用户",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ac_date")
                    {
                        text = "转移日期",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("notes")
                    {
                        text = "转移事由",
                        width = "200",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("op_username")
                    {
                        text = "操作人",
                        width = "200",
                        minWidth = "180",
                    });
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    /// <summary>
                    /// 关系类型id
                    /// </summary>
                    public int type_id;
                }
                #endregion
                #region ListData
                /// <summary>
                /// 获取当前登录user_sn的转移操作记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"user_sn = '{new UserIdentityBag().user_sn}' and o_type = '0' and relation_type_id = '{ModelEnum.UserRelationTypeEnum.运营邀厅管}'";
                    if (!reqJson.GetPara("username").ToNullableString().IsNullOrEmpty())
                    {
                        where += $" AND (user_names like '%{reqJson.GetPara("username")}%')";
                    }

                    //2.获取当前登录user_sn指定关系类型的转移操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where ,
                        orderby = "create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_relation_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_relation_log
                {
                    public string user_before_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.f_user_sn}'", false).username;
                        }
                    }
                    public string user_after_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.t_user_sn}'", false).username;
                        }
                    }
                    public string ac_date
                    {
                        get
                        {
                            return this.create_time.ToDateTime().ToString("yyyy-MM-HH");
                        }
                    }
                    public string op_username
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'").username;
                        }
                    }
                }
                #endregion
            }
        }
    }
}