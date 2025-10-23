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
    /// 主播目标
    /// </summary>
    public partial class PageFactory
    {
        public partial class Task
        {
            /// <summary>
            ///扫脸任务
            /// </summary>
            public class FaceList
            {

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

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_month")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.month,
                        placeholder = "选择月份",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.ting_sn.value%>"}
                            },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });
                    var option_zb = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn))
                    {
                        option_zb.Add(item.username, item.user_sn);
                    }



                    listFilter.formItems.Add(new ModelBasic.EmtInput("s_days")
                    {
                        width = "160px",

                        placeholder = "扫脸次数"
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
                    listDisplay.operateWidth = "180";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_month")
                    {
                        text = "月份",
                        width = "140",
                        minWidth = "140"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                    {
                        text = "主播",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "所属厅管",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "所属团队",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("s_days")
                    {
                        text = "有效天数",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("s_hours")
                    {
                        text = "有效时长",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("down_num")
                    {
                        text = "下发次数",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("saoed_num")
                    {
                        text = "已扫次数",
                        width = "100",
                        minWidth = "100"
                    });

                    #region 操作列按钮
                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        field_paras = "id",
                    //        url = "Edit"
                    //    },
                    //    text = "编辑",
                    //    disabled = true,
                    //});
                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    //    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    //    {
                    //        func = DelAction,
                    //        field_paras = "id"
                    //    },
                    //    text = "删除",
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

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = "1=1";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                    }
                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                    }
                    if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and zb_user_sn = '{req["zb_user_sn"].ToNullableString()}'";
                    }

                    if (!req["c_month"].ToNullableString().IsNullOrEmpty()) where += $" AND c_month ='{req["c_month"]}'";


                    if (!req["s_days"].ToNullableString().IsNullOrEmpty())
                    {

                        where += $" and s_days = '{req["s_days"].ToNullableString()}'";

                    }

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:

                            where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.yyer:

                            where += $" and yy_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_renwu_saolian, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    public string create_time { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_renwu_saolian
                {
                    public string zb_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.tg_user_sn)}'", false).name;
                        }
                    }
                    public string submit_time
                    {
                        get
                        {
                            return this.create_time.ToDate().ToString("yyyy-MM");
                        }
                    }

                }
                #endregion

                #region MyRegion
                /// <summary>
                /// 获取厅管筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {

                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString()))
                    {
                        option.Add(item.name, item.user_sn);
                    }
                    result.data = option.ToJson();
                    return result;
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
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 删除绩效目标
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
  

                    return info;
                }
                #endregion
            }

            /// <summary>
            ///扫脸任务-tg
            /// </summary>
            public class FaceList_TG
            {
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

                    string content = "";

                    var tgs = new ServiceFactory.UserInfo.Tg().GetBaseInfos(new ServiceFactory.UserInfo.Tg.TgBaseInfoFilter()
                    {
                        attachUserType = new ServiceFactory.UserInfo.Tg.TgBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Tg.TgBaseInfoFilter.AttachUserType.UserType.基地,
                            UserSn = new UserIdentityBag().user_sn
                        },
                        attachWhere = $"user_sn NOT IN (SELECT tg_user_sn FROM p_renwu_saolian_tg WHERE zt_user_sn = '{new UserIdentityBag().user_sn}')"
                    });

                    foreach(var item in tgs)
                    {
                        content += $"{item.name},";
                    }

                    pageModel.topPartial = new List<ModelBase>
                    {
                        new ModelBasic.EmtHtml("html_top")
                        {
                            Content = $"未查看：<p>{content}</p>"
                        }
                    };
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

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_month")
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.month,
                        placeholder = "选择月份",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "name,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                    {
                        placeholder = "直播厅",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.ting_sn.value%>"}
                            },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                            }
                        }
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
                    listDisplay.operateWidth = "180";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_month")
                    {
                        text = "月份",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "所属厅管",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "所属运营",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("last_time_text")
                    {
                        text = "最近查看时间",
                        width = "200",
                        minWidth = "100"
                    });
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {

                }

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = "1=1";
                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                    }
                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                    }
                    if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and zb_user_sn = '{req["zb_user_sn"].ToNullableString()}'";
                    }

                    if (!req["c_month"].ToNullableString().IsNullOrEmpty()) where += $" AND c_month ='{req["c_month"]}'";


                    if (!req["last_time"].ToNullableString().IsNullOrEmpty())
                    {

                        where += $" and last_time = '{req["last_time"].ToNullableString()}'";

                    }

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:

                            where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.yyer:

                            where += $" and yy_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_renwu_saolian_tg, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    public string create_time { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_renwu_saolian_tg
                {
                    public string tg_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.tg_user_sn)}'", false).name;
                        }
                    }
                    public string submit_time
                    {
                        get
                        {
                            return this.create_time.ToDate().ToString("yyyy-MM");
                        }
                    }
                    public string last_time_text
                    {
                        get
                        {
                            string text = UtilityStatic.DateHelper.DateDiff(DateTime.Now, Convert.ToDateTime(this.last_time));

                            return text;
                        }
                    }




                }
                #endregion

                #region MyRegion
                /// <summary>
                /// 获取厅管筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString()))
                    {
                        option.Add(item.name, item.user_sn);
                    }
                    result.data = option.ToJson();
                    return result;
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
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }
                #endregion
            }
        }


    }
}
