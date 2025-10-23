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
    /// <summary>
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 查看下属的账号数据
        /// </summary>
        public class UserList
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

        #region 待开账号列表

        /// <summary>
        /// 待开账号列表
        /// </summary>
        public class WaitUserList
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
                listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                {
                    options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
                        {"C","C"},
                        {"D","D"},
                    },
                    placeholder = "主播分级",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                {
                    placeholder = "微信账号",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                {
                    placeholder = "抖音账号",
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
                listDisplay.isHideOperate = false;
                listDisplay.isOpenNumbers = true;
                listDisplay.isOpenCheckBox = true;

                #region 显示列

                listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData,
                };
                listDisplay.listItems.Add(new EmtModel.ListItem("id")
                {
                    text = "单号",
                    disabled = true,
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("zb_level_text")
                {
                    width = "130",
                    text = "主播分级",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("sessions_text")
                {
                    width = "200",
                    text = "接档时间",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                {
                    width = "130",
                    text = "微信昵称",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                {
                    width = "130",
                    text = "微信账号",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                {
                    width = "130",
                    text = "抖音账号",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("age")
                {
                    width = "60",
                    text = "年龄",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                {
                    width = "150",
                    text = "地区",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("note")
                {
                    width = "280",
                    text = "说明",
                });

                #endregion 显示列

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = new PageCallBack().GetCallBackUrl("/ZbManage/Account/Create", UpdateInfoZb),
                        field_paras = "callback_para=id,user_info_zb_id=id"
                    },
                    text = "开通账号",
                    name = "BatchPost",
                    hideWith = new EmtModel.ListOperateItem.HideWith
                    {
                        field = "note",
                        compareType = EmtModel.ListOperateItem.CompareType.等于,
                        value = "内推人员请先完成背调",
                    },
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = $"BackTuning",
                        field_paras = "id"
                    },
                    text = "背调",
                    hideWith = new EmtModel.ListOperateItem.HideWith
                    {
                        field = "note",
                        compareType = EmtModel.ListOperateItem.CompareType.不等于,
                        value = "内推人员请先完成背调",
                    },
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "/ZbManage/ApplyZb/BackPost",
                        field_paras = "ids=id,tg_need_id"
                    },
                    hideWith = new EmtModel.ListOperateItem.HideWith
                    {
                        compareMode = EmtModel.ListOperateItem.CompareMode.字段比较,
                        compareType = EmtModel.ListOperateItem.CompareType.等于,
                        field = "tuihui_show",
                        value = "1",
                    },
                    text = "退回",
                    name = "BackPost"
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "/ZbManage/ApplyZb/CausePost",
                        field_paras = "id"
                    },
                    hideWith = new EmtModel.ListOperateItem.HideWith
                    {
                        compareMode = EmtModel.ListOperateItem.CompareMode.字段比较,
                        compareType = EmtModel.ListOperateItem.CompareType.等于,
                        field = "liushi_show",
                        value = "1",
                    },
                    text = "流失",
                    name = "CausePost"
                });
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

            /// <summary>
            /// 获取列表
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                //1.校验
                string where = $@"
tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and
mx_sn is not null and
mx_sn != '' and
user_sn = '' and
qun_time is NOT null and
tg_user_sn = '{new UserIdentityBag().user_sn}' AND 
zb_level != '-' and zb_level != 'C' and zb_level != 'D'";
                var status = reqJson.GetPara("status");
                //if (!status.IsNullOrEmpty()) where += $" and status = {status.ToInt()}";
                if (!reqJson.GetPara("sessions").IsNullOrEmpty())
                {
                    where += $" and sessions like '%{reqJson.GetPara("sessions")}%'";
                }
                if (!reqJson.GetPara("tg_need_id").IsNullOrEmpty())
                {
                    var need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{reqJson.GetPara("tg_need_id")}'", false);
                    if (!need.IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{need.tg_sex}'";
                    }
                }
                if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                {
                    where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                }
                if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                {
                    where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                }
                if (!reqJson.GetPara("sessionsIds").IsNullOrEmpty())
                {
                    where += $" and sessions in ({reqJson.GetPara("sessionsIds")})";
                }
                if (!reqJson.GetPara("zb_level").IsNullOrEmpty())
                {
                    where += $" and zb_level = '{reqJson.GetPara("zb_level")}'";
                }
                //2.获取所有厅管的申请操作记录
                var filter = new DoMySql.Filter
                {
                    where = where,
                    orderby = $" order by zb_level"
                };
                return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.user_info_zb
            {
                public string sessions_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", sessions);
                    }
                }
                public string mx_sn_text
                {
                    get
                    {
                        return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).name;
                    }
                }
                public string address_text
                {
                    get
                    {
                        if (!address.IsNullOrEmpty())
                        {
                            return address;
                        }
                        else
                        {
                            return province + city;
                        }
                    }
                }
                public string zb_level_text
                {
                    get
                    {
                        string result = zb_level;
                        if (is_fast == ModelDb.user_info_zb.is_fast_enum.加急.ToSByte())
                        {
                            result = zb_level + "(加急)";
                        }
                        if (is_change == ModelDb.user_info_zb.is_change_enum.换厅.ToSByte())
                        {
                            result = zb_level + "(换厅)";
                        }
                        return result;
                    }
                }
                public string tuihui_show
                {
                    get
                    {
                        return quit_status != ModelDb.user_info_zb.quit_status_enum.未退回.ToInt() || note == "内推人员请先完成背调" ? "1" : "0";
                    }
                }
                public string liushi_show
                {
                    get
                    {
                        return quit_status != ModelDb.user_info_zb.quit_status_enum.未退回.ToInt() || note == "内推人员请先完成背调" ? "1" : "0";
                    }
                }
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

            #region 异步请求处理
            public class DtoReqData : ModelDb.user_base
            {
                /// <summary>
                /// 
                /// </summary>
                public string id { get; set; }
            }

            //public JsonResultAction UpdateInfoZb(JsonRequestAction req)
            //{
            //    var result = new JsonResultAction();

            //    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(req.callback_para.ToInt(),false);
            //    var zbinfo = new ServiceFactory.UserInfo.Zb().GetZbInfo(req.GetPara("user_sn"));
            //    user_info_zb.user_sn = zbinfo.user_sn;
            //    user_info_zb.name = zbinfo.username;
            //    user_info_zb.ting_sn = zbinfo.ting_sn;

            //    user_info_zb.Update();



            //    return result;
            //}
            public JsonResultAction UpdateInfoZb(JsonRequestAction req)
            {
                var result = new JsonResultAction();

                var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(req.callback_para.ToInt(), false);
                var zbinfo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(req.GetPara("user_sn"));
                user_info_zb.user_sn = zbinfo.user_sn;
                user_info_zb.name = zbinfo.username;
                user_info_zb.ting_sn = zbinfo.ting_sn;
                user_info_zb.Update();

                return result;
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
        #endregion 待开账号列表
        

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
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
                var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{user_base.user_sn}'",false);
                #region 表单元素

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_sn")
                {
                    defaultValue = user_base.user_sn
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_info_zb_id")
                {
                    defaultValue = req.user_info_zb_id.ToString()
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
                    title = "登录账号",
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
                public int user_info_zb_id { get; set; }

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
                var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(req.GetPara("user_info_zb_id").ToInt(), false);


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
                    string user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_base.user_sn = user_sn;
                    user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode(user_base.user_type).id.ToSByte();
                    if (!new ServiceFactory.UserService().Post(user_base, relation_type))
                    {
                        throw new WeicodeException("创建失败");
                    }

                    if (relation_type == ModelEnum.UserRelationTypeEnum.运营邀厅管)
                    {
                        new ModelDb.user_info_tg
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            yy_user_sn = new UserIdentityBag().user_sn,
                            ting_sn = user_base.user_sn,    // UtilityStatic.CommonHelper.CreateUniqueSn(),
                            tg_user_sn = user_base.user_sn,
                            ting_name = user_base.username,
                        }.Insert();
                    }

                    if (relation_type == ModelEnum.UserRelationTypeEnum.厅管邀主播)
                    {
                        user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_info_zb_sn = '{user_info_zb.user_info_zb_sn}'");
                        var user_info_zhubo = user_info_zb.ToModel<ModelDb.user_info_zhubo>();

                        user_info_zhubo.user_sn = user_sn;
                        user_info_zhubo.user_name = user_base.name;
                        user_info_zhubo.ting_sn = user_info_zb.tg_user_sn;
                        user_info_zhubo.sources_name = "手工创建";
                        user_info_zhubo.sources_memo = "通过旧的端口手动创建，需要删除";

                        List<string> lSql = new List<string>();
                        lSql.Add(user_info_zhubo.InsertOrUpdateTran($"user_sn = '{user_sn}'"));
                        DoMySql.ExecuteSqlTran(lSql);
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
                
                user_info_zb.full_or_part = user_base.attach1;
                user_info_zb.position = user_base.attach2;
                if (!user_info_zb.IsNullOrEmpty())
                {
                    switch (user_info_zb.position)
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                            user_info_zb.level = "A";
                            break;
                        case "6":
                            user_info_zb.level = "B";
                            break;
                        case "7":
                            user_info_zb.level = "C";
                            break;
                        default:
                            break;
                    }
                    user_info_zb.Update();
                }

                result.data = new
                {
                    user_sn = user_base.user_sn
                };
                return result;
            }

            #endregion 异步请求处理
        }

        /// <summary>
        /// 管理端创建账号页面
        /// </summary>
        public class ManagerCreateTg : UserCreate
        {
            public new ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

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
                    placeholder = "厅名",
                    isRequired = true,
                    defaultValue = user_base.username
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    title = "所属运营",
                    isRequired = true,
                    options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "name,user_sn"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("f_user_sn").options(@"JSON.parse(res.data)")};"
                        }
                    },
                    defaultValue = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, user_base.user_sn)
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                {
                    title = "厅管",
                    isRequired = true,
                    defaultValue = user_base.name
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                {
                    title = "手机号",
                    isRequired = true,
                    defaultValue = user_base.mobile
                });


                if (user_base.IsNullOrEmpty())//创建账号
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                    {
                        title = "设置密码",
                        Content = "123456"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("f_user_sn")
                    {
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
                    //不能绑定上级厅管为自己,即把当前厅管从列表中删除
                    var options = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, user_base.user_sn));
                    options.RemoveAll(item => item.value == user_base.user_sn);
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

            public new JsonResultAction PostAction(JsonRequestAction req)
            {
                var user_base = req.data_json.ToModel<ServiceFactory.UserService.user_base>();

                var result = new JsonResultAction();
                if (user_base.id == 0)
                {
                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }

                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                    user_base.password = "123456";
                    user_base.user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("tger").id.ToSByte();

                    if (!new ServiceFactory.UserService().ManagerPostTg(user_base, req.GetPara()["yy_user_sn"].ToString()))
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
                    var TgDto = new ModelDbBasic.user_base();
                    TgDto.id = user_base.id;
                    TgDto.name = req.GetPara("name");
                    TgDto.username = req.GetPara("username");
                    TgDto.mobile = req.GetPara("mobile");
                    if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        TgDto.attach2 = req.GetPara("attach2");
                    }
                    if (!req.GetPara("password").IsNullOrEmpty()) TgDto.password = req.GetPara("password");
                    lSql.AddRange(new DomainBasic.UserApp().SetUserInfoByEntityTran(TgDto));
                    //3.修改厅管上级厅管
                    var parent_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, TgDto.user_sn);
                    if (parent_tg_user_sn != user_base.f_user_sn)
                    {//若上级厅管更改
                        //3.1.如果原来有上级厅管则删除原来的上级厅管
                        if (!parent_tg_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, parent_tg_user_sn, TgDto.user_sn));
                        //3.2.如果有新的上级厅管则添加新的上级厅管
                        if (!user_base.f_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, user_base.f_user_sn, TgDto.user_sn));
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                }

                var tgInfo = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_base.user_sn);
                
                return result;
            }

            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new List<ModelDoBasic.Option>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString()))
                {
                    if (item.user_sn != req["tg_user_sn"].ToNullableString())
                    {
                        option.Add(new ModelDoBasic.Option
                        {
                            value = item.user_sn,
                            text = item.name,
                        });
                    }
                }
                result.data = option.ToJson();
                return result;
            }
        }

        public class ManagerCreateZb : UserCreate
        {
            public new ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                var f_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn);

                #region 表单元素

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                {
                    title = "登录账号",
                    isRequired = true,
                    defaultValue = user_base.username
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    title = "所属运营",
                    isRequired = true,
                    options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "name,user_sn"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("f_user_sn").options(@"JSON.parse(res.data)")};"
                        }
                    },
                    defaultValue = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, f_user_sn)
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("name")
                {
                    defaultValue = user_base.name
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                {
                    title = "手机号",
                    isRequired = true,
                    defaultValue = user_base.mobile
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("attach1")
                {
                    title = "兼职/全职",
                    options = new Dictionary<string, string>
                    {
                        {"兼职","兼职"},
                        {"全职","全职"}
                    },
                    isRequired = true,
                    defaultValue = user_base.attach1
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("attach2")
                {
                    title = "职务",
                    options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                    defaultValue = user_base.attach2.IsNullOrEmpty() ? "5" : user_base.attach2,
                });

                if (user_base.IsNullOrEmpty())//创建账号
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                    {
                        title = "设置密码",
                        Content = "123456"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("f_user_sn")
                    {
                        isRequired = true,
                        title = "所属厅管"
                    });
                }
                else//编辑账号
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                    {
                        title = "设置密码",
                        placeholder = "不填代表不修改密码"
                    });

                    var options = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, f_user_sn));
                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("f_user_sn")
                    {
                        isRequired = true,
                        options = options,
                        title = "所属厅管",
                        defaultValue = f_user_sn,
                    });
                }

                #endregion 表单元素

                return formDisplay;
            }

            public new JsonResultAction PostAction(JsonRequestAction req)
            {
                var user_base = req.data_json.ToModel<ServiceFactory.UserService.user_base>();

                var result = new JsonResultAction();
                //如果创建
                if (user_base.id == 0)
                {
                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                    if (user_base.f_user_sn.IsNullOrEmpty()) throw new Exception("所属厅管不可为空");
                    user_base.password = "123456";
                    user_base.user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    user_base.user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("zber").id.ToSByte();
                    if (!new ServiceFactory.UserService().ManagerPostZb(user_base, req.GetPara()["f_user_sn"].ToString()))
                    {
                        throw new WeicodeException("创建失败");
                    }
                }
                //如果编辑
                else
                {
                    //1.校验信息
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");

                    var lSql = new List<string>();
                    //2.修改厅管基本信息
                    var ZbDto = new ModelDbBasic.user_base();
                    ZbDto.id = user_base.id;
                    ZbDto.name = req.GetPara("name");
                    ZbDto.username = req.GetPara("username");
                    ZbDto.mobile = req.GetPara("mobile");
                    ZbDto.attach1 = req.GetPara("attach1");
                    if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        ZbDto.attach2 = req.GetPara("attach2");
                    }
                    if (!req.GetPara("password").IsNullOrEmpty()) ZbDto.password = req.GetPara("password");
                    lSql.AddRange(new DomainBasic.UserApp().SetUserInfoByEntityTran(ZbDto));
                    //3.修改上级厅管
                    var parent_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, ZbDto.user_sn);
                    if (parent_tg_user_sn != user_base.f_user_sn)
                    {//若上级厅管更改
                        //3.1.如果原来有上级厅管则删除原来的上级厅管
                        if (!parent_tg_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, parent_tg_user_sn, ZbDto.user_sn));
                        //3.2.如果有新的上级厅管则添加新的上级厅管
                        if (!user_base.f_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.f_user_sn, ZbDto.user_sn));
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                }
                return result;
            }

            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new List<ModelDoBasic.Option>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString()))
                {
                    if (item.user_sn != req["tg_user_sn"].ToNullableString())
                    {
                        option.Add(new ModelDoBasic.Option
                        {
                            value = item.user_sn,
                            text = item.name,
                        });
                    }
                }
                result.data = option.ToJson();
                return result;
            }
        }

        public class UserUnDel
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

                listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                {
                    width = "100px",
                    placeholder = "登录账号"
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
                /*
                 * buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("UnDel")
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
                 */

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
                    text = "登录账号",
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
                    name = "UnDel",
                    style = "",
                    text = "恢复",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = UnDeletesAction,
                        field_paras = "id"
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
            }

            #endregion DefaultView

            #region ListData

            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{new UserIdentityBag().user_sn}'");
                string where = $"user_base.status = '{ModelDb.user_base.status_enum.逻辑删除.ToSByte()}'";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                //查询条件
                if (!dtoReqListData.keyword.IsNullOrEmpty()) where += $" AND (name like '%{dtoReqListData.keyword}%' OR introduce like '%{dtoReqListData.keyword}%')";

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by user_base.id desc ",
                    on = "user_base.user_sn=user_info_zb.user_sn"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ModelDb.user_base, ItemDataModel>(filter, reqJson);
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
                public string zhiwu
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", this.attach2);
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
            public JsonResultAction UnDeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                var user_base = new DomainBasic.UserApp().GetInfoById(dtoReqData.id.ToInt());
                if (user_base.status == ModelDb.user_base.status_enum.逻辑删除.ToSByte())
                {
                    new DomainBasic.UserApp().UnDelInfoByUserSn(user_base.user_sn);
                }
                return result;
            }

            public class DtoReqData : ModelDb.user_base
            {
                public string id { get; set; }
            }

            #endregion 异步请求处理
        }

        #region 背调
        /// <summary>
        /// 背调提交表单
        /// </summary>
        public class BdPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.style = @"background-image:url('/Assets/images/qgxkt_m.jpg');background-size: cover;background-position: center; background-repeat: no-repeat;margin:5px;";
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                    attachPara = new Dictionary<string, object>
                    {
                        {"id", req.id }
                    }
                };
                pageModel.postedReturn = new PagePost.PostedReturn
                {
                    returnType = PagePost.PostedReturn.ReturnType.刷新父窗口
                };
                return pageModel;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
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
                var user_info_zb = DomainBasicStatic.DoMySql.FindEntity<ModelDb.user_info_zb>($"id = '{req.id}'", false);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                {
                    title = "01 微信昵称",
                    colLength = 10,
                    isRequired = true,
                    style = "background-color: transparent;border:1px solid #cccccc",
                    defaultValue = user_info_zb.wechat_nickname,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                {
                    title = "02 微信账号",
                    colLength = 10,
                    isRequired = true,
                    defaultValue = user_info_zb.wechat_username,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                {
                    title = "03 抖音账号",
                    colLength = 10,
                    isRequired = true,
                    defaultValue = user_info_zb.dou_username,
                    style = "background-color: transparent;border:1px solid #cccccc",
                    placeholder = "（填写自己与公会签约的抖音号）"
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                {
                    title = "04 抖音昵称",
                    colLength = 10,
                    isRequired = true,
                    defaultValue = user_info_zb.dou_nickname,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtRadio("zb_sex")
                {
                    title = "05 性别",
                    isRequired = true,
                    options = new Dictionary<string, string>
                    {
                        {"男","男" },
                        {"女","女" },
                    },
                    defaultValue = user_info_zb.zb_sex,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("age")
                {
                    title = "06 年龄",
                    isRequired = true,
                    defaultValue = user_info_zb.age.ToString(),
                    style = "background-color: transparent;border:1px solid #cccccc",
                    placeholder = "（真实年龄）"
                });
                formDisplay.formItems.Add(new ModelBasic.EmtRadio("job")
                {
                    title = "07 现实工作",
                    isRequired = true,
                    options = new Dictionary<string, string>
                    {
                        {"宝妈","宝妈"},
                        {"上班族","上班族"},
                        {"学生党","学生党"},
                        {"自由职业者","自由职业者"},
                        {"其他","其他"}
                    },
                    defaultValue = user_info_zb.job,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("province")
                {
                    title = "08 所在省份",
                    colLength = 10,
                    isRequired = true,
                    defaultValue = user_info_zb.province,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("city")
                {
                    title = "09 所在城市",
                    colLength = 10,
                    isRequired = true,
                    defaultValue = user_info_zb.city,
                    style = "background-color: transparent;border:1px solid #cccccc",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessions")
                {
                    title = "10 [多选]接档时间(尽量不要选21-24，分厅比较慢)",
                    isRequired = true,
                    bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                    style = "background-color: transparent;border:1px solid #cccccc",
                    defaultValue = user_info_zb.sessions
                });
                formDisplay.formItems.Add(new ModelBasic.EmtRadio("full_or_part")
                {
                    title = "11 兼职/全职",
                    isRequired = true,
                    options = new Dictionary<string, string>
                    {
                            {"兼职","兼职" },
                            {"全职","全职" },
                    },
                    style = "background-color: transparent;border:1px solid #cccccc",
                    defaultValue = user_info_zb.full_or_part,
                });

                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }
            }
            #endregion

            #region 异步请求处理
            /// <summary>
            /// 更新萌新数据
            /// </summary>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var info = new JsonResultAction();
                try
                {
                    //user_info_zb表更新背调信息
                    var user_info_zb = req.GetPara().ToModel<ModelDb.user_info_zb>();
                    if (user_info_zb.dou_username.IsNullOrEmpty()) throw new WeicodeException("请填写抖音账号");
                    if (user_info_zb.dou_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写抖音昵称");
                    if (user_info_zb.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请填写微信账号");
                    if (user_info_zb.wechat_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写微信昵称");
                    if (!DoMySql.FindEntity<ModelDb.user_info_zb>($"wechat_username='{user_info_zb.wechat_username}' and tui_status!=2 and id!='{user_info_zb.id}'", false).IsNullOrEmpty())
                    {
                        throw new WeicodeException("微信账号已存在");
                    }
                    if (user_info_zb.zb_sex.IsNullOrEmpty()) throw new WeicodeException("请填写性别");
                    if (user_info_zb.age <= 0 || user_info_zb.age.ToString().IsNullOrEmpty()) throw new WeicodeException("请填写年龄");
                    if (user_info_zb.job.IsNullOrEmpty()) throw new WeicodeException("请填写现实工作");
                    if (user_info_zb.province.IsNullOrEmpty()) throw new WeicodeException("请填写所在省份");
                    if (user_info_zb.city.IsNullOrEmpty()) throw new WeicodeException("请填写所在城市");
                    if (user_info_zb.sessions.IsNullOrEmpty()) throw new WeicodeException("请填写接档时间");
                    if (user_info_zb.full_or_part.IsNullOrEmpty()) throw new WeicodeException("请填写兼职/全职");
                    if (user_info_zb.full_or_part == "兼职")
                    {
                        if (user_info_zb.sessions.Split(',').Length != 1)
                        {
                            throw new Exception("兼职主播请选择1个接档时间");
                        }
                    }
                    if (user_info_zb.full_or_part == "全职")
                    {
                        if (user_info_zb.sessions.Split(',').Length != 2)
                        {
                            throw new Exception("全职主播请选择2个接档时间");
                        }
                    }
                    user_info_zb.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    user_info_zb.note = "";
                    lSql.Add(user_info_zb.UpdateTran($"id = '{req.GetPara("id")}'"));

                    DoMySql.ExecuteSqlTran(lSql);
                }
                catch (Exception ex)
                {
                    info.code = 1;
                    info.msg = ex.Message;
                }
                return info;
            }
            #endregion
        }
        #endregion
}
}