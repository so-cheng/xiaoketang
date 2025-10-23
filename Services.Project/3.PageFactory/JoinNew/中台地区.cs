using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    /// <summary>
    /// 中台地区模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            #region 基地配置
            /// <summary>
            /// 基地补人城市优先级列表
            /// </summary>
            public class ZtCityList
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
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "80";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "城市名称",
                        width = "120",
                        minWidth = "120"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        }
                    });

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            {
                                new ModelBasic.EmtModel.ButtonItem("")
                                {
                                    text = "批量删除",
                                    mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                    eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                    {
                                        func = DeletesAction,
                                     },
                                }
                            }
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

                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"zt_user_sn = '{new UserIdentityBag().user_sn}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_join_new_citys_zt, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_join_new_citys_zt
                {

                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_join_new_citys_zt>();
                    var p_join_new_citys_zt = new ModelDb.p_join_new_citys_zt();

                    lSql.Add(p_join_new_citys_zt.DeleteTran($"id in ({reqData.id})"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_join_new_citys_zt = new ModelDb.p_join_new_citys_zt();
                    lSql.Add(p_join_new_citys_zt.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_citys_zt
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }

            /// <summary>
            /// 基地补人城市优先级新增
            /// </summary>
            public class ZtCityPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
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
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "城市名称",
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtButton("button")
                    {
                        width = "200px",
                        defaultValue = "选择城市",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "win_pop_iframe('选择城市', '/JoinNew/Citys/SelectCity');"
                            }
                        }
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
                    var p_join_new_citys_zt = req.data_json.ToModel<ModelDb.p_join_new_citys_zt>();

                    if (p_join_new_citys_zt.name.IsNullOrEmpty()) throw new Exception("请选择城市");

                    p_join_new_citys_zt.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    foreach (var name in p_join_new_citys_zt.name.Split(','))
                    {
                        p_join_new_citys_zt.zt_user_sn = new UserIdentityBag().user_sn;
                        p_join_new_citys_zt.name = name;

                        p_join_new_citys_zt.Insert();
                    }

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion

            #region 超管端配置中台地区
            /// <summary>
            /// 中台补人城市优先级列表
            /// </summary>
            public class AllZtCityList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                    {
                        width = "120px",
                        placeholder = "基地",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("jder").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}", "name,user_sn"),
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
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "80";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zt_name")
                    {
                        text = "基地名称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "城市名称",
                        width = "120",
                        minWidth = "120"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        }
                    });

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            {
                                new ModelBasic.EmtModel.ButtonItem("")
                                {
                                    text = "批量删除",
                                    mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                    eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                    {
                                        func = DeletesAction,
                                     },
                                }
                            }
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

                    if (!reqJson.GetPara("zt_user_sn").IsNullOrEmpty())
                    {
                        where += $" and zt_user_sn = '{reqJson.GetPara("zt_user_sn")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_join_new_citys_zt, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_join_new_citys_zt
                {
                    public string zt_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(zt_user_sn).name;
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 删除
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_join_new_citys_zt>();
                    var p_join_new_citys_zt = new ModelDb.p_join_new_citys_zt();

                    lSql.Add(p_join_new_citys_zt.DeleteTran($"id in ({reqData.id})"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_join_new_citys_zt = new ModelDb.p_join_new_citys_zt();
                    lSql.Add(p_join_new_citys_zt.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_citys_zt
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }

            /// <summary>
            /// 中台补人城市优先级新增
            /// </summary>
            public class AllZtCityPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
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
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                    {
                        title = "基地",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("jder").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}", "name,user_sn"),
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "城市名称",
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtButton("button")
                    {
                        width = "200px",
                        defaultValue = "选择城市",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "win_pop_iframe('选择城市', '/JoinNew/Citys/SelectCity');"
                            }
                        }
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
                    var p_join_new_citys_zt = req.data_json.ToModel<ModelDb.p_join_new_citys_zt>();

                    if (p_join_new_citys_zt.zt_user_sn.IsNullOrEmpty()) throw new Exception("请选择基地");
                    if (p_join_new_citys_zt.name.IsNullOrEmpty()) throw new Exception("请选择城市");

                    p_join_new_citys_zt.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    foreach (var name in p_join_new_citys_zt.name.Split(','))
                    {
                        p_join_new_citys_zt.name = name;

                        p_join_new_citys_zt.Insert();
                    }

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}
