using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.UserInfo;
using static WeiCode.ModelDbs.ModelDb;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            /// <summary>
            /// 查看下属的账号数据
            /// </summary>
            public class Ting_UserList
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
                        disabled = true
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
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_text")
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
                        title = "提示说明",
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
                            url = "fastlogin",
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

                    if (!dtoReqListData.yy_user_sn.IsNullOrEmpty())
                    {
                        where += $" AND (user_base.user_sn IN (SELECT t_user_sn FROM user_relation WHERE relation_type_id = 2 AND f_user_sn = '{dtoReqListData.yy_user_sn}'))";
                    }

                    //查询条件
                    if (!dtoReqListData.keyword.IsNullOrEmpty()) where += $" AND (user_base.name like '%{dtoReqListData.keyword}%' OR user_base.username like '%{dtoReqListData.keyword}%')";
                    if (!dtoReqListData.tg_user_sn.IsNullOrEmpty() && dtoReqListData.zb_user_sn.IsNullOrEmpty()) where += $" AND user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, dtoReqListData.tg_user_sn)}";
                    if (!dtoReqListData.zb_user_sn.IsNullOrEmpty()) where += $" AND user_base.user_sn = '{dtoReqListData.zb_user_sn}'";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by user_base.id desc ",
                        on = "user_base.user_sn=user_info_zb.user_sn"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
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
                    public string yy_user_sn { get; set; }

                    public string tg_user_sn { get; set; }
                    public string zb_user_sn { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string sessions_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", sessions);
                        }
                    }
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
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", this.attach2);
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
                /// 
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();


                    var user_base = new DomainBasic.UserApp().GetInfoById(dtoReqData.id.ToInt());

                    var user_type = new DomainBasic.UserTypeApp();

                    if (user_base.user_type_id == user_type.GetInfoByCode("zber").id)
                    {
                        new ServiceFactory.UserInfo.Zhubo().DeleteZb(user_base.user_sn);
                    }
                    if (user_base.user_type_id == user_type.GetInfoByCode("tger").id)
                    {
                        new ServiceFactory.UserInfo.Tg().DeleteTg(user_base.user_sn);
                    }
                    if (user_base.user_type_id == user_type.GetInfoByCode("yyer").id)
                    {
                        new ServiceFactory.UserInfo.Yy().DeleteYy(user_base.user_sn);
                    }

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

            #region 直播厅基础信息
            /// <summary>
            /// 
            /// </summary>
            public class TgInfoList
            {
                #region DefaultView
                /// <summary>
                /// 
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
                    var usertype = new ServiceFactory.UserInfo().GetUserType();
                    if (usertype == ModelEnum.UserTypeEnum.manager)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                        {
                            placeholder = "基地",
                            options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zter").id}'", "name,user_sn"),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "zt_user_sn","<%=page.zt_user_sn.value%>"}
                                    },
                                    func = GetYunying,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("yy_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("tg_user_sn").clear()};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                                }
                            }
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelectFull("yy_user_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForOption(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    UserSn = new UserIdentityBag().user_sn,
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                }
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
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                                }
                            }
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder || usertype == ModelEnum.UserTypeEnum.yyer)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                        {
                            placeholder = "厅管账号",
                            options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        });
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        width = "100px",
                        placeholder = "厅名"
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("InfoPost")
                    {
                        title = "InfoPost",
                        text = "开新厅（无申请单）",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "InfoPost"
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
                    var usertype = new ServiceFactory.UserInfo().GetUserType();

                    if (usertype == ModelEnum.UserTypeEnum.manager)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jd_name")
                        {
                            text = "基地",
                            width = "120",
                            minWidth = "120",
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                        {
                            text = "运营账号",
                            width = "120",
                            minWidth = "120",
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder || usertype == ModelEnum.UserTypeEnum.yyer || usertype == ModelEnum.UserTypeEnum.tger)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                        {
                            text = "厅管账号",
                            width = "120",
                            minWidth = "120",
                        });
                    }

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅名",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_user")
                    {
                        text = "大头抖音号",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jjr_name")
                    {
                        text = "运营经纪人",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_user")
                    {
                        text = "抖音号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_UID")
                    {
                        text = "UID",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_sex")
                    {
                        text = "男女厅",
                        width = "120",
                        minWidth = "120"
                    });


                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "InfoEdit",
                            field_paras = "id"
                        },
                        style = "",
                        text = "编辑",
                        name = "Post"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "关厅",
                        title = "关厅",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = ShutDownAction,
                            field_paras = "id"
                        }
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Transform",
                            field_paras = "id"
                        },
                        style = "",
                        text = "转移",
                        name = "Transform",
                        disabled = true,
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "DouUser",
                            field_paras = "id"
                        },
                        style = "",
                        text = "大头号",
                        disabled = true,
                        name = "DouUser"
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
                    var req = reqJson.GetPara();
                    string where = $"status = {ModelDb.user_info_tg.status_enum.正常.ToSByte()}";

                    //查询条件
                    if (!req["ting_name"].IsNullOrEmpty())
                    {
                        where += $" and ting_name like '%{req["ting_name"].ToString()}%'";
                    }

                    if (!req["tg_user_sn"].IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"].ToString()}'";
                    }

                    if (!req["yy_user_sn"].IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in({new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(req["yy_user_sn"].ToString())})";
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " create_time desc",
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_tg, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_tg
                {
                    public string jd_name
                    {
                        get
                        {
                            var jd_name = new ServiceFactory.UserInfo.Zt().GetInfoByUserSn(zt_user_sn).username;
                            if (jd_name.IsNullOrEmpty())
                            {
                                jd_name = "无所属基地";
                            }
                            return jd_name;
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string open_ting_time_text
                    {
                        get
                        {
                            return open_ting_time.ToString();
                        }
                    }
                    public string birthday_date
                    {
                        get
                        {
                            return birthday.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string current_open_dangwei_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", current_open_dangwei);
                        }
                    }
                    public string mbti_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", mbti);
                        }
                    }
                    public string join_party_time_text
                    {
                        get
                        {
                            return join_party_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                public JsonResultAction StartUpAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();

                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.GetPara("id").ToInt());
                    user_info_tg.status = ModelDb.user_info_tg.status_enum.逻辑删除.ToSByte();

                    user_info_tg.Update();

                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"删除厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},{user_info_tg.ting_sn}");
                    return result;
                }

                /// <summary>
                /// 关厅
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction ShutDownAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.GetPara("id").ToInt());
                    new ServiceFactory.UserInfo.Ting().CloseTing(user_info_tg.ting_sn);
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
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYunying(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();

                    result.data = new ServiceFactory.UserInfo.Yy().GetBaseInfosForOption(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                        {
                            UserSn = req["zt_user_sn"].ToNullableString(),
                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                        }
                    }).ToJson();
                    return result;
                }
                #endregion
            }

            /// <summary>
            /// 创建页面
            /// </summary>
            public class TgInfoPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                    string tg_user_sn = req.tg_user_sn;
                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(req.out_para.ToInt());
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("out_para")
                    {
                        defaultValue = req.out_para.ToString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "所属运营",
                        defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn,
                        colLength = 6,
                        index = 100,
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page_post.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        title = "所属厅管",
                        colLength = 6,
                        defaultValue = tg_user_sn,
                        options = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn),
                        index = 110,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_ting_sn")
                    {
                        title = "所属主厅",
                        placeholder = "训练厅选填",
                        colLength = 6,
                        defaultValue = user_info_ting_apply.p_ting_sn,
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                        {
                            attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                            {
                                userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                                UserSn = user_info_ting_apply.tg_user_sn.IsNullOrEmpty() ? tg_user_sn : user_info_ting_apply.tg_user_sn,
                            },
                            attachWhere = "p_ting_sn = ''",
                        }),
                        index = 110,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_user")
                    {
                        title = "抖音号大头",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = req.dou_user,
                        index = 130,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        title = "厅名",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = req.ting_name,
                        index = 133,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "经纪人名字",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = req.jjr_name,
                        index = 134,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        title = "男厅/女厅",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                        {"男","男"},
                        {"女","女"},
                    },
                        colLength = 6,
                        index = 140,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("open_ting_time")
                    {
                        title = "开厅时间",
                        colLength = 6,
                        mold = ModelBasic.EmtTimeSelect.Mold.time,
                        defaultValue = DateTime.Now.TimeOfDay.ToString(),
                        index = 190,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("current_open_dangwei")
                    {
                        title = "开厅时间段",
                        colLength = 6,
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        index = 200,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("join_party_time")
                    {
                        title = "加入公会时间",
                        colLength = 6,
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        index = 210,
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string tg_user_sn { get; set; }
                    public string relation_type { get; set; } = "";
                    public string user_type { get; set; }
                    public int out_para { get; set; }

                    /// <summary>
                    /// 直播厅名
                    /// </summary>
                    public string ting_name { get; set; }

                    /// <summary>
                    /// 抖音大头号
                    /// </summary>
                    public string dou_user { get; set; }
                    /// <summary>
                    /// 运营经纪人
                    /// </summary>
                    public string jjr_name { get; set; }

                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_tg = req.GetPara<ModelDb.user_info_tg>();
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_info_tg.tg_user_sn}'", false);
                    if (user_info_tg.ting_name.IsNullOrEmpty())
                    {
                        throw new Exception("请填写厅名");
                    }
                    if (user_info_tg.dou_user.IsNullOrEmpty())
                    {
                        throw new Exception("请填写抖音大头号");
                    }
                    if (user_info_tg.jjr_name.IsNullOrEmpty())
                    {
                        throw new Exception("请填写经纪人名字");
                    }
                    if (user_info_tg.manager_wx.IsNullOrEmpty())
                    {
                        throw new Exception("请填写管理微信号");
                    }
                    #region 绑定运营经营人
                    var JjrParam = new
                    {
                        jjr_name = user_info_tg.jjr_name
                    };
                    var JjrInfo = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Tg/GetJjrInfo", JjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    if (JjrInfo["code"].ToNullableString() == "1") throw new WeicodeException($@"运营经营人:""{user_info_tg.jjr_name}""不存在");

                    user_info_tg.jjr_uid = JjrInfo["data"]["broker_id"].ToNullableString();
                    #endregion

                    #region 获取抖音UID


                    Zhubo zhubo1 = new Zhubo();
                    dynamic dyCheckResult = zhubo1.VerificationDoUser(user_info_tg.dou_user);

                    user_info_tg.dou_UID = dyCheckResult != null ? dyCheckResult.dyCheckResult.anchor_id : "";


                    #endregion


                    if (user_base.IsNullOrEmpty()) throw new WeicodeException($@"厅管:""{new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_tg.tg_user_sn).username}""不存在");
                    user_info_tg.yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_tg.tg_user_sn).yy_sn;
                    user_info_tg.zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_info_tg.yy_user_sn).zt_user_sn;
                    user_info_tg.ting_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_info_tg.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    user_info_tg.InsertOrUpdate();

                    // 推送公众号
                    new ServiceFactory.Sdk.WeixinSendMsg().Approve("20250317180100680-405941020", $"直播厅:{user_info_tg.ting_name}开厅成功,抖音后台待处理", $"", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
                    {
                        person = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).name,
                        post_time = DateTime.Now
                    });

                    // 日志
                    new ServiceFactory.UserInfo.Ting().AddTingLog(ModelDb.user_info_ting_log.c_type_enum.开厅, $"创建一个直播厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},所属厅管：{new DomainBasic.UserApp().GetInfoByUserSn(user_info_tg.tg_user_sn).username}", user_info_tg);
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"创建直播厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},{user_info_tg.ting_sn}");


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
                    var option = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(req["yy_user_sn"].ToNullableString());
                    result.data = option.ToJson();
                    return result;
                }
                #endregion
            }


            /// <summary>
            /// 编辑页面
            /// </summary>
            public class TgInfoEdit
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.id, false);
                    string tg_user_sn = user_info_tg.tg_user_sn;
                    if (!req.tg_user_sn.IsNullOrEmpty()) tg_user_sn = req.tg_user_sn;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_tg.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("tg_user_sn")
                    {
                        defaultValue = user_info_tg.tg_user_sn.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_user")
                    {
                        title = "大头抖音号",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = user_info_tg.dou_user,
                        index = 130,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        title = "厅名",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = user_info_tg.ting_name,
                        index = 133,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "经纪人名字",
                        isRequired = true,
                        colLength = 6,
                        defaultValue = user_info_tg.jjr_name,
                        index = 134,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        title = "男厅/女厅",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                        {"男","男"},
                        {"女","女"},
                    },
                        colLength = 6,
                        defaultValue = user_info_tg.tg_sex,
                        index = 140,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("current_open_dangwei")
                    {
                        title = "开厅时间段",
                        colLength = 6,
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        defaultValue = user_info_tg.current_open_dangwei,
                        index = 200,
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string tg_user_sn { get; set; }
                    public string relation_type { get; set; } = "";
                    public string user_type { get; set; }
                    public int out_para { get; set; }
                    /// <summary>
                    /// 运营经纪人
                    /// </summary>
                    public string jjr_name { get; set; }

                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_tg = req.GetPara<ModelDb.user_info_tg>();
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_info_tg.tg_user_sn}'", false);

                    #region 绑定运营经营人
                    var JjrParam = new
                    {
                        dou_username = user_info_tg.jjr_name
                    };
                    var JjrInfo = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Tg/GetJjrInfo", JjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    if (JjrInfo["code"].ToNullableString() == "1") throw new WeicodeException($@"运营经营人:""{user_info_tg.jjr_name}""不存在");


                    Zhubo zhubo1 = new Zhubo();
                    dynamic jjrdyCheckResult = zhubo1.VerificationJjr(user_info_tg.jjr_name);

                    user_info_tg.jjr_uid = jjrdyCheckResult != null ? jjrdyCheckResult.dyCheckResult.anchor_id : "";
                    #endregion

                    #region 获取抖音UID
                    dynamic dyCheckResult = zhubo1.VerificationDoUser(user_info_tg.dou_user);

                    user_info_tg.dou_UID = dyCheckResult != null ? dyCheckResult.dyCheckResult.anchor_id : "";

                    #endregion

                    if (user_base.IsNullOrEmpty()) throw new WeicodeException($@"厅管:""{new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_tg.tg_user_sn).username}""不存在");
                    user_info_tg.yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_tg.tg_user_sn).yy_sn;
                    user_info_tg.zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_info_tg.yy_user_sn).zt_user_sn;
                    user_info_tg.ting_sn = user_info_tg.tg_user_sn;
                    user_info_tg.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    user_info_tg.InsertOrUpdate();

                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"编辑直播厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},{user_info_tg.ting_sn}");
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
                    var option = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(req["yy_user_sn"].ToNullableString());
                    result.data = option.ToJson();
                    return result;
                }
                #endregion
            }

            #endregion


            #region 恢复直播厅
            /// <summary>
            /// 
            /// </summary>
            public class CloseList
            {
                #region DefaultView
                /// <summary>
                /// 
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
                    var usertype = new ServiceFactory.UserInfo().GetUserType();
                    if (usertype == ModelEnum.UserTypeEnum.manager)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                        {
                            placeholder = "基地",
                            options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("zter").id}'", "name,user_sn"),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "zt_user_sn","<%=page.zt_user_sn.value%>"}
                                    },
                                    func = GetYunying,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("yy_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("tg_user_sn").clear()};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                                }
                            }
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelectFull("yy_user_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForOption(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    UserSn = new UserIdentityBag().user_sn,
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                }
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
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                                }
                            }
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder || usertype == ModelEnum.UserTypeEnum.yyer)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                        {
                            placeholder = "厅管账号",
                            options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        });
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        width = "100px",
                        placeholder = "厅名"
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
                    var usertype = new ServiceFactory.UserInfo().GetUserType();

                    if (usertype == ModelEnum.UserTypeEnum.manager)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jd_name")
                        {
                            text = "基地",
                            width = "120",
                            minWidth = "120",
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                        {
                            text = "运营账号",
                            width = "120",
                            minWidth = "120",
                        });
                    }
                    if (usertype == ModelEnum.UserTypeEnum.manager || usertype == ModelEnum.UserTypeEnum.zter || usertype == ModelEnum.UserTypeEnum.jder || usertype == ModelEnum.UserTypeEnum.yyer || usertype == ModelEnum.UserTypeEnum.tger)
                    {
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                        {
                            text = "厅管账号",
                            width = "120",
                            minWidth = "120",
                        });
                    }

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅名",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_user")
                    {
                        text = "大头抖音号",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jjr_name")
                    {
                        text = "运营经纪人",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_user")
                    {
                        text = "抖音号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_UID")
                    {
                        text = "UID",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_sex")
                    {
                        text = "男女厅",
                        width = "120",
                        minWidth = "120"
                    });


                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Restore",
                        style = "",
                        text = "恢复直播厅",
                        title = "恢复直播厅",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = RestoreAction,
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
                    var req = reqJson.GetPara();
                    string where = $"status = {ModelDb.user_info_tg.status_enum.逻辑删除.ToSByte()}";

                    //查询条件
                    if (!req["ting_name"].IsNullOrEmpty())
                    {
                        where += $" and ting_name like '%{req["ting_name"].ToString()}%'";
                    }

                    if (!req["tg_user_sn"].IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"].ToString()}'";
                    }

                    if (!req["yy_user_sn"].IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in({new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(req["yy_user_sn"].ToString())})";
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " create_time desc",
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_tg, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_tg
                {
                    public string jd_name
                    {
                        get
                        {
                            var jd_name = new ServiceFactory.UserInfo.Zt().GetInfoByUserSn(zt_user_sn).username;
                            if (jd_name.IsNullOrEmpty())
                            {
                                jd_name = "无所属基地";
                            }
                            return jd_name;
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string open_ting_time_text
                    {
                        get
                        {
                            return open_ting_time.ToString();
                        }
                    }
                    public string birthday_date
                    {
                        get
                        {
                            return birthday.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string current_open_dangwei_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", current_open_dangwei);
                        }
                    }
                    public string mbti_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", mbti);
                        }
                    }
                    public string join_party_time_text
                    {
                        get
                        {
                            return join_party_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 恢复直播厅
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction RestoreAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> sqls = new List<string>();
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.GetPara("id").ToInt());
                    if (user_info_tg.status != ModelDb.user_info_tg.status_enum.正常.ToSByte())
                    {
                        var yy = new ServiceFactory.UserInfo.Yy().YyGetNextTg(user_info_tg.yy_user_sn);
                        user_info_tg.tg_user_sn = yy.SingleOrDefault(a => string.IsNullOrEmpty(a.f_user_sn))?.user_sn;
                    }
                    //查到当前运营下所有的厅管，
                    user_info_tg.status = ModelDb.user_info_tg.status_enum.正常.ToSByte();
                    user_info_tg.Update();
                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"恢复厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},{user_info_tg.ting_sn}");
                    return result;
                }

                /// <summary>
                /// 关厅
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction ShutDownAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.GetPara("id").ToInt());
                    new ServiceFactory.UserInfo.Ting().CloseTing(user_info_tg.ting_sn);
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
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYunying(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();

                    result.data = new ServiceFactory.UserInfo.Yy().GetBaseInfosForOption(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                        {
                            UserSn = req["zt_user_sn"].ToNullableString(),
                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                        }
                    }).ToJson();
                    return result;
                }
                #endregion
            }


            #endregion



            /// <summary>
            /// 转移厅到新的厅管名下
            /// </summary>
            public class TransformPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.id, false);
                    string tg_user_sn = user_info_tg.tg_user_sn;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_tg.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ting_sn")
                    {
                        defaultValue = user_info_tg.ting_sn.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "所属运营",
                        defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn,
                        index = 100,
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page_post.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        title = "厅管账号",
                        defaultValue = tg_user_sn,
                        options = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn),
                        index = 110,
                    });
                    #endregion
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
                    var user_info_tg = req.GetPara<ModelDb.user_info_tg>();
                    var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_info_tg.tg_user_sn}'", false);
                    if (user_base.IsNullOrEmpty()) throw new Exception($@"厅管:""{new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_tg.tg_user_sn).username}""不存在");

                    List<string> lSql = new List<string>();
                    lSql.Add(user_info_tg.UpdateTran());

                    // 重置冗余主播所属运营和厅管
                    if (!user_info_tg.ting_sn.IsNullOrEmpty())
                    {
                        var user_info_zb = new ModelDb.user_info_zb
                        {
                            yy_user_sn = user_info_tg.yy_user_sn,
                            tg_user_sn = user_info_tg.tg_user_sn
                        };
                        lSql.Add(user_info_zb.UpdateTran($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and ting_sn = '{user_info_tg.ting_sn}'"));

                        var user_info_zhubo = new ModelDb.user_info_zhubo
                        {
                            yy_user_sn = user_info_tg.yy_user_sn,
                            tg_user_sn = user_info_tg.tg_user_sn
                        };
                        lSql.Add(user_info_zhubo.UpdateTran($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and ting_sn = '{user_info_tg.ting_sn}'"));
                    }

                    DoMySql.ExecuteSqlTran(lSql);

                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"转移厅：{new ServiceFactory.UserInfo.Ting().GetTingBySn(user_info_tg.ting_sn).ting_name},{user_info_tg.ting_sn}到厅管：{user_info_tg.tg_user_sn},运营{user_info_tg.yy_user_sn}");
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
                    var option = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(req["yy_user_sn"].ToNullableString());
                    result.data = option.ToJson();
                    return result;
                }
                #endregion
            }

            /// <summary>
            /// 绑定大头号页面
            /// </summary>
            public class SetDouUserPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                    var user_info_tg = DoMySql.FindEntityById<ModelDb.user_info_tg>(req.id, false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_tg.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("dou_user_type")
                    {
                        defaultValue = req.dou_user_type.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ting_sn")
                    {
                        defaultValue = user_info_tg.ting_sn.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "抖音大头号",
                        isRequired = true,
                        colLength = 6,
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public int dou_user_type { get; set; }
                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var reqData = req.data_json.ToModel<DtoReqData>();
                    var dou_username = reqData.dou_username;

                    // 获取UID/主播id/抖音作者id

                    Zhubo zhubo1 = new Zhubo();
                    dynamic dyCheckResult1 = zhubo1.VerificationDoUser(dou_username);

                    var anchor_id = dyCheckResult1.anchor_id;



                    dynamic jjr = zhubo1.GetBrokerByAnchorId(dou_username);
                    // 判断抖音大头号是否绑定过
                    // 经纪人是外宣审核字样的抖音号可以绑定
                    if (jjr.agent_name.ToNullableString().Contains("外宣审核-") || jjr.agent_name.ToNullableString().Contains("经纪人-"))
                    {
                        // 绑定抖音大头号

                        zhubo1.SetJjr(anchor_id, new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn).jjr_uid);
                        // 保存数据
                        switch (reqData.dou_user_type)
                        {
                            case 0:
                                reqData.dou_user = dou_username;
                                reqData.dou_UID = anchor_id;
                                break;
                            case 1:
                                reqData.dou_user1 = dou_username;
                                reqData.dou_UID1 = anchor_id;
                                break;
                            case 2:
                                reqData.dou_user2 = dou_username;
                                reqData.dou_UID2 = anchor_id;
                                break;
                        }
                        reqData.ToModel<ModelDb.user_info_tg>().Update();
                    }
                    // 已有经纪人
                    else
                    {
                        throw new Exception("已有运营经纪人，先解绑");
                    }


                    // 日志
                    new DomainBasic.SystemBizLogApp().Write("账号维护", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"绑定厅大头号：{new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn).ting_name},{reqData.ting_sn}");
                    new ServiceFactory.UserInfo.Ting().AddTingLog(ModelDb.user_info_ting_log.c_type_enum.绑定大头号, $"绑定厅大头号：{new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn).ting_name},{reqData.ting_sn}", reqData.ting_sn);
                    return result;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.user_info_tg
                {
                    public int dou_user_type { get; set; }
                    public string dou_username { get; set; }
                }

                /// <summary>
                /// 调用抖音接口请求参数
                /// </summary>
                public class dyCheckParam : Entity
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
                    /// 经纪人uid
                    /// </summary>
                    public object broker_id { get; set; }
                }
                #endregion
            }
        }
    }
}
