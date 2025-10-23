using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class Zhubojiezou
        {

            #region 
            /// <summary>
            /// 
            /// </summary>
            public class DetailZhuboRuleList
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public ModelBasic.PageList Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageList("pagelist");
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
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列                  
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "规则名称",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("key")
                    {
                        text = "规则键",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_avg_rule")
                    {
                        text = "平均每档拉新数规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num_rule")
                    {
                        text = "总建联数规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2_rule")
                    {
                        text = "总二消个数规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2_avg_rule")
                    {
                        text = "二消音浪值规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hx_amount_avg_rule")
                    {
                        text = "回消音量值规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("hdpk_amount_rule")
                    {
                        text = "活动PK音浪值规则",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                    {
                        text = "创建时间",
                        width = "150",
                        minWidth = "150"
                    });

                    #endregion

                    #region 3.操作列
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
                        title = "删除",
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
                public class DtoReq
                {
                }
                #endregion
                #region ListData
                /// <summary>
                /// 菜品表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"1=1";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.jiezou_zhubo_detail_rule, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public string id { get; set; }
                    public string status { get; set; }
                    public string parent_id { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.jiezou_zhubo_detail_rule
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
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var jiezou_zhubo_detail_rule = req.data_json.ToModel<ModelDb.jiezou_zhubo_detail_rule>();
                    jiezou_zhubo_detail_rule.Delete();
                    return result;
                }
                #endregion
            }
            /// <summary>
            /// 
            /// </summary>
            public class DetailZhuboRulePost
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
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction
                    };
                    return pageModel;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var jiezou_zhubo_detail_rule = DoMySql.FindEntityById<ModelDb.jiezou_zhubo_detail_rule>(req.id);
                   
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = jiezou_zhubo_detail_rule.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "规则名称",
                        defaultValue = jiezou_zhubo_detail_rule.name.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("new_num_avg_rule")
                    {
                        title = "平均每档拉新数规则",
                        defaultValue = jiezou_zhubo_detail_rule.new_num_avg_rule.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("contact_num_rule")
                    {
                        title = "总建联数规则",
                        defaultValue = jiezou_zhubo_detail_rule.contact_num_rule.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("num_2_rule")
                    {
                        title = "总二消个数规则",
                        defaultValue = jiezou_zhubo_detail_rule.num_2_rule.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("amount_2_avg_rule")
                    {
                        title = "二消音浪值规则",
                        defaultValue = jiezou_zhubo_detail_rule.amount_2_avg_rule.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("hx_amount_avg_rule")
                    {
                        title = "回消音量值规则",
                        defaultValue = jiezou_zhubo_detail_rule.hx_amount_avg_rule.ToString(),
                        colLength = 6,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("hdpk_amount_rule")
                    {
                        title = "活动PK音浪值规则",
                        defaultValue = jiezou_zhubo_detail_rule.hdpk_amount_rule.ToString(),
                        colLength = 6,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public int id { get; set; }
                }
                #endregion
                #region 
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var jiezou_zhubo_detail_rule = req.data_json.ToModel<ModelDb.jiezou_zhubo_detail_rule>();
                    jiezou_zhubo_detail_rule.Update();
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.jiezou_zhubo_detail_rule
                {
                }
                #endregion
            }

            #endregion
        }
    }
}
