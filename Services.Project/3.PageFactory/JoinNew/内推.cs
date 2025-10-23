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
    public partial class PageFactory
    {
        public partial class JoinNewPush
        {
            #region 内推申请列表（厅管，中台）
            public class InterpolateApplyList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("interpolateapplylist");
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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dy_account")
                    {
                        width = "140px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        width = "100px",
                        placeholder = "手机后四位",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "100px",
                        options = new Dictionary<string, string>
                        {
                            {"申请中",ModelDb.p_join_push_apply.status_enum.申请中.ToInt().ToString()},
                            {"申请完成",ModelDb.p_join_push_apply.status_enum.申请完成.ToInt().ToString()},
                            {"退回",ModelDb.p_join_push_apply.status_enum.退回.ToInt().ToString()}
                        },
                        disabled = false,
                        placeholder = "申请状态",
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
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isHideOperate = false;
                    listDisplay.operateWidth = "80";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅管名",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("jjr_name")
                    {
                        text = "经纪人后台",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("real_name")
                    {
                        text = "实名名字",
                        width = "90",
                        minWidth = "90",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dy_account")
                    {
                        text = "抖音号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("moblie_lastfour")
                    {
                        text = "手机后四位",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("commission_rate")
                    {
                        text = "主播点位",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("return_reason")
                    {
                        text = "备注",
                        width = "250",
                        minWidth = "250"
                    });

                    #endregion 显示列
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"interpolateApply",
                            field_paras = "id,ting_sn"
                        },
                        text = "修改",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = ModelDb.p_join_push_apply.status_enum.退回.ToInt().ToString()
                        },
                    });
                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("")
                    {
                        title = "内推申请"
                    };
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.tger:
                            var tings = new ServiceFactory.UserInfo.Ting().GetTingsByTgsn(new UserIdentityBag().user_sn);
                            foreach (var ting in tings)
                            {
                                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                                {
                                    text = tings.Count > 1 ? ting.ting_name : "内推申请",
                                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                                    {
                                        url = $"interpolateApply?ting_sn={ting.ting_sn}",
                                    },
                                });
                            }
                            break;
                        case ModelEnum.UserTypeEnum.jder:
                            buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                            {
                                text = "内推申请",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                                {
                                    url = $"interpolateApply",
                                },
                            });
                            break;
                    }

                    return buttonGroup;
                }
                public class DtoReq
                {
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.筛选条件
                    string where = $"1=1";
                    var real_name = reqJson.GetPara("real_name");
                    if (!real_name.IsNullOrEmpty()) where += $" and real_name like '%{real_name}%'";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.tger:
                            where += $" and tg_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.jder:
                            where += $" and ting_sn in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter { attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType { userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地, UserSn = new UserIdentityBag().user_sn } })}";
                            break;
                    }
                    if (!reqJson.GetPara("dy_account").IsNullOrEmpty())
                    {
                        where += $" and dy_account = '{reqJson.GetPara("dy_account")}'";
                    }
                    if (!reqJson.GetPara("moblie_lastfour").IsNullOrEmpty())
                    {
                        where += $" and moblie_lastfour = '{reqJson.GetPara("moblie_lastfour")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status = {reqJson.GetPara("status")}";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = $"ORDER BY create_time DESC"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_push_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_push_apply
                {
                    public string tg_username
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

                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                }

                #endregion ListData
            }

            #endregion

            #region 厅管内推申请操作
            public class NtPost
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
                    var user_base = DomainBasicStatic.DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{new UserIdentityBag().user_sn}'", false);
                    var applyinfo = new ModelDb.p_join_push_apply();
                    if (!req.id.IsNullOrEmpty())
                    {
                        applyinfo = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(req.id);
                    }

                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ting_sn")
                    {
                        defaultValue = req.ting_sn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("username")
                    {
                        title = "厅名",
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        disabled = false,
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.ting_sn).ting_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("commission_rate")
                    {
                        title = "主播点位",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        options = new Dictionary<string, string> {
                            {"28%", "28%"},
                            {"32%", "32%"},
                            {"36%", "36%"},
                        },
                        defaultValue = applyinfo.commission_rate
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "经纪人后台",
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        isRequired = true,
                        defaultValue = user_base.name,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dy_account")
                    {
                        title = "抖音号",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写自己与公会签约的抖音号）",
                        defaultValue = applyinfo.dy_account
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("real_name")
                    {
                        title = "真实姓名",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写本人真实姓名）",
                        defaultValue = applyinfo.real_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        title = "手机后四位",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写手机号的后四位）",
                        defaultValue = applyinfo.moblie_lastfour
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 厅sn
                    /// </summary>
                    public string ting_sn { get; set; }
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 提交内推申请
                /// </summary>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var info = new JsonResultAction();
                    try
                    {
                        var pushParam = req.GetPara().ToModel<ModelDb.p_join_push_apply>();
                        if (pushParam.real_name.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填真实姓名");
                        }
                        if (pushParam.dy_account.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填抖音号");
                        }
                        if (pushParam.moblie_lastfour.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填手机号后四位");
                        }
                        if (pushParam.moblie_lastfour.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填手机号后四位");
                        }
                        if (pushParam.commission_rate.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择主播点位");
                        }

                        var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                        {
                            dou_username = pushParam.dy_account,
                            last_four_number = pushParam.moblie_lastfour,
                            real_name = pushParam.real_name,
                        };
                        var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/CheckUserInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                        {
                            contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                        }).ToJObject();
                        if (dyCheckResult["code"].ToNullableString().Equals("1") && !dyCheckResult["msg"].ToNullableString().Equals("主播已加入公会，无法邀约"))
                        {
                            throw new WeicodeException(dyCheckResult["msg"].ToNullableString());
                        }

                        int id = req.GetPara("id").ToInt();
                        var applyinfo = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(id);
                        applyinfo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        applyinfo.tg_user_sn = new UserIdentityBag().user_sn;
                        applyinfo.ting_sn = req.GetPara("ting_sn");
                        applyinfo.dy_account = pushParam.dy_account;
                        applyinfo.real_name = pushParam.real_name;
                        applyinfo.moblie_lastfour = pushParam.moblie_lastfour;
                        applyinfo.jjr_name = pushParam.jjr_name;
                        applyinfo.commission_rate = pushParam.commission_rate;
                        applyinfo.status = ModelDb.p_join_push_apply.status_enum.申请中.ToInt();
                        //插入申请数据
                        lSql.Add(applyinfo.InsertOrUpdateTran());
                        DoMySql.ExecuteSqlTran(lSql);

                        #region 推送公众号-小昕审批
                        // 推送公众号
                        new ServiceFactory.Sdk.WeixinSendMsg().Approve("20250929154411042-2092974501", "有转介绍补人需要审核", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "manager").host_domain}/JoinNew/InterpolatApprove/ApplyList", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
                        {
                            person = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).name,
                            post_time = DateTime.Now
                        });
                        #endregion
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

            #region 中台内推申请操作
            public class ZtNtPost
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
                    var applyinfo = new ModelDb.p_join_push_apply();
                    if (!req.id.IsNullOrEmpty())
                    {
                        applyinfo = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(req.id);
                    }

                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        title = "直播厅",
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn
                            }
                        }),
                        isRequired = true,
                        defaultValue = applyinfo.ting_sn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "经纪人后台",
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        isRequired = true,
                        defaultValue = applyinfo.jjr_name,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dy_account")
                    {
                        title = "抖音号",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写与公会签约的抖音号）",
                        defaultValue = applyinfo.dy_account
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("real_name")
                    {
                        title = "真实姓名",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写本人真实姓名）",
                        defaultValue = applyinfo.real_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        title = "手机后四位",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写手机号的后四位）",
                        defaultValue = applyinfo.moblie_lastfour
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("commission_rate")
                    {
                        title = "主播点位",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        options = new Dictionary<string, string> {
                            {"28%", "28%"},
                            {"32%", "32%"},
                            {"36%", "36%"},
                        },
                        defaultValue = applyinfo.commission_rate
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 提交内推申请
                /// </summary>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var info = new JsonResultAction();
                    try
                    {
                        var pushParam = req.GetPara().ToModel<ModelDb.p_join_push_apply>();
                        if (pushParam.ting_sn.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择直播厅");
                        }
                        if (pushParam.jjr_name.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填经纪人后台");
                        }
                        if (pushParam.real_name.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填真实姓名");
                        }
                        if (pushParam.dy_account.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填抖音号");
                        }
                        if (pushParam.moblie_lastfour.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填手机号后四位");
                        }
                        if (pushParam.moblie_lastfour.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填手机号后四位");
                        }
                        if (pushParam.commission_rate.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择主播点位");
                        }

                        var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                        {
                            dou_username = pushParam.dy_account,
                            last_four_number = pushParam.moblie_lastfour,
                            real_name = pushParam.real_name,
                        };
                        var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/CheckUserInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                        {
                            contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                        }).ToJObject();
                        if (dyCheckResult["code"].ToNullableString().Equals("1") && !dyCheckResult["msg"].ToNullableString().Equals("主播已加入公会，无法邀约"))
                        {
                            throw new WeicodeException(dyCheckResult["msg"].ToNullableString());
                        }

                        int id = req.GetPara("id").ToInt();
                        var applyinfo = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(id);
                        applyinfo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        applyinfo.tg_user_sn = new ServiceFactory.UserInfo.Ting().GetTingBySn(pushParam.ting_sn).tg_user_sn;
                        applyinfo.ting_sn = pushParam.ting_sn;
                        applyinfo.dy_account = pushParam.dy_account;
                        applyinfo.real_name = pushParam.real_name;
                        applyinfo.moblie_lastfour = pushParam.moblie_lastfour;
                        applyinfo.jjr_name = pushParam.jjr_name;
                        applyinfo.commission_rate = pushParam.commission_rate;
                        applyinfo.status = ModelDb.p_join_push_apply.status_enum.申请中.ToInt();
                        //插入申请数据
                        lSql.Add(applyinfo.InsertOrUpdateTran());
                        DoMySql.ExecuteSqlTran(lSql);

                        #region 推送公众号-小昕审批
                        // 推送公众号
                        new ServiceFactory.Sdk.WeixinSendMsg().Approve("20250929154411042-2092974501", "有转介绍补人需要审核", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "manager").host_domain}/JoinNew/InterpolatApprove/ApplyList", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
                        {
                            person = new ServiceFactory.UserInfo.Ting().GetTingBySn(pushParam.ting_sn).ting_name,
                            post_time = DateTime.Now
                        });
                        #endregion
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

            #region 外宣主管内推审批列表
            public class InterpolateApproveList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("interpolateapprovelist");
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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dy_account")
                    {
                        width = "140px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        width = "100px",
                        placeholder = "手机后四位",
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
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isHideOperate = false;
                    listDisplay.operateWidth = "140";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅管名",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("jjr_name")
                    {
                        text = "经纪人后台",
                        width = "140",
                        minWidth = "140",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("real_name")
                    {
                        text = "实名名字",
                        width = "90",
                        minWidth = "90",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dy_account")
                    {
                        text = "抖音号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("moblie_lastfour")
                    {
                        text = "手机后四位",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("commission_rate")
                    {
                        text = "主播点位",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "160",
                        minWidth = "160"
                    });

                    #endregion 显示列

                    #region 操作列

                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.manager:
                            listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                            {
                                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                                {
                                    func = Approve,
                                    field_paras = "id"
                                },
                                style = "",
                                text = "签约完成",
                            });
                            listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                            {
                                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                                {
                                    url = "RollbackApply",
                                    field_paras = "id"
                                },
                                text = "退回",
                            });
                            break;
                    }

                    #endregion 操作列

                    return listDisplay;
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
                public class DtoReq
                {
                }

                #endregion DefaultView
                #region ListData
                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.筛选条件
                    string where = $"1=1";
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.manager:
                            where += $" and status = {ModelDb.p_join_push_apply.status_enum.申请中.ToInt()}";
                            break;
                        case ModelEnum.UserTypeEnum.yyer:
                            where += $" and tg_user_sn in {new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(new UserIdentityBag().user_sn)}";
                            break;
                        case ModelEnum.UserTypeEnum.tger:
                            where += $" and tg_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        default:
                            where += $" and 1!=1";
                            break;
                    }
                    if (!reqJson.GetPara("dy_account").IsNullOrEmpty())
                    {
                        where += $" and dy_account = '{reqJson.GetPara("dy_account")}'";
                    }
                    if (!reqJson.GetPara("moblie_lastfour").IsNullOrEmpty())
                    {
                        where += $" and moblie_lastfour = '{reqJson.GetPara("moblie_lastfour")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = $" ORDER BY create_time DESC"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_push_apply, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_push_apply
                {
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                }
                #endregion ListData
                #region 异步请求处理
                /// <summary>
                /// 签约成功操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction Approve(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        var userinfozbsn = UtilityStatic.CommonHelper.CreateOrderNo();

                        //修改申请状态
                        int id = req.GetPara("id").ToInt();
                        List<string> lSql = new List<string>();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(id);
                        if (!apply.status.Equals(ModelDb.p_join_push_apply.status_enum.申请中.ToInt())) throw new WeicodeException("状态不是申请中，请刷新页面");
                        apply.status = ModelDb.p_join_push_apply.status_enum.申请完成.ToInt();
                        apply.return_reason = "";
                        apply.user_info_zb_sn = userinfozbsn;
                        lSql.Add(apply.UpdateTran());

                        DoMySql.ExecuteSqlTran(lSql);

                        //内推签约完成通过写入user_info_zhubo中 
                        var yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(apply.tg_user_sn).yy_sn;
                        var user_info_zhubo = new ModelDb.user_info_zhubo
                        {
                            age = 0,
                            devices_num = 0,
                            quality = 0,
                            user_sn = "",
                            mx_sn = "内推无萌新",
                            zb_level = "A",
                            zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).zt_user_sn,
                            yy_user_sn = yy_user_sn,
                            tg_user_sn = apply.tg_user_sn,
                            ting_sn = apply.ting_sn,
                            note = "内推人员请先完成背调",
                            dou_username = apply.dy_account
                        };
                        var zhuboInfoDto = user_info_zhubo.ToModel<ServiceFactory.UserInfo.Zb_NewZhubo.ZhuboInfoDto>();
                        new ServiceFactory.UserInfo.Zb_NewZhubo().CreateNewZhubo("内推签约完成", $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了内推签约完成操作，抖音账号：{user_info_zhubo.dou_username}", zhuboInfoDto);
                    }
                    catch (Exception ex)
                    {
                        result.code = 1;
                        result.msg = ex.Message;
                    }
                    return result;
                }
                #endregion
            }

            #endregion

            #region 外宣主管内推退回操作
            public class RollBackPost
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
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("return_reason")
                    {
                        title = "退回原因",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写退回原因）"
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
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
                        var param = req.GetPara().ToModel<ModelDb.p_join_push_apply>();
                        if (param.return_reason.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请填退回原因");
                        }
                        int id = req.GetPara("id").ToInt();
                        var applyinfo = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(id);
                        applyinfo.return_reason = param.return_reason;
                        applyinfo.status = 2;
                        lSql.Add(applyinfo.UpdateTran());
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
}
