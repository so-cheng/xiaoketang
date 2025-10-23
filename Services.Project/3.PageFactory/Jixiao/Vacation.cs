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
    /// 设置主播请假
    /// </summary>
    public partial class PageFactory
    {

        /// <summary>
        /// 设置主播请假提交页
        /// </summary>
        public class VacationPost
        {
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
                    text = "提交记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/zbmanage/report/vacationlist",
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

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "日期",
                    isRequired = true,
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    defaultValue = DateTime.Today.ToString("yyyy-MM-dd")
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTableEdit("l_vacation")
                {
                    title = "请假表",
                    colItems = new List<ModelBasic.EmtTableEdit.ColItem>
                    {
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                            title = "缺档原因",
                            field="cause",
                            emtModel=new ModelBasic.EmtTableSelect("cause")
                            {
                                options=new Dictionary<string, string>()
                                {
                                    { "请假","请假"},
                                    { "不接档","不接档"}
                                }
                            }
                        },
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                            title = "请假主播",
                            field="zb_user_sn",
                            emtModel=new ModelBasic.EmtTableSelect("zb_user_sn_op")
                            {
                                options=new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播,new UserIdentityBag().user_sn)
                            }
                        },
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                            title = "替档主播",
                            field="new_zb_user_sn",
                            emtModel=new ModelBasic.EmtTableSelect("new_zb_user_sn_op")
                            {
                                options=new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播,new UserIdentityBag().user_sn)
                            }
                        },
                        new ModelBasic.EmtTableEdit.ColItem
                        {
                            title = "备注",
                            field="note",
                            emtModel=new ModelBasic.EmtTableInput("note"){
                            }
                        }
                     },
                    displayStatus = EmtModelBase.DisplayStatus.编辑
                });
                
                return formDisplay;
            }

            public class user_base : ModelDb.user_base
            {
                public string zb_user_sn { get; set; }
                public string amount { get; set; }
                public string new_num { get; set; }
                public string amount_2 { get; set; }
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
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                foreach (var item in dtoReqData.l_vacation)
                {
                    lSql.Add(new ModelDb.p_jixiao_vacation
                    {
                        c_date= dtoReqData.c_date.ToDate(),
                        tenant_id =new DomainBasic.TenantApp().GetInfo().id,
                        tg_user_sn=new UserIdentityBag().user_sn,
                        zb_user_sn=item.zb_user_sn_op,
                        new_zb_user_sn= item.new_zb_user_sn_op,
                        note = item.note
                    }.InsertOrUpdateTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>


            public class DtoReqData:ModelDb.p_jixiao_vacation
            {
                public List<vacation> l_vacation { get; set; }
            }
            public class vacation
            {
                public string zb_user_sn_op { get; set; }
                public string new_zb_user_sn_op { get; set; }
                public string note { get; set; }
            }


            #endregion
        }
        /// <summary>
        /// 设置主播请假列表页
        /// </summary>
        public class VacationList
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

                listFilter.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOption(new UserIdentityBag().user_sn),
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

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    placeholder = "请假日期",
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
                result.data = new ServiceFactory.UserInfo.Tg().GetTreeOption(req["yy_user_sn"].ToNullableString()).ToJson();
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("cause")
                {
                    text = "缺档原因",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_user_sn_text")
                {
                    text = "请假主播",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_zb_user_sn_text")
                {
                    text = "替档主播",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "请假日期",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("note")
                {
                    text = "备注",
                    width = "300",
                    minWidth = "300"
                });
                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name="Del",
                    disabled=true,
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
                var p_jixiao_vacation = req.data_json.ToModel<ModelDb.p_jixiao_vacation>();
                p_jixiao_vacation.Delete();
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
                string where = $"1=1";
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

                if (!req["c_date"].ToNullableString().IsNullOrEmpty()) where += $" AND (c_date ='{req["c_date"]}'";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_vacation, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_vacation
            {
                public string zb_user_sn_text
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.zb_user_sn}'", false).name;
                    }
                }
                public string new_zb_user_sn_text
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.new_zb_user_sn}'", false).name;
                    }
                }
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
            }
            #endregion
        }

    }
}
