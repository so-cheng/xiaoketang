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
    /// 目标提报
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 目标提报
            /// <summary>
            /// 提交目标创建/编辑页面
            /// </summary>
            public class TargetPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("");
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
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();

                    string msg = "";
                    if (p_tingzhan.IsNullOrEmpty()) msg = "厅战不存在";
                    if (p_tingzhan.start_time > DateTime.Now || p_tingzhan.end_time < DateTime.Now) msg = "不在填报时间内";

                    if (!msg.IsNullOrEmpty())
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtHtml("msg")
                        {
                            Content = $"<span style = 'color:red;'>{msg}</span>"
                        });
                        #region 表单元素（控制有判断）
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("c_date")
                        {
                            title = "厅战时间",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("start_time")
                        {
                            title = "提报开始",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("end_time")
                        {
                            title = "提报截止",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("yy_user_sn")
                        {
                            title = "运营账号",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("tg_user_sn")
                        {
                            title = "厅管账号",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("ting_sn")
                        {
                            title = "直播厅",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("is_join")
                        {
                            title = "是否参与",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("amont")
                        {
                            title = "目标音浪",
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("reason")
                        {
                            title = "不参加原因"
                        });
                        #endregion

                        return formDisplay;
                    }

                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("start_time")
                    {
                        title = "提报开始",
                        defaultValue = p_tingzhan.start_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("end_time")
                    {
                        title = "提报截止",
                        defaultValue = p_tingzhan.end_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                    });

                    var yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy_user = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(new UserIdentityBag().user_sn);
                            yy_options = new Dictionary<string, string> {
                                {
                                    yy_user.name,
                                    yy_user.user_sn
                                }
                            };
                            break;
                    }

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "运营账号",
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
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        title = "厅管账号",
                        options = new Dictionary<string, string> { },
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                                },
                                func = GetTings,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        title = "直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn)
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("is_join")
                    {
                        title = "是否参与",
                        options = new Dictionary<string, string>
                    {
                        {"是","1" },
                        {"否","0" },
                    },
                        defaultValue = "1",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "",
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amont")
                    {
                        title = "目标音浪",
                        placeholder = "单位(万)，不参与不需要填写"
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("reason")
                    {
                        title = "不参加原因"
                    });
                    #endregion
                    return formDisplay;
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
                /// 获取厅筛选项
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

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_tingzhan_target = req.data_json.ToModel<ModelDb.p_tingzhan_target>();
                    p_tingzhan_target.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    // 取最近的厅战
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                    if (p_tingzhan.IsNullOrEmpty()) throw new Exception("厅战不存在");
                    if (p_tingzhan.start_time > DateTime.Now || p_tingzhan.end_time < DateTime.Now) throw new Exception("不在填报时间内");
                    p_tingzhan_target.tingzhan_id = p_tingzhan.id;

                    var ting_sn = req.GetPara("ting_sn");
                    if (ting_sn.IsNullOrEmpty()) throw new Exception("请选择直播厅");

                    // 选择不参与 目标音浪存0
                    if (req.GetPara("is_join").Equals("0"))
                    {
                        p_tingzhan_target.amont = 0;
                    }
                    else
                    {
                        if (req.GetPara("amont").IsNullOrEmpty()) throw new Exception("请输入目标音浪");
                        if (req.GetPara("amont").ToDecimal() > 100) throw new Exception("目标音浪不能超过100");
                    }

                    // 获取厅信息
                    var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn);

                    // 取厅目标
                    var p_tingzhan_target_ = new ServiceFactory.TingZhanService().getCurrentTarget(p_tingzhan.id, ting_sn);
                    p_tingzhan_target.id = p_tingzhan_target_.id;

                    p_tingzhan_target.yy_user_sn = ting.yy_user_sn;
                    p_tingzhan_target.tg_user_sn = ting.tg_user_sn;

                    p_tingzhan_target.InsertOrUpdate();

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion

            #region 目标查看编辑页面
            /// <summary>
            /// 目标查看编辑页面
            /// </summary>
            public class Edit
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
                    var p_tingzhan_target = DoMySql.FindEntityById<ModelDb.p_tingzhan_target>(req.id);
                    var yy_user = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(p_tingzhan_target.yy_user_sn);
                    var tg_user = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_tingzhan_target.tg_user_sn);
                    var ting_user = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_tingzhan_target.ting_sn);
                    var p_tingzhan = DoMySql.FindEntityById<ModelDb.p_tingzhan>(p_tingzhan_target.tingzhan_id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_tingzhan_target.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("start_time")
                    {
                        title = "提报开始时间",
                        defaultValue = p_tingzhan.start_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("end_time")
                    {
                        title = "提报结束时间",
                        defaultValue = p_tingzhan.end_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("yy_name")
                    {
                        title = "运营账号",
                        defaultValue = yy_user.name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("tg_name")
                    {
                        title = "厅管账号",
                        defaultValue = tg_user.name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("ting_name")
                    {
                        title = "直播厅",
                        defaultValue = ting_user.ting_name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amont")
                    {
                        title = "目标音浪",
                        placeholder = "万",
                        defaultValue = p_tingzhan_target.amont.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("is_join")
                    {
                        title = "是否参与",
                        options = new Dictionary<string, string>
                    {
                        {"是","1" },
                        {"否","0" },
                    },
                        defaultValue = !p_tingzhan_target.amont.IsNullOrEmpty() ? p_tingzhan_target.amont.ToInt().Equals(0) ? "0" : "1" : "1",
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("reason")
                    {
                        title = "不参加原因",
                        defaultValue = p_tingzhan_target.reason
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
                    var p_tingzhan_target = req.data_json.ToModel<ModelDb.p_tingzhan_target>();
                    // 取最近的厅战
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                    if (p_tingzhan.IsNullOrEmpty()) throw new Exception("厅战不存在");
                    if (p_tingzhan.start_time > DateTime.Now || p_tingzhan.end_time < DateTime.Now) throw new Exception("不在填报时间内");
                    // 选择不参与 目标音浪存0
                    if (req.GetPara("is_join").Equals("0"))
                    {
                        p_tingzhan_target.amont = 0;
                    }
                    else
                    {
                        if (req.GetPara("amont").IsNullOrEmpty()) throw new Exception("请输入目标音浪");
                        if (req.GetPara("amont").ToDecimal() > 100) throw new Exception("目标音浪不能超过100");
                    }

                    p_tingzhan_target.Update();

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}
