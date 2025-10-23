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
    /// 签约提成档位规则
    /// </summary>
    public partial class PageFactory
    {
        public partial class QianYue
        {
            #region 规则新增/编辑页面
            /// <summary>
            /// 规则新增/编辑页面
            /// </summary>
            public class CommissionRulePost
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
                    var p_qianyue_commission_rule = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_qianyue_commission_rule>($"id = {req.id}", false);
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = p_qianyue_commission_rule.id.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("rate_s")
                    {
                        title = "签约率起始",
                        colLength = 6,
                        defaultValue = p_qianyue_commission_rule.rate_s.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("rate_e")
                    {
                        title = "签约率结束",
                        colLength = 6,
                        defaultValue = p_qianyue_commission_rule.rate_e.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInputMoney("amount")
                    {
                        title = "提成",
                        colLength = 6,
                        defaultValue = p_qianyue_commission_rule.amount.ToString()
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
                    var p_qianyue_commission_rule = req.data_json.ToModel<ModelDb.p_qianyue_commission_rule>();
                    if (p_qianyue_commission_rule.amount.IsNullOrEmpty()) throw new WeicodeException("请输入提成");

                    p_qianyue_commission_rule.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_qianyue_commission_rule.InsertOrUpdate();

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion

            #region 规则列表页面
            /// <summary>
            /// 提报记录
            /// </summary>
            public class CommissionRuleList
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("rate_s")
                    {
                        text = "签约率起始",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("rate_e")
                    {
                        text = "签约率结束",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                    {
                        text = "提成（元/签约一人）",
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

                    var req = reqJson.GetPara();

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by rate_s"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_qianyue_commission_rule, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_qianyue_commission_rule
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
                    var reqData = req.data_json.ToModel<ModelDb.p_qianyue_commission_rule>();
                    var p_qianyue_commission_rule = new ModelDb.p_qianyue_commission_rule();

                    lSql.Add(p_qianyue_commission_rule.DeleteTran($"id in ({reqData.id})"));

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                #endregion
            }
            #endregion
        }
    }
}
