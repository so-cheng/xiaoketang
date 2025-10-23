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
            #region 厅转移
            /// <summary>
            /// 主播账号转移训练厅
            /// </summary>
            public class Ting_MoveToTrainPost
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
                        title = "当前厅管",
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
                        title = "转移至厅管",
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


            #endregion
        }
    }
}