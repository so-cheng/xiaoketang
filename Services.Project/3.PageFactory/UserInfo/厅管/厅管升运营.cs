using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.UserInfo.Tg;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            /// <summary>
            /// 厅管列表页面
            /// </summary>
            public class TgPromotionYy
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
                    pageModel.listDisplay = GetListDisplay(req);
                    pageModel.listFilter.isExport = false;
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
                    var user_type = new ServiceFactory.UserInfo().GetUserType();
                    if (user_type == ModelEnum.UserTypeEnum.manager)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("zt_user_sn")
                        {
                            placeholder = "基地账号",
                            options = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = '{ModelEnum.UserTypeEnum.jder.ToInt()}'", "username,user_sn"),
                            eventJsChange = new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    attachPara = new Dictionary<string, object>
                                    {
                                        {"zt_user_sn","<%=page.zt_user_sn.value%>"}
                                    },
                                    func = GetYy,
                                    resCallJs = $"{new ModelBasic.EmtSelect.Js("yy_user_sn").options(@"JSON.parse(res.data)")};"
                                }
                            },
                        });
                    }
                    if (user_type == ModelEnum.UserTypeEnum.manager || user_type == ModelEnum.UserTypeEnum.jder)
                    {
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()
                            {
                                attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            }),
                        });
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "100px",
                        placeholder = "厅管昵称"
                    });
                    return listFilter;
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "昵称",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "120",
                        minWidth = "120"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_text")
                    {
                        text = "所属运营",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_sn_text")
                    {
                        text = "上级厅管",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "厅管名",
                        width = "120",
                        minWidth = "120"
                    });
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Promotion",
                        text = "升级运营",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new EmtModel.ListOperateItem.EventOpenLayer()
                        {
                            url = "Post",
                            field_paras = "user_sn"
                        },
                    });


                    #endregion 操作列按钮

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public string yy_user_sn { get; set; }
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"user_base.status = '{ModelDb.user_base.status_enum.正常.ToSByte()}' and user_type_id = '{ModelEnum.UserTypeEnum.tger.ToInt()}' and user_base.tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                    if (!dtoReqListData.yy_user_sn.IsNullOrEmpty())
                    {
                        where += $" AND (user_base.user_sn IN (SELECT t_user_sn FROM user_relation WHERE relation_type_id = 2 AND f_user_sn = '{dtoReqListData.yy_user_sn}'))";
                    }

                    //查询条件
                    if (!dtoReqListData.keyword.IsNullOrEmpty()) where += $" AND (user_base.name like '%{dtoReqListData.keyword}%' OR user_base.username like '%{dtoReqListData.keyword}%')";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by user_base.id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    ///
                    /// </summary>
                    public string keyword { get; set; }
                    public string yy_user_sn { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string yy_user_sn_text
                    {
                        get
                        {
                            string yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.user_sn);
                            return new DomainBasic.UserApp().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }

                    public string tg_user_sn_text
                    {
                        get
                        {
                            string tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, this.user_sn);
                            return new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn).name;
                        }
                    }

                }

                #endregion ListData

                #region 异步请求处理

                /// <summary>
                /// 
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();


                    var user_base = new DomainBasic.UserApp().GetInfoById(dtoReqData.id.ToInt());

                    var user_type = new DomainBasic.UserTypeApp();

                    if (user_base.user_type_id == user_type.GetInfoByCode("tger").id)
                    {
                        new ServiceFactory.UserInfo.Tg().DeleteTg(user_base.user_sn);
                    }

                    return result;
                }

                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
                }


                /// <summary>
                /// 获取运营选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetYy(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    result.data = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                            UserSn = reqJson.GetPara("zt_user_sn"),
                        },
                        status = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.Status.正常
                    }).ToJson();
                    return result;
                }
                #endregion 异步请求处理
            }

            /// <summary>
            /// 创建
            /// </summary>
            public class TgUpgradeYyPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
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
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("tg_user_sn")
                    {
                        title = "厅管sn",
                        isRequired = true,
                        defaultValue = req.tg_user_sn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("tg_username")
                    {
                        title = "账号",
                        isRequired = true
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                    {
                        title = "设置密码",
                        isRequired = true,
                        defaultValue = "123456"
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public string tg_user_sn { get; set; }
                }
                #endregion
                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    ServiceFactory.UserInfo.Tg info = new ServiceFactory.UserInfo.Tg();
                    info.TgPromotionYy(req.GetPara("tg_user_sn").ToString(), req.GetPara("tg_username").ToString(), req.GetPara("password").ToString());
                    return result;
                }

                #endregion
            }

        }
    }
}
