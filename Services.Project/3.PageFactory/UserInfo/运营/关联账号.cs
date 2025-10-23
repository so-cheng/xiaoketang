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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        public partial class UserInfo
        {

            #region 运营
            /// <summary>
            /// 查看关联账号
            /// </summary>
            public class WithList
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("f_yy_user_sn")
                    {
                        placeholder = "主运营",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("WithPost")
                    {
                        title = "WithPost",
                        text = "新增",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "WithPost",
                        },
                        
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("f_yy_name")
                    {
                        text = "主运营",
                        width = "120",
                        minWidth = "120",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("t_yy_name")
                    {
                        text = "关联运营",
                        width = "420",
                        minWidth = "420"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("able_names")
                    {
                        text = "可查看模块",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "是否启用",
                        width = "100",
                        minWidth = "100"
                    });
                    
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "WithPost",
                            field_paras = "id"
                        },
                        style = "",
                        text = "编辑",
                        name = "Post"
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "删除",
                        title = "提示说明",
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
                    string where = $"tenant_id =  status != '{ModelDb.user_info_yy_with.status_enum.逻辑删除.ToSByte()}'";

                    //查询条件
                    if (!req["f_yy_user_sn"].IsNullOrEmpty())
                    {
                        where += $" and f_yy_user_sn = '{req["f_yy_user_sn"].ToString()}'";
                    }


                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby=" create_time desc",
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_yy_with, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_yy_with
                {
                    public string f_yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(f_yy_user_sn).username;
                        }
                    }
                    public string t_yy_name
                    {
                        get
                        {
                            var t_yy_user_sns = this.t_yy_user_sns.Split(',');
                            string yy_names = "";
                            foreach (var item in t_yy_user_sns) 
                            {
                                yy_names += new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(item).username+",";
                            }
                            if (yy_names.Contains(","))
                            {
                                yy_names = yy_names.Substring(0,yy_names.Length-1);
                            }
                            return yy_names;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            string text = "";
                            if (status == ModelDb.user_info_yy_with.status_enum.正常.ToSByte())
                            {
                                text = "启用";
                            }
                            if (status == ModelDb.user_info_yy_with.status_enum.禁用.ToSByte())
                            {
                                text = "禁用";
                            }
                            return text;
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 批量删除资产
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();

                    var user_info_yy_with = DoMySql.FindEntityById<ModelDb.user_info_yy_with>(req.GetPara("id").ToInt());
                    user_info_yy_with.Delete();

                    return result;
                }
                #endregion
            }


            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class WithPost
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
                    var user_info_yy_with = DoMySql.FindEntityById<ModelDb.user_info_yy_with>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = user_info_yy_with.id.ToNullableString()
                    });
                    //formDisplay.formItems.Add(new ModelBasic.EmtInput("tg_username")
                    //{
                    //    title = "厅管账号",
                    //    defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(req.tg_user_sn).username,
                    //    colLength = 6,
                    //    index = 0,
                    //});

                    


                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("f_yy_user_sn")
                    {
                        title = "主运营",
                        defaultValue = user_info_yy_with.f_yy_user_sn,
                        colLength = 6,
                        index = 100,
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "f_yy_user_sn","<%=page_post.f_yy_user_sn.value%>"}
                                },
                                func = GetTYyUserSn,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("post.t_yy_user_sns").options(@"JSON.parse(res.data)")};"
                            }
                        }
                    });


                    
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("t_yy_user_sns")
                    {
                        title = "关联运营",
                        colLength = 6,
                        defaultValue= user_info_yy_with.t_yy_user_sns,
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        index = 110,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("able_names")
                    {
                        //todo:需要加上模式字典
                        title = "可查看模块",
                        options = new Dictionary<string, string> 
                        {
                            {"数据表格","数据表格"},
                            {"运营4.0","运营4.0"}
                        },
                        defaultValue = user_info_yy_with.able_names,
                        colLength = 6,
                        index = 120,
                    });
                    string default_status = ModelDb.user_info_yy_with.status_enum.正常.ToSByte().ToString();
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("status")
                    {
                        title = "是否启用",
                        isRequired = true,
                        colLength = 6,
                        options=new Dictionary<string, string>
                        {
                            {ModelDb.user_info_yy_with.status_enum.正常.ToString(),ModelDb.user_info_yy_with.status_enum.正常.ToSByte().ToString()},
                            {ModelDb.user_info_yy_with.status_enum.禁用.ToString(),ModelDb.user_info_yy_with.status_enum.禁用.ToSByte().ToString()}
                        },
                        defaultValue = user_info_yy_with.status.ToString().IsNullOrEmpty() ? default_status: user_info_yy_with.status.ToString(),
                        index = 130,
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
                    var user_info_yy_with = req.GetPara<ModelDb.user_info_yy_with>();

                    if (user_info_yy_with.f_yy_user_sn.IsNullOrEmpty())
                    {
                        throw new Exception("请选择主运营");
                    }
                    if (user_info_yy_with.t_yy_user_sns.IsNullOrEmpty())
                    {
                        throw new Exception("请选择关联运营");
                    }

                    var t_yy_user_sns = user_info_yy_with.t_yy_user_sns.Split(',');
                    foreach (var item in t_yy_user_sns)
                    {
                        var _user_info_yy_with = DoMySql.FindEntity<ModelDb.user_info_yy_with>($"f_yy_user_sn = '{user_info_yy_with.f_yy_user_sn}' and t_yy_user_sns = '{item}' and status != '{ModelDb.user_info_yy_with.status_enum.逻辑删除.ToSByte()}'", false);
                        if (user_info_yy_with.f_yy_user_sn == item)
                        {
                            throw new Exception($"运营:{new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_info_yy_with.f_yy_user_sn).username}是主运营,请勿在关联运营中选择");
                        }

                        if (!_user_info_yy_with.IsNullOrEmpty())
                        {
                            throw new Exception($"主运营:{new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_info_yy_with.f_yy_user_sn).username}的关联运营:{new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(item).username}已存在,不能重复提交");
                        }
                    }
                    user_info_yy_with.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    user_info_yy_with.InsertOrUpdate();
                    return result;
                }


                /// <summary>
                /// 获取厅管筛选项
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public JsonResultAction GetTYyUserSn(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    var req = reqJson.GetPara();
                    var option = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                    var f_yy = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(req["f_yy_user_sn"].ToString());
                    try
                    {
                        option.Remove(f_yy.username);
                    }
                    catch
                    {

                    }
                    result.data = option.ToJson();
                    return result;
                }
                #endregion
            }
            #endregion

        }
    }
}
