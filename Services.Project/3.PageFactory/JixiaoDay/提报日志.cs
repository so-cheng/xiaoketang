using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using static WeiCode.Models.ModelBasic;
namespace Services.Project
{
    /// <summary>
    /// 跨房
    /// </summary>
    public partial class PageFactory
    {
        public partial class JixiaoDay
        {
            /// <summary>
            /// 日志列表页
            /// </summary>
            public class LogList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("op_type")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"提交日报","提交"},
                            {"修改日报","修改"},
                            {"删除日报","删除"},
                        },
                        disabled = false,
                        placeholder = "操作类型",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("op_user")
                    {
                        disabled = false,
                        placeholder = "操作人",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("op_user_type")
                    {
                        disabled = false,
                        options = new Dictionary<string, string>
                        {
                            {"厅管",ModelEnum.UserTypeEnum.tger.ToInt().ToString()},
                            {"主播",ModelEnum.UserTypeEnum.zber.ToInt().ToString()}
                        },
                        placeholder = "操作人类型",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("op_date")
                    {
                        disabled = false,
                        mold = EmtTimeSelect.Mold.date,
                        placeholder = "操作日期",
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
                    listDisplay.isHideOperate = true;
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.operateWidth = "200";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("op_user_sn")
                    {
                        text = "操作人",
                        width = "150",
                        minWidth = "150",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("op_user_type")
                    {
                        text = "操作人类型",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "操作时间",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("memo")
                    {
                        text = "操作类型",
                        width = "700",
                        minWidth = "700",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("info")
                    {
                        text = "变更详情",
                        width = "800",
                        minWidth = "800",
                    });
                    #endregion 显示列
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
                    string where = $"modular_function = '数据提报' and tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'";

                    if (!reqJson.GetPara("op_type").IsNullOrEmpty())
                    {
                        where += $" and memo like '{reqJson.GetPara("op_type")}%'";
                    }
                    if (!reqJson.GetPara("op_user").IsNullOrEmpty())
                    {
                        where += $" and user_sn in (select user_sn from user_base where username like '%{reqJson.GetPara("op_user")}%')";
                    }
                    if (!reqJson.GetPara("op_user_type").IsNullOrEmpty())
                    {
                        where += $" and user_type_id = '{reqJson.GetPara("op_user_type")}'";
                    }
                    if (!reqJson.GetPara("op_date").IsNullOrEmpty())
                    {
                        where += $" and create_time >= '{reqJson.GetPara("op_date").ToDate().ToString("yyyy-MM-dd")}' and create_time <'{reqJson.GetPara("op_date").ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.sys_biz_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.sys_biz_log
                {
                    public string op_user_sn
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(user_sn).username;
                        }
                    }
                    public string op_user_type
                    {
                        get
                        {
                            return new DomainBasic.UserTypeApp().GetInfoById(user_type_id).name;
                        }
                    }
                }

                #endregion ListData
                #region 异步请求处理
                #endregion
            }

        }
    }
}
