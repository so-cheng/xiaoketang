using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// 待开账号模块
    /// </summary>
    partial class PageFactory
    {
        public partial class JoinNew
        {
            #region 待开账号列表

            /// <summary>
            /// 待开账号列表
            /// </summary>
            public class WaitOpenAccountList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level")
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
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = new PageCallBack().GetCallBackUrl("Create", UpdateInfoZb),
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
                    //TODO 退回账号萌新退还新主播，不再改变申请单的状态
                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = "/ZbManage/ApplyZb/BackPost",
                    //        field_paras = "ids=id,tg_need_id"
                    //    },
                    //    hideWith = new EmtModel.ListOperateItem.HideWith
                    //    {
                    //        compareMode = EmtModel.ListOperateItem.CompareMode.字段比较,
                    //        compareType = EmtModel.ListOperateItem.CompareType.等于,
                    //        field = "tuihui_show",
                    //        value = "1",
                    //    },
                    //    text = "退回",
                    //    name = "BackPost"
                    //});
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost"
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
                        status = '{ModelDb.user_info_zhubo.status_enum.待开账号.ToInt()}' and
                        tg_user_sn = '{new UserIdentityBag().user_sn}' ";
                    if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                    {
                        where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
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
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zhubo
                {
                    public string sessions_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", sessions);
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
                }
                #region 异步请求处理
                public JsonResultAction UpdateInfoZb(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.callback_para.ToInt(), false);
                    var zbinfo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(req.GetPara("user_sn"));
                    user_info_zhubo.user_sn = zbinfo.user_sn;
                    user_info_zhubo.ting_sn = zbinfo.ting_sn;
                    user_info_zhubo.Update();
                    return result;
                }
                #endregion 异步请求处理
            }

            /// <summary>
            /// 主播流失操作
            /// </summary>
            public class CausePost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                    {
                        {"id",req.id },
                    }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "ids",
                        colLength = 6,
                        defaultValue = req.id,
                    });

                    formDisplay.formItems.Add(new EmtTimeSelect("date")
                    {
                        mold = EmtTimeSelect.Mold.date,
                        title = "流失时间",
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });
                    formDisplay.formItems.Add(new EmtTimeSelect("training_date")
                    {
                        mold = EmtTimeSelect.Mold.date,
                        title = "培训时间",
                        colLength = 6,
                        defaultValue = "",
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "流失原因",
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public string id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    string date = req.GetPara()["date"].ToString();
                    if (date.IsNullOrEmpty()) { throw new Exception("请填写流失时间"); }
                    if (req.GetPara("cause").IsNullOrEmpty()) { throw new Exception("请填写流失原因"); }
                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("id").ToInt());
                    user_info_zhubo.no_share = $"流失时间:{date},原因:{req.GetPara("cause")}";
                    user_info_zhubo.status = ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte();
                    user_info_zhubo.training_date = req.GetPara("traning_date").ToDateTime();
                    lSql.Add(user_info_zhubo.UpdateTran($"id = {user_info_zhubo.id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }

            /// <summary>
            /// 创建/编辑主播账号
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
                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id='{req.user_info_zb_id}'", false);
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
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "昵称",
                        placeholder = "",
                        isRequired = true,
                        defaultValue = user_base.username
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("name")
                    {
                        defaultValue = user_base.name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true,
                        defaultValue = user_info_zhubo.mobile
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
                        defaultValue = user_info_zhubo.full_or_part,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("attach2")
                    {
                        title = "职务",
                        options = new DomainBasic.DictionaryApp().GetListForOption("职务"),
                        defaultValue = user_info_zhubo.position.IsNullOrEmpty() ? "5" : user_info_zhubo.position,
                    });

                    if (user_base.IsNullOrEmpty())//创建账号
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                        {
                            title = "设置密码",
                            Content = "123456"
                        });
                    }
                    else//编辑账号
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                        {
                            title = "设置密码",
                            placeholder = "不填代表不修改密码"
                        });
                    }

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    //user_base的id
                    public int id { get; set; }
                    //user_info_zhubo的id
                    public int user_info_zb_id { get; set; }
                    //当前创建的账号所属类型
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
                        if (!req.GetPara("password").IsNullOrEmpty()) tGDto.password = req.GetPara("password");

                        //TODO attach1和attach2后期可删除，用user_info_zhubo中的full_or_part和position代替，暂保留复用
                        tGDto.attach1 = req.GetPara("attach1");
                        if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                        {
                            tGDto.attach2 = req.GetPara("attach2");
                        }
                        lSql.AddRange(new DomainBasic.UserApp().SetUserInfoByEntityTran(tGDto));

                        DoMySql.ExecuteSqlTran(lSql);
                    }

                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("user_info_zb_id").ToInt(), false);
                    user_info_zhubo.full_or_part = user_base.attach1;
                    user_info_zhubo.position = user_base.attach2;
                    user_info_zhubo.mobile = user_base.mobile;
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                    if (!user_info_zhubo.IsNullOrEmpty())
                    {
                        switch (user_info_zhubo.position)
                        {
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                user_info_zhubo.level = "A";
                                break;
                            case "6":
                                user_info_zhubo.level = "B";
                                break;
                            case "7":
                                user_info_zhubo.level = "C";
                                break;
                            default:
                                break;
                        }
                        user_info_zhubo.Update();
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
            /// 主播信息背调
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
                    var user_info_zhubo = DomainBasicStatic.DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{req.id}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        title = "01 微信昵称",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = user_info_zhubo.wechat_nickname,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "02 微信账号",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = user_info_zhubo.wechat_username,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "03 抖音账号",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = user_info_zhubo.dou_username,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写自己与公会签约的抖音号）"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        title = "04 抖音昵称",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = user_info_zhubo.dou_nickname,
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
                        defaultValue = user_info_zhubo.zb_sex,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("age")
                    {
                        title = "06 年龄",
                        isRequired = true,
                        defaultValue = user_info_zhubo.age.ToString(),
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
                        defaultValue = user_info_zhubo.job,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("province")
                    {
                        title = "08 所在省份",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = user_info_zhubo.province,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("city")
                    {
                        title = "09 所在城市",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = user_info_zhubo.city,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessions")
                    {
                        title = "10 [多选]接档时间(尽量不要选21-24，分厅比较慢)",
                        isRequired = true,
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = user_info_zhubo.sessions
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
                        defaultValue = user_info_zhubo.full_or_part,
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
                        var user_info_zhubo = req.GetPara().ToModel<ModelDb.user_info_zhubo>();
                        if (user_info_zhubo.dou_username.IsNullOrEmpty()) throw new WeicodeException("请填写抖音账号");
                        if (user_info_zhubo.dou_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写抖音昵称");
                        if (user_info_zhubo.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请填写微信账号");
                        if (user_info_zhubo.wechat_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写微信昵称");
                        if (!DoMySql.FindEntity<ModelDb.user_info_zhubo>($"wechat_username='{user_info_zhubo.wechat_username}' and tui_status!=2 and id!='{user_info_zhubo.id}'", false).IsNullOrEmpty())
                        {
                            throw new WeicodeException("微信账号已存在");
                        }
                        if (user_info_zhubo.zb_sex.IsNullOrEmpty()) throw new WeicodeException("请填写性别");
                        if (user_info_zhubo.age <= 0 || user_info_zhubo.age.ToString().IsNullOrEmpty()) throw new WeicodeException("请填写年龄");
                        if (user_info_zhubo.job.IsNullOrEmpty()) throw new WeicodeException("请填写现实工作");
                        if (user_info_zhubo.province.IsNullOrEmpty()) throw new WeicodeException("请填写所在省份");
                        if (user_info_zhubo.city.IsNullOrEmpty()) throw new WeicodeException("请填写所在城市");
                        if (user_info_zhubo.sessions.IsNullOrEmpty()) throw new WeicodeException("请填写接档时间");
                        if (user_info_zhubo.full_or_part.IsNullOrEmpty()) throw new WeicodeException("请填写兼职/全职");
                        if (user_info_zhubo.full_or_part == "兼职")
                        {
                            if (user_info_zhubo.sessions.Split(',').Length != 1)
                            {
                                throw new Exception("兼职主播请选择1个接档时间");
                            }
                        }
                        if (user_info_zhubo.full_or_part == "全职")
                        {
                            if (user_info_zhubo.sessions.Split(',').Length != 2)
                            {
                                throw new Exception("全职主播请选择2个接档时间");
                            }
                        }
                        user_info_zhubo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        user_info_zhubo.note = "";
                        lSql.Add(user_info_zhubo.UpdateTran($"id = '{req.GetPara("id")}'"));

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

            #endregion 待开账号列表
        }
    }
}
