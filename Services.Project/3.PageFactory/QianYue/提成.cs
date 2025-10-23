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
    /// 流量提成
    /// </summary>
    public partial class PageFactory
    {
        public partial class QianYue
        {
            #region 已提交绩效展示页面（签约）
            /// <summary>
            /// 已提交绩效展示页面（签约）
            /// </summary>
            public class CommissionView
            {
                #region DefaultView
                public ModelBasic.PageDetail Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageDetail("details");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PageDetail pageModel, DtoReq req)
                {
                    var p_qianyue_commission = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_qianyue_commission>($"yearmonth = '{DateTime.Today.AddMonths(-1).ToString("yyyy-MM")}' and qy_user_sn = '{new UserIdentityBag().user_sn}'");
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHtml("yearmonth")
                    {
                        title = "绩效年月",
                        Content = p_qianyue_commission.yearmonth.ToString()
                    });

                    formDisplay.formItems.Add(new EmtHtml("wx_name")
                    {
                        title = "名字",
                        Content = p_qianyue_commission.qy_name.ToString()
                    });

                    formDisplay.formItems.Add(new EmtHtml("wechat_num")
                    {
                        title = "添加微信",
                        Content = p_qianyue_commission.wechat_num.ToString()
                    });

                    formDisplay.formItems.Add(new EmtHtml("f_num")
                    {
                        title = "签约人数",
                        Content = p_qianyue_commission.f_num.ToString()
                    });

                    formDisplay.formItems.Add(new EmtHtml("qianyue_rate")
                    {
                        title = "签约率",
                        Content = p_qianyue_commission.qianyue_rate.ToString() + '%'
                    });

                    formDisplay.formItems.Add(new EmtHtml("status")
                    {
                        title = "状态",
                        Content = ((ModelDb.p_qianyue_commission.status_enum)p_qianyue_commission.status).ToString()
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion
            }
            #endregion

            #region 绩效确认情况页面（外宣主管）
            /// <summary>
            /// 绩效确认情况页面（外宣主管）
            /// </summary>
            public class SituationView
            {
                #region DefaultView
                public ModelBasic.PageDetail Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageDetail("details");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PageDetail pageModel, DtoReq req)
                {
                    var yearmonth = DateTime.Today.AddMonths(-1).ToString("yyyy-MM");
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHtml("yearmonth")
                    {
                        title = "绩效年月",
                        Content = yearmonth
                    });

                    formDisplay.formItems.Add(new EmtHtml("count")
                    {
                        title = "总人数",
                        Content = DoMySql.FindField<ModelDb.user_base>("count(1)", $"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()}")[0]
                    });

                    formDisplay.formItems.Add(new EmtHtml("confirmed")
                    {
                        title = "已确认",
                        Content = $"<a style='color:blue;' href=\"javascript: win_pop_iframe('已确认名单', 'Confirmed', '100%', '100%', '');\">{DoMySql.FindField<ModelDb.p_qianyue_commission>("count(1)", $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and yearmonth = '{yearmonth}' and status = {ModelDb.p_qianyue_commission.status_enum.已确认.ToSByte()}")[0]}</a>"
                    });

                    formDisplay.formItems.Add(new EmtHtml("unconfirm")
                    {
                        title = "未确认",
                        Content = $"<a style='color:blue;' href=\"javascript: win_pop_iframe('未确认名单', 'UnConfirm', '100%', '100%', '');\">{DoMySql.FindField<ModelDb.user_base>("count(1)", $"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and not exists (select 1 from p_qianyue_commission where qy_user_sn = user_base.user_sn and yearmonth = '{yearmonth}' and status = {ModelDb.p_qianyue_commission.status_enum.已确认.ToSByte()})")[0]}</a>"
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion
            }
            #endregion

            #region 已确认名单
            /// <summary>
            /// 已确认名单
            /// </summary>
            public class Confirmed
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
                    listDisplay.operateWidth = "70";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("org_name")
                    {
                        text = "团队",
                        width = "150",
                        minWidth = "150"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qy_name")
                    {
                        text = "名字",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_num")
                    {
                        text = "添加微信",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("f_num")
                    {
                        text = "签约人数",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_rate_text")
                    {
                        text = "签约率",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "确认时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "退回",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = BackAction,
                            field_paras = "id",
                        },
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
                    string where = $"yearmonth = '{DateTime.Today.AddMonths(-1).ToString("yyyy-MM")}' and status = {ModelDb.p_qianyue_commission.status_enum.已确认.ToSByte()}";

                    var req = reqJson.GetPara();

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_qianyue_commission, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_qianyue_commission
                {
                    public string org_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.sys_organize>($"id = {new DomainBasic.UserApp().GetInfoByUserSn(qy_user_sn).organize_id}", false).name;
                        }
                    }
                    public string qianyue_rate_text
                    {
                        get
                        {
                            return qianyue_rate.ToString() + '%';
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 退回
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction BackAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var reqData = req.data_json.ToModel<ModelDb.p_qianyue_commission>();
                    var p_qianyue_commission = new ModelDb.p_qianyue_commission();

                    lSql.Add(p_qianyue_commission.DeleteTran($"id in ({reqData.id})"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                #endregion
            }
            #endregion

            #region 未确认名单
            /// <summary>
            /// 未确认名单
            /// </summary>
            public class UnConfirm
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
                    listDisplay.isHideOperate = true;
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("org_name")
                    {
                        text = "团队",
                        width = "150",
                        minWidth = "150"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "名字",
                        width = "100",
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
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and not exists (select 1 from p_qianyue_commission where qy_user_sn = user_base.user_sn and yearmonth = '{DateTime.Today.AddMonths(-1).ToString("yyyy-MM")}' and status = {ModelDb.p_qianyue_commission.status_enum.已确认.ToSByte()})";

                    var req = reqJson.GetPara();

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_base
                {
                    public string org_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.sys_organize>($"id = {new DomainBasic.UserApp().GetInfoByUserSn(user_sn).organize_id}", false).name;
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}
