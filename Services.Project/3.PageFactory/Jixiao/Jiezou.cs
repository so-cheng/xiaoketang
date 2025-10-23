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
    /// <summary>
    /// 绩效目标
    /// </summary>
    public partial class PageFactory
    {

        #region 

        public class JiezouList
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "Post",
                    },
                    disabled=true
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
                listDisplay.operateWidth = "220";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                {
                    text = "创建时间",
                    width = "160",
                    minWidth = "160"
                });
                
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                {
                    text = "轮次",
                    width = "220",
                    minWidth = "220"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy")
                {
                    text = "运营",
                    width = "120",
                    minWidth = "120"
                });



                #region 操作列按钮
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.跳转URL,
                    eventToUrl = new ModelBasic.EmtModel.ListOperateItem.EventToUrl
                    {
                        field_paras = "jiezou_sn",
                        target = "_bank",
                        url = "Item"
                    },
                    text = "详情",
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        field_paras = "id",
                        url = "Post"
                    },
                    text = "编辑"
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
                var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"id='{req.GetPara()["id"].ToNullableString()}'");
                lSql.Add(jiezou.DeleteTran($"id='{jiezou.id}'"));
                lSql.Add(new ModelDb.jiezou_item().DeleteTran($"jiezou_sn='{jiezou.jiezou_sn}'"));
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
                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                if (!req["create_time"].ToNullableString().IsNullOrEmpty()) where += $" AND (create_time >='{req["create_time"]}' and create_time <'{req["create_time"].ToDate().AddMonths(1)}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by create_time desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.jiezou, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.jiezou
            {

                public string create_time_text
                {
                    get
                    {
                        return this.create_time.ToDateTime().ToString("yyyy-MM-dd");
                    }
                }
                public string yy
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{this.yy_user_sn}'").username;
                    }
                }
            }
            #endregion
        }




        /// <summary>
        /// 
        /// </summary>
        public class JiezouPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("PagePost");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                if (req.id<=0)
                {
                    pageModel.postedReturn = new PagePost.PostedReturn
                    {
                        returnType = PagePost.PostedReturn.ReturnType.父窗口跳转URL,
                        returnUrl = "/jixiao/jiezou/Item"
                    };
                }
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
                    text = "创建记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/jixiao/jiezou/List",
                    },
                    disabled = true
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
                var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"id='{req.id}'", false);
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = jiezou.id.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("jiezou_sn")
                {
                    defaultValue = jiezou.jiezou_sn
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("term")
                {
                    title = "轮次",
                    defaultValue = jiezou.term.IsNullOrEmpty() ? $"新建轮次 {DateTime.Now.ToString("MM月dd日HH:mm:ss")}":jiezou.term,
                    colLength = 8
                });

                string defaultValue = "";
                if (!jiezou.jiezou_sn.IsNullOrEmpty())
                {
                    foreach (var item in DoMySql.FindList<ModelDb.jiezou_item>($"jiezou_sn='{jiezou.jiezou_sn}' and status=0"))
                    {
                        defaultValue += item.tg_user_sn + ",";
                    }
                }
                else
                {
                    foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn))
                    {
                        defaultValue += item.user_sn + ",";
                    }
                }
                if (defaultValue.Length > 0) 
                { 
                    defaultValue = defaultValue.Substring(0, defaultValue.Length - 1); 
                }
                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("tg_user_sns")
                {
                    bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn),
                    title = "厅管",
                    defaultValue = defaultValue,
                });

                #endregion
                return formDisplay;
            }


            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
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
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var e = DoMySql.FindEntity<ModelDb.jiezou>($"term='{dtoReqData.term}' and jiezou_sn != '{dtoReqData.jiezou_sn}' and yy_user_sn='{new UserIdentityBag().user_sn}'",false);
                if (!e.IsNullOrEmpty())
                {
                    throw new WeicodeException(@"轮次禁止重名");
                }
                
                if (dtoReqData.id == 0)
                {//如果是新增操作
                    string jiezou_sn = UtilityStatic.CommonHelper.CreateUniqueSn();

                    //1.找到该运营的当前节奏表，更新为无效状态
                    foreach (var item in DoMySql.FindList<ModelDb.jiezou>($"status=0 and yy_user_sn='{new UserIdentityBag().user_sn}'"))
                    {
                        item.status = 1;
                        lSql.Add(item.UpdateTran());
                    }

                    //2.新增一个节奏表
                    lSql.Add(new ModelDb.jiezou
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        yy_user_sn = new UserIdentityBag().user_sn,
                        term = dtoReqData.term,
                        jiezou_sn = jiezou_sn
                    }.InsertTran());
                    foreach (var item in req.GetPara()["tg_user_sns"].ToString().Split(','))
                    {
                        lSql.Add(new ModelDb.jiezou_item
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            yy_user_sn = new UserIdentityBag().user_sn,
                            tg_user_sn = item,
                            jiezou_sn = jiezou_sn
                        }.InsertTran());
                    }
                }
                else
                {//如果是编辑操作
                    //修改节奏表的轮次，厅管
                    lSql.Add(new ModelDb.jiezou
                    {
                        term = dtoReqData.term,
                    }.UpdateTran($"id='{dtoReqData.id}'"));
                    var l_tg = req.GetPara()["tg_user_sns"].ToString().Split(',');
                    lSql.AddRange(UpdateJiezouSql(l_tg,DoMySql.FindList<ModelDb.jiezou_item>($"jiezou_sn='{dtoReqData.jiezou_sn}' and status=0"), dtoReqData.jiezou_sn));
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                var result = new JsonResultAction();
                return result;
            }

            //获取更新节奏表的sql语句
            private static List<string> UpdateJiezouSql(string[] l_tg,List<ModelDb.jiezou_item> jiezou_item,string jiezou_sn)
            {
                List<string> list = new List<string>();
                int match = 0;

                //比对当前参与的厅管和用户编辑之后的厅管，如果用户选择了新的厅管，新增或修改这个厅管的记录
                foreach (var tg in l_tg)
                {
                    match = 0;
                    if (tg.IsNullOrEmpty()) { continue; }
                    foreach (var item in jiezou_item)
                    {
                        if (item.tg_user_sn == tg)
                        {
                            match++;
                        }
                    }
                    if (match == 0)
                    {
                        list.Add(new ModelDb.jiezou_item() 
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            yy_user_sn = new UserIdentityBag().user_sn,
                            tg_user_sn = tg,
                            jiezou_sn = jiezou_sn,
                            status=0
                        }.InsertOrUpdateTran($"jiezou_sn='{jiezou_sn}' and tg_user_sn='{tg}'"));
                    }
                }

                //比对当前参与的厅管和用户编辑之后的厅管，如果用户取消选择了厅管，这个厅管的记录变成无效
                foreach (var item in jiezou_item)
                {
                    match = 0;
                    foreach(var tg in l_tg)
                    {
                        if (item.tg_user_sn == tg)
                        {
                            match++;
                        }
                    }
                    if (match == 0)
                    {
                        list.Add(new ModelDb.jiezou_item()
                        {
                            status=1
                        }.UpdateTran($"id='{item.id}'"));
                    }
                }

                return list;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.jiezou
            {
            }

            #endregion
        }

        #endregion


        #region 

        public class JiezouQList
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

                listFilter.formItems.Add(new ModelBasic.EmtSelect("step")
                {
                    options=new Dictionary<string, string>
                    {
                        {"0.5阶段","1"},
                        {"1阶段","2"},
                        {"2阶段","3"},
                        {"3阶段","4"},
                        {"4阶段","5"},
                        {"其它","6"},
                    },
                    placeholder = "选择阶段",
                });

                listFilter.formItems.Add(new ModelBasic.EmtHidden("jiezou_sn")
                {
                    defaultValue=req.jiezou_sn
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = $"QAdd?jiezou_sn={req.jiezou_sn}",
                    }
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "详情",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面跳转按钮,
                    eventToUrl=new EmtModel.ButtonItem.EventToUrl
                    {
                        url= "QNSIndex",
                        target="_bank"
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
                listDisplay.operateWidth = "220";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                {
                    text = "创建时间",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("step_text")
                {
                    text = "阶段",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("question")
                {
                    text = "问题",
                    width = "400",
                    minWidth = "400"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sort")
                {
                    text = "排序",
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
                        url = "QPost"
                    },
                    text = "编辑",
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        field_paras = "qns_sn",
                        url = "SPost"
                    },
                    text = "解决方案",
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
                var qns_questions = DoMySql.FindEntity<ModelDb.qns_questions>($"id='{req.GetPara()["id"].ToNullableString()}'");
                lSql.Add(qns_questions.DeleteTran($"id='{qns_questions.id}'"));
                lSql.Add(new ModelDb.qns_solutions().DeleteTran($"qns_sn='{qns_questions.qns_sn}'"));
                MysqlHelper.ExecuteSqlTran(lSql);
                return info;
            }


            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                /// <summary>
                /// 所属节奏表(轮次)
                /// </summary>
                public string jiezou_sn { get; set; }
                /// <summary>
                /// 所属阶段
                /// </summary>
                public int step { get; set; }
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
                if (!req["step"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and step='{req["step"].ToNullableString()}'";
                }
                if (!req["jiezou_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and jiezou_sn='{req["jiezou_sn"].ToNullableString()}'";
                }

                var filter = new DoMySql.Filter
                {
                    where = where + " order by step,sort"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.qns_questions, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.qns_questions
            {
                public string create_time_text
                {
                    get
                    {
                        return this.create_time.ToDate().ToString("yyyy-MM-dd");
                    }
                }

                public string step_text
                {
                    get
                    {
                        if (this.step == 1) { return "0.5阶段"; }
                        if (this.step == 2) { return "1.0阶段"; }
                        if (this.step == 3) { return "2.0阶段"; }
                        if (this.step == 4) { return "3.0阶段"; }
                        if (this.step == 5) { return "4.0阶段"; }
                        return "";
                    }
                }
            }
            #endregion
        }


        public class JiezouQAdd
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
                var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"id='{req.id}'", false);
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = jiezou.id.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("step")
                {
                    title = "阶段",
                    options = new Dictionary<string, string>
                    {
                        {"0.5阶段","1" },
                        {"1阶段","2" },
                        {"2阶段","3" },
                        {"3阶段","4" },
                        {"4阶段","5" },
                        {"其他","6" },
                    },
                    defaultValue=req.step.ToString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_questions")
                {
                    title = "梳理",
                    colItems = new List<EmtDataSelect.ColItem>
                    {
                        new EmtDataSelect.ColItem("qns_sn")
                        {
                            isHide=true,
                            edit="text"
                        },
                        new EmtDataSelect.ColItem("question")
                        {
                            title="问题",
                            edit="text",
                            width="600"
                        }
                    }
                });



                #endregion
                return formDisplay;
            }


            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public int id { get; set; }
                /// <summary>
                /// 所属阶段
                /// </summary>
                public int step { get; set; }
                /// <summary>
                /// 所属节奏表(轮次)
                /// </summary>
                public string jiezou_sn { get; set; }
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
                foreach (var item in dtoReqData.l_questions)
                {
                    lSql.Add(new ModelDb.qns_questions
                    {
                        yy_user_sn=new UserIdentityBag().user_sn,
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        step = req.GetPara()["step"].ToInt(),
                        question = item.question,
                        qns_sn = UtilityStatic.CommonHelper.CreateUniqueSn()
                    }.InsertOrUpdateTran($"id='{req.GetPara()["id"].ToInt()}'"));
                }

                MysqlHelper.ExecuteSqlTran(lSql);
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                public List<l_Questions> l_questions { get; set; }
            }
            public class l_Questions
            {
                public string question { get; set; }
                public string qns_sn { get; set; }
                #endregion
            }




        }

        /// <summary>
        /// 
        /// </summary>
        public class JiezouQPost
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
                var qns_questions = DoMySql.FindEntity<ModelDb.qns_questions>($"id='{req.id}'", false);
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = qns_questions.id.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("step")
                {
                    title = "阶段",
                    options = new Dictionary<string, string>
                    {
                        {"0.5阶段","1" },
                        {"1阶段","2" },
                        {"2阶段","3" },
                        {"3阶段","4" },
                        {"4阶段","5" },
                        {"其他","6" },
                    },
                    defaultValue= qns_questions.step.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("question")
                {
                    title = "问题",
                    defaultValue = qns_questions.question,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("sort")
                {
                    title = "排序",
                    defaultValue = qns_questions.sort.ToString(),
                });
                #endregion
                return formDisplay;
            }


            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
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
                var qns_questions = req.data_json.ToModel<ModelDb.qns_questions>();
                qns_questions.Update();

                var result = new JsonResultAction();
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


        public class JiezouSPost
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
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.jiezou>($"yy_user_sn='{new UserIdentityBag().user_sn}'"))
                {
                    option.Add(item.term, item.jiezou_sn);
                }

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("qns_sn")
                {
                    defaultValue = req.qns_sn,
                    colLength = 6
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("question")
                {
                    displayStatus= EmtModelBase.DisplayStatus.只读,
                    title = "问题",
                    defaultValue = DoMySql.FindEntity<ModelDb.qns_questions>($"qns_sn='{req.qns_sn}'").question,
                    colLength = 6
                });

                var list = DoMySql.FindList<ModelDb.qns_solutions>($"qns_sn='{req.qns_sn}'");
                string defaultValue = "";
                if (list.Count > 0)
                {
                    int sort = 0;
                    foreach (var item in list)
                    {
                        sort++;
                        defaultValue += $@"{{id:'{sort}',solution_sn:'{item.solution_sn}',solution:'{item.solution}'}},";
                    }
                    defaultValue ="[" + defaultValue.Substring(0, defaultValue.Length - 1) + "]";
                }
                
                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_solutions")
                {
                    title = "解决方案",
                    colItems = new List<EmtDataSelect.ColItem>
                    {
                        new EmtDataSelect.ColItem("solution_sn")
                        {
                            isHide=true,
                            title="solution_sn",
                            edit="text",
                        },
                        new EmtDataSelect.ColItem("solution")
                        {
                            title="解决方案",
                            edit="text",
                            width="700"
                        }
                    },
                    defaultValue = defaultValue,
                    width = "800px"
                });

                #endregion
                return formDisplay;
            }


            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public int id { get; set; }
                public string qns_sn { get; set; }
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
                var list = DoMySql.FindList<ModelDb.qns_solutions>($"qns_sn='{req.GetPara()["qns_sn"].ToNullableString()}'");
                foreach(var item in list)
                {
                    if (dtoReqData.l_solutions.Find(x => x.solution_sn == item.solution_sn).IsNullOrEmpty())
                    {
                        lSql.Add(new ModelDb.qns_solutions().DeleteTran($"solution_sn='{item.solution_sn}'"));
                    }
                }
                foreach (var solution in dtoReqData.l_solutions)
                {
                    lSql.Add(new ModelDb.qns_solutions
                    {
                        qns_sn = req.GetPara()["qns_sn"].ToNullableString(),
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        solution = solution.solution,
                        yy_user_sn = new UserIdentityBag().user_sn,
                        solution_sn = (solution.solution_sn.IsNullOrEmpty() || solution.solution_sn == "输入内容") ? UtilityStatic.CommonHelper.CreateUniqueSn() : solution.solution_sn
                    }.InsertOrUpdateTran($"solution_sn='{solution.solution_sn}'"));
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                public List<l_Solutions> l_solutions { get; set; }
            }
            public class l_Solutions
            {
                public string solution_sn { get; set; }
                public string solution { get; set; }
            }
            #endregion
        }
        #endregion
    }
}
