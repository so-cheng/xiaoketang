using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserTable
        {
            public class TgInfoRank
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
                    if (!dtoReqListData.yy_user_sn.IsNullOrEmpty() && dtoReqListData.tg_user_sn.IsNullOrEmpty() && dtoReqListData.zb_user_sn.IsNullOrEmpty()) where += $" AND user_base.user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, dtoReqListData.yy_user_sn)}";
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

                    public string password_text
                    {
                        get
                        {
                            return getPassword_text();
                        }
                    }

                    public string getPassword_text()
                    {
                        if (password.IsNullOrEmpty() || password == "e10adc3949ba59abbe56e057f20f883e")
                        {
                            return "密码信息不完整";
                        }
                        else
                        {
                            return "";
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
                    List<string> lSql = new List<string>();
                    var user_base = new DomainBasic.UserApp().GetInfoById(dtoReqData.id.ToInt());
                    if (user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id && new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn).Count > 0)
                    {
                        throw new Exception("禁止删除名下有主播的厅管");
                    }
                    if (user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id && new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀厅管, user_base.user_sn).Count > 0)
                    {
                        throw new Exception("禁止删除名下有厅管的厅管");
                    }
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
                #endregion 异步请求处理
            }

            public class UserCreate
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
                    var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);

                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_type")
                    {
                        defaultValue = req.user_type.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "所属运营",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and status='{ModelDb.user_base.status_enum.正常}'", "username,user_sn"),
                        isRequired = true,
                        defaultValue = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, user_base.user_sn),
                        displayStatus = EmtModelBase.DisplayStatus.隐藏
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "昵称",
                        placeholder = req.user_type == "tger" ? "厅名" : "",
                        isRequired = true,
                        defaultValue = user_base.username
                    });
                    if (req.user_type == "tger")
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                        {
                            title = "厅管",
                            isRequired = true,
                            defaultValue = user_base.name
                        });
                    }
                    else
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("name")
                        {
                            defaultValue = user_base.name
                        });
                    }

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true,
                        defaultValue = user_base.mobile
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("attach1")
                    {
                        title = "兼职/全职",
                        options = new Dictionary<string, string>
                    {
                        {"兼职","兼职"},
                        {"全职","全职"}
                    },
                        isRequired = true,
                        defaultValue = user_base.attach1,
                        displayStatus = EmtModelBase.DisplayStatus.隐藏
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("attach2")
                    {
                        title = "职务",
                        displayStatus = EmtModelBase.DisplayStatus.隐藏,
                        options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                        defaultValue = user_base.attach2.IsNullOrEmpty() ? "5" : user_base.attach2,
                        //displayStatus = EmtModelBase.DisplayStatus.隐藏
                    });

                    //不能绑定上级厅管为自己,即把当前厅管从列表中删除
                    List<ModelDoBasic.Option> options = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn);
                    options.RemoveAll(item => item.value == user_base.user_sn);

                    if (user_base.IsNullOrEmpty())//创建账号
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                        {
                            title = "设置密码",
                            Content = "123456"
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("f_user_sn")
                        {
                            options = options,
                            title = "所属厅管",
                            placeholder = "如果没有多厅厅管则不选"
                        });
                    }
                    else//编辑账号
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                        {
                            title = "设置密码",
                            placeholder = "不填代表不修改密码"
                        });
                        //获取原来的所属厅管
                        var parent_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, user_base.user_sn);
                        formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("f_user_sn")
                        {
                            options = options,
                            title = "所属厅管",
                            defaultValue = parent_user_sn,
                            placeholder = "如果没有多厅厅管则不选"
                        });
                    }

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }

                    /// <summary>
                    /// 当前创建的账号所属类型
                    /// </summary>
                    public string user_type { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var user_base = req.data_json.ToModel<ServiceFactory.UserService.user_base>();

                    var result = new JsonResultAction();
                    if (user_base.id == 0)
                    {
                        if (user_base.name.IsNullOrEmpty())
                        {
                            user_base.name = user_base.username;
                        }
                        var relation_type = ModelEnum.UserRelationTypeEnum.厅管邀主播;
                        if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id)
                        {
                            relation_type = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                        }
                        if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                        if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                        user_base.password = "123456";
                        user_base.user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                        user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode(user_base.user_type).id.ToSByte();
                        if (!new ServiceFactory.UserService().Post(user_base, relation_type))
                        {
                            throw new WeicodeException("创建失败");
                        }
                    }
                    else
                    {
                        //1.校验信息
                        if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                        if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");

                        var lSql = new List<string>();
                        //2.修改厅管基本信息
                        var tGDto = new ModelDbBasic.user_base();
                        tGDto.id = user_base.id;
                        tGDto.name = req.GetPara("name");
                        tGDto.username = req.GetPara("username");
                        tGDto.mobile = req.GetPara("mobile");
                        tGDto.attach1 = req.GetPara("attach1");
                        if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                        {
                            tGDto.attach2 = req.GetPara("attach2");
                        }
                        if (!req.GetPara("password").IsNullOrEmpty()) tGDto.password = req.GetPara("password");
                        lSql.AddRange(new DomainBasic.UserApp().SetUserInfoByEntityTran(tGDto));
                        //3.修改厅管上级厅管
                        var parent_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tGDto.user_sn);
                        if (parent_tg_user_sn != user_base.f_user_sn)
                        {//若上级厅管更改
                         //3.1.如果原来有上级厅管则删除原来的上级厅管
                            if (!parent_tg_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, parent_tg_user_sn, tGDto.user_sn));
                            //3.2.如果有新的上级厅管则添加新的上级厅管
                            if (!user_base.f_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, user_base.f_user_sn, tGDto.user_sn));
                        }
                        DoMySql.ExecuteSqlTran(lSql);
                    }

                    result.data = new
                    {
                        user_sn = user_base.user_sn
                    };
                    return result;
                }

                #endregion 异步请求处理
            }
        }
    }
    
}
