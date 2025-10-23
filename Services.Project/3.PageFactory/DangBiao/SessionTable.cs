using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class DangBiao
        {
            public class TableList
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public PageList Get(DtoReq req)
                {
                    var pageModel = new PageList("pagelist");

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

                    var option = DomainBasicStatic.DoMySql.FindKvList<ModelDb.user_base>($" user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}'", "username,user_sn");
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        options = option,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                                {
                                    { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                                },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("ting_sn").options(@"JSON.parse(res.data)")};"
                        }
                    },
                    disabled = true,
                });
                listFilter.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                {
                    placeholder = "所属厅",
                    options =new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn) ,
                    disabled = true,
                });
                return listFilter;
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
                result.data = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                return result;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("button");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("addNewTable")
                {
                    text = "新增空白档表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                    eventToUrl = new EmtModel.ButtonItem.EventToUrl
                    {
                        url = $"table",
                        target = "_bank"
                    },
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("addTemplateTable")
                {
                    text = "新增模板档表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                    eventToUrl = new EmtModel.ButtonItem.EventToUrl
                    {
                        url = $"AddTableByTemplate",
                        target = "_bank"
                    },
                });
                return buttonGroup;
            }
            
            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                
                var listDisplay = new CtlListDisplay();
                listDisplay.operateWidth = "180";
                listDisplay.isOpenNumbers = true;
                #region 显示列
                listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData,
                };
                listDisplay.listItems.Add(new EmtModel.ListItem("tg_username")
                {
                    text = "厅管账号",
                    width = "200",
                    minWidth = "100",
                    disabled = true,
                });


                listDisplay.listItems.Add(new EmtModel.ListItem("ting_name")
                {
                    text = "厅名称",
                    width = "200",
                    minWidth = "100",
                });

                listDisplay.listItems.Add(new EmtModel.ListItem("dateRange")
                {
                    text = "日期范围",
                    width = "200",
                    minWidth = "100",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("note")
                {
                    text = "档表备注",
                    width = "200",
                    minWidth = "100",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("create_time")
                {
                    text = "创建时间",
                    width = "180",
                    minWidth = "100",
                });
                #endregion
                #region 操作列
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                    eventToUrl = new EmtModel.ListOperateItem.EventToUrl
                    {
                        url = $"ShowTableById?op_type=0",
                        field_paras = "id",
                        target = "_bank"
                    },
                    text = "查看",
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                    eventToUrl = new EmtModel.ListOperateItem.EventToUrl
                    {
                        url = $"UpdateTableById?op_type=1",
                        field_paras = "id",
                        target = "_bank"
                    },
                    text = "编辑"
                });
                #endregion
                return listDisplay;
            }
            public class DtoReq
            {
                /// <summary>
                /// 关系类型
                /// </summary>
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
            }
            #endregion
                #region ListData
                /// <summary>
                /// 获取当前登录厅管的档表记录
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
                {
                    //1.校验
                    string where = $"1=1";
                    if (!reqJson.GetPara("yy_user_sn").ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, reqJson.GetPara("yy_user_sn").ToNullableString())}";
                    }
                    if (!reqJson.GetPara("ting_sn").ToNullableString().IsNullOrEmpty())
                    {
                        where += $" and ting_sn = '{reqJson.GetPara("ting_sn")}'";
                    }
                    //2.获取档表记录
                    var filter = new DoMySql.Filter
                    {
                        where = where + $" order by create_time desc",
                    };
                    return new CtlListDisplay.ListData().getList<ModelDb.p_jixiao_dangbiao, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_jixiao_dangbiao
                {
                    public string dateRange
                    {
                        get
                        {
                            return ((DateTime)this.s_date).ToString("yyyy/MM/dd") + " ~ " + ((DateTime)this.e_date).ToString("yyyy/MM/dd");
                        }
                    }
                    public string tg_username
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            if (ting_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                        }
                    }
                }
                #endregion
            }

            #region 档表
            /// <summary>
            /// 接收档表ReqDto
            /// </summary>
            public class TableReqDto
            {
                /// <summary>
                /// 档表id
                /// </summary>
                public int table_id { get; set; }
                /// <summary>
                /// 所属厅
                /// </summary>
                public string ting_sn { get; set; }
                /// <summary>
                /// 日期范围
                /// </summary>
                public string c_date { get; set; }

                /// <summary>
                /// 挂位的档数
                /// </summary>
                public string gw_dangs { get; set; }

                /// <summary>
                /// 档位角色
                /// </summary>
                public string dang_wei_role { get; set; }

                /// <summary>
                /// 备注
                /// </summary>
                public string note { get; set; }
                /// <summary>
                /// 主播sn 0-1点(后面的属性依此类推)
                /// </summary>
                public string[] zb_0 { get; set; }
                public string[] zb_1 { get; set; }
                public string[] zb_2 { get; set; }
                public string[] zb_3 { get; set; }
                public string[] zb_4 { get; set; }
                public string[] zb_5 { get; set; }
                public string[] zb_6 { get; set; }
                public string[] zb_7 { get; set; }
                public string[] zb_8 { get; set; }
                public string[] zb_9 { get; set; }
                public string[] zb_10 { get; set; }
                public string[] zb_11 { get; set; }
                public string[] zb_12 { get; set; }
                public string[] zb_13 { get; set; }
                public string[] zb_14 { get; set; }
                public string[] zb_15 { get; set; }
                public string[] zb_16 { get; set; }
                public string[] zb_17 { get; set; }
                public string[] zb_18 { get; set; }
                public string[] zb_19 { get; set; }
                public string[] zb_20 { get; set; }
                public string[] zb_21 { get; set; }
                public string[] zb_22 { get; set; }
                public string[] zb_23 { get; set; }

                /// <summary>
                /// 备注信息
                /// </summary>
                public string[] memo_item { get; set; }

                /// <summary>
                /// 第一档备注
                /// </summary>
                public string memo_1 { get; set; }
                /// <summary>
                /// 第二档备注
                /// </summary>
                public string memo_2 { get; set; }
                /// <summary>
                /// 第三档备注
                /// </summary>
                public string memo_3 { get; set; }
                /// <summary>
                /// 第四档备注
                /// </summary>
                public string memo_4 { get; set; }
                /// <summary>
                /// 第五档备注
                /// </summary>
                public string memo_5 { get; set; }
                /// <summary>
                /// 第六档备注
                /// </summary>
                public string memo_6 { get; set; }
                /// <summary>
                /// 第七档备注
                /// </summary>
                public string memo_7 { get; set; }
                /// <summary>
                /// 第八档备注
                /// </summary>
                public string memo_8 { get; set; }

                /// <summary>
                /// 主播颜色
                /// </summary>
                public string zbys { get; set; }

            }

            /// <summary>
            /// 档表回显VO
            /// </summary>
            public class TableVO : TableReqDto
            {
                /// <summary>
                /// 操作类型:修改/查看
                /// </summary>
                public int op_type { get; set; }
                /// <summary>
                /// 主播时长明细
                /// </summary>
                public List<ZBItem> items { get; set; }

                /// <summary>
                /// 所属厅管user_sn
                /// </summary>
                public string tg_user_sn { get; set; }

                public TableVO()
                {
                    op_type = 0;
                    table_id = 0;
                    note = "";
                    dang_wei_role = "";
                    items = new List<ZBItem>();
                }
            }
            public class ZBItem : ModelDbBasic.user_base
            {
                public int zfp_hours { get; set; }
                public int gw_hours { get; set; }
                public string color { get; set; }
                /// <summary>
                /// 颜色快照
                /// </summary>
                public string db_color { get; set; }
            }

            public class TableColour : ModelDbBasic.user_base
            {
                public string color { get; set; }
            }
            #endregion

            /// <summary>
            /// 主播颜色
            /// </summary>
            public class ZbsrColor
            {
                public string user_sn { get; set; }
                public string color { get; set; }
            }
        }
    }
}
