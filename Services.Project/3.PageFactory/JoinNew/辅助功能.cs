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
    /// 辅助功能模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// 萌新数据列表（拉群期数）
            /// </summary>
            public class PeixunList
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("date", true)
                    {
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        placeholder = "拉群日期",
                    });


                    return listFilter;
                }



                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                /// 
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");

                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "新增",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Post",
                        }
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                    {
                        text = "拉群期数",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn_text")
                    {
                        text = "培训老师",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("date_text")
                    {
                        text = "拉群日期",
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
                        text = "详情",
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
                        public string user_sn { get; set; }
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
                    string where = $"1=1";

                    var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by date desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_mengxin, ItemDataModel>(filter, reqJson);
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
                    public string user_sn { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_mengxin
                {
                    public string date_text
                    {
                        get
                        {
                            return this.date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string user_sn_text
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{this.user_sn}'").username;
                        }
                    }
                    public string tg_user_sn_text
                    {
                        get
                        {
                            var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{tg_user_sn}'", false).username;
                        }
                    }

                    public string yy_user_sn_text
                    {
                        get
                        {
                            var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, this.user_sn);
                            var yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, tg_user_sn);
                            return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{yy_user_sn}'", false).username;
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
                    var p_mengxin = req.data_json.ToModel<p_mengxin>();
                    var result = new JsonResultAction();
                    p_mengxin.Delete();
                    return result;
                }
                #endregion
            }

            /// <summary>
            /// 拉群培训数据新增/编辑
            /// </summary>
            public class PeixunPost
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
                    var p_mengxin = DoMySql.FindEntityById<ModelDb.p_mengxin>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_mengxin.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_sn")
                    {
                        defaultValue = p_mengxin.IsNullOrEmpty() ? new UserIdentityBag().user_sn : p_mengxin.user_sn,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "基本信息",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("term")
                    {
                        title = "拉群期数",
                        placeholder = "",
                        defaultValue = p_mengxin.term,
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("date")
                    {
                        title = "拉群日期",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        placeholder = "",
                        defaultValue = req.id == 0 ? DateTime.Today.ToString("yyyy-MM-dd") : p_mengxin.date.ToDate().ToString("yyyy-MM-dd"),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("group_num")
                    {
                        title = "拉群人数",
                        placeholder = "",
                        defaultValue = p_mengxin.group_num.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("join_num")
                    {
                        title = "入会人数",
                        placeholder = "",
                        defaultValue = p_mengxin.join_num.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "第一天信息",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("in_group_1")
                    {
                        title = "第一天在群人数",
                        placeholder = "",
                        defaultValue = p_mengxin.in_group_1.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("in_class_1")
                    {
                        title = "第一天到课人数",
                        placeholder = "",
                        defaultValue = p_mengxin.in_class_1.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("playback_1")
                    {
                        title = "第一天回放人数",
                        placeholder = "",
                        defaultValue = p_mengxin.playback_1.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "第二天信息",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("in_group_2")
                    {
                        title = "第二天课前人数",
                        placeholder = "",
                        defaultValue = p_mengxin.in_group_2.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("before_class_2")
                    {
                        title = "第二天课后人数",
                        placeholder = "",
                        defaultValue = p_mengxin.before_class_2.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("in_class_2")
                    {
                        title = "第二天到课人数",
                        placeholder = "",
                        defaultValue = p_mengxin.in_class_2.ToString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("playback_2")
                    {
                        title = "第二天回放人数",
                        placeholder = "",
                        defaultValue = p_mengxin.playback_2.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "第三天信息",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("leave_group_3")
                    {
                        title = "课程结束后退会人数",
                        placeholder = "",
                        defaultValue = p_mengxin.leave_group_3.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                    {
                        title = "统计信息",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ignore_num")
                    {
                        title = "在群不理",
                        placeholder = "",
                        defaultValue = p_mengxin.ignore_num.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("no_exam_num")
                    {
                        title = "理了没考",
                        placeholder = "",
                        defaultValue = p_mengxin.no_exam_num.ToString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("no_job_num")
                    {
                        title = "未分配人数",
                        placeholder = "",
                        defaultValue = p_mengxin.no_job_num.ToString(),
                        colLength = 6
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
                    var lSql = new List<string>();
                    var p_mengxin = req.data_json.ToModel<ModelDb.p_mengxin>();

                    if (p_mengxin.id == 0)
                    {
                        p_mengxin.user_sn = new UserIdentityBag().user_sn;
                        p_mengxin.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    }

                    var entity = DoMySql.FindEntity<ModelDb.p_mengxin>($"user_sn='{p_mengxin.user_sn}' and date='{p_mengxin.date}'", false);
                    if (!entity.IsNullOrEmpty() && p_mengxin.id != entity.id)
                    {
                        throw new Exception($@"拉群日期:""{p_mengxin.date.ToDate().ToString("yyyy-MM-dd")}"" 当天资料已提交过,请勿重复提交,请点击当天资料的""详情""进行修改");
                    }
                    p_mengxin.leave_group_1 = p_mengxin.group_num - p_mengxin.in_group_1;
                    p_mengxin.leave_group_2 = p_mengxin.in_group_1 - p_mengxin.before_class_2;
                    lSql.Add(p_mengxin.InsertOrUpdateTran());

                    foreach (var item in DoMySql.FindList<ModelDb.p_mengxin>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and date='{p_mengxin.date}' and id != '{p_mengxin.id}'"))
                    {
                        item.join_num = p_mengxin.join_num;
                        lSql.Add(item.UpdateTran());
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }

            public class p_mengxin : ModelDb.p_mengxin
            {
                public int Count { get; set; }
            }
        }
    }
}
