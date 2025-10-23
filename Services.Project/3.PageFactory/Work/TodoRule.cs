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

        public partial class Work
        {
            public partial class TodoRule
            {
                #region 列表界面
                /// <summary>
                /// 工作-待办-规则设置
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
                        var pageModel = new PageList("pagelist");
                        //设置tab页

                        string top = "";
                        top += $@"<div class=""layui-tab layui-tab-brief"">";
                        top += $@"   <ul class=""layui-tab-title"">";
                        top += $@"       <li {(req.status == 0 ? @"class=""layui-this""" : "")} lay-id=""0"" onclick=""location.href='?status=0'"">正在进行中</li>";
                        top += $@"       <li {(req.status == 1 ? @"class=""layui-this""" : "")} lay-id=""1"" onclick=""location.href='?status=1'"">未开始</li>";
                        top += $@"       <li {(req.status == 2 ? @"class=""layui-this""" : "")} lay-id=""2"" onclick=""location.href='?status=2'"">已经结束</li>";
                        top += $@"   </ul>";
                        top += $@"</div>";

                        pageModel.topPartial = new List<ModelBase>
                    {
                        new ModelBasic.EmtHtml("html_top")
                        {
                            Content = top
                        }
                    };
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
                        listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_sn")
                        {
                            placeholder = "运营账号",
                            options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                            {
                                attachWhere = $"status = '{ModelDb.user_base.status_enum.正常.ToSByte()}'"
                            }),
                        });
                        listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                        {
                            mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                            placeholder = "创建时间",
                        });
                        listFilter.formItems.Add(new ModelBasic.EmtHidden("status")
                        {
                            defaultValue = req.status.ToString()
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
                        var buttonGroup = new ModelBasic.EmtButtonGroup("create");
                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Post")
                        {
                            title = "Post",
                            text = "创建规则",
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
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_sn_test")
                        {
                            text = "所属运营",
                            width = "200",
                            minWidth = "200",
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                        {
                            text = "待办内容",
                            width = "300",
                            minWidth = "300"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sort")
                        {
                            text = "排序号",
                            width = "120",
                            minWidth = "120"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_rule")
                        {
                            text = "重复规则",
                            width = "200",
                            minWidth = "200"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("s_time")
                        {
                            text = "开始时间",
                            width = "200",
                            minWidth = "200"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("e_time")
                        {
                            text = "结束时间",
                            width = "200",
                            minWidth = "200"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                        {
                            text = "创建时间",
                            width = "200",
                            minWidth = "200"
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
                        /// <summary>
                        /// 状态
                        /// </summary>
                        public int status { get; set; }

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

                        DateTime currentTime = DateTime.Now;

                        string where = $"1=1";
                        switch (req["status"].ToInt())
                        {
                            case 0:
                                where += $" and s_time < '{DateTime.Now}' and e_time > '{DateTime.Now}'";
                                break;
                            case 1:

                                where += $" and s_time >'{DateTime.Now}'";
                                break;
                            case 2:
                                where += $" and e_time < '{DateTime.Now}'";
                                break;
                        }

                        if (!req["yy_sn"].ToNullableString().IsNullOrEmpty())
                        {
                            where += $" and yy_sn = '{req["yy_sn"]}'";
                        }
                        if (!reqJson.GetPara("create_time").IsNullOrEmpty())
                        {
                            var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("create_time"), 0);
                            where += $" and create_time >= '{dateRange.date_range_s}' and create_time<'{dateRange.date_range_e.ToDate().AddDays(1).ToString("yyyy-MM-dd")}'";
                        }
                        //执行查询
                        var filter = new DoMySql.Filter
                        {
                            where = where,
                            orderby = " yy_sn asc"
                        };
                        return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_work_todo_rule, ItemDataModel>(filter, reqJson);
                    }
                    /// <summary>
                    /// 自定义筛选参数（自定义数据，与属性对应）
                    /// </summary>
                    public class DtoReqListData : ModelBasic.PageList.ListData.Req
                    {
                    }
                    /// <summary>
                    /// 数据项模型
                    /// </summary>
                    public class ItemDataModel : ModelDb.p_work_todo_rule
                    {
                        public string yy_sn_test
                        {
                            get
                            {
                                return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_sn).username;
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
                        var p_work_todo_rule = req.data_json.ToModel<ModelDb.p_work_todo_rule>();
                        p_work_todo_rule.Delete();
                        return result;
                    }
                    #endregion
                }

                #endregion

                #region 新增/编辑界面
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
                        var p_work_todo_rule = DoMySql.FindEntityById<ModelDb.p_work_todo_rule>(req.id);

                        #region 表单元素
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                        {
                            defaultValue = p_work_todo_rule.id.ToNullableString()
                        });
                        var yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();

                        formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_sn")
                        {
                            title = "运营账号",
                            options = yy_options,
                            defaultValue = p_work_todo_rule.yy_sn
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("content")
                        {
                            title = "待办内容",
                            defaultValue = p_work_todo_rule.content
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("sort")
                        {
                            title = "排序号",
                            defaultValue = p_work_todo_rule.sort.ToString()
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtExt.EmtRepeatRule("c_rule")
                        {
                            title = "重复规则",
                            defaultValue = p_work_todo_rule.c_rule
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("s_time")
                        {
                            title = "开始时间",
                            mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                            defaultValue = p_work_todo_rule.s_time.IsNullOrEmpty() ? "" : p_work_todo_rule.s_time.ToDateTime().ToString()
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("e_time")
                        {
                            title = "结束时间",
                            mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                            defaultValue = p_work_todo_rule.e_time.IsNullOrEmpty() ? "" : p_work_todo_rule.e_time.ToDateTime().ToString()
                        });
                        #endregion
                        return formDisplay;
                    }
                    public class DtoReq
                    {
                        public int id { get; set; }
                    }
                    #endregion
                    #region 异步
                    /// <summary>
                    /// 创建/编辑工作-待办-规则
                    /// </summary>
                    /// <param name="req">回调函数提交参数统一的封装对象</param>
                    /// <returns></returns>
                    public JsonResultAction PostAction(JsonRequestAction req)
                    {
                        var result = new JsonResultAction();
                        try
                        {
                            var p_work_todo_rule = req.data_json.ToModel<ModelDb.p_work_todo_rule>();
                            if (p_work_todo_rule.content.IsNullOrEmpty()) throw new Exception("请输入待办内容");
                            if (p_work_todo_rule.c_rule.IsNullOrEmpty()) throw new Exception("请选择重复规则");
                            if (p_work_todo_rule.s_time.IsNullOrEmpty()) throw new Exception("请选择开始时间");
                            if (p_work_todo_rule.e_time.IsNullOrEmpty()) throw new Exception("请选择结束时间");

                            //判断当前用户类型是否为 "运营"，如果是，则将当前用户的编号（user_sn）赋值给待办规则的yy_sn字段
                            if (new DomainBasic.UserTypeApp().GetInfo().sys_code == ModelEnum.UserTypeEnum.yyer.ToString())
                            {
                                p_work_todo_rule.yy_sn = new UserIdentityBag().user_sn;
                            }
                            p_work_todo_rule.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                            p_work_todo_rule.InsertOrUpdate();
                        }
                        catch (Exception e)
                        {
                            result.code = 1;
                            result.msg = e.Message;
                        }

                        return result;
                    }
                    #endregion
                }

                #endregion
            }
        }
    }
}
