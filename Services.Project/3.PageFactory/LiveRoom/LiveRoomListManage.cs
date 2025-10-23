using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using static WeiCode.Models.ModelBasic;
using System.Linq;

namespace Services.Project
{
    /// <summary>
    /// 直播间
    /// </summary>

    public partial class PageFactory
    {

        public partial class LiveRoom
        {

            /// <summary>
            /// 直播间日志
            /// </summary>
            public class LogList
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
                    pageModel.listDisplay = GetListDisplay(req);
                    return pageModel;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
                {
                    var listFilter = new ModelBasic.CtlListFilter();
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("c_type")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"直播间","1"},
                        {"直播间类别","2"},
                        {"直播间主播","3"}
                    },
                        placeholder = "日志类型",
                    });
                    return listFilter;
                }
                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("typename")
                    {
                        text = "类型",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                    {
                        text = "内容",
                        width = "300",
                        minWidth = "200",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "操作时间",
                        width = "200",
                        minWidth = "200",
                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string id { get; set; }
                }
                #endregion
                #region ListData

                /// <summary>
                /// 直播间日志查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and zt_user_sn = '{new UserIdentityBag().user_sn}'";

                    int c_type = req["c_type"].ToInt();
                    if (c_type != 0)
                    {
                        where += " and c_type=" + c_type;
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by create_time desc",
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_liveroom_log, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_liveroom_log
                {
                    /// <summary>
                    /// 状态文字描述
                    /// </summary>
                    public string typename
                    {
                        get
                        {
                            string name = "";
                            switch (this.c_type)
                            {
                                case 1:
                                    name = "直播间";
                                    break;
                                case 2:
                                    name = "直播间类别";
                                    break;
                                case 3:
                                    name = "主播";
                                    break;
                                default:

                                    break;
                            }

                            return name;
                        }
                    }
                }
                #endregion
            }


            /// <summary>
            /// 二维码列表
            /// </summary>
            public class LiveRoomList
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

                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    //中台看自己的,管理端看全部
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.manager:
                            break;
                        default:
                            where += $" and 1!=1";
                            break;
                    }

                    pageModel.leftPartial = new List<ModelBase>
                {
                   new ModelBasic.CtlTree("")
                   {

                       data = new DomainBasic.DataQueryApp().GetTree(0,"p_liveroom_area", null, where),
                       eventJsClick = new ModelBasic.CtlTree.EventJsClick
                       {
                            eventComponent = new ModelBasic.PageList.Js("pagelist").reload(new PageList.Js.Req{
                                s_type = "area_id",
                                s_value = "{{id}}"
                           })
                       }
                   }
                };
                    pageModel.leftPartialCols = 2;
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

                    listFilter.formItems.Add(new ModelBasic.EmtHidden("area_id")
                    {
                        placeholder = "区域",
                        defaultValue = req.area_id
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("status")
                    {
                        options = new Dictionary<string, string>
                    {
                        {"空闲","0"},
                        {"占用","1"},
                        {"预约","2"}
                    },
                        placeholder = "选择状态",
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "添加直播间",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/RoomPost",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "区域管理",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/AreaCate",
                        }
                    });
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "操作日志",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Manage/Log",
                        }
                    });
                    return buttonGroup;
                }

                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var shuye_c_qr = new ModelDb.p_liveroom();

                    //拼接id
                    List<string> idList = new List<string>();
                    if (!dtoReqData.ids.IsNullOrEmpty()) idList.AddRange(dtoReqData.ids.Split(',').ToList<string>());
                    if (!dtoReqData.id.IsNullOrEmpty()) idList.Add(dtoReqData.id.ToNullableString());
                    string ids = string.Join(",", idList);
                    //删除shuye_c_qr
                    lSql.Add(shuye_c_qr.DeleteTran($"id in ({ids})"));
                    var p_liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {dtoReqData.id}", exist: false);
                    DoMySql.ExecuteSqlTran(lSql);
                    new ServiceFactory.LiveRoomLogService().AddLog(ModelDb.p_liveroom_log.c_type_enum.直播间, $"删除了直播间:{p_liveroom.name}");
                    return result;
                }
                public class DtoReqData : ModelDb.p_liveroom
                {
                    public string ids { get; set; }
                }
                #endregion
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
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("id")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.隐藏字段,
                        text = "id",
                        width = "10",
                        minWidth = "10",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "名称",
                        width = "150",
                        minWidth = "150",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("catename")
                    {
                        text = "区域",
                        width = "180",
                        minWidth = "180",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("typename")
                    {
                        text = "直播间类型",
                        width = "100",
                        minWidth = "100",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zbname")
                    {
                        text = "主播",
                        width = "100",
                        minWidth = "100",
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("statusname")
                    {
                        text = "状态",
                        width = "100",
                        minWidth = "100",

                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sort")
                    {
                        text = "排序",
                        width = "50",
                        minWidth = "50",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "180",
                        minWidth = "180",
                    });
                    #endregion
                    #region 3.操作列


                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "分配主播",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/RepartitionZB?operatorType=1",
                            field_paras = "id"
                        }
                    });

                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "编辑",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/RoomPost",
                            field_paras = "id"
                        }
                    });


                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DeletesAction,
                            field_paras = "id"
                        },

                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    public string id { get; set; }

                    public string area_id { get; set; } = "0";
                }
                #endregion
                #region ListData

                /// <summary>
                /// 直播间列表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'  and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                    if (!req["status"].ToNullableString().IsNullOrEmpty()) where += $" AND (status = {req["status"]})"; //状态筛选

                    //中台看自己的,管理端看全部
                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.jder:
                            where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        case ModelEnum.UserTypeEnum.manager:
                            break;
                        default:
                            where += $" and 1!=1";
                            break;
                    }
                    #region  类别筛选
                    var list = new DomainBasic.DataQueryApp().FindAllChildsByParentId<ModelDb.p_liveroom_area>(req["area_id"].ToInt());
                    string ids = "";

                    foreach (var item in list)
                    {
                        ids += item.id + ",";
                    }
                    ids = ids.Substring(0, ids.Length - 1);
                    if (req["area_id"].ToInt() != 0)
                    {
                        if (!req["area_id"].ToNullableString().IsNullOrEmpty()) where += $" AND (area_id in ('{ids}'))";
                    }
                    #endregion
                    var filter = new DoMySql.Filter
                    {
                        where = where + "  order by sort ",
                    };
                    var result = new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_liveroom, ItemDataModel>(filter, reqJson);
                    return result;
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_liveroom
                {
                    public ItemDataModel()
                    {
                    }
                    public ItemDataModel(ModelDb.p_liveroom pmodel)
                    {
                        this.id = pmodel.id;
                        this.tenant_id = pmodel.tenant_id;

                        this.area_id = pmodel.area_id;
                        this.name = pmodel.name;
                        this.status = pmodel.status;


                    }


                    /// <summary>
                    /// 类别名称
                    /// </summary>
                    public string catename
                    {
                        get
                        {
                            string where = " 1=1 ";
                            //中台看自己的,管理端看全部
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.jder:
                                    where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                                    break;
                                case ModelEnum.UserTypeEnum.manager:
                                    break;
                                default:
                                    where += $" and 1!=1";
                                    break;
                            }
                            var data = DoMySql.FindList<ModelDb.p_liveroom_area>(where);
                            var result = new CategoryService(data).GetFullPath(this.area_id.ToInt());
                            return result;
                            // return DoMySql.FindEntityById<ModelDb.p_liveroom_area>(this.area_id).name;
                        }
                    }
                    /// <summary>
                    /// 状态文字描述
                    /// </summary>
                    public string statusname
                    {
                        get
                        {
                            if (this.status == 0) return ModelDb.p_liveroom.status_enum.空闲.ToString();
                            else if (this.status == 1) return ModelDb.p_liveroom.status_enum.占用.ToString();
                            else if (this.status == 2) return ModelDb.p_liveroom.status_enum.预订.ToString();
                            else return this.status.ToString();
                        }
                    }
                    /// <summary>
                    /// 直播间类型名称
                    /// </summary>
                    public string typename
                    {
                        get
                        {
                            var result = new DomainBasic.DictionaryApp().GetList(ModelEnum.DictCategory.直播间类型)
                                            .Where(t => t.d_value == this.liveroom_type_id.ToString()).FirstOrDefault()?.d_key;
                            if (this.iszhibo == 1)
                            {
                                return "(可直播)" + result;
                            }
                            else
                            {
                                return "(不可直播)" + result;
                            }

                        }
                    }

                    /// <summary>
                    /// 主播名称
                    /// </summary>
                    public string zbname
                    {
                        get
                        {

                            var name = "";
                            if (this.zb_user_sn1.IsNullOrEmpty() && this.zb_user_sn2.IsNullOrEmpty())
                            {
                                return name;
                            }
                            if (!string.IsNullOrEmpty(this.zb_user_sn1))
                            {
                                var zb = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(this.zb_user_sn1);
                                name += zb.user_name;
                            }
                            if (!string.IsNullOrEmpty(this.zb_user_sn2))
                            {
                                var zb = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(this.zb_user_sn2);
                                name += "    " + zb.user_name;
                            }
                            return name;
                        }
                    }
                }
                #endregion

            }
            /// <summary>
            /// 新增直播间
            /// </summary>
            public class LiveRoomPost
            {
                public JsonResultAction InsertLiveRoomFix(List<ModelDb.p_liveroom_fix> list)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    lSql.Add($"delete from  p_liveroom_fix where zt_user_sn='{new UserIdentityBag().user_sn}';");
                    foreach (var item in list)
                    {
                        item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        item.zt_user_sn = new UserIdentityBag().user_sn;
                        lSql.Add(item.InsertOrUpdateTran());
                    }
                    //先删除,后插入
                    var res = DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                public List<ModelDb.p_liveroom_fix> GetLiveRoomFix()
                {
                    var result = DoMySql.FindList<ModelDb.p_liveroom_fix>($" zt_user_sn='{new UserIdentityBag().user_sn}'");
                    return result;
                }

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
                    var p_liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {req.id}", exist: false);
                    #region 表单元素 
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "直播间名称",
                        defaultValue = p_liveroom.name,
                        colLength = 12
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("liveroom_type_id")
                    {
                        title = "直播间类型",
                        defaultValue = p_liveroom.liveroom_type_id.ToString(),
                        options = new DomainBasic.DictionaryApp().GetListForKv(ModelEnum.DictCategory.直播间类型),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.Dtree("area_id")
                    {
                        bindData = new DomainBasic.DataQueryApp().GetTree(0, "p_liveroom_area", null, $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id}"),
                        title = "区域",
                        defaultValue = p_liveroom.area_id.ToNullableString(),
                        colLength = 6
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("iszhibo")
                    {
                        title = "是否直播间",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_liveroom.iszhibo.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("sort")
                    {
                        title = "排序号",
                        defaultValue = p_liveroom.sort.ToNullableString(),
                        placeholder = "排序号，越小越靠前",
                        colLength = 12
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 直播间类型id
                    /// </summary>

                    public int liveroom_type_id { get; set; }
                }


                public enum RoomType
                {
                    Type = 23
                }
                #endregion
                #region 新建直播间
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var p_liveroom_post = req.data_json.ToModel<p_liveroom_post>();

                    if (p_liveroom_post.name.IsNullOrEmpty()) throw new WeicodeException("直播间名称不可为空！");
                    if (p_liveroom_post.area_id.IsNullOrEmpty()) throw new WeicodeException("区域必选！");

                    var p_liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {p_liveroom_post.id}", exist: false);
                    p_liveroom.tenant_id = new DomainBasic.TenantApp().GetInfo().id;

                    p_liveroom.area_id = p_liveroom_post.area_id;
                    p_liveroom.name = p_liveroom_post.name;
                    p_liveroom.zt_user_sn = new UserIdentityBag().user_sn;
                    p_liveroom.liveroom_type_id = p_liveroom_post.liveroom_type_id;
                    p_liveroom.sort = p_liveroom_post.sort;
                    p_liveroom.iszhibo = p_liveroom_post.iszhibo;
                    if (p_liveroom.id == 0)
                    {
                        p_liveroom.status = ModelDb.p_liveroom.status_enum.空闲.ToInt().ToSByte();
                        p_liveroom.create_time = DateTime.Now;
                    }
                    p_liveroom.modify_time = DateTime.Now;
                    lSql.Add(p_liveroom.InsertOrUpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);

                    return result;
                }
                public class p_liveroom_post
                {
                    /// <summary>
                    /// id
                    /// </summary>
                    public int id { get; set; }
                    /// <summary>
                    /// 类别id
                    /// </summary>
                    public Nullable<int> area_id { get; set; }
                    /// <summary>
                    /// 类别名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// tree选中的id
                    /// </summary>
                    public string tree_cate_id_select_nodeId { get; set; }
                    /// <summary>
                    /// tree选中的value
                    /// </summary>
                    public string tree_cate_id_select_input { get; set; }
                    /// <summary>
                    /// 排序号，越小越靠前
                    /// </summary>
                    public Nullable<int> sort { get; set; }
                    /// <summary>
                    /// 直播间类型id
                    /// </summary>
                    public int liveroom_type_id { get; set; }


                    public int iszhibo { get; set; }
                }
                #endregion
            }


            public class Category
            {
                public int id { get; set; }
                public int? parent_id { get; set; }
                public string name { get; set; }
            }


            public class CategoryService
            {
                List<ModelDb.p_liveroom_area> _areas;
                public CategoryService(List<ModelDb.p_liveroom_area> areas)
                {
                    _areas = areas;
                }
                public string GetFullPath(int id)
                {
                    var path = new List<string>();
                    GetParentNames(id, path);
                    path.Reverse();
                    string result = string.Join(" > ", path);
                    return result;
                }
                private void GetParentNames(int id, List<string> path)
                {
                    var category = _areas.FirstOrDefault(c => c.id == id);
                    if (category != null)
                    {
                        path.Add(category.name);
                        if (category.parent_id.HasValue)
                        {
                            GetParentNames(category.parent_id.Value, path);
                        }
                    }
                }
            }
            /// <summary>
            /// 主播分配
            /// </summary>
            public class RepartitionZB
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
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {req.id}", exist: false);
                    string user_sn1 = string.Empty;
                    string user_sn2 = string.Empty;

                    string oldSn = !string.IsNullOrEmpty(liveroom.zb_user_sn1) ? liveroom.zb_user_sn1 : liveroom.zb_user_sn2;
                    #region 表单元素 
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("operatorType")
                    {
                        title = "operatorType",
                        defaultValue = req.operatorType.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("oldSn")
                    {
                        title = "oldSn",
                        defaultValue = oldSn
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "直播间名称",
                        defaultValue = liveroom.name,
                        disabled = false,
                        colLength = 12
                    });
                    string zb_user_sn = req.type == 1 ? "zb_user_sn1" : "zb_user_sn2";
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        title = "运营账号",
                        placeholder = "运营账号",
                        disabled = true,
                        options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter()
                        {
                            attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType()
                            {
                                userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn,
                            }
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

                    formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("tg_user_sn")
                    {
                        title = "直播厅",
                        placeholder = "直播厅",
                        disabled = true,
                        options = new List<ModelDoBasic.Option>(),
                        // options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                                func = GetZhubo,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js($"{zb_user_sn}").options(@"JSON.parse(res.data)")}"
                            }
                        }
                    });


                    formDisplay.formItems.Add(new ModelBasic.EmtSelect($"{zb_user_sn}")
                    {
                        title = "主播账号",
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
                    #endregion
                    return formDisplay;
                }

                public class DtoReq
                {
                    /// <summary>
                    ///  id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 使用主播1
                    /// </summary>
                    public string zb_user_sn1 { get; set; }

                    /// <summary>
                    ///  使用主播2
                    /// </summary>
                    public string zb_user_sn2 { get; set; }


                    public int type { get; set; }


                    /// <summary>
                    /// 1 分配主播,2 转移主播
                    /// </summary>
                    public int operatorType { get; set; }

                    /// <summary>
                    /// 待转移主播sn
                    /// </summary>
                    public string oldSn { get; set; }

                }
                #endregion
                #region 直播间分配/转移主播
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var p_liveroom_post = req.data_json.ToModel<p_liveroom_post>();
                    //使用者/主播sn
                    var user_sn = !string.IsNullOrEmpty(p_liveroom_post.zb_user_sn1) ? p_liveroom_post.zb_user_sn1 : p_liveroom_post.zb_user_sn2;
                    var p_liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {p_liveroom_post.id}", exist: false);
                    p_liveroom.zb_user_sn1 = p_liveroom_post.zb_user_sn1;
                    p_liveroom.zb_user_sn2 = p_liveroom_post.zb_user_sn2;
                    p_liveroom.status = ModelDb.p_liveroom.status_enum.占用.ToSByte();
                    lSql.Add(p_liveroom.InsertOrUpdateTran());
                    //分配主播
                    if (p_liveroom_post.operatorType == 1)
                    {
                        var p_liveroom_usage = new ModelDb.p_live_room_usage()
                        {
                            begin_time = DateTime.Now,
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            zt_user_sn = new UserIdentityBag().user_sn,
                            operation_type = LiveRoomOperatorType.分配.ToInt(),
                            //主播sn
                            user_sn = user_sn,
                            live_room_id = p_liveroom_post.id
                        };
                        //插入一条数据
                        lSql.Add(p_liveroom_usage.InsertOrUpdateTran());
                        var zbinfo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_sn);
                        new ServiceFactory.LiveRoomLogService().AddLog(ModelDb.p_liveroom_log.c_type_enum.主播, $"给直播间:{p_liveroom.name} 分配了主播:{zbinfo.username}");
                    }
                    else
                    {
                        //转移主播 解除旧的，增加新的
                        var p_liveroom_usageOld = DoMySql.FindEntity<ModelDb.p_live_room_usage>($" live_room_id = '{p_liveroom_post.id}' and user_sn='{p_liveroom_post.oldSn}' and operation_type={LiveRoomOperatorType.分配.ToInt()} order by create_time desc  ", exist: false);
                        p_liveroom_usageOld.operation_type = LiveRoomOperatorType.解除.ToInt();
                        p_liveroom_usageOld.end_time = DateTime.Now;
                        lSql.Add(p_liveroom_usageOld.InsertOrUpdateTran());
                        var p_liveroom_usageNew = new ModelDb.p_live_room_usage()
                        {
                            begin_time = DateTime.Now,
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            zt_user_sn = new UserIdentityBag().user_sn,
                            operation_type = LiveRoomOperatorType.分配.ToInt(),
                            //主播sn
                            user_sn = user_sn,
                            live_room_id = p_liveroom_post.id
                        };
                        //插入一条数据
                        lSql.Add(p_liveroom_usageNew.InsertOrUpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                /// <summary>
                /// 删除主播
                /// </summary>
                /// <param name="id"></param>
                /// <param name="type"></param>
                /// <returns></returns>
                public JsonResultAction DeleteZbAction(int id, int type)
                {

                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var p_liveroom = DoMySql.FindEntity<ModelDb.p_liveroom>($"id = {id}", exist: false);
                    var user_sn = p_liveroom.zb_user_sn1 != "" ? p_liveroom.zb_user_sn1 : p_liveroom.zb_user_sn2;
                    if (type == 1)
                        p_liveroom.zb_user_sn1 = "";
                    else
                        p_liveroom.zb_user_sn2 = "";
                    if (p_liveroom.zb_user_sn1 == "" && p_liveroom.zb_user_sn2 == "")
                        p_liveroom.status = ModelDb.p_liveroom.status_enum.空闲.ToSByte();
                    lSql.Add(p_liveroom.InsertOrUpdateTran());
                    //删除主播 将主播的结束时间修改为当前时间
                    var p_liveroom_usage = DoMySql.FindEntity<ModelDb.p_live_room_usage>($" live_room_id = '{id}' and user_sn='{user_sn}' order by create_time desc  ", exist: false);
                    p_liveroom_usage.operation_type = LiveRoomOperatorType.解除.ToInt();
                    p_liveroom_usage.end_time = DateTime.Now;
                    lSql.Add(p_liveroom_usage.InsertOrUpdateTran());
                    var res = DoMySql.ExecuteSqlTran(lSql);
                    var zbinfo = new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_sn);
                    new ServiceFactory.LiveRoomLogService().AddLog(ModelDb.p_liveroom_log.c_type_enum.主播, $"解绑了直播间:{p_liveroom.name} 的主播:{zbinfo.username}");
                    return result;
                }

                /// <summary>
                /// 直播间操作类型
                /// </summary>
                public enum LiveRoomOperatorType
                {
                    分配 = 1,
                    解除 = 2,
                    转移 = 3
                }
                public class p_liveroom_post
                {
                    /// <summary>
                    /// id
                    /// </summary>
                    public int id { get; set; }


                    /// <summary>
                    /// 使用主播1
                    /// </summary>
                    public string zb_user_sn1 { get; set; }

                    /// <summary>
                    ///  使用主播2
                    /// </summary>
                    public string zb_user_sn2 { get; set; }

                    /// <summary>
                    /// 操作类型
                    /// </summary>
                    public int operatorType { get; set; }


                    public string oldSn { get; set; }
                }
                #endregion
            }


            public class RoomUsAge : ModelDb.p_live_room_usage
            {
                public string roomName { get; set; }

                public string zbName { get; set; }

                public string tingName { get; set; }

                public string yyName { get; set; }


                public int useCount { get; set; } = 1;
            }
        }



    }
}

