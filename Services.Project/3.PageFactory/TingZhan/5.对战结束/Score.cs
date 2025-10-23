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
    /// 战绩
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 战绩列表
            /// <summary>
            /// 
            /// </summary>
            public class MateList
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
                    listDisplay.operateWidth = "360";
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("start_time")
                    {
                        text = "提报开始时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("end_time")
                    {
                        text = "提报结束时间",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_count_text")
                    {
                        text = "总厅数",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("commit_count")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        text = "战绩已提报数",
                        width = "160",
                        minWidth = "160",
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            href = "PostList?id={{d.id}}",
                            title = "战绩已提报名单"
                        }
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("uncommit_count")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.链接地址,
                        text = "战绩未提报数",
                        width = "160",
                        minWidth = "160",
                        modeParaLink = new ModelBasic.EmtModel.ListItem.ModeParaLink
                        {
                            href = "UnPostList?id={{d.id}}",
                            title = "战绩未提报名单"
                        }
                    });
                    #region 操作列按钮

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
                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan, ItemDataModel>(filter, reqJson);
                }

                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.Req
                {

                    public string c_date { get; set; }
                }

                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_tingzhan
                {
                    public int ting_count_text
                    {
                        get
                        {
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_mate>($@"tingzhan_id = {id} and ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                        {
                                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                            UserSn = new UserIdentityBag().user_sn,
                                        }
                                    })}").Count +
                                     DoMySql.FindList<ModelDb.p_tingzhan_mate>($@"tingzhan_id = {id} and ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                     {
                                         attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                         {
                                             userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                             UserSn = new UserIdentityBag().user_sn,
                                         }
                                     })}").Count;
                            }
                            return DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tingzhan_id = {id}").Count * 2;
                        }
                    }
                    public int commit_count
                    {
                        get
                        {
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return DoMySql.FindList<ModelDb.p_tingzhan_mate>($@"tingzhan_id = {id} and ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                    {
                                        attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                        {
                                            userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                            UserSn = new UserIdentityBag().user_sn,
                                        }
                                    })} and score_1 > 0").Count +
                                     DoMySql.FindList<ModelDb.p_tingzhan_mate>($@"tingzhan_id = {id} and ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                                     {
                                         attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                         {
                                             userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                             UserSn = new UserIdentityBag().user_sn,
                                         }
                                     })} and score_2 > 0").Count;
                            }
                            return DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tingzhan_id = {id} and score_1 > 0").Count + DoMySql.FindList<ModelDb.p_tingzhan_mate>($"tingzhan_id = {id} and score_2 > 0").Count;
                        }
                    }
                    public int uncommit_count
                    {
                        get
                        {
                            return ting_count_text - commit_count;
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 战绩提报列表
            /// <summary>
            /// 
            /// </summary>
            public class ScorePostList
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
                    listDisplay.operateWidth = "120";

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                    {
                        text = "直播厅",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_1_text")
                    {
                        text = "第一局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_2_text")
                    {
                        text = "第一局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_1_text")
                    {
                        text = "第二局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_2_text")
                    {
                        text = "第二局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_3_1_text")
                    {
                        text = "第三局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_3_2_text")
                    {
                        text = "第三局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_text")
                    {
                        text = "总分",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_text")
                    {
                        text = "总分",
                        width = "160",
                        minWidth = "160",
                        sort = true
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
                        text = "提报",
                        name = "ScorePost"
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
                    var tingzhang_id = new ServiceFactory.TingZhanService().getNewTingzhan().id;
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {tingzhang_id} and (tg_user_sn1 = '{new UserIdentityBag().user_sn}' or tg_user_sn2 = '{new UserIdentityBag().user_sn}')";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_tingzhan_mate
                {
                    public ModelDb.p_tingzhan p_tingzhan
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_tingzhan>($"id = {tingzhan_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string ting1_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                        }
                    }
                    public string ting2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            if (tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                            {
                                return ting1_name;
                            }
                            else
                            {
                                return ting2_name;
                            }
                        }
                    }
                    public string score_1_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_1_1;
                        }
                    }
                    public string score_1_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_1_2;
                        }
                    }
                    public string score_2_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_2_1;
                        }
                    }
                    public string score_2_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_2_2;
                        }
                    }
                    public string score_3_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_3_1;
                        }
                    }
                    public string score_3_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_3_2;
                        }
                    }
                    public string score_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_1;
                        }
                    }
                    public string score_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_2;
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 战绩提报
            /// <summary>
            /// 战绩提报编辑页面
            /// </summary>
            public class ScorePost
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
                    var formDisplay = pageModel.formDisplay;

                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {req.id}", false);

                    var ting_name_1 = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_tingzhan_mate.ting_sn1).ting_name;
                    var ting_name_2 = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_tingzhan_mate.ting_sn2).ting_name;

                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = DoMySql.FindEntity<ModelDb.p_tingzhan>($"id = {p_tingzhan_mate.tingzhan_id}").c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_1_1")
                    {
                        title = "第1局 " + ting_name_1,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_1_1.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_1_2")
                    {
                        title = "第1局 " + ting_name_2,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_1_2.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_2_1")
                    {
                        title = "第2局 " + ting_name_1,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_2_1.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_2_2")
                    {
                        title = "第2局 " + ting_name_2,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_2_2.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_3_1")
                    {
                        title = "第3局 " + ting_name_1,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_3_1.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("score_3_2")
                    {
                        title = "第3局 " + ting_name_2,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_3_2.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("score_1")
                    {
                        title = "总分 " + ting_name_1,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_1.ToString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("score_2")
                    {
                        title = "总分 " + ting_name_2,
                        placeholder = "万",
                        defaultValue = p_tingzhan_mate.score_2.ToString()
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
                    var reqData = req.data_json.ToModel<ModelDb.p_tingzhan_mate>();
                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {reqData.id}", false);
                    if (!p_tingzhan_mate.IsNullOrEmpty())
                    {
                        p_tingzhan_mate.score_1_1 = reqData.score_1_1;
                        p_tingzhan_mate.score_1_2 = reqData.score_1_2;
                        p_tingzhan_mate.score_2_1 = reqData.score_2_1;
                        p_tingzhan_mate.score_2_2 = reqData.score_2_2;
                        p_tingzhan_mate.score_3_1 = reqData.score_3_1;
                        p_tingzhan_mate.score_3_2 = reqData.score_3_2;
                        p_tingzhan_mate.score_1 = reqData.score_1_1 + reqData.score_2_1 + reqData.score_3_1;
                        p_tingzhan_mate.score_2 = reqData.score_1_2 + reqData.score_2_2 + reqData.score_3_2;

                        p_tingzhan_mate.Update();
                    }

                    //更新对象容器数据
                    return result;
                }
                #endregion
            }
            #endregion

            #region 历史战绩列表
            /// <summary>
            /// 
            /// </summary>
            public class HistoryScoreList
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

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                    {
                        placeholder = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                                func = GetTinGuan,
                                resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};"
                            }
                        },
                        defaultValue = req.yy_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                    {
                        placeholder = "厅管账号",
                        options = new ServiceFactory.RelationService().GetTreeOptionDic(req.yy_user_sn),
                        defaultValue = req.tg_user_sn
                    });

                    listFilter.formItems.Add(new ModelBasic.EmtHidden("tingzhan_id")
                    {
                        defaultValue = req.id.ToString()
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
                    result.data = new ServiceFactory.RelationService().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
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
                    listDisplay.operateWidth = "120";

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                    {
                        text = "厅战时间",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting1_name")
                    {
                        text = "直播厅A",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting2_name")
                    {
                        text = "直播厅B",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_1_text")
                    {
                        text = "第一局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_2_text")
                    {
                        text = "第一局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_1_text")
                    {
                        text = "第二局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_2_text")
                    {
                        text = "第二局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_3_1_text")
                    {
                        text = "第三局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_3_2_text")
                    {
                        text = "第三局",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_1_text")
                    {
                        text = "总分",
                        width = "160",
                        minWidth = "160",
                        sort = true
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("score_2_text")
                    {
                        text = "总分",
                        width = "160",
                        minWidth = "160",
                        sort = true
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
                        name = "ScorePost"
                    });
                    #endregion
                    return listDisplay;
                }

                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq : ModelBasic.PageList.Req
                {
                    public string yy_user_sn { get; set; }
                    public string tg_user_sn { get; set; }
                    public int id { get; set; }
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
                    string where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {req["tingzhan_id"].ToInt()}";

                    if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" and (ting_sn1 in (select ting_sn from p_tingzhan_target where yy_user_sn = '{req["yy_user_sn"]}') or ting_sn2 in (select ting_sn from p_tingzhan_target where yy_user_sn = '{req["yy_user_sn"]}'))";

                    //查询条件
                    if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (tg_user_sn1 = '{req["tg_user_sn"]}' or tg_user_sn2 = '{req["tg_user_sn"]}')";

                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc "
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_tingzhan_mate, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_tingzhan_mate
                {
                    public ModelDb.p_tingzhan p_tingzhan
                    {
                        get
                        {
                            return DoMySql.FindEntity<ModelDb.p_tingzhan>($"id = {tingzhan_id}", false);
                        }
                    }
                    public string c_date_text
                    {
                        get
                        {
                            return p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd");
                        }
                    }
                    public string ting1_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                        }
                    }
                    public string ting2_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                        }
                    }
                    public string ting_name
                    {
                        get
                        {
                            if (tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                            {
                                return ting1_name;
                            }
                            else
                            {
                                return ting2_name;
                            }
                        }
                    }
                    public string score_1_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_1_1;
                        }
                    }
                    public string score_1_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_1_2;
                        }
                    }
                    public string score_2_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_2_1;
                        }
                    }
                    public string score_2_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_2_2;
                        }
                    }
                    public string score_3_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_3_1;
                        }
                    }
                    public string score_3_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_3_2;
                        }
                    }
                    public string score_1_text
                    {
                        get
                        {
                            return ting1_name + " " + score_1;
                        }
                    }
                    public string score_2_text
                    {
                        get
                        {
                            return ting2_name + " " + score_2;
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #endregion
            }
            #endregion
        }
    }
}