using Newtonsoft.Json.Linq;
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
    /// <summary>
    /// 学员资料模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            #region 申请单对应已补学员名单
            public class Stu_ZbList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("para_status")
                    {
                        defaultValue = req.para_status,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "140px",
                        placeholder = "抖音账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("month")
                    {
                        width = "100px",
                        mold = EmtTimeSelect.Mold.month,
                        placeholder = "月份",
                        disabled = true,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"正常",ModelDb.p_join_new_info.status_enum.补人完成.ToInt().ToString()},
                            {"流失",ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt().ToString()}
                        },
                        defaultValue = ModelDb.p_join_new_info.status_enum.补人完成.ToInt().ToString(),
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
                    listDisplay.operateWidth = "170";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        attachPara = new Dictionary<string, object>
                        {
                            { "tg_need_id",req.tg_need_id }
                        }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("mx_name")
                    {
                        text = "萌新",
                        width = "100",
                        minWidth = "100",
                    });
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
                        width = "60",
                        minWidth = "60",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140",
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("sessions_text")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220",
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
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_txt")
                    {
                        text = "当前状态",
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
                            url = "CausePost",
                            field_paras = "id"
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareMode = EmtModel.ListOperateItem.CompareMode.JS函数判断,
                            compareModeFunc = new EmtModel.ListOperateItem.CompareModeFunc
                            {
                                jsCode = $"d.status == {ModelDb.p_join_new_info.status_enum.补人完成.ToInt()} || d.status == {ModelDb.p_join_new_info.status_enum.等待退回.ToInt()}"
                            }
                        },
                        text = "流失",
                        name = "CausePost",
                        disabled = true //厅管端开启 管理端禁止
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Share/Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

                    return listDisplay;
                }

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

                    /// <summary>
                    /// 申请档表id
                    /// </summary>
                    public int apply_item_id { get; set; }

                    /// <summary>
                    /// 筛选状态
                    /// </summary>
                    public string para_status { get; set; }

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
                    var dou_username = reqJson.GetPara("dou_username");
                    if (!dou_username.IsNullOrEmpty()) where += $" and dou_username like '%{dou_username}%'";
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
                        if (reqJson.GetPara("status").Equals(ModelDb.p_join_new_info.status_enum.补人完成.ToInt().ToString()))// 在职状态:正常
                        {
                            if (!reqJson.GetPara("para_status").IsNullOrEmpty())
                            {
                                if (reqJson.GetPara("para_status").ToInt().Equals(-1))
                                {
                                    where += $" and status not in ({ModelDb.p_join_new_info.status_enum.等待入库.ToInt()},{ModelDb.p_join_new_info.status_enum.等待拉群.ToInt()},{ModelDb.p_join_new_info.status_enum.等待培训.ToInt()},{ModelDb.p_join_new_info.status_enum.补人完成.ToInt()},{ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()})";
                                }
                                else
                                {
                                    where += $" and status = {reqJson.GetPara("para_status").ToInt()}";
                                }
                            }
                            else
                            {
                                where += $" and status!='{ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()}'";
                            }
                        }
                        else
                        {
                            where += $" and status='{ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()}'";
                        }

                    }
                    //3.根据id获取主播名单
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "id desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string mx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).username;
                        }
                    }
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
                            return province + city;
                        }
                    }

                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }

                    public string status_txt
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

            #region 学员背调信息修改
            public class Stu_UserBatchPost
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
                    var p_join_new_info = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_new_info>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "基本信息",
                        width = "200",
                    });
                    formDisplay.formItems.Add(new EmtInput("wechat_nickname")
                    {
                        title = "微信昵称",
                        colLength = 6,
                        defaultValue = p_join_new_info.wechat_nickname,
                    });
                    formDisplay.formItems.Add(new EmtInput("dou_username")
                    {
                        title = "抖音账号",
                        colLength = 6,
                        defaultValue = p_join_new_info.dou_username,
                    });
                    formDisplay.formItems.Add(new EmtSelect("full_or_part")
                    {
                        title = "兼职/全职",
                        colLength = 6,
                        defaultValue = p_join_new_info.full_or_part,
                        options = new Dictionary<string, string>
                    {
                        {"兼职","兼职" },
                        {"全职","全职" },
                    }
                    });
                    formDisplay.formItems.Add(new EmtSelect("zb_sex")
                    {
                        title = "主播性别",
                        colLength = 6,
                        defaultValue = p_join_new_info.zb_sex,
                        options = new Dictionary<string, string>
                    {
                        {"男","男" },
                        {"女","女" },
                    }
                    });
                    formDisplay.formItems.Add(new ModelSite.EmtCityPicker("address")
                    {
                        title = "地区(省市)",
                        colLength = 12,
                        defaultValue = p_join_new_info.province + ',' + p_join_new_info.city
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessions")
                    {
                        title = "接档时间",
                        colLength = 6,
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        defaultValue = p_join_new_info.sessions
                    });
                    formDisplay.formItems.Add(new EmtInput("qun")
                    {
                        title = "对接群",
                        colLength = 6,
                        defaultValue = p_join_new_info.qun,
                    });

                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public int id { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    //1.数据校验
                    //2.修改
                    var lSql = new List<string>();
                    p_join_new_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_join_new_info.province = p_join_new_info.address.Split(',')[0];
                    p_join_new_info.city = p_join_new_info.address.Split(',')[1];
                    lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion

            #region 学员退回操作
            public class Stu_BackPost
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
                    /// 主播id
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
                        throw new WeicodeException("请选择退回理由");
                    }
                    foreach (var id in id_array)
                    {
                        var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(id.ToInt());
                        p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待退回.ToSByte();

                        string note = req.GetPara("note_select");
                        if (!req.GetPara("note").IsNullOrEmpty())
                        {
                            note = req.GetPara("note");
                        }
                        p_join_new_info.note = $"退回操作人:{new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).name}" + ";原因:" + note;

                        lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    foreach (var id in id_array)
                    {
                        var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(id.ToInt());

                        // 计算申请档位明细人数
                        new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                        // 添加日志
                        new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.退回, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待培训, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播退回操作");
                    }

                    return new JsonResultAction();
                }
                #endregion 异步请求处理
            }
            #endregion

            #region 学员流失操作
            public class Stu_CausePost
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
                    /// 主播id
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
                    var lSql = new List<string>();
                    string date = req.GetPara()["date"].ToString();
                    if (date.IsNullOrEmpty()) { throw new Exception("请填写流失时间"); }
                    if (req.GetPara("cause").IsNullOrEmpty()) { throw new Exception("请填写流失原因"); }
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());

                    p_join_new_info.no_share = $"流失时间:{date},原因:{req.GetPara("cause")}";
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte();

                    lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));

                    DoMySql.ExecuteSqlTran(lSql);
                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    // 添加日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.流失, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待分级, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播流失操作");
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 学员列表
            /// <summary>
            /// 萌新端用户分级列表展示、外宣管理端等待入库已经流失列表展示
            /// </summary>
            public class Stu_MxList
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
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

                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "180px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };
                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"term", "term" },
                            },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                                {
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=6,
                                        emtModelBase = new ModelBasic.EmtInput($"term_text")
                                        {
                                            width = "120"
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=3,
                                        emtModelBase = new ModelBasic.EmtButton("submit_term")
                                        {
                                            defaultValue = "提交",
                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                                {
                                                    attachPara=new Dictionary<string, object>
                                                    {
                                                        {"id","<%=page.focus.attr('data-id')%>" },
                                                        {"name","term" },
                                                        {"value","<%=page.term_text.value%>" }
                                                    },
                                                    resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                    func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                                }
                                            }
                                        }
                                    },
                                    new ModelBasic.EmtGrid.Item
                                    {
                                        colLength=3,
                                        emtModelBase = new ModelBasic.EmtButton("cancel_term")
                                        {
                                            defaultValue = "取消",

                                            eventJsChange = new EmtFormBase.EventJsChange
                                            {
                                                eventJavascript=new EventJavascript
                                                {
                                                    code="$('.floatlayer_div').hide();"
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"zb_level", "zb_level" },
                            },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_status")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"暂不分配",ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString()},
                                            {"等待分配",ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString()},
                                        },
                                        defaultValue = ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString(),
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"l_status","<%=page.l_status.value%>" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs=@"if(page.l_status.value == "+ModelDb.p_join_new_info.status_enum.等待分配.ToSByte()                          +@"){
                                                                page.focus.text(page.l_zb_level.value);
                                                                $('.floatlayer_div').hide();
                                                            }
                                                            else{
                                                                location.reload();
                                                            }",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
            }
        }
    }
},
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength = 5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
                                    {
                                        defaultValue = "取消",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript = new EventJavascript
                                            {
                                                code = "$('.floatlayer_div').hide();"
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength = 2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content = $@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_sn")
                    {
                        index = 1300,
                        text = "萌新sn",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        index = 1500,
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        index = 1400,
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        index = 1600,
                        text = "流失原因",
                        width = "300",
                        minWidth = "300"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "180"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Newcomer/Edit",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Edit",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion
                    #region 批量操作列

                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("level")
                    {
                        text = "批量分级",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "FastLevel",
                        },
                        disabled = true,
                    });

                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("fast")
                    {
                        text = "加急处理",
                        disabled = true,
                        buttonItems = new List<EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("cancelfast")
                        {
                            text="批量加急",
                            mode= EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction=new EmtModel.ButtonItem.EventCsAction
                            {
                                func= FastSupplementAction,
                            },
                        },
                        new ModelBasic.EmtModel.ButtonItem("cancelfast")
                        {
                            text="取消加急",
                            mode= EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction=new EmtModel.ButtonItem.EventCsAction
                            {
                                func= CancelFastSupplementAction,
                            },
                        }
                    }
                    });
                    #endregion

                    return listDisplay;
                }
                #region 请求参数对象
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待分级, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    p_join_new_info.Delete();
                    return info;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                #endregion
                #region 批量加急操作
                /// <summary>
                /// 批量加急回调
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction FastSupplementAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var lSql = new List<string>();
                    var p_join_new_info = DoMySql.FindList<ModelDb.p_join_new_info>($"id in ({req.GetPara("ids")})");
                    var NewTerm = DoMySql.FindEntity<ModelDb.p_mengxin>($"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' order by create_time desc");
                    if (p_join_new_info.Count > 0)
                    {
                        foreach (var item in p_join_new_info)
                        {
                            if (item.term == NewTerm.term)
                            {
                                throw new Exception($"最新一期的学员禁止加急:微信昵称{item.wechat_nickname}");
                            }
                            item.is_fast = ModelDb.p_join_new_info.is_fast_enum.加急.ToSByte();
                            lSql.Add(item.UpdateTran());
                        }
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);

                    lSql.Clear();
                    foreach (var item in p_join_new_info)
                    {
                        lSql.Add(new ServiceFactory.Join.MengxinSortService().SetFastZbSortForEntity(item.id).UpdateTran());
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);

                    return result;
                }

                /// <summary>
                /// 取消批量加急回调
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction CancelFastSupplementAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var lSql = new List<string>();
                    var p_join_new_info = DoMySql.FindList<ModelDb.p_join_new_info>($"id in ({req.GetPara("ids")})");
                    if (p_join_new_info.Count > 0)
                    {
                        foreach (var item in p_join_new_info)
                        {
                            if (item.is_fast == ModelDb.p_join_new_info.is_fast_enum.加急.ToSByte())
                            {
                                item.is_fast = ModelDb.p_join_new_info.is_fast_enum.不加急.ToSByte();
                                lSql.Add(item.UpdateTran());
                            }
                        }
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);
                    return result;
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("mx_sn").IsNullOrEmpty())
                    {
                        where += $" and mx_sn = '{reqJson.GetPara("mx_sn")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 学员基础信息编辑
            public class Stu_MxPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.style = @"background-image:url('/Assets/images/qgxkt_m.jpg');background-size: cover;background-position: center; background-repeat: no-repeat;margin:5px;";
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    if (!UtilityStatic.ClientHelper.IsMobileRequest())
                    {
                        pageModel.postedReturn = new PagePost.PostedReturn
                        {
                            returnType = PagePost.PostedReturn.ReturnType.关闭所属浮动层
                        };
                    }

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
                    var p_join_new_info = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_new_info>($"id = '{req.id}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = p_join_new_info.id.ToString(),

                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("openid")
                    {
                        title = "openid",
                        defaultValue = req.openid.IsNullOrEmpty() ? p_join_new_info.openid : req.openid,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("mx_sn")
                    {
                        title = "萌新sn",
                        isRequired = true,
                        defaultValue = p_join_new_info.mx_sn.IsNullOrEmpty() ? req.mx_sn : p_join_new_info.mx_sn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        title = "期数",
                        isRequired = true,
                        colLength = 10,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        displayStatus = EmtModelBase.DisplayStatus.隐藏,
                        defaultValue = p_join_new_info.term,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        title = "主播定级",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            {"A","A" },
                            {"B","B" },
                        },
                        displayStatus = EmtModelBase.DisplayStatus.隐藏,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = p_join_new_info.zb_level,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        title = "01 微信昵称",
                        colLength = 10,
                        isRequired = true,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = p_join_new_info.wechat_nickname,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "02 微信账号",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = p_join_new_info.wechat_username,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（有我好友的这个微信，不然找不到你）"
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        title = "03 抖音账号",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = p_join_new_info.dou_username,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（填写自己与公会签约的抖音号）",

                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "dou_username","<%=page_post.dou_username.value%>"}
                                },
                                func = getDouNickName,
                                resCallJs = $"page_post.dou_nickname.set(res.data.nickname);page_post.anchor_id.set(res.data.anchor_id)"
                            }
                        }
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        title = "04 抖音昵称",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = p_join_new_info.dou_nickname,
                        style = "background-color: transparent;border:1px solid #cccccc",
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("anchor_id")
                    {
                        title = "抖音作者id",
                        defaultValue = p_join_new_info.anchor_id,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("zb_sex")
                    {
                        title = "05 性别",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                        {"男","男" },
                        {"女","女" },
                    },
                        defaultValue = p_join_new_info.zb_sex,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("age")
                    {
                        title = "06 年龄",
                        isRequired = true,
                        defaultValue = p_join_new_info.age.ToString(),
                        style = "background-color: transparent;border:1px solid #cccccc",
                        placeholder = "（真实年龄）"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("job")
                    {
                        title = "07 现实工作",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                        {"宝妈","宝妈"},
                        {"上班族","上班族"},
                        {"学生党","学生党"},
                        {"自由职业者","自由职业者"},
                        {"其他","其他"}
                    },
                        defaultValue = p_join_new_info.job,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelSite.EmtCityPicker("address")
                    {
                        title = "08 所在省市",
                        colLength = 10,
                        isRequired = true,
                        defaultValue = p_join_new_info.address.IsNullOrEmpty() ? "" : p_join_new_info.province + ',' + p_join_new_info.city,
                        style = "background-color: transparent;border:1px solid #cccccc",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessions")
                    {
                        title = "09 [多选]接档时间(尽量不要选21-24，分厅比较慢)",
                        isRequired = true,
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = p_join_new_info.sessions
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("full_or_part")
                    {
                        title = "10 兼职/全职",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                            {"兼职","兼职" },
                            {"全职","全职" },
                    },
                        style = "background-color: transparent;border:1px solid #cccccc",
                        defaultValue = p_join_new_info.full_or_part,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public int id { get; set; }
                    public string mx_sn { get; set; }
                    public string openid { get; set; }
                }
                public class ResultData
                {
                    public string anchor_id { get; set; }
                    public string nickname { get; set; }
                }

                #region 异步处理
                /// <summary>
                /// 根据抖音账号获取抖音昵称和作者id
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction getDouNickName(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();

                    //调用抖音接口根据抖音账号查询抖音昵称及抖音作者id（抖音官方主播唯一身份id）
                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = req["dou_username"].ToNullableString()
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("请输入正确的抖音账号");
                    }
                    var obj = JObject.Parse(dyCheckResult["data"].ToNullableString());

                    var resultData = new ResultData
                    {
                        anchor_id = obj["anchor_id"].ToNullableString(),
                        nickname = obj["nickname"].ToNullableString()
                    };

                    result.data = resultData;
                    return result;
                }

                #endregion
                #endregion
            }
            #endregion

            #region 萌新-查询归属
            public class Stu_SearchList
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("mx_sn")
                    {
                        width = "120px",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id}' and status='{ModelDb.user_base.status_enum.正常}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        placeholder = "萌新老师"
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "状态",
                        width = "100px",
                        options = new Dictionary<string, string>
                        {
                            {"等待分级",ModelDb.p_join_new_info.status_enum.等待分级.ToInt().ToString()},
                            {"等待分配",ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString()},
                            {"等待入库",ModelDb.p_join_new_info.status_enum.等待入库.ToInt().ToString()},
                            {"等待拉群",ModelDb.p_join_new_info.status_enum.等待拉群.ToInt().ToString()},
                            {"补人完成",ModelDb.p_join_new_info.status_enum.补人完成.ToInt().ToString()},
                            {"逻辑删除",ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt().ToString()},
                        },
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("job")
                    {
                        width = "120px",
                        placeholder = "职业",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "100px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
                    });
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

                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.isHideOperate = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 300,
                        text = "期数",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 300,
                        text = "主播分级",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_sn")
                    {
                        index = 1300,
                        text = "萌新sn",
                        width = "140",
                        minWidth = "140"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_username")
                    {
                        index = 1400,
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        index = 1500,
                        text = "对接厅管",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        index = 1600,
                        text = "流失原因",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "180",
                        minWidth = "180"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("mx_sn").IsNullOrEmpty())
                    {
                        where += $" and mx_sn = '{reqJson.GetPara("mx_sn")}'";
                    }
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty())
                    {
                        where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status = '{reqJson.GetPara("status")}'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
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
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 学员列表（纯展示）
            /// <summary>
            /// 学员列表（纯展示）
            /// </summary>
            public class Stu_List
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
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

                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.isHideOperate = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };
                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_name")
                    {
                        index = 100,
                        text = "萌新老师",
                        width = "100",
                        minWidth = "100",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        text = "期数",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level")
                    {
                        index = 200,
                        text = "分级",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level_time_text")
                    {
                        index = 200,
                        text = "分级时间",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        index = 1500,
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        index = 1400,
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        index = 1600,
                        text = "流失原因",
                        width = "300",
                        minWidth = "300"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion

                    return listDisplay;
                }
                #region 请求参数对象
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " and create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string mx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).username;
                        }
                    }
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string zb_level_time_text
                    {
                        get
                        {
                            if (zb_level_time.IsNullOrEmpty()) return "";
                            var difference = (DateTime.Now - zb_level_time.ToDateTime()).Days;
                            if (difference == 0) return "今天";
                            if (difference == 1) return "昨天";
                            return $"{difference}天前";
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}
