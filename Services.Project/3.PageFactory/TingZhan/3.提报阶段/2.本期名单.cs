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
    /// 名单查看
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 厅战目标查看
            /// <summary>
            /// 厅战目标列表页面
            /// </summary>
            public class TargetList
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

                    var yy_options = new Dictionary<string, string>();
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            });
                            break;
                        case ModelEnum.UserTypeEnum.manager:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                        default:
                            yy_options = new Dictionary<string, string>();
                            break;
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "140px",
                        placeholder = "运营账号",
                        options = yy_options,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        },
                        defaultValue = req.yy_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "140px",
                        placeholder = "厅管账号",
                        options = new ServiceFactory.RelationService().GetTreeOptionDic(req.yy_user_sn),
                        defaultValue = req.tg_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtHidden("tingzhan_id")
                    {
                        defaultValue = req.id.ToString()
                    });
                    return listFilter;
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
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "目标提报",
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营账号",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amont_text")
                    {
                        text = "目标音浪",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("reason")
                    {
                        text = "不参加原因",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("start_time_text")
                    {
                        text = "提报开始时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("end_time_text")
                    {
                        text = "提报结束时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Edit",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Edit"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public string yy_user_sn { get; set; }
                    public string tg_user_sn { get; set; }
                    public int id { get; set; }
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id}";

                    var req = reqJson.GetPara();
                    if (req["tingzhan_id"].ToInt() > 0)
                    {
                        where += $" and tingzhan_id = {req["tingzhan_id"].ToInt()}";
                    }
                    else
                    {
                        where += $" and tingzhan_id = {new ServiceFactory.TingZhanService().getNewTingzhan().id}";
                    }

                    //查询条件
                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND tg_user_sn = '{req["tg_user_sn"]}'";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" and yy_user_sn = '{req["yy_user_sn"]}'";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            where += $@" and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            })}";
                            break;
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_target, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_tingzhan_target
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string amont_text
                    {
                        get
                        {
                            if (amont > 0)
                            {
                                return amont.ToInt().ToString() + "W";
                            }
                            else
                            {
                                return "不参与";
                            };
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
                    public string start_time_text
                    {
                        get
                        {
                            return p_tingzhan.start_time.ToDateTime().ToString();
                        }
                    }
                    public string end_time_text
                    {
                        get
                        {
                            return p_tingzhan.end_time.ToDateTime().ToString();
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                public class DtoReqData : ModelDb.p_tingzhan
                {
                }
                #endregion
            }
            #endregion

            #region 厅战目标未提报名单查看
            /// <summary>
            /// （管理员/运营/中台）厅战目标未提报名单列表页面
            /// </summary>
            public class UnTargetList
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

                    var yy_options = new Dictionary<string, string>();
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            });
                            break;
                        case ModelEnum.UserTypeEnum.manager:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                        default:
                            yy_options = new Dictionary<string, string>();
                            break;
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "140px",
                        placeholder = "运营账号",
                        options = yy_options,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        },
                        defaultValue = req.yy_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "140px",
                        placeholder = "厅管账号",
                        options = new ServiceFactory.RelationService().GetTreeOptionDic(req.yy_user_sn)
                    });
                    return listFilter;
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
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "目标提报",
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        attachPara = new Dictionary<string, object>
                        {
                            {"tingzhan_id",req.id.ToString()}
                        }
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "所属运营",
                        width = "140",
                        minWidth = "140"
                    });


                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "所属厅管",
                        width = "140",
                        minWidth = "140"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "140",
                        minWidth = "140"
                    });
                    #region 操作列按钮

                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public string yy_user_sn { get; set; }
                    public int id { get; set; }
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
                    int tingzhang_id;
                    if (req["tingzhan_id"].ToInt() > 0)
                    {
                        tingzhang_id = req["tingzhan_id"].ToInt();
                    }
                    else
                    {
                        tingzhang_id = new ServiceFactory.TingZhanService().getNewTingzhan().id;
                    }

                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_info_tg.status_enum.正常.ToSByte()} and ting_sn not in (select ting_sn from p_tingzhan_target where tingzhan_id = {tingzhang_id})";

                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND tg_user_sn = '{req["tg_user_sn"]}'";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" and yy_user_sn = '{req["yy_user_sn"]}'";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            where += $" and yy_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.jder:
                            where += $@" and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            })}";
                            break;
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_tg, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_tg
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                public class DtoReqData : ModelDb.p_tingzhan
                {
                }
                #endregion
            }
            #endregion
        }
    }
}
