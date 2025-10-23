using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
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
            public class AccountList
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
                    var options = new Dictionary<string, string>();
                    foreach (var item in DoMySql.FindList<ModelDb.user_type>("")) 
                    {
                        options.Add(item.name,item.id.ToString());
                    }

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("user_type_id")
                    {
                        width = "120px",
                        placeholder = "用户类型",
                        options = options
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtInput("keyword")
                    {
                        width = "160px",
                        placeholder = "账号/昵称/手机号"
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
                    var buttonGroup = new ModelBasic.EmtButtonGroup("EmtButtonGroup");

                    //buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("UnDel")
                    //{
                    //    title = "UnDel",
                    //    text = "恢复账号",
                    //    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    //    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    //    {
                    //        url = "UnDel"
                    //    },
                    //    disabled = true
                    //});

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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_type")
                    {
                        text = "类型",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("username")
                    {
                        text = "登录账号",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "昵称",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                    {
                        text = "手机号",
                        width = "140",
                        minWidth = "140"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                    {
                        text = "状态",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "180",
                        minWidth = "180"
                    });
                    #region 操作列按钮


                    string user_sn = "";
                    string secret = UtilityStatic.Md5.getMd5(user_sn + UtilityStatic.ConfigHelper.GetConfigString("AuthorizedKey"));
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "编辑",
                        name = "Edit",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer=new EmtModel.ListOperateItem.EventOpenLayer() 
                        { 
                            url = "/Basic/Manager/Edit",
                            field_paras = "id"
                        }
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        style = "",
                        text = "登录",
                        name = "login",
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                        eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                        {
                            url = "fastlogin",
                            field_paras = "user_sn",
                            target = "_bank",
                        }
                    });

                    #endregion 操作列按钮

                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {

                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";


                    //查询条件
                    if (!reqJson.GetPara("keyword").IsNullOrEmpty()) 
                    { 
                        where += $" AND (name like '%{reqJson.GetPara("keyword")}%' OR username like '%{reqJson.GetPara("keyword")}%' OR  mobile like '%{reqJson.GetPara("keyword")}%')"; 
                    }

                    if (!reqJson.GetPara("user_type_id").IsNullOrEmpty())
                    {
                        where += $" AND user_type_id = '{reqJson.GetPara("user_type_id")}'";
                    }

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ItemDataModel>(filter, reqJson);
                }


                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string user_type
                    {
                        get
                        {
                            return new DomainBasic.UserTypeApp().GetInfoById(user_type_id).name;
                        }
                    }
                    public string status_text
                    {
                        get
                        {
                            return status.ToEnum<ModelDb.user_base.status_enum>().ToString();
                        }
                    }
                }

                #endregion ListData

                #region 异步请求处理

                #endregion
            }

        }
    }
}
