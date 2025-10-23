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
    /// 跨房-活动报名
    /// </summary>
    public partial class PageFactory
    {
        public partial class KuaFangMateApply
        {
            #region 报名信息
            /// <summary>
            /// 报名信息（我的）
            /// </summary>
            public class MyList
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("id")
                    {

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
                        width = "110",
                        minWidth = "110"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amont")
                    {
                        text = "目标音浪",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "备注",
                        width = "200",
                        minWidth = "200"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "100",
                        minWidth = "100"
                    });
                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = CancelAction,
                            field_paras = "id"
                        },
                        style = "",
                        text = "取消"
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
                    var req = reqJson.GetPara();
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_user_sn = '{new UserIdentityBag().user_sn}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_kuafang_mate_apply, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_kuafang_mate_apply
                {
                    public ModelDb.p_kuafang p_kuafang
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_kuafang>($"id = {kuafang_id}", false);
                        }
                    }
                    public ModelDb.p_kuafang_mate p_kuafang_mate
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_kuafang_mate>($"id = {kuafang_mate_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_kuafang.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting
                    {
                        get
                        {
                            return !p_kuafang_mate.ting_sn1.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Ting().GetTingBySn(p_kuafang_mate.ting_sn1) : new ServiceFactory.UserInfo.Ting.TingInfo();
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting.yy_user_sn).name;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_kuafang_mate.tg_user_sn1).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting.ting_name;
                        }
                    }
                    public string amont
                    {
                        get
                        {
                            return p_kuafang_mate.amont.ToInt().ToString() + "W";
                        }
                    }
                    public string content
                    {
                        get
                        {
                            return p_kuafang_mate.content;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 取消报名
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction CancelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();

                    var p_kuafang_mate_apply = req.data_json.ToModel<ModelDb.p_kuafang_mate_apply>();
                    p_kuafang_mate_apply = DoMySql.FindEntity<ModelDb.p_kuafang_mate_apply>($"id = {p_kuafang_mate_apply.id}", false);

                    if (p_kuafang_mate_apply.status.Equals(ModelDb.p_kuafang_mate_apply.status_enum.已确认.ToSByte())) throw new Exception("当前报名已确认");

                    p_kuafang_mate_apply.status = ModelDb.p_kuafang_mate_apply.status_enum.无效.ToSByte();
                    p_kuafang_mate_apply.Update();

                    return info;
                }

                #endregion
            }
            #endregion
        }
    }
}
