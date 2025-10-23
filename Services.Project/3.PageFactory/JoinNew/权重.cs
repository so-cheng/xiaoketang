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
            #region 补人权重列表
            /// <summary>
            /// 补人权重列表
            /// </summary>
            public class WeightList
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

                    // 设置头部信息
                    var zb_count_sum = DoMySql.FindField<ModelDb.p_join_apply>("sum(zb_count)", $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0].ToInt();
                    var real_zb_count_sum = DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0].ToInt();
                    var rate = zb_count_sum > 0 ? Math.Round((real_zb_count_sum.ToDecimal() / zb_count_sum.ToDecimal() * 100), 2).ToString() + "%" : "0%";
                    string top = "";
                    top += $@"<div class=""layui-card"">";
                    top += $@"    <div class=""layui-row"">";
                    // 公会补人率
                    top += $@"      <div class=""layui-col-md3"">";
                    top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                    top += $@"          <div class=""layui-flex layui-items-center"">";
                    top += $@"            <div class=""layui-icon layui-icon-home"" style=""font-size: 24px; color: #1E9FFF; margin-right: 10px;""></div>";
                    top += $@"            <div>";
                    top += $@"              <div class=""text-muted"">公会补人率{rate}</div>";
                    top += $@"            </div>";
                    top += $@"          </div>";
                    top += $@"        </div>";
                    top += $@"      </div>";

                    top += $@"    </div>";
                    top += $@"</div>";

                    pageModel.topPartial = new List<ModelBase>
                    {
                        new ModelBasic.EmtHtml("html_top")
                        {
                            Content = top
                        }
                    };
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
                        width = "140px",
                        placeholder = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        },
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "140px",
                        placeholder = "厅管账号",
                        options = new Dictionary<string, string>(),
                    });
                    return listFilter;
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
                    result.data = new ServiceFactory.RelationService().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
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
                        text = "新增厅权重",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"TingPost",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "新增团队权重",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"YyPost",
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
                        funcGetListData = GetListData,
                        pageSize = 50
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("w_type_text")
                    {
                        text = "权重类型",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("apply_zb_count")
                    {
                        text = "提交人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("real_zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("join_rate")
                    {
                        text = "补人率",
                        width = "90",
                        minWidth = "90",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("quit_zb_count")
                    {
                        text = "流失人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("stay_rate")
                    {
                        text = "留人率",
                        width = "90",
                        minWidth = "90",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("weight")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "权重值",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"weight", "weight" },
                            },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"weight_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_weight")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","weight" },
                                                    {"value","<%=page.weight_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.weight_text.value);$('.floatlayer_div').hide();",
                                                func= new ServiceFactory.JoinNew().FastEditWeight
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_weight")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.floatlayer_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.weight_text.set(page.focus.attr('data-weight'));"
                                }
                            }
                        }
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
                    if (!reqJson.GetPara("tg_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{reqJson.GetPara("tg_user_sn")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_join_new_weight, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_join_new_weight
                {
                    public string w_type_text
                    {
                        get
                        {
                            return ((w_type_enum)w_type).ToString();
                        }
                    }
                    public ServiceFactory.UserInfo.Yy.YYInfo yy
                    {
                        get
                        {
                            return yy_user_sn.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Yy.YYInfo() : new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn);
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return yy.username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public ServiceFactory.UserInfo.Ting.TingInfo ting
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Ting.TingInfo() : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn);
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting.ting_name;
                        }
                    }
                    public string apply_zb_count
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return DoMySql.FindField<ModelDb.p_join_apply>("COALESCE(sum(zb_count), 0)", $"ting_sn = '{ting_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0];
                                case (sbyte)w_type_enum.运营:
                                    return DoMySql.FindField<ModelDb.p_join_apply>("COALESCE(sum(zb_count), 0)", $"yy_user_sn = '{yy_user_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0];
                                default:
                                    return "";
                            }
                        }
                    }
                    public string real_zb_count
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where ting_sn = '{ting_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0];
                                case (sbyte)w_type_enum.运营:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where yy_user_sn = '{yy_user_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0];
                                default:
                                    return "";
                            }
                        }
                    }
                    public string zb_count
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}')")[0];
                                case (sbyte)w_type_enum.运营:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"yy_user_sn = '{yy_user_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}')")[0];
                                default:
                                    return "";
                            }
                        }
                    }
                    public string quit_zb_count
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}') and not exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {ModelDb.user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0];
                                case (sbyte)w_type_enum.运营:
                                    return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"yy_user_sn = '{yy_user_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}') and not exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {ModelDb.user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0];
                                default:
                                    return "";
                            }
                        }
                    }
                    public string join_rate
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return ting.join_rate + "%";
                                case (sbyte)w_type_enum.运营:
                                    return yy.attach2 + "%";
                                default:
                                    return "";
                            }
                        }
                    }
                    public string stay_rate
                    {
                        get
                        {
                            switch (w_type)
                            {
                                case (sbyte)w_type_enum.厅:
                                    return ting.stay_rate + "%";
                                case (sbyte)w_type_enum.运营:
                                    return yy.attach3 + "%";
                                default:
                                    return "";
                            }
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
                    var reqData = req.data_json.ToModel<ModelDb.p_join_new_weight>();
                    var p_join_new_weight = new ModelDb.p_join_new_weight();

                    lSql.Add(p_join_new_weight.DeleteTran($"id in ({reqData.id})"));

                    DoMySql.ExecuteSqlTran(lSql);

                    // 更新补人申请表权重值
                    new ServiceFactory.JoinNew().UpdateWeight();
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
                    var p_join_new_weight = new ModelDb.p_join_new_weight();
                    lSql.Add(p_join_new_weight.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);

                    // 更新补人申请表权重值
                    new ServiceFactory.JoinNew().UpdateWeight();
                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_weight
                {
                    public string ids { get; set; }
                }
                #endregion
                #endregion
            }
            #endregion

            #region 厅补人权重新增
            /// <summary>
            /// 厅补人权重新增
            /// </summary>
            public class TingWeightPost
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
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_weight")
                    {
                        title = "权重信息",
                        selectUrl = "/JoinNew/Weight/TingSelect",
                        buttonText = "选择直播厅",
                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                        {
                            new ModelBasic.EmtDataSelect.ColItem("yy_name")
                            {
                                width = "120",
                                title = "运营",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("tg_name")
                            {
                                width = "120",
                                title = "厅管",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("ting_name")
                            {
                                width = "140",
                                title = "直播厅",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("join_rate")
                            {
                                width = "100",
                                title = "补人率",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("stay_rate")
                            {
                                width = "100",
                                title = "留人率",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("weight")
                            {
                                width = "100",
                                title = "权重值",
                                edit = "text",
                            }
                        }
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    if (dtoReqData.l_weight == null) throw new WeicodeException("请选择直播厅");
                    foreach (var item in dtoReqData.l_weight)
                    {
                        if (item.weight.IsNullOrEmpty() || item.weight.Equals("null")) throw new WeicodeException("请输入权重值");

                        item.w_type = ModelDb.p_join_new_weight.w_type_enum.厅.ToSByte().ToString();
                        lSql.Add(item.ToModel<ModelDb.p_join_new_weight>().InsertOrUpdateTran($"ting_sn = '{item.ting_sn}' and w_type = {ModelDb.p_join_new_weight.w_type_enum.厅.ToSByte()}"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    // 更新补人申请表权重值
                    new ServiceFactory.JoinNew().UpdateWeight();
                    return result;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    /// <summary>
                    /// 数据集合
                    /// </summary>
                    public List<p_join_new_weight> l_weight { get; set; }
                }
                public class p_join_new_weight
                {
                    /// <summary>
                    /// 租户id
                    /// </summary>
                    public Nullable<int> tenant_id { get; set; }
                    /// <summary>
                    /// 所属运营用户编号
                    /// </summary>
                    public string yy_user_sn { get; set; }
                    /// <summary>
                    /// 所属厅管用户编号
                    /// </summary>
                    public string tg_user_sn { get; set; }
                    /// <summary>
                    /// 厅sn
                    /// </summary>
                    public string ting_sn { get; set; }
                    /// <summary>
                    /// 权重值
                    /// </summary>
                    public string weight { get; set; }
                    /// <summary>
                    /// 权重类型
                    /// </summary>
                    public string w_type { get; set; }
                }
                #endregion
            }
            #endregion

            #region 选择直播厅表单
            /// <summary>
            /// 选择直播厅表单
            /// </summary>
            public class TingSelect
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");

                    pageModel.listFilter = GetListFilter(req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.listFilter.isExport = true;
                    pageModel.dataSelect.selectEvent.cbSelected = ModelBasic.EmtDataSelect.reloadListByData();
                    return pageModel;
                }
                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter(req);
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "140px",
                        placeholder = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        },
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "140px",
                        placeholder = "厅管账号",
                        options = new Dictionary<string, string>(),
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtLabel("label1")
                    {
                        defaultValue = "补人率：",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInputNumber("rate_s")
                    {
                        width = "90px",
                        placeholder = "开始",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtLabel("label2")
                    {
                        defaultValue = "-",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInputNumber("rate_e")
                    {
                        width = "90px",
                        placeholder = "结束",
                    });
                    return listFilter;
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
                    result.data = new ServiceFactory.RelationService().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new EmtButtonGroup("");
                    return buttonGroup;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay(req);
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_name")
                    {
                        text = "运营",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_name")
                    {
                        text = "厅管",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "130",
                        minWidth = "130",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("apply_zb_count")
                    {
                        text = "提交人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("real_zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("join_rate")
                    {
                        text = "补人率",
                        width = "100",
                        minWidth = "100",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("quit_zb_count")
                    {
                        text = "流失人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("stay_rate")
                    {
                        text = "留人率",
                        width = "100",
                        minWidth = "100",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("weight")
                    {
                        text = "权重值",
                        width = "80",
                        minWidth = "80",
                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : PageList.Req
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
                    string where = $"status != {ModelDb.user_info_tg.status_enum.逻辑删除.ToSByte()}";

                    //查询条件
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    if (!reqJson.GetPara("tg_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{reqJson.GetPara("tg_user_sn")}'";
                    }

                    if (!reqJson.GetPara("rate_s").IsNullOrEmpty())
                    {
                        where += $@" and join_rate >= {reqJson.GetPara("rate_s")}";
                    }
                    if (!reqJson.GetPara("rate_e").IsNullOrEmpty())
                    {
                        where += $@" and join_rate <= {reqJson.GetPara("rate_e")}";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by yy_user_sn,tg_user_sn"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_tg, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_tg
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public ModelDb.p_join_new_weight p_join_new_weight
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_weight>($"ting_sn = '{ting_sn}' and w_type = {ModelDb.p_join_new_weight.w_type_enum.厅.ToSByte()}", false);
                        }
                    }
                    public string apply_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_apply>("COALESCE(sum(zb_count), 0)", $"ting_sn = '{ting_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0];
                        }
                    }
                    public string real_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where ting_sn = '{ting_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0];
                        }
                    }
                    public string zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}')")[0];
                        }
                    }
                    public string quit_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"ting_sn = '{ting_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}') and not exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {ModelDb.user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0];
                        }
                    }
                    public int? weight
                    {
                        get
                        {
                            return p_join_new_weight.IsNullOrEmpty() ? 0 : p_join_new_weight.weight;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 团队补人权重新增
            /// <summary>
            /// 团队补人权重新增
            /// </summary>
            public class YyWeightPost
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
                    formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_weight")
                    {
                        title = "权重信息",
                        selectUrl = "/JoinNew/Weight/YySelect",
                        buttonText = "选择运营团队",
                        buttonAddOneText = null,
                        colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                        {
                            new ModelBasic.EmtDataSelect.ColItem("username")
                            {
                                width = "120",
                                title = "运营团队",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("attach2")
                            {
                                width = "100",
                                title = "补人率",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("attach3")
                            {
                                width = "100",
                                title = "留人率",
                            },
                            new ModelBasic.EmtDataSelect.ColItem("weight")
                            {
                                width = "100",
                                title = "权重值",
                                edit = "text",
                            }
                        }
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    if (dtoReqData.l_weight == null) throw new WeicodeException("请选择运营团队");
                    foreach (var item in dtoReqData.l_weight)
                    {
                        if (item.weight.IsNullOrEmpty() || item.weight.Equals("null")) throw new WeicodeException("请输入权重值");

                        item.yy_user_sn = item.user_sn;
                        item.w_type = ModelDb.p_join_new_weight.w_type_enum.运营.ToSByte().ToString();
                        lSql.Add(item.ToModel<ModelDb.p_join_new_weight>().InsertOrUpdateTran($"yy_user_sn = '{item.user_sn}' and w_type = {ModelDb.p_join_new_weight.w_type_enum.运营.ToSByte()}"));
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    // 更新补人申请表权重值
                    new ServiceFactory.JoinNew().UpdateWeight();
                    return result;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    /// <summary>
                    /// 数据集合
                    /// </summary>
                    public List<p_join_new_weight> l_weight { get; set; }
                }
                public class p_join_new_weight
                {
                    /// <summary>
                    /// 租户id
                    /// </summary>
                    public Nullable<int> tenant_id { get; set; }
                    /// <summary>
                    /// 运营编号
                    /// </summary>
                    public string yy_user_sn { get; set; }
                    /// <summary>
                    /// 用户编号
                    /// </summary>
                    public string user_sn { get; set; }
                    /// <summary>
                    /// 权重值
                    /// </summary>
                    public string weight { get; set; }
                    /// <summary>
                    /// 权重类型
                    /// </summary>
                    public string w_type { get; set; }
                }
                #endregion
            }
            #endregion

            #region 选择运营团队表单
            /// <summary>
            /// 选择运营团队表单
            /// </summary>
            public class YySelect
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");

                    pageModel.listFilter = GetListFilter(req);
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.listFilter.isExport = true;
                    pageModel.dataSelect.selectEvent.cbSelected = ModelBasic.EmtDataSelect.reloadListByData();
                    return pageModel;
                }
                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter(req);
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "140px",
                        placeholder = "运营团队",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv()
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtLabel("label1")
                    {
                        defaultValue = "补人率：",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInputNumber("rate_s")
                    {
                        width = "90px",
                        placeholder = "开始",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtLabel("label2")
                    {
                        defaultValue = "-",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInputNumber("rate_e")
                    {
                        width = "90px",
                        placeholder = "结束",
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new EmtButtonGroup("");
                    return buttonGroup;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay(req);
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "运营团队",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("apply_zb_count")
                    {
                        text = "提交人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("real_zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("attach2")
                    {
                        text = "补人率",
                        width = "100",
                        minWidth = "100",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_count")
                    {
                        text = "已补人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("quit_zb_count")
                    {
                        text = "流失人数",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("attach3")
                    {
                        text = "留人率",
                        width = "100",
                        minWidth = "100",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("weight")
                    {
                        text = "权重值",
                        width = "80",
                        minWidth = "80",
                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : PageList.Req
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
                    string where = $"status != {ModelDb.user_info_tg.status_enum.逻辑删除.ToSByte()} and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}";

                    //查询条件
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    if (!reqJson.GetPara("rate_s").IsNullOrEmpty())
                    {
                        where += $@" and attach2 >= {reqJson.GetPara("rate_s")}";
                    }
                    if (!reqJson.GetPara("rate_e").IsNullOrEmpty())
                    {
                        where += $@" and attach2 <= {reqJson.GetPara("rate_e")}";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where
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
                    public ModelDb.p_join_new_weight p_join_new_weight
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_weight>($"yy_user_sn = '{user_sn}' and w_type = {ModelDb.p_join_new_weight.w_type_enum.运营.ToSByte()}", false);
                        }
                    }
                    public string apply_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_apply>("COALESCE(sum(zb_count), 0)", $"yy_user_sn = '{user_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}'")[0];
                        }
                    }
                    public string real_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tg_need_id in (select id from p_join_apply where yy_user_sn = '{user_sn}' and status <= {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd")}') and (status = {ModelDb.p_join_new_info.status_enum.等待培训.ToSByte()} or status = {ModelDb.p_join_new_info.status_enum.补人完成.ToSByte()})")[0];
                        }
                    }
                    public string zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"yy_user_sn = '{user_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd")}')")[0];
                        }
                    }
                    public string quit_zb_count
                    {
                        get
                        {
                            return DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"yy_user_sn = '{user_sn}' and id in (select user_info_zb_id from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-15).ToString("yyyy-MM-dd")}') and not exists (select 1 from user_info_zhubo where dou_username = p_join_new_info.dou_username and tg_dangwei = p_join_new_info.tg_dangwei and exists (select 1 from user_info_zhubo_log where user_info_zb_sn = user_info_zhubo.user_info_zb_sn and c_type = {ModelDb.user_info_zhubo_log.c_type_enum.入职.ToSByte()} and create_time <= (select DATE_ADD(max(create_time), INTERVAL 5 DAY) from p_join_new_info_log where c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and create_time >= '{DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd")}' and create_time <= '{DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd")}' and user_info_zb_id = p_join_new_info.id)))")[0];
                        }
                    }
                    public int? weight
                    {
                        get
                        {
                            return p_join_new_weight.weight.IsNullOrEmpty() ? 0 : p_join_new_weight.weight;
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}
