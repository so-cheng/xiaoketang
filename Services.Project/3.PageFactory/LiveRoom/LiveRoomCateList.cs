using System;
using System.Collections.Generic;
using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;
using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using System.Linq;
namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class LiveRoom
        {
            /// <summary>
            /// 类别管理
            /// </summary>
            public class LiveRoomCateList
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
                        text = "添加区域",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/AreaCatePost",
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
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sort")
                    {
                        text = "排序号",
                        width = "100",
                        minWidth = "100",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "200",
                        minWidth = "200",
                    });
                    #endregion

                    #region 3.操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "编辑",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/LiveRoom/Config/AreaCatePost",
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
                }
                #endregion
                #region ListData

                /// <summary>
                /// 直播间类别查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by sort asc",
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_liveroom_area, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_liveroom_area
                {
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
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var shuye_c_qr_cate = new ModelDb.p_liveroom_area();
                    //拼接id
                    List<string> idList = new List<string>();
                    if (!dtoReqData.ids.IsNullOrEmpty()) idList.AddRange(dtoReqData.ids.Split(',').ToList<string>());
                    if (!dtoReqData.id.IsNullOrEmpty()) idList.Add(dtoReqData.id.ToNullableString());
                    string ids = string.Join(",", idList);
                    //删除shuye_c_qr_cate
                    lSql.Add(shuye_c_qr_cate.DeleteTran($"id in ({ids})"));
                    var p_liveroom_area = DoMySql.FindEntity<ModelDb.p_liveroom_area>($"id = {dtoReqData.id}", exist: false);
                    DoMySql.ExecuteSqlTran(lSql);
                    new ServiceFactory.LiveRoomLogService().AddLog(ModelDb.p_liveroom_log.c_type_enum.直播间区域, $"删除了区域:{p_liveroom_area.name}");
                    return result;
                }
                public class DtoReqData : ModelDb.p_liveroom_area
                {
                    public string ids { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 新增类别
            /// </summary>
            public class LiveRoomCatePost
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
                    var shuye_c_qr_cate = DoMySql.FindEntity<ModelDb.p_liveroom_area>($"id = {req.id}", exist: false);

                    #region 表单元素 
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        title = "id",
                        defaultValue = req.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "类别名称",
                        defaultValue = shuye_c_qr_cate.name,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.Dtree("parentid")
                    {
                        bindData = new DomainBasic.DataQueryApp().GetTree(0, "p_liveroom_area", null, $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id}"),
                        title = "类别",
                        defaultValue = shuye_c_qr_cate.parent_id.ToNullableString(),
                        colLength = 6
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("sort")
                    {
                        title = "排序号",
                        defaultValue = shuye_c_qr_cate.sort.ToNullableString(),
                        placeholder = "排序号，越小越靠前",
                        colLength = 12
                    });

                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// 类别id
                    /// </summary>
                    public int id { get; set; }
                }
                #endregion
                #region 新建类别
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var shuye_c_qr_cate_post = req.data_json.ToModel<shuye_c_qr_cate_post>();
                    if (shuye_c_qr_cate_post.name.IsNullOrEmpty()) throw new WeicodeException("名称不可为空！");
                    var p_liveroom_area = DoMySql.FindEntity<ModelDb.p_liveroom_area>($"id = {shuye_c_qr_cate_post.id}", exist: false);
                    p_liveroom_area.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    if (!shuye_c_qr_cate_post.tree_parentid_select_nodeId.IsNullOrEmpty()) p_liveroom_area.parent_id = int.Parse(shuye_c_qr_cate_post.tree_parentid_select_nodeId);
                    else p_liveroom_area.parent_id = 0;
                    p_liveroom_area.name = shuye_c_qr_cate_post.name;
                    p_liveroom_area.zt_user_sn = new UserIdentityBag().user_sn;
                    p_liveroom_area.sort = shuye_c_qr_cate_post.sort;
                    //{"id":"0","name":"aaaa","tree_parentid_select_nodeId":"2","tree_parentid_select_input":"住院","parentid":"","sort":"1"}
                    lSql.Add(p_liveroom_area.InsertOrUpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    new ServiceFactory.LiveRoomLogService().AddLog(ModelDb.p_liveroom_log.c_type_enum.直播间区域, $"增加了区域:{p_liveroom_area.name}");
                    return result;
                }
                public class shuye_c_qr_cate_post
                {
                    /// <summary>
                    /// 类别id
                    /// </summary>
                    public int id { get; set; }
                    /// <summary>
                    /// 类别名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// tree选中的id
                    /// </summary>
                    public string tree_parentid_select_nodeId { get; set; }
                    /// <summary>
                    /// tree选中的value
                    /// </summary>
                    public string tree_parentid_select_input { get; set; }
                    /// <summary>
                    /// 排序号，越小越靠前
                    /// </summary>
                    public Nullable<int> sort { get; set; }
                }
                #endregion
            }



        }
    }
}
