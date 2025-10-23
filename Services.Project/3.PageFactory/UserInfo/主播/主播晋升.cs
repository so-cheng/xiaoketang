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
using static Services.Project.ServiceFactory.UserInfo.Zhubo;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            //主播晋升列表
            public class ZbPromotionList
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
                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "140px",
                        placeholder = "主播昵称/抖音号"
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Create")
                    {
                        title = "Post",
                        text = "发起申请",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Creat"
                        },
                        disabled = true
                    });
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
                    listDisplay.operateWidth = "350";
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_name")
                    {
                        text = "主播昵称",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "所属厅",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("apply_cause")
                    {
                        text = "申请原因",
                        width = "250",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_name")
                    {
                        text = "审批状态",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("apply_time")
                    {
                        text = "审批时间",
                        width = "200",
                        minWidth = "200"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "申请时间",
                        width = "200",
                        minWidth = "200"
                    });
                    #region 操作列按钮

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Agree",
                        style = "",
                        text = "同意",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = PromotionZhuboApproval,
                            field_paras = "apply_sn",
                            attachPara = new Dictionary<string, object>
                            {
                                {
                                    "status", (sbyte)ModelDb.user_info_promotion_zhubo_apply.status_enum.同意
                                }
                            }
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Refuse",
                        style = "",
                        text = "拒绝",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = PromotionZhuboApproval,
                            field_paras = "apply_sn",
                            attachPara = new Dictionary<string, object>
                            {
                                {
                                    "status", (sbyte)ModelDb.user_info_promotion_zhubo_apply.status_enum.拒绝
                                }
                            }
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "删除",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DeletesAction,
                            field_paras = "apply_sn"
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Detail",
                        text = "详情",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer()
                        {
                            url = "Detail",
                            field_paras = "apply_sn"

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
                    /// <summary>
                    /// 
                    /// </summary>
                    public FilterForm filterForm { get; set; } = new FilterForm();

                    /// <summary>
                    /// 筛选参数（自定义）
                    /// </summary>
                    public class FilterForm
                    {
                        /// <summary>
                        /// 关键词
                        /// </summary>
                        public string keyword { get; set; }
                    }
                }
                #endregion


                #region ListData
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    ServiceFactory.UserInfo.ZbPromotion info = new ServiceFactory.UserInfo.ZbPromotion();
                    return info.GetZbPromotionPageList(reqJson);
                }
                #endregion
                #region 异步请求处理
                /// <summary>
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYy(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    result.data = new ServiceFactory.UserInfo.Yy().GetAllYyForKv().ToJson();
                    return result;
                }

                /// <summary>
                /// 删除申请
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    ServiceFactory.UserInfo.ZbPromotion info = new ServiceFactory.UserInfo.ZbPromotion();
                    info.DeletePromotionZhuboApply(req.GetPara("apply_sn").ToString());
                    return result;
                }

                /// <summary>
                /// 审批
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction PromotionZhuboApproval(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    ServiceFactory.UserInfo.ZbPromotion info = new ServiceFactory.UserInfo.ZbPromotion();
                    info.PromotionZhuboApproval(req.GetPara("apply_sn").ToString(), req.GetPara("status").ToInt());
                    return result;
                }

                #endregion


            }
            /// <summary>
            /// 主播晋升提交
            /// </summary>
            public class ZbPromotionPost
            {


                #region DefaultView

                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
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
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素

                    var zbBaseInfoFilter = new ZbBaseInfoFilter()
                    {
                        attachWhere = $"tg_user_sn = '{new UserIdentityBag().user_sn}'"
                    };

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("user_info_zb_sn")
                    {
                        title = "主播",
                        placeholder = "主播",
                        options = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(zbBaseInfoFilter),
                        disabled = true
                    });


                    formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_user")
                    {
                        title = "抖音号",
                        placeholder = "输入抖音号",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                    {
                        title = "经纪人",
                        placeholder = "输入经纪人名字",
                        isRequired = true
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                    {
                        title = "登录账号",
                        isRequired = true,
                        placeholder = "输入登录账号"
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                    {
                        title = "设置密码",
                        isRequired = true,
                        placeholder = "输入密码",
                        defaultValue = "123456"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true,
                        placeholder = "输入手机号"
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("apply_cause")
                    {
                        title = "申请原因",
                        placeholder = "输入申请原因"
                    });
                    #endregion 表单元素
                    return formDisplay;
                }

                #endregion


                #region 异步请求
                /// <summary>
                /// 发起申请
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();

                    var applyInfo = req.data_json.ToModel<ModelDb.user_info_promotion_zhubo_apply>();
                    ServiceFactory.UserInfo.ZbPromotion info = new ServiceFactory.UserInfo.ZbPromotion();
                    info.CommitPromotionZhuboApply(applyInfo);
                    return result;
                }


                public JsonResultAction GetZhubo(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new Dictionary<string, string>();
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString()))
                    {
                        option.Add(item.name, item.user_sn);
                    }
                    result.data = option.ToJson();
                    return result;
                }

                #endregion

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                }

            }
            /// <summary>
            /// 主播详情
            /// </summary>
            public class ZbPromotionDetail
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("detail");
                    var formDisplay = pageModel.formDisplay;
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
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
                    pageModel.disabled = true;
                    #region 表单元素
                    ServiceFactory.UserInfo.ZbPromotion info = new ServiceFactory.UserInfo.ZbPromotion();
                    var applyInfo = info.GetZbPromotionByID(req.apply_sn);
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("user_name")
                    {
                        title = "主播",
                        Content = applyInfo.user_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("dou_user")
                    {
                        title = "抖音号",
                        Content = applyInfo.dou_user
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("jjr_name")
                    {
                        title = "经纪人",
                        Content = applyInfo.jjr_name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("username")
                    {
                        title = "登录账号",
                        Content = applyInfo.username
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("password")
                    {
                        title = "设置密码",
                        Content = applyInfo.password
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("mobile")
                    {
                        title = "手机号",
                        Content = applyInfo.mobile
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("apply_cause")
                    {
                        title = "申请原因",
                        Content = applyInfo.apply_cause
                    });
                    #endregion 表单元素
                    return formDisplay;
                }
                #endregion
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public string apply_sn { get; set; }
                }
            }


            /// <summary>
            /// 数据项模型
            /// </summary>
            public class PromotionZhuboModel : ModelDb.user_info_promotion_zhubo_apply
            {

                public string user_name { get; set; }
                public string status_name
                {
                    get
                    {
                        return status.ToEnum<ModelDb.user_info_promotion_zhubo_apply.status_enum>();
                    }
                }
                public string tg_user_name
                {
                    get
                    {
                        if (tg_user_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                    }
                }

                public string ting_name
                {
                    get
                    {
                        if (ting_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }

                public string yy_user_name
                {
                    get
                    {
                        if (yy_user_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                    }
                }
            }

        }
    }
}
