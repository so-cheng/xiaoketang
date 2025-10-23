using Services.Project._3.PageFactory.Work;
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
    public partial class PageFactory
    {
        public partial class Work
        {
            public partial class TodoDay
            {

                #region 
                /// <summary>
                /// 工作-待办-明细
                /// </summary>
                public class List
                {
                    #region DefaultView
                    /// <summary>
                    /// 获取页面数据模型
                    /// </summary>
                    /// <returns></returns>
                    public ModelBasic.PageList Get(DtoReq req)
                    {
                        var pageModel = new ModelBasic.PageList("pagelist");
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


                        switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                        {
                            case ModelEnum.UserTypeEnum.yyer:
                                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_sn")
                                {
                                    placeholder = "厅管",
                                    options = new ServiceFactory.UserInfo.Yy().YyGetNextTgForKv(new UserIdentityBag().user_sn),
                                });
                                break;
                            case ModelEnum.UserTypeEnum.jder:
                                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_sn")
                                {
                                    placeholder = "运营",
                                    options = new ServiceFactory.UserInfo.Zt().ZtGetNextYyForKv(new UserIdentityBag().user_sn),
                                });
                                break;
                        }
                        
                        listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                        {
                            mold = ModelBasic.EmtTimeSelect.Mold.date,
                            placeholder = "待办日期",
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
                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Post")
                        {
                            title = "Post",
                            text = "创建",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = "Post"
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
                        var listDisplay = new ModelBasic.CtlListDisplay();
                        listDisplay.operateWidth = "220";
                        listDisplay.isOpenNumbers = true;
                        listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                        {
                            funcGetListData = GetListData
                        };
                        #region 1.显示列       
                        switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                        {
                            case ModelEnum.UserTypeEnum.yyer:
                                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_sn_text")
                                {
                                    text = "厅管",
                                    width = "150",
                                    minWidth = "150"
                                });
                                break;
                            case ModelEnum.UserTypeEnum.jder:
                                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_sn_text")
                                {
                                    text = "运营",
                                    width = "150",
                                    minWidth = "150"
                                });
                                break;
                        }
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                        {
                            text = "待办日期",
                            width = "180",
                            minWidth = "180"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                        {
                            text = "待办内容",
                            width = "300",
                            minWidth = "300"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("status_text")
                        {
                            text = "状态",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("e_date_time")
                        {
                            text = "待办结束时间",
                            width = "180",
                            minWidth = "180"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("t_date_time")
                        {
                            text = "剩余待办时间",
                            width = "180",
                            minWidth = "180"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                        {
                            text = "创建时间",
                            width = "180",
                            minWidth = "180"
                        });


                        #endregion

                        #region 3.操作列
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

                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            name = "Del",
                            style = "",
                            text = "删除",
                            title = "删除",
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
                    public class DtoReq
                    {
                    }
                    #endregion
                    #region ListData
                    /// <summary>
                    /// data查询
                    /// </summary>
                    /// <returns></returns>
                    public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                    {
                        var req = reqJson.GetPara();
                        string where = $"1=1";

                        if (!req["tg_sn"].ToNullableString().IsNullOrEmpty())
                        {
                            where += $" and tg_sn = '{req["tg_sn"]}'";
                        }
                        if (!req["yy_sn"].ToNullableString().IsNullOrEmpty())
                        {
                            where += $" and yy_sn = '{req["yy_sn"]}'";
                        }
                        if (!reqJson.GetPara("c_date").IsNullOrEmpty())
                        {
                            where += $" and c_date = '{req["c_date"]}'";
                        }
                        //执行查询
                        var filter = new DoMySql.Filter
                        {
                            where = where,
                            orderby = " id desc"
                        };

                        var data = new ModelBasic.CtlListDisplay.ListData().getList<p_work_todo, ItemDataModel>(filter, reqJson); 
                        return data;
                    }
                    /// <summary>
                    /// 工作-待办-明细
                    /// </summary>
                    /// <param name="reqJson"></param>
                    /// <returns></returns>
                    public List<p_work_todo> GetWorkToDoData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                    {
                        var req = reqJson.GetPara();
                        string where = $"1=1";

                        if (!req["tg_sn"].ToNullableString().IsNullOrEmpty())
                        {
                            where += $" and tg_sn = '{req["tg_sn"]}'";
                        }
                        if (!reqJson.GetPara("c_date").IsNullOrEmpty())
                        {
                            where += $" and c_date = '{req["c_date"]}'";
                        }
                        //执行查询
                        var filter = new DoMySql.Filter
                        {
                            where = where,
                            orderby = " id desc"
                        };
                        return DoMySql.FindList<p_work_todo>(filter);
                    }
                    /// <summary>
                    /// 自定义筛选参数（自定义数据，与属性对应）
                    /// </summary>
                    public class DtoReqListData : ModelBasic.PageList.ListData.Req
                    {
                        /// <summary>
                        /// 
                        /// </summary>
                        public string name { get; set; }
                        public string id { get; set; }
                        public string status { get; set; }
                        public string parent_id { get; set; }

                    }
                    /// <summary>
                    /// 数据项模型
                    /// </summary>
                    public class ItemDataModel : ModelDb.p_work_todo
                    {
                        public string status_text
                        {
                            get
                            {
                                return ((status_enum)status).ToString();
                            }
                        }
                        public string c_date_text
                        {
                            get
                            {
                                return s_date_time.ToDate().ToString("yyyy-MM-dd");
                            }
                        }
                        public string tg_sn_text
                        {
                            get
                            {
                                return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_sn).name;
                            }
                        }
                        public string yy_sn_text
                        {
                            get
                            {
                                return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_sn).name;
                            }
                        }
                    }
                    #endregion
                    #region 批量删除操作
                    /// <summary>
                    /// 链接提交处理的回调函数
                    /// </summary>
                    /// <param name="req">回调函数提交统一的封装对象</param>
                    /// <returns></returns>
                    public JsonResultAction DeletesAction(JsonRequestAction req)
                    {
                        var result = new JsonResultAction();
                        var p_work_todo_day = req.data_json.ToModel<ModelDb.p_work_todo>();
                        p_work_todo_day.Delete();
                        return result;
                    }
                    #endregion
                }
                /// <summary>
                /// 
                /// </summary>
                public class Post
                {
                    #region DefaultView
                    public ModelBasic.PagePost Get(DtoReq req)
                    {
                        var pageModel = new ModelBasic.PagePost("");
                        pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                        {
                            returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                        };
                        pageModel.formDisplay = GetFormDisplay(pageModel, req);
                        pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                        {
                            func = PostAction
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
                        var p_work_todo_day = DoMySql.FindEntityById<ModelDb.p_work_todo>(req.id);

                        #region 表单元素
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                        {
                            defaultValue = p_work_todo_day.id.ToNullableString()
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("s_date_time")
                        {
                            title = "待办开始日期",
                            mold = ModelBasic.EmtTimeSelect.Mold.date,
                            defaultValue = p_work_todo_day.s_date_time.IsNullOrEmpty() ? "" : p_work_todo_day.s_date_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("e_date_time")
                        {
                            title = "待办结束时间",
                            mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                            defaultValue = p_work_todo_day.e_date_time.IsNullOrEmpty() ? DateTime.Today.AddHours(18).ToString("yyyy-MM-dd HH:mm:ss") : p_work_todo_day.e_date_time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                        {
                            case ModelEnum.UserTypeEnum.yyer:
                                var yy_sn = new UserIdentityBag().user_sn;
                                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("tg_sn")
                                {
                                    title = "厅管",
                                    bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_sn),
                                    defaultValue = p_work_todo_day.tg_sn,
                                    colLength = 12
                                });
                                break;
                            case ModelEnum.UserTypeEnum.jder:
                                var zt_sn = new UserIdentityBag().user_sn;
                                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("yy_sn")
                                {
                                    title = "运营",
                                    bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.基地邀运营, zt_sn),
                                    defaultValue = p_work_todo_day.tg_sn,
                                    colLength = 12
                                });
                                break;
                        }
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("content")
                        {
                            title = "待办事项",
                            defaultValue = p_work_todo_day.content,
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("sort")
                        {
                            title = "排序号",
                            defaultValue = p_work_todo_day.sort.ToString(),
                        });

                        #endregion
                        return formDisplay;
                    }
                    public class DtoReq
                    {
                        public int id { get; set; }
                    }
                    #endregion
                    #region 
                    /// <summary>
                    /// 表单提交处理的回调函数
                    /// </summary>
                    /// <param name="req">回调函数提交参数统一的封装对象</param>
                    /// <returns></returns>
                    public JsonResultAction PostAction(JsonRequestAction req)
                    {
                        var result = new JsonResultAction();
                        List<string> lSql = new List<string>();
                        var p_work_todo_day = req.data_json.ToModel<ModelDb.p_work_todo>();
                        if (p_work_todo_day.s_date_time.IsNullOrEmpty()) throw new WeicodeException("请选择待办日期!");
                        if (req.GetPara("tg_sn").IsNullOrEmpty() && req.GetPara("yy_sn").IsNullOrEmpty()) throw new WeicodeException("请选择厅管或者运营!");
                        if (p_work_todo_day.content.IsNullOrEmpty()) throw new WeicodeException("请输入待办事项!");

                        var tg_sn = req.GetPara("tg_sn").Split(',').ToList();
                        var yy_sn = req.GetPara("yy_sn").Split(',').ToList();
                        if (!string.IsNullOrEmpty(req.GetPara("tg_sn")))
                        {
                            p_work_todo_day.yy_sn = new UserIdentityBag().user_sn;
                            p_work_todo_day.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                            foreach (var user_sn in tg_sn)
                            {
                                p_work_todo_day.tg_sn = user_sn;
                                lSql.Add(p_work_todo_day.InsertOrUpdateTran());
                            }
                        }
                        else
                        {
                            p_work_todo_day.zt_sn = new UserIdentityBag().user_sn;
                            p_work_todo_day.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                            foreach (var user_sn in yy_sn)
                            {
                                p_work_todo_day.yy_sn = user_sn;
                                lSql.Add(p_work_todo_day.InsertOrUpdateTran());
                            }

                        }
                        DoMySql.ExecuteSqlTran(lSql);
                        return result;
                    }
                    /// <summary>
                    /// 定义表单模型
                    /// </summary>
                    public class DtoReqData : ModelDb.p_work_todo
                    {
                    }
                    #endregion
                }

                #endregion

            }
        }
    }
}
