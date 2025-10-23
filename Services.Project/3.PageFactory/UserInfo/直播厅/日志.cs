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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        public partial class UserInfo
        {

            #region 日志
            public class TingLogList
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("c_type")
                    {
                        width = "120px",
                        options=new Dictionary<string, string>()
                        {
                            {"开厅",ModelDb.user_info_ting_log.c_type_enum.开厅.ToSByte().ToString()},
                            {"关厅",ModelDb.user_info_ting_log.c_type_enum.关厅.ToSByte().ToString()},
                            {"绑定大头号",ModelDb.user_info_ting_log.c_type_enum.绑定大头号.ToSByte().ToString()},
                            {"解绑大头号",ModelDb.user_info_ting_log.c_type_enum.解绑大头号.ToSByte().ToString()},
                        },
                        title = "操作类型",
                        placeholder = "操作类型"
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        width = "160px",
                        mold = EmtTimeSelect.Mold.date_range,
                        title = "操作时间",
                        placeholder = "操作时间"
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
                    //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LossList")
                    //{
                    //    title = "LossList",
                    //    text = "流失名单",
                    //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //    {
                    //        url = "LossList"
                    //    }
                    //});

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
                    listDisplay.operateWidth = "350";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_type_name")
                    {
                        text = "操作类型",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "操作时间",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("operate_user")
                    {
                        text = "操作人",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "说明",
                        width = "220",
                        minWidth = "220"
                    });

                    #region 操作列按钮
                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = "Post",
                    //        field_paras = "id"
                    //    },
                    //    style = "",
                    //    text = "编辑",
                    //    name = "Post"
                    //});
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

                    //查询条件
                    if (!reqJson.GetPara("c_type").IsNullOrEmpty()) where += $" AND c_type = '{reqJson.GetPara("c_type")}'";
                    if (!reqJson.GetPara("dateRange").IsNullOrEmpty()) 
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange"));

                        where += $" and create_time >= '{dateRange.date_range_s}' and create_time <= '{dateRange.date_range_e}'";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_ting_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string keyword { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_ting_log
                {
                    /// <summary>
                    /// 操作类型
                    /// </summary>
                    public string c_type_name
                    {
                        get
                        {
                            return Enum.GetName(typeof(ModelDb.user_info_ting_log.c_type_enum),c_type);
                        }
                    }

                    /// <summary>
                    /// 操作人
                    /// </summary>
                    public string operate_user
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(user_sn).username;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{user_info_ting_sn}'", false).ting_name;
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
