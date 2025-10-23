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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            #region 新主播（待开账号）
            /// <summary>
            /// 查看下属的账号数据
            /// </summary>
            public class NewList
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

                    listFilter.formItems.Add(new ModelBasic.EmtInput("user_name")
                    {
                        width = "100px",
                        placeholder = "主播"
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LossList")
                    {
                        title = "LossList",
                        text = "流失名单",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "LossList"
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
                    listDisplay.operateWidth = "150";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sources_name")
                    {
                        text = "来源名称",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_sex")
                    {
                        text = "性别",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                    {
                        text = "兼职/全职",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_username")
                    {
                        text = "微信账号",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_nickname")
                    {
                        text = "微信昵称",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_username")
                    {
                        text = "抖音账号",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                    {
                        text = "抖音昵称",
                        width = "150",
                        minWidth = "150"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "加入时间",
                        width = "180",
                        minWidth = "180"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "所属厅",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_user_name")
                    {
                        text = "所属厅管",
                        width = "100",
                        minWidth = "100"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_name")
                    {
                        text = "所属运营",
                        width = "100",
                        minWidth = "100"
                    });

                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "/UserInfo/Zhubo/Create",
                            field_paras = "user_info_zhubo_id=id,ting_sn"    //ting_sn表示在指定厅下创建账号
                        },
                        text = "开通账号",
                        hideWith = new EmtModel.ListOperateItem.HideWith
                        {
                            field = "status",
                            compareType = EmtModel.ListOperateItem.CompareType.不等于,
                            value = ModelDb.user_info_zhubo.status_enum.待开账号.ToInt().ToString(),
                        },
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        name = "Del",
                        style = "",
                        text = "流失",
                        title = "提示说明",
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
                public class DtoReq : ModelBasic.PageList.Req
                {
                }
                #endregion

                #region ListData
                /// <summary>
                /// data数据
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    string where = $"status = '{ModelDb.user_info_zhubo.status_enum.待开账号.ToInt()}'";

                    //查询条件
                    if (!reqJson.GetPara("user_name").IsNullOrEmpty()) where += $" AND user_name like '%{reqJson.GetPara("user_name")}%'";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_zhubo, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string keyword { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.user_info_zhubo
                {
                    public string tg_user_name
                    {
                        get
                        {
                            if (tg_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
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

                    public string yy_user_name
                    {
                        get
                        {
                            if (yy_user_sn.IsNullOrEmpty())
                            {
                                return "";
                            }
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                }
                #endregion

                #region 异步请求处理
                /// <summary>
                /// 批量删除账号
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.GetPara("id").ToInt());
                    if (zhubo.status != ModelDb.user_info_zhubo.status_enum.待开账号.ToSByte())
                    {
                        throw new Exception("主播状态已更新,禁止操作");
                    }
                    zhubo.status = ModelDb.user_info_zhubo.status_enum.已离职.ToSByte();
                    lSql.Add(zhubo.UpdateTran());

                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn = '{zhubo.user_sn}'", false);
                    if (user_info_zhubo.IsNullOrEmpty())
                    {
                        user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.已离职.ToSByte();
                        lSql.Add(user_info_zhubo.UpdateTran());
                    }

                    lSql.Add(new ServiceFactory.UserInfo.Zhubo().AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.离职,
                        "主播流失",
                        zhubo));


                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.user_base
                {
                    public string id { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class NewPost
            {

                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
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
                    var user_info_zhubo = DoMySql.FindEntityById<ModelDb.user_info_zhubo>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("o_type")
                    {
                        title = "主播类型",
                        options = new Dictionary<string, string>
                        {
                            {"线上",ModelDb.user_info_zhubo.o_type_enum.线上.ToSByte().ToString()},
                            {"线下",ModelDb.user_info_zhubo.o_type_enum.线下.ToSByte().ToString()}
                        },
                        defaultValue = user_info_zhubo.o_type.ToNullableString()
                    });
                    #endregion 表单元素
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
                    var user_info_zhubo = req.data_json.ToModel<ModelDb.user_info_zhubo>();
                    user_info_zhubo.Update();
                    return result;
                }

                #endregion

            }
            #endregion
        }
    }
}
