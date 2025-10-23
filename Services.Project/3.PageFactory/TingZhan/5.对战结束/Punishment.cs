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
    /// 惩罚
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 惩罚内容列表
            /// <summary>
            /// 
            /// </summary>
            public class CfContentList
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

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("cf_content")
                    {
                        text = "惩罚内容",
                        width = "160",
                        minWidth = "160",
                        sort = true
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
                        name = "CfContent"
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
                    var tingzhang_id = new ServiceFactory.TingZhanService().getNewTingzhan().id;
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhang_id} and (tg_user_sn1 = '{new UserIdentityBag().user_sn}' or tg_user_sn2 = '{new UserIdentityBag().user_sn}')";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_tingzhan_mate
                {
                    public ModelDb.p_tingzhan p_tingzhan
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_tingzhan>($"id = {tingzhan_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            if (tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                            }
                            else
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                            }
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 惩罚内容
            /// <summary>
            /// 惩罚内容编辑页面
            /// </summary>
            public class CfContentPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("");
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

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = new ServiceFactory.TingZhanService().getNewTingzhan().c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("cf_content")
                    {
                        title = "惩罚内容",
                        defaultValue = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {req.id}", false).cf_content
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
                    var reqData = req.data_json.ToModel<ModelDb.p_tingzhan_mate>();

                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {reqData.id}", false);
                    if (!p_tingzhan_mate.IsNullOrEmpty())
                    {
                        p_tingzhan_mate.cf_content = reqData.cf_content;
                        p_tingzhan_mate.Update();
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