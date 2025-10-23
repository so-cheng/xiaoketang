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
    /// 厅战排布
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 厅战排布
            /// <summary>
            /// 
            /// </summary>
            public class PaibuList
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

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "创建厅战",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"Post",
                        }
                    });

                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay(req);
                    listDisplay.operateWidth = "460";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("start_time")
                    {
                        text = "提报开始时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("end_time")
                    {
                        text = "提报结束时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_count_text")
                    {
                        text = "总厅数",
                        width = "80",
                        minWidth = "80"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("join_count")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        text = "参加数",
                        width = "80",
                        minWidth = "80",
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            href = "/TingZhan/Target/Join?id={{d.id}}",
                            title = "参加名单"
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("unjoin_count")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        text = "不参加数",
                        width = "90",
                        minWidth = "90",
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            href = "/TingZhan/Target/UnJoin?id={{d.id}}",
                            title = "不参加名单"
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("uncommit_count")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        text = "未提报数",
                        width = "90",
                        minWidth = "90",
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            href = "/TingZhan/Target/UnList?id={{d.id}}",
                            title = "未提报名单"
                        }
                    });

                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Post",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Post"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/TingZhan/Target/DayPost",
                            field_paras = "id"
                        },
                        text = "上传日均数据",
                        name = "DayPost"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            field_paras = "id",
                            target = "_bank",
                            url = "/TingZhan/Target/GradeList"
                        },
                        text = "对战匹配",
                        name = "GradeList"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            field_paras = "id",
                            target = "_bank",
                            url = "/TingZhan/Score/Total"
                        },
                        text = "对战数据"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            field_paras = "id",
                            target = "_bank",
                            url = "/TingZhan/Statistical/Analysis"
                        },
                        text = "统计报表"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            field_paras = "id",
                            target = "_bank",
                            url = "/TingZhan/Score/HistoryList"
                        },
                        text = "历史战绩",
                        name = "Score"
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
                    string where = $"1=1";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    public string c_date { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_tingzhan
                {
                    public int ting_count_text
                    {
                        get
                        {
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                        {
                                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                            UserSn = new UserIdentityBag().user_sn,
                                        }
                                    }).Count;
                                case ModelEnum.UserTypeEnum.jder:
                                    return new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                        {
                                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地,
                                            UserSn = new UserIdentityBag().user_sn
                                        }
                                    }).Count;
                                case ModelEnum.UserTypeEnum.manager:
                                    return ting_count.ToInt();
                                default:
                                    return 0;
                            }
                        }
                    }
                    public int join_count
                    {
                        get
                        {
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($"tingzhan_id = {id} and yy_user_sn = '{new UserIdentityBag().user_sn}' and amont > 0").Count;
                                case ModelEnum.UserTypeEnum.jder:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($@"tingzhan_id = {id} and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                        {
                                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                            UserSn = new UserIdentityBag().user_sn
                                        }
                                    })}
                            and amont > 0").Count;
                                case ModelEnum.UserTypeEnum.manager:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($"tingzhan_id = {id} and amont > 0").Count;
                                default:
                                    return 0;
                            }
                        }
                    }
                    public int unjoin_count
                    {
                        get
                        {
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($"tingzhan_id = {id} and yy_user_sn = '{new UserIdentityBag().user_sn}' and amont = 0").Count;
                                case ModelEnum.UserTypeEnum.jder:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($@"tingzhan_id = {id} and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                        {
                                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                            UserSn = new UserIdentityBag().user_sn
                                        }
                                    })}
                            and amont = 0").Count;
                                case ModelEnum.UserTypeEnum.manager:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_target>($"tingzhan_id = {id} and amont = 0").Count;
                                default:
                                    return 0;
                            }
                        }
                    }
                    public int uncommit_count
                    {
                        get
                        {
                            return ting_count_text - join_count - unjoin_count;
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                /// <summary>
                /// 批量删除资产
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    var p_tingzhan = new ModelDb.p_tingzhan();
                    p_tingzhan.Delete($"id = ({dtoReqData.id})");
                    return result;
                }
                public class DtoReqData : ModelDb.p_tingzhan
                {
                }
                #endregion
            }
            #endregion

            #region 创建/编辑厅战期数
            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class PaibuPost
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
                    /*
                     buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                    {
                        title = "List",
                        text = "反馈记录",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/Service/FeedBack/List",
                        },
                    });
                     */
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
                    var p_tingzhan = DoMySql.FindEntityById<ModelDb.p_tingzhan>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_tingzhan.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "厅战时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_tingzhan.c_date.IsNullOrEmpty() ? "" : p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("start_time")
                    {
                        title = "提报开始时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        defaultValue = p_tingzhan.start_time.IsNullOrEmpty() ? "" : p_tingzhan.start_time.ToDateTime().ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("end_time")
                    {
                        title = "提报结束时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        defaultValue = p_tingzhan.end_time.IsNullOrEmpty() ? "" : p_tingzhan.end_time.ToDateTime().ToString()
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
                    var p_tingzhan = req.data_json.ToModel<ModelDb.p_tingzhan>();
                    p_tingzhan.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    if (req.GetPara("c_date").IsNullOrEmpty()) throw new Exception("请选择厅战时间");
                    if (req.GetPara("start_time").IsNullOrEmpty()) throw new Exception("请选择提报开始时间");
                    if (req.GetPara("end_time").IsNullOrEmpty()) throw new Exception("请选择提报结束时间");

                    p_tingzhan.ting_count = new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()).Count;

                    p_tingzhan.InsertOrUpdate();

                    if (p_tingzhan.id == 0)
                    {
                        // 长期规则
                        new ServiceFactory.TingZhanService().SetRulelong();
                    }

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}