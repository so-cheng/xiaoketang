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
    /// 跨房
    /// </summary>
    public partial class PageFactory
    {
        public partial class KuaFang
        {
            #region 活动列表
            /// <summary>
            /// 活动列表页
            /// </summary>
            public class List
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
                        text = "创建跨房",
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
                    listDisplay.operateWidth = "160";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "跨房时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("start_time")
                    {
                        text = "活动开始时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("end_time")
                    {
                        text = "活动结束时间",
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
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_kuafang, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_kuafang
                {
                    
                    public string c_date_text
                    {
                        get
                        {
                            return c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion
                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 创建/编辑跨房
            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class Post
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
                    var p_kuafang = DoMySql.FindEntityById<ModelDb.p_kuafang>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_kuafang.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "跨房时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        defaultValue = p_kuafang.c_date.IsNullOrEmpty() ? "" : p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("start_time")
                    {
                        title = "活动开始时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        defaultValue = p_kuafang.start_time.IsNullOrEmpty() ? "" : p_kuafang.start_time.ToDateTime().ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("end_time")
                    {
                        title = "活动结束时间",
                        mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                        defaultValue = p_kuafang.end_time.IsNullOrEmpty() ? "" : p_kuafang.end_time.ToDateTime().ToString()
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
                    var p_kuafang = req.data_json.ToModel<ModelDb.p_kuafang>();
                    p_kuafang.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    if (req.GetPara("c_date").IsNullOrEmpty()) throw new Exception("请选择跨房时间");
                    if (req.GetPara("start_time").IsNullOrEmpty()) throw new Exception("请选择活动开始时间");
                    if (req.GetPara("end_time").IsNullOrEmpty()) throw new Exception("请选择活动结束时间");

                    p_kuafang.InsertOrUpdate();

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}
