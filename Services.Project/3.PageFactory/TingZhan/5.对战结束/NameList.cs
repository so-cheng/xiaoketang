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
    /// 名单查看
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 厅战战绩已提报名单查看
            /// <summary>
            /// （管理员/运营）厅战战绩已提报名单列表页面
            /// </summary>
            public class MateTargetList
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_1_name")
                    {
                        text = "直播厅",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_2_name")
                    {
                        text = "直播厅",
                        width = "160",
                        minWidth = "160"
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
                    public string yy_user_sn { get; set; }
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
                    string where = $"tingzhan_id = {req["tingzhan_id"].ToInt()} and (score_1 > 0 or score_2 > 0)";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            where += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })})";
                            break;
                    }

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
                    public string ting_1_name
                    {
                        get
                        {
                            if (score_1 == 0) return "";
                            var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1);
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return !ting.yy_user_sn.Equals(new UserIdentityBag().user_sn) ? "" : ting.ting_name;
                            }
                            return ting.ting_name;
                        }
                    }
                    public string ting_2_name
                    {
                        get
                        {
                            if (score_2 == 0) return "";
                            var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2);
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return !ting.yy_user_sn.Equals(new UserIdentityBag().user_sn) ? "" : ting.ting_name;
                            }
                            return ting.ting_name;
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                public class DtoReqData : ModelDb.p_tingzhan_mate
                {
                }
                #endregion
            }
            #endregion

            #region 厅战战绩未提报名单查看
            /// <summary>
            /// （管理员/运营）厅战战绩未提报名单列表页面
            /// </summary>
            public class UnMateTargetList
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
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.isOpenNumbers = false;

                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };


                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name_1")
                    {
                        text = "所属运营1",
                        width = "160",
                        minWidth = "160"
                    });


                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_user_1_name")
                    {
                        text = "直播厅1",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name_2")
                    {
                        text = "所属运营2",
                        width = "160",
                        minWidth = "160"
                    });


                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_user_2_name")
                    {
                        text = "直播厅2",
                        width = "160",
                        minWidth = "160"
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
                    public string yy_user_sn { get; set; }
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
                    string where = $"tingzhan_id = {req["tingzhan_id"].ToInt()} and (score_1 = 0 or score_2 = 0)";

                    switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            where += $@" and (ting_sn1 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })} or ting_sn2 in {new ServiceFactory.UserInfo.Ting().GetBaseInfosForSql(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter {
                                attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                                {
                                    userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.运营,
                                    UserSn = new UserIdentityBag().user_sn,
                                }
                            })})";
                            break;
                    }

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
                    public ModelDb.user_info_tg ting_user_1
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1);
                        }
                    }
                    public string yy_name_1
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting_user_1.yy_user_sn).name;
                        }
                    }
                    public string ting_user_1_name
                    {
                        get
                        {
                            if (score_1 > 0) return "";
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return !ting_user_1.yy_user_sn.Equals(new UserIdentityBag().user_sn) ? "" : ting_user_1.ting_name;
                            }
                            return ting_user_1.ting_name;
                        }
                    }

                    public ModelDb.user_info_tg ting_user_2
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2);
                        }
                    }
                    public string yy_name_2
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting_user_2.yy_user_sn).name;
                        }
                    }

                    public string ting_user_2_name
                    {
                        get
                        {
                            if (score_2 > 0) return "";
                            switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                            {
                                case ModelEnum.UserTypeEnum.yyer:
                                    return !ting_user_2.yy_user_sn.Equals(new UserIdentityBag().user_sn) ? "" : ting_user_2.ting_name;
                            }
                            return ting_user_2.ting_name;
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                public class DtoReqData : ModelDb.p_tingzhan_mate
                {
                }
                #endregion
            }
            #endregion
        }
    }
}