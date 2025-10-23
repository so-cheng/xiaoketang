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
        public partial class JoinPush
        {
            #region 厅管内推申请列表

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
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        placeholder = "手机号后四位",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
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
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅名",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("jjr_name")
                    {
                        text = "经纪人后台",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("real_name")
                    {
                        text = "实名名字",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dy_account")
                    {
                        text = "抖音号",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("moblie_lastfour")
                    {
                        text = "手机后四位",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("return_reason")
                    {
                        text = "备注",
                        width = "300",
                        minWidth = "300"
                    });

                    #endregion 显示列
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"interpolateApply",
                            field_paras = "id"
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
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");

                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "内推申请",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"interpolateApply?id={0}",
                        }
                    });
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
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        where += $" and tg_user_sn='{new UserIdentityBag().user_sn}'";
                    }
                    if (!reqJson.GetPara("dy_account").IsNullOrEmpty())
                    {
                        where += $" and dy_account='{reqJson.GetPara("dy_account")}'";
                    }
                    if (!reqJson.GetPara("moblie_lastfour").IsNullOrEmpty())
                    {
                        where += $" and moblie_lastfour='{reqJson.GetPara("moblie_lastfour")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status='{reqJson.GetPara("status")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
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
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("username")
                    {
                        title = "厅名",
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        disabled = false,
                        defaultValue = user_base.username,
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

                        var dyParam = new ServiceFactory.Join.dyCheckParam() {
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
                        applyinfo.dy_account = pushParam.dy_account;
                        applyinfo.real_name = pushParam.real_name;
                        applyinfo.moblie_lastfour = pushParam.moblie_lastfour;
                        applyinfo.jjr_name = pushParam.jjr_name;
                        applyinfo.status = ModelDb.p_join_push_apply.status_enum.申请中.ToInt();
                        //插入申请数据
                        lSql.Add(applyinfo.InsertOrUpdateTran());
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
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        placeholder = "手机号后四位",
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
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅名",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("jjr_name")
                    {
                        text = "经纪人后台",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("real_name")
                    {
                        text = "实名名字",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dy_account")
                    {
                        text = "抖音号",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("moblie_lastfour")
                    {
                        text = "手机后四位",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120"
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = approve,
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
                            url = $"rollbackApply",
                            field_paras = "id"
                        },
                        text = "退回",
                    });

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
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        where += $" and tg_user_sn='{new UserIdentityBag().user_sn}'";
                    }
                    if (!reqJson.GetPara("dy_account").IsNullOrEmpty())
                    {
                        where += $" and dy_account='{reqJson.GetPara("dy_account")}'";
                    }
                    if (!reqJson.GetPara("moblie_lastfour").IsNullOrEmpty())
                    {
                        where += $" and moblie_lastfour='{reqJson.GetPara("moblie_lastfour")}'";
                    }
                    where += $" and status='{ModelDb.p_join_push_apply.status_enum.申请中.ToInt()}'";
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
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
                public JsonResultAction approve(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        var userinfozbsn = UtilityStatic.CommonHelper.CreateOrderNo();

                        //修改申请状态
                        int id = req.GetPara("id").ToInt();
                        List<string> lSql = new List<string>();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_push_apply>(id);
                        apply.status = ModelDb.p_join_push_apply.status_enum.申请完成.ToInt();
                        apply.return_reason = "";
                        apply.user_info_zb_sn = userinfozbsn;
                        lSql.Add(apply.UpdateTran());
                        //新增user_info_zb记录 赋默认值，待开通账号列表中背调时设置基础数据
                        var user_info_zb = new ModelDb.user_info_zb();
                        user_info_zb.age = 0;
                        user_info_zb.devices_num = 0;
                        user_info_zb.quality = 0;
                        user_info_zb.status = 0;
                        user_info_zb.user_info_zb_sn = userinfozbsn;
                        user_info_zb.user_sn = "";
                        user_info_zb.mx_sn = "内推无萌新";
                        user_info_zb.qun_time = DateTime.Today;
                        user_info_zb.zb_level = "A";
                        user_info_zb.tg_user_sn = apply.tg_user_sn;
                        user_info_zb.note = "内推人员请先完成背调";
                        user_info_zb.dou_username = apply.dy_account;
                        //插入主播数据
                        if (!new ServiceFactory.UserInfo.User().ManagerPostUserInfoZb(user_info_zb))
                        {
                            throw new WeicodeException("创建主播失败");
                        }
                        DoMySql.ExecuteSqlTran(lSql);
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
