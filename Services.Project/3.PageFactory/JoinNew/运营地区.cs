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
    /// 运营地区模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// 运营团队补人城市优先级列表
            /// </summary>
            public class YyCityList
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
                        width = "110px",
                        title = "所属运营",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        placeholder = "所属运营",
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营账号",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "600",
                        minWidth = "600"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "城市名称",
                        width = "100",
                        minWidth = "100"
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

                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by yy_user_sn,id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_join_new_citys, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_join_new_citys
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            var ting_name = "";
                            foreach (var ting_id in ting_ids.Split(','))
                            {
                                ting_name += DoMySql.FindEntityById<ModelDb.user_info_tg>(ting_id.ToInt()).ting_name + ",";
                            }
                            return ting_name.TrimEnd(',');
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
                    var reqData = req.data_json.ToModel<ModelDb.p_join_new_citys>();
                    var p_join_new_citys = new ModelDb.p_join_new_citys();

                    lSql.Add(p_join_new_citys.DeleteTran($"id in ({reqData.id})"));

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
                    var p_join_new_citys = new ModelDb.p_join_new_citys();
                    lSql.Add(p_join_new_citys.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_citys
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }

            /// <summary>
            /// 运营团队补人城市优先级新增
            /// </summary>
            public class YyCityPost
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
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                        }),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page_post.yy_user_sn.value%>"}
                                },
                                func = GetTings,
                                resCallJs = $"{new ModelBasic.EmtExt.XmSelect.Js("post.ting_ids").optionObjects(@"JSON.parse(res.data)")}"
                            }
                        }
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("ting_ids")
                    {
                        title = "直播厅",
                        bindData = ""
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
                                code = "win_pop_iframe('选择城市', '/JoinNew/YyCity/SelectCity');"
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

                /// <summary>
                /// 获取直播厅筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTings(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Ting().GetTingidsOptions(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var p_join_new_citys = req.data_json.ToModel<ModelDb.p_join_new_citys>();

                    if (p_join_new_citys.yy_user_sn.IsNullOrEmpty()) throw new Exception("请选择运营账号");
                    if (p_join_new_citys.ting_ids.IsNullOrEmpty()) throw new Exception("请选择直播厅");
                    if (p_join_new_citys.name.IsNullOrEmpty()) throw new Exception("请选择城市");

                    p_join_new_citys.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    foreach (var name in p_join_new_citys.name.Split(','))
                    {
                        p_join_new_citys.name = name;

                        p_join_new_citys.Insert();
                    }

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
        }
    }
}
