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
    /// 跨房-活动
    /// </summary>
    public partial class PageFactory
    {
        public partial class KuaFangMate
        {
            #region 目标提报
            /// <summary>
            /// （厅管）提交目标创建/编辑页面
            /// </summary>
            public class MatePost
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
                    var p_kuafang = new ServiceFactory.KuaFang.Common().getNewKuaFang();

                    #region 表单元素

                    if (p_kuafang.IsNullOrEmpty())
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("msg")
                        {
                            defaultValue = "不在活动时间内"
                        });
                    }
                    else
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                        {
                            title = "跨房时间",
                            defaultValue = p_kuafang.c_date.IsNullOrEmpty() ? "" : p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd")
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("start_time")
                        {
                            title = "提报开始",
                            defaultValue = p_kuafang.start_time.IsNullOrEmpty() ? "" : p_kuafang.start_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("end_time")
                        {
                            title = "提报截止",
                            defaultValue = p_kuafang.end_time.IsNullOrEmpty() ? "" : p_kuafang.end_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn1")
                        {
                            title = "直播厅",
                            options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn)
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtRadio("is_open")
                        {
                            title = "是否跨团队",
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
                            placeholder = "单位(万)"
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtInput("content")
                        {
                            title = "备注",
                            placeholder = ""
                        });
                    }

                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    // 取当前的跨房
                    var p_kuafang = new ServiceFactory.KuaFang.Common().getNewKuaFang();
                    if (p_kuafang.IsNullOrEmpty()) throw new Exception("不在填报时间内");
                    if (p_kuafang.start_time > DateTime.Now || p_kuafang.end_time < DateTime.Now) throw new Exception("不在填报时间内");

                    var result = new JsonResultAction();
                    var reqData = req.data_json.ToModel<DtoReqData>();

                    if (reqData.ting_sn1.IsNullOrEmpty()) throw new Exception("请选择直播厅");

                    // 判断当前跨房已经匹配成功，不可再提报
                    var mate_exists = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang.id} and ting_sn2 is not null and (ting_sn1 = '{reqData.ting_sn1}' or ting_sn2 = '{reqData.ting_sn1}')", false);
                    if (!mate_exists.IsNullOrEmpty()) throw new Exception("当前跨房活动已参加");

                    // 判断存在未确认的跨房报名，不可填报
                    var mate_apply_exists = DoMySql.FindEntity<ModelDb.p_kuafang_mate_apply>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang.id} and ting_sn = '{reqData.ting_sn1}' and status != {ModelDb.p_kuafang_mate_apply.status_enum.无效.ToSByte()}", false);
                    if (!mate_apply_exists.IsNullOrEmpty()) throw new Exception("当前跨房已有报名");

                    if (reqData.amont.IsNullOrEmpty()) throw new Exception("请输入目标音浪");

                    // 新增/编辑
                    var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang.id} and ting_sn1 = '{reqData.ting_sn1}'", false);
                    if (p_kuafang_mate.IsNullOrEmpty())
                    {
                        p_kuafang_mate.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_kuafang_mate.kuafang_id = p_kuafang.id;
                        p_kuafang_mate.tg_user_sn1 = new UserIdentityBag().user_sn;
                        p_kuafang_mate.ting_sn1 = reqData.ting_sn1;
                        p_kuafang_mate.is_open = reqData.is_open;
                        p_kuafang_mate.amont = reqData.amont;
                        p_kuafang_mate.content = reqData.content;

                        p_kuafang_mate.Insert();
                    }
                    else
                    {
                        // 判断已有厅报名当前跨房，不可修改
                        var exists_apply = DoMySql.FindEntity<ModelDb.p_kuafang_mate_apply>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and kuafang_id = {p_kuafang.id} and kuafang_mate_id = {p_kuafang_mate.id} and status != {ModelDb.p_kuafang_mate_apply.status_enum.无效.ToSByte()}", false);
                        if (!exists_apply.IsNullOrEmpty()) throw new Exception("当前跨房已有报名不可修改");

                        p_kuafang_mate.tg_user_sn2 = null;
                        p_kuafang_mate.ting_sn2 = null;
                        p_kuafang_mate.is_open = reqData.is_open;
                        p_kuafang_mate.amont = reqData.amont;
                        p_kuafang_mate.content = reqData.content;

                        p_kuafang_mate.Update();
                    }

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_kuafang_mate
                {

                }
                #endregion
            }
            #endregion

            #region 填报信息列表（我的）
            /// <summary>
            /// 填报信息列表
            /// </summary>
            public class MyList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("id")
                    {

                    });
                    return listFilter;
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "跨房时间",
                        width = "110",
                        minWidth = "110"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amont_text")
                    {
                        text = "目标音浪",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy2_name")
                    {
                        text = "对手运营",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg2_name")
                    {
                        text = "对手厅管",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting2_name")
                    {
                        text = "对手厅名",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "解除报名",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = TerminatedAction,
                            field_paras = "id",
                        },
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.等于,
                            field = "tg_user_sn2",
                            value = null
                        }
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        },
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                            field = "tg_user_sn2",
                            value = null
                        }
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
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_user_sn1 = '{new UserIdentityBag().user_sn}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_kuafang_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_kuafang_mate
                {
                    public ModelDb.p_kuafang p_kuafang
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_kuafang>($"id = {kuafang_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1);
                        }
                    }
                    public string amont_text
                    {
                        get
                        {
                            return amont.ToInt().ToString() + "W";
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting.ting_name;
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting2
                    {
                        get
                        {
                            return !ting_sn2.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2) : new ServiceFactory.UserInfo.Ting.TingInfo();
                        }
                    }
                    public string yy2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting2.yy_user_sn).name;
                        }
                    }
                    public string tg2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn2).name;
                        }
                    }
                    public string ting2_name
                    {
                        get
                        {
                            return ting2.ting_name;
                        }
                    }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 解除报名
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction TerminatedAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_kuafang_mate>();
                    var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {reqData.id}");
                    p_kuafang_mate.tg_user_sn2 = "[null]";
                    p_kuafang_mate.ting_sn2 = "[null]";
                    lSql.Add(p_kuafang_mate.UpdateTran($"id = {p_kuafang_mate.id}"));

                    // 删除报名记录
                    var p_kuafang_mate_apply = DoMySql.FindEntity<ModelDb.p_kuafang_mate_apply>($"kuafang_mate_id = {p_kuafang_mate.id}");
                    p_kuafang_mate_apply.status = ModelDb.p_kuafang_mate_apply.status_enum.无效.ToSByte();
                    lSql.Add(p_kuafang_mate_apply.UpdateTran($"id = {p_kuafang_mate_apply.id}"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_kuafang_mate>();
                    var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {reqData.id}");

                    if (!p_kuafang_mate.ting_sn2.IsNullOrEmpty()) throw new Exception("跨房已匹配成功不可删除");

                    lSql.Add(p_kuafang_mate.DeleteTran($"id in ({p_kuafang_mate.id})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                #endregion
            }
            #endregion

            #region 对方信息列表
            /// <summary>
            /// 对方信息列表
            /// </summary>
            public class UserInfoList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("id")
                    {

                    });
                    return listFilter;
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "跨房时间",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/KuaFang/Mate/UserInfo",
                            field_paras = "id"
                        },
                        text = "查看",
                        name = "UserInfo"
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
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and kuafang_id = {new ServiceFactory.KuaFang.Common().getNewKuaFang().id} and (tg_user_sn1 = '{new UserIdentityBag().user_sn}' or tg_user_sn2 = '{new UserIdentityBag().user_sn}')";

                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_kuafang_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_kuafang_mate
                {
                    public ModelDb.p_kuafang p_kuafang
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_kuafang>($"id = {kuafang_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            if (tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                            }
                            else
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                            }
                        }
                    }
                }
                #endregion
                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 对方信息
            /// <summary>
            /// 对方信息页面
            /// </summary>
            public class UserInfoView
            {
                #region DefaultView
                public ModelBasic.PageDetail Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageDetail("");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PageDetail pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_kuafang = new ServiceFactory.KuaFang.Common().getNewKuaFang();
                    var df_ting_sn = ""; //对方厅sn
                    var step = 0; // 阶段（0.未提交未报名 1.已发起但没有人报名 4.配对完成）
                    var p_kuafang_mate = DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {req.id}");
                    if (p_kuafang_mate.tg_user_sn1 == new UserIdentityBag().user_sn)
                    {
                        df_ting_sn = p_kuafang_mate.ting_sn2;
                        if (df_ting_sn.IsNullOrEmpty())
                        {
                            step = 1;
                        }
                    }
                    else
                    {
                        df_ting_sn = p_kuafang_mate.ting_sn1;
                    }
                    var dfTingInfo = !df_ting_sn.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Ting().GetTingBySn(df_ting_sn) : new ServiceFactory.UserInfo.Ting.TingInfo();

                    if (!df_ting_sn.IsNullOrEmpty())
                    {
                        step = 4;
                    }

                    #region 表单元素

                    if (!p_kuafang.IsNullOrEmpty())
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                        {
                            title = "跨房时间",
                            defaultValue = p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd")
                        });
                    }

                    if (step < 4)
                    {
                        var msg = "";
                        switch (step)
                        {
                            case 0:
                                msg = "你没有发起或报名任何跨房";
                                break;
                            case 1:
                                msg = "你发起的跨房还没有厅报名";
                                break;
                                break;
                        }
                        if (p_kuafang.IsNullOrEmpty())
                        {
                            msg = "无跨房活动";
                        }

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("msg")
                        {
                            defaultValue = msg
                        });
                    }
                    else
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("weixin")
                        {
                            title = "微信号",
                            defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(dfTingInfo.tg_user_sn).wechat_username
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("dou_user")
                        {
                            title = "抖音号",
                            defaultValue = dfTingInfo.dou_user
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("yy_name")
                        {
                            title = "运营团队",
                            defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(dfTingInfo.yy_user_sn).name
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("ting_name")
                        {
                            title = "厅名",
                            defaultValue = dfTingInfo.ting_name
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("tg_name")
                        {
                            title = "厅管名称",
                            defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(dfTingInfo.tg_user_sn).name
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtLabel("phone")
                        {
                            title = "手机号",
                            defaultValue = dfTingInfo.phone
                        });
                    }

                    #endregion
                    return formDisplay;
                }
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                }
                #endregion
            }
            #endregion

            #region 对战列表
            /// <summary>
            /// 对战列表
            /// </summary>
            public class List
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString()
                    });
                    return listFilter;
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
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "100";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "跨房时间",
                        width = "110",
                        minWidth = "110"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管",
                        width = "140",
                        minWidth = "140"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amont_text")
                    {
                        text = "目标音浪",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy2_name")
                    {
                        text = "对手运营",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg2_name")
                    {
                        text = "对手厅管",
                        width = "140",
                        minWidth = "140"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting2_name")
                    {
                        text = "对手厅名",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        }
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public int id { get; set; }
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"ting_sn2 is not null and ting_sn2 != ''";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            where += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })})";
                            break;
                        case ModelEnum.UserTypeEnum.jder:
                            where += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })})";
                            break;
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_kuafang_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_kuafang_mate
                {
                    public string c_date_text
                    {
                        get
                        {
                            return DoMySql.FindEntityById<ModelDb.p_kuafang>(kuafang_id).c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1);
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting.yy_user_sn).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn1).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting.ting_name;
                        }
                    }
                    public string amont_text
                    {
                        get
                        {
                            return amont.ToInt().ToString() + "W";
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting2
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2);
                        }
                    }
                    public string yy2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting2.yy_user_sn).name;
                        }
                    }
                    public string tg2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn2).name;
                        }
                    }
                    public string ting2_name
                    {
                        get
                        {
                            return ting2.ting_name;
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_kuafang_mate>();
                    var p_kuafang_mate = new ModelDb.p_kuafang_mate();

                    lSql.Add(p_kuafang_mate.DeleteTran($"id in ({reqData.id})"));

                    // 删除报名记录
                    var p_kuafang_mate_apply = new ModelDb.p_kuafang_mate_apply()
                    {
                        status = ModelDb.p_kuafang_mate_apply.status_enum.无效.ToSByte()
                    };
                    lSql.Add(p_kuafang_mate_apply.UpdateTran($"kuafang_mate_id = {reqData.id}"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                #endregion
            }
            #endregion
        }
    }
}
