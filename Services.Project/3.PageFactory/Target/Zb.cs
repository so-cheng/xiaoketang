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
    /// 主播目标
    /// </summary>
    public partial class PageFactory
    {

        /// <summary>
        /// 设定主播目标
        /// </summary>
        public class ZbTargetPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*
                 * buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "月目标记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/ZbManage/target/List",
                    }
                });
                 */

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

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                {
                    title = "目标月份",
                    isRequired = true,
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    defaultValue = DateTime.Today.ToString("yyyy-MM-dd").ToDate().ToString("yyyy-MM"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                {"yearmonth","<%=page.yearmonth.value%>"}
                            },
                            func = GetUserList,
                            resCallJs = $"{new ModelBasic.EmtTableEdit.Js("l_target").set(@"JSON.parse(res.data)")}"
                        }
                    },
                    colLength = 6
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTableDataEdit("l_target")
                {
                    title = @"月度目标<br /><span style=""color:red;font-size:11px"">点击单元格编辑</span>",
                    width="1500px",
                    limit=20,
                    height="600px",
                    colItems = new List<ModelBasic.EmtTableDataEdit.ColItem>
                    {
                        new ModelBasic.EmtTableDataEdit.ColItem("name")
                        {
                         title = "所属主播",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount")
                        {
                         title = "目标音浪",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("new_num")
                        {
                         title = "目标拉新",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("contact_num")
                        {
                         title = "目标建联",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("num_2")
                        {
                         title = "目标二消",
                         edit = "text",
                        },
                     },
                    //左表:厅管名下的所有主播,右表:已设置的主播目标记录
                    defaultValue = DoMySql.FindObjectsBySql($"select user_base.id as id,name,user_sn as zb_user_sn,amount,contact_num,new_num,num_2 from user_base left join (select * from p_jixiao_target where yearmonth='{DateTime.Today.ToString("yyyy-MM-dd").ToDate().ToString("yyyy-MM")}') as t1 on  user_base.user_sn = t1.zb_user_sn where  user_base.tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播,new UserIdentityBag().user_sn)} and user_base.status='{ModelDb.user_base.status_enum.正常.ToInt()}'").ToJson(),
                displayStatus = EmtModelBase.DisplayStatus.编辑
                });

                #endregion
                return formDisplay;
            }

            public JsonResultAction GetUserList(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                string tg_user_sn = req["tg_user_sn"].ToNullableString();
                if (tg_user_sn.IsNullOrEmpty())
                {
                    tg_user_sn = new UserIdentityBag().user_sn;
                }

                //左表:厅管名下的所有主播,右表:已设置的主播目标记录
                var data = DoMySql.FindObjectsBySql($"select user_base.id as id,name,user_sn as zb_user_sn,amount,contact_num,new_num,num_2 from user_base left join (select * from p_jixiao_target where yearmonth='{req["yearmonth"].ToNullableString()}') as t1 on user_base.user_sn = t1.zb_user_sn where  user_base.tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn)} and user_base.status='{ModelDb.user_base.status_enum.正常.ToInt()}'");
                result.data = data.ToJson();
                return result;
            }

            public class user_base : ModelDb.user_base
            {
                public string zb_user_sn { get; set; }
                public string amount { get; set; }
                public string new_num { get; set; }
                public string amount_2 { get; set; }
                public string num_2 { get; set; }
            }

            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public FormData formData { get; set; } = new FormData();
                public class FormData
                {
                    public int id { get; set; }
                }
            }
            #region 异步请求处理
            /// <summary>
            /// 提交目标
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();

                if (dtoReqData.yearmonth.IsNullOrEmpty()) throw new WeicodeException("请选择目标月份！");
                foreach (var item in dtoReqData.l_target)
                {
                    /*if (!DoMySql.FindEntity<ModelDb.p_jixiao_target>($"zb_user_sn = '{item.zb_user_sn}' and  yearmonth = '{dtoReqData.yearmonth}'", false).IsNullOrEmpty()) throw new WeicodeException($"主播:'{item.name}'已经设定过{dtoReqData.yearmonth}月份目标");*/
                    if (item.amount < 1000) throw new WeicodeException("音浪必须大于1000！");
                    if (item.new_num < 0) throw new WeicodeException("拉新必须为数字！");
                    item.yearmonth = dtoReqData.yearmonth;
                    item.tg_user_sn = new UserIdentityBag().user_sn;
                    item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    lSql.Add(item.ToModel<ModelDb.p_jixiao_target>().InsertOrUpdateTran($"zb_user_sn='{item.zb_user_sn}' and yearmonth='{item.yearmonth}'"));

                    int daysInMonth = DateTime.DaysInMonth(item.yearmonth.ToDate().Year, item.yearmonth.ToDate().Month);
                    var avg_amount = Math.Round((item.amount / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_new = Math.Round((item.new_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_contact = Math.Round((item.contact_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_num2 = Math.Round((item.num_2.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();

                    var target_amount = new Dictionary<string, string>();
                    target_amount.Add("tenant_id", item.tenant_id.ToString());
                    target_amount.Add("zb_user_sn", item.zb_user_sn);
                    target_amount.Add("tg_user_sn", item.tg_user_sn);
                    target_amount.Add("yearmonth", item.yearmonth);

                    var target_new = new Dictionary<string, string>();
                    target_new.Add("tenant_id", item.tenant_id.ToString());
                    target_new.Add("zb_user_sn", item.zb_user_sn);
                    target_new.Add("tg_user_sn", item.tg_user_sn);
                    target_new.Add("yearmonth", item.yearmonth);

                    var target_contact = new Dictionary<string, string>();
                    target_contact.Add("tenant_id", item.tenant_id.ToString());
                    target_contact.Add("zb_user_sn", item.zb_user_sn);
                    target_contact.Add("tg_user_sn", item.tg_user_sn);
                    target_contact.Add("yearmonth", item.yearmonth);

                    var target_num2 = new Dictionary<string, string>();
                    target_num2.Add("tenant_id", item.tenant_id.ToString());
                    target_num2.Add("zb_user_sn", item.zb_user_sn);
                    target_num2.Add("tg_user_sn", item.tg_user_sn);
                    target_num2.Add("yearmonth", item.yearmonth);

                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        target_amount.Add($"amount_{i}", avg_amount.ToString());
                        target_new.Add($"new_{i}", avg_new.ToString());
                        target_contact.Add($"contact_{i}", avg_contact.ToString());
                        target_num2.Add($"num2_{i}", avg_num2.ToString());
                    }
                    lSql.Add(target_amount.ToModel<ModelDb.p_jixiao_target_day>().InsertOrUpdateTran($"zb_user_sn='{item.zb_user_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_new.ToModel<ModelDb.p_jixiao_target_new>().InsertOrUpdateTran($"zb_user_sn='{item.zb_user_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_contact.ToModel<ModelDb.p_jixiao_target_contact>().InsertOrUpdateTran($"zb_user_sn='{item.zb_user_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_num2.ToModel<ModelDb.p_jixiao_target_num2>().InsertOrUpdateTran($"zb_user_sn='{item.zb_user_sn}' and yearmonth='{item.yearmonth}'"));
                }
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                /// <summary>
                /// 目标数据集合
                /// </summary>
                public List<p_jixiao_target> l_target { get; set; }

                /// <summary>
                /// 目标月份
                /// </summary>
                public string yearmonth { get; set; }
            }


            public class p_jixiao_target : ModelDb.p_jixiao_target
            {
                /// <summary>
                /// 主播名字
                /// </summary>
                public string name { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 编辑单个主播月目标
        /// </summary>
        public class ZbTargetEdit
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "主播每月目标列表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/ZbManage/target/List",
                    }
                });*/
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
                var p_jixiao_target = DoMySql.FindEntity<ModelDb.p_jixiao_target>($"id='{req.id}'");
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    title = "id",
                    defaultValue = p_jixiao_target.id.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("yearmonth")
                {
                    title = "目标月份",
                    displayStatus = EmtModelBase.DisplayStatus.只读,
                    defaultValue = p_jixiao_target.yearmonth,
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount")
                {
                    title = "目标音浪",
                    isRequired = true,
                    defaultValue = p_jixiao_target.amount.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("new_num")
                {
                    title = "目标拉新",
                    isRequired = true,
                    defaultValue = p_jixiao_target.new_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("contact_num")
                {
                    title = "目标建联",
                    isRequired = true,
                    defaultValue = p_jixiao_target.contact_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("num_2")
                {
                    title = "目标二消",
                    isRequired = true,
                    defaultValue = p_jixiao_target.num_2.ToString(),
                });


                #endregion
                return formDisplay;
            }

            public class DtoReq
            {
                /// <summary>
                /// 主播目标记录id
                /// </summary>
                public int id { get; set; }
            }
            #region 异步请求处理
            /// <summary>
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var p_jixiao_target = DoMySql.FindEntity<ModelDb.p_jixiao_target>($"id='{req.GetPara()["id"].ToNullableString()}'", false);
                p_jixiao_target.amount = req.GetPara()["amount"].ToNullableString().ToDecimal();
                p_jixiao_target.new_num = req.GetPara()["new_num"].ToNullableString().ToInt();
                p_jixiao_target.contact_num = req.GetPara()["contact_num"].ToNullableString().ToInt();
                p_jixiao_target.num_2 = req.GetPara()["num_2"].ToNullableString().ToInt();


                if (p_jixiao_target.IsNullOrEmpty()) throw new WeicodeException("无效的月目标");
                if (p_jixiao_target.amount < 1000) throw new WeicodeException("音浪必须大于1000！");
                if (p_jixiao_target.new_num < 0) throw new WeicodeException("拉新必须为数字！");
                lSql.Add(p_jixiao_target.ToModel<ModelDb.p_jixiao_target>().UpdateTran());

                int daysInMonth = DateTime.DaysInMonth(p_jixiao_target.yearmonth.ToDate().Year, p_jixiao_target.yearmonth.ToDate().Month);
                var avg_amount = Math.Round((p_jixiao_target.amount / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_new = Math.Round((p_jixiao_target.new_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_contact = Math.Round((p_jixiao_target.contact_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_num2 = Math.Round((p_jixiao_target.num_2.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();

                var target_amount = new Dictionary<string, string>();
                target_amount.Add("tenant_id", p_jixiao_target.tenant_id.ToString());
                target_amount.Add("zb_user_sn", p_jixiao_target.zb_user_sn);
                target_amount.Add("tg_user_sn", p_jixiao_target.tg_user_sn);
                target_amount.Add("yearmonth", p_jixiao_target.yearmonth);

                var target_new = new Dictionary<string, string>();
                target_new.Add("tenant_id", p_jixiao_target.tenant_id.ToString());
                target_new.Add("zb_user_sn", p_jixiao_target.zb_user_sn);
                target_new.Add("tg_user_sn", p_jixiao_target.tg_user_sn);
                target_new.Add("yearmonth", p_jixiao_target.yearmonth);

                var target_contact = new Dictionary<string, string>();
                target_contact.Add("tenant_id", p_jixiao_target.tenant_id.ToString());
                target_contact.Add("zb_user_sn", p_jixiao_target.zb_user_sn);
                target_contact.Add("tg_user_sn", p_jixiao_target.tg_user_sn);
                target_contact.Add("yearmonth", p_jixiao_target.yearmonth);

                var target_num2 = new Dictionary<string, string>();
                target_num2.Add("tenant_id", p_jixiao_target.tenant_id.ToString());
                target_num2.Add("zb_user_sn", p_jixiao_target.zb_user_sn);
                target_num2.Add("tg_user_sn", p_jixiao_target.tg_user_sn);
                target_num2.Add("yearmonth", p_jixiao_target.yearmonth);

                for (int i = 1; i <= daysInMonth; i++)
                {
                    target_amount.Add($"amount_{i}", avg_amount.ToString());
                    target_new.Add($"new_{i}", avg_new.ToString());
                    target_contact.Add($"contact_{i}", avg_contact.ToString());
                    target_num2.Add($"num2_{i}", avg_num2.ToString());
                }
                lSql.Add(target_amount.ToModel<ModelDb.p_jixiao_target_day>().InsertOrUpdateTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(target_new.ToModel<ModelDb.p_jixiao_target_new>().InsertOrUpdateTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(target_contact.ToModel<ModelDb.p_jixiao_target_contact>().InsertOrUpdateTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(target_num2.ToModel<ModelDb.p_jixiao_target_num2>().InsertOrUpdateTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {

            }

            #endregion
        }

        /// <summary>
        ///主播目标
        /// </summary>
        public class ZbTargetList
        {

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

                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "运营账号",
                    disabled = true,
                    options = DoMySql.FindKvList<ModelDb.user_base>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_type_id='12'", "name,user_sn"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{new ModelBasic.EmtSelect.Js("zb_user_sn").clear()};"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = new ServiceFactory.RelationService().GetTreeOptionDic(new UserIdentityBag().user_sn),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")}"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn")
                {
                    placeholder = "主播账号",
                    options = new Dictionary<string, string>(),
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    placeholder = "选择月份",
                });
                return listFilter;
            }

            /// <summary>
            /// 获取厅管筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetZhubo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 获取厅管筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                result.data =new ServiceFactory.RelationService().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                return result;
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

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                var listDisplay = new ModelBasic.CtlListDisplay(req);
                listDisplay.operateWidth = "180";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                {
                    text = "所属运营",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "所属厅管",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "所属主播",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yearmonth")
                {
                    text = "目标月份",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "目标音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_complete")
                {
                    text = "已完成音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "目标拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_complete")
                {
                    text = "已完成拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2")
                {
                    text = "目标二消",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2_complete")
                {
                    text = "已完成二消",
                    width = "120",
                    minWidth = "120"
                });


                #region 操作列按钮
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        field_paras = "id",
                        url = "Edit"
                    },
                    text = "编辑",
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    text = "删除",
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 删除绩效目标
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var info = new JsonResultAction();
                var lSql = new List<string>();
                var p_jixiao_target = req.data_json.ToModel<ModelDb.p_jixiao_target>();
                p_jixiao_target = DoMySql.FindEntity<ModelDb.p_jixiao_target>($"id='{req.GetPara()["id"].ToNullableString()}'");
                lSql.Add(p_jixiao_target.DeleteTran());

                lSql.Add(new ModelDb.p_jixiao_target_day().DeleteTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_target_new().DeleteTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_target_contact().DeleteTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_target_num2().DeleteTran($"zb_user_sn='{p_jixiao_target.zb_user_sn}' and yearmonth='{p_jixiao_target.yearmonth}'"));

                MysqlHelper.ExecuteSqlTran(lSql);

                return info;
            }


            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
            }

            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = "1=1";
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                }
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                }
                if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and zb_user_sn = '{req["zb_user_sn"].ToNullableString()}'";
                }

                if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += $" AND (create_time >='{req["create_time"]}' and create_time <'{req["create_time"].ToDate().AddMonths(1)}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_target, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string create_time { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_target
            {
                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).username;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                    }
                }
                public string yy_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, this.tg_user_sn)}'", false).name;
                    }
                }
                public string submit_time
                {
                    get
                    {
                        return this.create_time.ToDate().ToString("yyyy-MM");
                    }
                }

                public string amount_complete
                {
                    get
                    {
                        int? count = 0;
                        foreach(var item in DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{this.zb_user_sn}' and c_date >='{this.yearmonth.ToDate().ToString("yyyy-MM-01")}' and c_date<'{this.yearmonth.ToDate().AddMonths(1).ToString("yyyy-MM-01")}'"))
                        {
                            count += item.amount;
                        }
                        return count.ToString();
                    }
                }
                public string new_num_complete
                {
                    get
                    {
                        int? count = 0;
                        foreach (var item in DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{this.zb_user_sn}' and c_date >='{this.yearmonth.ToDate().ToString("yyyy-MM-01")}' and c_date<'{this.yearmonth.ToDate().AddMonths(1).ToString("yyyy-MM-01")}'"))
                        {
                            count += item.new_num;
                        }
                        return count.ToString();
                    }
                }
                public string num_2_complete
                {
                    get
                    {
                        int? count = 0;
                        foreach (var item in DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{this.zb_user_sn}' and c_date >='{this.yearmonth.ToDate().ToString("yyyy-MM-01")}' and c_date<'{this.yearmonth.ToDate().AddMonths(1).ToString("yyyy-MM-01")}'"))
                        {
                            count += item.num_2;
                        }
                        return count.ToString();
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 设定主播日目标
        /// </summary>
        public class ZbStandardPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                
                 buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "主播每日目标列表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/zbmanage/target/standardlist",
                    }
                });

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
                var p_jixiao_standard = DoMySql.FindEntityById<ModelDb.p_jixiao_standard>(req.id);
                
                var date = p_jixiao_standard.s_date + " ~ " + p_jixiao_standard.e_date;
                if (p_jixiao_standard.IsNullOrEmpty())
                {
                    date = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
                }
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                {
                    title = "日期范围",
                    isRequired = true,
                    defaultValue = date,
                    
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range
                });



                formDisplay.formItems.Add(new ModelBasic.EmtTableDataEdit("l_target")
                {
                    title = "每日目标",
                    
                    colItems = new List<ModelBasic.EmtTableDataEdit.ColItem>
                    {
                        new ModelBasic.EmtTableDataEdit.ColItem("name")
                        {
                         title = "所属主播",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("new_num")
                        {
                         title = "音浪指标",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount")
                        {
                         title = "拉新数指标",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount_2")
                        {
                         title = "二消数指标",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("contact_num")
                        {
                         title = "建联数指标",
                         edit = "text",
                        },
                     },
                    defaultValue = DoMySql.FindObjects<ModelDb.user_base>(new DoMySql.Filter
                    {
                        fields = "name,user_sn as zb_user_sn,0 as new_num,0 as amount,0 as amount_2,0 as contact_num",
                        where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} and status='{ModelDb.user_base.status_enum.正常.ToInt()}'",
                    }).ToJson(),
                    displayStatus = EmtModelBase.DisplayStatus.编辑
                });

                #endregion

                return formDisplay;

            }

            public JsonResultAction GetUserList(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var data = DoMySql.FindObjects<ModelDb.user_base>(new DoMySql.Filter
                {
                    fields = "name,user_sn as zb_user_sn,0 as new_num,0 as amount,0 as amount_2,0 as contact_num",
                    where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} and status='{ModelDb.user_base.status_enum.正常.ToInt()}'",
                });
                result.data = data.ToJson();
                return result;
            }




            public class DtoReq
            {
                public int id { get; set; }
            }
            #region 异步请求处理
            /// <summary>
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                List<string> lSql = new List<string>();
                var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(dtoReqData.dateRange, 0);
                var p_jixiao_standard = req.data_json.ToModel<ModelDb.p_jixiao_standard>();
                p_jixiao_standard.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                p_jixiao_standard.st_sn = UtilityStatic.CommonHelper.CreateOrderNo();
                p_jixiao_standard.s_date = dateRange.date_range_s.ToDate();
                p_jixiao_standard.e_date = dateRange.date_range_e.ToDate();
                p_jixiao_standard.tg_user_sn = new UserIdentityBag().user_sn;

                if(!DoMySql.FindEntity<ModelDb.p_jixiao_standard>($"tg_user_sn='{p_jixiao_standard.tg_user_sn}' and (s_date<='{p_jixiao_standard.s_date}' and e_date>='{p_jixiao_standard.s_date}' or s_date<='{p_jixiao_standard.e_date}' and e_date>='{p_jixiao_standard.e_date}' or s_date>='{p_jixiao_standard.s_date}' and e_date<='{p_jixiao_standard.e_date}')", false).IsNullOrEmpty())
                {
                    throw new WeicodeException("当前所选时间段已存在安排表");
                }


                lSql.Add(p_jixiao_standard.InsertTran());
                foreach (var item in dtoReqData.l_target)
                {
                    item.tenant_id = p_jixiao_standard.tenant_id;
                    item.st_sn = p_jixiao_standard.st_sn;
                    lSql.Add(item.ToModel<ModelDb.p_jixiao_standard_item>().InsertTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);


                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                /// <summary>
                /// 目标数据集合
                /// </summary>
                public List<p_jixiao_standard_item> l_target { get; set; }


                /// <summary>
                /// 目标日期范围
                /// </summary>
                public string dateRange { get; set; }
            }

            public class p_jixiao_standard_item : ModelDb.p_jixiao_standard_item
            {
                /// <summary>
                /// 主播名字
                /// </summary>
                public string name { get; set; }
            }


            #endregion
        }

        public class ZbStandardList
        {

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


                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("create_time")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    placeholder = "选择月份",
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
                listDisplay.operateWidth = "180";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dateRange")
                {
                    text = "时间范围",
                    width = "210",
                    minWidth = "210"
                });

                #region 操作列按钮

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer=new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                         field_paras="st_sn",
                         url= "StandardListItem"
                    },
                    text = "明细",

                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    text = "删除",

                });
                #endregion

                return listDisplay;
            }

            /// <summary>
            /// 删除绩效目标
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var info = new JsonResultAction();
                var p_jixiao_standard = DoMySql.FindEntityById<ModelDb.p_jixiao_standard>(req.GetPara("id").ToInt());
                var lSql = new List<string>();
                lSql.Add(p_jixiao_standard.DeleteTran());
                foreach (var item in DoMySql.FindList<ModelDb.p_jixiao_standard_item>($"st_sn='{p_jixiao_standard.st_sn}'"))
                {
                    lSql.Add(item.DeleteTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                return info;
            }


            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
            }

            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = $"tg_user_sn='{new UserIdentityBag().user_sn}'";
                if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += $" AND (create_time >='{req["create_time"]}' and create_time <'{req["create_time"].ToDate().AddMonths(1)}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_standard, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string create_time { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_standard
            {
                public string dateRange
                {
                    get
                    {
                        var p_jixiao_standard = DoMySql.FindEntity<ModelDb.p_jixiao_standard>($"st_sn='{this.st_sn}'");
                        return p_jixiao_standard.s_date.ToDate().ToString("yyyy-MM-dd") + " ~ " + p_jixiao_standard.e_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
            }
            #endregion
        }

        public class ZbStandardListItem
        {

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

                listFilter.formItems.Add(new ModelBasic.EmtHidden("st_sn")
                {
                    defaultValue=req.st_sn
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
                listDisplay.operateWidth = "180";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dateRange")
                {
                    text = "时间范围",
                    width = "210",
                    minWidth = "210"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "所属主播",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "目标拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "目标音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2")
                {
                    text = "目标二消数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_num")
                {
                    text = "目标建联数",
                    width = "120",
                    minWidth = "120"
                });
                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    text = "删除",
                    disabled=true
                });
                #endregion

                return listDisplay;
            }

            /// <summary>
            /// 删除绩效目标
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var info = new JsonResultAction();
                var p_jixiao_target = req.data_json.ToModel<ModelDb.p_jixiao_target>();
                p_jixiao_target.Delete();
                return info;
            }


            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                public string st_sn { get; set; }
            }

            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = $"zb_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} and st_sn='{req["st_sn"].ToNullableString()}'";
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}";
                }
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                }
                if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and zb_user_sn = '{req["zb_user_sn"].ToNullableString()}'";
                }

                if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += $" AND (create_time >='{req["create_time"]}' and create_time <'{req["create_time"].ToDate().AddMonths(1)}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_standard_item, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string create_time { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_standard_item
            {
                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                    }
                }
                public string dateRange
                {
                    get
                    {
                        var p_jixiao_standard = DoMySql.FindEntity<ModelDb.p_jixiao_standard>($"st_sn='{this.st_sn}'");
                        return p_jixiao_standard.s_date.ToDate().ToString("yyyy-MM-dd") + " ~ " + p_jixiao_standard.e_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
            }
            #endregion
        }

        public class ZbStandardPostItem
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");

                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "主播每日目标列表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/zbmanage/target/standardlist",
                    }
                });

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
                var p_jixiao_standard = DoMySql.FindEntityById<ModelDb.p_jixiao_standard>(req.id);

                var date = p_jixiao_standard.s_date + " ~ " + p_jixiao_standard.e_date;
                if (p_jixiao_standard.IsNullOrEmpty())
                {
                    date = DateTime.Today.ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
                }
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                {
                    title = "日期范围",
                    isRequired = true,
                    defaultValue = date,
                    displayStatus= EmtModelBase.DisplayStatus.只读,
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range
                });



                formDisplay.formItems.Add(new ModelBasic.EmtTableDataEdit("l_target")
                {
                    title = "主播每日目标",

                    colItems = new List<ModelBasic.EmtTableDataEdit.ColItem>
                    {
                        new ModelBasic.EmtTableDataEdit.ColItem("name")
                        {
                         title = "所属主播",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount")
                        {
                         title = "拉新数指标",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("new_num")
                        {
                         title = "音浪指标",
                         edit = "text",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount_2_rate")
                        {
                         title = "二消率指标(%)",
                         edit = "text",
                        },
                     },
                    defaultValue = DoMySql.FindObjects<ModelDb.user_base>(new DoMySql.Filter
                    {
                        fields = "name,user_sn as zb_user_sn,0 as amount,0 as new_num,0 as amount_2_rate",
                        where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} and status='{ModelDb.user_base.status_enum.正常.ToInt()}'",
                    }).ToJson(),
                    displayStatus = EmtModelBase.DisplayStatus.编辑
                });
                var defaultValue = DoMySql.FindList<ModelDb.user_base,ModelDb.user_base>();
                #endregion
                return formDisplay;
            }

            

            public JsonResultAction GetUserList(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var data = DoMySql.FindObjects<ModelDb.user_base>(new DoMySql.Filter
                {
                    fields = "name,user_sn as zb_user_sn,0 as amount,0 as new_num,0 as amount_2_rate",
                    where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn IN {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn)} and status='{ModelDb.user_base.status_enum.正常.ToInt()}'",
                });
                result.data = data.ToJson();
                return result;
            }




            public class DtoReq
            {
                public int id { get; set; }
            }
            #region 异步请求处理
            /// <summary>
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                List<string> lSql = new List<string>();
                var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(dtoReqData.dateRange, 0);
                var p_jixiao_standard = req.data_json.ToModel<ModelDb.p_jixiao_standard>();
                p_jixiao_standard.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                p_jixiao_standard.st_sn = UtilityStatic.CommonHelper.CreateOrderNo();
                p_jixiao_standard.s_date = dateRange.date_range_s.ToDate();
                p_jixiao_standard.e_date = dateRange.date_range_e.ToDate();
                p_jixiao_standard.tg_user_sn = new UserIdentityBag().user_sn;

                if (!DoMySql.FindEntity<ModelDb.p_jixiao_standard>($"tg_user_sn='{p_jixiao_standard.tg_user_sn}' and (s_date<='{p_jixiao_standard.s_date}' and e_date>='{p_jixiao_standard.s_date}' or s_date<='{p_jixiao_standard.e_date}' and e_date>='{p_jixiao_standard.e_date}' or s_date>='{p_jixiao_standard.s_date}' and e_date<='{p_jixiao_standard.e_date}')", false).IsNullOrEmpty())
                {
                    throw new WeicodeException("当前所选时间段已存在安排表");
                }


                lSql.Add(p_jixiao_standard.InsertTran());
                foreach (var item in dtoReqData.l_target)
                {
                    item.tenant_id = p_jixiao_standard.tenant_id;
                    item.st_sn = p_jixiao_standard.st_sn;
                    lSql.Add(item.ToModel<ModelDb.p_jixiao_standard_item>().InsertTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);


                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                /// <summary>
                /// 目标数据集合
                /// </summary>
                public List<p_jixiao_standard_item> l_target { get; set; }


                /// <summary>
                /// 目标日期范围
                /// </summary>
                public string dateRange { get; set; }
            }

            public class p_jixiao_standard_item : ModelDb.p_jixiao_standard_item
            {
                /// <summary>
                /// 主播名字
                /// </summary>
                public string name { get; set; }
            }


            #endregion
        }

        /// <summary>
        /// 主播目标进度
        /// </summary>
        public class ScheduleZbList
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
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    defaultValue = DateTime.Today.ToString("yyyy-MM"),
                    placeholder = "选择月份",
                });
                listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                {
                    disabled = true,
                    options = new ServiceFactory.RelationService().GetTreeOption(new UserIdentityBag().user_sn),
                    title = "所属厅管",
                    placeholder = "选择所属厅管",
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
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;
                listDisplay.operateWidth = "120px";
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "所属厅管",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "所属主播",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yearmonth")
                {
                    text = "目标月份",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "目标音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "目标拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_finish")
                {
                    text = "已完成音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_finish")
                {
                    text = "已完成拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_progress")
                {
                    mode = ModelBasic.EmtModel.ListItem.Mode.进度条,
                    text = "目标音浪进度",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_progress")
                {
                    mode = ModelBasic.EmtModel.ListItem.Mode.进度条,
                    text = "目标拉新进度",
                    width = "120",
                    minWidth = "120"
                });
                #region 操作列
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                Enum e { get; set; }
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
                string where = $"1=1";
                if (!req["yearmonth"].ToNullableString().IsNullOrEmpty()) where += $" AND (yearmonth ='{req["yearmonth"]}')";
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (tg_user_sn ='{req["tg_user_sn"]}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_target, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string yearmonth { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_target
            {


                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.tg_user_sn}'", false).name;
                    }
                }
                public string amount_progress
                {
                    get
                    {
                        var p_jixiao_day = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn ='{this.zb_user_sn}' and c_date > '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        decimal? sum = 0;
                        foreach (var item in p_jixiao_day)
                        {
                            sum += item.amount;
                        }
                        var result = this.amount == 0 ? "0" : ((sum / this.amount).ToDouble() * 100).ToFixed(2) + "%";
                        return result;
                    }
                }
                public string new_num_progress
                {
                    get
                    {
                        var p_jixiao_day = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn ='{this.zb_user_sn}' and c_date > '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        decimal? sum = 0;
                        foreach (var item in p_jixiao_day)
                        {
                            sum += item.new_num;
                        }

                        return this.new_num == 0 ? "0" : ((sum / this.new_num).ToDouble().ToFixed(2) * 100) + "%";
                    }
                }
                public string new_num_finish
                {
                    get
                    {
                        var p_jixiao_day = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn ='{this.zb_user_sn}' and c_date > '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        decimal? sum = 0;
                        foreach (var item in p_jixiao_day)
                        {
                            sum += item.new_num;
                        }

                        return sum.ToString();
                    }
                }
                public string amount_finish
                {
                    get
                    {
                        var p_jixiao_day = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn ='{this.zb_user_sn}' and c_date > '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        decimal? sum = 0;
                        foreach (var item in p_jixiao_day)
                        {
                            sum += item.amount;
                        }

                        return sum.ToString();
                    }
                }
            }

            #endregion
        }
    }
}
