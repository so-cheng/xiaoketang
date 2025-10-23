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
    /// 绩效上报
    /// </summary>
    public partial class PageFactory
    {

        /// <summary>
        /// 绩效列表
        /// </summary>
        public class UserDayReport
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
                //pageModel.buttonGroup = GetButtonGroup(req);
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
                    disabled = true,
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
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(req.relation_type, new UserIdentityBag().user_sn)}", "name,user_sn"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                {
                    placeholder = "主播账号",
                    options = new Dictionary<string, string>(),
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "绩效发生日期",
                    defaultValue = req.c_date_range
                });
                return listFilter;
            }

            public JsonResultAction GetZhubo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
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
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "上报绩效",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Crm/Customer/ReportPost",
                    }
                });
                */
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "绩效发生日期",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "厅管账号",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "主播账号",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "音浪值",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "拉新数",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num")
                {
                    text = "建联数",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("datou_num")
                {
                    text = "误刷大头",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_text")
                {
                    text = "时间段",
                    width = "100",
                    minWidth = "100"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("question_demo")
                {
                    text = "今日问题",
                    width = "240",
                    minWidth = "240"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("summary_demo")
                {
                    text = "今日总结",
                    width = "280",
                    minWidth = "280"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("review_demo")
                {
                    text = "反思",
                    width = "280",
                    minWidth = "280"
                });
                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name="edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    disabled=true,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "/PCrm/Report/Post",
                        field_paras = "c_date,session"
                    },
                    text = "编辑"
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                public string keyword { get; set; }
                public string c_date_range { get; set; }
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
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{new UserIdentityBag().user_sn}'");

                string where = "1=1";
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                }

                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                }

                if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                }

                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                }



                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_session, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_day_session
            {
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                    }
                }
                public string session_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次", this.session.ToString());
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// 按分档显示绩效列表
        /// </summary>
        public class UserDayReportSession
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
                    disabled = true,
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
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
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
                listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                {
                    placeholder = "主播账号",
                    options = option_zb,
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range",true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "绩效发生日期",
                    defaultValue = req.c_date_range
                });
                return listFilter;
            }

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
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                
                //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("reportday")
                //{
                //    text = "数据提交",
                //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                //    {
                //        url = "Post",
                //    },
                //    disabled=true
                //});
                
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "绩效发生日期",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "厅管账号",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "主播账号",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_option")
                {
                    text = "时间段",
                    width = "100",
                    minWidth = "100"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_1")
                {
                    text = "首消音浪",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2")
                {
                    text = "二消个数",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2")
                {
                    text = "二消音浪",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_amount")
                {
                    text = "老用户",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "拉新数",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num")
                {
                    text = "建联数",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("datou_num")
                {
                    text = "误刷大头",
                    width = "100",
                    minWidth = "100",
                    summaryReq = new Pagination.SummaryReq
                    {
                        summaryType = Pagination.SummaryType.SUM
                    }
                });
                /*
                 listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("summary_demo")
                {
                    text = "今日总结",
                    width = "280",
                    minWidth = "280"
                });
                 */

                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "Post?is_edit=1",
                        field_paras = "id,zb_user_sn"
                    },
                    text = "编辑"
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    disabled = true,
                    eventCsAction=new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        field_paras="id",
                        func = DelAction
                    },
                    text = "删除"
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                public string keyword { get; set; }
                public string c_date_range { get; set; }
            }


            #region 回调cs函数
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_jixiao_day_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.GetPara()["id"].ToNullableString().ToInt());

                if(p_jixiao_day_session.c_date <= DateTime.Today.AddDays(-2))
                {
                    result.code = 1;
                    result.msg = "删除失败,不能删除提交超过3天的数据";
                    return result;
                }

                if (p_jixiao_day_session.Delete() > 0)
                {
                    var sum = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(amount_1),sum(num_2),sum(amount_2),sum(old_amount),sum(amount),sum(contact_num),sum(datou_num),sum(new_num)", $"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                    new ModelDb.p_jixiao_day
                    {
                        amount_1 = sum[0].ToInt(),
                        num_2 = sum[1].ToInt(),
                        amount_2 = sum[2].ToInt(),
                        old_amount = sum[3].ToInt(),
                        amount = sum[4].ToInt(),
                        contact_num = sum[5].ToInt(),
                        datou_num = sum[6].ToInt(),
                        new_num = sum[7].ToInt(),
                        session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                    }.Update($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                }

                return result;
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
                var req = reqJson.GetPara();
                string where = "1=1";
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                }

                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                }

                if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                }

                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                }

                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_session, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_day_session
            {
                public string session_option
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次",this.session.ToString());
                    }
                }
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// 每日上报绩效数据
        /// </summary>
        public class UserDayReportPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.style = @"background-image:url('/Assets/images/qgxkt_m.jpg');background-size: cover;background-position: center; background-repeat: no-repeat;";
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };

                pageModel.submitButton.eventJsClick.eventJavascript.code = @"if((page_post.new_num.value==='0'||page_post.new_num.value==='')&&
                                      (page_post.amount.value==='0'||page_post.amount.value==='')&&
                                      (page_post.num_2.value==='0'||page_post.num_2.value==='')&&
                                      (page_post.amount_2.value==='0'||page_post.amount_2.value==='')&&
                                      (page_post.contact_num.value==='0'||page_post.contact_num.value==='')&&
                                      (page_post.datou_num.value==='0'||page_post.datou_num.value==='')){
                                        page.floatlayer3.show();
                                        return false;
                                   }else{
                                    }";

                pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                {
                    position = AdjFloatLayer.Position.固定定位,
                    positionFixed = new AdjFloatLayer.PositionFixed
                    {
                        bottom = 10,
                        left = 30
                    },
                    emtModelBase = new EmtHtml("feedback")
                    {
                        Content = @"<a href=""/Service/FeedBack/Post"" style=""color:#3399FF;"">匿名反馈</a>",
                    }
                });

                pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer2")
                {
                    position = AdjFloatLayer.Position.固定定位,
                    positionFixed = new AdjFloatLayer.PositionFixed
                    {
                        bottom = 10,
                        left = 100
                    },
                    emtModelBase = new EmtHtml("CreateUser")
                    {
                        Content = @"<a href=""/pcrm/pcrm/crmpost"" style=""color:#3399FF;"">新建用户</a>",
                    }
                });

                pageModel.adjuncts.Add(new ModelBasic.AdjFloatLayer("floatlayer3")
                {
                    height="100px",
                    style = " border:5px solid #F0F0F0; background-color:#FDFDFD; padding:10px;",
                    position = ModelBasic.AdjFloatLayer.Position.固定定位,
                    positionRelative = new ModelBasic.AdjFloatLayer.PositionRelative
                    {
                        className = "post_submit_post",
                        offset_x = 20,
                        offset_y = 20
                    },
                    positionFixed = new ModelBasic.AdjFloatLayer.PositionFixed
                    {
                        display = ModelBasic.AdjFloatLayer.Display.none,
                        bottom = 40,
                        left = 10
                    },
                    emtModelBase = new ModelBasic.EmtGrid("Confirm")
                    {
                        items = new List<ModelBasic.EmtGrid.Item>
                        {
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=12,
                                emtModelBase=new ModelBasic.EmtLabel("tip")
                                {
                                    width="50px",
                                    defaultValue="当前是否为挂麦?",
                                    displayStatus= EmtModelBase.DisplayStatus.只读
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("true")
                                {
                                    width="50px",
                                    defaultValue="是",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code = "page_post.is_guamai.set('1')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtSubmitButton("false")
                                {
                                    width="50px",
                                    defaultValue="否",
                                    eventJsClick=new EventJsBasic
                                    {
                                        eventJavascript= new EventJavascript
                                        {
                                            code="page_post.is_guamai.set('0')"
                                        }
                                    }
                                }
                            },
                            new ModelBasic.EmtGrid.Item
                            {
                                colLength=3,
                                emtModelBase=new ModelBasic.EmtButton("cancle")
                                {
                                    width="200px",
                                    defaultValue="取消",
                                    eventJsChange=new EmtFormBase.EventJsChange
                                    {
                                        eventJavascript=new EventJavascript
                                        {
                                            code="page.floatlayer3.hide();"
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

                return pageModel;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("bg");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b1")
                {
                    text = "提交记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/PCrm/Report/List",
                    }
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b2")
                {
                    text = "首页",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                    eventToUrl=new EmtModel.ButtonItem.EventToUrl
                    {
                        url= "/Default/MainPage/Index",
                        target="top"
                    }
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b3")
                {
                    text = "新增用户",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                    eventToUrl = new EmtModel.ButtonItem.EventToUrl
                    {
                        url = "/PCrm/PCrm/CrmPost",
                        target = "top"
                    }
                });

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
                var p_jixiao_session = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(req.id, false);
                var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{p_jixiao_session.zb_user_sn}' and c_date='{p_jixiao_session.c_date.ToDate().ToString("yyyy-MM-dd")}'", false);
                if (p_jixiao_day.IsNullOrEmpty())
                {
                    p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{new UserIdentityBag().user_sn}' and c_date='{DateTime.Today.ToString("yyyy-MM-dd")}'", false);
                }
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("is_guamai")
                {
                    defaultValue = "0"
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("tips")
                {
                    defaultValue = "二消定义：首消用户本档内第二次消费礼物（如果二消客户送了全麦或者除拉新主播的其他主播礼物，拉新该用户主播二消个数加1，其余主播二消个数不增加，而其他主播礼物数算其他主播的二消音浪数。）",
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });

                if (new DomainBasic.UserTypeApp().GetInfoByCode("tger").id == new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                    {
                        title = "所属主播",
                        style = "background-color: transparent;",
                        options = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn),
                        defaultValue = req.zb_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "c_date","<%=page_post.c_date.value%>"},
                                { "zb_user_sn","<%=page_post.zb_user_sn.value%>"}
                            },
                                func = GetZbSessionInfo,
                                resCallJs = $"page_post.amount.setPlaceholder('本日累计:'+res.data.amount);" +
                                        $"page_post.amount_1.setPlaceholder('本日累计:'+res.data.amount_1);" +
                                        $"page_post.num_2.setPlaceholder('本日累计:'+res.data.num_2);" +
                                        $"page_post.amount_2.setPlaceholder('本日累计:'+res.data.amount_2);" +
                                        $"page_post.old_amount.setPlaceholder('本日累计:'+res.data.old_amount);" +
                                        $"page_post.new_num.setPlaceholder('本日累计:'+res.data.new_num);" +
                                        $"page_post.contact_num.setPlaceholder('本日累计:'+res.data.contact_num);" +
                                        $"page_post.datou_num.setPlaceholder('本日累计:'+res.data.datou_num);" 
                            }
                        }
                    });
                }
                else 
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtTextarea("zb")
                    {
                        title = "所属主播",
                        style = "background-color: transparent;",
                        defaultValue = p_jixiao_session.zb_user_sn.IsNullOrEmpty() ? new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).username : new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_session.zb_user_sn).username,
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("zb_user_sn")
                    {
                        title = "主播user_sn",
                        defaultValue = req.zb_user_sn
                    });
                }
                
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("is_edit")
                {
                    defaultValue = req.is_edit.ToNullableString()
                });
                var defaultDate = p_jixiao_session.c_date;
                if (defaultDate.IsNullOrEmpty())
                {
                    defaultDate = DateTime.Today;
                }

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "绩效日期",
                    style = "background-color: transparent;",
                    defaultValue = defaultDate.ToDate().ToString("yyyy-MM-dd"),
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "c_date","<%=page_post.c_date.value%>"},
                                { "zb_user_sn","<%=page_post.zb_user_sn.value%>"}
                            },
                            func = GetZbSessionInfo,
                            resCallJs = $"page_post.amount.setPlaceholder('本日累计:'+res.data.amount);" +
                                        $"page_post.amount_1.setPlaceholder('本日累计:'+res.data.amount_1);" +
                                        $"page_post.num_2.setPlaceholder('本日累计:'+res.data.num_2);" +
                                        $"page_post.amount_2.setPlaceholder('本日累计:'+res.data.amount_2);" +
                                        $"page_post.old_amount.setPlaceholder('本日累计:'+res.data.old_amount);" +
                                        $"page_post.new_num.setPlaceholder('本日累计:'+res.data.new_num);" +
                                        $"page_post.contact_num.setPlaceholder('本日累计:'+res.data.contact_num);" +
                                        $"page_post.datou_num.setPlaceholder('本日累计:'+res.data.datou_num);" 
                        }
                    }
                });
                /*formDisplay.formItems.Add(new ModelBasic.EmtSelect("is_leader")
                {
                    title = "我是组长",
                    colLength = 6,
                    options = new Dictionary<string, string>
                    {
                        { "是","1"},
                        { "否","0"}
                    },
                    defaultValue=p_jixiao_session.is_leader.ToString()
                });*/


                formDisplay.formItems.Add(new ModelBasic.EmtSelect("session")
                {
                    title = "时间段",
                    style = "background-color: transparent;",
                    colLength = 12,
                    options = new DomainBasic.DictionaryApp().GetListForOption("场次"),
                    defaultValue=p_jixiao_session.session.ToNullableString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtRadio("is_rest")
                {
                    title="是否请假",
                    style = "background-color: transparent;",
                    options =new Dictionary<string, string>
                    {
                        {"是","1" },
                        {"否","0" }
                    },
                    defaultValue = p_jixiao_session.is_rest.ToNullableString().IsNullOrEmpty()?"0": p_jixiao_session.is_rest.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                {
                    title = "基本信息",
                    style = "background-color: transparent;",
                });

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("new_num")
                {
                    title = "拉新数",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.new_num,
                    colLength = 6,
                    defaultValue = p_jixiao_session.new_num.ToNullableString()
                });
                string code = @"if(page_post.amount_1.value===''){page_post.amount_1.value = parseFloat(0);}
                                if(page_post.amount_2.value===''){page_post.amount_2.value = parseFloat(0);}
                                if(page_post.old_amount.value===''){page_post.old_amount.value = parseFloat(0);}
                                page_post.amount.set(parseFloat(page_post.amount_1.value)+parseFloat(page_post.amount_2.value)+parseFloat(page_post.old_amount.value));";
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_1")
                {
                    title = "首消音浪",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.amount_1,
                    colLength = 6,
                    defaultValue=p_jixiao_session.amount_1.ToNullableString(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventJavascript = new EventJavascript
                        {
                            code = code
                        }
                    }

                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("num_2")
                {
                    title = "二消个数",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.num_2,
                    colLength = 6,
                    defaultValue = p_jixiao_session.num_2.ToNullableString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2")
                {
                    title = "二消音浪",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.amount_2,
                    colLength = 6,
                    defaultValue = p_jixiao_session.amount_2.ToNullableString(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventJavascript = new EventJavascript
                        {
                            code = code
                        }
                    }
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("old_amount")
                {
                    title = "老用户音浪",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.old_amount,
                    colLength = 6,
                    defaultValue = p_jixiao_session.old_amount.ToNullableString(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventJavascript = new EventJavascript
                        {
                            code = code
                        }
                    }
                });
                
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("contact_num")
                {
                    title = "建联数",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.contact_num,
                    colLength = 6,
                    defaultValue = p_jixiao_session.contact_num.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("datou_num")
                {
                    title = "误刷大头",
                    style = "background-color: transparent;",
                    colLength = 6,
                    placeholder ="本日累计:" + p_jixiao_day.datou_num,
                    defaultValue = p_jixiao_session.datou_num.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount")
                {
                    title = "总音浪",
                    style = "background-color: transparent;",
                    placeholder = "本日累计:" + p_jixiao_day.amount,
                    colLength = 6,
                    defaultValue = p_jixiao_session.amount.ToString().IsNullOrEmpty() ? "0" : p_jixiao_session.amount.ToString(),
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });


                formDisplay.formItems.Add(new ModelBasic.EmtInput("new_pay_num")
                {
                    title = "新付费人数",
                    style = "background-color: transparent;",
                    colLength = 6,
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                /*
                 formDisplay.formItems.Add(new ModelBasic.EmtTextarea("summary_demo")
                {
                    title = "今日总结",
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    defaultValue = p_jixiao_day.summary_demo,
                    height = 100
                });
                 */

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("question_demo")
                {
                    title = "今日问题",

                    //mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    defaultValue = p_jixiao_day.question_demo,
                    //height=100
                });

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("review_demo")
                {
                    title = "反思",
                    //mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    defaultValue = p_jixiao_day.review_demo,
                    //height = 100
                });

                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; } = 0;
                public int is_edit { get; set; } = 0;
                public string zb_user_sn { get; set; }
            }


            /// <summary>
            /// 获取主播提交记录
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetZbSessionInfo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();

                var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"c_date='{req["c_date"].ToNullableString()}' and zb_user_sn='{req["zb_user_sn"].ToNullableString()}'",false);

                result.data =new
                {
                    amount_1 = (p_jixiao_day.amount_1.IsNullOrEmpty() ? 0: p_jixiao_day.amount_1),
                    num_2 = (p_jixiao_day.num_2.IsNullOrEmpty() ? 0 : p_jixiao_day.num_2),
                    amount_2 = (p_jixiao_day.amount_2.IsNullOrEmpty() ? 0 : p_jixiao_day.amount_2),
                    old_amount = (p_jixiao_day.old_amount.IsNullOrEmpty() ? 0 : p_jixiao_day.old_amount),
                    new_num = (p_jixiao_day.new_num.IsNullOrEmpty() ? 0 : p_jixiao_day.new_num),
                    contact_num= (p_jixiao_day.contact_num.IsNullOrEmpty() ? 0 : p_jixiao_day.contact_num),
                    datou_num= (p_jixiao_day.datou_num.IsNullOrEmpty() ? 0 : p_jixiao_day.datou_num),
                    summary_demo=(p_jixiao_day.summary_demo.IsNullOrEmpty() ? "" : p_jixiao_day.summary_demo)
                };
                return result;
            }

            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增每日上报
            /// </summary>
            /// <param name="req">1.主播提交每日数据;2.主播修改每日数据;3.厅管修改下级主播每日数据</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {

                var result = new JsonResultAction();
                
                var p_jixiao_day_session= req.data_json.ToModel<ModelDb.p_jixiao_day_session>();
                //var summary_demo = req.data_json.ToModel<ReqDto>().summary_demo;
                if(new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).IsNullOrEmpty())
                {
                    throw new Exception("主播不存在或未选择主播");
                }
                if (new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("主播账号已被删除，无法提交");
                if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).status == ModelDb.user_base.status_enum.逻辑删除.ToSByte()) throw new Exception("当前账号已被删除，无法提交");
                //如果是新增状态
                if (p_jixiao_day_session.id <= 0)
                {
                    p_jixiao_day_session.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    if(new DomainBasic.UserTypeApp().GetInfoByCode("zber").id!=new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).user_type_id && new DomainBasic.UserTypeApp().GetInfoByCode("tger").id != new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).user_type_id)
                    {
                        throw new Exception("当前用户类型禁止提交");
                    }
                    p_jixiao_day_session.tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, p_jixiao_day_session.zb_user_sn);

                    var p_session = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and session='{p_jixiao_day_session.session}'", false);
                    if (!p_session.IsNullOrEmpty()) throw new WeicodeException($"该时间段:{new DomainBasic.DictionaryApp().GetKeyFromValue("场次",p_jixiao_day_session.session.ToString())}已提交过,提交人:{new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.zb_user_sn).name},厅管:{new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_day_session.tg_user_sn).name},提交时间:{p_session.create_time}");
                    if (p_jixiao_day_session.c_date.IsNullOrEmpty()) throw new WeicodeException("请选择日期");
                }

                //如果是编辑状态
                if (p_jixiao_day_session.id > 0)
                {
                    var entity = DoMySql.FindEntityById<ModelDb.p_jixiao_day_session>(p_jixiao_day_session.id);
                    //校验数据
                    if (entity.id <= 0)
                    {
                        throw new Exception("此提报已经不存在");
                    }

                    //校验主播数据
                    if (new DomainBasic.UserTypeApp().GetInfo().sys_code == "zber")
                    {
                        if (entity.zb_user_sn != new UserIdentityBag().user_sn) throw new WeicodeException("无修改权限");
                    }

                    //校验厅管数据
                    if (new DomainBasic.UserTypeApp().GetInfo().sys_code == "tger")
                    {
                        if (new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, entity.zb_user_sn) != new UserIdentityBag().user_sn)
                        {
                            throw new WeicodeException("无修改权限");
                        }
                    }

                    var e = DoMySql.FindEntity<ModelDb.p_jixiao_day_session>($"c_date='{entity.c_date}' and session='{p_jixiao_day_session.session}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and id!='{p_jixiao_day_session.id}'", false);
                    if (e.id>0)
                    {
                        throw new Exception($"主播:{new DomainBasic.UserApp().GetInfoByUserSn(entity.zb_user_sn).username},日期:{entity.c_date},档位:{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", entity.session.ToString())}提交的数据已经存在");
                    }

                    p_jixiao_day_session.tenant_id = entity.tenant_id;
                    p_jixiao_day_session.c_date = entity.c_date;
                    p_jixiao_day_session.zb_user_sn = entity.zb_user_sn;
                    p_jixiao_day_session.tg_user_sn = entity.tg_user_sn;
                }

                if (p_jixiao_day_session.c_date > DateTime.Today) throw new WeicodeException("所选日期不能超过今日");
                if (p_jixiao_day_session.session.IsNullOrEmpty()) throw new WeicodeException("请选择时间段");
                var session = new DomainBasic.DictionaryApp().GetKeyFromValue("场次",p_jixiao_day_session.session.ToString());
                var session_last_time = session.Substring(session.IndexOf("-")+1);
                session_last_time = session_last_time.Substring(0, session_last_time.IndexOf(":"));

                if (DateTime.Now.ToString("HH").ToInt() < session_last_time.ToInt()&& p_jixiao_day_session.c_date == DateTime.Today&&DateTime.Now<DateTime.Today.AddDays(1).AddMinutes(-10)) throw new Exception("所选场次时间还未结束，当前无法提交");
                if (p_jixiao_day_session.new_num == null) p_jixiao_day_session.new_num = 0;
                if (p_jixiao_day_session.amount_1 == null) p_jixiao_day_session.amount_1 = 0;
                if (p_jixiao_day_session.num_2 == null) p_jixiao_day_session.num_2 = 0;
                if (p_jixiao_day_session.amount_2 == null) p_jixiao_day_session.amount_2 = 0;
                if (p_jixiao_day_session.old_amount == null) p_jixiao_day_session.old_amount = 0;
                if (p_jixiao_day_session.contact_num == null) p_jixiao_day_session.contact_num = 0;
                if (p_jixiao_day_session.new_num > 50) { throw new Exception("拉新数不能超过50"); }
                p_jixiao_day_session.new_num.ToInt("拉新数量不可小于0",ConvertExt.IntType.非负整数);
                p_jixiao_day_session.amount_1.ToInt("首消音浪值不可小于0", ConvertExt.IntType.非负整数);
                p_jixiao_day_session.num_2.ToInt("二消个数不可小于0", ConvertExt.IntType.非负整数);
                if (p_jixiao_day_session.num_2 > p_jixiao_day_session.new_num) { throw new Exception("二消个数必须小于等于拉新数"); }
                p_jixiao_day_session.amount_2.ToInt("二消音浪值不可小于0", ConvertExt.IntType.非负整数);
                p_jixiao_day_session.old_amount.ToInt("老客户音浪值不可小于0", ConvertExt.IntType.非负整数);
                p_jixiao_day_session.contact_num.ToInt("建联数不可小于0", ConvertExt.IntType.非负整数);

                p_jixiao_day_session.amount = p_jixiao_day_session.amount_1 + p_jixiao_day_session.amount_2 + p_jixiao_day_session.old_amount;

                if (p_jixiao_day_session.InsertOrUpdate() > 0)
                {
                    var sum = DoMySql.FindField<ModelDb.p_jixiao_day_session>("sum(amount_1),sum(num_2),sum(amount_2),sum(old_amount),sum(amount),sum(contact_num),sum(datou_num),sum(new_num)", $"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");

                    //新增状态
                    if (p_jixiao_day_session.id <= 0)
                    {
                        new ModelDb.p_jixiao_day
                        {
                            c_date = p_jixiao_day_session.c_date,
                            tenant_id = p_jixiao_day_session.tenant_id,
                            zb_user_sn = p_jixiao_day_session.zb_user_sn,
                            tg_user_sn = p_jixiao_day_session.tg_user_sn,
                            amount_1 = sum[0].ToInt(),
                            num_2 = sum[1].ToInt(),
                            amount_2 = sum[2].ToInt(),
                            old_amount = sum[3].ToInt(),
                            amount = sum[4].ToInt(),
                            contact_num = sum[5].ToInt(),
                            datou_num = sum[6].ToInt(),
                            new_num = sum[7].ToInt(),
                            job = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{p_jixiao_day_session.zb_user_sn}'").attach1,
                            session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            //summary_demo = summary_demo
                        }.InsertOrUpdate($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                    }
                    //编辑状态
                    if (p_jixiao_day_session.id > 0)
                    {
                        new ModelDb.p_jixiao_day
                        {
                            amount_1 = sum[0].ToInt(),
                            num_2 = sum[1].ToInt(),
                            amount_2 = sum[2].ToInt(),
                            old_amount = sum[3].ToInt(),
                            amount = sum[4].ToInt(),
                            contact_num = sum[5].ToInt(),
                            datou_num = sum[6].ToInt(),
                            new_num = sum[7].ToInt(),
                            session_count = DoMySql.FindField<ModelDb.p_jixiao_day_session>("count(*)", $"zb_user_sn='{p_jixiao_day_session.zb_user_sn}' and c_date='{p_jixiao_day_session.c_date}'")[0].ToInt()
                            //summary_demo = summary_demo
                        }.Update($"c_date='{p_jixiao_day_session.c_date}' and zb_user_sn='{p_jixiao_day_session.zb_user_sn}'");
                    }
                }
                else
                {
                    throw new WeicodeException("提交失败");
                }
                return result;
            }

            /// <summary>
            /// 额外请求参数
            /// </summary>
            public class ReqDto
            {
                /// <summary>
                /// 今日总结
                /// </summary>
                public string summary_demo { get; set; }
            }
            #endregion
        }



        #region 厅管设置直播类型
        /// <summary>
        /// 设置直播档类型
        /// </summary>
        public class SetType
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                if (req.is_table == 1)
                {
                    pageModel.style = "background-color: #E3F2D9";
                }
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
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
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("detail")
                {
                    text = "明细",
                    disabled = true,
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/ZbManage/Report/SetTypeList",
                    }
                });
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
                var p_jixiao_day_type = DoMySql.FindEntityById<ModelDb.p_jixiao_day_type>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    title = "id",
                    defaultValue = req.id.ToString()
                });
                if (req.id > 0)
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "直播日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_jixiao_day_type.c_date.ToDate().ToString("yyyy-MM-dd"),
                        isRequired = true
                    });
                }
                else
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        title = "直播日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                        defaultValue = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd"),
                        isRequired = true
                    });
                }

                formDisplay.formItems.Add(new ModelBasic.EmtSelect("session")
                {
                    title = "场次",
                    options = new DomainBasic.DictionaryApp().GetListForOption("场次"),
                    displayStatus = EmtModelBase.DisplayStatus.编辑,
                    defaultValue = p_jixiao_day_type.session.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("c_type")
                {
                    title = "类型",
                    options = new List<ModelDoBasic.Option>
                    {
                        new ModelDoBasic.Option
                        {
                            text="普通游戏档",
                            value="1",
                            displayNames="type_name"
                        },
                        new ModelDoBasic.Option
                        {
                            text="游戏PK档",
                            value="2"
                        },
                        new ModelDoBasic.Option
                        {
                            text="跨房",
                            value="3",
                            displayNames="type_name"
                        },
                        new ModelDoBasic.Option
                        {
                            text="厅战",
                            value="4"
                        },
                        new ModelDoBasic.Option
                        {
                            text="其他",
                            value="5",
                            displayNames="type_name"
                        },
                    },
                    defaultValue = p_jixiao_day_type.c_type.ToString(),
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("type_name")
                {
                    title = "名称",
                    displayStatus = p_jixiao_day_type.c_type==1 ? EmtModelBase.DisplayStatus.编辑: EmtModelBase.DisplayStatus.隐藏,
                    defaultValue = p_jixiao_day_type.type_name
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("tips")
                {
                    defaultValue = @"普通游戏档是指 抢帽子等游戏;游戏PK档 单指红蓝PK游戏;覆盖厅管的设置是指 运营设置厅战和厅管设置的PK活动相重合，是—以运营设置为主；否—以厅管设置为主",
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; } = 0;
                public int is_edit { get; set; } = 0;
                public int is_show { get; set; } = 1;
                public int is_table { get; set; } = 0;
                public string c_date { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
            }

            /// <summary>
            /// 额外参数
            /// </summary>
            public class ReqPara
            {
                public string dateRange { get; set; }
            }

            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增每日上报
            /// </summary>
            /// <param name="req">1.主播提交每日数据;2.主播修改每日数据;3.厅管修改下级主播每日数据</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {

                var result = new JsonResultAction();
                var p_jixiao_day_type = req.data_json.ToModel<ModelDb.p_jixiao_day_type>();
                p_jixiao_day_type.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                p_jixiao_day_type.tg_user_sn = new UserIdentityBag().user_sn;
                if (p_jixiao_day_type.session.IsNullOrEmpty()) throw new WeicodeException("场次不可为空");
                if (p_jixiao_day_type.c_type == null)
                {
                    p_jixiao_day_type.c_type = 0;
                }

                
                //更新状态
                if (p_jixiao_day_type.id > 0)
                {
                    var p_type = DoMySql.FindEntity<ModelDb.p_jixiao_day_type>($"tg_user_sn='{p_jixiao_day_type.tg_user_sn}' and c_date='{p_jixiao_day_type.c_date}' and session='{p_jixiao_day_type.session}'", false);
                    if (p_jixiao_day_type.c_date.IsNullOrEmpty()) throw new WeicodeException("直播日期不可为空");
                    if (p_type.c_type > 2)
                    {
                        throw new Exception("禁止修改状态");
                    }
                    p_jixiao_day_type.Update();
                }
                //新增状态
                else
                {
                    var lSql = new List<string>();
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req.data_json.ToModel<ReqPara>().dateRange, 0);
                    for (var date = dateRange.date_range_s; date.ToDate() <= dateRange.date_range_e.ToDate(); date = date.ToDate().AddDays(1).ToString())
                    {
                        p_jixiao_day_type.c_date = date.ToDate();
                        var p_type = DoMySql.FindEntity<ModelDb.p_jixiao_day_type>($"tg_user_sn='{p_jixiao_day_type.tg_user_sn}' and c_date='{p_jixiao_day_type.c_date}' and session='{p_jixiao_day_type.session}'", false);
                        if (p_type.c_type > 2)
                        {
                            continue;
                        }
                        lSql.Add(p_jixiao_day_type.InsertOrUpdateTran($"c_date='{date.ToDate()}' and session='{p_jixiao_day_type.session}' and tg_user_sn='{p_jixiao_day_type.tg_user_sn}'"));
                        if (lSql.Count > 60) throw new Exception("日期范围过大");
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                }
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 设置直播档类型列表
        /// </summary>
        public class SetTypeList
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
                /*
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "上报绩效",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Crm/Customer/ReportPost",
                    }
                });
                */
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "直播日期",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_text")
                {
                    text = "场次",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_type_text")
                {
                    text = "类型",
                    width = "160",
                    minWidth = "160"
                });

                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "/ZbManage/Report/SetType",
                        field_paras = "id"
                    },
                    text = "编辑"
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        field_paras = "id",
                        func = DelAction
                    },
                    text = "删除"
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public string relation_type { get; set; } = "运营邀厅管";
                public string keyword { get; set; }
                public string c_date_range { get; set; }
            }


            #region 回调cs函数
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_jixiao_day_type = DoMySql.FindEntityById<ModelDb.p_jixiao_day_type>(req.GetPara()["id"].ToNullableString().ToInt());
                //如果所选条目的user_sn不属于当前用户，或者运营已经设置过，则禁止删除
                //todo: 临时的判断方法，在字典拥有分组后需要修改
                if (p_jixiao_day_type.tg_user_sn != new UserIdentityBag().user_sn || p_jixiao_day_type.c_type>2)
                {
                    result.code = 1;
                    result.msg = "删除失败,无删除权限";
                    return result;
                }

                p_jixiao_day_type.Delete();

                return result;
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
                var req = reqJson.GetPara();
                string where = $"tg_user_sn='{new UserIdentityBag().user_sn}'";

                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_type, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_day_type
            {
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy年MM月dd日");
                    }
                }
                public string session_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次", this.session.ToString());
                    }
                }
                public string c_type_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次类型", this.c_type.ToString());
                    }
                }
            }
            #endregion
        }
        #endregion




        #region 运营设置直播类型
        /// <summary>
        /// 设置直播档类型
        /// </summary>
        public class YYSetType
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
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
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                if (req.id <= 0)
                {
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("detail")
                    {
                        text = "明细",
                        disabled = true,
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/TgManage/Report/SetTypeList",
                        }
                    });
                }
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
                var p_jixiao_day_type = DoMySql.FindEntityById<ModelDb.p_jixiao_day_type>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    title = "id",
                    defaultValue = req.id.ToString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "直播日期",
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    defaultValue = DateTime.Today.ToString("yyyy-MM-dd"),
                });
                if (req.id <= 0)
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("l_tg")
                    {
                        bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn),
                        title = "直播厅",
                    });
                }
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("session")
                {
                    title = "场次",
                    options = new DomainBasic.DictionaryApp().GetListForOption("场次"),
                    displayStatus = EmtModelBase.DisplayStatus.编辑,
                    defaultValue = p_jixiao_day_type.session.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("c_type")
                {
                    title = "类型",
                    options = new Dictionary<string, string>
                    {
                        { "跨房","3" },
                        { "厅战","4" }
                    },
                    defaultValue = p_jixiao_day_type.c_type.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("is_cover")
                {
                    title = "覆盖厅管的设置",
                    options = new Dictionary<string, string>
                    {
                        { "是","1" },
                        { "否","0" }
                    },
                    defaultValue = "1",
                });

                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int is_show { get; set; } = 1;
                public int id { get; set; } = 0;
                public int is_edit { get; set; } = 0;
                public string c_date { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
            }

            /// <summary>
            /// 额外参数
            /// </summary>
            public class ReqPara
            {
                public string dateRange { get; set; }
                public string[] l_tg { get; set; }
                public string is_cover { get; set; }
            }

            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增每日上报
            /// </summary>
            /// <param name="req">1.主播提交每日数据;2.主播修改每日数据;3.厅管修改下级主播每日数据</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {

                var result = new JsonResultAction();
                var p_jixiao_day_type = req.data_json.ToModel<ModelDb.p_jixiao_day_type>();
                p_jixiao_day_type.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_jixiao_day_type.session.IsNullOrEmpty()) throw new WeicodeException("场次不可为空");
                if (p_jixiao_day_type.c_date.IsNullOrEmpty()) throw new WeicodeException("直播日期不可为空");
                if (p_jixiao_day_type.c_type == null)
                {
                    p_jixiao_day_type.c_type = 0;
                }

                //更新状态
                if (p_jixiao_day_type.id > 0)
                {
                    p_jixiao_day_type.Update();
                }
                //新增状态
                else
                {
                    var lSql = new List<string>();
                    foreach (var tg_user_sn in req.data_json.ToModel<ReqPara>().l_tg[0].Split(','))
                    {
                        var p_type = DoMySql.FindEntity<ModelDb.p_jixiao_day_type>($"tg_user_sn='{tg_user_sn}' and c_date='{p_jixiao_day_type.c_date}' and session='{p_jixiao_day_type.session}'", false);
                        p_jixiao_day_type.tg_user_sn = tg_user_sn;
                        //如果运营选择不覆盖厅管的设置，并且厅管已经设置了直播类型,则将直播类型换为厅管设置的类型
                        //todo: 临时的判断方法，在字典拥有分组后需要修改
                        if (req.data_json.ToModel<ReqPara>().is_cover == "0" && p_type.c_type>0&& p_type.c_type<3)
                        {
                            p_jixiao_day_type.c_type = p_type.c_type;
                        }
                        lSql.Add(p_jixiao_day_type.InsertOrUpdateTran($"tg_user_sn='{tg_user_sn}' and c_date='{p_jixiao_day_type.c_date}' and session='{p_jixiao_day_type.session}'"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                }
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 设置直播档类型列表
        /// </summary>
        public class YYSetTypeList
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
                /*
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "上报绩效",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Crm/Customer/ReportPost",
                    }
                });
                */
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                {
                    text = "直播厅",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "直播日期",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("session_text")
                {
                    text = "场次",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_type_text")
                {
                    text = "类型",
                    width = "160",
                    minWidth = "160"
                });

                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "/TgManage/Report/SetType",
                        field_paras = "id"
                    },
                    text = "编辑"
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        field_paras = "id",
                        func = DelAction
                    },
                    text = "删除"
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public string relation_type { get; set; } = "运营邀厅管";
                public string keyword { get; set; }
                public string c_date_range { get; set; }
            }


            #region 回调cs函数
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_jixiao_day_type = DoMySql.FindEntityById<ModelDb.p_jixiao_day_type>(req.GetPara()["id"].ToNullableString().ToInt());

                if (new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, p_jixiao_day_type.tg_user_sn)!= new UserIdentityBag().user_sn)
                {
                    result.code = 1;
                    result.msg = "删除失败,无删除权限";
                    return result;
                }

                p_jixiao_day_type.Delete();

                return result;
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
                var req = reqJson.GetPara();
                string where = $"tg_user_sn in{new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管,new UserIdentityBag().user_sn)}";

                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_day_type, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_day_type
            {
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy年MM月dd日");
                    }
                }
                public string tg_user_sn_text
                {
                    get
                    {
                        return new DomainBasic.UserApp().GetInfoByUserSn(this.tg_user_sn).username;
                    }
                }
                public string session_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次", this.session.ToString());
                    }
                }
                public string c_type_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("场次类型", this.c_type.ToString());
                    }
                }
            }
            #endregion
        }
        #endregion

    }

}
