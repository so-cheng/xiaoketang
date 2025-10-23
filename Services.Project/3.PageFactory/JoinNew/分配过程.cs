using Newtonsoft.Json.Linq;
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
    /// 分配流程模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            #region 外宣主管-档表申请列表
            public class TGApplyZbList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("PageList");

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
                    top += $@"            <div class=""layui-icon layui-icon-home"" style=""font-size: 20px; color: #1E9FFF; margin-right: 10px;float: left;""></div>";
                    top += $@"            <div>";
                    top += $@"              <div class=""text-muted"">公会补人率{rate}</div>";
                    top += $@"            </div>";
                    top += $@"          </div>";
                    top += $@"        </div>";
                    top += $@"      </div>";

                    top += $@"    </div>";
                    top += $@"</div>";

                    // 设置tab页
                    top += $@"<div class=""layui-tab layui-tab-brief"">";
                    top += $@"   <ul class=""layui-tab-title"">";
                    top += $@"       <li {(req.completeStatus == -1 ? @"class=""layui-this""" : "")} lay-id=""0"" onclick=""location.href='?completeStatus=-1'"">待分配</li>";
                    top += $@"       <li {(req.completeStatus == 0 ? @"class=""layui-this""" : "")} lay-id=""1"" onclick=""location.href='?completeStatus={ModelDb.p_join_apply_item.status_enum.未完成.ToInt()}'"">未完成</li>";
                    top += $@"       <li {(req.completeStatus == 1 ? @"class=""layui-this""" : "")} lay-id=""2"" onclick=""location.href='?completeStatus={ModelDb.p_join_apply_item.status_enum.已完成.ToInt()}'"">已完成</li>";
                    top += $@"       <li {(req.completeStatus == 10 ? @"class=""layui-this""" : "")} lay-id=""3"" onclick=""location.href='?completeStatus=10'"">超时关闭</li>";
                    top += $@"   </ul>";
                    top += $@"</div>";

                    pageModel.topPartial = new List<ModelBase>
                    {
                        new ModelBasic.EmtHtml("html_top")
                        {
                            Content = top
                        }
                    };

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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtInput("apply_sn")
                    {
                        width = "250px",
                        placeholder = "补人单号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "130px",
                        placeholder = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelectFull("ting_sn")
                    {
                        width = "140px",
                        placeholder = "直播厅",
                        options = new List<ModelDoBasic.Option>()
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("tg_username")
                    {
                        width = "100px",
                        placeholder = "厅管",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        width = "130px",
                        placeholder = "厅名",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        width = "100px",
                        placeholder = "男女厅",
                        title = "男女厅",
                        options = new Dictionary<string, string>
                    {
                        { "男","男"},
                        { "女","女"},
                    },
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("dangwei")
                    {
                        width = "140px",
                        placeholder = "档位",
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.档位时段)
                    });
                    //listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    //{
                    //    mold = EmtTimeSelect.Mold.date_range,
                    //    placeholder = "申请时间",
                    //    title = "申请时间",
                    //    defaultValue = req.dateRange
                    //});
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("last_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "最近补人时间",
                        title = "最近补人时间"
                    });
                    return listFilter;
                }

                /// <summary>
                /// 获取厅管树形结构
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
                    return result;
                }

                /// <summary>
                /// 扩展按钮
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "190";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50,
                        attachPara = new Dictionary<string, object>
                        {
                            {"complete_status", req.completeStatus },
                        }
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time_text")
                    {
                        text = "申请时间",
                        width = "110",
                        minWidth = "110",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_text")
                    {
                        text = "所属运营",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_name")
                    {
                        text = "厅管用户名",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("join_rate")
                    {
                        text = "补人率",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("last_time")
                    {
                        text = "最近补人时间",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dangwei_txt")
                    {
                        text = "申请档位",
                        width = "270"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请数",
                        width = "90",
                        minWidth = "90",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "待分配",
                        width = "90",
                        minWidth = "90",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("recruited_count")
                    {
                        text = "待入库",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=3&apply_item_id={{d.id}}",
                            title = "待入库主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("put_count")
                    {
                        text = "待拉群",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=4&apply_item_id={{d.id}}",
                            title = "待拉群主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_count")
                    {
                        text = "待培训",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=11&apply_item_id={{d.id}}",
                            title = "待培训主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("training_zb_count")
                    {
                        text = "已完成",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=0&apply_item_id={{d.id}}",
                            title = "已完成主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        text = "流失数",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=9&apply_item_id={{d.id}}",
                            title = "流失主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("other_count")
                    {
                        text = "其他",
                        width = "80",
                        minWidth = "80",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=-1&apply_item_id={{d.id}}",
                            title = "其他主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_sn")
                    {
                        text = "补人单号",
                        width = "250",
                        minWidth = "250"
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"MxChooseZbPost?completeStatus={req.completeStatus}",
                            field_paras = "apply_item_id=id"
                        },
                        text = "补人",
                        hideWith =
                        {
                            compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.等于,
                            field = "status",
                            value = ModelDb.p_join_apply_item.status_enum.已完成.ToSByte().ToString()
                        },
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "apply_item_id=id"
                        },
                        text = "主播明细"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/ApproveApplication/Log",
                            field_paras = "apply_sn"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单是否完成，-1:未分配,0:未完成,1:已完成
                    /// </summary>
                    public int completeStatus { get; set; } = 0;

                    /// <summary>
                    /// 时间范围
                    /// </summary>
                    public string dateRange { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有的审批申请
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    var complete_status = reqJson.GetPara("complete_status");
                    var tg_username = reqJson.GetPara("tg_username");
                    var ting_name = reqJson.GetPara("ting_name");
                    if (complete_status.IsNullOrEmpty()) throw new WeicodeException("请选择tab页");

                    //2.筛选
                    string where = $"1=1";
                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty())
                    {
                        where += $" and p_join_apply.apply_sn = '{reqJson.GetPara("apply_sn")}'";
                    }
                    switch (complete_status.ToInt())
                    {
                        case -1:// 未分配
                            where += $" and p_join_apply.status = {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()} and p_join_apply_item.unsupplement_count > 0";
                            break;
                        case (int)ModelDb.p_join_apply_item.status_enum.未完成:
                            where += $" and p_join_apply.status = {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()} and p_join_apply_item.status = {ModelDb.p_join_apply_item.status_enum.未完成.ToInt()}";
                            break;
                        case (int)ModelDb.p_join_apply_item.status_enum.已完成:
                            where += $" and p_join_apply_item.status = {ModelDb.p_join_apply_item.status_enum.已完成.ToInt()} and p_join_apply_item.training_zb_count = p_join_apply_item.zb_count";
                            break;
                        case 10:// 超时关闭
                            where += $" and p_join_apply_item.status = {ModelDb.p_join_apply_item.status_enum.已完成.ToInt()} and p_join_apply_item.training_zb_count < p_join_apply_item.zb_count";
                            break;
                    };

                    if (!tg_username.IsNullOrEmpty()) where += $" and p_join_apply.tg_user_sn in (select user_sn from user_base where name like '%{tg_username}%')";
                    if (!ting_name.IsNullOrEmpty()) where += $" and p_join_apply.ting_sn in (select ting_sn from user_info_tg where ting_name like '%{ting_name}%')";
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and p_join_apply.yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }
                    if (!reqJson.GetPara("ting_sn").IsNullOrEmpty())
                    {
                        where += $" and p_join_apply.ting_sn = '{reqJson.GetPara("ting_sn")}'";
                    }
                    if (!reqJson.GetPara("tg_sex").IsNullOrEmpty())
                    {
                        where += $" and p_join_apply.tg_sex = '{reqJson.GetPara("tg_sex")}'";
                    }
                    if (!reqJson.GetPara("dangwei").IsNullOrEmpty())
                    {
                        where += $" and dangwei = '{reqJson.GetPara("dangwei")}'";
                    }

                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and p_join_apply.create_time >= '{dateRange.date_range_s}' and p_join_apply.create_time <= '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }

                    if (!reqJson.GetPara("last_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("last_time"), 0);
                        where += $" and p_join_apply_item.last_time >= '{dateRange.date_range_s}' and p_join_apply_item.last_time <= '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }

                    //3.获取所有审批的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        fields = "p_join_apply.yy_user_sn, p_join_apply.tg_user_sn, p_join_apply.ting_sn, p_join_apply_item.*",
                        on = "p_join_apply.apply_sn = p_join_apply_item.apply_sn",
                        where = where,
                        orderby = "order by p_join_apply.weight desc,p_join_apply.join_rate,p_join_apply.stay_rate desc,p_join_apply_item.create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply, ModelDb.p_join_apply_item, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply_item
                {
                    public string yy_user_sn { get; set; }
                    public string yy_text
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(this.yy_user_sn).username;
                        }
                    }
                    public string tg_user_sn { get; set; }
                    /// 厅管用户名
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string ting_sn { get; set; }
                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string create_time_text
                    {
                        get
                        {
                            return create_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public ModelDb.p_join_apply p_join_apply
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'");
                        }
                    }
                    public string dangwei_txt
                    {
                        get
                        {
                            var count_text = "";
                            var count = DoMySql.FindField<ModelDb.p_join_new_info>("count(1)", $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} AND STATUS = {ModelDb.p_join_new_info.status_enum.等待分配.ToSByte()} AND sessions LIKE '%{dangwei}%' AND zb_sex = '{p_join_apply.tg_sex}'")[0];
                            if (count.ToInt() > 0)
                            {
                                count_text = $"<span style='color:red;'>{count}</span>";
                            }
                            else
                            {
                                count_text = count;
                            }
                            return $"{new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), dangwei)}(剩余:{count_text}人)";
                        }
                    }
                    public string join_rate
                    {
                        get
                        {
                            return p_join_apply.join_rate + "%";
                        }
                    }
                }
                #endregion ListData
            }
            #endregion

            #region 外宣主管-补人分配操作
            public class WX_ChooseZbList
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
                    pageModel.listDisplay = GetListDisplay(req);

                    return pageModel;
                }

                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    var p_join_apply_item = DoMySql.FindEntityById<ModelDb.p_join_apply_item>(req.apply_item_id, false);
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("sessions")
                    {
                        defaultValue = p_join_apply_item.dangwei,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("apply_item_id")
                    {
                        defaultValue = req.apply_item_id.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("ting_id")
                    {
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{p_join_apply_item.apply_sn}'").ting_sn).id.ToString()
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        placeholder = "审批状态",
                        options = new Dictionary<string, string>
                        {
                            {"未审批", ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString()},
                            {"已审批", ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString()},
                        },
                        defaultValue = ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString(),
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
                    },
                        placeholder = "主播分级",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        width = "140px",
                        placeholder = "微信账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "140px",
                        placeholder = "抖音账号",
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "100";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_user_sn")
                    {
                        text = "tg_user_sn",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level_text")
                    {
                        width = "60",
                        minWidth = "60",
                        text = "分级",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level_time_text")
                    {
                        width = "90",
                        text = "分级时间",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        width = "160",
                        text = "微信昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        width = "160",
                        text = "微信账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        width = "160",
                        text = "抖音账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("anchor_id")
                    {
                        width = "130",
                        text = "抖音作者id",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        width = "60",
                        text = "年龄",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        width = "140",
                        text = "地区",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        width = "260",
                        text = "说明",
                    });

                    #endregion 显示列

                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Share/Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            new EmtModel.ButtonItem("")
                            {
                                text = "补人",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = ApproveAction,
                                    attachPara= new Dictionary<string, object>
                                    {
                                        {"ting_sn",req.ting_sn},
                                        {"apply_item_id",req.apply_item_id},
                                        {"apply_id",req.apply_id},
                                    },
                                    //returnReload = EmtModel.ButtonItem.EventCsAction.ReturnReload.parent
                                },
                            }
                        }
                    });

                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction ApproveAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.GetPara();
                    var zbList = dtoReqData["check_data"].ToModel<List<ModelDb.p_join_new_info>>();
                    var ting_sn = dtoReqData["ting_sn"].ToString();
                    var apply_id = dtoReqData["apply_id"].ToInt();
                    var apply_item_id = dtoReqData["apply_item_id"].ToInt();
                    //判断补人是否超过申请上限
                    var p_join_apply_item = DoMySql.FindEntityById<ModelDb.p_join_apply_item>(apply_item_id);
                    var p_join_new_infos = DoMySql.FindList<ModelDb.p_join_new_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_dangwei = {apply_item_id} and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte()}");
                    if (p_join_new_infos.Count + zbList.Count > p_join_apply_item.zb_count.ToInt())
                    {
                        throw new Exception("补人已超过需求上限");
                    }

                    //汇总操作数据对申请单进行更新
                    var p_join_apply = DoMySql.FindEntityById<ModelDb.p_join_apply>(apply_id);
                    p_join_apply.recruited_count = p_join_apply.recruited_count.ToInt() + zbList.Count;
                    lSql.Add(p_join_apply.UpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);

                    //调用补人操作
                    new ServiceFactory.JoinNew().SupplementAction(zbList, ting_sn, apply_id, apply_item_id);

                    //最近补人时间
                    p_join_apply_item.last_time = DateTime.Now;
                    p_join_apply_item.Update();

                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(apply_item_id);
                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                {
                    public int apply_item_id { get; set; }
                    public int apply_id { get; set; }
                    public string ting_sn { get; set; }
                }
                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有萌新主播
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"p_join_new_info.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = '{ModelDb.p_join_new_info.status_enum.等待分配.ToInt()}'";

                    if (!reqJson.GetPara("sessions").IsNullOrEmpty())
                    {
                        where += $" and sessions like '%{reqJson.GetPara("sessions")}%'";
                    }
                    if (!reqJson.GetPara("apply_item_id").IsNullOrEmpty())
                    {
                        var p_join_apply = DoMySql.FindListBySql<ModelDb.p_join_apply>($"select t2.* from p_join_apply_item t1 left join p_join_apply t2 on t1.apply_sn = t2.apply_sn where t1.id = '{reqJson.GetPara("apply_item_id")}'");
                        if (!p_join_apply.IsNullOrEmpty())
                        {
                            where += $" and zb_sex = '{p_join_apply[0].tg_sex}'";
                        }
                    }
                    //筛选
                    if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                    {
                        where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("zb_level").IsNullOrEmpty())
                    {
                        where += $" and zb_level = '{reqJson.GetPara("zb_level")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        fields = "p_join_new_info.id,p_join_new_info.tenant_id,p_join_new_info_sn,user_sn,tg_user_sn,ting_sn,p_join_new_info.yy_user_sn,qy_sn,mx_sn,old_tg_user_sn,old_tg_username,tg_need_id,tg_dangwei,age,marriage,child,birthday,star_sign,talent,sound_card,way,mobile,devices_num,devices_num,mbti,is_low,province,city,experience,job,revenue,zb_sex,openid,wechat_nickname,wechat_username,dou_username,dou_nickname,anchor_id,upload_records,sessions,full_or_part,term,zb_level,zb_level_time,note,quality,supplement_sort,qun,no_share,status,is_fast,p_join_new_info.create_time,p_join_new_info.modify_time,p_join_new_citys.priority address",
                        // 运营团队-补人城市优先级
                        on = $"SUBSTRING_INDEX(p_join_new_info.city, '市', 1) = p_join_new_citys.name and p_join_new_citys.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and CONCAT(',', p_join_new_citys.ting_ids, ',') LIKE '%,{reqJson.GetPara("ting_id")},%'",
                        where = where,
                        orderby = $" ORDER BY COALESCE (p_join_new_citys.priority, 999),zb_level_time IS NULL,zb_level_time,zb_level"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ModelDb.p_join_new_citys, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string zb_level_time_text
                    {
                        get
                        {
                            if (zb_level_time.IsNullOrEmpty()) return "";
                            var difference = (DateTime.Now - zb_level_time.ToDateTime()).Days;
                            if (difference == 0) return "今天";
                            if (difference == 1) return "昨天";
                            return $"{difference}天前";
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return (address.IsNullOrEmpty() ? "" : "<span style='color:red;'>(优先)</span>") + province + city;
                        }
                    }

                    public string zb_level_text
                    {
                        get
                        {
                            string result = zb_level;
                            if (is_fast == ModelDb.p_join_new_info.is_fast_enum.加急.ToSByte())
                            {
                                result = zb_level + "(加急)";
                            }
                            return result;
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 外宣主管-加急补人主播列表
            public class WX_FastZbList
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
                    pageModel.listDisplay = GetListDisplay(req);

                    return pageModel;
                }

                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"A","A"},
                        {"B","B"},
                    },
                        placeholder = "主播分级",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_username")
                    {
                        placeholder = "微信账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        placeholder = "抖音账号",
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.isOpenNumbers = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_user_sn")
                    {
                        text = "tg_user_sn",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_level")
                    {
                        width = "100",
                        minWidth = "100",
                        text = "主播分级",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_nickname")
                    {
                        width = "160",
                        text = "微信昵称",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("wechat_username")
                    {
                        width = "160",
                        text = "微信账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dou_username")
                    {
                        width = "160",
                        text = "抖音账号",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("anchor_id")
                    {
                        width = "130",
                        text = "抖音作者id",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("age")
                    {
                        width = "60",
                        text = "年龄",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("address_text")
                    {
                        width = "140",
                        text = "地区",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("note")
                    {
                        width = "280",
                        text = "说明",
                    });

                    #endregion 显示列
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "WxChooseJoinApplyPost",
                            field_paras = "p_join_new_info_id=id"
                        },
                        text = "分厅"
                    });

                    #endregion

                    return listDisplay;
                }

                public class DtoReq
                {

                }
                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有加急主播
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = '{ModelDb.p_join_new_info.status_enum.等待分配.ToInt()}' and is_fast = {ModelDb.p_join_new_info.is_fast_enum.加急.ToSByte()}";

                    //筛选
                    if (!reqJson.GetPara("wechat_username").IsNullOrEmpty())
                    {
                        where += $" and wechat_username like '%{reqJson.GetPara("wechat_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("zb_level").IsNullOrEmpty())
                    {
                        where += $" and zb_level = '{reqJson.GetPara("zb_level")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = $" ORDER BY zb_level"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 选择档表分厅列表
            public class WX_ChooseJoinApplyList
            {
                #region DefaultView

                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("PageList");
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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("p_join_new_info_id")
                    {
                        defaultValue = req.p_join_new_info_id.ToString(),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("apply_sn")
                    {
                        width = "250px",
                        placeholder = "补人单号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("tg_username")
                    {
                        width = "100px",
                        placeholder = "厅管",
                    });

                    var yy_options = new Dictionary<string, string>();
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            });
                            break;
                        case ModelEnum.UserTypeEnum.manager:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                        case ModelEnum.UserTypeEnum.mxer:
                            yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                            break;
                        default:
                            yy_options = new Dictionary<string, string>();
                            break;
                    }
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "100px",
                        title = "所属运营",
                        options = yy_options,
                        placeholder = "运营",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "申请时间",
                        title = "申请时间",
                        defaultValue = req.dateRange
                    });
                    return listFilter;
                }

                /// <summary>
                /// 扩展按钮
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new CtlListDisplay();
                    listDisplay.operateWidth = "190";
                    listDisplay.isOpenNumbers = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time_text")
                    {
                        text = "申请时间",
                        width = "110",
                        minWidth = "110",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_text")
                    {
                        text = "所属运营",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("step_text")
                    {
                        text = "所属阶段",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                    {
                        text = "厅管用户名",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_sex")
                    {
                        text = "厅成员",
                        width = "80",
                        minWidth = "80"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("dangwei_txt")
                    {
                        text = "申请档位",
                        width = "200"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请数",
                        width = "90",
                        minWidth = "90",
                        sort = true
                    });

                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "待分配",
                        width = "90",
                        minWidth = "90",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("recruited_count")
                    {
                        text = "待入库",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=3&apply_item_id={{d.id}}",
                            title = "待入库主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("put_count")
                    {
                        text = "待拉群",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=4&apply_item_id={{d.id}}",
                            title = "待拉群主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_count")
                    {
                        text = "待培训",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=11&apply_item_id={{d.id}}",
                            title = "待培训主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("training_zb_count")
                    {
                        text = "已完成",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=0&apply_item_id={{d.id}}",
                            title = "已完成主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_count")
                    {
                        text = "流失数",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=9&apply_item_id={{d.id}}",
                            title = "流失主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("other_count")
                    {
                        text = "其他",
                        width = "80",
                        minWidth = "80",
                        sort = true,
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=-1&apply_item_id={{d.id}}",
                            title = "其他主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_sn")
                    {
                        text = "补人单号",
                        width = "250",
                        minWidth = "250"
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = AllocationAction,
                            field_paras = "id",
                            attachPara = new Dictionary<string, object>
                            {
                                { "p_join_new_info_id",req.p_join_new_info_id}
                            }
                        },
                        text = "分配"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "apply_item_id=id"
                        },
                        text = "主播明细"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "ApplyLog",
                            field_paras = "apply_sn"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

                    return listDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 时间范围
                    /// </summary>
                    public string dateRange { get; set; }
                    public int p_join_new_info_id { get; set; }
                }

                /// <summary>
                /// 补人分配操作
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction AllocationAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var lSql = new List<string>();
                    var dtoReqData = req.GetPara();
                    var apply_item_id = dtoReqData["id"].ToInt();

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(dtoReqData["p_join_new_info_id"].ToInt());
                    if (p_join_new_info.status != ModelDb.p_join_new_info.status_enum.等待分配.ToSByte() && p_join_new_info.status != ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte()) throw new Exception("当前不可分配");
                    var zbList = new List<ModelDb.p_join_new_info>();
                    zbList.Add(p_join_new_info);

                    //判断补人是否超过申请上限
                    var p_join_apply_item = DoMySql.FindEntityById<ModelDb.p_join_apply_item>(apply_item_id);
                    var p_join_new_infos = DoMySql.FindList<ModelDb.p_join_new_info>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_dangwei = {apply_item_id} and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte()}");
                    if (p_join_new_infos.Count + zbList.Count > p_join_apply_item.zb_count.ToInt())
                    {
                        throw new Exception("补人已超过需求上限");
                    }

                    //汇总操作数据对申请单进行更新
                    var p_join_apply = DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{p_join_apply_item.apply_sn}'");
                    p_join_apply.recruited_count = p_join_apply.recruited_count.ToInt() + zbList.Count;
                    lSql.Add(p_join_apply.UpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);

                    //调用补人操作
                    new ServiceFactory.JoinNew().SupplementAction(zbList, p_join_apply.ting_sn, p_join_apply.id, apply_item_id);

                    //最近补人时间
                    p_join_apply_item.last_time = DateTime.Now;
                    p_join_apply_item.Update();

                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(apply_item_id);
                    return info;
                }

                #endregion DefaultView

                #region ListData
                /// <summary>
                /// 获取所有的审批申请
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    var tg_username = reqJson.GetPara("tg_username");
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(reqJson.GetPara("p_join_new_info_id").ToInt());

                    //2.筛选（根据档位和性别分厅）
                    string where = $"p_join_apply_item.status = {ModelDb.p_join_apply_item.status_enum.未完成.ToSByte()} and p_join_apply_item.unsupplement_count > 0 and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()}) and p_join_apply_item.dangwei in ({p_join_new_info.sessions}) and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_sex = '{p_join_new_info.zb_sex}')";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            where += $@" and apply_sn in (select apply_sn from p_join_apply where yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn
                                }
                            })})";
                            break;
                    }

                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty())
                    {
                        where += $" and apply_sn = '{reqJson.GetPara("apply_sn")}'";
                    }
                    if (!tg_username.IsNullOrEmpty()) where += $" and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_username like '%{tg_username}%')";
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}')";
                    }

                    if (!reqJson.GetPara("tg_sex").IsNullOrEmpty())
                    {
                        where += $" and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_sex = '{reqJson.GetPara("tg_sex")}')";
                    }

                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and apply_sn in (select apply_sn from p_join_apply where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and create_time >= '{dateRange.date_range_s}' and create_time <= '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}')";
                    }
                    //3.获取所有审批的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        on = $"SUBSTRING_INDEX('{DoMySql.FindEntityById<ModelDb.p_join_new_info>(reqJson.GetPara("p_join_new_info_id").ToInt()).city}', '市', 1) = p_join_new_citys.name and p_join_new_citys.tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and CONCAT(',', p_join_new_citys.ting_ids, ',') LIKE CONCAT('%,', (select id from user_info_tg where ting_sn = (select ting_sn from p_join_apply where apply_sn = p_join_apply_item.apply_sn)), ',%')",
                        where = where + $"",
                        orderby = " order by COALESCE (p_join_new_citys.priority, 999),p_join_apply_item.create_time",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply_item, ModelDb.p_join_new_citys, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply_item
                {
                    public ModelDb.p_join_apply p_join_apply
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'");
                        }
                    }
                    public string tg_sex
                    {
                        get
                        {
                            return p_join_apply.tg_sex;
                        }
                    }
                    public string yy_text
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(p_join_apply.yy_user_sn).username;
                        }
                    }
                    /// 厅管用户名
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_join_apply.tg_user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn).ting_name;
                        }
                    }
                    public string step_text
                    {
                        get
                        {
                            var jiezou = DoMySql.FindEntity<ModelDb.jiezou_detail>($"ting_sn = '{p_join_apply.ting_sn}' and data_time = '{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}'", false);

                            return jiezou.step > 0 ? jiezou.step + "阶段" : "-";
                        }
                    }
                    public string create_time_text
                    {
                        get
                        {
                            return create_time.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string dangwei_txt
                    {
                        get
                        {
                            return new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), dangwei); ;
                        }
                    }
                }
                #endregion ListData
            }
            #endregion

            #region  外宣主管-等待入库学员列表
            public class WaitPut
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
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
                    listDisplay.operateWidth = "120px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "200px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtInputSelect($"cause")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"(女厅)培训后观察觉得不合适","(女厅)培训后观察觉得不合适"},
                                            {"(女厅)宝妈觉得有小孩不方便","(女厅)宝妈觉得有小孩不方便"},
                                            {"(女厅)上麦第一天不说话然后第二天就拉黑","(女厅)上麦第一天不说话然后第二天就拉黑"},
                                            {"(女厅)请长假或者刚开始就要请假","(女厅)请长假或者刚开始就要请假"},
                                            {"(女厅)无缘无故拉黑","(女厅)无缘无故拉黑"},
                                            {"(男厅)工作原因时间冲突","(男厅)工作原因时间冲突"},
                                            {"(男厅)上麦发现不合适","(男厅)上麦发现不合适"},
                                            {"(男厅)环境条件不允许","(男厅)环境条件不允许"},
                                            {"(男厅)偏小年龄爱打游戏失踪","(男厅)偏小年龄爱打游戏失踪"},
                                        },
                                        placeholder="流失原因",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_sn")
                    {
                        text = "萌新sn",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });
                    #endregion
                    #region 批量操作列

                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("group")
                    {
                        text = "批量操作",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                        {
                            func = PutAction,
                        },
                        disabled = true,

                        buttonItems = new List<EmtModel.ButtonItem>
                        {
                            new EmtModel.ButtonItem("put")
                            {
                                text = "批量入库",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = PutAction,
                                },
                            },
                            new EmtModel.ButtonItem("mark")
                            {
                                text = "标记错误",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                                eventOpenLayer=new EmtModel.ButtonItem.EventOpenLayer
                                {
                                    url="Mark"
                                },
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = Mark,
                                },
                            }
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
                #region 异步请求处理
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info.Delete();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待入库, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    return info;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                #endregion
                #region 批量操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PutAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var list = DoMySql.FindList<ModelDb.p_join_new_info>($"id in ({dtoReqData.ids})");
                    foreach (var item in list)
                    {
                        item.status = ModelDb.p_join_new_info.status_enum.等待拉群.ToInt().ToSByte();
                        lSql.Add(item.UpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    foreach (var item in list)
                    {
                        // 计算申请档位明细人数
                        new ServiceFactory.JoinNew().JisuanCount(item.tg_dangwei);
                        //记录分配时间日志
                        new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.入库, item.id, ModelDb.p_join_new_info.status_enum.等待入库, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了入库");
                    }

                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_info
                {
                    public string ids { get; set; }
                }

                /// <summary>
                /// 标记错误
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction Mark(JsonRequestAction req)
                {
                    var dtoReqData = req.GetPara();
                    List<string> lSql = new List<string>();
                    var p_join_new_info = dtoReqData["check_data"].ToModel<List<ModelDb.p_join_new_info>>();

                    foreach (var item in p_join_new_info)
                    {
                        if (!item.dou_username.Contains("(错误)"))
                        {
                            item.dou_username = "(错误)" + item.dou_username;
                            lSql.Add(item.UpdateTran());
                        }
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    //2.获取邀约记录
                    where += "order by create_time desc";
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string supplement_time
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_info_log>($"user_info_zb_id = '{this.id}' and c_type = '{ModelDb.p_join_new_info_log.c_type_enum.分配.ToInt()}'").create_time.ToString();
                        }
                    }

                }
                #endregion
            }
            #endregion

            #region 外宣主管-标记错误
            public class MarkPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
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
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
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
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ids")
                    {
                        title = "ids",
                        defaultValue = req.ids,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("cause")
                    {
                        title = "错误原因",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            {"退会","退会" },
                            {"抖音号错误","抖音号错误" },
                            {"已有对接厅","已有对接厅" },
                            {"未签约","未签约" },
                        },
                    });

                    formDisplay.formItems.Add(new EmtInput("old_tg_username")
                    {
                        title = "原对接厅"
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public string ids { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    var ids = req.GetPara("ids").Split(',');
                    foreach (var id in ids)
                    {
                        var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(id.ToInt());
                        p_join_new_info.dou_username = "(" + req.GetPara("cause") + ")" + p_join_new_info.dou_username;
                        switch (req.GetPara("cause"))
                        {
                            case "退会":
                                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.改抖音号.ToSByte();
                                break;
                            case "抖音号错误":
                                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.改抖音号.ToSByte();
                                break;
                            case "已有对接厅":
                                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.有对接厅.ToSByte();
                                p_join_new_info.old_tg_username = req.GetPara("old_tg_username");
                                break;
                            case "未签约":
                                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.改抖音号.ToSByte();
                                break;
                            default:
                                p_join_new_info.status = ModelDb.p_join_new_info.status_enum.改抖音号.ToSByte();
                                break;
                        }
                        lSql.Add(p_join_new_info.UpdateTran());
                    }

                    DoMySql.ExecuteSqlTran(lSql);
                    foreach (var id in ids)
                    {
                        var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(id.ToInt());
                        // 计算申请档位明细人数
                        new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                        //记录分配时间日志
                        new ServiceFactory.JoinNew().AddJoinNewLog(p_join_new_info.status == ModelDb.p_join_new_info.status_enum.有对接厅.ToSByte() ? ModelDb.p_join_new_info_log.c_type_enum.入库失败 : ModelDb.p_join_new_info_log.c_type_enum.改抖音号, id.ToInt(), ModelDb.p_join_new_info.status_enum.等待入库, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了标记错误：{req.GetPara("cause")}");
                    }
                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion

            #region 萌新-批量分级操作
            public class FastLevel
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.buttonGroup = GetButtonGroup(req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };

                    return pageModel;
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
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("ids")
                    {
                        title = "ids",
                        defaultValue = req.ids,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_level")
                    {
                        title = "主播定级",
                        isRequired = true,
                        options = new Dictionary<string, string>
                    {
                        {"A","A" },
                        {"B","B" },
                    },
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("describe")
                    {
                        Content = @"A级标准为以下<br>
                            声音优质，试音过程自信且培训过程全程主动配合<br>
                            <br>
                            B级标准为以下<br>
                            声音一般但配合度高的<br>
                            试音过程紧张尴尬<br>
                            培训需要催促才会发试音的<br>
                            <br>",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        title = "是否分配",
                        isRequired = true,
                        options = new Dictionary<string, string>
                        {
                            {"暂不分配",ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString()},
                            {"等待分配",ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString() },
                        },
                        defaultValue = ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString()
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    public string ids { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    var ids = req.GetPara("ids").Split(',');
                    foreach (var id in ids)
                    {
                        var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(id.ToInt());
                        p_join_new_info.zb_level = req.GetPara("zb_level");
                        p_join_new_info.zb_level_time = DateTime.Now;
                        p_join_new_info.status = req.GetPara("status").ToSByte();
                        //记录首次主播分级时间
                        new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.分级, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待分级, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播分级");
                        lSql.Add(p_join_new_info.UpdateTran());
                    }

                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion

            #region 萌新-等待拉群列表展示
            public class WaitQun
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
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
                    listDisplay.operateWidth = "170px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "200px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_sn")
                    {
                        text = "萌新sn",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = InQun,
                            field_paras = "ids=id"
                        },
                        style = "",
                        text = "拉群"

                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });
                    #endregion
                    #region 批量操作列

                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("group")
                    {
                        text = "批量拉群",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                        {
                            func = InQun,
                        },
                        disabled = true,
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

                #region 异步请求处理
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info.Delete();
                    return info;
                }

                /// <summary>
                /// 入群
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction InQun(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    string ids = dtoReqData.ids;
                    var result = new JsonResultAction();

                    var p_join_new_info = DoMySql.FindList<ModelDb.p_join_new_info>($" id in ({ids})");
                    foreach (var item in p_join_new_info)
                    {
                        //p_join_new_info更新状态
                        item.status = ModelDb.p_join_new_info.status_enum.等待培训.ToSByte();
                        item.Update();
                        new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.拉群, item.id, ModelDb.p_join_new_info.status_enum.等待拉群, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播拉群");
                        // 计算申请档位明细人数
                        new ServiceFactory.JoinNew().JisuanCount(item.tg_dangwei);
                    }

                    return result;
                }
                public class DtoReqData : ModelDb.p_join_new_info
                {
                    public string ids { get; set; }
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " order by tg_user_sn desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-已拉群列表展示
            public class Qun
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("qun_date_range")
                    {
                        placeholder = "拉群时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
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
                    listDisplay.isHideOperate = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_ting_name")
                    {
                        text = "原对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "200px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qun_time")
                    {
                        text = "拉群时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
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
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"mx_sn = '{new UserIdentityBag().user_sn}' and id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_sn = '{new UserIdentityBag().user_sn}')";
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("qun_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("qun_date_range").ToNullableString(), 0);
                        where += $" and id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_sn = '{new UserIdentityBag().user_sn}' and create_time >= '{dateRange.date_range_s}' and create_time <= '{dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1)}')";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " order by create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string old_ting_name
                    {
                        get
                        {
                            return old_tg_user_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(old_tg_user_sn).ting_name;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return ((status_enum)status).ToString();
                        }
                    }
                    public string qun_time
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_info_log>($"c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = {id}").create_time.ToString();
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-暂未分配列表展示
            public class UnShareList
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("mx_sn")
                    {
                        width = "120px",
                        disabled = true,
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id}' and status='{ModelDb.user_base.status_enum.正常.ToSByte()}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        placeholder = "萌新老师",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "240px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_name")
                    {
                        index = 100,
                        text = "所属萌新老师",
                        width = "120",
                        minWidth = "120",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"zb_level", "zb_level" },
                            },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_status")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"暂不分配",ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString()},
                                            {"等待分配",ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString()},
                                        },
                                        defaultValue = ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString(),
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"l_status","<%=page.l_status.value%>" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs=@"if(page.l_status.value == "+ModelDb.p_join_new_info.status_enum.等待分配.ToSByte()                          +@"){
                                                                page.focus.text(page.l_zb_level.value);
                                                                $('.floatlayer_div').hide();
                                                            }
                                                            else{
                                                                location.reload();
                                                            }",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level_time_text")
                    {
                        index = 250,
                        text = "主播分级时间",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "WxChooseJoinApplyPost",
                            field_paras = "p_join_new_info_id=id"
                        },
                        text = "分厅"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Newcomer/Edit",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Edit"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "layui-btn-normal",
                        text = "锁定",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = LockAction,
                            field_paras = "id",
                        },
                        disabled = true,
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.等于,
                            value = ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte().ToString()
                        },
                        name = "Lock",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "layui-btn-danger",
                        text = "释放",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = ReleaseAction,
                            field_paras = "id",
                        },
                        disabled = true,
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte().ToString()
                        },
                        name = "Release",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "操作记录",
                        name = "Log",
                    });

                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 锁定操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction LockAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(p_join_new_info.id);
                    if (p_join_new_info.status != ModelDb.p_join_new_info.status_enum.等待分配.ToSByte()) throw new WeicodeException("当前不可锁定，请刷新页面");

                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte();
                    p_join_new_info.Update();

                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.中台锁定, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了中台锁定操作");
                    return info;
                }

                /// <summary>
                /// 释放操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction ReleaseAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(p_join_new_info.id);
                    if (p_join_new_info.status != ModelDb.p_join_new_info.status_enum.中台锁定.ToSByte()) throw new WeicodeException("当前不可释放，请刷新页面");

                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待分配.ToSByte();
                    p_join_new_info.Update();

                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.恢复分配, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.中台锁定, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了释放到公海操作");
                    return info;
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("mx_sn").IsNullOrEmpty())
                    {
                        where += $" and mx_sn = '{reqJson.GetPara("mx_sn")}'";
                    }
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string mx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).username;
                        }
                    }
                    public string zb_level_time_text
                    {
                        get
                        {
                            if (zb_level_time.IsNullOrEmpty()) return "";
                            var difference = (DateTime.Now - zb_level_time.ToDateTime()).Days;
                            if (difference == 0) return "今天";
                            if (difference == 1) return "昨天";
                            return $"{difference}天前";
                        }
                    }
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-暂不分配列表展示
            public class UndistributedList
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "240px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Newcomer/Edit",
                            field_paras = "id"
                        },
                        text = "编辑"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = RestoreAllocationAction,
                            field_paras = "id"
                        },
                        style = "",
                        text = "恢复分配"

                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 删除操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info.Delete();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.暂不分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    return info;
                }

                /// <summary>
                /// 恢复分配操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RestoreAllocationAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待分配.ToSByte();
                    p_join_new_info.Update();

                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.恢复分配, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.暂不分配, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了恢复分配");
                    return info;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_join_new_info
                {
                    public string ids { get; set; }
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-改抖音号列表展示
            public class ChangeDyAccountList
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "210px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "ChangeDyAction",
                            field_paras = "id"
                        },
                        text = "修改抖音号"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });
                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 删除操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.改抖音号, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    p_join_new_info.Delete();
                    return info;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-改抖音号操作
            public class ChangeDyAction
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.id.ToInt());

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "ids",
                        colLength = 6,
                        defaultValue = req.id,
                    });

                    formDisplay.formItems.Add(new EmtInput("dou_username")
                    {
                        title = "抖音账号",
                        colLength = 6,
                        defaultValue = p_join_new_info.dou_username,
                        isRequired = true,
                    });
                    formDisplay.formItems.Add(new EmtInput("dou_nickname")
                    {
                        title = "抖音昵称",
                        colLength = 6,
                        defaultValue = p_join_new_info.dou_nickname,
                        isRequired = true,
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public string id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());
                    p_join_new_info.dou_username = req.GetPara("dou_username");
                    p_join_new_info.dou_nickname = req.GetPara("dou_nickname");
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待入库.ToSByte();
                    lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    // 日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.改抖音号, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.改抖音号, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了改抖音号");
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 萌新-有对接厅列表展示
            public class HavingTing
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "170px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("cause")
                    {
                        text = "退回原因",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_tg_username")
                    {
                        index = 150,
                        text = "原对接厅",
                        width = "120",
                        minWidth = "120"
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "ChangeTingAction",
                            field_paras = "id"
                        },
                        text = "转厅"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });
                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 删除操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.有对接厅, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    p_join_new_info.Delete();
                    return info;
                }
                #endregion
                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string cause
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_info_log>($"user_info_zb_id = {id} and last_status = {ModelDb.p_join_new_info_log.last_status_enum.入库失败.ToSByte()} order by create_time desc", false).content;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-有对接厅上传图片操作
            public class ChangeTingAction
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.id.ToInt());

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "ids",
                        colLength = 6,
                        defaultValue = req.id,
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTextarea("upload_records")
                    {
                        title = "上传记录",
                        defaultValue = p_join_new_info.upload_records,
                        disabled = true
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public string id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 背调表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());
                    p_join_new_info.upload_records = req.GetPara("upload_records");
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.入库失败.ToSByte();
                    lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.入库失败, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.有对接厅, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了有对接厅申请提交");
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 萌新-查询归属列表展示
            public class Search
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
                    listFilter.isExport = true;
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("mx_sn")
                    {
                        width = "120px",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id}' and status='{ModelDb.user_base.status_enum.正常.ToSByte()}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        placeholder = "萌新老师",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelSite.EmtCityPicker("address")
                    {
                        title = "所在省市",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "110px",
                        options = new Dictionary<string, string>
                        {
                            {"等待分级",ModelDb.p_join_new_info.status_enum.等待分级.ToInt().ToString()},
                            {"等待分配",ModelDb.p_join_new_info.status_enum.等待分配.ToInt().ToString()},
                            {"等待入库",ModelDb.p_join_new_info.status_enum.等待入库.ToInt().ToString()},
                            {"等待拉群",ModelDb.p_join_new_info.status_enum.等待拉群.ToInt().ToString()},
                            {"等待退回",ModelDb.p_join_new_info.status_enum.等待退回.ToInt().ToString()},
                            {"暂不分配",ModelDb.p_join_new_info.status_enum.暂不分配.ToInt().ToString()},
                            {"中台锁定",ModelDb.p_join_new_info.status_enum.中台锁定.ToInt().ToString()},
                            {"改抖音号",ModelDb.p_join_new_info.status_enum.改抖音号.ToInt().ToString()},
                            {"入库失败",ModelDb.p_join_new_info.status_enum.入库失败.ToInt().ToString()},
                            {"流失",ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt().ToString()},
                            {"有直播厅",ModelDb.p_join_new_info.status_enum.有对接厅.ToInt().ToString()},
                            {"等待培训",ModelDb.p_join_new_info.status_enum.等待培训.ToInt().ToString()},
                            {"补人完成",ModelDb.p_join_new_info.status_enum.补人完成.ToInt().ToString()},
                        },
                        placeholder = "状态",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("is_qun")
                    {
                        width = "100px",
                        placeholder = "是否拉群",
                        options = new Dictionary<string, string>
                        {
                            {"是","是"},
                            {"否","否"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "170px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_name")
                    {
                        index = 100,
                        text = "所属萌新老师",
                        width = "150",
                        minWidth = "150",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "200px",
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"zb_level", "zb_level" },
                            },
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });

                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/Newcomer/Edit",
                            field_paras = "id"
                        },
                        text = "编辑",
                        name = "Edit"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Transforms",
                            field_paras = "id"
                        },
                        disabled = true,
                        text = "转移",
                        name = "Transform",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
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
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("mx_sn").IsNullOrEmpty())
                    {
                        where += $" and mx_sn = '{reqJson.GetPara("mx_sn")}'";
                    }
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("___province").IsNullOrEmpty())
                    {
                        where += $" and province like '%{reqJson.GetPara("___province")}%'";
                    }
                    if (!reqJson.GetPara("___city").IsNullOrEmpty())
                    {
                        where += $" and city like '%{reqJson.GetPara("___city")}%'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        where += $" and status = {reqJson.GetPara("status")}";
                    }
                    if (!reqJson.GetPara("is_qun").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("is_qun"))
                        {
                            case "是":
                                where += $" and id in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_sn = '{new UserIdentityBag().user_sn}')";
                                break;
                            case "否":
                                where += $" and id not in (select user_info_zb_id from p_join_new_info_log where tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_sn = '{new UserIdentityBag().user_sn}')";
                                break;
                        }
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                    public string mx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(mx_sn).username;
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
            }
            #endregion

            #region 萌新主管-转移萌新
            public class Transforms
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("mx_sn")
                    {
                        title = "萌新老师",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}", "username,user_sn"),
                        placeholder = "请选择",
                        isRequired = true,
                    });
                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理
                /// <summary>
                /// 异步请求处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    if (req.GetPara("mx_sn").IsNullOrEmpty()) throw new WeicodeException("请选择萌新");

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());

                    var old_mx_name = new DomainBasic.UserApp().GetInfoByUserSn(p_join_new_info.mx_sn).username;

                    p_join_new_info.mx_sn = req.GetPara("mx_sn");
                    p_join_new_info.Update();

                    // 日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.转移萌新, p_join_new_info.id, ModelDb.p_join_new_info_log.last_status_enum.无, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了转移操作，原萌新:{old_mx_name}");

                    return new JsonResultAction();
                }
                #endregion 异步请求处理
            }
            #endregion

            #region 萌新-等待退回列表展示
            public class WaitBack
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
                    listFilter.isExport = false;
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
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
                    listDisplay.operateWidth = "170px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    #region 显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        text = "期数",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("note")
                    {
                        text = "退回说明",
                        width = "300",
                        minWidth = "300"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        index = 1200,
                        text = "原对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        index = 1300,
                        text = "原对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "ResetPost",
                            field_paras = "id"
                        },
                        text = "同意",
                        name = "BackPost"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "BackCancelPost",
                            field_paras = "id"
                        },
                        text = "拒绝",
                        name = "BackPost"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
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
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"status = {ModelDb.p_join_new_info.status_enum.等待退回.ToSByte()}";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 萌新-退回审批重新分配操作
            public class ResetPost
            {
                #region DefaultView
                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "id",
                        colLength = 6,
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtInput("reason")
                    {
                        title = "实际原因",
                        colLength = 6,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new EmtInput("result")
                    {
                        title = "处理结果",
                        colLength = 6,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new EmtSelect("processing")
                    {
                        title = "处理方式",
                        colLength = 6,
                        isRequired = true,
                        options = new Dictionary<string, string> {
                            {"暂未分配", "暂未分配"},
                            {"流失", "流失"}
                        },
                    });
                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理
                /// <summary>
                /// 异步请求处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    if (req.GetPara("reason").IsNullOrEmpty()) throw new WeicodeException("请输入实际原因");
                    if (req.GetPara("result").IsNullOrEmpty()) throw new WeicodeException("请输入处理结果");
                    if (req.GetPara("processing").IsNullOrEmpty()) throw new WeicodeException("请选择处理方式");

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());
                    var tg_dangwei = p_join_new_info.tg_dangwei;// 用于计算档位明细人数
                    p_join_new_info.note = $"{p_join_new_info.note};实际原因:{req.GetPara("reason")};处理结果:{req.GetPara("result")}";
                    switch (req.GetPara("processing"))
                    {
                        case "暂未分配":
                            p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待分配.ToSByte();
                            p_join_new_info.old_tg_user_sn = p_join_new_info.ting_sn;
                            p_join_new_info.ting_sn = "[null]";
                            p_join_new_info.tg_user_sn = "[null]";
                            p_join_new_info.yy_user_sn = "[null]";
                            p_join_new_info.tg_need_id = 0;
                            p_join_new_info.tg_dangwei = 0;
                            break;
                        case "流失":
                            p_join_new_info.status = ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte();
                            p_join_new_info.no_share = $"流失时间:{DateTime.Now.ToString("yyyy-MM-dd")},原因:{req.GetPara("result")}";
                            break;
                    }

                    p_join_new_info.Update();

                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(tg_dangwei);
                    // 日志
                    switch (req.GetPara("processing"))
                    {
                        case "暂未分配":
                            new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.恢复分配, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待退回, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了恢复分配操作");
                            break;
                        case "流失":
                            new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.流失, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待退回, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了主播流失操作");
                            break;
                    }

                    return new JsonResultAction();
                }
                #endregion 异步请求处理
            }
            #endregion

            #region 萌新-退回审批取消退回操作
            public class BackCancelPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "id",
                        colLength = 6,
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtInput("reason")
                    {
                        title = "拒绝原因",
                        colLength = 6,
                        isRequired = true
                    });
                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理
                /// <summary>
                /// 异步请求处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    if (req.GetPara("reason").IsNullOrEmpty()) throw new WeicodeException("请输入拒绝原因");

                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待培训.ToSByte();
                    p_join_new_info.note = $"{p_join_new_info.note};拒绝原因:{req.GetPara("reason")}";

                    p_join_new_info.Update();

                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    // 日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.完成培训, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待退回, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了取消退回操作");
                    return new JsonResultAction();
                }
                #endregion 异步请求处理
            }
            #endregion

            #region 萌新-退回明细列表展示
            public class Back
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("mx_sn")
                    {
                        width = "120px",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("mxer").id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}", "username,user_sn"),
                        placeholder = "萌新老师",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
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
                    listDisplay.isHideOperate = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mx_name")
                    {
                        text = "萌新老师",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        text = "期数",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_ting_name")
                    {
                        text = "原对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_level")
                    {
                        text = "主播分级",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("note")
                    {
                        text = "退回说明",
                        width = "600",
                        minWidth = "600"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "退回时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("back_status")
                    {
                        text = "退回状态",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("no_share")
                    {
                        text = "流失原因",
                        width = "240",
                        minWidth = "240"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qun_time")
                    {
                        text = "拉群时间",
                        width = "160",
                        minWidth = "160"
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
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"last_status = {ModelDb.p_join_new_info_log.last_status_enum.等待退回.ToSByte()}";
                    if (!reqJson.GetPara("mx_sn").IsNullOrEmpty())
                    {
                        where += $" and mx_sn = '{reqJson.GetPara("mx_sn")}'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        on = "p_join_new_info.id = p_join_new_info_log.user_info_zb_id",
                        orderby = " order by p_join_new_info_log.create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info_log, ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info_log
                {
                    public ModelDb.p_join_new_info p_join_new_info
                    {
                        get
                        {
                            return DoMySql.FindEntityById<ModelDb.p_join_new_info>(user_info_zb_id);
                        }
                    }
                    public string mx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(p_join_new_info.mx_sn).username;
                        }
                    }
                    public string term
                    {
                        get
                        {
                            return p_join_new_info.term.IsNullOrEmpty() ? "-" : p_join_new_info.term;
                        }
                    }
                    public string zb_level
                    {
                        get
                        {
                            return p_join_new_info.zb_level;
                        }
                    }
                    public string wechat_nickname
                    {
                        get
                        {
                            return p_join_new_info.wechat_nickname;
                        }
                    }
                    public string wechat_username
                    {
                        get
                        {
                            return p_join_new_info.wechat_username;
                        }
                    }
                    public string dou_nickname
                    {
                        get
                        {
                            return p_join_new_info.dou_nickname;
                        }
                    }
                    public string zb_sex
                    {
                        get
                        {
                            return p_join_new_info.zb_sex;
                        }
                    }
                    public string age
                    {
                        get
                        {
                            return p_join_new_info.age.ToString();
                        }
                    }
                    public string job
                    {
                        get
                        {
                            return p_join_new_info.job;
                        }
                    }
                    public string dou_username
                    {
                        get
                        {
                            return p_join_new_info.dou_username;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = p_join_new_info.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }
                    public string full_or_part
                    {
                        get
                        {
                            return p_join_new_info.full_or_part;
                        }
                    }
                    public string note
                    {
                        get
                        {
                            return p_join_new_info.note;
                        }
                    }
                    public string no_share
                    {
                        get
                        {
                            return p_join_new_info.no_share;
                        }
                    }
                    public string address_text
                    {
                        get
                        {
                            return p_join_new_info.province + p_join_new_info.city;
                        }
                    }

                    public string old_ting_name
                    {
                        get
                        {
                            return p_join_new_info.old_tg_user_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_new_info.old_tg_user_sn).ting_name;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return p_join_new_info.ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_new_info.ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_join_new_info.tg_user_sn).name;
                        }
                    }
                    public string back_status
                    {
                        get
                        {
                            switch (c_type)
                            {
                                case (sbyte)c_type_enum.完成培训:
                                    return "否";
                                case (sbyte)c_type_enum.恢复分配:
                                    return "是";
                                default:
                                    return "";
                            }
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return ((ModelDb.p_join_new_info.status_enum)p_join_new_info.status).ToString();
                        }
                    }
                    public string qun_time
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_info_log>($"c_type = {ModelDb.p_join_new_info_log.c_type_enum.拉群.ToSByte()} and user_info_zb_id = {user_info_zb_id}").create_time.ToString();
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 账号管理-入库失败列表展示
            public class StorageFailed
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "320px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "对接厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });

                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion

                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "UploadRecords",
                            field_paras = "id"
                        },
                        text = "上传记录",
                        name = "UploadRecords",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = RestorageAction,
                            field_paras = "id"
                        },
                        text = "重新入库"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "StorageFailedBack",
                            field_paras = "id"
                        },
                        text = "退回",
                        name = "StorageFailedBack",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });
                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 重新入库操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RestorageAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.等待拉群.ToSByte();
                    // 抖音账号去掉(已有对接厅)标记
                    string toRemove = "(已有对接厅)";
                    if (p_join_new_info.dou_username.IndexOf(toRemove) == 0)
                    {
                        p_join_new_info.dou_username = p_join_new_info.dou_username.Remove(p_join_new_info.dou_username.IndexOf(toRemove), toRemove.Length);
                    }

                    p_join_new_info.Update();

                    // 计算申请档位明细人数
                    p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(p_join_new_info.id);
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    // 添加日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.入库, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.入库失败, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了重新入库操作");
                    return info;
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }

                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 账号管理-查看萌新上传记录
            public class UploadRecords
            {
                #region DefaultView
                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("");

                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PageDetail pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHtml("upload_records")
                    {
                        title = "已有对接厅记录",
                        Content = p_join_new_info.upload_records
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView
            }
            #endregion

            #region 账号管理-入库失败退回操作
            public class StorageFailedBack
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");

                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                    };
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                        attachPara = new Dictionary<string, object>
                        {
                            {"id",req.id },
                        }
                    };
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        defaultValue = req.id,
                    });

                    formDisplay.formItems.Add(new EmtInput("cause")
                    {
                        title = "退回原因",
                        colLength = 6,
                        defaultValue = "",
                        isRequired = true,
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 主播id
                    /// </summary>
                    public string id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 异步请求处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var lSql = new List<string>();
                    if (req.GetPara("cause").IsNullOrEmpty()) { throw new Exception("请填写退回原因"); }
                    var p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(req.GetPara("id").ToInt());

                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.有对接厅.ToSByte();

                    lSql.Add(p_join_new_info.UpdateTran($"id = {p_join_new_info.id}"));

                    DoMySql.ExecuteSqlTran(lSql);
                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);
                    // 添加日志
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.退回, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.入库失败, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了退回操作，原因：{req.GetPara("cause")}");
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 厅管-待培训学员列表
            public class WaitTraining
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
                    listFilter.isExport = true;
                    listFilter.exportFilename = "资料收集.xlsx";
                    listFilter.formItems.Add(new ModelBasic.EmtInput("wechat_nickname")
                    {
                        width = "160px",
                        placeholder = "微信昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "160px",
                        placeholder = "抖音帐号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                    {
                        width = "160px",
                        placeholder = "抖音昵称",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        width = "120px",
                        placeholder = "期数",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_sex")
                    {
                        width = "100px",
                        placeholder = "性别",
                        options = new Dictionary<string, string>
                        {
                            {"男","男"},
                            {"女","女"},
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间范围",
                        mold = EmtTimeSelect.Mold.date_range,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("orderby")
                    {
                        defaultValue = req.orderby
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("job")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"宝妈","宝妈"},
                            {"上班族","上班族"},
                            {"学生党","学生党"},
                            {"自由职业者","自由职业者"},
                            {"其他","其他"}
                        },
                        placeholder = "现实工作",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"兼职","兼职"},
                            {"全职","全职"}
                        },
                        placeholder = "兼职/全职",
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
                    listDisplay.operateWidth = "240px";
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 100
                    };

                    #region 显示列

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        index = 100,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "期数",
                        width = "120",
                        minWidth = "120",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"term", "term" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"term_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_term")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","term" },
                                                    {"value","<%=page.term_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.term_text.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_term")
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
                                    code = "page.term_text.set(page.focus.attr('data-term'));"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        index = 150,
                        text = "对接厅",
                        width = "120",
                        minWidth = "120"
                    });
                    string level_javascript = @"A级标准为以下
                        声音优质，试音过程自信且培训过程全程主动配合

                        B级标准为以下
                        声音一般但配合度高的
                        试音过程紧张尴尬
                        培训需要催促才会发试音的";
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("level_text")
                    {
                        index = 200,
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "主播分级",
                        width = "100",
                        minWidth = "100",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            width = "260px",
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"zb_level", "zb_level" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=12,
                                    emtModelBase = new ModelBasic.EmtSelect($"l_zb_level")
                                    {
                                        options=new Dictionary<string, string>
                                        {
                                            {"A","A"},
                                            {"B","B"},
                                        },
                                        width = "120",
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("submit")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","zb_level" },
                                                    {"value","<%=page.l_zb_level.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.l_zb_level.value);$('.floatlayer_div').hide();",
                                                func=new ServiceFactory.JoinNew().FastEditUserInfoZb
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=5,
                                    emtModelBase = new ModelBasic.EmtButton("cancel")
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
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=2,
                                    emtModelBase = new ModelBasic.EmtHtml("detail")
                                    {
                                        Content=$@"<i class=""layui-icon layui-icon-tips"" title=""{level_javascript}""></i> ",
                                    },
                                },
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.l_zb_level.set(page.focus.attr('data-zb_level'));"
                                }
                            }
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        index = 300,
                        text = "微信昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        index = 400,
                        text = "微信账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        index = 500,
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        index = 600,
                        text = "抖音昵称",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        index = 700,
                        text = "性别",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                    {
                        index = 800,
                        text = "年龄",
                        width = "60",
                        minWidth = "60"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                    {
                        index = 900,
                        text = "现实工作",
                        width = "100",
                        minWidth = "100"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address_text")
                    {
                        index = 1000,
                        text = "地区(省市)",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                    {
                        index = 1100,
                        text = "接档时间",
                        width = "220",
                        minWidth = "220"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        index = 1200,
                        text = "兼职/全职",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        index = 1700,
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });

                    #endregion
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = TrainingAction,
                            field_paras = "id"
                        },
                        text = "完成培训"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "BackPost",
                            field_paras = "ids=id,tg_need_id"
                        },
                        text = "退回",
                        name = "BackPost"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "CausePost",
                            field_paras = "id"
                        },
                        text = "流失",
                        name = "CausePost",
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "id"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    //    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    //    {
                    //        func = RestorageAction,
                    //        field_paras = "id"
                    //    },
                    //    text = "申请更换"
                    //});

                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    //    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    //    {
                    //        func = RestorageAction,
                    //        field_paras = "id"
                    //    },
                    //    text = "流失"
                    //});

                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string orderby { get; set; } = @" ORDER BY create_time DESC";
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 删除操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.删除, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.入库失败, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了删除");
                    p_join_new_info.Delete();
                    return info;
                }
                /// <summary>
                /// 完成培训操作
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction TrainingAction(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    var p_join_new_info = req.GetPara<ModelDb.p_join_new_info>();
                    p_join_new_info.status = ModelDb.p_join_new_info.status_enum.补人完成.ToSByte();
                    p_join_new_info.Update();

                    p_join_new_info = DoMySql.FindEntityById<ModelDb.p_join_new_info>(p_join_new_info.id);
                    // 计算申请档位明细人数
                    new ServiceFactory.JoinNew().JisuanCount(p_join_new_info.tg_dangwei);

                    //完成入职的主播写入user_info_zhubo中 
                    var zhuboInfoDto = p_join_new_info.ToModel<ServiceFactory.UserInfo.Zb_NewZhubo.ZhuboInfoDto>();
                    new ServiceFactory.UserInfo.Zb_NewZhubo().CreateNewZhubo("外宣补人完成培训", $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了培训操作，微信账号：{p_join_new_info.wechat_username}，微信昵称：{p_join_new_info.wechat_nickname}，抖音账号：{p_join_new_info.dou_username}，抖音昵称：{p_join_new_info.dou_nickname}", zhuboInfoDto);

                    new ServiceFactory.JoinNew().AddJoinNewLog(ModelDb.p_join_new_info_log.c_type_enum.完成培训, p_join_new_info.id, ModelDb.p_join_new_info.status_enum.等待培训, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'进行了培训操作");

                    return info;
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = "1=1";
                    if (!reqJson.GetPara("wechat_nickname").IsNullOrEmpty())
                    {
                        where += $" and wechat_nickname like '%{reqJson.GetPara("wechat_nickname")}%'";
                    }
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and dou_username like '%{reqJson.GetPara("dou_username")}%'";
                    }
                    if (!reqJson.GetPara("dou_nickname").IsNullOrEmpty()) where += $" and dou_nickname like '%{reqJson.GetPara("dou_nickname")}%'";
                    if (!reqJson.GetPara("term").IsNullOrEmpty())
                    {
                        where += $" and term like '%{reqJson.GetPara("term")}%'";
                    }
                    if (!reqJson.GetPara("zb_sex").IsNullOrEmpty())
                    {
                        where += $" and zb_sex = '{reqJson.GetPara("zb_sex")}'";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <='" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    if (!reqJson.GetPara("job").IsNullOrEmpty())
                    {
                        where += $" and job like '%{reqJson.GetPara("job")}%'";
                    }
                    if (!reqJson.GetPara("full_or_part").IsNullOrEmpty())
                    {
                        where += $" and full_or_part = '{reqJson.GetPara("full_or_part")}'";
                    }
                    //2.获取邀约记录
                    //判断是否已有order by

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = reqJson.GetPara("orderby")
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info
                {
                    public string term_text
                    {
                        get
                        {
                            return this.term.IsNullOrEmpty() ? "-" : this.term;
                        }
                    }
                    public string sessions_name
                    {
                        get
                        {
                            var result = this.sessions.Split(',')
                                             .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                             .ToArray();
                            return string.Join(";", result);
                        }
                    }

                    public string address_text
                    {
                        get
                        {
                            return province + city;
                        }
                    }

                    public string ting_name
                    {
                        get
                        {
                            return ting_sn.IsNullOrEmpty() ? "" : new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string level_text
                    {
                        get
                        {
                            return is_fast == is_fast_enum.加急.ToSByte() ? zb_level + "(加急)" : zb_level;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 分配日志
            public class ShareLog
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("c_type")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"资料收集",ModelDb.p_join_new_info_log.c_type_enum.资料收集.ToInt().ToString()},
                            {"分级",ModelDb.p_join_new_info_log.c_type_enum.分级.ToInt().ToString()},
                            {"分配",ModelDb.p_join_new_info_log.c_type_enum.分配.ToInt().ToString()},
                            {"入库",ModelDb.p_join_new_info_log.c_type_enum.入库.ToInt().ToString()},
                            {"拉群",ModelDb.p_join_new_info_log.c_type_enum.拉群.ToInt().ToString()},
                            {"流失",ModelDb.p_join_new_info_log.c_type_enum.流失.ToInt().ToString()},
                            {"退回",ModelDb.p_join_new_info_log.c_type_enum.退回.ToInt().ToString()},
                            {"恢复分配",ModelDb.p_join_new_info_log.c_type_enum.恢复分配.ToInt().ToString()},
                            {"改抖音号",ModelDb.p_join_new_info_log.c_type_enum.改抖音号.ToInt().ToString()},
                            {"入库失败",ModelDb.p_join_new_info_log.c_type_enum.入库失败.ToInt().ToString()},
                            {"重新入库",ModelDb.p_join_new_info_log.c_type_enum.重新入库.ToInt().ToString()},
                            {"完成培训",ModelDb.p_join_new_info_log.c_type_enum.完成培训.ToInt().ToString()},
                            {"中台锁定",ModelDb.p_join_new_info_log.c_type_enum.中台锁定.ToInt().ToString()},
                            {"转移萌新",ModelDb.p_join_new_info_log.c_type_enum.转移萌新.ToInt().ToString()}
                        },
                        disabled = false,
                        placeholder = "操作类型",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("dou_username")
                    {
                        width = "140px",
                        placeholder = "抖音账号",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        width = "120px",
                        placeholder = "操作人",
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                    {
                        placeholder = "创建时间",
                        mold = EmtTimeSelect.Mold.date_range,
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
                    listDisplay.isHideOperate = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };

                    #region 显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_type_text")
                    {
                        text = "操作类型",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("last_status_text")
                    {
                        text = "操作前状态",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "描述",
                        width = "460",
                        minWidth = "460"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_type_name")
                    {
                        text = "操作人用户类型",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "操作人",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
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
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";

                    if (!reqJson.GetPara("c_type").IsNullOrEmpty()) where += $" and c_type = {reqJson.GetPara("c_type")}";
                    if (!reqJson.GetPara("dou_username").IsNullOrEmpty())
                    {
                        where += $" and user_info_zb_id in (select id from p_join_new_info where dou_username like '%{reqJson.GetPara("dou_username")}%')";
                    }
                    if (!reqJson.GetPara("user_name").IsNullOrEmpty())
                    {
                        where += $" and user_sn in (select user_sn from user_base where username like '%{reqJson.GetPara("user_name")}%')";
                    }
                    if (!reqJson.GetPara("c_date_range").ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("c_date_range").ToNullableString(), 0);
                        where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <= '" + dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1) + "'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " order by id desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_new_info_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_new_info_log
                {
                    public string c_type_text
                    {
                        get
                        {
                            return ((c_type_enum)c_type).ToString();
                        }
                    }
                    public string last_status_text
                    {
                        get
                        {
                            return ((last_status_enum)last_status).ToString();
                        }
                    }
                    public string dou_username
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_new_info>($"id = {user_info_zb_id}", false).dou_username;
                        }
                    }
                    public string user_type_name
                    {
                        get
                        {
                            return new DomainBasic.UserTypeApp().GetInfoById(user_type_id).name;
                        }
                    }
                    public string user_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{user_sn}'", false).username;
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}