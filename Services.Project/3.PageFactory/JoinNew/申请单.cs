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
    /// 申请单模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            #region 厅管提交补人申请
            public class App_ZbPost
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
                    var tinginfo = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.ting_sn);
                    var p_join_apply = DoMySql.FindEntityById<ModelDb.p_join_apply>(req.id, false);

                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = req.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtHidden("apply_sn")
                    {
                        title = "apply_sn",
                        defaultValue = p_join_apply.apply_sn.IsNullOrEmpty() ? UtilityStatic.CommonHelper.CreateUniqueSn() : p_join_apply.apply_sn,
                    });
                    formDisplay.formItems.Add(new EmtHidden("status")
                    {
                        title = "status",
                        defaultValue = p_join_apply.IsNullOrEmpty() ? ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString() : p_join_apply.status.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtHidden("yy_user_sn")
                    {
                        title = "运营sn",
                        defaultValue = tinginfo.yy_user_sn,
                    });
                    formDisplay.formItems.Add(new EmtHidden("tg_user_sn")
                    {
                        title = "所属厅管",
                        defaultValue = new UserIdentityBag().user_sn,
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营团队",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(tinginfo.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅管账号",
                        defaultValue = new UserIdentityBag().name,
                    });
                    formDisplay.formItems.Add(new EmtHidden("ting_sn")
                    {
                        defaultValue = req.ting_sn.ToString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("ting_name")
                    {
                        title = "直播厅",
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.ting_sn).ting_name
                    });
                    formDisplay.formItems.Add(new EmtRadio("tg_sex")
                    {
                        title = "厅成员",
                        options = new Dictionary<string, string>
                        {
                            {"男", "男"},
                            {"女", "女"}
                        },
                        defaultValue = p_join_apply.IsNullOrEmpty() ? tinginfo.tg_sex : p_join_apply.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtInput("manager")
                    {
                        title = "管理名称",
                        defaultValue = p_join_apply.manager
                    });
                    formDisplay.formItems.Add(new EmtInputNumber("manager_age")
                    {
                        title = "管理年龄",
                        defaultValue = p_join_apply.manager_age.ToString()
                    });

                    var jiezou_detail = DoMySql.FindEntity<ModelDb.jiezou_detail>($"ting_sn = '{req.ting_sn}' and data_time = '{DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd")}'", false);

                    decimal? step = 0;
                    if (jiezou_detail.IsNullOrEmpty() || jiezou_detail.step.IsNullOrEmpty())
                    {
                        step = 0.5m;
                    }
                    else
                    {
                        step = jiezou_detail.step;
                    }
                    formDisplay.formItems.Add(new EmtInput("step")
                    {
                        title = "阶段",
                        defaultValue = step.ToString(),
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });

                    formDisplay.formItems.Add(new EmtInputNumber("open_hours")
                    {
                        title = "开厅时长",
                        placeholder = "小时",
                        defaultValue = p_join_apply.open_hours.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("current_open_dangwei")
                    {
                        title = "在开档",
                        bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                        defaultValue = p_join_apply.IsNullOrEmpty() ? "" : p_join_apply.current_open_dangwei
                    });

                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.档位时段))
                    {
                        string key = item.Key;
                        string value = item.Value;

                        // 当前厅档位审批数
                        int ApproveNum = new ServiceFactory.JoinNew().GetDangweiApproveCountByTingSn(req.ting_sn, item.Value);

                        // 当前厅档位申请数
                        int JoinNum = new ServiceFactory.JoinNew().GetDangweiApplyCountByTingSn(req.ting_sn, item.Value);

                        if (ApproveNum > 0) { key += $"({ApproveNum}个申请正在审批)"; }
                        if (JoinNum > 0) { key += $"({JoinNum}个申请正在补人中)"; }

                        // 当前档位未分配新人主播数
                        int zb_count = new ServiceFactory.JoinNew().GetUnShareZbCountByDangwei(tinginfo.tg_sex, item.Value);

                        // 当前档位厅申请未分配新人主播数（包括审批中的人数）
                        var apply_zb_count = new ServiceFactory.JoinNew().GetApplyZbCountByDangwei(tinginfo.tg_sex, item.Value);

                        // 盈余数量 = 当前档位未分配新人主播数 - 申请中人数（当前档位厅申请未分配新人主播数）
                        int surplus = zb_count - apply_zb_count;

                        key += $"(申请中{apply_zb_count}人，{(surplus > 0 ? $"盈余{surplus}人" : $"紧张{surplus}人")})";
                        option.Add(key, value);
                    }

                    //关联p_join_apply_item查询现有开档申请
                    var applyItemList = new List<ModelDb.p_join_apply_item>();
                    if (!p_join_apply.apply_sn.IsNullOrEmpty())
                    {
                        applyItemList = DoMySql.FindList<ModelDb.p_join_apply_item>($@"apply_sn = '{p_join_apply.apply_sn}'");
                    }

                    string default_apply_item = "";
                    default_apply_item += "[";
                    foreach (var item in applyItemList)
                    {
                        default_apply_item += $@"{{""dangwei"":""{item.dangwei}"",""zb_count"":""{item.zb_count}"",""id"":""{item.id}""}},";
                    }
                    default_apply_item = default_apply_item.Substring(0, default_apply_item.Length - 1);
                    default_apply_item += "]";
                    formDisplay.formItems.Add(new ModelBasic.EmtTableEdit("l_apply_item")
                    {
                        title = "补人需求",
                        colItems = new List<ModelBasic.EmtTableEdit.ColItem>
                        {
                            new ModelBasic.EmtTableEdit.ColItem
                            {
                                 emtModel = new ModelBasic.EmtTableSelect("dangwei")
                                 {
                                     title = "档位",
                                     options = option
                                 }
                            },
                            new ModelBasic.EmtTableEdit.ColItem
                            {
                                 emtModel = new ModelBasic.EmtTableInput("zb_count")
                                 {
                                     title = "人数",
                                     placeholder = "补充人数"
                                 }
                            },
                        },
                        defaultValue = p_join_apply.IsNullOrEmpty() ? "" : default_apply_item
                    });

                    formDisplay.formItems.Add(new EmtInput("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_apply.apply_cause
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    public int id { get; set; }
                    public string ting_sn { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 申请表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    #region 构建厅补人申请表模型
                    var p_join_apply = req.GetPara<p_join_apply>();
                    p_join_apply.zb_count = p_join_apply.l_apply_item.Sum(item => item.zb_count.ToInt());
                    p_join_apply.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_join_apply.leave_rate = new ServiceFactory.UserInfo.Tg().GetLeaveRate(p_join_apply.tg_user_sn);
                    var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn);
                    p_join_apply.join_rate = ting.join_rate;
                    p_join_apply.stay_rate = ting.stay_rate;
                    if (p_join_apply.apply_sn.IsNullOrEmpty())
                    {
                        p_join_apply.apply_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    }

                    #endregion

                    #region 构建厅补人申请档位模型
                    var applyItemList = p_join_apply.l_apply_item;
                    foreach (var item in applyItemList)
                    {
                        item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        if (item.apply_sn.IsNullOrEmpty())
                        {
                            item.apply_sn = p_join_apply.apply_sn;
                        }
                        if (item.apply_item_sn.IsNullOrEmpty())
                        {
                            item.apply_item_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                        }
                        item.unsupplement_count = item.zb_count;
                    }
                    #endregion

                    #region 校验厅补人申请

                    if (p_join_apply.id > 0 && p_join_apply.status != ModelDb.p_join_apply.status_enum.等待运营审批.ToInt())
                    {
                        throw new Exception("已审核通过，禁止再次提交");
                    }
                    if (p_join_apply.yy_user_sn.IsNullOrEmpty()) throw new WeicodeException("所属运营不存在");
                    if (p_join_apply.tg_user_sn.IsNullOrEmpty()) throw new WeicodeException("厅管不存在");
                    if (p_join_apply.tg_sex.IsNullOrEmpty()) throw new WeicodeException("请选择男女厅");
                    if (p_join_apply.current_open_dangwei.IsNullOrEmpty()) throw new WeicodeException("请选择在开档");
                    if (p_join_apply.l_apply_item.Count == 0) throw new WeicodeException("请添加补人需求");
                    if (p_join_apply.zb_count == 0) throw new WeicodeException("请输入申请人数");
                    if (p_join_apply.apply_cause.IsNullOrEmpty()) throw new WeicodeException("请填写申请原因");

                    //校验明细表信息
                    new ServiceFactory.JoinNew().CheckAccessApplyZb(applyItemList, p_join_apply);

                    int total_count = 0;
                    #region 计算厅拥有主播数
                    //总数 = 现有主播数 + 待开账号数 + 在补主播数 + 申请主播数
                    var zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = p_join_apply.ting_sn
                        }
                    }).Count;
                    var new_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.待开账号,
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                            UserSn = p_join_apply.ting_sn
                        }
                    }).Count;
                    var apply_zb_count = new ServiceFactory.JoinNew().GetApplyZbCountNew(p_join_apply.ting_sn);

                    total_count = zhubo_count + new_zhubo_count + apply_zb_count + p_join_apply.zb_count.ToInt();
                    #endregion
                    //大于18人为超额,禁止补人
                    if (total_count > 18)
                    {
                        throw new Exception($"当前{total_count}人（已有主播{zhubo_count}+在补人数{new_zhubo_count + apply_zb_count}+申请人数{p_join_apply.zb_count.ToInt()}），已超18人上限（离职主播请即时删除）");
                    }
                    #endregion

                    #region 写入p_join_apply数据库表
                    if (!DoMySql.FindEntity<ModelDb.p_join_whitelist>($"ting_sn = '{p_join_apply.ting_sn}'", false).IsNullOrEmpty())
                    {
                        p_join_apply.status = ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte();
                    }
                    p_join_apply.ToModel<ModelDb.p_join_apply>().InsertOrUpdate();
                    #endregion

                    #region 写入p_join_apply_item数据库表
                    //更新时先删除后插入
                    var delItemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{p_join_apply.apply_sn}'");
                    if (!delItemList.IsNullOrEmpty())
                    {
                        foreach (var item in delItemList)
                        {
                            item.Delete();
                        }
                    }
                    foreach (var item in applyItemList)
                    {
                        item.InsertOrUpdate();
                    }
                    #endregion

                    #region 更新权重值
                    new ServiceFactory.JoinNew().UpdateWeight();
                    #endregion

                    #region 添加日志
                    new ServiceFactory.JoinNew().AddApplyLog(p_join_apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.补人申请, $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'提交了补人申请");
                    #endregion

                    #region 厅管推送公众号-运营审批
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.tger:
                            try
                            {
                                // 推送公众号
                                new ServiceFactory.Sdk.WeixinSendMsg().Approve(p_join_apply.yy_user_sn, "有补人申请需要审核", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "yyer").host_domain}/JoinNew/ApproveApplication/ApproveApplicationPost?id={DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn='{p_join_apply.apply_sn}'").id}", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
                                {
                                    person = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).name,
                                    post_time = DateTime.Now
                                });
                            }
                            catch (Exception ex)
                            {
                                return new JsonResultAction();
                            }

                            break;
                    }
                    #endregion

                    return new JsonResultAction();
                }

                /// <summary>
                /// 提交表单
                /// </summary>

                public class p_join_apply : ModelDb.p_join_apply
                {
                    public List<ModelDb.p_join_apply_item> l_apply_item { get; set; }
                }
                #endregion 异步请求处理
            }
            #endregion

            #region 补人申请记录

            public class App_ZbList
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
                    return pageModel;
                }

                /// <summary>
                /// 设置列表筛选表单的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtInput("apply_sn")
                    {
                        width = "250px",
                        placeholder = "补人单号"
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "130px",
                        placeholder = "状态",
                        options = new Dictionary<string, string>
                        {
                            {"已拒绝", ModelDb.p_join_apply.status_enum.已拒绝.ToInt().ToString()},
                            {"等待运营审批", ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待公会审批", ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString()},
                            {"等待外宣补人", ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString()},
                            {"已完成", "已完成"},
                            {"超时关闭", "超时关闭"},
                            {"已取消", ModelDb.p_join_apply.status_enum.已取消.ToInt().ToString()},
                        },
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        placeholder = "申请时间",
                        mold = EmtTimeSelect.Mold.date_range,
                        defaultValue = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd")
                    });

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button")
                    {
                        title = "申请主播"
                    };
                    var tings = new ServiceFactory.UserInfo.Ting().GetTingsByTgsn(new UserIdentityBag().user_sn);
                    foreach (var ting in tings)
                    {
                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("apply")
                        {
                            text = tings.Count > 1 ? ting.ting_name : "申请主播",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = $"ApplyZbPost?ting_sn={ting.ting_sn}",
                            },
                        });
                    }
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
                    listDisplay.operateWidth = "240";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isTotalRow = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        width = "100",
                        minWidth = "100",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_sn")
                    {
                        text = "补人单号",
                        width = "250",
                        minWidth = "250",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "360",
                        minWidth = "360",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请人数",
                        width = "130",
                        minWidth = "130",
                        sort = true,
                        summaryReq = new Pagination.SummaryReq
                        {
                            title = "申请总人数",
                            summaryType = Pagination.SummaryType.SUM
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "待分配",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("recruited_sum")
                    {
                        text = "待入库",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=3&tg_need_id={{d.id}}",
                            title = "待入库主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("put_sum")
                    {
                        text = "待拉群",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=4&tg_need_id={{d.id}}",
                            title = "待拉群主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_sum")
                    {
                        text = "待培训",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=11&tg_need_id={{d.id}}",
                            title = "待培训主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("training_zb_sum")
                    {
                        text = "已完成",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=0&tg_need_id={{d.id}}",
                            title = "已完成主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_sum")
                    {
                        text = "流失数",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=9&tg_need_id={{d.id}}",
                            title = "流失主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("other_count")
                    {
                        text = "其他",
                        width = "60",
                        minWidth = "60",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=-1&tg_need_id={{d.id}}",
                            title = "其他主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_cause")
                    {
                        text = "备注",
                        width = "280",
                        minWidth = "280",
                    });

                    #endregion 显示列

                    #region 操作列

                    //完成的主播名单
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbList",
                            field_paras = "tg_need_id=id"
                        },
                        text = "主播明细"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "详情",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.等于,
                            value = ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApplyZbPost",
                            field_paras = "id,ting_sn"
                        },
                        text = "编辑",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinNew().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_apply.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/ApplyZb/Log",
                            field_paras = "apply_sn"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    //listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    //{
                    //    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    //    {
                    //        url = $"ZhuDongLingQu",
                    //        field_paras = "id"
                    //    },
                    //    hideWith = new EmtModel.ListOperateItem.HideWith
                    //    {
                    //        compareMode = EmtModel.ListOperateItem.CompareMode.JS函数判断,
                    //        compareModeFunc = new EmtModel.ListOperateItem.CompareModeFunc
                    //        {
                    //            jsCode = $" !(d.status == '{ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()}' && new Date(d.create_time) <= new Date('{DateTime.Now.AddDays(-5)}'))"
                    //        }
                    //    },

                    //    text = "主动领取"
                    //});

                    #endregion 操作列

                    return listDisplay;
                }

                public class DtoReq
                { }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    var req = reqJson.GetPara();
                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty()) where += $" and apply_sn = '{reqJson.GetPara("apply_sn").ToNullableString()}'";
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("status"))
                        {
                            case "超时关闭":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) > (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            case "已完成":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) = (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            default:
                                where += $" and status = {reqJson.GetPara("status").ToInt()}";
                                break;
                        }
                    }
                    if (!req["create_time"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["create_time"].ToString(), 0);
                        where += $" and create_time>='{dateRange.date_range_s}' and create_time <='{dateRange.date_range_e.ToDate().AddDays(1)}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where + $" AND (tg_user_sn = '{new UserIdentityBag().user_sn}') order by create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply
                {
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            if (status.Equals(status_enum.已完成.ToInt()))
                            {
                                var p_join_apply_items = DoMySql.FindField<ModelDb.p_join_apply_item>("sum(zb_count), sum(training_zb_count)", $"apply_sn = '{apply_sn}'");
                                if (p_join_apply_items[0] == p_join_apply_items[1])
                                {
                                    return "已完成";
                                }
                                else
                                {
                                    return "超时关闭";
                                }
                            }
                            else
                            {
                                return ((status_enum)status).ToString();
                            }
                        }
                    }
                    public string zb_count_text
                    {
                        get
                        {
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            string result = "";
                            foreach (var item in itemList)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.zb_count}人;";
                            }
                            return result;
                        }
                    }
                    public class ZbCount
                    {
                        /// <summary>
                        /// 待入库
                        /// </summary>
                        public int? recruited_sum = 0;
                        /// <summary>
                        /// 待拉群
                        /// </summary>
                        public int? put_sum = 0;
                        /// <summary>
                        /// 已拉群
                        /// </summary>
                        public int? finish_zb_sum = 0;
                        /// <summary>
                        /// 已培训
                        /// </summary>
                        public int? training_zb_sum = 0;
                        /// <summary>
                        /// 流失数
                        /// </summary>
                        public int? quit_sum = 0;
                    }
                    public ZbCount zbCount
                    {
                        get
                        {
                            var zbCount = new ZbCount();
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            foreach (var item in itemList)
                            {
                                zbCount.recruited_sum += item.recruited_count;
                                zbCount.put_sum += item.put_count;
                                zbCount.finish_zb_sum += item.finish_zb_count;
                                zbCount.training_zb_sum += item.training_zb_count;
                                zbCount.quit_sum += item.quit_count;
                            }
                            return zbCount;
                        }
                    }
                    public int unsupplement_count
                    {
                        get
                        {
                            return (zb_count - DoMySql.FindList<ModelDb.p_join_new_info>($"tg_need_id = {id} and status != {ModelDb.p_join_new_info.status_enum.逻辑删除.ToSByte()}").Count).ToInt();
                        }
                    }
                    public string recruited_sum
                    {
                        get
                        {
                            return zbCount.recruited_sum.ToString();
                        }
                    }
                    public string put_sum
                    {
                        get
                        {
                            return zbCount.put_sum.ToString();
                        }
                    }
                    public string finish_zb_sum
                    {
                        get
                        {
                            return zbCount.finish_zb_sum.ToString();
                        }
                    }
                    public string training_zb_sum
                    {
                        get
                        {
                            return zbCount.training_zb_sum.ToString();
                        }
                    }
                    public string quit_sum
                    {
                        get
                        {
                            return zbCount.quit_sum.ToString();
                        }
                    }
                    public int other_count
                    {
                        get
                        {
                            return (zb_count - unsupplement_count - recruited_sum.ToInt() - put_sum.ToInt() - finish_zb_sum.ToInt() - training_zb_sum.ToInt()).ToInt();
                        }
                    }
                }

                public class apply_details
                {
                    public string dangwei { get; set; }
                    public string count { get; set; }
                    public string recruited_count { get; set; }
                }

                #endregion ListData
            }

            #endregion 申请主播记录

            #region 补人申请表单详情
            public class App_ZbDetails
            {
                public PageDetail Get(DtoReq req)
                {
                    var pageModel = new PageDetail("details");
                    pageModel.formDisplay = GetDetails(pageModel, req);
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetDetails(PageDetail pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_apply = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_apply>($"id = {req.id}");
                    var formDisplay = pageModel.formDisplay;

                    #region 表单元素
                    formDisplay.formItems.Add(new EmtLabel("apply_sn")
                    {
                        title = "补人单号",
                        defaultValue = p_join_apply.apply_sn,
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(p_join_apply.yy_user_sn).username,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅管账号",
                        defaultValue = p_join_apply.tg_username,
                    });
                    formDisplay.formItems.Add(new EmtLabel("ting_name")
                    {
                        title = "厅名",
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn).ting_name,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_apply.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_apply.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_apply.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_apply.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //获取补人节奏信息
                    var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{p_join_apply.apply_sn}'");
                    string l_apply_item_content = "";
                    foreach (var item in itemList)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr><td>{dangwei_name}</td><td>{item.zb_count}</td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th>申请人数</th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播数",
                        defaultValue = p_join_apply.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_apply.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_apply.create_time.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("status")
                    {
                        title = "审批结果",
                        defaultValue = p_join_apply.status.ToEnum<ModelDb.p_join_apply.status_enum>().ToString(),
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单id
                    /// </summary>
                    public int id { get; set; }
                }
            }

            #endregion 补人表单详情

            #region 运营提交补人申请（暂时不用）
            public class App_YyApplyZbPost
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
                    return formDisplay;
                }

                public class DtoReq
                {
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 申请表单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    return new JsonResultAction();
                }

                public class p_join_apply : ModelDb.p_join_apply
                {
                    public List<ModelDb.p_join_apply_item> l_apply_item { get; set; }
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 补人申请列表-运营审批
            public class App_YYApproveApplyZb
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
                    pageModel.buttonGroup = GetButtonGroup(req);

                    // 设置头部信息
                    int? pintotop_times = 0;
                    var user_info_yunying = DoMySql.FindEntity<ModelDb.user_info_yunying>($"yy_user_sn = '{new UserIdentityBag().user_sn}'", false);
                    if (!user_info_yunying.IsNullOrEmpty())
                    {
                        pintotop_times = user_info_yunying.join_pintotop_times;
                    }
                    string top = "";
                    top += $@"<div class=""layui-card"">";
                    top += $@"    <div class=""layui-row"">";
                    // 补人置顶次数
                    top += $@"      <div class=""layui-col-md3"">";
                    top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                    top += $@"          <div class=""layui-flex layui-items-center"">";
                    top += $@"            <div class=""layui-icon layui-icon-top"" style=""font-size: 24px; color: #1E9FFF; margin-right: 10px;""></div>";
                    top += $@"            <div>";
                    top += $@"              <div class=""text-muted"">本月剩余补人置顶次数：{pintotop_times}</div>";
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
                public CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtInput("apply_sn")
                    {
                        width = "250px",
                        placeholder = "补人单号",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "130px",
                        placeholder = "状态",
                        options = new Dictionary<string, string>
                        {
                            {"已拒绝", ModelDb.p_join_apply.status_enum.已拒绝.ToInt().ToString()},
                            {"等待运营审批", ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待公会审批", ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString()},
                            {"等待外宣补人", ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString()},
                            {"已完成", "已完成"},
                            {"超时关闭", "超时关闭"},
                            {"已取消", ModelDb.p_join_apply.status_enum.已取消.ToInt().ToString()},
                        },
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "100px",
                        placeholder = "所属厅管",
                        options = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(new UserIdentityBag().user_sn),
                        defaultValue = req.tg_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                    {
                        mold = EmtTimeSelect.Mold.date_range,
                        placeholder = "申请日期范围",
                        defaultValue = req.dateRange
                    });
                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
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
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "220";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zhubo_text")
                    {
                        text = "已有主播",
                        width = "250",
                        minWidth = "250",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_sn")
                    {
                        text = "补人单号",
                        width = "250",
                        minWidth = "250",
                        disabled = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("top_text")
                    {
                        text = "置顶",
                        width = "70",
                        minWidth = "70",
                        disabled = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_name")
                    {
                        text = "厅管用户名",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "360",
                        minWidth = "360",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请主播人数",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("month_count")
                    {
                        text = "本月申请人数",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "待分配",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("recruited_sum")
                    {
                        text = "待入库",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=3&apply_sn={{d.apply_sn}}",
                            title = "待入库主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("put_sum")
                    {
                        text = "待拉群",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=4&apply_sn={{d.apply_sn}}",
                            title = "待拉群主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_sum")
                    {
                        text = "待培训",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=11&apply_sn={{d.apply_sn}}",
                            title = "待培训主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("training_zb_sum")
                    {
                        text = "已完成",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=0&apply_sn={{d.apply_sn}}",
                            title = "已完成主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_sum")
                    {
                        text = "流失数",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=9&apply_sn={{d.apply_sn}}",
                            title = "流失主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("other_count")
                    {
                        text = "其他",
                        width = "60",
                        minWidth = "60",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=-1&apply_sn={{d.apply_sn}}",
                            title = "其他主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("apply_cause")
                    {
                        text = "申请原因",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approve_time")
                    {
                        text = "运营审批时间",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("notes")
                    {
                        text = "运营审批原因",
                        width = "200",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("approver_username")
                    {
                        text = "运营审批人",
                        disabled = true,
                    });

                    #endregion 显示列

                    #region 批量操作

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量审批",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                        {
                            new EmtModel.ButtonItem("")
                            {
                                text = "批量审批",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                                eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                                {
                                    func = ApproveAction,
                                },
                            },
                        }
                    });

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Approve",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApproveApplicationPost",
                            field_paras = "id",
                        },
                        text = "审批",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString(),
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "PinToTop",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "layui-btn-normal",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinNew().PinToTopAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()}",
                        },
                        text = "置顶"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "详情",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.等于,
                            field = "status",
                            value = ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString(),
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinNew().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_apply.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/JoinNew/ApplyZb/Log",
                            field_paras = "apply_sn"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

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
                    //1.校验
                    var ids = req.GetPara("ids") + req.GetPara("id");
                    if (ids.IsNullOrEmpty()) throw new WeicodeException("请选择申请项!");
                    //2.更新数据
                    var lSql = new List<string>();
                    var applyList = DoMySql.FindList<ModelDb.p_join_apply>($"id in ({ids})");
                    foreach (var apply in applyList)
                    {
                        apply.status = ModelDb.p_join_apply.status_enum.等待公会审批.ToInt();
                        apply.Update();
                        //操作日志记录
                        new ServiceFactory.JoinNew().AddApplyLog(apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.运营审批);
                    }
                    new PlatformSdk.WeixinMP().SendTemplateMessage("BIYWu0LpzvnAdQOKkKO2I6Zt6SXgUtFqXKztkSbkGbU", new DomainBasic.UserApp().GetInfoByUserSn("20210504154936061-1809088913").attach4, "有补人申请需要审核", "/Waixuan/ApproveApplication/ApproveApplicationList");

                    return new JsonResultAction();
                }


                #endregion 请求回调函数

                public class DtoReq
                {
                    public string tg_user_sn { get; set; }
                    public string dateRange { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    //2.获取当前运营
                    string cur_yy_user_sn = new UserIdentityBag().user_sn;
                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty())
                    {
                        where += $" and apply_sn = '{reqJson.GetPara("apply_sn")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("status"))
                        {
                            case "超时关闭":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) > (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            case "已完成":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) = (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            default:
                                where += $" and status = {reqJson.GetPara("status").ToInt()}";
                                break;
                        }
                    }
                    where += $" and yy_user_sn = '{cur_yy_user_sn}'";
                    if (!reqJson.GetPara("tg_user_sn").IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn = '{reqJson.GetPara("tg_user_sn")}'";
                    }
                    if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                    {
                        var date = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange").ToString());
                        where += $" and create_time >= '{date.date_range_s}' and create_time < '{date.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //3.获取当前运营下所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply
                {
                    public string zhubo_text
                    {
                        get
                        {
                            if (status != status_enum.等待运营审批.ToSByte()) return "";
                            // 已有主播
                            var zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                            {
                                status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                                attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                    UserSn = ting_sn
                                }
                            }).Count;
                            // 全职
                            var qz_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                            {
                                status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                                attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                    UserSn = ting_sn
                                },
                                attachWhere = "full_or_part = '全职'"
                            }).Count;
                            // 兼职
                            var jz_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                            {
                                status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                                attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                    UserSn = ting_sn
                                },
                                attachWhere = "full_or_part = '兼职'"
                            }).Count;
                            // 待开账号
                            var new_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                            {
                                status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.待开账号,
                                attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                                {
                                    userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                    UserSn = ting_sn
                                }
                            }).Count;
                            // 在补主播
                            var apply_zb_count = new ServiceFactory.JoinNew().GetApplyZbCountNew(ting_sn);

                            return $"已有主播{zhubo_count}人，全职{qz_zhubo_count}人，兼职{jz_zhubo_count}人；申请中{new_zhubo_count + apply_zb_count}人，本次申请{zb_count}人";
                        }
                    }
                    public string top_text
                    {
                        get
                        {
                            return weight.Equals(1000) ? "是" : "否";
                        }
                    }
                    public class ZbCount
                    {
                        /// <summary>
                        /// 未分配
                        /// </summary>
                        public int? unsupplement_sum = 0;
                        /// <summary>
                        /// 待入库
                        /// </summary>
                        public int? recruited_sum = 0;
                        /// <summary>
                        /// 待拉群
                        /// </summary>
                        public int? put_sum = 0;
                        /// <summary>
                        /// 已拉群
                        /// </summary>
                        public int? finish_zb_sum = 0;
                        /// <summary>
                        /// 已培训
                        /// </summary>
                        public int? training_zb_sum = 0;
                        /// <summary>
                        /// 流失数
                        /// </summary>
                        public int? quit_sum = 0;
                        /// <summary>
                        /// 其他
                        /// </summary>
                        public int? other_sum = 0;
                    }
                    public ZbCount zbCount
                    {
                        get
                        {
                            var zbCount = new ZbCount();
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            foreach (var item in itemList)
                            {
                                zbCount.unsupplement_sum += item.unsupplement_count;
                                zbCount.recruited_sum += item.recruited_count;
                                zbCount.put_sum += item.put_count;
                                zbCount.finish_zb_sum += item.finish_zb_count;
                                zbCount.training_zb_sum += item.training_zb_count;
                                zbCount.quit_sum += item.quit_count;
                                zbCount.other_sum += item.other_count;
                            }
                            return zbCount;
                        }
                    }
                    public string unsupplement_count
                    {
                        get
                        {
                            return zbCount.unsupplement_sum.ToString();
                        }
                    }
                    public string recruited_sum
                    {
                        get
                        {
                            return zbCount.recruited_sum.ToString();
                        }
                    }
                    public string put_sum
                    {
                        get
                        {
                            return zbCount.put_sum.ToString();
                        }
                    }
                    public string finish_zb_sum
                    {
                        get
                        {
                            return zbCount.finish_zb_sum.ToString();
                        }
                    }
                    public string training_zb_sum
                    {
                        get
                        {
                            return zbCount.training_zb_sum.ToString();
                        }
                    }
                    public string quit_sum
                    {
                        get
                        {
                            return zbCount.quit_sum.ToString();
                        }
                    }
                    public string other_count
                    {
                        get
                        {
                            return zbCount.other_sum.ToString();
                        }
                    }
                    public string zb_count_text
                    {
                        get
                        {
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            string result = "";
                            foreach (var item in itemList)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.zb_count}人;";
                            }
                            return result;
                        }
                    }
                    public string month_count
                    {
                        get
                        {
                            var list = DoMySql.FindList<ModelDb.p_join_apply>($"ting_sn='{ting_sn}' and create_time>='{create_time.ToDate().ToString("yyyy-MM-01")}' and create_time<='{create_time.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToDateString()}'");
                            return list.Sum(x => x.zb_count).ToString();
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            if (status.Equals(status_enum.已完成.ToInt()))
                            {
                                var p_join_apply_items = DoMySql.FindField<ModelDb.p_join_apply_item>("sum(zb_count), sum(training_zb_count)", $"apply_sn = '{apply_sn}'");
                                if (p_join_apply_items[0] == p_join_apply_items[1])
                                {
                                    return "已完成";
                                }
                                else
                                {
                                    return "超时关闭";
                                }
                            }
                            else
                            {
                                return ((status_enum)status).ToString();
                            }
                        }
                    }
                    public string notes
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_apply_log>($"apply_sn = '{this.apply_sn}' and c_type = '{ModelDb.p_join_apply_log.c_type_enum.运营审批}'", false).content;
                        }
                    }
                    //运营审批人
                    public string approver_username
                    {
                        get
                        {
                            var applyLog = DoMySql.FindEntity<ModelDb.p_join_apply_log>($"apply_sn = '{this.apply_sn}' and c_type = '{ModelDb.p_join_apply_log.c_type_enum.运营审批.ToSByte()}'", false);
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(applyLog.user_sn).username;
                        }
                    }
                    public string tg_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(this.tg_user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(this.ting_sn).ting_name;
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 补人申请-运营审批操作
            public class App_YYApproveZbPost
            {
                #region DefaultView

                public PagePost Get(DtoReq req)
                {
                    var pageModel = new PagePost("post");
                    var p_join_apply = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_apply>($"id = {req.id}", false);
                    if (p_join_apply.id == 0)
                    {
                        pageModel.formDisplay.formItems.Add(new EmtLabel("")
                        {
                            title = "",
                            defaultValue = "该申请已被删除",
                        });
                        return pageModel;
                    }

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
                            {"pjn_id", req.id }
                        }
                    };
                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            right = 100,
                        },
                        emtModelBase = new EmtSubmitButton("refuse")
                        {
                            width = "50px",
                            className = "layui-btn layui-btn-primary layui-border-blue btn-submit c__refuse",
                            defaultValue = "拒绝",
                            eventJsClick = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = $"page_post.status.set('{ModelDb.p_join_apply.status_enum.已拒绝.ToInt()}')"
                                }
                            }
                        }
                    });
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_apply = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_apply>($"id = {req.id}", false);
                    if (p_join_apply.id == 0)
                    {
                        throw new Exception("该申请已被删除");
                    }
                    var tinginfo = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn);
                    var formDisplay = pageModel.formDisplay;
                    formDisplay.buttonSubmitText = "同意";

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtHidden("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_apply.id.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_sn")
                    {
                        title = "补人单号",
                        defaultValue = p_join_apply.apply_sn,
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(p_join_apply.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅管",
                        defaultValue = p_join_apply.tg_username,
                    });
                    formDisplay.formItems.Add(new EmtHidden("tg_user_sn")
                    {
                        title = "tg_user_sn",
                        defaultValue = p_join_apply.tg_user_sn,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_apply.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("ting_name")
                    {
                        title = "厅名",
                        defaultValue = tinginfo.ting_name,
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_apply.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_apply.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_apply.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前在开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 300px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //关联p_join_apply_item查询现有开档申请
                    var applyItemList = new List<ModelDb.p_join_apply_item>();
                    if (!p_join_apply.apply_sn.IsNullOrEmpty())
                    {
                        applyItemList = DoMySql.FindList<ModelDb.p_join_apply_item>($@"apply_sn = '{p_join_apply.apply_sn}'");
                    }

                    string l_apply_item_content = "";
                    foreach (var item in applyItemList)
                    {
                        // 已有主播数
                        var zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                        {
                            status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                UserSn = p_join_apply.ting_sn
                            },
                            attachWhere = $"sessions like '%{item.dangwei}%'"
                        }).Count;
                        // 全职人数
                        var qz_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                        {
                            status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                UserSn = p_join_apply.ting_sn
                            },
                            attachWhere = $"sessions like '%{item.dangwei}%' and full_or_part = '全职'"
                        }).Count;
                        // 兼职人数
                        var jz_zhubo_count = new ServiceFactory.UserInfo.Zhubo().GetBaseInfos(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                        {
                            status = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.Status.正常,
                            attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅,
                                UserSn = p_join_apply.ting_sn
                            },
                            attachWhere = $"sessions like '%{item.dangwei}%' and full_or_part = '兼职'"
                        }).Count;

                        // 当前档位未分配新人主播数
                        int zb_count = new ServiceFactory.JoinNew().GetUnShareZbCountByDangwei(tinginfo.tg_sex, item.dangwei);

                        // 当前档位厅申请未分配新人主播数（包括审批中的人数）
                        var apply_zb_count = new ServiceFactory.JoinNew().GetApplyZbCountByDangwei(tinginfo.tg_sex, item.dangwei);

                        // 盈余数量 = 当前档位未分配新人主播数 - 申请中人数（当前档位厅申请未分配新人主播数）
                        int surplus = zb_count - apply_zb_count;

                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr><td>{dangwei_name}</td><td>{item.zb_count}</td><td>已有主播{zhubo_count}人，全职{qz_zhubo_count}人，兼职{jz_zhubo_count}人</td><td>申请中{apply_zb_count}人，{(surplus > 0 ? $"盈余{surplus}人" : $"紧张{surplus}人")}</td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th>" +
                        "<th style='text-align: center;'>人数</th>" +
                        "<th style='text-align: center;'></th>" +
                        "<th style='text-align: center;'></th>" +
                        "</tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播人数",
                        defaultValue = p_join_apply.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_apply.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_apply.create_time.ToString(),
                    });

                    string defaultStatus = "";
                    if (p_join_apply.status == ModelDb.p_join_apply.status_enum.等待运营审批.ToInt())
                    {
                        defaultStatus = ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString();
                    }
                    else
                    {
                        defaultStatus = p_join_apply.status.ToString();
                    }
                    formDisplay.formItems.Add(new EmtHidden("status")
                    {
                        title = "审批结果",
                        defaultValue = defaultStatus
                    });
                    formDisplay.formItems.Add(new EmtInput("notes")
                    {
                        title = "审批备注",
                    });

                    int buren = 0;
                    int liushi = 0;
                    foreach (var tg in new ServiceFactory.UserInfo.Yy().YyGetNextTg(new UserIdentityBag().user_sn))
                    {
                        var needs = DomainBasicStatic.DoMySql.FindList<ModelDb.p_join_apply>($"tg_user_sn = {tg.user_sn} and create_time>='{DateTime.Today.ToString("yyyy-MM-01")}' and create_time<='{DateTime.Today.ToString("yyyy-MM-dd")}'");
                        foreach (var item in needs)
                        {
                            buren += item.finish_zb_count.ToInt();
                        }
                        liushi += DoMySql.FindList<ModelDb.user_info_zhubo>($"status = '{ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte()}' and user_sn in {new ServiceFactory.UserInfo.Tg().TgGetNextZbForSql(tg.user_sn)}").Count;
                    }
                    formDisplay.formItems.Add(new EmtLabel("buren")
                    {
                        title = "本月补人人数",
                        defaultValue = buren.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("liushi")
                    {
                        title = "本月流失人数",
                        defaultValue = liushi.ToString(),
                    });
                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    var pjn_id = req.GetPara("pjn_id");
                    var status = req.GetPara("status");
                    var notes = req.GetPara("notes");
                    if (pjn_id.IsNullOrEmpty()) throw new WeicodeException();
                    var p_join_apply = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_apply>($"id = {pjn_id}");
                    if (p_join_apply.status != ModelDb.p_join_apply.status_enum.等待运营审批.ToInt()) throw new WeicodeException("该申请单已审批，不可重复审批!");
                    if (status.IsNullOrEmpty()) throw new WeicodeException("请添加审批结果!");

                    if (!DoMySql.FindEntity<ModelDb.p_join_whitelist>($"ting_sn = '{p_join_apply.ting_sn}'", false).IsNullOrEmpty() && status == ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString())
                    {
                        status = ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString();
                    }

                    //2.提交审批表单,更新数据
                    p_join_apply.status = status.ToInt();
                    p_join_apply.Update();

                    //记录操作日志
                    new ServiceFactory.JoinNew().AddApplyLog(p_join_apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.运营审批, notes);

                    if (status == ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString())
                    {
                        try
                        {
                            new PlatformSdk.WeixinMP().SendTemplateMessage("BIYWu0LpzvnAdQOKkKO2I6Zt6SXgUtFqXKztkSbkGbU", new DomainBasic.UserApp().GetInfoByUserSn("20210504154936061-1809088913").attach4, "有补人申请需要审核", $"/Waixuan/ApproveApplication/ApproveApplicationList");
                        }
                        catch
                        {
                        }
                    }
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
                public class p_join_apply : ModelDb.p_join_apply
                {
                    public List<ModelDb.p_join_apply_item> l_apply_item { get; set; }
                }
            }
            #endregion

            #region 补人申请列表-管理员审批
            public class App_ApproveApplyZb
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
                        placeholder = "补人单号"
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        width = "130px",
                        placeholder = "状态",
                        options = new Dictionary<string, string>
                        {
                            {"已拒绝", ModelDb.p_join_apply.status_enum.已拒绝.ToInt().ToString()},
                            {"等待运营审批", ModelDb.p_join_apply.status_enum.等待运营审批.ToInt().ToString()},
                            {"等待公会审批", ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString()},
                            {"等待外宣补人", ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString()},
                            {"已完成", "已完成"},
                            {"超时关闭", "超时关闭"},
                            {"已取消", ModelDb.p_join_apply.status_enum.已取消.ToInt().ToString()},
                        },
                        disabled = true
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                    {
                        placeholder = "申请时间",
                        mold = EmtTimeSelect.Mold.date_range,
                        defaultValue = req.dateRange,
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                    {
                        width = "100px",
                        placeholder = "男女厅",
                        options = new Dictionary<string, string>
                        {
                            {"男", "男"},
                            {"女", "女"},
                        },
                        disabled = true
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        width = "110px",
                        placeholder = "运营账号",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        defaultValue = req.yy_user_sn,
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        width = "100px",
                        placeholder = "厅管名",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                    });

                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "免审白名单",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"WhiteList",
                        },
                    });
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
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("id")
                    {
                        text = "申请单号",
                        disabled = true,
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "160",
                        minWidth = "160",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("yy_username")
                    {
                        text = "所属团队",
                        width = "110",
                        minWidth = "110",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "厅名",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("join_rate_txt")
                    {
                        text = "补人率",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("stay_rate_txt")
                    {
                        text = "留人率",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count_text")
                    {
                        //xxx档:需求xx人;xxx档:需求xx人
                        text = "补人需求",
                        width = "360",
                        minWidth = "360",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("tg_sex")
                    {
                        text = "男女厅",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("zb_count")
                    {
                        text = "申请数",
                        width = "90",
                        minWidth = "90",
                        sort = true
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("month_count")
                    {
                        text = "本月申请人数",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("unsupplement_count")
                    {
                        text = "待分配",
                        width = "80",
                        minWidth = "80",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("recruited_sum")
                    {
                        text = "待入库",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=3&apply_sn={{d.apply_sn}}",
                            title = "待入库主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("put_sum")
                    {
                        text = "待拉群",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=4&apply_sn={{d.apply_sn}}",
                            title = "待拉群主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("finish_zb_sum")
                    {
                        text = "待培训",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=11&apply_sn={{d.apply_sn}}",
                            title = "待培训主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("training_zb_sum")
                    {
                        text = "已完成",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=0&apply_sn={{d.apply_sn}}",
                            title = "已完成主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("quit_sum")
                    {
                        text = "流失数",
                        width = "80",
                        minWidth = "80",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=9&apply_sn={{d.apply_sn}}",
                            title = "流失主播"
                        }
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("other_count")
                    {
                        text = "其他",
                        width = "60",
                        minWidth = "60",
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            width = 1000,
                            height = 800,
                            href = "ZbList?para_status=-1&apply_sn={{d.apply_sn}}",
                            title = "其他主播"
                        }
                    });

                    #endregion 显示列

                    #region 批量操作

                    listDisplay.listBatchItems.Add(new EmtModel.ButtonItem("")
                    {
                        text = "批量审批",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new EmtModel.ButtonItem("")
                        {
                            text = "同意",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = ApproveAction,
                            },
                        },
                    }
                    });

                    #endregion 批量操作

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ApproveApplicationPost",
                            field_paras = "id",
                        },
                        text = "审批",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            field = "status",
                            value = ModelDb.p_join_apply.status_enum.等待公会审批.ToInt().ToString()
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = $"ZbDetails",
                            field_paras = "id"
                        },
                        text = "详情"
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Cancel",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            field_paras = "id",
                            func = new ServiceFactory.JoinNew().CancelAction
                        },
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            compareType = EmtModel.ListOperateItem.CompareType.不包含,
                            field = "status",
                            value = $"{ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt()},{ModelDb.p_join_apply.status_enum.等待运营审批.ToInt()},{ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()}",
                        },
                        text = "取消"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "Log",
                            field_paras = "apply_sn"
                        },
                        text = "日志",
                        name = "Log",
                    });

                    #endregion 操作列

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
                    //1.校验
                    var ids = req.GetPara("ids");
                    if (ids.IsNullOrEmpty()) throw new WeicodeException("请选择申请项!");
                    var p_join_apply_list = req.GetPara<List<ModelDb.p_join_apply>>("check_data");
                    //2.更新数据
                    List<string> lSql = new List<string>();
                    foreach (var p_join_apply in p_join_apply_list)
                    {
                        lSql.Add(new ModelDb.p_join_apply
                        {
                            status = ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt(),
                        }.UpdateTran($"id = {p_join_apply.id}"));
                        new ServiceFactory.JoinNew().AddApplyLog(DoMySql.FindEntityById<ModelDb.p_join_apply>(p_join_apply.id).apply_sn, ModelDb.p_join_apply_log.c_type_enum.公会审批);
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                {
                    public string yy_user_sn { get; set; }
                    public string dateRange { get; set; }
                    public string status { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty())
                    {
                        where += $" and apply_sn = '{reqJson.GetPara("apply_sn")}'";
                    }
                    if (!reqJson.GetPara("status").IsNullOrEmpty())
                    {
                        switch (reqJson.GetPara("status"))
                        {
                            case "超时关闭":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) > (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            case "已完成":
                                where += $" and status = {ModelDb.p_join_apply.status_enum.已完成.ToSByte()} and (select sum(zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn) = (select sum(training_zb_count) from p_join_apply_item where apply_sn = p_join_apply.apply_sn)";
                                break;
                            default:
                                where += $" and status = {reqJson.GetPara("status").ToInt()}";
                                break;
                        }
                    }
                    var tg_username = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(reqJson.GetPara("tg_user_sn")).username;
                    if (!tg_username.IsNullOrEmpty())
                    {
                        where += $" and tg_username like '%{tg_username}%'";
                    }
                    if (!reqJson.GetPara("tg_sex").IsNullOrEmpty())
                    {
                        where += $" and tg_sex = '{reqJson.GetPara("tg_sex")}'";
                    }
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }
                    if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                        where += $" and create_time >= '{dateRange.date_range_s}' and create_time < '{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                    }
                    //2.获取所有厅管的申请操作记录
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply
                {
                    public string join_rate_txt
                    {
                        get
                        {
                            return join_rate + "%";
                        }
                    }
                    public string stay_rate_txt
                    {
                        get
                        {
                            return stay_rate + "%";
                        }
                    }
                    public class ZbCount
                    {
                        /// <summary>
                        /// 未分配
                        /// </summary>
                        public int? unsupplement_sum = 0;
                        /// <summary>
                        /// 待入库
                        /// </summary>
                        public int? recruited_sum = 0;
                        /// <summary>
                        /// 待拉群
                        /// </summary>
                        public int? put_sum = 0;
                        /// <summary>
                        /// 已拉群
                        /// </summary>
                        public int? finish_zb_sum = 0;
                        /// <summary>
                        /// 已培训
                        /// </summary>
                        public int? training_zb_sum = 0;
                        /// <summary>
                        /// 流失数
                        /// </summary>
                        public int? quit_sum = 0;
                        /// <summary>
                        /// 其他
                        /// </summary>
                        public int? other_sum = 0;
                    }
                    public ZbCount zbCount
                    {
                        get
                        {
                            var zbCount = new ZbCount();
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            foreach (var item in itemList)
                            {
                                zbCount.unsupplement_sum += item.unsupplement_count;
                                zbCount.recruited_sum += item.recruited_count;
                                zbCount.put_sum += item.put_count;
                                zbCount.finish_zb_sum += item.finish_zb_count;
                                zbCount.training_zb_sum += item.training_zb_count;
                                zbCount.quit_sum += item.quit_count;
                                zbCount.other_sum += item.other_count;
                            }
                            return zbCount;
                        }
                    }
                    public string unsupplement_count
                    {
                        get
                        {
                            return zbCount.unsupplement_sum.ToString();
                        }
                    }
                    public string recruited_sum
                    {
                        get
                        {
                            return zbCount.recruited_sum.ToString();
                        }
                    }
                    public string put_sum
                    {
                        get
                        {
                            return zbCount.put_sum.ToString();
                        }
                    }
                    public string finish_zb_sum
                    {
                        get
                        {
                            return zbCount.finish_zb_sum.ToString();
                        }
                    }
                    public string training_zb_sum
                    {
                        get
                        {
                            return zbCount.training_zb_sum.ToString();
                        }
                    }
                    public string quit_sum
                    {
                        get
                        {
                            return zbCount.quit_sum.ToString();
                        }
                    }
                    public string other_count
                    {
                        get
                        {
                            return zbCount.other_sum.ToString();
                        }
                    }
                    public string zb_count_text
                    {
                        get
                        {
                            var itemList = DoMySql.FindList<ModelDb.p_join_apply_item>($"apply_sn = '{this.apply_sn}'");
                            string result = "";
                            foreach (var item in itemList)
                            {
                                result += $"{new DomainBasic.DictionaryApp().GetKeyFromValue("档位时段", item.dangwei)}:需求{item.zb_count}人;";
                            }
                            return result;
                        }
                    }
                    public string month_count
                    {
                        get
                        {
                            var list = DoMySql.FindList<ModelDb.p_join_apply>($"ting_sn='{ting_sn}' and create_time>='{create_time.ToDate().ToString("yyyy-MM-01")}' and create_time<='{create_time.ToDate().AddMonths(1).ToString("yyyy-MM-01").ToDate().AddDays(-1).ToDateString()}'");
                            return list.Sum(x => x.zb_count).ToString();
                        }
                    }
                    public string yy_username
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
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            if (status.Equals(status_enum.已完成.ToInt()))
                            {
                                var p_join_apply_items = DoMySql.FindField<ModelDb.p_join_apply_item>("sum(zb_count), sum(training_zb_count)", $"apply_sn = '{apply_sn}'");
                                if (p_join_apply_items[0] == p_join_apply_items[1])
                                {
                                    return "已完成";
                                }
                                else
                                {
                                    return "超时关闭";
                                }
                            }
                            else
                            {
                                return ((status_enum)status).ToString();
                            }
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 补人申请-管理员审批操作
            public class App_ApproveZbPost
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
                            {"pjn_id", req.id }
                        }
                    };
                    pageModel.adjuncts.Add(new AdjFloatLayer("floatlayer")
                    {
                        position = AdjFloatLayer.Position.固定定位,
                        positionFixed = new AdjFloatLayer.PositionFixed
                        {
                            bottom = 10,
                            right = 100,
                        },
                        emtModelBase = new EmtSubmitButton("refuse")
                        {
                            width = "50px",
                            className = "layui-btn layui-btn-primary layui-border-blue btn-submit c__refuse",
                            defaultValue = "拒绝",
                            eventJsClick = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = $"page_post.status.set('{ModelDb.p_join_apply.status_enum.已拒绝.ToInt()}')"
                                }
                            }
                        }
                    });
                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
                {
                    //获取申请单
                    var p_join_apply = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_join_apply>($"id = {req.id}");
                    var tinginfo = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_join_apply.ting_sn);
                    var formDisplay = pageModel.formDisplay;
                    formDisplay.buttonSubmitText = "同意";

                    #region 表单元素

                    formDisplay.formItems.Add(new EmtLabel("id")
                    {
                        title = "申请单号",
                        defaultValue = p_join_apply.id.ToString(),
                        isDisplay = false,
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_sn")
                    {
                        title = "补人单号",
                        defaultValue = p_join_apply.apply_sn
                    });
                    formDisplay.formItems.Add(new EmtLabel("yy_username")
                    {
                        title = "运营账号",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(p_join_apply.yy_user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "申请人(厅管)",
                        defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(p_join_apply.tg_user_sn).username,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_username")
                    {
                        title = "厅名",
                        defaultValue = tinginfo.ting_name,
                    });
                    formDisplay.formItems.Add(new EmtLabel("tg_sex")
                    {
                        title = "厅管性别",
                        defaultValue = p_join_apply.tg_sex
                    });
                    formDisplay.formItems.Add(new EmtLabel("manager")
                    {
                        title = "管理",
                        defaultValue = p_join_apply.manager
                    });
                    formDisplay.formItems.Add(new EmtLabel("open_hours")
                    {
                        title = "开厅时长(h)",
                        defaultValue = p_join_apply.open_hours.ToString(),
                    });

                    //获取目前在开档信息
                    string current_open_dangwei_Content = "";
                    var dangwei_values = p_join_apply.current_open_dangwei.Split(',');
                    foreach (var value in dangwei_values)
                    {
                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), value);
                        current_open_dangwei_Content += $"<tr><td>{dangwei_name}</td></tr>";
                    }
                    current_open_dangwei_Content = "<thead><tr><th style='text-align: center;'>档位</th></tr></thead><tbody>" + current_open_dangwei_Content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("current_open_dangwei")
                    {
                        title = "目前在开档",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 500px;'>"
                                         +
                                            current_open_dangwei_Content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    //关联p_join_apply_item查询现有开档申请
                    var applyItemList = new List<ModelDb.p_join_apply_item>();
                    if (!p_join_apply.apply_sn.IsNullOrEmpty())
                    {
                        applyItemList = DoMySql.FindList<ModelDb.p_join_apply_item>($@"apply_sn = '{p_join_apply.apply_sn}'");
                    }
                    string l_apply_item_content = "";
                    foreach (var item in applyItemList)
                    {
                        // 当前档位未分配新人主播数
                        int zb_count = new ServiceFactory.JoinNew().GetUnShareZbCountByDangwei(tinginfo.tg_sex, item.dangwei);

                        // 当前档位厅申请未分配新人主播数（包括审批中的人数）
                        var apply_zb_count = new ServiceFactory.JoinNew().GetApplyZbCountByDangwei(tinginfo.tg_sex, item.dangwei);

                        // 盈余数量 = 当前档位未分配新人主播数 - 申请中人数（当前档位厅申请未分配新人主播数）
                        int surplus = zb_count - apply_zb_count;

                        var dangwei_name = new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), item.dangwei);
                        l_apply_item_content += $"<tr><td>{dangwei_name}</td><td>{item.zb_count}</td><td>{apply_zb_count}人</td><td>{(surplus > 0 ? $"盈余{surplus}人" : $"紧张{surplus}人")}</td></tr>";
                    }
                    l_apply_item_content = "<thead><tr><th style='text-align: center;'>档位</th><th style='text-align: center;'>人数</th><th style='text-align: center;'>申请中</th><th></th></tr></thead><tbody>" + l_apply_item_content + "</tbody>";
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("l_apply_item")
                    {
                        title = "补人需求",
                        Content = "<div>" +
                                    "<table class = 'layui-table' style='text-align: center; width: 500px;'>"
                                         +
                                            l_apply_item_content
                                         +
                                    "</table>" +
                                  "</div>",
                    });

                    formDisplay.formItems.Add(new EmtLabel("zb_count")
                    {
                        title = "申请主播人数",
                        defaultValue = p_join_apply.zb_count.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtLabel("apply_cause")
                    {
                        title = "申请原因",
                        defaultValue = p_join_apply.apply_cause,
                    });
                    formDisplay.formItems.Add(new EmtLabel("create_time")
                    {
                        title = "申请时间",
                        defaultValue = p_join_apply.create_time.ToString(),
                    });
                    string defaultStatus = "";
                    if (p_join_apply.status == ModelDb.p_join_apply.status_enum.等待公会审批.ToInt())
                    {
                        defaultStatus = ModelDb.p_join_apply.status_enum.等待外宣补人.ToInt().ToString();
                    }
                    else
                    {
                        defaultStatus = p_join_apply.status.ToString();
                    }
                    formDisplay.formItems.Add(new EmtHidden("status")
                    {
                        title = "审批结果",
                        defaultValue = defaultStatus
                    });
                    formDisplay.formItems.Add(new EmtInput("m_notes")
                    {
                        title = "审批原因",
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 申请单号id
                    /// </summary>
                    public int id { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    var pjn_id = req.GetPara("pjn_id");
                    var zb_count = req.GetPara("zb_count");
                    var status = req.GetPara("status");
                    var m_notes = req.GetPara("m_notes");
                    if (pjn_id.IsNullOrEmpty()) throw new WeicodeException();
                    if (zb_count.IsNullOrEmpty()) throw new WeicodeException("申请人数不能为空!");
                    if (!zb_count.IsValidInt()) throw new WeicodeException("申请人数必须为整数!");
                    if (status.IsNullOrEmpty()) throw new WeicodeException("请添加审批结果!");
                    var p_join_apply = DoMySql.FindEntityById<ModelDb.p_join_apply>(pjn_id.ToInt(), false);
                    if (p_join_apply.status != ModelDb.p_join_apply.status_enum.等待公会审批.ToInt()) throw new WeicodeException("该申请单已审批，不可重复审批!");

                    //2.提交审批表单,更新数据
                    p_join_apply.status = status.ToInt();
                    p_join_apply.zb_count = zb_count.ToInt();
                    p_join_apply.Update();

                    //记录操作日志
                    new ServiceFactory.JoinNew().AddApplyLog(p_join_apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.公会审批, m_notes);

                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 运营免审核白名单列表
            /// <summary>
            /// 运营免审核白名单
            /// </summary>
            public class WhiteList
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
                    pageModel.buttonGroup = GetButtonGroup(req);

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
                    return listFilter;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("apply")
                    {
                        text = "添加白名单",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"AddWhite",
                        },
                    });
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
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.operateWidth = "80";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "厅管用户名",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                    });

                    #endregion 显示列

                    #region 操作列

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,

                        eventCsAction = new EmtModel.ListOperateItem.EventCsAction
                        {
                            func = WhiteListAction,
                            field_paras = "id"
                        },
                        text = "移除",
                    });

                    #endregion 操作列

                    return listDisplay;
                }

                #region 请求回调函数

                /// <summary>
                /// 审批处理函数
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction WhiteListAction(JsonRequestAction req)
                {
                    var p_join_whitelist = DoMySql.FindEntity<ModelDb.p_join_whitelist>($"id='{req.GetPara("id")}'", false);
                    if (p_join_whitelist.id > 0)
                    {
                        p_join_whitelist.Delete();
                    }

                    return new JsonResultAction();
                }

                #endregion 请求回调函数

                public class DtoReq
                { }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前运营下所有厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"1=1";

                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_whitelist, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_whitelist
                {
                    public string username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_sn).name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 运营免审核白名单新增
            public class WhitePost
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

                    formDisplay.formItems.Add(new EmtSelect("yy_user_sn")
                    {
                        title = "选择运营",
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                        }),
                        defaultValue = req.yy_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "window.location.href='/JoinNew/ApproveApplication/AddWhite?yy_user_sn='+page_post.yy_user_sn.value",
                            }
                        },
                    });

                    formDisplay.formItems.Add(new EmtExt.XmSelect("ting_sns")
                    {
                        title = "选择直播厅",
                        bindOptions = new ServiceFactory.UserInfo.Ting().GetTingsOptions(req.yy_user_sn)
                    });

                    #endregion 表单元素

                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 所属运营
                    /// </summary>
                    public string yy_user_sn { get; set; }
                }

                #endregion DefaultView

                #region 异步请求处理

                /// <summary>
                /// 审批申请单处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var l_ting_sn = req.GetPara("ting_sns").Split(',');
                    var lSql = new List<string>();
                    var tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    foreach (var ting_sn in l_ting_sn)
                    {
                        var p_join_whitelist = DoMySql.FindEntity<ModelDb.p_join_whitelist>($"ting_sn = '{ting_sn}'", false);
                        if (p_join_whitelist.id == 0)
                        {
                            p_join_whitelist.tenant_id = tenant_id;
                            p_join_whitelist.user_sn = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).tg_user_sn;
                            p_join_whitelist.ting_sn = ting_sn;
                            lSql.Add(p_join_whitelist.InsertOrUpdateTran($"ting_sn = '{ting_sn}'"));
                        }
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);
                    return new JsonResultAction();
                }

                #endregion 异步请求处理
            }
            #endregion

            #region 申请日志
            public class ApplyLog
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
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("apply_sn")
                    {
                        defaultValue = req.apply_sn,
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("c_type")
                    {
                        width = "120px",
                        options = new Dictionary<string, string>
                        {
                            {"补人申请",ModelDb.p_join_apply_log.c_type_enum.补人申请.ToInt().ToString()},
                            {"取消",ModelDb.p_join_apply_log.c_type_enum.取消.ToInt().ToString()},
                            {"删除",ModelDb.p_join_apply_log.c_type_enum.删除.ToInt().ToString()},
                            {"审批通过",ModelDb.p_join_apply_log.c_type_enum.审批通过.ToInt().ToString()},
                            {"运营审批",ModelDb.p_join_apply_log.c_type_enum.运营审批.ToInt().ToString()},
                            {"公会审批",ModelDb.p_join_apply_log.c_type_enum.公会审批.ToInt().ToString()}
                        },
                        disabled = false,
                        placeholder = "操作类型",
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
                    listDisplay.operateWidth = "0px";
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "描述",
                        width = "440",
                        minWidth = "440"
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
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #endregion
                    #region 操作列按钮

                    #endregion

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string apply_sn { get; set; }
                }
                #endregion

                #region 异步请求处理
                public class DtoReqData : ModelDb.p_join_apply_log
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

                    if (!reqJson.GetPara("apply_sn").IsNullOrEmpty()) where += $" and apply_sn = '{reqJson.GetPara("apply_sn")}'";
                    if (!reqJson.GetPara("c_type").IsNullOrEmpty()) where += $" and c_type = {reqJson.GetPara("c_type")}";
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
                    return new CtlListDisplay.ListData().getList<ModelDb.p_join_apply_log, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_join_apply_log
                {
                    public string c_type_text
                    {
                        get
                        {
                            return ((c_type_enum)c_type).ToString();
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
                    public ModelDb.p_join_apply p_join_apply
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_join_apply>($"apply_sn = '{apply_sn}'");
                        }
                    }
                    public string tg_name
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
                }
                #endregion
            }
            #endregion
        }
    }
}
