using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Domain;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static WeiCode.Models.ModelBasic;
using WeiCode.Modular;

namespace Services.Project
{
    public partial class PageFactory
    {
        #region 用户列表
        /// <summary>
        /// 用户列表
        /// </summary>
        public class CrmList : CrmBaseTemplate<CrmList.Req>
        {
            protected override CrmList.Req SetCrmListReq(CrmList.Req req)
            {
                req.crm_type_name = "用户";
                return req;
            }

            #region DefaultView
            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override CtlListFilter SetListFilter(CrmList.Req req)
            {

                var listFilter = new CtlListFilter(req);
                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "运营账号",
                    disabled = true,
                    options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                    {
                        attachWhere = $"status = {ModelDb.user_base.status_enum.正常.ToSByte()}"
                    }),
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
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
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
                    options = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                    {
                        attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                        {
                            userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅管,
                            UserSn = new UserIdentityBag().user_sn
                        }                        
                    }),
                    disabled = true
                });

                listFilter.formItems.Add(new ModelBasic.EmtInput("nick")
                {
                    placeholder = "用户昵称",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("dou_user")
                {
                    placeholder = "用户抖音号",
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
            /// 获取主播筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetZhubo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new ServiceFactory.UserInfo.Zhubo().GetBaseInfosForKv(new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter
                {
                    attachUserType = new ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType
                    {
                        userType = ServiceFactory.UserInfo.Zhubo.ZbBaseInfoFilter.AttachUserType.UserType.厅管,
                        UserSn = req["tg_user_sn"].ToNullableString()
                    } 
                });
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override EmtButtonGroup SetButtonGroup(CrmList.Req req)
            {
                var buttonGroup = new EmtButtonGroup("");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("create")
                {
                    title = "create",
                    text = "新建用户",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/PCrm/PCrm/CrmPost",
                    },
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("gradeLog")
                {
                    text = "等级变化",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Crm/Follow/GradeLogList",
                    },
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("follow")
                {
                    text = "跟进记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Crm/Follow/List",
                    },
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("recover")
                {
                    text = "流失回收",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/PCrm/PCrm/Recover",
                    },
                });
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("import")
                {
                    text = "导入用户",
                    title = "import",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/PCrm/PCrm/Import",
                    },
                    disabled = true
                });
                return buttonGroup;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override CtlListDisplay SetListDisplay(CrmList.Req req)
            {
                var listDisplay = new CtlListDisplay(req);
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = new CrmListReq().GetListData
                };
                #region 1.显示列
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("first_time")
                {
                    text = "第一次进厅时间",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "厅管昵称",
                    width = "120",
                    minWidth = "120",
                    disabled = true
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "主播昵称",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("nick")
                {
                    text = "用户昵称",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_user")
                {
                    text = "用户抖音号",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_level")
                {
                    text = "抖音号等级",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("has_wechat")
                {
                    text = "是否加V",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("first_consum")
                {
                    text = "首次消费值",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_time")
                {
                    text = "建联时间",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("contact_days")
                {
                    text = "建联时长",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address")
                {
                    text = "家乡/现居地",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("type_text")
                {
                    text = "认识方式",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_job")
                {
                    text = "用户职业",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("life_text")
                {
                    text = "用户作息",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("emo_status")
                {
                    text = "感情状态",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_like")
                {
                    text = "爱好",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_birthday")
                {
                    text = "生日",
                    width = "120",
                    minWidth = "120",
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("demo")
                {
                    text = "备注",
                    width = "280",
                    minWidth = "280",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                {
                    text = "创建时间",
                    width = "180",
                    minWidth = "180",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mbti_text")
                {
                    text = "人格属性(MBTI)",
                    width = "180",
                    minWidth = "180",
                });
                #endregion
                #region 2.批量操作列
                //listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                //{
                //    text = "批量操作",

                //    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                //    {
                //        new ModelBasic.EmtModel.ButtonItem("")
                //        {
                //            text = "转移所属用户",
                //            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                //            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                //            {
                //                url="/PCrm/Customer/Moves"
                //             },
                //        },
                //    }
                //});
                #endregion
                #region 3.操作列
                listDisplay.listOperateItems.Add(new EmtModel.ListOperateItem()
                {
                    name = "edit",
                    text = "编辑",
                    actionEvent = EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "CrmPost",
                        field_paras = "id"
                    },
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "del",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    text = "删除",
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "recover",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    disabled = true,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = Recover,
                        field_paras = "id",
                    },
                    text = "恢复",
                });
                #endregion
                return listDisplay;
            }
            #endregion
            #region 回调cs函数
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_crm_customer = DoMySql.FindEntity<ModelDb.p_crm_customer>($"id='{req.GetPara()["id"].ToNullableString().ToInt()}'", false);
                if (!p_crm_customer.IsNullOrEmpty())
                {
                    p_crm_customer.status = 9;
                    p_crm_customer.Update();
                }
                else
                {
                    throw new WeicodeException("未找到此用户，无法恢复");
                }
                return result;
            }

            public JsonResultAction Recover(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_crm_customer = DoMySql.FindEntity<ModelDb.p_crm_customer>($"id='{req.GetPara()["id"].ToNullableString().ToInt()}'", false);
                if (!p_crm_customer.IsNullOrEmpty())
                {
                    p_crm_customer.status = 0;
                    p_crm_customer.Update();
                }
                else
                {
                    throw new WeicodeException("未找到此用户，无法恢复");
                }
                return result;
            }
            #endregion


            public class Req : CrmBaseTemplateReq
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
            }
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        public class CrmListReq : CrmBaseTemplateReq
        {
            #region AJAX函数:ListData

            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = $"1=1";
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn in {new ServiceFactory.UserInfo.Tg().GetTreeOptionForSql(req["yy_user_sn"].ToNullableString())}";
                }
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"]}'";
                }
                if (!req["zb_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and zb_user_sn = '{req["zb_user_sn"]}'";
                }
                if (!req["nick"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and nick like '%{req["nick"]}%'";
                }
                if (!req["dou_user"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and dou_user = '{req["dou_user"]}'";
                }
                where = where + " order by p_crm_customer.id desc";
                return new CrmBaseTemplateHelp().getList<ModelDb.p_crm_customer, ItemDataModel>(where, reqJson);
            }

            /// <summary>
            /// 获取厅管筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                result.data = DoMySql.FindKvList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, new UserIdentityBag().user_sn)}", "name,user_sn");
                return result;
            }

            /// <summary>
            /// 获取主播筛选项
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
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_crm_customer
            {
                public string contact_days
                {
                    get
                    {
                        return contact_time.IsNullOrEmpty() ? "" : (DateTime.Now - contact_time.ToDate()).Days.ToString();
                    }
                }
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
                public string type_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("与用户的认识方式", know_type);
                    }
                }
                public string life_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("用户生活作息", user_life);
                    }
                }
                public string mbti_text
                {
                    get
                    {
                        return new DomainBasic.DictionaryApp().GetKeyFromValue("MBTI", mbti);
                    }
                }
            }
            #endregion
            public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
        }
        #endregion

        #region 新增用户信息
        /// <summary>
        /// 新增用户信息
        /// </summary>
        public class CrmPost : CrmPostBaseTemplate<CrmPost.Req>
        {
            protected override CrmPost.Req SetCrmPostReq(CrmPost.Req req)
            {
                req.crm_type_name = "用户";
                //1.校验
                if (req.p_Crm_Customer.id != 0)
                {
                    //2.编辑页面回显
                    req.p_Crm_Customer = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_crm_customer>($"id = {req.p_Crm_Customer.id}");
                    req.crm_sn = req.p_Crm_Customer.crm_sn;
                }
                return req;
            }

            protected override PagePost.EventCsAction SetEventCsAction(CrmPost.Req req)
            {
                var eventCsAction = new PagePost.EventCsAction
                {
                    func = new CrmPostObj().PostAction,
                };
                return eventCsAction;
            }

            #region DefaultView

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            protected override CtlFormDisplay SetFormDisplay(CrmPost.Req req)
            {
                var formDisplay = new CtlFormDisplay();
                var p_crm_customer = req.p_Crm_Customer;
                formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                {
                    title = "基本信息",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("p_Crm_Customer.id")
                {
                    title = "id",
                    defaultValue = p_crm_customer.id.ToString(),
                    colLength = 6
                });
                if (new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("zber").id)
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("p_Crm_Customer.zb_user_sn")
                    {
                        title = "zb_user_sn",
                        defaultValue = new UserIdentityBag().user_sn,
                        colLength = 6
                    });
                }
                else
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_Crm_Customer.zb_user_sn")
                    {
                        title = "所属主播",
                        isRequired = true,
                        options = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.厅管邀主播, new UserIdentityBag().user_sn),
                        defaultValue = p_crm_customer.zb_user_sn.ToNullableString(),
                        colLength = 6
                    });
                }

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("p_Crm_Customer.crm_sn")
                {
                    title = "crm_sn",
                    defaultValue = p_crm_customer.crm_sn,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtImageSelect("p_Crm_Customer.img_url")
                {
                    title = "用户头像",
                    defaultValue = p_crm_customer.img_url,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.nick")
                {
                    title = "用户昵称",
                    isRequired = true,
                    defaultValue = p_crm_customer.nick,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.dou_user")
                {
                    title = "用户抖音号",
                    isRequired = true,
                    defaultValue = p_crm_customer.dou_user,
                    colLength = 6,
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.user_level")
                {
                    title = "抖音号等级",
                    defaultValue = p_crm_customer.user_level,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.address")
                {
                    title = "家乡/现居地",
                    defaultValue = p_crm_customer.address,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_Crm_Customer.know_type")
                {
                    title = "认识方式",
                    isRequired = true,
                    options = new DomainBasic.DictionaryApp().GetListForOption("与用户的认识方式"),
                    defaultValue = p_crm_customer.know_type,
                    colLength = 6
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.user_job")
                {
                    title = "用户职业",
                    defaultValue = p_crm_customer.user_job,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_Crm_Customer.user_life")
                {
                    title = "生活作息",
                    options = new DomainBasic.DictionaryApp().GetListForOption("用户生活作息"),
                    defaultValue = p_crm_customer.user_life,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.emo_status")
                {
                    title = "感情状态",
                    defaultValue = p_crm_customer.emo_status,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.user_like")
                {
                    title = "爱好",
                    defaultValue = p_crm_customer.user_like,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("p_Crm_Customer.user_birthday")
                {
                    title = "生日",
                    defaultValue = p_crm_customer.user_birthday,
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.date
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("p_Crm_Customer.first_time")
                {
                    title = "首次进厅",
                    defaultValue = p_crm_customer.first_time.ToString(),
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.datetime
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_Crm_Customer.mbti")
                {
                    title = "人格属性(MBTI)",
                    options = new DomainBasic.DictionaryApp().GetListForOption("MBTI"),
                    defaultValue = p_crm_customer.mbti,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("p_Crm_Customer.has_wechat")
                {
                    title = "是否加V",
                    isRequired = false,
                    options = new Dictionary<string, string>
                    {
                        {"是","是"},
                        {"否","否"},
                    },
                    colLength = 6,
                    defaultValue = p_crm_customer.has_wechat,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("p_Crm_Customer.first_consum")
                {
                    title = "首次消费值",
                    defaultValue = p_crm_customer.first_consum,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("p_Crm_Customer.contact_time")
                {
                    title = "建联时间",
                    defaultValue = p_crm_customer.contact_time.ToString(),
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.datetime
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("p_Crm_Customer.demo")
                {
                    title = "备注",
                    defaultValue = p_crm_customer.demo,
                    colLength = 12,
                    mode = EmtTextarea.Mode.TextArea,
                    placeholder = "最大不超过500个字符!",//todo: 未实现
                    index = 2000
                });
                return formDisplay;
            }
            #endregion

            /// <summary>
            /// 客户对象
            /// </summary>
            public class Req : CrmPostBaseTemplateReq
            {
                public ModelDb.p_crm_customer p_Crm_Customer { get; set; }
            }
        }

        /// <summary>
        /// 粉丝对象
        /// </summary>
        public class CrmPostObj
        {
            #region AJAX函数:表单提交
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var grade_id = req.GetPara("grade_id").ToInt();
                var p_crm_customer = req.GetPara<ModelDb.p_crm_customer>("p_Crm_Customer");

                if (p_crm_customer.nick.IsNullOrEmpty()) throw new WeicodeException("用户昵称不可为空！");
                p_crm_customer.user_grade = grade_id.ToString();
                if (p_crm_customer.user_grade.IsNullOrEmpty() || p_crm_customer.user_grade == "0") throw new WeicodeException("客户等级必填");

                if (p_crm_customer.id > 0)//修改
                {
                    lSql.Add(new ModelDb.crm_base
                    {
                        grade_id = grade_id
                    }.UpdateTran($"crm_sn = '{p_crm_customer.crm_sn}'"));
                    lSql.Add(p_crm_customer.InsertOrUpdateTran($"id = {p_crm_customer.id}"));
                }
                else
                {
                    p_crm_customer.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_crm_customer.tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, p_crm_customer.zb_user_sn);
                    p_crm_customer.crm_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    lSql.Add(new ModelDb.crm_base
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        crm_sn = p_crm_customer.crm_sn,
                        user_sn = new UserIdentityBag().user_sn,
                        type_id = 1,
                        grade_id = grade_id
                    }.InsertTran());
                    lSql.Add(p_crm_customer.InsertOrUpdateTran());
                }
                DoMySql.ExecuteSqlTran(lSql);

                return result;
            }
            /// <summary>
            /// 客户对象
            /// </summary>
            public class CrmPostReq : CrmPostBaseTemplateReq
            {
                public ModelDb.p_crm_customer p_Crm_Customer { get; set; }
            }
            #endregion
        }
        #endregion

        #region 导入用户
        /// <summary>
        /// 导入用户页面
        /// </summary>
        public class CrmImport
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
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
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
                #region 表单元素

                formDisplay.formItems.Add(new ModelBasic.EmtExcelRead("l_p_crm_customer")
                {
                    title = "",
                    colItems = new List<ModelBasic.EmtExcelRead.ColItem>
                    {
                        new ModelBasic.EmtExcelRead.ColItem("zb_username")
                        {
                            title = "主播名称",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("nick")
                        {
                            title = "用户昵称",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("dou_user")
                        {
                            title = "用户抖音号",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_level")
                        {
                            title = "用户等级",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("demo")
                        {
                            title = "备注",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("address")
                        {
                            title = "家乡/现居地",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("know_type_text")
                        {
                            title = "认识方式",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_job")
                        {
                            title = "用户职业",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_life_text")
                        {
                            title = "生活作息",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("emo_status")
                        {
                            title = "感情状态",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_like")
                        {
                            title = "爱好",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_birthday")
                        {
                            title = "生日",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("first_time_text")
                        {
                            title = "首次进厅",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("mbti_text")
                        {
                            title = "人格属性(MBTI)",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("has_wechat")
                        {
                            title = "是否加V",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("first_consum")
                        {
                            title = "首次消费值",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("contact_time_text")
                        {
                            title = "建联时间",
                            edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("user_grade_text")
                        {
                            title = "用户评级",
                            edit = "text",
                        },
                    },
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq : ModelBasic.PagePost.Req
            {

            }
            #endregion

            #region 异步请求处理
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                if (dtoReqData.l_p_crm_customer.IsNullOrEmpty()) throw new WeicodeException("请上传数据");
                int sort = 0;
                foreach (var item in dtoReqData.l_p_crm_customer)
                {
                    sort++;
                    if (item.nick.IsNullOrEmpty()) throw new WeicodeException("用户昵称不可为空");
                    // --TODO 需要修改
                    //var zb_user = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(new UserIdentityBag().user_sn);
                    //if (!zb_user.IsNullOrEmpty())
                    //{

                    //}
                    // --TODO 需要修改
                    var zb_user = DoMySql.FindEntity<ModelDb.user_info_zb>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tg_user_sn = '{new UserIdentityBag().user_sn}' and name = '{item.zb_username}'", false);
                    if (zb_user.IsNullOrEmpty()) throw new WeicodeException("主播名称不存在");

                    item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    item.crm_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    item.zb_user_sn = zb_user.user_sn;
                    item.tg_user_sn = new UserIdentityBag().user_sn;
                    item.know_type = new DomainBasic.DictionaryApp().GetValueFromKey("与用户的认识方式", item.know_type_text);
                    item.user_life = new DomainBasic.DictionaryApp().GetValueFromKey("用户生活作息", item.user_life_text);
                    item.mbti = new DomainBasic.DictionaryApp().GetValueFromKey("MBTI", item.mbti_text);
                    // --TODO 需要修改
                    int? grade_id = DoMySql.FindEntity<ModelDb.crm_grade>($"type_id = 1 and name = '{item.user_grade_text}'", false).id;
                    item.user_grade = grade_id.ToString();

                    if (item.user_grade.IsNullOrEmpty() || item.user_grade == "0") throw new WeicodeException("客户等级必填");

                    if (!item.first_time_text.Equals("-")) item.first_time = item.first_time_text.ToDateTime();
                    if (!item.contact_time_text.Equals("-")) item.contact_time = item.contact_time_text.ToDateTime();

                    lSql.Add(item.ToModel<ModelDb.p_crm_customer>().InsertTran());
                    lSql.Add(new ModelDb.crm_base
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        crm_sn = item.crm_sn,
                        user_sn = new UserIdentityBag().user_sn,
                        type_id = 1,
                        grade_id = grade_id
                    }.InsertTran());
                }
                DoMySql.ExecuteSqlTran(lSql);

                //更新对象容器数据
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.p_crm_customer
            {
                public List<p_crm_customer> l_p_crm_customer { get; set; }
            }
            public class p_crm_customer : ModelDb.p_crm_customer
            {
                public string zb_username { get; set; }
                public string know_type_text { get; set; }
                public string user_life_text { get; set; }
                public string mbti_text { get; set; }
                public string user_grade_text { get; set; }
                public string first_time_text { get; set; }
                public string contact_time_text { get; set; }

            }
            #endregion
        }
        #endregion
    }
}
