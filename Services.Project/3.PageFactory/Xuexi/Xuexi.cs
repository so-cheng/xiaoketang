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
        /// 
        /// </summary>
        public class XuexiList
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
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "提交日期",
                    disabled = true,
                    //defaultValue = req.c_date
                });

                var option = new Dictionary<string,string>();
                foreach (var item in DoMySql.FindList<ModelDb.xuexi_category>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status=0"))
                {
                    option.Add(item.name,item.id.ToString());
                }
                listFilter.formItems.Add(new ModelBasic.EmtSelect("category_id")
                {
                    placeholder = "类型",
                    options=option,
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                {
                    text = "创建时间",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("category_id_text")
                {
                    text = "类别",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("title")
                {
                    text = "标题",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("introduction")
                {
                    text = "介绍",
                    width = "400",
                    minWidth = "400"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("url")
                {
                    text = "链接地址",
                    width = "250",
                    minWidth = "250"
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
                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status=0";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                if (!dtoReqListData.create_time.IsNullOrEmpty())
                {
                    where += $" and create_time='{dtoReqListData.create_time}'";
                }
                if (!dtoReqListData.category_id.IsNullOrEmpty())
                {
                    where += $" and category_id='{dtoReqListData.category_id}'";
                }
                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.xuexi_base, ItemDataModel>(filter, reqJson);
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
                public string category_id { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.xuexi_base
            {
                public string create_time_text
                {
                    get
                    {
                        return create_time.ToDate().ToString("yyyy-MM-dd");
                    }
                }
                public string category_id_text
                {
                    get
                    {
                        return DoMySql.FindEntityById<ModelDb.xuexi_category>(category_id).name;
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
                var xuexi_base = new ModelDb.xuexi_base();
                xuexi_base.Delete($"id = ({dtoReqData.id})");
                return result;
            }
            public class DtoReqData : ModelDb.xuexi_base
            {
            }
            #endregion
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class XuexiPost
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
                /*
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
                 */
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
                var xuexi_base = DoMySql.FindEntityById<ModelDb.xuexi_base>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = xuexi_base.id.ToNullableString()
                });

                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.xuexi_category>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status=0 order by sort desc"))
                {
                    option.Add(item.name,item.id.ToString());
                }
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("category_id")
                {
                    title = "类型",
                    options= option,
                    defaultValue = xuexi_base.category_id.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("title")
                {
                    title = "标题",
                    defaultValue = xuexi_base.title
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("introduction")
                {
                    title = "介绍",
                    defaultValue = xuexi_base.title
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("url")
                {
                    title = "链接地址",
                    defaultValue = xuexi_base.title
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("sort")
                {
                    title = "排序",
                    defaultValue = xuexi_base.sort.ToNullableString()
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
                var xuexi_base = req.data_json.ToModel<ModelDb.xuexi_base>();
                xuexi_base.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                xuexi_base.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }




        /// <summary>
        /// 
        /// </summary>
        public class XuexiCategoryList
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
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "提交日期",
                    disabled = true,
                    //defaultValue = req.c_date
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = $"CategoryPost",
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                {
                    text = "创建时间",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                {
                    text = "名称",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sort")
                {
                    text = "排序",
                    width = "160",
                    minWidth = "160"
                });
                #region 操作列按钮
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "CategoryPost",
                        field_paras = "id"
                    },
                    text = "编辑",
                    name = "CategoryPost"
                });

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
                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status=0";

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                if (!dtoReqListData.create_time.IsNullOrEmpty())
                {
                    where += $" and create_time='{dtoReqListData.create_time}'";
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.xuexi_category, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.xuexi_category
            {
                public string create_time_text
                {
                    get
                    {
                        return create_time.ToDate().ToString("yyyy-MM-dd");
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
                var xuexi_category = new ModelDb.xuexi_category();
                xuexi_category.Delete($"id = ({dtoReqData.id})");
                return result;
            }
            public class DtoReqData : ModelDb.xuexi_category
            {
            }
            #endregion
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class XuexiCategoryPost
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
                /*
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
                 */
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
                var xuexi_category = DoMySql.FindEntityById<ModelDb.xuexi_category>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = xuexi_category.id.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                {
                    title = "名称",
                    defaultValue = xuexi_category.name
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("sort")
                {
                    title = "排序",
                    defaultValue = xuexi_category.sort.ToNullableString()
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
                var xuexi_category = req.data_json.ToModel<ModelDb.xuexi_category>();
                xuexi_category.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                xuexi_category.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }
    }
}
