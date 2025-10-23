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
    /// 基础数据
    /// </summary>
    public partial class PageFactory
    {
        public partial class JixiaoDay
        {
            #region 请假列表
            /// <summary>
            /// 请假列表
            /// </summary>
            public class QingjiaList
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
                    pageModel.buttonGroup = GetButtonGroup(req);
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
                        disabled = true,

                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()),
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "厅管账号",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                                func = GetTings,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });
                   
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "ting_sn","<%=page.ting_sn.value%>"}
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        placeholder = "请假时间",
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
                /// 获取主播筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅管,
                            UserSn = req["tg_user_sn"].ToNullableString()
                        }
                    });
                    result.data = option.ToJson();
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
                    
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "160";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_test")
                    {
                        text = "所属运营",
                        width = "100",
                        minWidth = "100",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_test")
                    {
                        text = "所属厅管",
                        width = "100",
                        minWidth = "100",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn_test")
                    {
                        text = "直播厅",
                        width = "100",
                        minWidth = "100"
                    });
                    
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_user_sn_test")
                    {
                        text = "请假主播",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("s_date")
                    {
                        text = "开始日期",
                        width = "170",
                        minWidth = "170"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("e_date")
                    {
                        text = "结束日期",
                        width = "170",
                        minWidth = "170"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("note")
                    {
                        text = "事由说明",
                        width = "250",
                        minWidth = "250"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_zb_user_sn_test")
                    {
                        text = "替档主播",
                        width = "100",
                        minWidth = "100",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_test")
                    {
                        text = "审核状态",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "170",
                        minWidth = "170"
                    });
                    #region 操作列按钮


                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = PassAction,
                            field_paras = "id"
                        },
                        style = "",
                        text = "同意",
                        disabled = true,
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte().ToString()
                        },
                        name= "Pass"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = RejectAction,
                            field_paras = "id"
                        },
                        style = "",
                        text = "拒绝",
                        disabled = true,
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte().ToString()
                        },
                        name="Reject"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = CancelAction,
                            field_paras = "id"
                        },
                        style = "",
                        text = "取消",
                        disabled = true,
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte().ToString()
                        },
                        name = "Cancel"
                    }) ;
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
                    string where = $"1=1";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                    }

                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                    }

                    if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                    }

                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and create_time >= '{dateRange.date_range_s}' and create_time<'{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by create_time desc"
                    };
                   
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_qingjia, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_jixiao_qingjia
                {
                    public string yy_user_sn_test
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string tg_user_sn_test
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string ting_sn_test
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                                
                    public string zb_user_sn_test
                    {
                        get
                        {
                            var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);
                            return zhubo.name;
                        }
                    }
                    public string new_zb_user_sn_test
                    {
                        get
                        {
                            var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);
                            return new_zb_user_sn.IsNullOrEmpty() ? "": zhubo.name;
                        }
                    }

                    public string status_test
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 审核通过请假申请
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PassAction(JsonRequestAction req)
                {
                    var p_jixiao_qingjia = req.data_json.ToModel<ModelDb.p_jixiao_qingjia>();
                    p_jixiao_qingjia = DoMySql.FindEntity<ModelDb.p_jixiao_qingjia>($"id = {p_jixiao_qingjia.id}", false);
                    var info = new JsonResultAction();
                    var lSql = new List<string>();

                    // 检查状态是否为等待审批
                    if (p_jixiao_qingjia.status > ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte())
                    {
                        return info;
                    }

                    // 更新状态为审批同意
                    p_jixiao_qingjia.status = (sbyte)ModelDb.p_jixiao_qingjia.status_enum.审批同意;
                    p_jixiao_qingjia.modify_time = DateTime.Now;
                    lSql.Add(p_jixiao_qingjia.UpdateTran());

                    DoMySql.ExecuteSqlTran(lSql);
                   
                   
                    return info;
                }

                /// <summary>
                /// 拒绝审核通过请假申请
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RejectAction(JsonRequestAction req)
                {
                    var p_jixiao_qingjia = req.data_json.ToModel<ModelDb.p_jixiao_qingjia>();
                    p_jixiao_qingjia = DoMySql.FindEntity<ModelDb.p_jixiao_qingjia>($"id = {p_jixiao_qingjia.id}", false);
                    var info = new JsonResultAction();
                    var lSql = new List<string>();
                    if (p_jixiao_qingjia.status > ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte())
                    {
                        return info;
                    }
                    // 审批拒绝                 
                    p_jixiao_qingjia.status = (sbyte)ModelDb.p_jixiao_qingjia.status_enum.审批拒绝;                   
                    p_jixiao_qingjia.modify_time = DateTime.Now;
                    lSql.Add(p_jixiao_qingjia.UpdateTran());
                    
                    DoMySql.ExecuteSqlTran(lSql);
                    
                    
                    return info;
                }

                /// <summary>
                /// 取消审批通过请假申请
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction CancelAction(JsonRequestAction req)
                {
                    var p_jixiao_qingjia = req.data_json.ToModel<ModelDb.p_jixiao_qingjia>();
                    p_jixiao_qingjia = DoMySql.FindEntity<ModelDb.p_jixiao_qingjia>($"id = {p_jixiao_qingjia.id}", false);
                    var info = new JsonResultAction();
                    var lSql = new List<string>();
                    if (p_jixiao_qingjia.status > ModelDb.p_jixiao_qingjia.status_enum.等待审批.ToSByte())
                    {
                        return info;
                    }
                    //取消审批
                    p_jixiao_qingjia.status = (sbyte)ModelDb.p_jixiao_qingjia.status_enum.已取消;
                  
                    p_jixiao_qingjia.modify_time = DateTime.Now;
                    lSql.Add(p_jixiao_qingjia.UpdateTran());                  
                    DoMySql.ExecuteSqlTran(lSql);
                    // 日志
                  
                    return info;
                }
                #endregion
            }
            #endregion

            #region 创建请假单
            /// <summary>
            /// 创建页面
            /// </summary>
            public class QingJiaPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    return pageModel;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
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
                    var p_jixiao_qingjia = DoMySql.FindEntityById<ModelDb.p_jixiao_qingjia>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_jixiao_qingjia.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("s_date")
                    {
                        title = "请假开始时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_jixiao_qingjia.s_date.IsNullOrEmpty() ? "" : p_jixiao_qingjia.s_date.ToDateTime().ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("e_date")
                    {
                        title = "请假结束时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_jixiao_qingjia.e_date.IsNullOrEmpty() ? "" : p_jixiao_qingjia.e_date.ToDateTime().ToString()
                    });
                    // 初始化替档主播下拉列表选项：获取当前厅下的所有主播
                    var zb_user = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);
                    // 添加替档主播下拉列表
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("new_zb_user_sn")
                    {
                        title = "替档主播",
                        options = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter {
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅管,
                                UserSn = zb_user.ting_sn
                            }
                        }),
                        defaultValue = p_jixiao_qingjia.new_zb_user_sn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("note")
                    {
                        title = "请假事由"
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
                    var p_jixiao_qingjia = req.data_json.ToModel<ModelDb.p_jixiao_qingjia>();
                    p_jixiao_qingjia.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    if (p_jixiao_qingjia.s_date.IsNullOrEmpty()) throw new Exception("请选择请假开始时间");
                    if (p_jixiao_qingjia.e_date.IsNullOrEmpty()) throw new Exception("请选择请假结束时间");
                    if (p_jixiao_qingjia.note.IsNullOrEmpty()) throw new Exception("请输入请假事由");

                    var zb_user = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);

                    p_jixiao_qingjia.zb_user_sn = new UserIdentityBag().user_sn;
                    p_jixiao_qingjia.ting_sn = zb_user.ting_sn;
                    p_jixiao_qingjia.tg_user_sn = zb_user.tg_user_sn;
                    p_jixiao_qingjia.yy_user_sn = zb_user.yy_user_sn;
                    p_jixiao_qingjia.zt_user_sn = zb_user.zt_user_sn;

                    p_jixiao_qingjia.InsertOrUpdate();

                    // 日志

                    new DomainBasic.SystemBizLogApp().Write("基础数据", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"请假：{zb_user.name}");

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}
