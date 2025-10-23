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
    /// 流量目标
    /// </summary>
    public partial class PageFactory
    {
        public partial class LiuLiang
        {
            #region 目标列表页面
            /// <summary>
            /// 目标记录
            /// </summary>
            public class TargetList
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                    {
                        width = "120px",
                        mold = ModelBasic.EmtTimeSelect.Mold.month,
                        placeholder = "绩效年月",
                    });

                    switch (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).organize_wt)
                    {
                        // 主管
                        case 2:
                            listFilter.formItems.Add(new ModelBasic.EmtSelect("wx_user_sn")
                            {
                                width = "120px",
                                placeholder = "流量用户",
                                options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and organize_id = {new UserIdentityBag().cur_organize_id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()}", "username,user_sn")
                            });
                            break;
                    }

                    return listFilter;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    switch (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).organize_wt)
                    {
                        // 主管
                        case 2:
                            buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                            {
                                text = "新增",
                                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                                {
                                    url = $"Post",
                                }
                            });
                            break;
                    }

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
                    listDisplay.operateWidth = "70";
                    switch (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).organize_wt)
                    {
                        // 员工
                        case 1:
                            listDisplay.isHideOperate = true;
                            break;
                    }
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yearmonth")
                    {
                        text = "绩效年月",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("org_name")
                    {
                        text = "团队",
                        width = "120",
                        minWidth = "120"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wx_name")
                    {
                        text = "流量用户",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num")
                    {
                        text = "目标推出",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "160",
                        minWidth = "160"
                    });
                    #region 操作列按钮

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

                    switch (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).organize_wt)
                    {
                        // 员工
                        case 1:
                            where += $" and wx_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        // 主管
                        case 2:
                            where += $" and wx_user_sn in (select user_sn from user_base where organize_id = {new UserIdentityBag().cur_organize_id})";
                            break;
                    }

                    var req = reqJson.GetPara();
                    if (!req["yearmonth"].ToNullableString().IsNullOrEmpty()) where += $" AND yearmonth = '{req["yearmonth"]}'";
                    if (!req["wx_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND wx_user_sn = '{req["wx_user_sn"]}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_liuliang_target, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_liuliang_target
                {
                    public string org_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.sys_organize>($"id = {new DomainBasic.UserApp().GetInfoByUserSn(wx_user_sn).organize_id}", false).name;
                        }
                    }
                    public string wx_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(wx_user_sn).username;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region 目标新增/编辑页面
            /// <summary>
            /// 目标新增/编辑
            /// </summary>
            public class TargetPost
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
                    var p_liuliang_target = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_liuliang_target>($"id = {req.id}", false);
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtTimeSelect("yearmonth")
                    {
                        title = "绩效年月",
                        colLength = 6,
                        isRequired = true,
                        mold = EmtTimeSelect.Mold.month,
                        defaultValue = p_liuliang_target.id > 0 ? p_liuliang_target.yearmonth : DateTime.Today.ToString("yyyy-MM")
                    });

                    // 主管可选择自己部门的流量用户
                    var options = new Dictionary<string, string>();
                    switch (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).organize_wt)
                    {
                        // 主管
                        case 2:
                            options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("wxer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and organize_id = {new UserIdentityBag().cur_organize_id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()}", "username,user_sn");
                            break;
                    }

                    formDisplay.formItems.Add(new EmtSelect("wx_user_sn")
                    {
                        title = "流量用户",
                        colLength = 6,
                        isRequired = true,
                        placeholder = "请选择",
                        options = options,
                        defaultValue = p_liuliang_target.wx_user_sn
                    });

                    formDisplay.formItems.Add(new EmtInput("num")
                    {
                        title = "目标推出",
                        colLength = 6,
                        isRequired = true,
                        defaultValue = p_liuliang_target.num.ToString()
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
                    var p_liuliang_target = req.data_json.ToModel<DtoReqData>();
                    if (p_liuliang_target.yearmonth.IsNullOrEmpty()) throw new Exception("请选择绩效年月");
                    if (p_liuliang_target.wx_user_sn.IsNullOrEmpty()) throw new Exception("请选择流量用户");
                    if (p_liuliang_target.num.IsNullOrEmpty()) throw new Exception("请输入目标推出");

                    p_liuliang_target.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    p_liuliang_target.ToModel<ModelDb.p_liuliang_target>().InsertOrUpdate($"wx_user_sn = '{p_liuliang_target.wx_user_sn}' and yearmonth = '{p_liuliang_target.yearmonth}'");

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_liuliang_target
                {

                }
                #endregion
            }
            #endregion
        }
    }
}
