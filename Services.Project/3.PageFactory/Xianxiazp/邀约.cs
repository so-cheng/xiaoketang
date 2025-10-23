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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 线下直聘
        /// </summary>
        public partial class Xianxiazp
        {
            #region 邀约列表
            /// <summary>
            /// 邀约列表
            /// </summary>
            public class YaoYueList
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
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.manager:
                            listFilter.formItems.Add(new ModelBasic.EmtSelect("wx_user_sn")
                            {
                                width = "120px",
                                placeholder = "外宣用户",
                                options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and organize_id in (select d2.id from sys_organize d1 left join sys_organize d2 on d1.id = d2.parent_id where d1.id = (select id from sys_organize where user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and name = '线下'))", "username,user_sn")
                            });
                            listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                            {
                                width = "120px",
                                placeholder = "中台地区",
                                options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = '{ModelEnum.UserTypeEnum.zter.ToSByte()}' and tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                            });
                            break;
                    }
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        width = "130px",
                        placeholder = "预约面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("interview_dateRange")
                    {
                        width = "130px",
                        placeholder = "实际面试日期",
                        disabled = true,
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "新增",
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.manager:
                            listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wx_name")
                            {
                                text = "外宣用户",
                                width = "100",
                                minWidth = "100"
                            });
                            break;
                    }
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date")
                    {
                        text = "邀约日期",
                        dateFormat = "yyyy-MM-dd",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_user_id")
                    {
                        text = "微信号",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "姓名",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("phone")
                    {
                        text = "联系电话",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("gender")
                    {
                        text = "性别",
                        width = "70",
                        minWidth = "70"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zt_name")
                    {
                        text = "中台地区",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zp_channel_txt")
                    {
                        text = "招聘渠道",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_interview_date")
                    {
                        text = "预约面试时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_date")
                    {
                        text = "实际面试日期",
                        dateFormat = "yyyy-MM-dd",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interviewer")
                    {
                        text = "面试人",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_result_txt")
                    {
                        text = "面试结果",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_ruzhi_date")
                    {
                        text = "预计入职时间",
                        dateFormat = "yyyy-MM-dd",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ruzhi_date")
                    {
                        text = "入职时间",
                        dateFormat = "yyyy-MM-dd",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "入职厅名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interviewer_feedback")
                    {
                        text = "面试官意见",
                        width = "120",
                        minWidth = "120"
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
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "layui-btn-normal",
                        text = "撤销",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = CancelAction,
                            field_paras = "id",
                        },
                        disabled = true,
                        name = "Cancel"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "JianLi",
                            field_paras = "id=jianli_id"
                        },
                        disabled = true,
                        text = "简历",
                        name = "JianLi"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public FilterForm filterForm { get; set; } = new FilterForm();

                    /// <summary>
                    /// 筛选参数（自定义）
                    /// </summary>
                    public class FilterForm
                    {

                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 撤销简历登记操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction CancelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_xianxiazp_info = req.GetPara<ModelDb.p_xianxiazp_info>();
                    p_xianxiazp_info.jianli_id = 0;
                    p_xianxiazp_info.Update();
                    return info;
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

                    if (!reqJson.GetPara("wx_user_sn").IsNullOrEmpty()) where += $" and p_xianxiazp_info.wx_user_sn = '{reqJson.GetPara("wx_user_sn")}'";
                    if (!reqJson.GetPara("zt_user_sn").IsNullOrEmpty()) where += $" and zt_user_sn = '{reqJson.GetPara("zt_user_sn")}'";
                    if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange"), 0);
                        where += $" and yy_interview_date >= '{dateRange.date_range_s}' and yy_interview_date <= '{dateRange.date_range_e}'";
                    }
                    if (!reqJson.GetPara("interview_dateRange").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("interview_dateRange"), 0);
                        where += $" and interview_date >= '{dateRange.date_range_s}' and interview_date <= '{dateRange.date_range_e}'";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        fields = "p_xianxiazp_info.*,p_xianxiazp_jianli.username,p_xianxiazp_jianli.phone",
                        on = "p_xianxiazp_info.jianli_id = p_xianxiazp_jianli.id",
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_xianxiazp_info, ModelDb.p_xianxiazp_jianli, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_xianxiazp_info
                {
                    public string username { get; set; }
                    public string phone { get; set; }
                    public string wx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetUserByUserSn(wx_user_sn).name;
                        }
                    }
                    public string zt_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(zt_user_sn).name;
                        }
                    }
                    public string zp_channel_txt
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue("招聘渠道", zp_channel.ToString());
                        }
                    }
                    public string interview_result_txt
                    {
                        get
                        {
                            return ((interview_result_enum)interview_result).ToString();
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 邀约登记
            /// <summary>
            /// 邀约创建/编辑页面
            /// </summary>
            public class YaoYuePost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口,
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
                    var p_xianxiazp_info = DoMySql.FindEntityById<ModelDb.p_xianxiazp_info>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_xianxiazp_info.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "邀约日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.id == 0 ? DateTime.Now.ToString("yyyy-MM-dd") : p_xianxiazp_info.c_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "微信名",
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.wechat_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_user_id")
                    {
                        title = "微信号",
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.wechat_user_id,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("gender")
                    {
                        title = "性别",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            { "男","男" },
                            { "女","女"},
                        },
                        defaultValue = p_xianxiazp_info.gender,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                    {
                        title = "中台地区",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("zter").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}", "name,user_sn"),
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.zt_user_sn,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zp_channel")
                    {
                        title = "招聘渠道",
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.招聘渠道),
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.zp_channel.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("yy_interview_date")
                    {
                        title = "预约面试时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        isRequired = true,
                        defaultValue = p_xianxiazp_info.yy_interview_date.ToString(),
                        colLength = 6,
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; } = 0;
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_xianxiazp_info = req.data_json.ToModel<ModelDb.p_xianxiazp_info>();
                    if (p_xianxiazp_info.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择邀约日期");
                    if (p_xianxiazp_info.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请输入微信名");
                    if (p_xianxiazp_info.wechat_user_id.IsNullOrEmpty()) throw new WeicodeException("请输入微信号");
                    if (p_xianxiazp_info.gender.IsNullOrEmpty()) throw new WeicodeException("请选择性别");
                    if (p_xianxiazp_info.zt_user_sn.IsNullOrEmpty()) throw new WeicodeException("请选择中台地区");
                    if (p_xianxiazp_info.zp_channel.IsNullOrEmpty()) throw new WeicodeException("请选择招聘渠道");
                    if (p_xianxiazp_info.yy_interview_date.IsNullOrEmpty()) throw new WeicodeException("请选择预约面试时间");

                    if (p_xianxiazp_info.id == 0)
                    {
                        p_xianxiazp_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_xianxiazp_info.wx_user_sn = new UserIdentityBag().user_sn;
                    }

                    p_xianxiazp_info.InsertOrUpdate();
                    return result;
                }
                #endregion
            }
            #endregion

            #region 编辑面试信息（外宣）
            /// <summary>
            /// 编辑面试信息（外宣）
            /// </summary>
            public class WX_Edit
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口,
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
                    var p_xianxiazp_info = DoMySql.FindEntityById<ModelDb.p_xianxiazp_info>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("interview_date")
                    {
                        title = "实际面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_xianxiazp_info.interview_date.IsNullOrEmpty() ? DateTime.Now.ToString("yyyy-MM-dd") : p_xianxiazp_info.interview_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 12,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; } = 0;
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_xianxiazp_info = req.data_json.ToModel<ModelDb.p_xianxiazp_info>();
                    if (p_xianxiazp_info.interview_date.IsNullOrEmpty()) throw new WeicodeException("请选择实际面试日期");

                    p_xianxiazp_info.Update();

                    return result;
                }
                #endregion
            }
            #endregion

            #region 编辑面试信息（中台）
            /// <summary>
            /// 编辑面试信息（中台）
            /// </summary>
            public class ZT_Edit
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口,
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
                    var p_xianxiazp_info = DoMySql.FindEntityById<ModelDb.p_xianxiazp_info>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("interview_result")
                    {
                        title = "面试结果",
                        isRequired = true,
                        options = new Dictionary<string, string> {
                            { "不通过", "1" },
                            { "通过", "2" }
                        },
                        defaultValue = p_xianxiazp_info.interview_result.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("yy_ruzhi_date")
                    {
                        title = "预计入职日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_xianxiazp_info.yy_ruzhi_date.IsNullOrEmpty() ? "" : p_xianxiazp_info.yy_ruzhi_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("ruzhi_date")
                    {
                        title = "入职日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_xianxiazp_info.ruzhi_date.IsNullOrEmpty() ? "" : p_xianxiazp_info.ruzhi_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        title = "入职厅",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()),
                        defaultValue = p_xianxiazp_info.ting_sn,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTextarea("interviewer_feedback")
                    {
                        mode = ModelBasic.EmtTextarea.Mode.TextArea,
                        title = "面试意见",
                        defaultValue = p_xianxiazp_info.interviewer_feedback,
                        colLength = 12,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; } = 0;
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_xianxiazp_info = req.data_json.ToModel<ModelDb.p_xianxiazp_info>();
                    if (p_xianxiazp_info.interview_result.IsNullOrEmpty()) throw new WeicodeException("请选择面试结果");

                    p_xianxiazp_info.Update();

                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}