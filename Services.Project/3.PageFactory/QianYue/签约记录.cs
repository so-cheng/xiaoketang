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
    /// 签约记录
    /// </summary>
    public partial class PageFactory
    {
        public partial class QianYue
        {
            #region 签约提报页面
            /// <summary>
            /// 签约提报
            /// </summary>
            public class Post
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
                    var p_qianyue_info = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_qianyue_info>($"id = {req.id}", false);
                    var formDisplay = pageModel.formDisplay;
                    #region 表单元素
                    formDisplay.formItems.Add(new EmtTimeSelect("c_date")
                    {
                        title = "绩效日期",
                        mold = EmtTimeSelect.Mold.date,
                        defaultValue = p_qianyue_info.id > 0 ? p_qianyue_info.c_date.ToDateString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd"),
                    });

                    formDisplay.formItems.Add(new EmtTextarea("info")
                    {
                        mode = EmtTextarea.Mode.TextArea,
                        title = "粘贴信息",
                        placeholder = @"数据格式：
XXX
添加微信：X
签约人数：X
签约男生：X
签约女生：X",
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = @"var info = page_post.info.value;

                                         var name = info.match(/^(.+)/)[1].trim();
                                         var wechat_num = info.match(/添加微信：(.+)/)[1].trim();
                                         var f_num = info.match(/签约人数：(.+)/)[1].trim();
                                         var qianyue_male = info.match(/签约男生：(.+)/)[1].trim();
                                         var qianyue_female = info.match(/签约女生：(.+)/)[1].trim();

                                         page_post.wechat_num.set(wechat_num);
                                         page_post.f_num.set(f_num);
                                         page_post.qianyue_male.set(qianyue_male);
                                         page_post.qianyue_female.set(qianyue_female);
                                         page_post.qianyue_rate.set(wechat_num == 0 ? 0 : Math.round(parseFloat(f_num) / parseFloat(wechat_num) * 100, 2));
                                         page_post.qy_name.set(name);",
                            }
                        }
                    });

                    formDisplay.formItems.Add(new EmtInput("wechat_num")
                    {
                        title = "添加微信",
                        colLength = 6,
                        defaultValue = p_qianyue_info.wechat_num.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("f_num")
                    {
                        title = "签约人数",
                        colLength = 6,
                        defaultValue = p_qianyue_info.f_num.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("qianyue_male")
                    {
                        title = "签约男生",
                        colLength = 6,
                        defaultValue = p_qianyue_info.qianyue_male.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("qianyue_female")
                    {
                        title = "签约女生",
                        colLength = 6,
                        defaultValue = p_qianyue_info.qianyue_female.ToString()
                    });

                    formDisplay.formItems.Add(new EmtInput("qianyue_rate")
                    {
                        title = "签约率",
                        colLength = 6,
                        defaultValue = p_qianyue_info.qianyue_rate.ToString(),
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });

                    formDisplay.formItems.Add(new EmtInput("qy_name")
                    {
                        title = "名字",
                        colLength = 6,
                        defaultValue = new DomainBasic.UserApp().GetInfoByUserSn(p_qianyue_info.qy_user_sn).username,
                        displayStatus = EmtModelBase.DisplayStatus.只读
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
                    var p_qianyue_info = req.data_json.ToModel<DtoReqData>();
                    if (p_qianyue_info.qy_name.IsNullOrEmpty()) throw new Exception("签约名字不能为空");
                    var qy_user = DoMySql.FindEntity<ModelDb.user_base>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and username = '{p_qianyue_info.qy_name}' and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id}", false);
                    if (qy_user.IsNullOrEmpty()) throw new Exception("未找到签约用户");

                    if (p_qianyue_info.c_date.IsNullOrEmpty()) throw new Exception("请选择绩效日期");
                    if (p_qianyue_info.wechat_num.IsNullOrEmpty()) throw new Exception("添加微信不能为空");
                    if (p_qianyue_info.f_num.IsNullOrEmpty()) throw new Exception("签约人数不能为空");
                    if (p_qianyue_info.qianyue_male.IsNullOrEmpty()) throw new Exception("签约男生不能为空");
                    if (p_qianyue_info.qianyue_female.IsNullOrEmpty()) throw new Exception("签约女生不能为空");

                    p_qianyue_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_qianyue_info.qy_user_sn = qy_user.user_sn;
                    p_qianyue_info.qianyue_rate = Math.Round(p_qianyue_info.f_num.ToDecimal() / p_qianyue_info.wechat_num.ToDecimal() * 100, 2);

                    p_qianyue_info.ToModel<ModelDb.p_qianyue_info>().InsertOrUpdate($"qy_user_sn = '{p_qianyue_info.qy_user_sn}' and c_date = '{p_qianyue_info.c_date}'");

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_qianyue_info
                {
                    // 签约名字
                    public string qy_name { get; set; }
                }
                #endregion
            }
            #endregion

            #region 提报记录列表页面
            /// <summary>
            /// 提报记录
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
                    listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        width = "120px",
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        placeholder = "绩效日期",
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("organize_id")
                    {
                        width = "150px",
                        placeholder = "所属团队",
                        options = DoMySql.FindKvList<ModelDb.sys_organize>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by sort", "name,id")
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("qy_user_sn")
                    {
                        width = "120px",
                        placeholder = "签约用户",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("qyer").id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_base.status_enum.正常.ToSByte()}", "username,user_sn")
                    });
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
                    listDisplay.operateWidth = "120";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "绩效日期",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("org_name")
                    {
                        text = "团队",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qy_name")
                    {
                        text = "签约用户",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_num")
                    {
                        text = "添加微信",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("f_num")
                    {
                        text = "签约人数",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_rate_text")
                    {
                        text = "签约率",
                        width = "90",
                        minWidth = "90"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_male")
                    {
                        text = "签约男生",
                        width = "110",
                        minWidth = "110"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_female")
                    {
                        text = "签约女生",
                        width = "110",
                        minWidth = "110"
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

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
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

                }
                #endregion

                #region 异步处理
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_qianyue_info = req.data_json.ToModel<ModelDb.p_qianyue_info>();
                    lSql.Add(p_qianyue_info.DeleteTran($"id in ({p_qianyue_info.id})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
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

                    var req = reqJson.GetPara();
                    if (!req["c_date"].ToNullableString().IsNullOrEmpty()) where += $" AND c_date = '{req["c_date"]}'";
                    if (!req["organize_id"].ToNullableString().IsNullOrEmpty()) where += $" AND qy_user_sn in (select user_sn from user_base where organize_id = {req["organize_id"]})";
                    if (!req["qy_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND qy_user_sn = '{req["qy_user_sn"]}'";

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_qianyue_info, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_qianyue_info
                {
                    public string c_date_text
                    {
                        get
                        {
                            return c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string org_name
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.sys_organize>($"id = {new DomainBasic.UserApp().GetInfoByUserSn(qy_user_sn).organize_id}", false).name;
                        }
                    }
                    public string qy_name
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(qy_user_sn).username;
                        }
                    }
                    public string qianyue_rate_text
                    {
                        get
                        {
                            return qianyue_rate.IsNullOrEmpty() ? "" : qianyue_rate + "%";
                        }
                    }
                }
                #endregion
            }
            #endregion
        }
    }
}
