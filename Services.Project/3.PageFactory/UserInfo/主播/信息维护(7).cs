using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.UserInfo;
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
            #region 1.流失库
            public class LossList
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

                    listFilter.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        width = "100px",
                        placeholder = "主播"
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "主播用户名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sources_name")
                    {
                        text = "来源名称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "加入时间",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_name")
                    {
                        text = "所属厅管",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_name")
                    {
                        text = "所属运营",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("o_type_text")
                    {
                        text = "主播类型",
                        width = "180",
                        minWidth = "180"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "UnDel",
                        style = "",
                        text = "恢复",
                        title = "提示说明",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = UnDeletesAction,
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
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"status = '{ModelDb.user_info_zhubo.status_enum.已离职.ToInt()}'";

                    //查询条件
                    if (!reqJson.GetPara("user_name").IsNullOrEmpty()) where += $" AND user_name like '%{reqJson.GetPara("user_name")}%'";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_zhubo
                {
                    public string tg_user_name
                    {
                        get
                        {
                            if (tg_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    public string yy_user_name
                    {
                        get
                        {
                            if (yy_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string o_type_text
                    {
                        get
                        {
                            return Enum.GetName(typeof(ModelDb.user_info_zhubo.o_type_enum), o_type);
                        }
                    }
                }
                #endregion

                #region 异步请求处理


                /// <summary>
                /// 恢复流失主播
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction UnDeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("id").ToInt());

                    List<string> lSql = new List<string>();
                    if (!user_info_zhubo.IsNullOrEmpty())
                    {
                        user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                        user_info_zhubo.Update();
                    }
                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.入职,
                        "恢复已流失的主播",
                        user_info_zhubo));
                    DoMySql.ExecuteSqlTran(lSql);

                    return result;
                }
                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
                }
                #endregion
            }
            #endregion

            #region 2.在职主播
            public class OnJobList
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
                    var user_type = new ServiceFactory.UserInfo().GetUserType();

                    if (user_type == ModelEnum.UserTypeEnum.manager)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                        {
                            placeholder = "中台账号",
                            options = new ServiceFactory.UserInfo.Zt().GetZtBaseInfosForKv(new ServiceFactory.UserInfo.Zt.ZtBaseInfoFilter()),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                {
                                    {"zt_user_sn","<%=page.zt_user_sn.value%>"}
                                },
                                    func = GetYy,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("yy_user_sn").options(@"JSON.parse(res.data)")};"
                                }
                            },
                            width = "120px",
                        });
                    }
                    if (user_type == ModelEnum.UserTypeEnum.manager || user_type == ModelEnum.UserTypeEnum.zter)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                },
                                status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                            }),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                    },
                                    func = GetTinGuan,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};"
                                }
                            },
                            width = "120px",
                        });
                    }
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                        width = "120px",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("o_type")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"线上",ModelDb.user_info_zhubo.o_type_enum.线上.ToInt().ToString()},
                            {"线下",ModelDb.user_info_zhubo.o_type_enum.线下.ToInt().ToString()},
                        },
                        width = "120px",
                        disabled = false,
                        placeholder = "主播类型",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        },
                        width = "80px",
                        disabled = false,
                        placeholder = "性别",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("position")
                    {
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.职务),
                        width = "80px",
                        disabled = false,
                        placeholder = "职务",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "140px",
                        placeholder = "主播昵称/抖音号"
                    });

                    if (user_type == ModelEnum.UserTypeEnum.zter)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("info_type")
                        {
                            options = new Dictionary<string, string>()
                            {
                                {"所有主播","0"},
                                {"作者id缺失","1"},
                            },
                            width = "140px",
                            disabled = false,
                            placeholder = "主播信息完整度",
                        });
                    }
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
                    //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LossList")
                    //{
                    //    title = "LossList",
                    //    text = "恢复账号",
                    //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //    {
                    //        url = "LossList"
                    //    },
                    //});
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Create")
                    {
                        title = "Post",
                        text = "创建账号",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Create"
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
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "主播昵称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn_text")
                    {
                        text = "所属厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("position_text")
                    {
                        text = "职务",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "100",
                        minWidth = "100"
                    });
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "加入时间",
                        width = "180",
                        minWidth = "180"
                    });
                    #region 批量操作
                    //listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    //{
                    //    text = "批量操作",
                    //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    //    {
                    //        new ModelBasic.EmtModel.ButtonItem("")
                    //        {
                    //            text = "批量换厅",
                    //            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //            {
                    //                url = "ChangeTing",
                    //            }
                    //        }
                    //    }
                    //});
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Edit",
                        text = "编辑",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer()
                        {
                            url = "Edit",
                            field_paras = "user_sn",
                        },
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "InfoEdit",
                        text = "背调",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer()
                        {
                            url = "InfoEdit",
                            field_paras = "id",
                        },
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
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "FastLogin",
                        style = "",
                        text = "登录",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            url = "FastLogin",
                            field_paras = "user_sn",
                            target = "_bank",
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
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"status = '{ModelDb.user_info_zhubo.status_enum.正常.ToInt()}'";

                    //查询条件
                    if (!reqJson.GetPara("keyword").IsNullOrEmpty()) where += $" AND (user_name like '%{reqJson.GetPara("keyword")}%' OR dou_username like '%{reqJson.GetPara("keyword")}%')";

                    //查询条件
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    if (!reqJson.GetPara("ting_sn").IsNullOrEmpty())
                    {
                        where += $" and ting_sn = '{reqJson.GetPara("ting_sn")}'";
                    }
                    if (!reqJson.GetPara("o_type").IsNullOrEmpty())
                    {
                        where += $" and o_type = '{reqJson.GetPara("o_type")}'";
                    }

                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }

                    if (!reqJson.GetPara("position").IsNullOrEmpty())
                    {
                        where += $" and position = '{reqJson.GetPara("position")}'";
                    }

                    if (!reqJson.GetPara("info_type").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("info_type"))
                        {
                            case "1":
                                where += $" and (anchor_id = '' or anchor_id is null)";
                                break;
                            default:
                                break;
                        }
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_zhubo
                {
                    public string position_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", position);
                        }
                    }
                    public string tg_user_name
                    {
                        get
                        {
                            if (tg_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string yy_user_name
                    {
                        get
                        {
                            if (yy_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string ting_sn_text
                    {
                        get
                        {
                            if (ting_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    /// <summary>
                    /// 主播类型
                    /// </summary>
                    public string o_type_text
                    {
                        get
                        {
                            return Enum.GetName(typeof(ModelDb.user_info_zhubo.o_type_enum), o_type);
                        }
                    }


                    public string star_sign_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("星座", this.star_sign);
                        }
                    }
                    public string mbti_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", this.mbti);
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                /// <summary>
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYy(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    result.data = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                            UserSn = reqJson.GetPara("zt_user_sn"),
                        },
                        status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                    }).ToJson();
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
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 获取直播厅筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTings(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(req["tg_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 操作流失
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("id").ToInt());
                    zhubo.status = ModelDb.user_info_zhubo.status_enum.已离职.ToSByte();
                    lSql.Add(zhubo.UpdateTran());
                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.离职,
                        "主播流失",
                        zhubo));

                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{zhubo.user_sn}'", false);
                    if (!user_base.IsNullOrEmpty())
                    {
                        user_base.status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                        lSql.Add(user_base.UpdateTran());
                    }

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 召回主播
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RecallAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    ServiceFactory.UserInfo.Zhubo zhubo = new ServiceFactory.UserInfo.Zhubo();
                    zhubo.RecallZb(req.GetPara("user_sn"));

                    return result;
                }


                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 编辑主播信息
            /// </summary>
            public class OnJobPost
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
                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_sn")
                    {
                        defaultValue = user_info_zhubo.user_sn
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("full_or_part")
                    {
                        title = "兼职/全职",
                        options = new Dictionary<string, string>
                    {
                        {"兼职","兼职"},
                        {"全职","全职"}
                    },
                        isRequired = true,
                        defaultValue = user_info_zhubo.full_or_part,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("position")
                    {
                        title = "职务",
                        options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                        defaultValue = user_info_zhubo.position
                    });

                    pageModel.formDisplay.formItems.Add(new ModelBasic.EmtHtml("c_html")
                    {
                        Content = "A类主播：具备1麦带档能力<br>B类主播：具备2麦配合能力<br>C类主播：具备3麦互动能力"
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("o_type")
                    {
                        title = "主播类型",
                        options = new Dictionary<string, string>
                        {
                            {"线上",ModelDb.user_info_zhubo.o_type_enum.线上.ToSByte().ToString()},
                            {"线下",ModelDb.user_info_zhubo.o_type_enum.线下.ToSByte().ToString()}
                        },
                        defaultValue = user_info_zhubo.o_type.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        title = "主播昵称",
                        defaultValue = user_info_zhubo.user_name,
                        isRequired = false,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "抖音号",
                        isRequired = true,
                        defaultValue = user_info_zhubo.dou_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("join_date")
                    {
                        title = "入职时间",
                        isRequired = false,
                        mold = EmtTimeSelect.Mold.date,
                        defaultValue = user_info_zhubo.join_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("age")
                    {
                        title = "年龄",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.age.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("zb_sex")
                    {
                        title = "性别",
                        options = new Dictionary<string, string>()
                        {
                            {"男","男"},
                            {"女","女"},
                        },
                        colLength = 6,
                        defaultValue = user_info_zhubo.zb_sex.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("marriage")
                    {
                        title = "是否已婚",
                        isRequired = false,
                        colLength = 6,
                        options = new Dictionary<string, string>
                        {
                            {"已婚","已婚"},
                            {"未婚","未婚"},
                            {"离异","离异"}
                        },
                        defaultValue = user_info_zhubo.marriage,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("child")
                    {
                        title = "有无孩子",
                        isRequired = false,
                        options = new Dictionary<string, string>
                    {
                        {"有","有"},
                        {"无","无"},
                    },
                        colLength = 6,
                        defaultValue = user_info_zhubo.child,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("sound_card")
                    {
                        title = "声卡",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.sound_card,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                    {
                        title = "地区",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.address,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("experience")
                    {
                        title = "直播经验",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.experience,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("job")
                    {
                        title = "现实工作",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.job,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("revenue")
                    {
                        title = "目标收入",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.revenue,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("sessions")
                    {
                        title = "接档时间",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.sessions,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("birthday")
                    {
                        title = "生日",
                        isRequired = false,
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        colLength = 6,
                        defaultValue = user_info_zhubo.birthday
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("star_sign")
                    {
                        title = "星座",
                        isRequired = false,
                        colLength = 6,
                        options = new DomainBasic.DictionaryApp().GetListForOption("星座"),
                        defaultValue = user_info_zhubo.star_sign,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("talent")
                    {
                        title = "才艺",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.talent,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("way")
                    {
                        title = "招聘渠道",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.way,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "电话号码",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.mobile,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("devices_num")
                    {
                        title = "设备数量",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.devices_num.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("mbti")
                    {
                        title = "mbti人格",
                        isRequired = false,
                        colLength = 6,
                        defaultValue = user_info_zhubo.mbti,
                        options = new DomainBasic.DictionaryApp().GetListForOption("MBTI"),
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var lSql = new List<string>();
                    var user_info_zhubo = req.data_json.ToModel<ModelDb.user_info_zhubo>();
                    if (user_info_zhubo.dou_username.IsNullOrEmpty())
                    {
                        throw new Exception("请填写抖音号");
                    }
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{user_info_zhubo.user_sn}'");
                    user_base.name = user_info_zhubo.user_name;

                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = user_info_zhubo.dou_username.ToNullableString()
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("错误:抖音号不存在");
                    }
                    user_info_zhubo.anchor_id = dyCheckResult["data"]["anchor_id"].ToNullableString();



                    lSql.Add(user_info_zhubo.UpdateTran());
                    lSql.Add(user_base.UpdateTran());
                    MysqlHelper.ExecuteSqlTran(lSql);
                    return result;
                }
                #endregion
            }


            #region 批量换厅页面
            /// <summary>
            /// 批量换厅页面（主厅与训练厅更换）
            /// </summary>
            public class ChangeTing
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("PagePost");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };
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
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ids")
                    {
                        defaultValue = req.ids
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        title = "直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn)
                    });
                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public string ids { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var ting_sn = dtoReqData.ting_sn;
                    if (ting_sn.IsNullOrEmpty()) throw new Exception("请选择直播厅");
                    foreach (string id in dtoReqData.ids.Split(','))
                    {
                        var user_base = DoMySql.FindEntityById<ModelDb.user_base>(id.ToInt());
                        var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn = '{user_base.user_sn}'");
                        user_info_zhubo.ting_sn = ting_sn;
                        user_info_zhubo.Update();
                    }

                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"批量换厅：{dtoReqData.ids.Split(',').Length}个主播");

                    var result = new JsonResultAction();
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.user_info_zhubo
                {
                    public string ids { get; set; }
                }

                #endregion
            }
            #endregion

            #region 详情页面
            public class OnJobDetail
            {
                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("details");
                    pageModel.formDisplay = GetDetails(pageModel, req);
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetDetails(PageDetail pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = p_join_need.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_name")
                    {
                        title = "所属运营",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_name")
                    {
                        title = "所属厅管",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("ting_name")
                    {
                        title = "所属厅",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });


                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                }
            }
            #endregion

            #endregion

            #region 3.主播信息(手机端编辑)
            /// <summary>
            /// 信息编辑页面:user_info_zhubo修改
            /// </summary>
            public class ZbMobileInfoPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnUrl = "MobileView"
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
                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'", false);

                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_zhubo.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtImageSelect("img_url")
                    {
                        title = "头像",
                        defaultValue = user_info_zhubo.img_url,
                        //defaultValue = "http://zb.xiaoketang.com/UploadFile/images/202507241615114,",
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "抖音号",
                        defaultValue = user_info_zhubo.dou_username,
                        isRequired = true,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "微信号",
                        defaultValue = user_info_zhubo.wechat_username,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        title = "主播昵称",
                        defaultValue = user_info_zhubo.user_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("age")
                    {
                        title = "年龄",
                        defaultValue = user_info_zhubo.age.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                    {
                        title = "地区",
                        defaultValue = user_info_zhubo.address,
                    });

                    /*
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        title = "兼职/全职",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"},
                        },
                        defaultValue = user_info_zb.full_or_part,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("mbti")
                    {
                        title = "MBTI",
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.MBTI),
                        defaultValue = user_info_zb.mbti,
                    });*/

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("sound_card")
                    {
                        title = "声卡(是/否)",
                        options = new Dictionary<string, string>
                        {
                            {"是","是"},
                            {"否","否"},
                        },
                        defaultValue = user_info_zhubo.sound_card,
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

                    var lSql = new List<string>();
                    var zhubo = req.data_json.ToModel<ModelDb.user_info_zhubo>();
                    if (zhubo.dou_username.IsNullOrEmpty())
                    {
                        throw new Exception($"抖音号必填!");
                    }

                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = zhubo.dou_username.ToNullableString()
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    //暂时取消校验
                    //if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    //{
                    //    throw new Exception("抖音账号输入错误!");
                    //}
                    if (!dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        zhubo.anchor_id = dyCheckResult["data"]["anchor_id"].ToNullableString();
                    }


                    lSql.Add(zhubo.UpdateTran());

                    MysqlHelper.ExecuteSqlTran(lSql);

                    return result;
                }

                #endregion
            }
            #endregion


            #region 4.离职主播
            public class ResignList
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
                    var user_type = new ServiceFactory.UserInfo().GetUserType();

                    if (user_type == ModelEnum.UserTypeEnum.manager)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                        {
                            placeholder = "中台账号",
                            options = new ServiceFactory.UserInfo.Zt().GetZtBaseInfosForKv(new ServiceFactory.UserInfo.Zt.ZtBaseInfoFilter()),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                {
                                    {"zt_user_sn","<%=page.zt_user_sn.value%>"}
                                },
                                    func = GetYy,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("yy_user_sn").options(@"JSON.parse(res.data)")};"
                                }
                            },
                            width = "120px",
                        });
                    }
                    if (user_type == ModelEnum.UserTypeEnum.manager || user_type == ModelEnum.UserTypeEnum.zter)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                },
                                status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                            }),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                    },
                                    func = GetTinGuan,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};"
                                }
                            },
                            width = "120px",
                        });
                    }
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                        width = "120px",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("o_type")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"线上",ModelDb.user_info_zhubo.o_type_enum.线上.ToInt().ToString()},
                            {"线下",ModelDb.user_info_zhubo.o_type_enum.线下.ToInt().ToString()},
                        },
                        width = "120px",
                        disabled = false,
                        placeholder = "主播类型",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        },
                        width = "80px",
                        disabled = false,
                        placeholder = "性别",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("position")
                    {
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.职务),
                        width = "80px",
                        disabled = false,
                        placeholder = "职务",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "140px",
                        placeholder = "主播昵称/抖音号"
                    });

                    if (user_type == ModelEnum.UserTypeEnum.zter)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("info_type")
                        {
                            options = new Dictionary<string, string>()
                            {
                                {"所有主播","0"},
                                {"作者id缺失","1"},
                            },
                            width = "140px",
                            disabled = false,
                            placeholder = "主播信息完整度",
                        });
                    }
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
                    //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LossList")
                    //{
                    //    title = "LossList",
                    //    text = "恢复账号",
                    //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //    {
                    //        url = "LossList"
                    //    },
                    //});
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Create")
                    {
                        title = "Post",
                        text = "创建账号",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Create"
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
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "主播昵称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn_text")
                    {
                        text = "所属厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("position_text")
                    {
                        text = "职务",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "100",
                        minWidth = "100"
                    });
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "加入时间",
                        width = "180",
                        minWidth = "180"
                    });
                    #region 批量操作
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Recall",
                        style = "",
                        text = "召回",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = RecallAction,
                            field_paras = "user_sn"
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
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"status = '{ModelDb.user_info_zhubo.status_enum.已离职.ToInt()}'";

                    //查询条件
                    if (!reqJson.GetPara("keyword").IsNullOrEmpty()) where += $" AND (user_name like '%{reqJson.GetPara("keyword")}%' OR dou_username like '%{reqJson.GetPara("keyword")}%')";

                    //查询条件
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    if (!reqJson.GetPara("ting_sn").IsNullOrEmpty())
                    {
                        where += $" and ting_sn = '{reqJson.GetPara("ting_sn")}'";
                    }
                    if (!reqJson.GetPara("o_type").IsNullOrEmpty())
                    {
                        where += $" and o_type = '{reqJson.GetPara("o_type")}'";
                    }

                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }

                    if (!reqJson.GetPara("position").IsNullOrEmpty())
                    {
                        where += $" and position = '{reqJson.GetPara("position")}'";
                    }

                    if (!reqJson.GetPara("info_type").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("info_type"))
                        {
                            case "1":
                                where += $" and (anchor_id = '' or anchor_id is null)";
                                break;
                            default:
                                break;
                        }
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_zhubo
                {
                    public string position_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", position);
                        }
                    }
                    public string tg_user_name
                    {
                        get
                        {
                            if (tg_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string yy_user_name
                    {
                        get
                        {
                            if (yy_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string ting_sn_text
                    {
                        get
                        {
                            if (ting_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    /// <summary>
                    /// 主播类型
                    /// </summary>
                    public string o_type_text
                    {
                        get
                        {
                            return Enum.GetName(typeof(ModelDb.user_info_zhubo.o_type_enum), o_type);
                        }
                    }


                    public string star_sign_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("星座", this.star_sign);
                        }
                    }
                    public string mbti_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", this.mbti);
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                /// <summary>
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYy(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    result.data = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                            UserSn = reqJson.GetPara("zt_user_sn"),
                        },
                        status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                    }).ToJson();
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
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 获取直播厅筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTings(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(req["tg_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 操作流失
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("id").ToInt());
                    zhubo.status = ModelDb.user_info_zhubo.status_enum.已离职.ToSByte();
                    lSql.Add(zhubo.UpdateTran());
                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.离职,
                        "主播流失",
                        zhubo));

                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{zhubo.user_sn}'", false);
                    if (!user_base.IsNullOrEmpty())
                    {
                        user_base.status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                        lSql.Add(user_base.UpdateTran());
                    }

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 召回主播
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RecallAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    ServiceFactory.UserInfo.Zhubo zhubo = new ServiceFactory.UserInfo.Zhubo();
                    zhubo.RecallZb(req.GetPara("user_sn"));

                    return result;
                }


                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
                }
                #endregion
            }
            #endregion

            #region 5.更换接档号

            public class ReplaceDouUserName
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnUrl = "MobileView"
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
                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'", false);

                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_zhubo.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "抖音号",
                        isRequired = true,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号后四位",
                        isRequired = true,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "真实姓名",
                        isRequired = true,
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string dou_username { get; set; }

                    public string mobile { get; set; }

                    public string name { get; set; }
                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    //查询当前主播信息
                    var zhubo = req.data_json.ToModel<DtoReq>();
                    ServiceFactory.UserInfo.Zhubo zb = new ServiceFactory.UserInfo.Zhubo();
                    zb.ReplaceDouUserName(zhubo);
                    return result;
                }
                /// <summary>
                /// 调用抖音接口请求参数
                /// </summary>
                public class dyQianyueParam : Entity
                {
                    /// <summary>
                    /// 抖音账号
                    /// </summary>
                    public object dou_username { get; set; }
                    /// <summary>
                    /// 抖音作者id
                    /// </summary>
                    public object anchor_id { get; set; }
                    /// <summary>
                    /// 经纪人id
                    /// </summary>
                    public object jjranchor_id { get; set; }
                    /// <summary>
                    /// 手机后四位
                    /// </summary>
                    public object last_four_number { get; set; }
                    /// <summary>
                    /// 真实姓名
                    /// </summary>
                    public object real_name { get; set; }
                    /// <summary>
                    /// 经纪人姓名
                    /// </summary>
                    public object jjr_name { get; set; }

                    public string broker_id { get; set; }
                }
                #endregion

            }

            #endregion

            public class Bindingjjr
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnUrl = "MobileView"
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
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_username")
                    {
                        title = "经纪人名称",
                        isRequired = true,
                    });
                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string dou_username { get; set; }
                    public string jjr_username { get; set; }
                    public string mobile { get; set; }

                    public string name { get; set; }
                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var zhubo = req.data_json.ToModel<DtoReq>();
                    ServiceFactory.UserInfo.Zhubo zb = new ServiceFactory.UserInfo.Zhubo();
                    zb.Bindingjjr(zhubo.jjr_username);
                    return result;
                }
                /// <summary>
                /// 调用抖音接口请求参数
                /// </summary>
                public class dyQianyueParam : Entity
                {
                    /// <summary>
                    /// 抖音账号
                    /// </summary>
                    public object dou_username { get; set; }
                    /// <summary>
                    /// 抖音作者id
                    /// </summary>
                    public object anchor_id { get; set; }
                    /// <summary>
                    /// 经纪人id
                    /// </summary>
                    public object jjranchor_id { get; set; }
                    /// <summary>
                    /// 手机后四位
                    /// </summary>
                    public object last_four_number { get; set; }
                    /// <summary>
                    /// 真实姓名
                    /// </summary>
                    public object real_name { get; set; }
                }
                #endregion
            }
        }
    }
}
