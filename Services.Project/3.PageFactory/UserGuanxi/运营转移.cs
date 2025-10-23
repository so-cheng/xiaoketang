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

            
            #region 中台名下运营
            /// <summary>
            /// 主播账号转移训练厅
            /// </summary>
            public class Yy_RelationPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    //设置tab页
                    var pageModel = new PagePost("post");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新当前窗口,
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
                        text = "关系列表",
                        mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"YyMoveList",
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
                    formDisplay.formItems.Add(new EmtSelect("zt_user_sn")
                    {
                        title = "选择中台",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = '{ModelEnum.UserTypeEnum.zter.ToSByte()}' and tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'","username,user_sn"),
                        defaultValue = req.ting_sn_before
                    });
                    formDisplay.formItems.Add(new EmtExt.XmSelect("yy_user_sns")
                    {
                        title = "选择运营",
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter() 
                        { 
                            attachWhere = $"user_sn not in(select t_user_sn from user_relation where t_user_type_id = '{ModelEnum.UserTypeEnum.yyer.ToSByte()}')",
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
                    if (req.GetPara("zt_user_sn").IsNullOrEmpty()) throw new WeicodeException("请选择中台");
                    if (req.GetPara("yy_user_sns").IsNullOrEmpty()) throw new WeicodeException("请选择运营!");
                    foreach (var yy_user_sn in req.GetPara("yy_user_sns").Split(','))
                    {
                        lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.基地邀运营, req.GetPara("zt_user_sn"), yy_user_sn,"管理员添加中台运营关系"));
                        lSql.Add(new ModelDb.user_info_zhubo() 
                        {
                            zt_user_sn = req.GetPara("zt_user_sn")
                        }.UpdateTran($"yy_user_sn = '{yy_user_sn}'"));
                        lSql.Add(new ModelDb.user_info_tg()
                        {
                            zt_user_sn = req.GetPara("zt_user_sn")
                        }.UpdateTran($"yy_user_sn = '{yy_user_sn}'"));
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

            public class Yy_RelationList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                    {
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = '{ModelEnum.UserTypeEnum.zter.ToSByte()}' and tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        disabled = false,
                        placeholder = "所属中台",
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("zt_username")
                    {
                        text = "所属中台",
                        width = "200",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "运营名称",
                        width = "200",
                        minWidth = "100",
                    });
                    #endregion 显示列
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = UnBind,
                            field_paras = "ids=id"
                        },
                        style = "",
                        text = "解绑关系",
                    });


                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("agree")
                    {
                        text = "批量解绑",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                        {
                            func = UnBind,
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
                    string where = $"user_base.user_type_id = '{ModelEnum.UserTypeEnum.yyer.ToSByte()}' and user_base.status = '{ModelDb.user_base.status_enum.正常.ToSByte()}' and user_relation.f_user_sn is not null";
                    if (!reqJson.GetPara("zt_user_sn").IsNullOrEmpty())
                    {
                        where += $" and user_relation.f_user_sn = '{reqJson.GetPara("zt_user_sn")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        on = "user_base.user_sn = user_relation.t_user_sn",
                        orderby = "user_relation.f_user_sn"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_base,ModelDb.user_relation, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string f_user_sn { get; set; }
                    public string zt_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(f_user_sn).username;
                        }
                    }
                }

                #endregion ListData

                #region 异步请求处理
                /// <summary>
                /// 解绑操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction UnBind(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        var lSql = new List<string>();

                        foreach (var id in req.GetPara("ids").Split(','))
                        {
                            var user_base = DoMySql.FindEntityById<ModelDb.user_base>(id.ToInt());
                            var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"t_user_sn = '{user_base.user_sn}'");
                            lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.基地邀运营,user_relation.f_user_sn, user_relation.t_user_sn, "管理员解绑基地运营"));
                            lSql.Add($"Update user_info_zhubo set zt_user_sn = '' where yy_user_sn = '{user_relation.t_user_sn}'");
                            lSql.Add($"Update user_info_tg set zt_user_sn = '' where yy_user_sn = '{user_relation.t_user_sn}'");
                        }
                        MysqlHelper.ExecuteSqlTran(lSql);
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
            #endregion

        }
    }
}