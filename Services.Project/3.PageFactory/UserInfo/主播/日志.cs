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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("c_type")
                    {
                        width = "100px",
                        options=new Dictionary<string, string>()
                        {
                            {"新的主播",ModelDb.user_info_zhubo_log.c_type_enum.新的主播.ToSByte().ToString()},
                            {"入职",ModelDb.user_info_zhubo_log.c_type_enum.入职.ToSByte().ToString()},
                            {"离职",ModelDb.user_info_zhubo_log.c_type_enum.离职.ToSByte().ToString()},
                        },
                        title = "操作类型",
                        placeholder = "操作类型"
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        width = "100px",
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
                    listDisplay.operateWidth = "80";
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zhubo_name")
                    {
                        text = "主播",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "说明",
                        width = "760",
                        minWidth = "760"
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
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo_log, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_zhubo_log
                {
                    /// <summary>
                    /// 操作类型
                    /// </summary>
                    public string c_type_name
                    {
                        get
                        {
                            return Enum.GetName(typeof(ModelDb.user_info_zhubo_log.c_type_enum),c_type);
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

                    public string zhubo_name
                    {
                        get
                        {
                            var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_info_zb_sn = '{user_info_zb_sn}'",false);
                            return new DomainBasic.UserApp().GetInfoByUserSn(user_info_zhubo.user_sn).username;
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
