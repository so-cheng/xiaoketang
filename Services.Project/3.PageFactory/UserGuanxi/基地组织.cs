using System;
using System.Collections.Generic;
using System.Linq;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Modular;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserGuanxi
        {
            #region 基地关联列表
            /// <summary>
            /// 基地关联列表
            /// </summary>
            public class JdOrganizeList
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
                    listDisplay.operateWidth = "120";

                    #region 显示列

                    listDisplay.listData = new CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                    };
                    listDisplay.listItems.Add(new EmtModel.ListItem("username")
                    {
                        text = "基地名称",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new EmtModel.ListItem("org_name")
                    {
                        text = "组织部门",
                        width = "120",
                        minWidth = "120",
                    });
                    #endregion 显示列
                    #region 操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "SetOrganize",
                            field_paras = "user_sn"
                        },
                        text = "设置组织部门",
                        name = "SetOrganize"
                    });

                    #endregion
                    return listDisplay;
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
                public class DtoReq
                {
                }

                #endregion DefaultView

                #region ListData

                /// <summary>
                /// 获取当前登录厅管的申请记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.筛选条件
                    string where = $"user_base.user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("jder").id} and user_base.status = {ModelDb.user_base.status_enum.正常.ToSByte()}";

                    //2.执行查询
                    var filter = new DoMySql.Filter
                    {
                        fields = "user_base.user_sn,user_base.username,user_relation__organize.organize_id",
                        where = where,
                        on = "user_base.user_sn = user_relation__organize.jd_user_sn",
                        orderby = "order by user_base.id"
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.user_base, ModelDb.user_relation__organize, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_base
                {
                    public string org_name
                    {
                        get
                        {
                            return organize_id > 0 ? DoMySql.FindEntityById<ModelDb.sys_organize>(organize_id).name : "";
                        }
                    }
                }

                #endregion ListData
            }
            #endregion

            #region 设置组织部门
            /// <summary>
            /// 基地关联组织部门
            /// </summary>
            public class SetOrganize
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
                    var user_relation__organize = DoMySql.FindEntity<ModelDb.user_relation__organize>($"jd_user_sn = '{req.user_sn}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtHidden("user_sn")
                    {
                        defaultValue = req.user_sn
                    });
                    formDisplay.formItems.Add(new EmtLabel("user_name")
                    {
                        title = "基地名称",
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(req.user_sn).username
                    });
                    formDisplay.formItems.Add(new EmtSelect("organize_id")
                    {
                        title = "组织部门",
                        options = DoMySql.FindKvList<ModelDb.sys_organize>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("zter").id}", "name,id"),
                        defaultValue = user_relation__organize.organize_id.ToString()
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    /// 基地sn
                    /// </summary>
                    public string user_sn { get; set; }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 异步请求处理
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //1.数据校验
                    if (req.GetPara("organize_id").IsNullOrEmpty()) throw new WeicodeException("请选择组织部门");

                    //2.执行语句
                    var user_sn = req.GetPara("user_sn");
                    new ModelDb.user_relation__organize()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        organize_id = req.GetPara("organize_id").ToInt(),
                        jd_user_sn = user_sn
                    }.InsertOrUpdate($"jd_user_sn = '{user_sn}'");

                    return new JsonResultAction();
                }
                #endregion
            }
            #endregion
        }
    }
}