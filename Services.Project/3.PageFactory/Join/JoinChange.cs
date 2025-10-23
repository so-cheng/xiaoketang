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

namespace Services.Project {
    public partial class PageFactory
    {
        public partial class JoinChange
        {
            #region 厅管申请转厅主播列表
            public class UserList
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
                            {"转厅申请中",ModelDb.p_join_change_apply.status_enum.等待运营审批.ToInt().ToString()},
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

                    #endregion 显示列
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = YyApprove,
                            field_paras = "id"
                        },
                        style = "",
                        text = "同意转厅",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "is_show_button",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = "4",
                        },

                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = TgApprove,
                            field_paras = "id"
                        },
                        style = "",
                        text = "同意接收",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "is_show_button",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = "1",
                        },

                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = refuse,
                            field_paras = "id"
                        },
                        style = "",
                        text = "拒绝",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "is_show_button",
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            value = "1,4",
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
                        text = "转厅申请",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"changeApply",
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
                    
                    where += $" and (tg_user_sn = '{new UserIdentityBag().user_sn}' or t_tg_user_sn = '{new UserIdentityBag().user_sn}')";
                   
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
                            return new ServiceFactory.JoinChangeService().GetUsernameByUniqueKey(user_info_zb_sn)[0].username.ToString();
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
                            return status == status_enum.等待运营审批.ToInt() && t_tg_user_sn.Equals(new UserIdentityBag().user_sn)?"1":"0";
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
                        var id = req.GetPara("id").ToInt();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id);
                        //修改申请状态
                        List<string> lSql = new List<string>();
                        apply.status = ModelDb.p_join_change_apply.status_enum.转厅成功.ToInt();
                        apply.return_reason = "";
                        lSql.Add(apply.UpdateTran());

                        //1.user_info_zb修改上级厅管
                        var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_info_zb_sn='{apply.user_info_zb_sn}'");
                        user_info_zb.old_tg_user_sn = apply.tg_user_sn;
                        user_info_zb.old_tg_username = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(apply.tg_user_sn).username;
                        user_info_zb.tg_user_sn = apply.t_tg_user_sn;
                        lSql.Add(user_info_zb.UpdateTran());
                        //修改主播上级厅管信息
                        if (!apply.zb_user_sn.IsNullOrEmpty())
                        {
                            //已开通账号主播
                            //user_relation修改主播从属关系
                            var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"f_user_sn = '{apply.tg_user_sn}' and t_user_sn = '{apply.zb_user_sn}'");
                            user_relation.f_user_sn = apply.t_tg_user_sn;
                            lSql.Add(user_relation.UpdateTran());
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
                /// <summary>
                /// 同意接收操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction TgApprove(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    try
                    {
                        var id = req.GetPara("id").ToInt();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id);
                        //修改申请状态
                        List<string> lSql = new List<string>();
                        apply.status = ModelDb.p_join_change_apply.status_enum.转厅成功.ToInt();
                        apply.return_reason = "";
                        lSql.Add(apply.UpdateTran());

                        //1.user_info_zb修改上级厅管
                        var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_info_zb_sn='{apply.user_info_zb_sn}'");
                        user_info_zb.old_tg_user_sn = apply.tg_user_sn;
                        user_info_zb.old_tg_username = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(apply.tg_user_sn).username;
                        user_info_zb.tg_user_sn = apply.t_tg_user_sn;
                        lSql.Add(user_info_zb.UpdateTran());
                        //修改主播上级厅管信息
                        if (!apply.zb_user_sn.IsNullOrEmpty())
                        {
                            //已开通账号主播
                            //user_relation修改主播从属关系
                            var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"f_user_sn = '{apply.tg_user_sn}' and t_user_sn = '{apply.zb_user_sn}'");
                            user_relation.f_user_sn = apply.t_tg_user_sn;
                            lSql.Add(user_relation.UpdateTran());
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
                        var id = req.GetPara("id").ToInt();
                        var apply = DoMySql.FindEntityById<ModelDb.p_join_change_apply>(id);
                        //修改申请状态
                        List<string> lSql = new List<string>();
                        apply.status = ModelDb.p_join_change_apply.status_enum.转厅失败.ToInt();
                        apply.return_reason = "拒绝转厅,请沟通后重新申请!";
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

            #region 厅管申请转厅操作
            public class ChangePost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("changepost");
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
                    var tg_user_sn = new UserIdentityBag().user_sn;
                    //获取当前厅管下的所有主播和当前厅管下未开通账号的所有主播
                    var optionZb = new Dictionary<string, string>();
                    string zbListSql = $"select t1.user_info_zb_sn,CONCAT('(待开账号)' ,t1.wechat_username) as username,'1' as flag from user_info_zb t1 where t1.tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and t1.tg_user_sn = '{new UserIdentityBag().user_sn}'  and t1.status = 0 and t1.user_sn != '' and t1.wechat_username is not null and t1.wechat_username != ''";
                    string dkzhListSql = $"SELECT t1.user_info_zb_sn,CONCAT('(已开账号)' ,t1.wechat_username) AS username,'0' as flag FROM user_info_zb t1 WHERE t1.tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' AND t1.mx_sn IS NOT NULL AND t1.mx_sn != '' AND t1.user_sn = '' AND t1.qun_time IS NOT NULL AND t1.tg_user_sn = '{new UserIdentityBag().user_sn}' AND t1.zb_level != '-' AND t1.zb_level != 'C' AND t1.zb_level != 'D' and t1.wechat_username is not null and t1.wechat_username != ''";
                    var zbList = DoMySql.FindListBySql<ServiceFactory.JoinChangeTable>($"{zbListSql} union all {dkzhListSql}");
                    foreach (var item in zbList)
                    {
                        optionZb.Add(item.username.ToString(), item.user_info_zb_sn);
                    }
                    //获取当前运营下的所有厅
                    var yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).yy_sn;
                    var tgList = new ServiceFactory.UserInfo.Yy().YyGetNextTg(yy_user_sn);
                    var optionTg = new Dictionary<string, string>();
                    foreach (var item in tgList)
                    {
                        optionTg.Add(item.username, item.user_sn);
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtSelect("user_info_zb_sn")
                    {
                        title = "转厅主播(微信账号)",
                        options = optionZb,
                    });
                    formDisplay.formItems.Add(new EmtSelect("t_tg_user_sn")
                    {
                        title = "转至厅(厅名)",
                        options = optionTg,
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
                        var tg_user_sn = new UserIdentityBag().user_sn;
                        //校验请求参数
                        var changeParam = req.GetPara().ToModel<ModelDb.p_join_change_apply>();
                        if (changeParam.t_tg_user_sn.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择转入的厅!");
                        }
                        if (changeParam.user_info_zb_sn.IsNullOrEmpty())
                        {
                            throw new WeicodeException("请选择需要转厅的主播!");
                        }
                        //校验是否有重复申请记录
                        var check = DoMySql.FindListBySql<ModelDb.p_join_change_apply>($"select * from p_join_change_apply where user_info_zb_sn = '{changeParam.user_info_zb_sn}' and status = 2");
                        if (!check.IsNullOrEmpty() && check.Count()!=0)
                        {
                            throw new WeicodeException("该主播已有转厅申请,请勿重复提交!");
                        }

                        var applyinfo = new ModelDb.p_join_change_apply();
                        applyinfo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        applyinfo.tg_user_sn = tg_user_sn;
                        applyinfo.user_info_zb_sn = changeParam.user_info_zb_sn;
                        applyinfo.zb_user_sn = new ServiceFactory.JoinChangeService().GetUsernameByUniqueKey(changeParam.user_info_zb_sn)[0].user_sn;
                        applyinfo.t_tg_user_sn = changeParam.t_tg_user_sn;
                        applyinfo.c_date = DateTime.Now;
                        applyinfo.status = ModelDb.p_join_change_apply.status_enum.等待运营审批.ToInt();
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

        }
    }
}
