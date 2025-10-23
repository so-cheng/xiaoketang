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
    /// 基础数据
    /// </summary>
    public partial class PageFactory
    {
        public partial class JixiaoDay
        {
            #region 厅收益上交列表
            /// <summary>
            /// 厅收益上交列表
            /// </summary>
            public class ShangjiaoList
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
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.buttonGroup = GetButtonGroup(req);
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",                     
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter { 
                            attachWhere = $"status = '{ModelDb.user_base.status_enum.正常.ToSByte()}'"
                        }),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "厅管账号",                      
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                                func = GetTings,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn)
                    });
                          
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        placeholder = "收益日期",
                    });
                   
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        options = new Dictionary<string, string>
                        {
                            {"已上交",ModelDb.p_jixiao_day_ting_shangjiao.status_enum.已上交.ToInt().ToString()},
                            {"未上交",ModelDb.p_jixiao_day_ting_shangjiao.status_enum.未上交.ToInt().ToString()},
                            {"待生成",ModelDb.p_jixiao_day_ting_shangjiao.status_enum.待生成.ToInt().ToString()},                          
                        },                     
                        placeholder = "支付状态",
                    });
                    return listFilter;
                }
              

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


                /// <summary>
                /// 获取直播厅筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTings(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(req["tg_user_sn"].ToNullableString()).ToJson();
                    return result;
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "所属运营",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "所属厅管",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "收益发生日期",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("income")
                    {
                        text = "连线收入服务费",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("datou")
                    {
                        text = "误刷大头",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("xing")
                    {
                        text = "星守护",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("huodong")
                    {
                        text = "活动",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                    {
                        text = "应付金额",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amounted")
                    {
                        text = "已付金额",
                        width = "130",
                        minWidth = "130",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_test")
                    {
                        text = "状态",
                        width = "130",
                        minWidth = "130"
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
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{req["yy_user_sn"]}'";
                    }

                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                    }                  

                    if (!reqJson.GetPara("c_date").IsNullOrEmpty())
                    {
                        where += $" and c_date = '{req["c_date"]}'";                   
                    }
                    if (!req["status"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and status = '{req["status"]}'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by c_date desc"
                    };
                   
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_ting_shangjiao, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_jixiao_day_ting_shangjiao
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string status_test
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
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
