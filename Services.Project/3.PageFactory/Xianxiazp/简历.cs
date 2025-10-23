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
    /// 线下直聘
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 简历
        /// </summary>
        public partial class Xianxiazp
        {
            #region 简历登记
            /// <summary>
            /// 简历登记
            /// </summary>
            public class ResumePost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
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
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("wx_user_sn")
                    {
                        defaultValue = req.wx_user_sn,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        isRequired = true,
                        defaultValue = DateTime.Now.ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "姓名",
                        isRequired = true,
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
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("phone")
                    {
                        isRequired = true,
                        title = "联系电话",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "微信名",
                        isRequired = true,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_user_id")
                    {
                        title = "微信号",
                        isRequired = true,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("birthday")
                    {
                        title = "出生年月",
                        mold = ModelBasic.EmtTimeSelect.Mold.month,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("marriage")
                    {
                        title = "婚姻",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("education")
                    {
                        title = "学历",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                    {
                        title = "现住址",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("label")
                    {
                        title = "家庭成员",
                        colLength = 12,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_username")
                    {
                        title = "姓名",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_relationship")
                    {
                        title = "关系",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_age")
                    {
                        title = "年龄",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_job_company")
                    {
                        title = "单位名称",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_job")
                    {
                        title = "职务",
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_phone")
                    {
                        title = "联系方式",
                        colLength = 6,
                    });

                    #region 可选字段
                    var p_xianxiazp_filed = DoMySql.FindEntity<ModelDb.p_xianxiazp_filed>($"zt_user_sn = '{1}'");
                    if (p_xianxiazp_filed.fileds.IndexOf(",政治面貌,") >= 0)
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                        {
                            title = "政治面貌",
                            colLength = 12
                        });
                    }

                    if (p_xianxiazp_filed.fileds.IndexOf(",家庭成员数,") >= 0)
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                        {
                            title = "家庭成员数",
                            colLength = 12
                        });
                    }

                    if (p_xianxiazp_filed.fileds.IndexOf(",婚姻状况,") >= 0)
                    {
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                        {
                            title = "婚姻状况",
                            colLength = 12
                        });
                    }
                    #endregion
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public string wx_user_sn { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_xianxiazp_jianli = req.data_json.ToModel<ModelDb.p_xianxiazp_jianli>();
                    if (p_xianxiazp_jianli.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择面试日期");
                    if (p_xianxiazp_jianli.username.IsNullOrEmpty()) throw new WeicodeException("请输入姓名");
                    if (p_xianxiazp_jianli.gender.IsNullOrEmpty()) throw new WeicodeException("请选择性别");
                    if (p_xianxiazp_jianli.phone.IsNullOrEmpty()) throw new WeicodeException("请输入联系电话");
                    if (p_xianxiazp_jianli.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请输入微信名");
                    if (p_xianxiazp_jianli.wechat_user_id.IsNullOrEmpty()) throw new WeicodeException("请输入微信号");

                    p_xianxiazp_jianli.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    p_xianxiazp_jianli.Insert();
                    return result;
                }
                #endregion
            }
            #endregion

            #region 简历列表
            /// <summary>
            /// 简历列表
            /// </summary>
            public class ResumeList
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        placeholder = "预约面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        defaultValue = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd")
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
                    var listDisplay = new ModelBasic.CtlListDisplay(req);
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("id")
                    {
                        text = "id",
                        width = "120",
                        minWidth = "120",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date")
                    {
                        text = "面试日期",
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
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "姓名",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("gender")
                    {
                        text = "性别",
                        width = "70",
                        minWidth = "70"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("birthday")
                    {
                        text = "出生年月",
                        dateFormat = "yyyy-MM",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("phone")
                    {
                        text = "联系方式",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("marriage")
                    {
                        text = "婚姻状况",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("education")
                    {
                        text = "学历",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address")
                    {
                        text = "现住址",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_username")
                    {
                        text = "家庭成员姓名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_relationship")
                    {
                        text = "与家庭成员关系",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_age")
                    {
                        text = "家庭成员年龄",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_job_company")
                    {
                        text = "家庭成员单位名称",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_job")
                    {
                        text = "家庭成员职务",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("family_phone")
                    {
                        text = "家庭成员联系方式",
                        width = "150",
                        minWidth = "150"
                    });

                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CheckIn",
                            field_paras = "jianli_id=id"
                        },
                        style = "layui-btn-normal",
                        text = "登记",
                        name = "CheckIn"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Edit",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Edit"
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

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"1=1";

                    var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();
                    if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange"), 0);
                        where += $" and c_date >= '{dateRange.date_range_s}' and c_date <= '{dateRange.date_range_e}'";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_xianxiazp_jianli, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string create_time { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_xianxiazp_jianli
                {
                }
                #endregion
            }
            #endregion

            #region 简历编辑
            /// <summary>
            /// 简历编辑
            /// </summary>
            public class ResumeEdit
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
                    var p_xianxiazp_jianli = DoMySql.FindEntityById<ModelDb.p_xianxiazp_jianli>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        isRequired = true,
                        defaultValue = p_xianxiazp_jianli.c_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "姓名",
                        isRequired = true,
                        defaultValue = p_xianxiazp_jianli.username,
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
                        defaultValue = p_xianxiazp_jianli.gender,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("phone")
                    {
                        title = "联系电话",
                        isRequired = true,
                        defaultValue = p_xianxiazp_jianli.phone,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        title = "微信名",
                        isRequired = true,
                        defaultValue = p_xianxiazp_jianli.wechat_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("wechat_user_id")
                    {
                        title = "微信号",
                        isRequired = true,
                        defaultValue = p_xianxiazp_jianli.wechat_user_id,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("birthday")
                    {
                        title = "出生年月",
                        mold = ModelBasic.EmtTimeSelect.Mold.month,
                        defaultValue = p_xianxiazp_jianli.birthday.ToDate().ToString("yyyy_MM"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("marriage")
                    {
                        title = "婚姻",
                        defaultValue = p_xianxiazp_jianli.marriage,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("education")
                    {
                        title = "学历",
                        defaultValue = p_xianxiazp_jianli.education,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                    {
                        title = "现住址",
                        defaultValue = p_xianxiazp_jianli.address,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("label")
                    {
                        title = "家庭成员",
                        colLength = 12,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_username")
                    {
                        title = "姓名",
                        defaultValue = p_xianxiazp_jianli.family_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_relationship")
                    {
                        title = "关系",
                        defaultValue = p_xianxiazp_jianli.family_relationship,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_age")
                    {
                        title = "年龄",
                        defaultValue = p_xianxiazp_jianli.family_age,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_job_company")
                    {
                        title = "单位名称",
                        defaultValue = p_xianxiazp_jianli.family_job_company,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_job")
                    {
                        title = "职务",
                        defaultValue = p_xianxiazp_jianli.family_job,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("family_phone")
                    {
                        title = "联系方式",
                        defaultValue = p_xianxiazp_jianli.family_phone,
                        colLength = 6,
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
                    var p_xianxiazp_jianli = req.data_json.ToModel<ModelDb.p_xianxiazp_jianli>();
                    if (p_xianxiazp_jianli.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择面试日期");
                    if (p_xianxiazp_jianli.username.IsNullOrEmpty()) throw new WeicodeException("请输入姓名");
                    if (p_xianxiazp_jianli.gender.IsNullOrEmpty()) throw new WeicodeException("请选择性别");
                    if (p_xianxiazp_jianli.phone.IsNullOrEmpty()) throw new WeicodeException("请输入联系电话");
                    if (p_xianxiazp_jianli.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请输入微信名");
                    if (p_xianxiazp_jianli.wechat_user_id.IsNullOrEmpty()) throw new WeicodeException("请输入微信号");

                    p_xianxiazp_jianli.Update();
                    return result;
                }
                #endregion
            }
            #endregion

            #region 登记（登记表与简历关联）
            /// <summary>
            /// 登记表与简历关联
            /// </summary>
            public class ResumeCheckInPost
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
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("jianli_id")
                    {
                        defaultValue = req.jianli_id.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("id")
                    {
                        title = "选择登记表",
                        options = DoMySql.FindKvList<ModelDb.p_xianxiazp_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and jianli_id = 0", "wechat_username,id"),
                        colLength = 12,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("interview_date")
                    {
                        title = "实际面试日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = DateTime.Today.ToString("yyyy-MM-dd"),
                        colLength = 12,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    /// <summary>
                    /// 简历id
                    /// </summary>
                    public int jianli_id { get; set; } = 0;
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_xianxiazp_info = req.data_json.ToModel<ModelDb.p_xianxiazp_info>();
                    if (p_xianxiazp_info.interview_date.IsNullOrEmpty()) throw new WeicodeException("请选择实际面试日期");

                    p_xianxiazp_info.Update($"id = {p_xianxiazp_info.id}");

                    return result;
                }
                #endregion
            }
            #endregion

            #region 简历查看
            /// <summary>
            /// 简历查看
            /// </summary>
            public class ResumeView
            {
                #region DefaultView
                public ModelBasic.PageDetail Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageDetail("post");
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
                    var p_xianxiazp_jianli = DoMySql.FindEntityById<ModelDb.p_xianxiazp_jianli>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "面试日期",
                        defaultValue = p_xianxiazp_jianli.c_date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("username")
                    {
                        title = "姓名",
                        defaultValue = p_xianxiazp_jianli.username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("gender")
                    {
                        title = "性别",
                        defaultValue = p_xianxiazp_jianli.gender,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("phone")
                    {
                        title = "联系电话",
                        defaultValue = p_xianxiazp_jianli.phone,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("wechat_username")
                    {
                        title = "微信名",
                        defaultValue = p_xianxiazp_jianli.wechat_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("wechat_user_id")
                    {
                        title = "微信号",
                        defaultValue = p_xianxiazp_jianli.wechat_user_id,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("birthday")
                    {
                        title = "出生年月",
                        defaultValue = p_xianxiazp_jianli.birthday.IsNullOrEmpty() ? "" : p_xianxiazp_jianli.birthday.ToDate().ToString("yyyy_MM"),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("marriage")
                    {
                        title = "婚姻",
                        defaultValue = p_xianxiazp_jianli.marriage,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("education")
                    {
                        title = "学历",
                        defaultValue = p_xianxiazp_jianli.education,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("address")
                    {
                        title = "现住址",
                        defaultValue = p_xianxiazp_jianli.address,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("label")
                    {
                        title = "家庭成员",
                        colLength = 12,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_username")
                    {
                        title = "姓名",
                        defaultValue = p_xianxiazp_jianli.family_username,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_relationship")
                    {
                        title = "关系",
                        defaultValue = p_xianxiazp_jianli.family_relationship,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_age")
                    {
                        title = "年龄",
                        defaultValue = p_xianxiazp_jianli.family_age,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_job_company")
                    {
                        title = "单位名称",
                        defaultValue = p_xianxiazp_jianli.family_job_company,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_job")
                    {
                        title = "职务",
                        defaultValue = p_xianxiazp_jianli.family_job,
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("family_phone")
                    {
                        title = "联系方式",
                        defaultValue = p_xianxiazp_jianli.family_phone,
                        colLength = 6,
                    });
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

            #region 基地-简历字段配置
            /// <summary>
            /// 简历字段配置
            /// </summary>
            public class FiledConfig
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新当前窗口,
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
                    var p_xianxiazp_filed = DoMySql.FindEntity<ModelDb.p_xianxiazp_filed>($"zt_user_sn = '{new UserIdentityBag().user_sn}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtCheckbox("fileds")
                    {
                        title = "可选字段",
                        options = new Dictionary<string, string>
                        {
                            {"政治面貌", "政治面貌" },
                            {"家庭成员数", "家庭成员数" },
                            {"婚姻状况", "婚姻状况" }
                        },
                        defaultValue = p_xianxiazp_filed.fileds
                    });
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
                    var result = new JsonResultAction();
                    var p_xianxiazp_filed = req.data_json.ToModel<ModelDb.p_xianxiazp_filed>();
                    p_xianxiazp_filed.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_xianxiazp_filed.jd_user_sn = new UserIdentityBag().user_sn;

                    p_xianxiazp_filed.InsertOrUpdate($"zt_user_sn = '{new UserIdentityBag().user_sn}'");

                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}