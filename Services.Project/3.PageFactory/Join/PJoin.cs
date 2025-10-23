using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// 流程名字
        /// </summary>
        public partial class Join
        {

            #region 申请主播

            /// <summary>
            /// 厅管提交补人申请
            /// </summary>
            public class ApplyZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;
                    return formDisplay;
                }

                public class DtoReq
                {
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 申请表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {

                    return new JsonResultAction();
                }

                /// <summary>
                /// 提交表单
                /// </summary>

                public class p_join_need : ModelDb.p_join_need
                {
                    public List<ServiceFactory.Join.MengxinZbService.ApplyItem> l_apply_item { get; set; }
                }

                /// <summary>
                /// 提交表单
                /// </summary>
                public class PostDto : ModelDb.p_join_need
                {
                    public List<ApplyItem> l_apply_item { get; set; }
                    public object age_need_arr { get; set; }
                }

                public class ApplyItem
                {
                    /// <summary>
                    /// 所属档位
                    /// </summary>
                    public string dangwei { get; set; }

                    /// <summary>
                    /// 申请总人数
                    /// </summary>
                    public string count { get; set; }

                    /// <summary>
                    /// 已分配人数
                    /// </summary>
                    public string recruited_count { get; set; }

                    /// <summary>
                    /// 流失人数
                    /// </summary>
                    public string quite_count { get; set; }

                    /// <summary>
                    /// 拉群人数
                    /// </summary>
                    public string inqun_count { get; set; }

                    /// <summary>
                    /// 是否完成此档申请
                    /// </summary>
                    public string is_complete { get; set; }
                }

                #endregion 异步请求处理
            }

            /// <summary>
            /// 运营帮厅管提交补人申请
            /// </summary>
            public class YyApplyZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;
                    return formDisplay;
                }

                public class DtoReq
                {
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 申请表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    return new JsonResultAction();
                }

                public class p_join_need : ModelDb.p_join_need
                {
                    public List<ServiceFactory.Join.MengxinZbService.ApplyItem> l_apply_item { get; set; }
                }

                #endregion 异步请求处理
            }

            #endregion 申请主播

            #region 申请主播记录

            public class ApplyZbList
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
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
                    return pageModel;
                }

                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "审批状态",
                        options = UtilityStatic.CommonHelper.EnumToDictionary<ModelDb.p_join_need.status_enum>(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        placeholder = "申请时间",
                        mold = EmtTimeSelect.Mold.date_range,
                        defaultValue = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd")
                    });

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("apply")
                    {
                        text = "申请主播",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"ApplyZbPost",
                        },
                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "400";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isTotalRow = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        width = "100",
                        minWidth = "100",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "180",
                        minWidth = "180",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "420",
                        minWidth = "420",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请人数",
                        width = "120",
                        minWidth = "120",
                        summaryReq = new Pagination.SummaryReq
                        {
                            title = "申请总人数",
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_status")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_count")
                    {
                        text = "已补人数",
                        width = "150",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            title = "已补总人数",
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        text = "流失人数",
                        width = "150",
                        minWidth = "100",
                        summaryReq = new Pagination.SummaryReq
                        {
                            title = "流失总人数",
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_cause")
                    {
                        text = "备注",
                        width = "280",
                        minWidth = "280",
                    });

                    #endregion 显示列

                    #region 操作列

                    //完成的主播名单
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "tg_need_id=id"
                        },
                        text = "已补名单"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "明细",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.等于,
                            value = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApplyZbPost",
                            field_paras = "id"
                        },
                        text = "编辑",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().DelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()},{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}",
                        },
                        text = "删除"
                    });



                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZhuDongLingQu",
                            field_paras = "id"
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareMode = EmtModel.ListOperateItem.CompareMode.JS函数判断,
                            compareModeFunc = new EmtModel.ListOperateItem.CompareModeFunc
                            {
                                jsCode = $" !(d.status == '{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()}' && new Date(d.create_time) <= new Date('{DateTime.Now.AddDays(-5)}'))"
                            }
                        },

                        text = "主动领取"
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                public class DtoReq
                { }

                #endregion DefaultView

                #region 回调cs函数


                #endregion 回调cs函数

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    var req = reqJson.GetPara();
                    var status = reqJson.GetPara("status");
                    if (!status.IsNullOrEmpty()) where += $" and status = {status.ToInt()}";
                    if (!req["create_time"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["create_time"].ToString(), 0);
                        where += $" and create_time>='{dateRange.date_range_s}' and create_time <='{dateRange.date_range_e.ToDate().AddDays(1)}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where + $" AND (tg_user_sn = '{new UserIdentityBag().user_sn}') order by create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_need, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_need
                {
                    public string approve_status
                    {
                        get
                        {
                            return ((ModelDb.p_join_need.status_enum)this.status).ToString();
                        }
                    }

                    /// <summary>
                    /// 申请主播的完成状态
                    /// </summary>
                    public string applyZB_status
                    {
                        get
                        {
                            return ((ModelDb.p_join_need.complete_status_enum)this.complete_status).ToString();
                        }
                    }

                    public string zb_count_text
                    {
                        get
                        {
                            var apply_details = this.apply_details.ToModel<List<ServiceFactory.Join.MengxinZbService.ApplyItem>>();
                            string result = "";
                            foreach (var item in apply_details)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.count}人;";
                            }
                            return result;
                        }
                    }
                }

                public class apply_details
                {
                    public string dangwei { get; set; }
                    public string count { get; set; }
                    public string recruited_count { get; set; }
                }

                #endregion ListData
            }

            #endregion 申请主播记录

            #region 主播名单

            public class ZbList
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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("zb_username")
                    {
                        placeholder = "主播账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("month")
                    {
                        mold = EmtTimeSelect.Mold.month,
                        placeholder = "月份",
                        disabled = true,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"在职",ModelDb.user_info_zb.status_enum.正常.ToInt().ToString()},
                            {"流失",ModelDb.user_info_zb.status_enum.已流失.ToInt().ToString()}
                        },
                        disabled = true,
                        placeholder = "在职状态",
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
                        attachPara = new Dictionary<string, object>
                        {
                            { "tg_need_id",req.tg_need_id }
                        }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("term")
                    {
                        text = "期数",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level")
                    {
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("qun_time")
                    {
                        text = "拉群时间",
                        width = "160",
                        minWidth = "160",
                        disabled = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_username")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "120",
                        minWidth = "120",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("sessions_text")
                    {
                        text = "接档时间",
                        width = "180",
                        minWidth = "180",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("qun")
                    {
                        text = "对接群",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("qun_in")
                    {
                        text = "是否进群",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        text = "备注",
                        width = "380",
                        minWidth = "180",
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "BatchPost",
                            field_paras = "id"
                        },
                        text = "背调",
                        name = "BatchPost"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "BackPost",
                            field_paras = "ids=id,tg_need_id"
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareMode = EmtModel.ListOperateItem.CompareMode.JS函数判断,
                            compareModeFunc = new EmtModel.ListOperateItem.CompareModeFunc
                            {
                                jsCode = $"d.is_qun != '{ModelDb.user_info_zb.is_qun_enum.已拉群.ToInt()}' || d.quit_status!='{ModelDb.user_info_zb.quit_status_enum.未退回.ToInt()}'"
                            }
                        },
                        text = "退回",
                        name = "BackPost"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = CancelAction,
                            field_paras = "id"
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareMode = EmtModel.ListOperateItem.CompareMode.字段比较,
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            field = "quit_status",
                            value = $"{ModelDb.user_info_zb.quit_status_enum.等待萌新处理.ToInt()}",
                        },
                        text = "撤销退回",
                        name = "BackPost"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareMode = EmtModel.ListOperateItem.CompareMode.JS函数判断,
                            compareModeFunc = new EmtModel.ListOperateItem.CompareModeFunc
                            {
                                jsCode = $"d.is_qun != '{ModelDb.user_info_zb.is_qun_enum.已拉群.ToInt()}' || d.quit_status!='{ModelDb.user_info_zb.quit_status_enum.未退回.ToInt()}'"
                            }
                        },
                        text = "流失",
                        name = "CausePost",
                        disabled = true
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 异步请求处理
                /// <summary>
                /// 取消退回
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction CancelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(req.GetPara<ModelDb.user_info_zb>().id);
                    if (user_info_zb.quit_status == ModelDb.user_info_zb.quit_status_enum.等待萌新处理.ToSByte())
                    {
                        user_info_zb.quit_status = ModelDb.user_info_zb.quit_status_enum.未退回.ToSByte();
                        user_info_zb.note = $"日期:{DateTime.Today.ToString("yyyy-MM-dd")}  已撤销退回";
                        user_info_zb.Update();
                    }
                    return info;
                }


                /// <summary>
                /// 定义表单模型
                /// </summary>
                #endregion

                public class DtoReq
                {
                    /// <summary>
                    /// 厅管补人申请单id
                    /// </summary>
                    public int tg_need_id { get; set; }

                    /// <summary>
                    /// 是否为总数据
                    /// </summary>
                    public bool isTotalInfo { get; set; } = true;

                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    var tg_need_id = reqJson.GetPara("tg_need_id").ToInt();

                    //2.筛选条件
                    string where = $"1=1";
                    var zb_username = reqJson.GetPara("zb_username");
                    if (!zb_username.IsNullOrEmpty()) where += $" and zb_username like '%{zb_username}%'";
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        where += $" and tg_user_sn='{new UserIdentityBag().user_sn}'";
                    }
                    if (tg_need_id > 0)
                    {
                        where += $" AND (tg_need_id = {tg_need_id})";
                    }
                    if (!reqJson.GetPara("month").IsNullOrEmpty())
                    {
                        where += $" and create_time>='{reqJson.GetPara("month").ToDate().ToString("yyyy-MM-dd")}' and create_time<='{reqJson.GetPara("month").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status='{reqJson.GetPara("status")}'";
                    }
                    //3.根据id获取主播名单
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "id desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zb
                {
                    public string sessions_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", sessions);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return address.IsNullOrEmpty() ? province + city : address;
                        }
                    }

                    public string tg_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn).username;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn).name;
                        }
                    }
                }

                #endregion ListData
            }

            public class QuitedZbList
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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("zb_username")
                    {
                        placeholder = "主播账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("month")
                    {
                        mold = EmtTimeSelect.Mold.month,
                        placeholder = "月份",
                        disabled = true,
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
                        attachPara = new Dictionary<string, object>
                        {
                            { "id", req.id },
                            { "yy_user_sn", req.yy_user_sn },
                            { "tg_user_sn", req.tg_user_sn },
                            { "LeaveIndays", req.LeaveIndays },
                        }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("old_tg_username")
                    {
                        text = "流失前所属厅",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "380",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        text = "备注",
                        width = "380",
                        minWidth = "180",
                    });

                    #endregion 显示列

                    #region 操作列


                    #endregion 操作列

                    return listDisplay;

                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                    /// <summary>
                    /// 所属厅管
                    /// </summary>
                    public string tg_user_sn { get; set; }
                    /// <summary>
                    /// 所属运营
                    /// </summary>
                    public string yy_user_sn { get; set; }
                    /// <summary>
                    /// 多少天内流失
                    /// </summary>
                    public string LeaveIndays { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    var p_join_need_id = reqJson.GetPara("id").ToInt();

                    //2.筛选条件
                    string where = $"(no_share_time is not null OR leave_date is not null)";
                    var zb_username = reqJson.GetPara("zb_username");
                    if (!zb_username.IsNullOrEmpty()) where += $" and zb_username like '%{zb_username}%'";
                    if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        where += $" and tg_user_sn='{new UserIdentityBag().user_sn}'";
                    }
                    //if (p_join_need_id >= 1)
                    //{
                    //    where += $" AND (tg_need_id = {p_join_need_id})"; 
                    //}

                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" AND old_tg_user_sn in {new ServiceFactory.YyInfoService().YyGetNextTgForSql(reqJson.GetPara("yy_user_sn"))}";
                    }

                    if (!reqJson.GetPara("tg_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn='{reqJson.GetPara("tg_user_sn")}'";
                    }

                    if (!reqJson.GetPara("LeaveIndays").IsNullOrEmpty())
                    {
                        int days = reqJson.GetPara("LeaveIndays").ToInt();
                        where += $" AND (no_share_time <= qun_time + INTERVAL {days} DAY OR leave_date <= qun_time + INTERVAL {days} DAY)";
                    }

                    if (!reqJson.GetPara("month").IsNullOrEmpty())
                    {
                        where += $" and create_time>='{reqJson.GetPara("month").ToDate().ToString("yyyy-MM-dd")}' and create_time<='{reqJson.GetPara("month").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'";
                    }

                    if (!reqJson.GetPara("tui_status").IsNullOrEmpty())
                    {
                        if (reqJson.GetPara("tui_status") == "0") { where += $" and (tui_status in (0,1,4) or tui_status is null or tui_status = '')"; }
                        if (reqJson.GetPara("tui_status") == "1") { where += $" and tui_status in (2,3)"; }
                    }

                    //3.根据id获取主播名单
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zb
                {

                }

                #endregion ListData
            }

            /// <summary>
            /// 主播流失原因
            /// </summary>
            public class ZbCausePost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                    {
                        {"id",req.id },
                    }
                    };
                    return pageModel;
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

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "ids",
                        colLength = 6,
                        defaultValue = req.id,
                    });

                    formDisplay.formItems.Add(new EmtTimeSelect("date")
                    {
                        mold = EmtTimeSelect.Mold.date,
                        title = "流失时间",
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });
                    formDisplay.formItems.Add(new EmtSelect("cause_select")
                    {
                        title = "流失原因",
                        options = new Dictionary<string, string>
                    {
                        {"联系不到人","联系不到人"},
                        {"未参加培训就离开","未参加培训就离开"},
                        {"参加完培训/观察完厅后/上麦后觉得不合适流失","参加完培训/观察完厅后/上麦后觉得不合适流失"},
                        {"沟通正常，突然联系不到/拉黑","沟通正常，突然联系不到/拉黑"},
                        {"刚对接开始请长假","刚对接开始请长假"},
                        {"工作原因冲突","工作原因冲突"},
                        {"环境不允许","环境不允许"},
                    },
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });
                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "流失原因",
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 签约id
                    /// </summary>
                    public string id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    string date = req.GetPara()["date"].ToString();
                    string cause = req.GetPara()["cause_select"].ToString();
                    if (!req.GetPara("cause").IsNullOrEmpty())
                    {
                        cause = req.GetPara("cause");
                    }

                    if (date.IsNullOrEmpty()) { throw new Exception("请填写流失时间"); }
                    if (cause.IsNullOrEmpty()) { throw new Exception("请填写流失原因"); }
                    var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(req.GetPara("id").ToInt());
                    user_info_zb.no_share = $"流失时间:{date},原因:{cause}";
                    user_info_zb.status = ModelDb.user_info_zb.status_enum.已流失.ToSByte();
                    user_info_zb.quit_status = ModelDb.user_info_zb.quit_status_enum.流失.ToSByte();
                    new ServiceFactory.Join.MengxinZbService().ChangeLevel(user_info_zb, "D", cause);
                    return new JsonResultAction();
                }

                public class p_join_need : ModelDb.p_join_need
                {
                    public List<ApplyItem> l_apply_item { get; set; }
                    public string[] age_need_arr { get; set; }
                }

                public class ApplyItem
                {
                    public int dangwei { get; set; }
                    public int count { get; set; }
                    public int recruited_count { get; set; }
                }

                #endregion 异步请求处理
            }

            #endregion 主播名单

            #region 补人表单详情

            public class ZbDetails
            {
                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("details");
                    pageModel.formDisplay = GetDetails(pageModel, req);
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetDetails(PageDetail pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                        isDisplay = false,
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅名",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.tg_user_sn).username,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_need.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyZbPost.ApplyItem>>();
                    string l_apply_item_content = "";
                    foreach (var item in apply_details)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr><td>{dangwei_name}</td><td>{item.count}</td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th>申请人数</th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播数",
                        defaultValue = p_join_need.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_need.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_need.create_time.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("approver_username")
                    {
                        title = "运营审批",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username,
                        displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("approve_time")
                    {
                        title = "运营审批时间",
                        defaultValue = p_join_need.approve_time.ToString(),
                        displayStatus = p_join_need.approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("notes")
                    {
                        title = "运营审批原因",
                        defaultValue = p_join_need.notes,
                        displayStatus = p_join_need.notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("m_approver_username")
                    {
                        title = "公会审批",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username,
                        displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("m_approve_time")
                    {
                        title = "管理审批时间",
                        defaultValue = p_join_need.m_approve_time.ToString(),
                        displayStatus = p_join_need.m_approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("m_notes")
                    {
                        title = "管理审批原因",
                        defaultValue = p_join_need.m_notes,
                        displayStatus = p_join_need.m_notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("status")
                    {
                        title = "审批结果",
                        defaultValue = p_join_need.status.ToEnum<ModelDb.p_join_need.status_enum>().ToString(),
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                }
            }

            #endregion 补人表单详情

            #region 补人退回

            /// <summary>
            /// 修改主播背调
            /// </summary>
            public class ZbBackPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"ids",req.ids },
                            {"tg_need_id",req.tg_need_id },
                        }
                    };
                    return pageModel;
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

                    formDisplay.formItems.Add(new EmtHidden("ids")
                    {
                        title = "ids",
                        colLength = 6,
                        defaultValue = req.ids,
                    });
                    formDisplay.formItems.Add(new EmtHidden("tg_need_id")
                    {
                        title = "tg_need_id",
                        colLength = 6,
                        defaultValue = req.tg_need_id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtInput("note")
                    {
                        title = "退回理由",
                        colLength = 6,
                        defaultValue = "",
                        displayStatus = EmtModelBase.DisplayStatus.隐藏
                    });

                    formDisplay.formItems.Add(new EmtSelect("note_select")
                    {
                        title = "退回理由",
                        options = new Dictionary<string, string>
                        {
                            {"时间段不匹配","时间段不匹配"},
                            {"学员觉得原厅不合适，仅允许换一次","学员觉得原厅不合适，仅允许换一次"},
                            {"分厅后两天联系不上","分厅后两天联系不上"},
                        },
                        colLength = 12,
                        defaultValue = "",
                    });

                    formDisplay.formItems.Add(new EmtHtml("html")
                    {
                        colLength = 12,
                        Content = "如有特殊情况联系萌新培训老师"
                    });
                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 签约id
                    /// </summary>
                    public string ids { get; set; }

                    public int tg_need_id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    string ids = req.GetPara()["ids"].ToString();


                    var id_array = ids.Split(',');
                    if (req.GetPara("note_select").IsNullOrEmpty() && req.GetPara("note").IsNullOrEmpty())
                    {
                        throw new WeicodeException("请选择退回原因");
                    }
                    foreach (var id in id_array)
                    {
                        var user_info_zb = DoMySql.FindEntityById<ModelDb.user_info_zb>(id.ToInt());
                        user_info_zb.quit_status = ModelDb.user_info_zb.quit_status_enum.等待萌新处理.ToSByte();

                        string note = req.GetPara("note_select");
                        if (!req.GetPara("note").IsNullOrEmpty())
                        {
                            note = req.GetPara("note");
                        }
                        user_info_zb.note = $"退回操作人:{new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).name}" + ";原因:" + note;

                        lSql.Add(user_info_zb.UpdateTran($"id = {user_info_zb.id}"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                public class p_join_need : ModelDb.p_join_need
                {
                    public List<ApplyItem> l_apply_item { get; set; }
                    public string[] age_need_arr { get; set; }
                }

                public class ApplyItem
                {
                    public int dangwei { get; set; }
                    public int count { get; set; }
                    public int recruited_count { get; set; }
                    public int quite { get; set; }
                }

                #endregion 异步请求处理
            }

            #endregion 补人退回

            #region 管理端_补人详情

            public class WX_ZG_ZbDetails
            {
                #region DefaultView

                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("details");

                    pageModel.formDisplay = GetDetails(pageModel, req);
                    return pageModel;
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlFormDisplay GetDetails(PageDetail pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                        isDisplay = false,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_need.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyZbPost.ApplyItem>>();
                    string l_apply_item_content = "";
                    foreach (var item in apply_details)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr>" +
                            $"<td>{dangwei_name}</td>" +
                            $"<td>{item.count.ToInt()}</td>" +
                            $"<td>{item.recruited_count.ToInt()}</td>" +
                            $"<td>{item.recruited_count.ToInt() - item.inqun_count.ToInt()}</td>" +
                            $"<td>{item.inqun_count.ToInt()}</td>" +
                            $"<td><a class='layui-btn c_button' href=\"javascript: win_pop_iframe('补人', 'MX_ChooseZbPost?id={p_join_need.id}&dangwei={item.dangwei}&tg_user_sn={p_join_need.tg_user_sn}', '100%', '100%', ''); \">补人</a></td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th>申请人数</th><th>已分配</th><th>待拉群</th><th>已补人数</th><th>操作</th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 700px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请人数",
                        defaultValue = p_join_need.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "备注",
                        defaultValue = p_join_need.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_need.create_time.ToString(),
                    });
                    //formDisplay.formItems.Add(new EmtLabel("approver_username")
                    //{
                    //    title = "运营审批人",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username,
                    //    displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    //});
                    formDisplay.formItems.Add(new EmtLabel("approve_time")
                    {
                        title = "运营审批时间",
                        defaultValue = p_join_need.approve_time.ToString(),
                        displayStatus = p_join_need.approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("notes")
                    {
                        title = "运营审批原因",
                        defaultValue = p_join_need.notes,
                        displayStatus = p_join_need.notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    //formDisplay.formItems.Add(new EmtLabel("m_approver_username")
                    //{
                    //    title = "管理审批人",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username,
                    //    displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    //});
                    formDisplay.formItems.Add(new EmtLabel("m_approve_time")
                    {
                        title = "审批时间",
                        defaultValue = p_join_need.m_approve_time.ToString(),
                        displayStatus = p_join_need.m_approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("m_notes")
                    {
                        title = "管理审批原因",
                        defaultValue = p_join_need.m_notes,
                        displayStatus = p_join_need.m_notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("status")
                    {
                        title = "状态",
                        defaultValue = p_join_need.status.ToEnum<ModelDb.p_join_need.status_enum>().ToString(),
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion ListData
            }

            #endregion 管理端_补人详情

            #region 主动领取

            public class ZhuDongLingQu
            {
                #region DefaultView

                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("details");

                    pageModel.formDisplay = GetDetails(pageModel, req);
                    return pageModel;
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlFormDisplay GetDetails(PageDetail pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                        isDisplay = false,
                    });
                    //formDisplay.formItems.Add(new EmtLabel("yy_username")
                    //{
                    //    title = "运营账号",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    //});
                    //formDisplay.formItems.Add(new EmtLabel("tg_username")
                    //{
                    //    title = "申请人(厅管)",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.tg_user_sn).username,
                    //});
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_need.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyZbPost.ApplyItem>>();
                    string l_apply_item_content = "";
                    foreach (var item in apply_details)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr>" +
                            $"<td>{dangwei_name}</td>" +
                            $"<td>{item.count}</td>" +
                            $"<td>{item.recruited_count}</td>" +
                            $"<td>{item.inqun_count}</td>" +
                            $"<td><a class='layui-btn c_button' href=\"javascript: win_pop_iframe('补人', 'Zdlq_ChooseZbPost?id={p_join_need.id}&dangwei={item.dangwei}&tg_user_sn={p_join_need.tg_user_sn}', '100%', '100%', ''); \">补人</a></td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th>人数</th><th>已补</th><th style='width:70px;'>已加群人数</th><th>操作</th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 500px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请人数",
                        defaultValue = p_join_need.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "备注",
                        defaultValue = p_join_need.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_need.create_time.ToString(),
                    });
                    //formDisplay.formItems.Add(new EmtLabel("approver_username")
                    //{
                    //    title = "运营审批人",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username,
                    //    displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    //});
                    formDisplay.formItems.Add(new EmtLabel("approve_time")
                    {
                        title = "运营审批时间",
                        defaultValue = p_join_need.approve_time.ToString(),
                        displayStatus = p_join_need.approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("notes")
                    {
                        title = "运营审批原因",
                        defaultValue = p_join_need.notes,
                        displayStatus = p_join_need.notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    //formDisplay.formItems.Add(new EmtLabel("m_approver_username")
                    //{
                    //    title = "管理审批人",
                    //    defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username,
                    //    displayStatus = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.m_approver_user_sn).username.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    //});
                    formDisplay.formItems.Add(new EmtLabel("m_approve_time")
                    {
                        title = "审批时间",
                        defaultValue = p_join_need.m_approve_time.ToString(),
                        displayStatus = p_join_need.m_approve_time.ToString().IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("m_notes")
                    {
                        title = "管理审批原因",
                        defaultValue = p_join_need.m_notes,
                        displayStatus = p_join_need.m_notes.IsNullOrEmpty() ? EmtModelBase.DisplayStatus.隐藏 : EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new EmtLabel("status")
                    {
                        title = "状态",
                        defaultValue = p_join_need.status.ToEnum<ModelDb.p_join_need.status_enum>().ToString(),
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion ListData
            }

            #endregion 主动领取

            #region 主动领取_补人列表
            public class Zdlq_ChooseZbList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");
                    //pageModel.postedReturn = new PagePost.PostedReturn
                    //{
                    //    returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    //};
                    pageModel.listFilter = GetListFilter(req);
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("sessions")
                    {
                        defaultValue = req.tg_dangwei.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("tg_need_id")
                    {
                        defaultValue = req.tg_need_id.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "审批状态",
                        options = new Dictionary<string, string>
                    {
                        {"未审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                        {"已审批", ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString()},
                    },
                        defaultValue = ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString(),
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
                        {"C","C"},
                        {"D","D"},
                    },
                        placeholder = "主播分级",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        placeholder = "微信账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        placeholder = "抖音账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessionsIds")
                    {
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        placeholder = "接档时间",
                        width = "200"
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
                    listDisplay.isHideOperate = true;
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level_text")
                    {
                        width = "130",
                        text = "主播分级",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("sessions_text")
                    {
                        width = "200",
                        text = "接档时间",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        width = "130",
                        text = "微信昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        width = "130",
                        text = "微信账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        width = "130",
                        text = "抖音账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        width = "60",
                        text = "年龄",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        width = "150",
                        text = "地区",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        width = "280",
                        text = "说明",
                    });

                    #endregion 显示列

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new EmtModel.ButtonItem("")
                        {
                            text = "补人",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = ApproveAction,
                                attachPara= new Dictionary<string, object>
                                {
                                    {"tg_user_sn",req.tg_user_sn},
                                    {"tg_need_id",req.tg_need_id},
                                    {"tg_dangwei",req.tg_dangwei},
                                },
                                returnReload = EmtModel.ButtonItem.EventCsAction.ReturnReload.parent
                            },
                        },
                        new EmtModel.ButtonItem("")
                        {
                            text = "标记错误",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = Mark,
                                returnReload = EmtModel.ButtonItem.EventCsAction.ReturnReload.self
                            },
                        },
                    }
                    });
                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction ApproveAction(JsonRequestAction req)
                {
                    var dtoReqData = req.GetPara();
                    List<string> lSql = new List<string>();
                    var l_user_info_zb = dtoReqData["check_data"].ToModel<List<ModelDb.user_info_zb>>();
                    foreach (var item in l_user_info_zb)
                    {
                        item.tg_user_sn = dtoReqData["tg_user_sn"].ToString();
                        item.tg_need_id = dtoReqData["tg_need_id"].ToInt();
                        item.tg_dangwei = dtoReqData["tg_dangwei"].ToInt();
                        item.supplement_time = DateTime.Now;
                        item.is_change = ModelDb.user_info_zb.is_change_enum.不换.ToSByte();
                        item.is_fast = ModelDb.user_info_zb.is_fast_enum.不加急.ToSByte();
                        lSql.Add(item.UpdateTran());
                    }

                    var p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{dtoReqData["tg_need_id"].ToString()}'");
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyItem>>();
                    int finish_zb_count = p_join_need.finish_zb_count.ToInt();

                    //todo:这里没有调用JoinService的方法，需要修改
                    foreach (var item in apply_details)
                    {
                        if (item.dangwei == dtoReqData["tg_dangwei"].ToString())
                        {
                            int no_supplement = item.count.ToInt() - item.recruited_count.ToInt();
                            item.recruited_count = (item.recruited_count.ToInt() + l_user_info_zb.Count).ToString();
                            if (item.recruited_count.ToInt() > item.count.ToInt())
                            {
                                throw new Exception($"领取人数过多(未分配{no_supplement}人,你领取了{l_user_info_zb.Count}人)");
                            }

                            finish_zb_count += l_user_info_zb.Count;
                        }
                    }
                    p_join_need.apply_details = apply_details.ToJson();

                    p_join_need = new ServiceFactory.Join.MengxinZbService().CaculateApproveInfo(p_join_need);
                    lSql.Add(p_join_need.UpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                public JsonResultAction Mark(JsonRequestAction req)
                {
                    var dtoReqData = req.GetPara();
                    List<string> lSql = new List<string>();
                    var l_user_info_zb = dtoReqData["check_data"].ToModel<List<ModelDb.user_info_zb>>();
                    foreach (var item in l_user_info_zb)
                    {
                        item.dou_username = "(错误)" + item.dou_username;
                        lSql.Add(item.UpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int tg_need_id { get; set; }
                    public int tg_dangwei { get; set; }
                    public string tg_user_sn { get; set; }
                }

                public class ApplyItem
                {
                    public string dangwei { get; set; }
                    public string count { get; set; }
                    public string recruited_count { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and mx_sn is not null and mx_sn!='' and (tg_user_sn is null or tg_user_sn ='') and zb_level != '-' and zb_level != 'C' and zb_level != 'D'";
                    var status = reqJson.GetPara("status");
                    //if (!status.IsNullOrEmpty()) where += $" and status = {status.ToInt()}";
                    if (!reqJson.GetPara("sessions").IsNullOrEmpty())
                    {
                        where += $" and sessions like '%{reqJson.GetPara("sessions")}%'";
                    }
                    if (!reqJson.GetPara("tg_need_id").IsNullOrEmpty())
                    {
                        var need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{reqJson.GetPara("tg_need_id")}'", false);
                        if (!need.IsNullOrEmpty())
                        {
                            where += $" and zb_sex = '{need.tg_sex}'";
                        }
                    }
                    if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                    {
                        where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (reqJson.GetPara("sessions").ToInt() > 0)
                    {
                        //厅管自主领取不限档位
                        //where += $" and sessions like '%{reqJson.GetPara("sessions")}%'";
                    }
                    if (!reqJson.GetPara("zb_level").IsNullOrEmpty())
                    {
                        where += $" and zb_level = '{reqJson.GetPara("zb_level")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = $" ORDER BY is_fast DESC,zb_level"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zb
                {
                    public string sessions_text
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", sessions);
                        }
                    }

                    public string mx_sn_text
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).name;
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            if (!address.IsNullOrEmpty())
                            {
                                return address;
                            }
                            else
                            {
                                return province + city;
                            }
                        }
                    }

                    public string zb_level_text
                    {
                        get
                        {
                            string result = zb_level;
                            if (is_fast == ModelDb.user_info_zb.is_fast_enum.加急.ToSByte())
                            {
                                result = zb_level + "(加急)";
                            }
                            if (is_change == ModelDb.user_info_zb.is_change_enum.换厅.ToSByte())
                            {
                                result = zb_level + "(换厅)";
                            }
                            return result;
                        }
                    }
                }

                #endregion ListData
            }

            #endregion 主动领取_补人列表

            #region 外宣主管_补人

            public class WX_ChooseZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                    {
                        {"pjn_id", req.p_join_need_id }
                    }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.p_join_need_id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                    });

                    var bindOptions = new List<ModelDoBasic.Option>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_info_zb>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and mx_sn is not null and tg_user_sn is null"))
                    {
                        bindOptions.Add(new ModelDoBasic.Option
                        {
                            text = new DomainBasic.UserApp().GetInfoByUserSn(item.mx_sn).name,
                            value = item.mx_sn
                        });
                    }
                    formDisplay.formItems.Add(new EmtExt.XmSelect("l_zb")
                    {
                        title = "选择主播",
                        bindOptions = bindOptions
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 档位
                    /// </summary>
                    public string dangwei { get; set; }

                    /// <summary>
                    /// p_join_need_id
                    /// </summary>
                    public int p_join_need_id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    var pjn_id = req.GetPara("pjn_id");
                    var zb_count = req.GetPara("zb_count");
                    var status = req.GetPara("status");
                    var m_notes = req.GetPara("m_notes");
                    if (pjn_id.IsNullOrEmpty()) throw new WeicodeException();
                    if (zb_count.IsNullOrEmpty()) throw new WeicodeException("申请人数不能为空!");
                    if (!zb_count.IsValidInt()) throw new WeicodeException("申请人数必须为整数!");
                    if (status.IsNullOrEmpty()) throw new WeicodeException("请添加审批结果!");

                    //2.提交审批表单,更新数据
                    var lSql = new List<string>();
                    lSql.Add(new ModelDb.p_join_need
                    {
                        status = status.ToInt(),
                        m_approver_user_sn = new UserIdentityBag().user_sn,
                        m_approve_time = DateTime.Now,
                        zb_count = zb_count.ToInt(),
                        m_notes = m_notes,
                    }.UpdateTran($"id = {pjn_id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }

            public class WX_ChooseZbList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");
                    //pageModel.postedReturn = new PagePost.PostedReturn
                    //{
                    //    returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    //};
                    pageModel.listFilter = GetListFilter(req);
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("sessions")
                    {
                        defaultValue = req.tg_dangwei.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("tg_need_id")
                    {
                        defaultValue = req.tg_need_id.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "审批状态",
                        options = new Dictionary<string, string>
                    {
                        {"未审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                        {"已审批", ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString()},
                    },
                        defaultValue = ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString(),
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
                        {"C","C"},
                        {"D","D"},
                    },
                        placeholder = "主播分级",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        placeholder = "微信账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        placeholder = "抖音账号",
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
                    listDisplay.isHideOperate = true;
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level_text")
                    {
                        width = "130",
                        text = "主播分级",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        width = "130",
                        text = "微信昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        width = "130",
                        text = "微信账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        width = "130",
                        text = "抖音账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        width = "60",
                        text = "年龄",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        width = "150",
                        text = "地区",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        width = "280",
                        text = "说明",
                    });

                    #endregion 显示列

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            new EmtModel.ButtonItem("")
                            {
                                text = "补人",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = ApproveAction,
                                    attachPara= new Dictionary<string, object>
                                    {
                                        {"tg_user_sn",req.tg_user_sn},
                                        {"tg_need_id",req.tg_need_id},
                                        {"tg_dangwei",req.tg_dangwei},
                                    },
                                    returnReload = EmtModel.ButtonItem.EventCsAction.ReturnReload.parent
                                },
                            }
                        }
                    });

                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction ApproveAction(JsonRequestAction req)
                {
                    var dtoReqData = req.GetPara();
                    List<string> lSql = new List<string>();
                    var l_user_info_zb = dtoReqData["check_data"].ToModel<List<ModelDb.user_info_zb>>();
                    var zblist = new List<ModelDb.p_join_new_info>();
                    foreach (var item in l_user_info_zb)
                    {
                        item.tg_user_sn = dtoReqData["tg_user_sn"].ToString();
                        item.yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(item.tg_user_sn).yy_sn;
                        item.tg_need_id = dtoReqData["tg_need_id"].ToInt();
                        item.tg_dangwei = dtoReqData["tg_dangwei"].ToInt();
                        item.supplement_time = DateTime.Now;
                        item.is_change = ModelDb.user_info_zb.is_change_enum.不换.ToSByte();
                        item.is_fast = ModelDb.user_info_zb.is_fast_enum.不加急.ToSByte();
                        lSql.Add(item.UpdateTran());
                        zblist.Add(new ModelDb.p_join_new_info 
                        {
                            id=item.id,
                        });
                    }

                    new ServiceFactory.JoinNew().SupplementAction(
                        zblist, 
                        dtoReqData["tg_user_sn"].ToString(),
                        0,
                        0
                        );

                    //todo:这里没有调用JoinService的方法，需要修改
                    var p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{dtoReqData["tg_need_id"].ToString()}'");
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyItem>>();
                    int finish_zb_count = p_join_need.finish_zb_count.ToInt();
                    foreach (var item in apply_details)
                    {
                        if (item.dangwei == dtoReqData["tg_dangwei"].ToString())
                        {
                            item.recruited_count = (item.recruited_count.ToInt() + l_user_info_zb.Count).ToString();
                            if (item.recruited_count.ToInt() > item.count.ToInt())
                            {
                                throw new Exception("补人已超过需求上限");
                            }

                            finish_zb_count += l_user_info_zb.Count;
                        }
                    }
                    p_join_need.apply_details = apply_details.ToJson();

                    p_join_need = new ServiceFactory.Join.MengxinZbService().CaculateApproveInfo(p_join_need);
                    lSql.Add(p_join_need.UpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int tg_need_id { get; set; }
                    public int tg_dangwei { get; set; }
                    public string tg_user_sn { get; set; }
                }

                public class ApplyItem
                {
                    public string dangwei { get; set; }
                    public string count { get; set; }
                    public string recruited_count { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and mx_sn is not null and mx_sn!='' and (tg_user_sn is null or tg_user_sn ='') and zb_level != '-' and zb_level != 'C' and zb_level != 'D'";

                    if (!reqJson.GetPara("sessions").IsNullOrEmpty())
                    {
                        where += $" and sessions like '%{reqJson.GetPara("sessions")}%'";
                    }
                    if (!reqJson.GetPara("tg_need_id").IsNullOrEmpty())
                    {
                        var need = DoMySql.FindEntity<ModelDb.p_join_need>($"id='{reqJson.GetPara("tg_need_id")}'", false);
                        if (!need.IsNullOrEmpty())
                        {
                            where += $" and zb_sex = '{need.tg_sex}'";
                        }
                    }
                    if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                    {
                        where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("zb_level").IsNullOrEmpty())
                    {
                        where += $" and zb_level = '{reqJson.GetPara("zb_level")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = $" ORDER BY is_fast DESC,zb_level"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zb
                {
                    public string mx_sn_text
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).name;
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            if (!address.IsNullOrEmpty())
                            {
                                return address;
                            }
                            else
                            {
                                return province + city;
                            }
                        }
                    }

                    public string zb_level_text
                    {
                        get
                        {
                            string result = zb_level;
                            if (is_fast == ModelDb.user_info_zb.is_fast_enum.加急.ToSByte())
                            {
                                result = zb_level + "(加急)";
                            }
                            if (is_change == ModelDb.user_info_zb.is_change_enum.换厅.ToSByte())
                            {
                                result = zb_level + "(换厅)";
                            }
                            return result;
                        }
                    }
                }

                #endregion ListData
            }

            #endregion 外宣主管_补人

            #region 管理员审批申请List

            public class ApproveApplyZb
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
                        placeholder = "审批状态",
                        options = new Dictionary<string, string>
                        {
                            {"已拒绝", ModelDb.p_join_need.status_enum.已拒绝.ToInt().ToString()},
                            {"等待运营审批", ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待公会审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                            {"等待外宣补人", ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString()},
                            {"已完成", ModelDb.p_join_need.status_enum.已完成.ToInt().ToString()},
                            {"已取消", ModelDb.p_join_need.status_enum.已取消.ToInt().ToString()},
                        },
                        //defaultValue = ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString(),
                        disabled = true
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        placeholder = "申请时间",
                        mold = EmtTimeSelect.Mold.date_range,
                        defaultValue = req.dateRange,
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        placeholder = "男厅/女厅",
                        options = new Dictionary<string, string>
                        {
                            {"男厅", "男"},
                            {"女厅", "女"},
                        },
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        disabled = true,
                        options = DoMySql.FindKvList<ModelDb.user_base>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id='12'", "name,user_sn"),
                        defaultValue = req.yy_user_sn,
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "厅名",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                    });

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "免审白名单",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"WhiteList",
                        },
                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "280";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_username")
                    {
                        text = "所属团队",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅名",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "420",
                        minWidth = "420",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_sex")
                    {
                        width = "100",
                        text = "男女厅",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        width = "170",
                        text = "申请时间",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_created_total")
                    {
                        width = "100",
                        text = "已有主播",
                    });
                    //listDisplay.listItems.Add(new EmtModel.ListItem("open_hours")
                    //{
                    //    width = "100",
                    //    text = "开档时长(h)",
                    //});
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        width = "100",
                        text = "申请数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("month_count")
                    {
                        text = "本月申请人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        width = "100",
                        text = "未分配",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unin_qun_count")
                    {
                        width = "100",
                        text = "待拉群",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("inqun_count")
                    {
                        width = "100",
                        text = "已拉群",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        width = "100",
                        text = "流失数",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_status")
                    {
                        width = "140",
                        text = "审批状态",
                    });

                    #endregion 显示列

                    #region 批量操作

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("aaa")
                    {
                        text = "批量审批",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new EmtModel.ButtonItem("aaa")
                        {
                            text = "同意",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = ApproveAction,
                            },
                        },
                    }
                    });

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApproveApplicationPost",
                            field_paras = "id",
                        },
                        text = "审批",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "tg_need_id=id"
                        },
                        text = "主播名单",
                        disabled = true,
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "详情"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().DelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()},{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}",
                        },
                        text = "删除"
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 回调cs函数
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

                #endregion 回调cs函数

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction ApproveAction(JsonRequestAction req)
                {
                    //1.校验
                    var ids = req.GetPara("ids");
                    if (ids.IsNullOrEmpty()) throw new WeicodeException("请选择申请项!");
                    var p_join_need_list = req.GetPara<List<ModelDb.p_join_need>>("check_data");
                    //2.更新数据
                    List<string> lSql = new List<string>();
                    foreach (var p_join_need in p_join_need_list)
                    {
                        lSql.Add(new ModelDb.p_join_need
                        {
                            status = ModelDb.p_join_need.status_enum.等待外宣补人.ToInt(),
                            m_approver_user_sn = new UserIdentityBag().user_sn,
                            m_approve_time = DateTime.Now,
                        }.UpdateTran($"id = {p_join_need.id}"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                {
                    public string yy_user_sn { get; set; }
                    public string dateRange { get; set; }
                    public string status { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    var status = reqJson.GetPara("status");
                    if (!status.IsNullOrEmpty())
                    {
                        where += $" and status = '{status.ToInt()}'";
                    }

                    var tg_username = new ServiceFactory.TgInfoService().GetTgInfo(reqJson.GetPara("tg_user_sn"));
                    if (!tg_username.IsNullOrEmpty())
                    {
                        where += $" and tg_username like '%{tg_username}%'";
                    }
                    if (!reqJson.GetPara("tg_sex").IsNullOrEmpty())
                    {
                        where += $" and tg_sex='{reqJson.GetPara("tg_sex")}'";
                    }
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn='{reqJson.GetPara("yy_user_sn")}'";
                    }
                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and create_time >= '{dateRange.date_range_s}' and create_time<'{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_need, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_need
                {
                    public string zb_count_text
                    {
                        get
                        {
                            var apply_details = this.apply_details.ToModel<List<ServiceFactory.Join.MengxinZbService.ApplyItem>>();
                            string result = "";
                            foreach (var item in apply_details)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.count}人;";
                            }
                            return result;
                        }
                    }
                    public string month_count
                    {
                        get
                        {

                            var list = DoMySql.FindList<ModelDb.p_join_need>($"tg_user_sn='{tg_user_sn}' and create_time>='{create_time.ToDate().ToString("yyyy-MM-01")}' and create_time<='{create_time.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToDateString()}'");
                            return list.Sum(x => x.zb_count).ToString();
                        }
                    }
                    public string yy_username
                    {
                        get
                        {
                            //todo:0621修改为ServiceFactory.userinfo.Yy
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }

                    public string tg_username
                    {
                        get
                        {
                            //todo:0621修改为ServiceFactory.userinfo.Tg
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    /// <summary>
                    /// 创建人数
                    /// </summary>
                    public int zb_created_total
                    {
                        get
                        {
                            //todo:0621修改为ServiceFactory.userinfo.Tg
                            return new ServiceFactory.UserInfo.Tg().TgGetNextZb(tg_user_sn).Count;
                        }
                    }

                    /// <summary>
                    /// 未分配
                    /// </summary>
                    public int unsupplement_count
                    {
                        get
                        {
                            return (int)(this.zb_count - this.supplement_count);
                        }
                    }

                    public int unin_qun_count
                    {
                        get
                        {
                            return (int)(this.supplement_count - this.inqun_count);
                        }
                    }

                    public string approve_status
                    {
                        get
                        {
                            return ((ModelDb.p_join_need.status_enum)this.status).ToString();
                        }
                    }
                }

                #endregion ListData
            }

            #endregion 管理员审批申请List

            #region 管理员审批主播表单

            public class ApproveZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                    {
                        {"pjn_id", req.id }
                    }
                    };
                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            right = 100,
                        },
                        emtModelBase = new EmtSubmitButton("refuse")
                        {
                            width = "50px",
                            className = "layui-btn layui-btn-primary layui-border-blue btn-submit c__refuse",
                            defaultValue = "拒绝",
                            eventJsClick = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = $"page_post.status.set('{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}')"
                                }
                            }
                        }
                    });
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}");
                    if (p_join_need.status != ModelDb.p_join_need.status_enum.等待公会审批.ToInt()) throw new WeicodeException($"该申请单的审批状态为{p_join_need.status.ToEnum<ModelDb.p_join_need.status_enum>().ToString()}, 管理员无法审批!");
                    var formDisplay = pageModel.formDisplay;
                    formDisplay.buttonSubmitText = "同意";

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                        isDisplay = false,
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "申请人(厅管)",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.tg_user_sn).username,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_need.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前在开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyZbPost.ApplyItem>>();
                    string l_apply_item_content = "";
                    foreach (var item in apply_details)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr><td>{dangwei_name}</td><td>{item.count}</td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th>人数</th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人节奏",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播人数",
                        defaultValue = p_join_need.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_need.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_need.create_time.ToString(),
                    });
                    string defaultStatus = "";
                    if (p_join_need.status == ModelDb.p_join_need.status_enum.等待公会审批.ToInt())
                    {
                        defaultStatus = ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString();
                    }
                    else
                    {
                        defaultStatus = p_join_need.status.ToString();
                    }
                    formDisplay.formItems.Add(new EmtHidden("status")
                    {
                        title = "审批结果",
                        defaultValue = defaultStatus
                    });
                    formDisplay.formItems.Add(new EmtInput("m_notes")
                    {
                        title = "审批原因",
                        defaultValue = p_join_need.m_notes,
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    var pjn_id = req.GetPara("pjn_id");
                    var zb_count = req.GetPara("zb_count");
                    var status = req.GetPara("status");
                    var m_notes = req.GetPara("m_notes");
                    if (pjn_id.IsNullOrEmpty()) throw new WeicodeException();
                    if (zb_count.IsNullOrEmpty()) throw new WeicodeException("申请人数不能为空!");
                    if (!zb_count.IsValidInt()) throw new WeicodeException("申请人数必须为整数!");
                    if (status.IsNullOrEmpty()) throw new WeicodeException("请添加审批结果!");

                    //2.提交审批表单,更新数据
                    var lSql = new List<string>();
                    lSql.Add(new ModelDb.p_join_need
                    {
                        status = status.ToInt(),
                        m_approver_user_sn = new UserIdentityBag().user_sn,
                        m_approve_time = DateTime.Now,
                        zb_count = zb_count.ToInt(),
                        m_notes = m_notes,
                    }.UpdateTran($"id = {pjn_id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }

            #endregion 管理员审批主播表单

            #region 厅管申请主播列表
            public class TGApplyZbList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");

                    //设置tab页

                    string top = "";
                    top += $@"<div class=""layui-tab layui-tab-brief"">";
                    top += $@"   <ul class=""layui-tab-title"">";
                    top += $@"       <li {(req.completeStatus == 0 ? @"class=""layui-this""" : "")} lay-id=""1"" onclick=""location.href='?completeStatus={ModelDb.p_join_need.complete_status_enum.未完成.ToInt()}&approve_status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()}'"">未完成</li>";
                    top += $@"       <li {(req.completeStatus == 1 ? @"class=""layui-this""" : "")} lay-id=""2"" onclick=""location.href='?completeStatus={ModelDb.p_join_need.complete_status_enum.已完成.ToInt()}&approve_status={ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()}'"">已完成</li>";
                    top += $@"   </ul>";
                    top += $@"</div>";

                    pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = top
                    }
                };

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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("tg_username")
                    {
                        placeholder = "厅名",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "所属运营",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status='{ModelDb.user_base.status_enum.正常}'", "username,user_sn"),
                        placeholder = "运营",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("step")
                    {
                        placeholder = "所属阶段",
                        title = "所属阶段",
                        options = new Dictionary<string, string>
                    {
                        { "0.5阶段","0.5"},
                        { "1阶段","1"},
                        { "2阶段","2"},
                        { "3阶段","3"},
                        { "4阶段","4"},
                        { "5阶段","5"},
                        { "6阶段","6"},
                    },
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        placeholder = "男女厅",
                        title = "男女厅",
                        options = new Dictionary<string, string>
                    {
                        { "男","男"},
                        { "女","女"},
                    },
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "申请时间",
                        title = "申请时间",
                        defaultValue = req.dateRange
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtButton("days7")
                    {
                        placeholder = "7天之前",
                        defaultValue = "7天之前",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = $"window.location.href='/Join/CreateZBAccount/TGApplicationList?dateRange='+'{"2025-01-01 ~ " + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")}';"
                            }
                        },
                        title = "7天之前",
                    });
                    return listFilter;
                }

                /// <summary>
                /// 扩展按钮
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("createZb_Btn")
                    {
                        text = "创建主播",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"CreateZbPage",
                        },
                    });
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "280";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50,
                        attachPara = new Dictionary<string, object>
                    {
                        {"complete_status", req.completeStatus },
                        {"approve_status", req.approve_status },
                        {"group_id", req.group_id }
                    }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time_text")
                    {
                        text = "申请时间",
                        sort = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_text")
                    {
                        text = "所属运营",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("step_text")
                    {
                        text = "所属阶段",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅管用户名",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请数",
                        sort = true
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "未分配",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unin_qun_count")
                    {
                        text = "待拉群",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("inqun_count")
                    {
                        text = "已拉群",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        text = "流失数",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("")
                    {
                        text = "本厅流失率",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("")
                    {
                        text = "上月流失率",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("")
                    {
                        text = "申请男生",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_time")
                    {
                        text = "审批时间",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approver_username")
                    {
                        text = "审批人",
                        disabled = true,
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "补人"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "tg_need_id=id"
                        },
                        text = "已补主播"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"QuitedZbList",
                            field_paras = "id"
                        },
                        text = "流失主播"
                    });

                    #endregion 操作列

                    #region 批量操作列
                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量刷新",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                        {
                            func = ReFreshAction,
                        },
                    });
                    #endregion
                    return listDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单是否完成，0:未完成,1:已完成
                    /// </summary>
                    public int completeStatus { get; set; } = 0;

                    /// <summary>
                    /// 审批状态
                    /// </summary>
                    public int approve_status { get; set; } = 2;

                    /// <summary>
                    /// 所属团队id
                    /// </summary>
                    public int group_id { get; set; }

                    /// <summary>
                    /// 时间范围
                    /// </summary>
                    public string dateRange { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有的审批申请
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    var complete_status = reqJson.GetPara("complete_status");
                    var approve_status = reqJson.GetPara("approve_status");
                    var tg_username = reqJson.GetPara("tg_username");
                    if (complete_status.IsNullOrEmpty() || approve_status.IsNullOrEmpty()) throw new WeicodeException("请选择tab页");

                    //2.筛选
                    string where = $"status = {approve_status.ToInt()} and complete_status = {complete_status.ToInt()} and yy_user_sn in(select user_sn from user_base where user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and status=0)";
                    if (!tg_username.IsNullOrEmpty()) where += $" and tg_username like '%{tg_username}%'";
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn='{reqJson.GetPara("yy_user_sn")}'";
                    }
                    if (!reqJson.GetPara("step").IsNullOrEmpty())
                    {
                        where += $" and step='{reqJson.GetPara("step")}'";
                    }

                    if (!reqJson.GetPara("tg_sex").IsNullOrEmpty())
                    {
                        where += $" and tg_sex = '{reqJson.GetPara("tg_sex")}'";
                    }

                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and create_time>='{dateRange.date_range_s}' and create_time <= '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //3.获取所有审批的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        fields = "*,(zb_count-finish_zb_count) as supplement_count,create_time as create_time_text",
                        where = where + $"",
                        orderby = " order by create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_need, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_need
                {
                    public string yy_text
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.yy_user_sn).username;
                        }
                    }

                    public string step_text
                    {
                        get
                        {
                            var jiezou = DoMySql.FindEntity<ModelDb.jiezou_detail>($"tg_user_sn='{tg_user_sn}' order by create_time desc", false);

                            return jiezou.step > 0 ? jiezou.step + "阶段" : "-";
                        }
                    }

                    public int unsupplement_count
                    {
                        get
                        {
                            return (int)(this.zb_count - this.supplement_count);
                        }
                    }

                    public int unin_qun_count
                    {
                        get
                        {
                            return (int)(this.supplement_count - this.inqun_count);
                        }
                    }

                    public string approver_username
                    {
                        get
                        {
                            return DomainBasicStatic.DoMySql.FindEntity<ModelDbBasic.user_base>($"user_sn = '{this.approver_user_sn}'", false).username;
                        }
                    }

                    public string create_time_text
                    {
                        get
                        {
                            return create_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }


                #region 回调函数
                public JsonResultAction ReFreshAction(JsonRequestAction req)
                {
                    //1.校验
                    var ids = req.GetPara("ids") + req.GetPara("id");
                    if (ids.IsNullOrEmpty()) throw new WeicodeException("请至少选择一项!");
                    var p_join_need = DoMySql.FindList<ModelDb.p_join_need>($"id in ({ids})");
                    //2.更新数据
                    var lSql = new List<string>();

                    foreach (var need in p_join_need)
                    {
                        lSql.Add(new ServiceFactory.Join.MengxinZbService().ResetPJoinNeedForEntity(need).UpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    return new JsonResultAction();
                }
                #endregion


                #endregion ListData
            }

            #endregion 厅管申请主播列表

            #region 导入主播

            public class CreateZBPage
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };

                    return pageModel;
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

                    pageModel.formDisplay.formItems.Add(new EmtExcelRead("l_excel")
                    {
                        title = "选择excel表",
                        colItems = new List<EmtExcelRead.ColItem>
                    {
                        new EmtExcelRead.ColItem("zb_username")
                        {
                             title = "主播用户名",
                             edit = "text",
                        },
                        new EmtExcelRead.ColItem("tg_username")
                        {
                             title = "厅管用户名",
                             edit = "text",
                        },
                    },
                        TemplateUrl = "导入创建主播账号.xlsx",
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// p_join_need.id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 提交表单数据
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    var l_excel = req.GetPara<List<Excel>>("l_excel");
                    //1.校验数据
                    if (l_excel == null || l_excel.Count == 0) throw new WeicodeException("数据不存在!");

                    //2 分类：按照厅管分类
                    var dictionary = new Dictionary<string, List<string>>();
                    foreach (var item in l_excel)
                    {
                        if (!dictionary.ContainsKey(item.tg_username))
                        {
                            dictionary[item.tg_username] = new List<string>();
                        }
                        dictionary[item.tg_username].Add(item.zb_username);
                    }
                    //3.遍历厅管完成申请单
                    foreach (var item in dictionary)
                    {
                        //3.1 获取未完成的申请单
                        var unfinishedApplications = GetTgUnfinishedApplications(item.Key);
                        //3.2 完成申请单
                        lSql.AddRange(CompleteApplication(unfinishedApplications, item.Value));
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    return new JsonResultAction();
                }

                /// <summary>
                /// 按照订单顺序完成申请单
                /// </summary>
                /// <param name="p_join_needs">未完成的申请单</param>
                /// <param name="zbs">主播账号</param>
                /// <returns></returns>
                public List<string> CompleteApplication(List<ModelDb.p_join_need> p_join_needs, List<string> zbs)
                {
                    var lSql = new List<string>();
                    var remainZB_count = zbs.Count;
                    int i_zbs = 0;//主播zbs的索引
                    foreach (var item in p_join_needs)
                    {
                        if (remainZB_count <= 0 || item.finish_zb_count >= item.zb_count) break;
                        var allocateZB_count = Math.Min((int)(item.zb_count - item.finish_zb_count), remainZB_count);

                        //创建主播
                        for (int i = i_zbs; i < i_zbs + allocateZB_count; i++)
                        {
                            //1.生成主播
                            var zb_user_base = new ModelDbBasic.user_base
                            {
                                username = zbs[i],
                                user_sn = UtilityStatic.CommonHelper.CreateUniqueSn(),
                                password = "123456",
                                user_type_id = ModelEnum.UserTypeEnum.zber.ToInt(),
                                name = zbs[i]
                            };
                            lSql.Add(new DomainBasic.UserApp().Create(zb_user_base, true));
                            //2.创建主播与厅管的关系
                            lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, item.tg_user_sn, zb_user_base.user_sn));
                            //3.保存excel数据
                            var p_join_finish = new ModelDb.p_join_finish
                            {
                                tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                                tg_user_sn = item.tg_user_sn,
                                zb_user_sn = zb_user_base.user_sn,
                                zb_username = zb_user_base.username,
                                p_join_need_id = item.id,
                            };
                            lSql.Add(p_join_finish.InsertTran());
                        }

                        //更新p_join_need
                        var updateFinishZBCount = item.finish_zb_count + allocateZB_count;
                        lSql.Add(new ModelDb.p_join_need
                        {
                            finish_zb_count = updateFinishZBCount,
                            complete_status = (item.zb_count == updateFinishZBCount) ? ModelDb.p_join_need.complete_status_enum.已完成.ToInt() : ModelDb.p_join_need.complete_status_enum.未完成.ToInt(),
                            status = (item.zb_count == updateFinishZBCount) ? ModelDb.p_join_need.status_enum.已完成.ToInt() : item.status,
                        }.UpdateTran($"id = {item.id}"));

                        i_zbs += allocateZB_count;
                        remainZB_count -= allocateZB_count;
                    }
                    return lSql;
                }

                /// <summary>
                /// 获取厅管未完成的申请单,并按照时间从小到大排序
                /// </summary>
                /// <param name="tg_username"></param>
                /// <returns></returns>
                public List<ModelDb.p_join_need> GetTgUnfinishedApplications(string tg_username)
                {
                    //1.校验厅管是否存在
                    if (DomainBasicStatic.DoMySql.FindEntity<ModelDbBasic.user_base>($"username = '{tg_username}'", false).IsNullOrEmpty()) throw new WeicodeException($"厅管 '{tg_username}' 不存在!");
                    //2.按照时间从小到大的顺序，获取厅管所有未完成的申请单
                    var p_join_needs = DomainBasicStatic.DoMySql.FindList<ModelDb.p_join_need>($"tg_username = '{tg_username}' and zb_count > finish_zb_count and complete_status = {ModelDb.p_join_need.complete_status_enum.未完成.ToInt()} and status = {ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()} order by create_time asc");
                    //3.校验是否有未完成的申请单
                    if (p_join_needs.Count == 0) throw new WeicodeException($"厅管 '{tg_username}' 无可创建主播的名额!");
                    return p_join_needs;
                }

                public class Excel
                {
                    public string id;
                    public string zb_username;
                    public string tg_username;
                }

                #endregion 异步请求处理
            }

            #endregion 导入主播

            #region 运营审批申请List

            public class YYApproveApplyZb
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
                    pageModel.buttonGroup = GetButtonGroup(req);

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
                        placeholder = "审批状态",
                        options = new Dictionary<string, string>
                        {
                            {"已拒绝", ModelDb.p_join_need.status_enum.已拒绝.ToInt().ToString()},
                            {"等待运营审批", ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待公会审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                            {"等待外宣补人", ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString()},
                            {"已完成", ModelDb.p_join_need.status_enum.已完成.ToInt().ToString()},
                            {"已取消", ModelDb.p_join_need.status_enum.已取消.ToInt().ToString()},
                        },
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "所属厅",
                        options = new ServiceFactory.YyInfoService().YyGetNextTgForKv(new UserIdentityBag().user_sn),
                        defaultValue = req.tg_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "申请日期范围",
                        defaultValue = req.dateRange
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("apply")
                    {
                        text = "申请主播",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"/TgManage/ApplyZb/ApplyZbPost",
                        },
                    });
                    return buttonGroup;
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅管用户名",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "420",
                        minWidth = "420",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_total")
                    {
                        text = "名下主播数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("over_count")
                    {
                        text = "超额人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        text = "流失人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请主播人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_count")
                    {
                        text = "已补人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("month_count")
                    {
                        text = "本月申请人数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_cause")
                    {
                        text = "申请原因",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_status")
                    {
                        text = "审批状态",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_time")
                    {
                        text = "运营审批时间",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("notes")
                    {
                        text = "运营审批原因",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approver_username")
                    {
                        text = "运营审批人",
                        disabled = true,
                    });

                    #endregion 显示列

                    #region 批量操作

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量审批",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new EmtModel.ButtonItem("")
                        {
                            text = "批量审批",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = ApproveAction,
                            },
                        },
                    }
                    });

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApproveApplicationPost",
                            field_paras = "id",
                        },
                        text = "审批",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString(),
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "tg_need_id=id"
                        },
                        text = "主播名单",
                        disabled = true,
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "详情",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.等于,
                            field = "status",
                            value = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString(),
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinService().DelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_need.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_need.status_enum.等待公会审批.ToInt()},{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}",
                        },
                        text = "删除"
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction ApproveAction(JsonRequestAction req)
                {
                    //1.校验
                    var ids = req.GetPara("ids") + req.GetPara("id");
                    if (ids.IsNullOrEmpty()) throw new WeicodeException("请选择申请项!");
                    //2.更新数据
                    var lSql = new List<string>();
                    lSql.Add(new ModelDb.p_join_need
                    {
                        status = ModelDb.p_join_need.status_enum.等待公会审批.ToInt(),
                        approver_user_sn = new UserIdentityBag().user_sn,
                        approve_time = DateTime.Now,
                    }.UpdateTran($"id in ({ids})"));
                    DoMySql.ExecuteSqlTran(lSql);

                    new PlatformSdk.WeixinMP().SendTemplateMessage("BIYWu0LpzvnAdQOKkKO2I6Zt6SXgUtFqXKztkSbkGbU", new DomainBasic.UserApp().GetInfoByUserSn("20210504154936061-1809088913").attach4, "有补人申请需要审核", "/Waixuan/ApproveApplication/ApproveApplicationList");

                    return new JsonResultAction();
                }


                #endregion 请求回调函数

                public class DtoReq
                {
                    public string tg_user_sn { get; set; }
                    public string dateRange { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    //2.获取当前运营
                    string cur_yy_user_sn = new UserIdentityBag().user_sn;
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status='{reqJson.GetPara("status")}'";
                    }
                    where += $" and yy_user_sn = '{cur_yy_user_sn}'";
                    if (!reqJson.GetPara("tg_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn='{reqJson.GetPara("tg_user_sn")}'";
                    }
                    if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                    {
                        var date = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange").ToString());
                        where += $" and create_time>='{date.date_range_s}' and create_time < '{date.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //3.获取当前运营下所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_need, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_need
                {

                    public string zb_count_text
                    {
                        get
                        {
                            var apply_details = this.apply_details.ToModel<List<ServiceFactory.Join.MengxinZbService.ApplyItem>>();
                            string result = "";
                            foreach (var item in apply_details)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.count}人;";
                            }
                            return result;
                        }
                    }
                    public string month_count
                    {
                        get
                        {
                            var list = DoMySql.FindList<ModelDb.p_join_need>($"tg_user_sn='{tg_user_sn}' and create_time>='{create_time.ToDate().ToString("yyyy-MM-01")}' and create_time<='{create_time.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToDateString()}'");
                            return list.Sum(x => x.zb_count).ToString();
                        }
                    }
                    public string approve_status
                    {
                        get
                        {
                            return ((ModelDb.p_join_need.status_enum)this.status).ToString();
                        }
                    }

                    //运营审批人
                    public string approver_username
                    {
                        get
                        {
                            return DomainBasicStatic.DoMySql.FindEntity<ModelDbBasic.user_base>($"user_sn = '{this.approver_user_sn}'", false).username;
                        }
                    }

                    public string tg_username
                    {
                        get
                        {
                            return DomainBasicStatic.DoMySql.FindEntity<ModelDbBasic.user_base>($"user_sn = '{this.tg_user_sn}'").username;
                        }
                    }

                    /// <summary>
                    /// 创建人数
                    /// </summary>
                    public int zb_total
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn).Count;
                        }
                    }

                    /// <summary>
                    /// 在职人数
                    /// </summary>
                    public int zb_employed_count
                    {
                        get
                        {
                            var zbs = new DomainUserBasic.UserRelationApp()
                                        .GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.tg_user_sn)
                                        .Where(x => x.status != ModelDb.user_base.status_enum.逻辑删除.ToInt());
                            return zbs.Count();
                        }
                    }

                    /// <summary>
                    /// 离职人数
                    /// </summary>
                    public int zb_departed_count
                    {
                        get
                        {
                            return new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn, DomainUserBasic.UserRelationApp.GetNextUsersType.仅删除).Count;
                        }
                    }

                    public string over_count
                    {
                        get
                        {
                            int total_count = 0;
                            total_count += new ServiceFactory.TgInfoService().TgGetNextZb(tg_user_sn).Count;
                            total_count += new ServiceFactory.UserInfo.Tg().GetApplyZbCount(tg_user_sn);
                            total_count += zb_count.ToInt();

                            return total_count > 15 ? (total_count - 15).ToString() : "0";

                        }
                    }
                }

                #endregion ListData
            }

            /// <summary>
            /// 运营免审核白名单
            /// </summary>
            public class WhiteList
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
                    pageModel.buttonGroup = GetButtonGroup(req);

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
                    //listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    //{
                    //    placeholder = "审批状态",
                    //    options = new Dictionary<string, string>
                    //    {
                    //        {"未审批", ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()},
                    //        {"已审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                    //    },
                    //    defaultValue = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString(),
                    //});

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("apply")
                    {
                        text = "添加白名单",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"AddWhite",
                        },
                    });
                    return buttonGroup;
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "运营用户名",
                    });

                    #endregion 显示列

                    #region 批量操作

                    //listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    //{
                    //    text = "批量审批",
                    //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    //    {
                    //        new EmtModel.ButtonItem("")
                    //        {
                    //            text = "批量审批",
                    //            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                    //            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                    //            {
                    //                func = ApproveAction,
                    //            },
                    //        },
                    //    }
                    //});

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,

                        eventCsAction = new EmtModel.ListOperateItem.EventCsAction
                        {
                            func = WhiteListAction,
                            field_paras = "id"
                        },
                        text = "移除",
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction WhiteListAction(JsonRequestAction req)
                {
                    var p_join_whitelist = DoMySql.FindEntity<ModelDb.p_join_whitelist>($"id='{req.GetPara("id")}'", false);
                    if (p_join_whitelist.id > 0)
                    {
                        p_join_whitelist.Delete();
                    }

                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                { }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"1=1";

                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_whitelist, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_whitelist
                {
                    public string username
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}'", false).username;
                        }
                    }
                }

                #endregion ListData
            }

            public class WhitePost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                        }
                    };
                    return pageModel;
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

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id.ToString(),
                        isDisplay = false,
                    });

                    formDisplay.formItems.Add(new EmtSelect("yy_user_sn")
                    {
                        title = "选择运营",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        defaultValue = req.yy_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "window.location.href='/join/createzbaccount/AddWhite?yy_user_sn='+page_post.yy_user_sn.value",
                            }
                        },
                    });

                    var tgs = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req.yy_user_sn);

                    var list = new List<ModelDoBasic.Option>();
                    foreach (var tg in tgs)
                    {
                        if (DoMySql.FindEntity<ModelDb.p_join_whitelist>($"user_sn='{tg.Value}'", false).IsNullOrEmpty())
                        {
                            list.Add(new ModelDoBasic.Option
                            {
                                text = tg.Key,
                                value = tg.Value
                            });
                        }
                    }

                    formDisplay.formItems.Add(new EmtExt.XmSelect("tg_user_sn")
                    {
                        bindOptions = list,
                        title = "选择厅管",
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; } = 0;

                    /// <summary>
                    /// 所属运营
                    /// </summary>
                    public string yy_user_sn { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var l_tg_user_sn = req.GetPara("tg_user_sn").Split(',');
                    var lSql = new List<string>();
                    var tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    foreach (var tg_user_sn in l_tg_user_sn)
                    {
                        var p_join_whitelist = DoMySql.FindEntity<ModelDb.p_join_whitelist>($"user_sn='{tg_user_sn}'", false);
                        if (p_join_whitelist.id == 0)
                        {
                            p_join_whitelist.tenant_id = tenant_id;
                            p_join_whitelist.user_sn = tg_user_sn;
                            lSql.Add(p_join_whitelist.InsertOrUpdateTran($"user_sn='{tg_user_sn}'"));
                        }
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }

            #endregion 运营审批申请List

            #region 运营审批

            /// <summary>
            /// 运营审批主播表单
            /// </summary>
            public class YYApproveZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}", false);
                    if (p_join_need.id == 0)
                    {
                        pageModel.formDisplay.formItems.Add(new EmtLabel("")
                        {
                            title = "",
                            defaultValue = "该申请已被删除",
                        });
                        return pageModel;
                    }

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                    {
                        {"pjn_id", req.id }
                    }
                    };
                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            right = 100,
                        },
                        emtModelBase = new EmtSubmitButton("refuse")
                        {
                            width = "50px",
                            className = "layui-btn layui-btn-primary layui-border-blue btn-submit c__refuse",
                            defaultValue = "拒绝",
                            eventJsClick = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = $"page_post.status.set('{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}')"
                                }
                            }
                        }
                    });
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {req.id}", false);
                    if (p_join_need.id == 0)
                    {
                        throw new Exception("该申请已被删除");
                    }
                    var formDisplay = pageModel.formDisplay;
                    formDisplay.buttonSubmitText = "同意";

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_need.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅管",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_join_need.tg_user_sn).username,
                    });
                    formDisplay.formItems.Add(new EmtHidden("tg_user_sn")
                    {
                        title = "tg_user_sn",
                        defaultValue = p_join_need.tg_user_sn,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_need.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_need.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_need.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_need.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前在开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var apply_details = p_join_need.apply_details.ToModel<List<ApplyZbPost.ApplyItem>>();

                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.档位时段))
                    {
                        //todo:0621修改为ServiceFactory.userinfo.Yy
                        int ApproveNum = DoMySql.FindList<ModelDb.p_join_need>($@"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and (status='0' or status='1') and tg_user_sn in {new ServiceFactory.UserInfo.Yy().YyGetNextTgForSql(new UserIdentityBag().user_sn)} and apply_details like '%""dangwei"":{item.Value}%'").Count;
                        string key = item.Key;
                        string value = item.Value;

                        if (ApproveNum > 0) { key += $"({ApproveNum}个申请正在审批)"; }
                        option.Add(key, value);
                    }

                    string apply_item_default = "[";

                    foreach (var item in apply_details)
                    {
                        apply_item_default += "{";
                        apply_item_default += $@"""dangwei"":""{item.dangwei}"",";
                        apply_item_default += $@"""count"":{item.count}";
                        apply_item_default += "},";
                    }
                    apply_item_default = apply_item_default.Substring(0, apply_item_default.Length - 1);
                    apply_item_default += "]";

                    formDisplay.formItems.Add(new ModelBasic.EmtTableEdit("l_apply_item")
                    {
                        title = "补人需求",
                        colItems = new List<ModelBasic.EmtTableEdit.ColItem>
                    {
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                             emtModel = new ModelBasic.EmtTableSelect("dangwei")
                             {
                                 title = "档位",
                                 options = option
                             }
                        },
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                             emtModel = new ModelBasic.EmtTableInput("count")
                             {
                                 title = "人数",
                                 placeholder = "补充人数"
                             }
                        },
                    },
                        defaultValue = apply_item_default,
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播人数",
                        defaultValue = p_join_need.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "备注",
                        defaultValue = p_join_need.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_need.create_time.ToString(),
                    });

                    string defaultStatus = "";
                    if (p_join_need.status == ModelDb.p_join_need.status_enum.等待运营审批.ToInt())
                    {
                        defaultStatus = ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString();
                    }
                    else
                    {
                        defaultStatus = p_join_need.status.ToString();
                    }
                    formDisplay.formItems.Add(new EmtHidden("status")
                    {
                        title = "审批结果",
                        defaultValue = defaultStatus
                    });
                    formDisplay.formItems.Add(new EmtInput("notes")
                    {
                        title = "备注",
                        defaultValue = p_join_need.notes,
                    });

                    //todo:(已完成)修改成本月补人的总量
                    int buren = 0;
                    int liushi = 0;
                    //todo:0621修改为ServiceFactory.userinfo.Yy
                    foreach (var tg in new ServiceFactory.UserInfo.Yy().YyGetNextTg(new UserIdentityBag().user_sn))
                    {
                        var needs = DomainBasicStatic.DoMySql.FindList<ModelDb.p_join_need>($"tg_user_sn = {tg.user_sn} and create_time>='{DateTime.Today.ToString("yyyy-MM-01")}' and create_time<='{DateTime.Today.ToString("yyyy-MM-dd")}'");
                        foreach (var item in needs)
                        {
                            buren += item.finish_zb_count.ToInt();
                        }
                        liushi += new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg.user_sn, DomainUserBasic.UserRelationApp.GetNextUsersType.仅删除).Count;
                    }

                    //formDisplay.formItems.Add(new ModelBasic.EmtHtml("title1")
                    //{
                    //    title = "补人申请历史数据审核",
                    //});
                    formDisplay.formItems.Add(new EmtLabel("buren")
                    {
                        title = "本月补人人数",
                        defaultValue = buren.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("liushi")
                    {
                        title = "本月流失人数",
                        defaultValue = liushi.ToString(),
                    });


                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    var pjn_id = req.GetPara("pjn_id");
                    var zb_count = req.GetPara("zb_count");
                    var status = req.GetPara("status");
                    var notes = req.GetPara("notes");
                    var l_apply_item = req.GetPara("l_apply_item").ToModel<List<ApplyZbPost.ApplyItem>>();
                    if (pjn_id.IsNullOrEmpty()) throw new WeicodeException();
                    var p_join_need = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_need>($"id = {pjn_id}");
                    if (p_join_need.status != ModelDb.p_join_need.status_enum.等待运营审批.ToInt()) throw new WeicodeException("该申请单已审批，不可重复审批!");
                    if (zb_count.IsNullOrEmpty()) throw new WeicodeException("申请人数不能为空!");
                    if (!zb_count.IsValidInt()) throw new WeicodeException("申请人数必须为整数!");
                    if (status.IsNullOrEmpty()) throw new WeicodeException("请添加审批结果!");

                    if (!DoMySql.FindEntity<ModelDb.p_join_whitelist>($"user_sn='{p_join_need.tg_user_sn}'", false).IsNullOrEmpty() && status == ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString())
                    {
                        status = ModelDb.p_join_need.status_enum.等待外宣补人.ToInt().ToString();
                    }

                    foreach (var item in l_apply_item)
                    {
                        if (item.dangwei.IsNullOrEmpty())
                        {
                            throw new Exception("档位不可为空");
                        }
                        if (item.count.ToInt() <= 0)
                        {
                            throw new Exception("人数必须大于0");
                        }

                        var compare_p_join_need = DoMySql.FindEntity<ModelDb.p_join_need>($"tg_user_sn = '{req.GetPara("tg_user_sn")}' and apply_details like '%\"dangwei\":\"{item.dangwei}\"%' and id != '{req.GetPara("id")}' and status != '{ModelDb.p_join_need.status_enum.已完成.ToInt()}' and status != '{ModelDb.p_join_need.status_enum.已拒绝.ToInt()}' and status != '{ModelDb.p_join_need.status_enum.已取消.ToInt()}'", false);

                        if (!compare_p_join_need.IsNullOrEmpty())
                        {
                            foreach (var detail in compare_p_join_need.apply_details.ToModel<List<ServiceFactory.Join.MengxinZbService.ApplyItem>>())
                            {
                                if (detail.dangwei == item.dangwei && detail.is_complete != "1")
                                {
                                    throw new Exception($"档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}已被申请,请勿重复提交同档位的申请");
                                }
                            }
                        }
                    }
                    zb_count = l_apply_item.Sum(x => x.count.ToInt()).ToString();
                    //2.提交审批表单,更新数据
                    var lSql = new List<string>();
                    lSql.Add(new ModelDb.p_join_need
                    {
                        status = status.ToInt(),
                        approver_user_sn = new UserIdentityBag().user_sn,
                        approve_time = DateTime.Now,
                        zb_count = zb_count.ToInt(),
                        notes = notes,
                        apply_details = l_apply_item.ToJson()
                    }.UpdateTran($"id = {pjn_id}"));
                    DoMySql.ExecuteSqlTran(lSql);

                    if (status == ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString())
                    {
                        try
                        {
                            new PlatformSdk.WeixinMP().SendTemplateMessage("BIYWu0LpzvnAdQOKkKO2I6Zt6SXgUtFqXKztkSbkGbU", new DomainBasic.UserApp().GetInfoByUserSn("20210504154936061-1809088913").attach4, "有补人申请需要审核", $"/Waixuan/ApproveApplication/ApproveApplicationList");
                        }
                        catch
                        {
                        }
                    }
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }

            #endregion 运营审批

            #region 运营数据分析

            public class YyApproveAnalyse
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        title = "申请日期",
                        placeholder = "申请日期",
                        mold = EmtTimeSelect.Mold.date_range,
                        defaultValue = DateTime.Today.ToString("yyyy-MM-01") + " ~ " + DateTime.Today.AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToString("yyyy-MM-dd")
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "200";
                    listDisplay.isTotalRow = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_user_sn_text")
                    {
                        text = "厅名",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_sum")
                    {
                        text = "申请数",
                        summaryReq = new Pagination.SummaryReq
                        {
                            fields = "zb_count",
                            summaryType = Pagination.SummaryType.SUM,
                            title = "总申请数"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("supplement_count_sum")
                    {
                        text = "已补数",
                        summaryReq = new Pagination.SummaryReq
                        {
                            fields = "supplement_count",
                            summaryType = Pagination.SummaryType.SUM,
                            title = "总已补数"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count_sum")
                    {
                        text = "流失数",
                        summaryReq = new Pagination.SummaryReq
                        {
                            fields = "quit_count",
                            summaryType = Pagination.SummaryType.SUM,
                            title = "总流失数"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_rate")
                    {
                        text = "流失率",
                    });

                    #endregion 显示列

                    #region 批量操作

                    //listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    //{
                    //    text = "批量审批",
                    //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    //    {
                    //        new EmtModel.ButtonItem("")
                    //        {
                    //            text = "批量审批",
                    //            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                    //            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                    //            {
                    //                func = ApproveAction,
                    //            },
                    //        },
                    //    }
                    //});

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"QuitList",
                            field_paras = "tg_user_sn"
                        },
                        text = "流失明细",
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                //public JsonResultAction ApproveAction(JsonRequestAction req)
                //{
                //    return new JsonResultAction();
                //}

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"yy_user_sn = '{new UserIdentityBag().user_sn}'";
                    var req = reqJson.GetPara();
                    if (!req["dateRange"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["dateRange"].ToString(), 0);
                        where += $" and create_time>='{dateRange.date_range_s}' and create_time < '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        fields = "tg_user_sn,sum(zb_count) as zb_count_sum,sum(supplement_count) as supplement_count_sum,sum(quit_count) as quit_count_sum",
                        where = where,
                        orderby = " create_time desc",
                        groupby = "group by tg_user_sn"
                    };
                    return new CtlListDisplay.ListData().getList<p_join_need, ItemDataModel>(filter, reqJson);
                }

                public class p_join_need : ModelDb.p_join_need
                {
                    public int zb_count_sum { get; set; }
                    public int supplement_count_sum { get; set; }
                    public int quit_count_sum { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : p_join_need
                {
                    public string tg_user_sn_text
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetUserByUserSn(tg_user_sn).username;
                        }
                    }

                    public string quit_rate
                    {
                        get
                        {
                            return supplement_count_sum + quit_count_sum > 0 ? (100 * quit_count_sum.ToDouble() / (quit_count_sum + supplement_count_sum).ToDouble()).ToString("0.00") + "%" : "0.00%";
                        }
                    }
                }

                #endregion ListData
            }

            /// <summary>
            /// 流失表格
            /// </summary>
            public class YyQuitList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("tg_user_sn")
                    {
                        title = "申请日期",
                        defaultValue = req.tg_user_sn
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "id",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("old_tg_username")
                    {
                        text = "流失前所属厅",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                    });

                    #endregion 显示列

                    #region 批量操作

                    //listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    //{
                    //    text = "批量审批",
                    //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    //    {
                    //        new EmtModel.ButtonItem("")
                    //        {
                    //            text = "批量审批",
                    //            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                    //            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                    //            {
                    //                func = ApproveAction,
                    //            },
                    //        },
                    //    }
                    //});

                    #endregion 批量操作

                    #region 操作列

                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = $"ZbDetails",
                    //        field_paras = "tg_user_sn"
                    //    },
                    //    text = "流失明细",
                    //});

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                //public JsonResultAction ApproveAction(JsonRequestAction req)
                //{
                //    return new JsonResultAction();
                //}

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int id { get; set; }
                    public string tg_user_sn { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"(zb_level = 'C' or zb_level = 'D')";
                    var req = reqJson.GetPara();
                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"].ToString()}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_info_zb, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zb
                {
                }

                #endregion ListData
            }

            #endregion 运营数据分析

            #region 补人申请数据汇总

            public class ApplySum
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
                    //listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    //{
                    //    placeholder = "审批状态",
                    //    options = new Dictionary<string, string>
                    //    {
                    //        {"未审批", ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString()},
                    //        {"已审批", ModelDb.p_join_need.status_enum.等待公会审批.ToInt().ToString()},
                    //    },
                    //    defaultValue = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString(),
                    //});

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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("c_date")
                    {
                        text = "统计日期",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("need_sum")
                    {
                        text = "提报总数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("male_sum")
                    {
                        text = "男性数量",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("female_sum")
                    {
                        text = "女性数量",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_sum")
                    {
                        text = "已补总数",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("male_finish_sum")
                    {
                        text = "男性已补数量",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("female_finish_sum")
                    {
                        text = "女性已补数量",
                    });

                    #endregion 显示列

                    #region 批量操作

                    //listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    //{
                    //    text = "批量审批",
                    //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    //    {
                    //        new EmtModel.ButtonItem("")
                    //        {
                    //            text = "批量审批",
                    //            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                    //            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                    //            {
                    //                func = ApproveAction,
                    //            },
                    //        },
                    //    }
                    //});

                    #endregion 批量操作

                    #region 操作列

                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = $"ZbDetails",
                    //        field_paras = "id"
                    //    },
                    //    text = "详情",
                    //    hideWith = new EmtModel.ListOperateItem.HideWith
                    //    {
                    //        compareType = EmtModel.ListOperateItem.CompareType.等于,
                    //        field = "status",
                    //        value = ModelDb.p_join_need.status_enum.等待运营审批.ToInt().ToString(),
                    //    }
                    //});

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                //public JsonResultAction ApproveAction(JsonRequestAction req)
                //{
                //    return new JsonResultAction();
                //}

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";

                    //3.获取当前运营下所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.need_count, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.need_count
                {
                }

                #endregion ListData
            }

            #endregion 补人申请数据汇总
        }
    }
}