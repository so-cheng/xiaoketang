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
        #region 转移厅管
        /// <summary>
        /// 账号转移管理
        /// </summary>
        public class YYMovePost
        {
            #region DefaultView
            public PagePost Get(DtoReq req)
            {
                //设置tab页
                var pageModel = new PagePost("");
                string htmlTop = @"<div class=""layui-tab"" style=""width:100%;"">
                                        <ul class=""layui-tab-title"">";

                foreach (var item in new DomainUserBasic.UserRelationTypeApp().GetUser_Relation_Types())
                {
                    if ((ModelEnum.UserRelationTypeEnum)item.id == ModelEnum.UserRelationTypeEnum.运营邀厅管) //运营没有转移厅管到其他运营的功能
                    {
                        continue;
                    }
                    htmlTop += $@"<li class=""{(req.type_id.Equals(item.id) ? "layui-this" : "")}"" onclick=""location.href='?type_id={item.id}'"">{item.cname}</li>";
                }
                htmlTop += @"</ul>
                        </div>";
                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = htmlTop
                    }
                };

                pageModel.postedReturn = new PagePost.PostedReturn
                {
                    returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                };
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                    attachPara = new Dictionary<string, object>
                    {
                        {"type_id", req.type_id}
                    }
                };
                return pageModel;
            }

            /// <summary>
            /// 配置按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public EmtButtonGroup GetButtonGroup(DtoReq req)
            {
                var buttonGroup = new EmtButtonGroup("");

                buttonGroup.buttonItems.Add(new EmtModel.ButtonItem("查看转移记录")
                {
                    text = "转移记录",
                    mode = EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = $"MoveList?type_id={req.type_id}",
                    }
                });
                return buttonGroup;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
            {
                //获取下拉数据
                //判断当前关系类型
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                //获取当前运营的所有下属厅管
                List<ModelDoBasic.Option> users = new ServiceFactory.UserInfo.Ting().GetTingidsOptions(new UserIdentityBag().user_sn);



                //new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管,new UserIdentityBag().user_sn);

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new EmtSelectFull("user_sn_before")
                {
                    title = "当前厅",
                    options = users,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventComponent = new EmtDataSelect.Js("l_move").clear()
                    }
                });
                formDisplay.formItems.Add(new EmtSelectFull("user_sn_after")
                {
                    title = "转移至",
                    options = users,
                });
                formDisplay.formItems.Add(new EmtInput("cause")
                {
                    title = "转移事由",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                {
                    title = "转移账号",
                    selectUrl = $"Select?user_sn=<%=page.user_sn_before.value%>&type_id={req.type_id}&isolated=0",
                    buttonText = "选择需要流转的账号",
                    buttonAddOneText = null,
                    colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                    {
                         new ModelBasic.EmtDataSelect.ColItem("username")
                         {
                              edit = "text",
                              title = "用户名"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              edit = "text",
                              title = "备注"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("mobile")
                         {
                              edit = "text",
                              title = "手机号码",
                         },
                    }
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                /// <summary>
                /// 类型id
                /// </summary>
                public int type_id { get; set; }
            }
            #endregion

            #region 异步请求处理
            /// <summary>
            /// 转移用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                //1.数据校验
                int type_id = req.GetPara("type_id").ToInt();
                if (type_id <= 0) throw new WeicodeException("type_id不能为空!");
                if (req.GetPara("user_sn_before").IsNullOrEmpty()) throw new WeicodeException("选择当前厅管不能为空!");
                if (req.GetPara("user_sn_after").IsNullOrEmpty()) throw new WeicodeException("选择目标厅管不能为空!");
                var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");

                //todo: 判断目标用户是否为当前运营的厅管
                //todo: 如果转移主播，则需要判断目标厅管是否为单厅管还是多厅管？
                //todo: 判断user_sn_before和所属的moveitems是否一致
                //方案1：联动刷新

                //3.转移到目标用户
                var _ = new DomainUserBasic.UserRelationApp().MoveNextUsersToUserSn((ModelEnum.UserRelationTypeEnum)type_id, req.GetPara("user_sn_before"), moveItems, req.GetPara("user_sn_after"), req.GetPara("cause"));

                //更新user_info_zb表的tg_user_sn与ting_sn字段
                switch (type_id)
                {
                    case 1:   //厅管邀主播
                        foreach (var item in moveItems)
                        {
                            var user_relation = DoMySql.FindEntity<ModelDb.user_relation>($"id = '{item.id}'");
                            new ModelDb.user_info_zb
                            {
                                ting_sn = req.GetPara("user_sn_after"),
                                tg_user_sn = req.GetPara("user_sn_after"),
                            }.Update($"user_sn = '{user_relation.t_user_sn}'");

                            new ModelDb.user_info_zhubo
                            {
                                ting_sn = req.GetPara("user_sn_after"),
                                tg_user_sn = req.GetPara("user_sn_after"),
                            }.Update($"user_sn = '{user_relation.t_user_sn}'");
                        }
                        break;
                    case 3:   //厅管邀厅管

                        break;
                }
                return new JsonResultAction();
            }
            #endregion
        }

        /// <summary>
        /// 选择用户表单
        /// </summary>
        public class YYSelect
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
                pageModel.listDisplay = GetListDisplay(req);
                pageModel.listFilter.isExport = true;
                pageModel.dataSelect.selectEvent.cbSelected = ModelBasic.EmtDataSelect.reloadListByData();
                return pageModel;
            }
            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new CtlListFilter(req);
                listFilter.formItems.Add(new EmtInput("username")
                {
                    placeholder = "用户名称",
                    defaultValue = ""
                });
                listFilter.formItems.Add(new EmtHidden("isolated")
                {
                    placeholder = "是否孤立",
                    defaultValue = req.isolated.ToString()
                });
                return listFilter;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                var listDisplay = new CtlListDisplay(req);
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData,
                    attachPara = new Dictionary<string, object>
                    {
                        {"type_id", req.type_id},
                        {"user_sn", req.user_sn}
                    }
                };
                #region 1.显示列
                listDisplay.listItems.Add(new EmtModel.ListItem("username")
                {
                    text = "用户名称",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("mobile")
                {
                    text = "手机号",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("name")
                {
                    text = "备注",
                    width = "120",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("user_type_name")
                {
                    text = "用户类型",
                    width = "120",
                    minWidth = "180",
                });
                #endregion
                return listDisplay;
            }
            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : PageList.Req
            {
                /// <summary>
                /// 是否孤立 1:是，0:否
                /// </summary>
                public int isolated { get; set; }
                /// <summary>
                /// 关系类型id
                /// </summary>
                public int type_id { get; set; }

                /// <summary>
                /// user_sn
                /// </summary>
                public string user_sn { get; set; }
            }
            #endregion
            #region ListData
            /// <summary>
            /// 用户表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                string where = "";
                var isolated = reqJson.GetPara("isolated");
                int type_id = reqJson.GetPara("type_id").ToInt();
                string user_sn = reqJson.GetPara("user_sn");

                if (isolated.IsNullOrEmpty()) throw new WeicodeException("是否为独立用户不能为空!");
                if (type_id <= 0) throw new WeicodeException("type_id不能为空!");

                if (!reqJson.GetPara("username").IsNullOrEmpty()) where += $"username like '%{reqJson.GetPara("username")}%'";

                if (isolated == "1")//独立
                {
                    switch ((ModelEnum.UserRelationTypeEnum)type_id)
                    {
                        case ModelEnum.UserRelationTypeEnum.运营邀厅管:
                        case ModelEnum.UserRelationTypeEnum.厅管邀主播:
                            return new ModularUserBasic.UserRelationApp().GetNextUsersUnrelat<ItemDataModel>(reqJson, (ModelEnum.UserRelationTypeEnum)type_id,
                                new DoMySql.Filter
                                {
                                    where = where
                                }
                             );
                        case ModelEnum.UserRelationTypeEnum.厅管邀厅管:
                            return new ModularUserBasic.UserRelationApp().GetNextUsersUnrelat<ItemDataModel>(reqJson, (ModelEnum.UserRelationTypeEnum)type_id,
                                new DoMySql.Filter
                                {
                                    where = where + $" user_sn in ({new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)})"
                                }
                             );
                        default:
                            throw new WeicodeException("关系类型不正确!");
                    }
                }
                return new ModularUserBasic.UserRelationApp().GetNextUsers<ItemDataModel>(reqJson, (ModelEnum.UserRelationTypeEnum)type_id, user_sn,
                    new DoMySql.Filter
                    {
                        where = where
                    }
                );
            }
            ///// <summary>
            ///// 数据项模型
            ///// </summary>
            public class ItemDataModel : ModelDb.user_base
            {
                public string user_type_name
                {
                    get
                    {
                        return DoMySql.FindEntityById<ModelDb.user_type>(this.user_type_id).name;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 用户转移列表
        /// </summary>
        public class YYMoveList
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
                pageModel.listDisplay = GetListDisplay(req);
                pageModel.listFilter.isExport = true;
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
                listFilter.formItems.Add(new EmtInput("username")
                {
                    placeholder = "用户名称",
                    defaultValue = ""
                });

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
                listDisplay.operateWidth = "180";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData,
                    attachPara = new Dictionary<string, object>
                    {
                        {"type_id",req.type_id}
                    }
                };
                listDisplay.listItems.Add(new EmtModel.ListItem("id")
                {
                    text = "转移单号",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("user_names")
                {
                    text = "用户名",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("user_before_name")
                {
                    text = "原上级用户",
                    width = "100",
                    minWidth = "100",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("user_after_name")
                {
                    text = "现上级用户",
                    width = "100",
                    minWidth = "100",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("ac_date")
                {
                    text = "转移日期",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("notes")
                {
                    text = "转移事由",
                    width = "200",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("op_username")
                {
                    text = "操作人",
                    width = "200",
                    minWidth = "180",
                });
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                /// <summary>
                /// 关系类型id
                /// </summary>
                public int type_id;
            }
            #endregion
            #region ListData
            /// <summary>
            /// 获取当前登录user_sn的转移操作记录
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                //1.校验
                int type_id = reqJson.GetPara("type_id").ToInt();
                if (type_id <= 0) throw new WeicodeException("type_id不能为空!");
                string where = $"1=1";
                if (!reqJson.GetPara("username").ToNullableString().IsNullOrEmpty()) where += $" AND (user_names like '%{reqJson.GetPara("username")}%')";

                //2.获取当前登录user_sn指定关系类型的转移操作记录
                var filter = new DoMySql.Filter
                {
                    where = where + $" AND (relation_type_id = {type_id})"
                                  + $"AND (user_sn = '{new UserIdentityBag().user_sn}')",
                };
                return new CtlListDisplay.ListData().getList<ModelDb.user_relation_log, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.user_relation_log
            {
                public string user_before_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.f_user_sn}'", false).username;
                    }
                }
                public string user_after_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.t_user_sn}'", false).username;
                    }
                }
                public string ac_date
                {
                    get
                    {
                        return this.create_time.ToDateTime().ToString("yyyy-MM-HH");
                    }
                }
                public string op_username
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'").username;
                    }
                }
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 解除厅管列表
        /// </summary>
        public class YYRemoveList
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
                pageModel.listDisplay = GetListDisplay(req);
                pageModel.listFilter.isExport = true;
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
                listFilter.formItems.Add(new EmtInput("username")
                {
                    placeholder = "用户名称",
                    defaultValue = ""
                });
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
                listDisplay.operateWidth = "180";
                listDisplay.isOpenNumbers = true;
                listDisplay.isOpenCheckBox = true;
                listDisplay.listData = new CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 显示列
                listDisplay.listItems.Add(new EmtModel.ListItem("username")
                {
                    text = "用户名",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("user_type_name")
                {
                    text = "用户类型",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("mobile")
                {
                    text = "手机号码",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new EmtModel.ListItem("name")
                {
                    text = "备注",
                    width = "280",
                    minWidth = "280",
                });
                #endregion
                #region 批量操作
                listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "批量操作",
                    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量解绑用户",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = RemoveAction,
                            },
                        },
                    }
                });
                #endregion
                return listDisplay;
            }
            /// <summary>
            /// 解绑操作
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction RemoveAction(JsonRequestAction req)
            {
                //1.获取id(user_relation中的id)
                var ids = req.GetPara("ids").Split(',').Select(int.Parse).ToArray();
                var users = req.GetPara<List<ListDataModel>>("check_data");

                //2.从user_relation中删除这些id
                List<string> lSql = new List<string>();
                foreach (var id in ids)
                {
                    lSql.Add(new ModelDb.user_relation().DeleteTran($"id = {id}"));
                }
                //3.将删除信息记录在log中
                lSql.Add(new ModelDb.user_relation_log
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    user_type_id = new DomainBasic.UserTypeApp().GetInfo().id,
                    user_sn = new UserIdentityBag().user_sn,
                    relation_type_id = new DomainUserBasic.UserRelationApp().GetInfoById(users[0].user_relation_id).relation_type_id,
                    f_user_type_id = new DomainUserBasic.UserRelationApp().GetInfoById(users[0].user_relation_id).f_user_type_id,
                    f_user_sn = new DomainUserBasic.UserRelationApp().GetInfoById(users[0].user_relation_id).f_user_sn,
                    t_user_type_id = null,
                    t_user_sn = null,
                    user_ids = string.Join(",", users.Select(p => p.user_id)),
                    user_names = string.Join(",", users.Select(p => p.username))
                }.InsertTran());
                DoMySql.ExecuteSqlTran(lSql);
                return new JsonResultAction();
            }

            /// <summary>
            /// 列表项模型
            /// </summary>
            public class ListDataModel : ModelDb.user_base
            {
                /// <summary>
                /// 用户关系表id
                /// </summary>
                public int user_relation_id { get; set; }
                /// <summary>
                /// 用户表id
                /// </summary>
                public int user_id { get; set; }
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
            /// 下属用户list
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                string where = $"1=1";
                if (!reqJson.GetPara("username").ToNullableString().IsNullOrEmpty()) where += $" AND (username like '%{reqJson.GetPara("username")}%')";

                //获取当前运营账号的下级用户
                return new ModularUserBasic.UserRelationApp().GetNextUsers<ItemDataModel>(reqJson, ModelEnum.UserRelationTypeEnum.运营邀厅管,
                    new UserIdentityBag().user_sn, new DoMySql.Filter
                    {
                        where = where
                    }
                );
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.user_base
            {
                public int user_relation_id
                {
                    get
                    {
                        return this.id;
                    }
                }

                public string t_user_sn { get; set; }

                public int user_id
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.t_user_sn}'").id;
                    }
                }

                public string user_type_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_type>($"id = '{this.user_type_id}'").name;
                    }
                }
            }
            #endregion
        }
        /// <summary>
        /// 用户关系绑定列表
        /// </summary>
        public class YYBindList
        {
            #region DefaultView
            public PagePost Get(DtoReq req)
            {
                var pageModel = new PagePost("");
                string htmlTop = @"<div class=""layui-tab"" style=""width:100%;"">
                                        <ul class=""layui-tab-title"">";

                foreach (var item in new DomainUserBasic.UserRelationTypeApp().GetUser_Relation_Types())
                {
                    htmlTop += $@"<li class=""{(req.type_id.Equals(item.id) ? "layui-this" : "")}"" onclick=""location.href='?type_id={item.id}'"">{item.name}</li>";
                }
                htmlTop += @"</ul>
                        </div>";
                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = htmlTop
                    }
                };

                pageModel.postedReturn = new PagePost.PostedReturn
                {
                    returnType = PagePost.PostedReturn.ReturnType.刷新父窗口,
                };

                //判断当前tab页，不同的tab页面不同
                switch ((ModelEnum.UserRelationTypeEnum)req.type_id)
                {
                    case ModelEnum.UserRelationTypeEnum.运营邀厅管:
                        pageModel.formDisplay = GetFormDisplayYYTG(pageModel, req);
                        pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                        {
                            func = PostActionYYTG,
                            attachPara = new Dictionary<string, object>
                            {
                                {"type_id", req.type_id}
                            }
                        };
                        break;
                    case ModelEnum.UserRelationTypeEnum.厅管邀厅管:
                        pageModel.formDisplay = GetFormDisplayTGTG(pageModel, req);
                        pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                        {
                            func = PostActionTGTG,
                            attachPara = new Dictionary<string, object>
                            {
                                {"type_id", req.type_id}
                            }
                        };
                        break;
                    case ModelEnum.UserRelationTypeEnum.厅管邀主播:
                        pageModel.formDisplay = GetFormDisplayTGZB(pageModel, req);
                        pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                        {
                            func = PostActionTGZB,
                            attachPara = new Dictionary<string, object>
                            {
                                {"type_id", req.type_id}
                            }
                        };
                        break;
                    default:
                        throw new WeicodeException("请选择正确的tab页!");
                }


                return pageModel;
            }

            /// <summary>
            /// 配置运营邀厅管表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplayYYTG(PagePost pageModel, DtoReq req = null)
            {
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new EmtInput("cause")
                {
                    title = "绑定事由",
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                {
                    title = "绑定厅管",
                    selectUrl = $"Select?user_sn={new UserIdentityBag().user_sn}&isolated=1&type_id={req.type_id}",
                    buttonText = "选择独立厅管",
                    buttonAddOneText = null,
                    colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                    {
                        new ModelBasic.EmtDataSelect.ColItem("user_sn")
                         {
                              edit = "text",
                              title = "用户sn",
                              isHide = true
                         },
                         new ModelBasic.EmtDataSelect.ColItem("username")
                         {
                              edit = "text",
                              title = "用户名"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              edit = "text",
                              title = "备注"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("mobile")
                         {
                              edit = "text",
                              title = "手机号码",
                         },
                    }
                });
                #endregion
                return formDisplay;
            }
            /// <summary>
            /// 配置厅管邀厅管表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplayTGTG(PagePost pageModel, DtoReq req = null)
            {
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                //获取当前运营的所有下属厅管
                List<ModelDoBasic.Option> users = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn);

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new EmtSelectFull("user_sn_before")
                {
                    title = "选择下级厅管",
                    options = users,
                    colLength = 6
                });
                formDisplay.formItems.Add(new EmtInput("cause")
                {
                    title = "绑定事由",
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                {
                    title = "绑定厅管",
                    selectUrl = $"Select?user_sn={new UserIdentityBag().user_sn}&isolated=1&type_id={req.type_id}",
                    buttonText = "选择独立厅管",
                    buttonAddOneText = null,
                    colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                    {
                        new ModelBasic.EmtDataSelect.ColItem("user_sn")
                         {
                              edit = "text",
                              title = "用户sn",
                              isHide = true
                         },
                         new ModelBasic.EmtDataSelect.ColItem("username")
                         {
                              edit = "text",
                              title = "用户名"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              edit = "text",
                              title = "备注"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("mobile")
                         {
                              edit = "text",
                              title = "手机号码",
                         },
                    }
                });
                #endregion
                return formDisplay;
            }
            /// <summary>
            /// 配置厅管邀主播表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplayTGZB(PagePost pageModel, DtoReq req = null)
            {
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");

                //获取当前运营的所有下属厅管
                List<ModelDoBasic.Option> users = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn);

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new EmtInput("cause")
                {
                    title = "绑定事由",
                    colLength = 6
                });
                formDisplay.formItems.Add(new EmtSelectFull("user_sn_before")
                {
                    title = "选择下级厅管",
                    options = users,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtDataSelect("l_move")
                {
                    title = "绑定主播",
                    selectUrl = $"Select?isolated=1&type_id={req.type_id}",
                    buttonText = "选择独立主播",
                    buttonAddOneText = null,
                    colItems = new List<ModelBasic.EmtDataSelect.ColItem>
                    {
                        new ModelBasic.EmtDataSelect.ColItem("user_sn")
                         {
                              edit = "text",
                              title = "用户sn",
                              isHide = true
                         },
                         new ModelBasic.EmtDataSelect.ColItem("username")
                         {
                              edit = "text",
                              title = "用户名"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("name")
                         {
                              edit = "text",
                              title = "备注"
                         },
                         new ModelBasic.EmtDataSelect.ColItem("mobile")
                         {
                              edit = "text",
                              title = "手机号码",
                         },
                    }
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                /// <summary>
                /// 关系类型id
                /// </summary>
                public int type_id;
            }
            #endregion

            #region 异步请求处理
            /// <summary>
            /// 提交运营绑厅管的表单
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostActionYYTG(JsonRequestAction req)
            {
                //1.数据校验
                var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                if (moveItems.Count < 1) throw new WeicodeException("请选择用户!");

                //3.添加到数据库
                List<string> lSql = new List<string>();
                foreach (var item in moveItems)
                {
                    lSql.Add(new ModelDb.user_relation
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        relation_type_id = ModelEnum.UserRelationTypeEnum.运营邀厅管.ToInt(),
                        f_user_type_id = ModelEnum.UserTypeEnum.yyer.ToInt(),
                        f_user_sn = new UserIdentityBag().user_sn,
                        t_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                        t_user_sn = new DomainBasic.UserApp().GetInfoById(item.id).user_sn,
                        notes = req.GetPara("cause")
                    }.InsertTran());
                }
                //4.添加日志
                lSql.Add(new ModelDb.user_relation_log
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    o_type = 2,//绑定用户
                    user_type_id = new DomainBasic.UserTypeApp().GetInfo().id,
                    user_sn = new UserIdentityBag().user_sn,
                    relation_type_id = ModelEnum.UserRelationTypeEnum.运营邀厅管.ToInt(),
                    f_user_type_id = 0,
                    f_user_sn = null,
                    t_user_type_id = ModelEnum.UserTypeEnum.yyer.ToInt(),
                    t_user_sn = new UserIdentityBag().user_sn,
                    user_ids = string.Join(",", moveItems.Select(p => p.id)),
                    user_names = string.Join(",", moveItems.Select(p => p.username)),
                    notes = req.GetPara("cause"),
                }.InsertTran());
                DoMySql.ExecuteSqlTran(lSql);
                return new JsonResultAction();
            }
            /// <summary>
            /// 提交厅管绑厅管的表单
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostActionTGTG(JsonRequestAction req)
            {
                //1.数据校验
                var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                var user_sn_selected = req.GetPara("user_sn_before");
                if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");
                if (req.GetPara("user_sn_before").IsNullOrEmpty()) throw new WeicodeException("请选择下级厅管");

                //3.添加到数据库
                List<string> lSql = new List<string>();
                foreach (var item in moveItems)
                {
                    lSql.Add(new ModelDb.user_relation
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        relation_type_id = ModelEnum.UserRelationTypeEnum.厅管邀厅管.ToInt(),
                        f_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                        f_user_sn = user_sn_selected,
                        t_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                        t_user_sn = new DomainBasic.UserApp().GetInfoById(item.id).user_sn,
                        notes = req.GetPara("cause")
                    }.InsertTran());
                }
                //4.添加日志
                lSql.Add(new ModelDb.user_relation_log
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    o_type = 2,//绑定用户
                    user_type_id = new DomainBasic.UserTypeApp().GetInfo().id,
                    user_sn = new UserIdentityBag().user_sn,
                    relation_type_id = ModelEnum.UserRelationTypeEnum.厅管邀厅管.ToInt(),
                    f_user_type_id = ModelEnum.UserTypeEnum.yyer.ToInt(),
                    f_user_sn = new UserIdentityBag().user_sn,
                    t_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                    t_user_sn = user_sn_selected,
                    user_ids = string.Join(",", moveItems.Select(p => p.id)),
                    user_names = string.Join(",", moveItems.Select(p => p.username)),
                    notes = req.GetPara("cause"),
                }.InsertTran());
                DoMySql.ExecuteSqlTran(lSql);
                return new JsonResultAction();
            }
            /// <summary>
            /// 提交厅管绑主播的表单
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostActionTGZB(JsonRequestAction req)
            {
                //1.数据校验
                var moveItems = req.GetPara<List<DomainUserBasic.UserRelationApp.MoveItem>>("l_move");
                var user_sn_selected = req.GetPara("user_sn_before");
                if (moveItems == null || moveItems.Count < 1) throw new WeicodeException("请选择用户!");
                if (req.GetPara("user_sn_before").IsNullOrEmpty()) throw new WeicodeException("请选择下级厅管");

                //3.添加到数据库
                List<string> lSql = new List<string>();
                foreach (var item in moveItems)
                {
                    lSql.Add(new ModelDb.user_relation
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        relation_type_id = ModelEnum.UserRelationTypeEnum.厅管邀主播.ToInt(),
                        f_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                        f_user_sn = user_sn_selected,
                        t_user_type_id = ModelEnum.UserTypeEnum.zber.ToInt(),
                        t_user_sn = new DomainBasic.UserApp().GetInfoById(item.id).user_sn,
                        notes = req.GetPara("cause")
                    }.InsertTran());
                }
                //4.添加日志
                lSql.Add(new ModelDb.user_relation_log
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    o_type = 2,//绑定用户
                    user_type_id = new DomainBasic.UserTypeApp().GetInfo().id,
                    user_sn = new UserIdentityBag().user_sn,
                    relation_type_id = ModelEnum.UserRelationTypeEnum.厅管邀主播.ToInt(),
                    f_user_type_id = 0,
                    f_user_sn = null,
                    t_user_type_id = ModelEnum.UserTypeEnum.tger.ToInt(),
                    t_user_sn = user_sn_selected,
                    user_ids = string.Join(",", moveItems.Select(p => p.id)),
                    user_names = string.Join(",", moveItems.Select(p => p.username)),
                    notes = req.GetPara("cause"),
                }.InsertTran());
                DoMySql.ExecuteSqlTran(lSql);
                return new JsonResultAction();
            }
            #endregion
        }

        #region 更新上级用户关系
        public class UpdateUserRelation
        {
            #region DefaultView
            public PagePost Get(DtoReq req)
            {
                #region 设置tab页
                var pageModel = new PagePost("post");
                string htmlTop = @"<div class=""layui-tab"" style=""width:100%;"">
                                        <ul class=""layui-tab-title"">";

                foreach (var item in new DomainUserBasic.UserRelationTypeApp().GetUser_Relation_Types())
                {
                    htmlTop += $@"<li class=""{(req.type_id.Equals(item.id) ? "layui-this" : "")}"" onclick=""location.href='?type_id={item.id}'"">{item.name}</li>";
                }
                htmlTop += @"</ul>
                        </div>";
                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = htmlTop
                    }
                };
                #endregion
                #region 设置页面模型
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.jsFileNames.Add(PageModelBase.JsFileName.通知消息);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                    attachPara = new Dictionary<string, object>
                    {
                        {"type_id", req.type_id}
                    }
                };
                return pageModel;
                #endregion
            }
            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
            {
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");
                //1.获取当前tab的所有上级用户
                List<ModelDoBasic.Option> users = new DomainBasic.UserApp().GetUsersByUserTypeIdForOption(new DomainUserBasic.UserRelationTypeApp().GetInfoById(req.type_id).f_user_type_id);

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                if ((ModelEnum.UserRelationTypeEnum)req.type_id == ModelEnum.UserRelationTypeEnum.厅管邀厅管)
                {
                    formDisplay.formItems.Add(new EmtInput("username")
                    {
                        title = "用户名",
                        placeholder = "请输入用户名",
                        colLength = 6,
                        eventJsChanges = new List<EmtFormBase.EventJsChange>
                        {
                            new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    func = GetParentUser,
                                    resCallJs = $@"(res.data == null) ? { new WuiPage.Notify().Info("没有上级用户!")} : {new WuiPage("post.superior_username").Set(new WuiPageJs.Code().Run("res.data"))}",
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "type_id", req.type_id },
                                        { "username", $"{new WuiPageJs("post.username").Value}"},
                                    }
                                }
                            },
                            new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    func = GetOtherTgUserInCurrentYY,
                                    resCallJs = $@"{new ModelBasic.EmtSelect.Js("post.user_sn_selected").options(@"JSON.parse(res.data)")}",
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "username", $"{new WuiPageJs("post.username").Value}"},
                                    }
                                }
                            },
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelect("user_sn_selected")
                    {
                        title = "选择新的上级用户",
                        options = new Dictionary<string, string>(),
                        colLength = 6
                    });
                }
                else
                {
                    formDisplay.formItems.Add(new EmtInput("username")
                    {
                        title = "用户名",
                        placeholder = "请输入用户名",
                        colLength = 6,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                func = GetParentUser,
                                resCallJs = $@"(res.data == null) ? {new WuiPage.Notify().Info("没有上级用户!")} : {new WuiPage("post.superior_username").Set(new WuiPageJs.Code().Run("res.data"))}",
                                attachPara = new Dictionary<string, object>
                                {
                                    { "type_id", req.type_id },
                                    { "username", $"{new WuiPageJs("post.username").Value}"},
                                }
                            }
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_selected")
                    {
                        title = "选择新的上级用户",
                        options = users,
                        colLength = 6
                    });
                }

                formDisplay.formItems.Add(new ModelBasic.EmtLabel("superior_username")
                {
                    title = "当前上级用户",
                    colLength = 6,
                });

                #endregion
                return formDisplay;
            }

            /// <summary>
            /// 获取厅管所属运营下的厅管
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public JsonResultAction GetOtherTgUserInCurrentYY(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                //1.获取req中的信息
                var username = req.GetPara("username");

                //2.校验tg是否存在
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空");
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {ModelEnum.UserTypeEnum.tger.ToInt()}");
                if (users == null || users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");

                //3.获取tg所在的运营
                var superior_yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, users[0].user_sn);

                //4.获取运营下的厅管
                var option = new Dictionary<string, string>();
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, superior_yy_user_sn))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 获取上级用户
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public JsonResultAction GetParentUser(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                //1.获取req中的信息
                var username = req.GetPara("username");
                var type_id = req.GetPara("type_id").ToInt();
                int t_user_type_id = new DomainUserBasic.UserRelationTypeApp().GetInfoById(type_id).t_user_type_id;

                //2.校验用户名是否存在
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空");
                if (type_id < 1) throw new WeicodeException("请选择tab页");
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {t_user_type_id}");
                if (users == null || users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");

                //3.获取上级用户
                //3.1运营邀请厅管：判断厅管是否有上级厅管，有则不能修改
                if ((ModelEnum.UserRelationTypeEnum)type_id == ModelEnum.UserRelationTypeEnum.运营邀厅管)
                {
                    var superior_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, users[0].user_sn);
                    if (!superior_tg_user_sn.IsNullOrEmpty()) throw new WeicodeException($"用户 '{username}' 存在上级厅管，请先解除上级厅管!");
                }
                var superior_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn((ModelEnum.UserRelationTypeEnum)type_id, users[0].user_sn);
                if (superior_user_sn.IsNullOrEmpty()) throw new WeicodeException($"用户 '{username}' 不存在上级，请绑定上级用户!");
                result.data = new DomainBasic.UserApp().GetInfoByUserSn(superior_user_sn).username;
                return result;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                /// <summary>
                /// 类型id
                /// </summary>
                public int type_id { get; set; }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 更新上级用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                //1.数据校验
                //1.1 校验数据是否存在
                var username = req.GetPara("username");
                var type_id = req.GetPara("type_id").ToInt();
                var new_superior_user_sn = req.GetPara("user_sn_selected");
                var superior_username = req.GetPara("superior_username");
                if (type_id <= 0) throw new WeicodeException("type_id不能为空!");
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空!");
                if (new_superior_user_sn.IsNullOrEmpty()) throw new WeicodeException("请选择新的上级用户!");

                var user_relation_type = new DomainUserBasic.UserRelationTypeApp().GetInfoById(type_id);
                //1.2 判断用户是否存在
                int t_user_type_id = user_relation_type.t_user_type_id;
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {t_user_type_id}");
                if (users == null || users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");
                var t_user_sn = users[0].user_sn;

                //1.3 判断上级用户是否存在
                int f_user_type_id = user_relation_type.f_user_type_id;
                //todo：根据用户名和用户类型查找用户
                var superior_users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{superior_username}' and user_type_id = {f_user_type_id}");

                List<string> lSql = new List<string>();
                if (superior_users != null && superior_users.Count == 1)//上级用户存在
                {
                    //2 解绑用户关系
                    var superior_user_sn = superior_users[0].user_sn;
                    lSql = new DomainUserBasic.UserRelationApp().UnBindTran((ModelEnum.UserRelationTypeEnum)type_id, superior_user_sn, t_user_sn);
                }

                //3 绑定用户关系
                lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran((ModelEnum.UserRelationTypeEnum)type_id, new_superior_user_sn, t_user_sn));
                DoMySql.ExecuteSqlTran(lSql);

                return new JsonResultAction();
            }
            #endregion
        }
        #endregion

    }
}