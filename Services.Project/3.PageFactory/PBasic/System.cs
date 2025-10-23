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
    /// 系统
    /// </summary>
    public partial class PageFactory
    {
        public partial class System
        {
            #region 日志列表
            /// <summary>
            /// 日志列表
            /// </summary>
            public class LogList
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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        placeholder = "操作人",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtInput("memo")
                    {
                        placeholder = "备注信息",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        placeholder = "创建时间",
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("log_type_text")
                    {
                        text = "日志类型",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("modular_function")
                    {
                        text = "模块功能名称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("client_ip")
                    {
                        text = "客户端ip",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_type_id_text")
                    {
                        text = "操作人类型",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn_text")
                    {
                        text = "操作人",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("memo")
                    {
                        text = "备注信息",
                        width = "200",
                        minWidth = "200"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮

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
                    var req = reqJson.GetPara();
                    string where = "1=1";

                    //查询条件
                    if (!req["user_name"].IsNullOrEmpty())
                    {
                        where += $" and user_sn in (select user_sn from user_base where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and name like '%{req["user_name"].ToString()}%')";
                    }

                    if (!req["memo"].IsNullOrEmpty())
                    {
                        where += $" and memo like '%{req["memo"].ToString()}%'";
                    }

                    if (!req["create_time"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["create_time"].ToNullableString(), 0);
                        where += " and create_time >= '" + dateRange.date_range_s + "' and create_time <= '" + dateRange.date_range_e + "'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.sys_biz_log, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.sys_biz_log
                {
                    public string log_type_text
                    {
                        get
                        {
                            return ((log_type_enum)log_type).ToString();
                        }
                    }
                    public string user_type_id_text
                    {
                        get
                        {
                            return new DomainBasic.UserTypeApp().GetInfoById(user_type_id).name;
                        }
                    }
                    public string user_sn_text
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{user_sn}'", false).name;
                        }
                    }
                }
                #endregion
                #region 异步请求处理

                #endregion
            }
            #endregion
        }
    }
}
