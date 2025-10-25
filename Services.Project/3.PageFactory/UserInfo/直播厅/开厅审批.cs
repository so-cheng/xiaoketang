using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.Sdk.WeixinSendMsg;
using static Services.Project.ServiceFactory.UserInfo;

namespace Services.Project
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            #region 开厅申请列表
            /// <summary>
            /// 开厅申请列表
            /// </summary>
            public class TingApplyList
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
                    listDisplay.operateWidth = "160";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                    {
                        text = "厅管账号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_ting_name")
                    {
                        text = "原接档厅",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("p_ting_name")
                    {
                        text = "所属主厅",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dy_account")
                    {
                        text = "抖音大头号",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("real_name")
                    {
                        text = "昵称",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("moblie_lastfour")
                    {
                        text = "手机后四位",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jjr_name")
                    {
                        text = "运营经纪人",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮
                    if (new DomainBasic.UserTypeApp().GetInfo().id == ModelEnum.UserTypeEnum.yyer.ToSByte())
                    {
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            name = "Del",
                            style = "",
                            text = "取消",
                            title = "取消",
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = CancelAction,
                                field_paras = "id"
                            },
                            hideWith = new ModelBasic.EmtModel.ListOperateItem.HideWith
                            {
                                compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                                field = "status",
                                value = ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToInt().ToString()
                            }
                        });
                    }

                    if (new DomainBasic.UserTypeApp().GetInfo().id == ModelEnum.UserTypeEnum.manager.ToSByte())
                    {
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            name = "Pass",
                            style = "",
                            text = "通过",
                            title = "通过",
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                            eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                            {
                                url = new PageCallBack().GetCallBackUrl("/UserInfo/TingInfo/InfoPost", UpdateTingApply),
                                field_paras = "callback_para=id,out_para=id,tg_user_sn,jjr_name,dou_user=dy_account"
                            }
                        });

                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            name = "Refuse",
                            style = "",
                            text = "拒绝",
                            title = "拒绝",
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                            {
                                func = RefuseAction,
                                field_paras = "id"
                            },
                            hideWith = new ModelBasic.EmtModel.ListOperateItem.HideWith
                            {
                                compareType = ModelBasic.EmtModel.ListOperateItem.CompareType.不等于,
                                field = "status",
                                value = ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToInt().ToString()
                            }
                        });
                    }



                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public FilterForm filterForm { get; set; } = new FilterForm();

                    /// <summary>
                    /// 筛选参数（自定义）
                    /// </summary>
                    public class FilterForm
                    {

                    }
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
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id}";

                    var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                    //查询条件

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_ting_apply, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_ting_apply
                {
                    public string p_ting_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn='{p_ting_sn}'", false).ting_name;
                        }
                    }
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).name;
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
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 取消
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction CancelAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(dtoReqData.id);
                    if (user_info_ting_apply.status > ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()) throw new Exception("已审批通过不可取消");

                    UpdateStatus(ModelDb.user_info_ting_apply.status_enum.已取消.ToSByte(), dtoReqData.id);
                    return result;
                }

                /// <summary>
                /// 通过
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PassAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(dtoReqData.id);
                    if (user_info_ting_apply.status > ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()) throw new Exception("已审批通过不可取消");

                    UpdateStatus(ModelDb.user_info_ting_apply.status_enum.等待开通.ToSByte(), dtoReqData.id);
                    return result;
                }

                /// <summary>
                /// 拒绝
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction RefuseAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();

                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(dtoReqData.id);
                    if (user_info_ting_apply.status > ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()) throw new Exception("已审批通过不可取消");

                    UpdateStatus(ModelDb.user_info_ting_apply.status_enum.已拒绝.ToSByte(), dtoReqData.id);
                    return result;
                }

                /// <summary>
                /// 更新开厅申请完成
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction UpdateTingApply(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    UpdateStatus(ModelDb.user_info_ting_apply.status_enum.已完成.ToSByte(), req.callback_para.ToInt());

                    return result;
                }

                /// <summary>
                /// 更新开厅申请状态
                /// </summary>
                /// <returns></returns>
                public void UpdateStatus(sbyte status, int id)
                {
                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(id);

                    user_info_ting_apply.status = status;
                    user_info_ting_apply.Update();
                }

                public class DtoReqData : ModelDb.user_info_ting_apply
                {

                }
                #endregion
            }
            #endregion

            #region 提交开厅申请页面
            /// <summary>
            /// 提交开厅申请页面
            /// </summary>
            public class TingApplyPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
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
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                    {
                        title = "List",
                        text = "历史记录",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "List"
                        }
                    });
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
                    var user_info_ting_apply = DoMySql.FindEntityById<ModelDb.user_info_ting_apply>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_ting_apply.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("status")
                    {
                        defaultValue = user_info_ting_apply.status.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        title = "所属厅管",
                        placeholder = "厅管选填",
                        options = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(new UserIdentityBag().user_sn),
                        defaultValue = user_info_ting_apply.p_ting_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page_post.tg_user_sn.value%>"}
                            },
                                func = GetTing,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.p_ting_sn").options(@"JSON.parse(res.data)")};"
                            }
                        }

                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_ting_sn")
                    {
                        title = "所属主厅",
                        placeholder = "训练厅选填",
                        options = new Dictionary<string, string>(),
                        defaultValue = user_info_ting_apply.p_ting_sn,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("old_ting_name")
                    {
                        title = "原接档厅",
                        defaultValue = user_info_ting_apply.dy_account,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dy_account")
                    {
                        title = "抖音大头号",
                        defaultValue = user_info_ting_apply.dy_account,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("real_name")
                    {
                        title = "厅名",
                        defaultValue = user_info_ting_apply.real_name,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("moblie_lastfour")
                    {
                        title = "手机后四位",
                        defaultValue = user_info_ting_apply.moblie_lastfour,
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "运营经纪人",
                        defaultValue = user_info_ting_apply.jjr_name,
                        placeholder = "设置一个运营经纪人名字用于登录抖音管理后台(格式：厅名-厅管名),训练厅不需要填写运营经纪人",
                        isRequired = true
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
                    var user_info_ting_apply = req.data_json.ToModel<ModelDb.user_info_ting_apply>();

                    #region 校验
                    if (user_info_ting_apply.old_ting_name.IsNullOrEmpty()) throw new Exception("请输入原接档厅");
                    if (user_info_ting_apply.dy_account.IsNullOrEmpty()) throw new Exception("请输入抖音大头号");
                    if (user_info_ting_apply.real_name.IsNullOrEmpty()) throw new Exception("请输入昵称");
                    if (user_info_ting_apply.moblie_lastfour.IsNullOrEmpty()) throw new Exception("请输入手机后四位");
                    if (user_info_ting_apply.p_ting_sn.IsNullOrEmpty())
                    {
                        if (user_info_ting_apply.jjr_name.IsNullOrEmpty()) throw new Exception("请输入经纪人");
                    }

                    if (user_info_ting_apply.status > ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte()) throw new Exception("已审批通过不可修改");

                        

                    //调用抖音接口根据抖音账号查询抖音昵称及抖音作者id（抖音官方主播唯一身份id）



                    Zhubo zhubo1 = new Zhubo();
                    dynamic dyCheckResult1 = zhubo1.VerificationDoUser(user_info_ting_apply.dy_account);

                    //查询抖音经纪人是否存在
                    dynamic dyInfo = zhubo1.VerificationJjr(user_info_ting_apply.jjr_name);

                    #endregion

                    #region 新增审批数据
                    if (user_info_ting_apply.id == 0)
                    {
                        user_info_ting_apply.apply_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    }

                    user_info_ting_apply.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    user_info_ting_apply.yy_user_sn = new UserIdentityBag().user_sn;
                    user_info_ting_apply.status = ModelDb.user_info_ting_apply.status_enum.等待超管审批.ToSByte();
                    user_info_ting_apply.InsertOrUpdate();
                    #endregion

                    #region 向超管推送审批消息
                    //new ServiceFactory.Sdk.WeixinSendMsg().Approve("目标user_sn","消息内容","url",new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo 
                    //{
                    //    person = new UserIdentityBag().username,
                    //    post_time = DateTime.Now
                    //});
                    #endregion

                    return result;
                }

                public JsonResultAction GetTing(JsonRequestAction req)
                {
                    var info = new JsonResultAction();
                    info.data = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                    {
                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                        {
                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                            UserSn = req.GetPara("tg_user_sn"),
                        },
                        attachWhere = "p_ting_sn = ''",
                    }).ToJson();
                    return info;
                }
                #endregion
            }
            #endregion
        }
    }
}
