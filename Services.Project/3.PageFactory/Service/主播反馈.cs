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
        #region 意见反馈
        /// <summary>
        /// 查看下属的账号数据
        /// </summary>
        public class FeedBackList
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


                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "所属运营",
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
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("user_sn").clear()};"
                        }
                    }
                });

                var option_tg = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn))
                {
                    option_tg.Add(item.name, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "所属厅管",
                    disabled = true,
                    options = option_tg,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("user_sn").options(@"JSON.parse(res.data)")}"
                        }
                    }
                });

                var option_zb = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn))
                {
                    option_zb.Add(item.username, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("user_sn")
                {
                    placeholder = "主播",
                    options = option_zb,
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "提交日期",
                    disabled = true,
                    //defaultValue = req.c_date
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
                result.data = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                {
                    text = "反馈时间",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn_text")
                {
                    text = "所属主播",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                {
                    text = "所属厅管",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_text")
                {
                    text = "所属运营",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("feedback")
                {
                    text = "反馈内容",
                    width = "500",
                    minWidth = "500"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("pic_url_href")
                {
                    text = "图片",
                    width = "200",
                    minWidth = "200"
                });
                //listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("info")
                //{
                //    text = "联系方式",
                //    width = "160",
                //    minWidth = "160"
                //});
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

                /*
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
                    title = "提示说明",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DeletesAction,
                        field_paras = "id"
                    }
                });
                */
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
                    /// <summary>
                    /// 关键词
                    /// </summary>
                    public string keyword { get; set; }
                    public string user_sn { get; set; }
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
                string where = $"(feedback is not null and feedback != '' and feedback_type = '{ModelDb.p_service_feedback.feedback_type_enum.倾诉箱.ToSByte()}')";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                if (!dtoReqListData.user_sn.IsNullOrEmpty())
                {
                    where += $" and user_sn='{dtoReqListData.user_sn}'";
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_service_feedback, ItemDataModel>(filter, reqJson);
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
                public string user_sn { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_service_feedback
            {
                public string user_sn_text
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{this.user_sn}'").username;
                    }
                }
                public string tg_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
                    }
                }

                public string yy_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        var yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, tg_user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{yy_user_sn}'", false).username;
                    }
                }
                public string pic_url_href
                {
                    get
                    {
                        return $"<a href='{pic_url}' target='_blank'>{pic_url}</a>";
                    }
                }
            }
            #endregion

            #region 异步请求处理

            /// <summary>
            /// 批量删除资产
            /// </summary>
            /// <param name="req">回调函数提交统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                List<string> lSql = new List<string>();
                var user_base = new ModelDb.user_base();
                user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                lSql.Add(user_base.UpdateTran($"id = ({dtoReqData.id})"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            public class DtoReqData : ModelDb.user_base
            {
                public string id { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class FeedBackPost
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                {
                    title = "List",
                    text = "反馈记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Service/FeedBack/List",
                    },
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
                var p_service_feedback = DoMySql.FindEntityById<ModelDb.p_service_feedback>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_service_feedback.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("tips")
                {
                    defaultValue = "哈喽，亲爱的千广宝宝，如果你在工作中遇到什么困难委屈或者有什么想对公司说的话，都可以在这里写下来。你的反馈和诉求将直接越过厅管和运营，直达公司管理部。收到你的反馈以后，我们会一一听取，尽最大努力解决问题！感恩我们的相遇，愿你在千广能收获自己的梦想！",
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("info")
                {
                    title = "联系方式",
                    placeholder = "邮箱/电话/微信",
                    defaultValue = p_service_feedback.info.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("feedback")
                {
                    title = "反馈",
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    defaultValue = p_service_feedback.feedback.ToNullableString()
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
                var p_service_feedback = req.data_json.ToModel<ModelDb.p_service_feedback>();

                if (p_service_feedback.id == 0)
                {
                    p_service_feedback.user_sn = new UserIdentityBag().user_sn;
                    p_service_feedback.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                }
                if (p_service_feedback.info.IsNullOrEmpty())
                {
                    throw new Exception("请填写联系方式");
                }
                if (p_service_feedback.feedback.IsNullOrEmpty())
                {
                    throw new Exception("请填写反馈内容");
                }



                p_service_feedback.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }
        #endregion


        #region 倾诉箱
        public class ConfideList
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


                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "所属运营",
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
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("user_sn").clear()};"
                        }
                    }
                });

                var option_tg = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn))
                {
                    option_tg.Add(item.name, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "所属厅管",
                    disabled = true,
                    options = option_tg,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("user_sn").options(@"JSON.parse(res.data)")}"
                        }
                    }
                });

                var option_zb = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn))
                {
                    option_zb.Add(item.username, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("user_sn")
                {
                    placeholder = "主播",
                    options = option_zb,
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "提交日期",
                    disabled = true,
                    //defaultValue = req.c_date
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                {
                    text = "反馈时间",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn_text")
                {
                    text = "所属主播",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                {
                    text = "所属厅管",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_text")
                {
                    text = "所属运营",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("feedback")
                {
                    text = "反馈",
                    width = "500",
                    minWidth = "500"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("info")
                {
                    text = "联系方式",
                    width = "160",
                    minWidth = "160"
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

                /*
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
                    title = "提示说明",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DeletesAction,
                        field_paras = "id"
                    }
                });
                */
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
                    /// <summary>
                    /// 关键词
                    /// </summary>
                    public string keyword { get; set; }
                    public string user_sn { get; set; }
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
                string where = $"(feedback is not null and feedback != '')";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                if (!dtoReqListData.user_sn.IsNullOrEmpty())
                {
                    where += $" and user_sn='{dtoReqListData.user_sn}'";
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_service_feedback, ItemDataModel>(filter, reqJson);
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
                public string user_sn { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_service_feedback
            {
                public string user_sn_text
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{this.user_sn}'").username;
                    }
                }
                public string tg_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
                    }
                }

                public string yy_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        var yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, tg_user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{yy_user_sn}'", false).username;
                    }
                }
            }
            #endregion

            #region 异步请求处理

            /// <summary>
            /// 批量删除资产
            /// </summary>
            /// <param name="req">回调函数提交统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                List<string> lSql = new List<string>();
                var user_base = new ModelDb.user_base();
                user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                lSql.Add(user_base.UpdateTran($"id = ({dtoReqData.id})"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            public class DtoReqData : ModelDb.user_base
            {
                public string id { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 倾诉箱提交
        /// </summary>
        public class ConfidePost
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
                //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                //{
                //    title = "List",
                //    text = "反馈记录",
                //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                //    {
                //        url = "/Service/FeedBack/List",
                //    },
                //});
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
                var p_service_feedback = DoMySql.FindEntityById<ModelDb.p_service_feedback>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_service_feedback.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("feedback")
                {
                    title = "倾诉",
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtImageSelect("pic_url")
                {
                    title = "上传截图",
                    
                });
                formDisplay.formItems.Add(new ModelBasic.EmtRadio("hidename")
                {
                    title = "匿名提交",
                    options = new Dictionary<string, string>
                    {
                        {"是","1"},
                        {"否","0"},
                    },
                    defaultValue = "1"
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
                var p_service_feedback = req.data_json.ToModel<ModelDb.p_service_feedback>();
                p_service_feedback.feedback_type = ModelDb.p_service_feedback.feedback_type_enum.倾诉箱.ToSByte();
                if (p_service_feedback.id == 0)
                {
                    p_service_feedback.user_sn = new UserIdentityBag().user_sn;
                    p_service_feedback.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                }
                if (p_service_feedback.feedback.IsNullOrEmpty())
                {
                    throw new Exception("请填写反馈内容");
                }



                p_service_feedback.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }
        #endregion


        #region 推荐
        public class RecommendList
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


                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "所属运营",
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
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("user_sn").clear()};"
                        }
                    }
                });

                var option_tg = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn))
                {
                    option_tg.Add(item.name, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "所属厅管",
                    disabled = true,
                    options = option_tg,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("user_sn").options(@"JSON.parse(res.data)")}"
                        }
                    }
                });

                var option_zb = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn))
                {
                    option_zb.Add(item.username, item.user_sn);
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("user_sn")
                {
                    placeholder = "主播",
                    options = option_zb,
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "提交日期",
                    disabled = true,
                    //defaultValue = req.c_date
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                {
                    text = "反馈时间",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn_text")
                {
                    text = "所属主播",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                {
                    text = "所属厅管",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_text")
                {
                    text = "所属运营",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("feedback")
                {
                    text = "反馈",
                    width = "500",
                    minWidth = "500"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("info")
                {
                    text = "联系方式",
                    width = "160",
                    minWidth = "160"
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

                /*
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
                    title = "提示说明",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DeletesAction,
                        field_paras = "id"
                    }
                });
                */
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
                    /// <summary>
                    /// 关键词
                    /// </summary>
                    public string keyword { get; set; }
                    public string user_sn { get; set; }
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
                string where = $"(feedback is not null and feedback != '')";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                if (!dtoReqListData.user_sn.IsNullOrEmpty())
                {
                    where += $" and user_sn='{dtoReqListData.user_sn}'";
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_service_feedback, ItemDataModel>(filter, reqJson);
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
                public string user_sn { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_service_feedback
            {
                public string user_sn_text
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{this.user_sn}'").username;
                    }
                }
                public string tg_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
                    }
                }

                public string yy_user_sn_text
                {
                    get
                    {
                        var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                        var yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, tg_user_sn);
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{yy_user_sn}'", false).username;
                    }
                }
            }
            #endregion

            #region 异步请求处理

            /// <summary>
            /// 批量删除资产
            /// </summary>
            /// <param name="req">回调函数提交统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                List<string> lSql = new List<string>();
                var user_base = new ModelDb.user_base();
                user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                lSql.Add(user_base.UpdateTran($"id = ({dtoReqData.id})"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            public class DtoReqData : ModelDb.user_base
            {
                public string id { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 倾诉箱提交
        /// </summary>
        public class RecommendPost
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
                //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                //{
                //    title = "List",
                //    text = "反馈记录",
                //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                //    {
                //        url = "/Service/FeedBack/List",
                //    },
                //});
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
                var p_service_feedback = DoMySql.FindEntityById<ModelDb.p_service_feedback>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_service_feedback.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("like")
                {
                    title = "用户喜好",
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("lives")
                {
                    title = "直播推荐",
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtImageSelect("pic")
                {
                    title = "上传截图",
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
                var p_service_feedback = req.data_json.ToModel<ModelDb.p_service_feedback>();
                p_service_feedback.feedback_type = ModelDb.p_service_feedback.feedback_type_enum.推荐.ToSByte();

                string info = "";
                info += $"用户喜好:{req.GetPara("like")};";
                info += $"直播推荐:{req.GetPara("lives")};";
                if (p_service_feedback.id == 0)
                {
                    p_service_feedback.user_sn = new UserIdentityBag().user_sn;
                    p_service_feedback.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                }


                p_service_feedback.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }
        #endregion
    }
}
