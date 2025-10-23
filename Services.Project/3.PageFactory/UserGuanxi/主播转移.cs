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

            #region 训练厅
            /// <summary>
            /// 主播账号转移训练厅
            /// </summary>
            public class Zhubo_MoveToTrainPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    //设置tab页
                    var pageModel = new PagePost("post");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
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

                    buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("movelog")
                    {
                        text = "转移记录",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"MoveLogList",
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
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelect("ting_sn_before")
                    {
                        title = "当前厅",
                        options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(req.tg_user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "ting_sn_before","<%=page_post.ting_sn_before.value%>"},
                                    {"tg_user_sn",req.tg_user_sn }
                                },
                                func = GetTrainTing,
                                resCallJs = $"{new EmtSelect.Js("post.ting_sn_after").clear()};{new ModelBasic.EmtSelect.Js("post.ting_sn_after").options(@"JSON.parse(res.data)")};"
                            }
                        },
                        defaultValue = req.ting_sn_before
                    });
                    formDisplay.formItems.Add(new EmtSelectFull("ting_sn_after")
                    {
                        title = "转移至厅",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForOption(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                        {
                            attachWhere = $"p_ting_sn = '{req.ting_sn_before}'"
                        }),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "ting_sn_before","<%=page_post.ting_sn_before.value%>"}
                                },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.zhubos").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "转移事由",
                    });
                    formDisplay.formItems.Add(new EmtExt.XmSelect("zhubos")
                    {
                        title = "选择主播",
                        options = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter()
                        {
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType()
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                UserSn = req.ting_sn_before,
                            }
                        })
                    });

                    #endregion
                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 原所属厅
                    /// </summary>
                    public string ting_sn_before { get; set; }
                    /// <summary>
                    /// 所属厅管
                    /// </summary>
                    public string tg_user_sn { get; set; }
                    /// <summary>
                    /// 所属运营
                    /// </summary>
                    public string yy_user_sn { get; set; }
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
                    var lSql = new List<string>();
                    //1.数据校验
                    if (req.GetPara("ting_sn_before").IsNullOrEmpty()) throw new WeicodeException("选择当前厅管不能为空!");
                    if (req.GetPara("ting_sn_after").IsNullOrEmpty()) throw new WeicodeException("选择目标厅管不能为空!");
                    if (req.GetPara("zhubos").IsNullOrEmpty()) throw new WeicodeException("请选择转移主播!");

                    //原所属厅
                    var ting_before = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.GetPara("ting_sn_before"));
                    //目标厅
                    var ting_after = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.GetPara("ting_sn_after"));

                    foreach (var zhubo_sn in req.GetPara("zhubos").Split(','))
                    {
                        //主播基本信息
                        var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(zhubo_sn);

                        lSql.Add(new ModelDb.user_info_zb
                        {
                            ting_sn = ting_after.ting_sn,
                            tg_user_sn = ting_after.tg_user_sn,
                            yy_user_sn = ting_after.yy_user_sn
                        }.UpdateTran($"user_sn = '{zhubo_sn}'"));

                        lSql.Add(new ModelDb.user_info_zhubo
                        {
                            ting_sn = ting_after.ting_sn,
                            tg_user_sn = ting_after.tg_user_sn,
                            yy_user_sn = ting_after.yy_user_sn
                        }.UpdateTran($"user_sn = '{zhubo_sn}'"));

                        lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(
                            ModelDb.user_info_zhubo_log.c_type_enum.转厅, 
                            $"训练厅转厅,主播:{zhubo.username},操作人:{new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(new UserIdentityBag().user_sn).username}(厅管),原所属主厅:{ting_before.ting_name},转至厅:{ting_after.ting_name};转厅事由:{req.GetPara("cause")}", 
                            zhubo));
                    }

                    MysqlHelper.ExecuteSqlTran(lSql);

                    return new JsonResultAction();
                }


                /// <summary>
                /// 获取训练厅
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTrainTing(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var option = new List<ModelDoBasic.Option>();
                    if (reqJson.GetPara("ting_sn_before").IsNullOrEmpty()) { throw new Exception("请选择当前厅!"); }

                    var ting_before = new ServiceFactory.UserInfo.Ting().GetTingBySn(reqJson.GetPara("ting_sn_before"));
                    

                    //p_ting_sn不为空则为主厅，目标厅选项为训练厅
                    if (ting_before.p_ting_sn.IsNullOrEmpty())
                    {
                        option = new ServiceFactory.UserInfo.Ting().GetBaseInfosForOption(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                        {
                            attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                            {
                                userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                                UserSn = reqJson.GetPara("tg_user_sn"),
                            },
                            attachWhere = $"p_ting_sn = '{reqJson.GetPara("ting_sn_before")}'"
                        });
                    }
                    //p_ting_sn为空则为训练厅，目标厅选项为所属主厅
                    else
                    {
                        option.Add(new ModelDoBasic.Option() 
                        {
                            text = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_before.p_ting_sn).ting_name,
                            value = ting_before.p_ting_sn,
                        });
                    }
                    
                    result.data = option.ToJson();
                    return result;
                }

                /// <summary>
                /// 获取主播
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    if (reqJson.GetPara("ting_sn_before").IsNullOrEmpty()) { throw new Exception("请选择当前厅!"); }
                    var option = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter()
                    {
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = reqJson.GetPara("ting_sn_before"),
                        }
                    });

                    result.data = option.ToJson();
                    return result;
                }

                #endregion
            }


            #endregion 直播厅

            #region 普通直播厅
            #region 申请转主播
            /// <summary>
            /// 主播转厅提交页
            /// </summary>
            public class Zhubo_MoveToTingPost
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "申请记录",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"MoveToTingList",
                        }
                    });
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
                    var yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).yy_sn;

                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelect("ting_sn")
                    {
                        title = "选择直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                        {
                            attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                            {
                                UserSn = new UserIdentityBag().user_sn,
                                userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅
                            },
                            attachWhere = "p_ting_sn = ''",
                        }),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new ModelBase.EventJsBase.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    {"ting_sn","<%=page_post.ting_sn.value%>"}
                                },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.user_info_zhubo_id").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelect("user_info_zhubo_id")
                    {
                        title = "转厅主播",
                        options = new Dictionary<string, string>(),
                    });
                    formDisplay.formItems.Add(new EmtSelect("t_ting_sn")
                    {
                        title = "转至厅(厅名)",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(yy_user_sn),
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 提交转厅申请记录数据
                /// </summary>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var info = new JsonResultAction();
                    try
                    {
                        //校验请求参数
                        var p_join_change_apply = req.GetPara().ToModel<ModelDb.p_join_change_apply>();
                        if (p_join_change_apply.t_ting_sn.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择转入的厅!");
                        }
                        if (p_join_change_apply.user_info_zhubo_id.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择需要转厅的主播!");
                        }
                        //校验是否有重复申请记录
                        if (DoMySql.FindListBySql<ModelDb.p_join_change_apply>($"select * from p_join_change_apply where user_info_zhubo_id = '{p_join_change_apply.user_info_zhubo_id}' and status = 2").Count > 0)
                        {
                            throw new WeicodeException("该主播已有转厅申请,请勿重复提交!");
                        }

                        var ting_after = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_change_apply.t_ting_sn);
                        var ting_before = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_change_apply.ting_sn);
                        var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{req.GetPara("user_info_zhubo_id")}'", false);

                        p_join_change_apply.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_join_change_apply.tg_user_sn = ting_before.tg_user_sn;
                        p_join_change_apply.user_info_zb_sn = user_info_zhubo.user_info_zb_sn;
                        p_join_change_apply.t_tg_user_sn = ting_after.tg_user_sn;
                        p_join_change_apply.c_date = DateTime.Now;
                        p_join_change_apply.status = ModelDb.p_join_change_apply.status_enum.等待运营审批.ToInt();

                        //插入申请数据
                        lSql.Add(p_join_change_apply.InsertOrUpdateTran());
                        DoMySql.ExecuteSqlTran(lSql);

                        new ServiceFactory.NoticeService().AddNoticeLog(
                            ServiceFactory.NoticeService.CategoryEnum.审批提醒,
                            ting_after.yy_user_sn,
                            $"主播转厅申请 发起人:{new UserIdentityBag().username} {DateTime.Now.ToString("MM月dd日HH:mm")}",
                            $"厅管:{new UserIdentityBag().username}发起了一条主播转厅申请，主播:{DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{p_join_change_apply.user_info_zb_sn}'", false).user_name},目标厅:{ting_after.ting_name}",
                            "/UserGuanxi/Move/MoveToTingList"
                        );
                    }
                    catch (Exception ex)
                    {
                        info.code = 1;
                        info.msg = ex.Message;
                    }
                    return info;
                }
                /// <summary>
                /// 获取所选厅名下主播
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction GetZhubo(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    if (req.GetPara("ting_sn").IsNullOrEmpty()) { throw new Exception("请选择直播厅"); }
                    var dictionary = new Dictionary<string, string>();
                    var list = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter()
                    {
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = req.GetPara("ting_sn"),
                        },
                        status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.待开账号
                    });

                    foreach (var item in list)
                    {
                        dictionary.Add(item.user_name, item.id.ToString());
                    }


                    info.data = dictionary.ToJson();

                    return info;
                }
                #endregion
            }

            /// <summary>
            /// 主播转厅列表
            /// </summary>
            public class Zhubo_MoveToTingList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("userlist");
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"等待运营审批",ModelDb.p_join_change_apply.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待对方同意",ModelDb.p_join_change_apply.status_enum.等待对方同意.ToInt().ToString()},
                            {"转厅成功",ModelDb.p_join_change_apply.status_enum.转厅成功.ToInt().ToString()},
                            {"转厅失败",ModelDb.p_join_change_apply.status_enum.转厅失败.ToInt().ToString()},
                            {"申请退回",ModelDb.p_join_change_apply.status_enum.申请退回.ToInt().ToString()}
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
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "申请厅",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_username")
                    {
                        text = "转厅主播",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("t_ting_name")
                    {
                        text = "接受厅",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date")
                    {
                        text = "申请日期",
                        width = "200",
                        minWidth = "200"
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
                    if (new ServiceFactory.UserInfo().GetUserType() == ModelEnum.UserTypeEnum.yyer)
                    {
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = YyApprove,
                                field_paras = "ids=id"
                            },
                            style = "",
                            text = "同意转厅",
                            hideWith = new EmtModel.ListOperateItem.HideWith
                            {
                                field = "status",
                                compareType = EmtModel.ListOperateItem.CompareType.不等于,
                                value = "2",
                            },

                        });
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = refuse,
                                field_paras = "ids=id"
                            },
                            style = "",
                            text = "拒绝",
                            hideWith = new EmtModel.ListOperateItem.HideWith
                            {
                                field = "status",
                                compareType = EmtModel.ListOperateItem.CompareType.不包含,
                                value = "2,4",
                            },
                        });
                    }
                    if (new ServiceFactory.UserInfo().GetUserType() == ModelEnum.UserTypeEnum.tger)
                    {
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = Cancel,
                                field_paras = "id"
                            },
                            style = "",
                            text = "取消",
                            hideWith = new EmtModel.ListOperateItem.HideWith
                            {
                                field = "status",
                                compareType = EmtModel.ListOperateItem.CompareType.不包含,
                                value = "2,4",
                            },
                        });
                    }


                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("agree")
                        {
                            text = "批量同意",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = YyApprove,
                             },
                        },
                        new ModelBasic.EmtModel.ButtonItem("disagree")
                        {
                            text = "批量拒绝",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = refuse,
                             },
                        },
                    }
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

                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status='{reqJson.GetPara("status")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_change_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_change_apply
                {
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    public string zb_username
                    {
                        get
                        {

                            return DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{user_info_zhubo_id}'", false).user_name;
                        }
                    }

                    public string t_ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(t_ting_sn).username;
                        }
                    }

                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                    public string is_show_button
                    {
                        get
                        {
                            return status == status_enum.等待运营审批.ToInt() && t_tg_user_sn.Equals(new UserIdentityBag().user_sn) ? "1" : "0";
                        }
                    }
                }

                #endregion ListData
                #region 异步请求处理
                /// <summary>
                /// 同意转厅操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction YyApprove(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        foreach (var id in req.GetPara("ids").Split(','))
                        {
                            //查询勾选的数据,将状态改成等待对方厅管接收
                            var p_join_change_apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id.ToInt());
                            p_join_change_apply.status = ModelDb.p_join_change_apply.status_enum.等待对方同意.ToInt();
                            p_join_change_apply.Update();

                            new ServiceFactory.NoticeService().AddNoticeLog(
                            ServiceFactory.NoticeService.CategoryEnum.审批提醒,
                            p_join_change_apply.t_tg_user_sn,
                            $"主播转厅申请 发起人:{new DomainBasic.UserApp().GetInfoByUserSn(p_join_change_apply.tg_user_sn).username} {DateTime.Now.ToString("MM月dd日HH:mm")}",
                            $"厅管:{new DomainBasic.UserApp().GetInfoByUserSn(p_join_change_apply.tg_user_sn).username}发起了一条主播转厅申请，主播:{DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{p_join_change_apply.user_info_zb_sn}'", false).user_name}",
                            "/UserGuanxi/Move/MoveToTingApprove"
                        );
                        }
                    }
                    catch (Exception ex)
                    {
                        result.code = 1;
                        result.msg = ex.Message;
                    }
                    return result;
                }

                /// <summary>
                /// 拒绝转厅操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction refuse(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        foreach (var id in req.GetPara("ids").Split(','))
                        {
                            //查询勾选的数据,将状态改成转厅失败
                            var p_join_change_apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id.ToInt());
                            p_join_change_apply.status = ModelDb.p_join_change_apply.status_enum.转厅失败.ToInt();
                            p_join_change_apply.return_reason = "拒绝转厅,请沟通后重新申请!";
                            p_join_change_apply.Update();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.code = 1;
                        result.msg = ex.Message;
                    }
                    return result;
                }

                /// <summary>
                /// 取消转厅
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction Cancel(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        //查询申请数据,将状态改成申请退回
                        var id = req.GetPara("id").ToInt();
                        var p_join_change_apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id);
                        p_join_change_apply.status = ModelDb.p_join_change_apply.status_enum.申请退回.ToInt();
                        p_join_change_apply.return_reason = "厅管已取消转厅申请";
                        p_join_change_apply.Update();
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

            /// <summary>
            /// 主播转厅审批接收
            /// </summary>
            public class Zhubo_MoveToTingApprove
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("userlist");
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
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "申请厅",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_username")
                    {
                        text = "转厅主播",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("position")
                    {
                        text = "职位",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("level")
                    {
                        text = "评级",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date")
                    {
                        text = "申请日期",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("return_reason")
                    {
                        text = "备注",
                        width = "300",
                        minWidth = "300"
                    });

                    #endregion 显示列
                    #region 操作列

                    if (new ServiceFactory.UserInfo().GetUserType() == ModelEnum.UserTypeEnum.tger)
                    {
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = TgApprove,
                                field_paras = "ids=id"
                            },
                            style = "",
                            text = "同意接收",
                            hideWith = new EmtModel.ListOperateItem.HideWith
                            {
                                field = "status",
                                compareType = EmtModel.ListOperateItem.CompareType.不等于,
                                value = "4",
                            },

                        });
                    }

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = refuse,
                            field_paras = "ids=id"
                        },
                        style = "",
                        text = "拒绝",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            value = "2,4",
                        },
                    });
                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            new ModelBasic.EmtModel.ButtonItem("")
                            {
                                text = "批量接收",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = TgApprove,
                                 },
                            },
                            new ModelBasic.EmtModel.ButtonItem("")
                            {
                                text = "批量拒绝",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = refuse,
                                 },
                            },
                        }
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
                    string where = $"t_tg_user_sn = '{new UserIdentityBag().user_sn}' and status = '{ModelDb.p_join_change_apply.status_enum.等待对方同意.ToSByte()}'";

                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status='{reqJson.GetPara("status")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_change_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_change_apply
                {
                    public ModelDb.user_info_zhubo user_info_zhubo
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{user_info_zhubo_id}'", false);
                        }
                    }
                    public string full_or_part
                    {
                        get
                        {
                            return user_info_zhubo.full_or_part;
                        }
                    }
                    public string zb_sex
                    {
                        get
                        {
                            return user_info_zhubo.zb_sex;
                        }
                    }
                    public string position
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("职务", user_info_zhubo.position);
                        }
                    }
                    public string level
                    {
                        get
                        {
                            return user_info_zhubo.level;
                        }
                    }
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    public string zb_username
                    {
                        get
                        {
                            return user_info_zhubo.user_name;
                        }
                    }

                    public string t_tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(t_tg_user_sn).username;
                        }
                    }

                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                    public string is_show_button
                    {
                        get
                        {
                            return status == status_enum.等待运营审批.ToInt() && t_tg_user_sn.Equals(new UserIdentityBag().user_sn) ? "1" : "0";
                        }
                    }
                }

                #endregion ListData
                #region 异步请求处理
                /// <summary>
                /// 同意接收操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction TgApprove(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    foreach (var id in req.GetPara("ids").Split(','))
                    {
                        var p_join_change_apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id.ToInt());
                        //修改申请状态
                        List<string> lSql = new List<string>();
                        p_join_change_apply.status = ModelDb.p_join_change_apply.status_enum.转厅成功.ToInt();
                        lSql.Add(p_join_change_apply.UpdateTran());

                        //修改主播所属厅管与所属厅信息
                        var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"id = '{p_join_change_apply.user_info_zhubo_id}'", false);
                        user_info_zhubo.tg_user_sn = p_join_change_apply.t_tg_user_sn;
                        user_info_zhubo.ting_sn = p_join_change_apply.t_ting_sn;
                        lSql.Add(user_info_zhubo.UpdateTran());

                        DoMySql.ExecuteSqlTran(lSql);
                    }
                    return result;
                }

                /// <summary>
                /// 拒绝转厅操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction refuse(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        foreach (var id in req.GetPara("ids").Split(','))
                        {
                            var apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id.ToInt());
                            //修改申请状态
                            List<string> lSql = new List<string>();
                            apply.status = ModelDb.p_join_change_apply.status_enum.转厅失败.ToInt();
                            apply.return_reason = "拒绝转厅,请沟通后重新申请!";
                            lSql.Add(apply.UpdateTran());

                            DoMySql.ExecuteSqlTran(lSql);
                        }

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

            #region 直接转主播
            /// <summary>
            /// 主播账号转移_直接提交
            /// </summary>
            public class Zhubo_MovePost
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
                            url = $"MoveList?type_id={req.type_id}",
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
                    List<ModelDoBasic.Option> options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForOption(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                    {
                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                        {
                            UserSn = new UserIdentityBag().user_sn,
                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营
                        },
                        attachWhere = "p_ting_sn = ''"
                    });

                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_before")
                    {
                        title = "当前厅",
                        options = options,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventComponent = new EmtDataSelect.Js("l_move").clear()
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_after")
                    {
                        title = "转移至",
                        options = options,
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "转移事由",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                    {
                        title = "转移账号",
                        selectUrl = $"ZhuboSelect?user_sn=<%=page.user_sn_before.value%>&type_id={req.type_id}&isolated=0",
                        buttonText = "选择需要流转的账号",
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
                    /// 类型id
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
                    if (req.GetPara("user_sn_before").IsNullOrEmpty()) throw new WeicodeException("选择当前厅管不能为空!");
                    if (req.GetPara("user_sn_after").IsNullOrEmpty()) throw new WeicodeException("选择目标厅管不能为空!");
                    var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                    if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");

                    //2.转移到目标用户
                    var _ = new DomainUserBasic.UserRelationApp().MoveNextUsersToUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, req.GetPara("user_sn_before"), moveItems, req.GetPara("user_sn_after"), req.GetPara("cause"));

                    //更新user_info_zb表的tg_user_sn与ting_sn字段
                    foreach (var item in moveItems)
                    {
                        var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"id = '{item.id}'");
                        var zhubo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_relation.t_user_sn);
                        new ModelDb.user_info_zb
                        {
                            ting_sn = req.GetPara("user_sn_after"),
                            tg_user_sn = req.GetPara("user_sn_after"),
                        }.Update($"user_sn = '{user_relation.t_user_sn}'");

                        new ModelDb.user_info_zhubo
                        {
                            ting_sn = req.GetPara("user_sn_after"),
                            tg_user_sn = req.GetPara("user_sn_after"),
                        }.Update($"user_sn = '{user_relation.t_user_sn}'");
                        var sql = new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(
                            ModelDb.user_info_zhubo_log.c_type_enum.转厅,
                            $"主播'{zhubo.username}'转移到厅'{new ServiceFactory.UserInfo.Ting().GetTingBySn(req.GetPara("user_sn_after")).ting_name}'",
                            zhubo
                            );
                        MysqlHelper.ExecuteSql(sql);
                    }
                    return new JsonResultAction();
                }
                #endregion
            }

            public class Zhubo_MoveList
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
                    string where = $"user_sn = '{new UserIdentityBag().user_sn}' and o_type = '0' and relation_type_id = '{ModelEnum.UserRelationTypeEnum.厅管邀主播}'";
                    if (!reqJson.GetPara("username").ToNullableString().IsNullOrEmpty())
                    {
                        where += $" AND (user_names like '%{reqJson.GetPara("username")}%')";
                    }

                    //2.获取当前登录user_sn指定关系类型的转移操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
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

            /// <summary>
            /// 选择用户表单
            /// </summary>
            public class Zhubo_Select
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
                        {"type_id", req.type_id},
                        {"user_sn", req.user_sn}
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
                    public string user_sn { get; set; }
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
                    var isolated = reqJson.GetPara("isolated");
                    string user_sn = reqJson.GetPara("user_sn");

                    if (isolated.IsNullOrEmpty()) throw new WeicodeException("是否为独立用户不能为空!");

                    if (!reqJson.GetPara("username").IsNullOrEmpty()) where += $"username like '%{reqJson.GetPara("username")}%'";

                    if (isolated == "1")//独立
                    {
                        return new ModularUserBasic.UserRelationApp().GetNextUsersUnrelat<ItemDataModel>(reqJson, ModelEnum.UserRelationTypeEnum.厅管邀主播,
                                    new DoMySql.Filter
                                    {
                                        where = where
                                    }
                                 );
                    }
                    return new ModularUserBasic.UserRelationApp().GetNextUsers<ItemDataModel>(reqJson, ModelEnum.UserRelationTypeEnum.厅管邀主播, user_sn,
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

            #endregion


            #region 账号管理操作转厅抖音后台
            public class ChangeList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("changelist");
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("is_dy_done")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"未完成",ModelDb.p_join_change_apply.is_dy_done_enum.未完成.ToInt().ToString()},
                            {"已完成",ModelDb.p_join_change_apply.is_dy_done_enum.已完成.ToInt().ToString()}
                        },
                        disabled = false,
                        placeholder = "抖音后台操作完成状态",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()),
                        disabled = false,
                        placeholder = "所属运营团队",
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
                        text = "申请厅(厅名)",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_username")
                    {
                        text = "转厅主播(微信账号)",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dy_username")
                    {
                        text = "转厅主播(抖音号)",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("t_tg_username")
                    {
                        text = "接受厅(厅名)",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date")
                    {
                        text = "申请日期",
                        width = "200",
                        minWidth = "200"
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("is_dy_done_text")
                    {
                        text = "抖音后台操作是否完成",
                        width = "300",
                        minWidth = "300"
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
                        text = "抖音操作完成",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "is_dy_done",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = "0",
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

                    if (!reqJson.GetPara("is_dy_done").IsNullOrEmpty())
                    {
                        where += $" and is_dy_done='{reqJson.GetPara("is_dy_done")}'";
                    }

                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in ({new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(reqJson.GetPara("yy_user_sn"))})";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_change_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_change_apply
                {
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    public string zb_username
                    {
                        get
                        {
                            return new ServiceFactory.JoinChangeService().GetUsernameByUniqueKey(user_info_zb_sn)[0].username.ToString();
                        }
                    }
                    public string dy_username
                    {
                        get
                        {
                            return new ServiceFactory.JoinChangeService().GetUsernameByUniqueKey(user_info_zb_sn)[0].dou_username.ToString();
                        }
                    }

                    public string t_tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(t_tg_user_sn).username;
                        }
                    }

                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                    public string is_dy_done_text
                    {
                        get
                        {
                            return ((is_dy_done_enum)is_dy_done).ToString();
                        }
                    }
                }

                #endregion ListData
                #region 异步请求处理
                /// <summary>
                /// 同意转厅操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction approve(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        var id = req.GetPara("id").ToInt();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id);
                        //修改申请状态
                        List<string> lSql = new List<string>();
                        apply.is_dy_done = ModelDb.p_join_change_apply.is_dy_done_enum.已完成.ToInt();
                        lSql.Add(apply.UpdateTran());
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

            /// <summary>
            /// 主播转移日志列表
            /// </summary>
            public class Zhubo_MoveLogList
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
                    listFilter.isExport = false;
                    //listFilter.formItems.Add(new EmtInput("keyword")
                    //{
                    //    placeholder = "主播名称/微信号/抖音号",
                    //    defaultValue = ""
                    //});

                    listFilter.formItems.Add(new EmtTimeSelect("date")
                    {
                        placeholder = "日期",
                        mold = EmtTimeSelect.Mold.date,
                        defaultValue = DateTime.Today.ToDate().ToString("yyyy-MM-dd")
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("user_name")
                    {
                        text = "主播",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_sn_before")
                    {
                        text = "原所属厅",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_sn_after")
                    {
                        text = "转移厅",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("move_date")
                    {
                        text = "转厅日期",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("content")
                    {
                        text = "说明",
                        width = "600",
                        minWidth = "600",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("op_username")
                    {
                        text = "操作人",
                        width = "120",
                        minWidth = "120",
                    });
                    #region 操作列按钮
                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    name = "ZhuanZheng",
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = "/PCrm/Report/Post",
                    //        field_paras = "c_date,session"
                    //    },
                    //    text = "转到正式厅"
                    //});
                    #endregion
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
                    //筛选训练厅
                    string where = $"c_type = '{ModelDb.user_info_zhubo_log.c_type_enum.转厅.ToSByte()}'";

                    if (!reqJson.GetPara("date").IsNullOrEmpty())
                    {
                        where += $" AND create_time >= '{reqJson.GetPara("date")}' and create_time < '{reqJson.GetPara("date").ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zhubo_log
                {
                    public ModelDb.user_info_zhubo user_info_zhubo
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_info_zb_sn = '{user_info_zb_sn}'");
                        }
                    }
                    public string user_name
                    {
                        get
                        {
                            return user_info_zhubo.user_name;
                        }
                    }
                    /// <summary>
                    /// 操作人
                    /// </summary>
                    public string op_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(user_sn).username;
                        }
                    }
                    public string move_date
                    {
                        get
                        {
                            return create_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }

                }
                #endregion
            }
        }
    }
}