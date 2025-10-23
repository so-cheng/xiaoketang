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
    /// 对方信息
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 对方信息列表
            /// <summary>
            /// 
            /// </summary>
            public class UserInfoList
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
                    #region 操作列按钮
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            url = "View",
                            field_paras = "id"
                        },
                        text = "查看",
                        name = "UserInfoView"
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
                    public string ting_name
                    {
                        get
                        {
                            if (tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn1).ting_name;
                            }
                            else
                            {
                                return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn2).ting_name;
                            }
                        }
                    }
                }
                #endregion

                #region 异步请求处理

                #endregion
            }
            #endregion

            #region 对方信息
            /// <summary>
            /// 对方信息页面
            /// </summary>
            public class UserInfoView
            {
                #region DefaultView
                public ModelBasic.PageDetail Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PageDetail("");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);

                    return pageModel;
                }

                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PageDetail pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();
                    var df_ting_sn = ""; //对方厅sn
                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {req.id}");
                    if (p_tingzhan_mate.tg_user_sn1.Equals(new UserIdentityBag().user_sn))
                    {
                        df_ting_sn = p_tingzhan_mate.ting_sn2;
                    }
                    else
                    {
                        df_ting_sn = p_tingzhan_mate.ting_sn1;
                    }
                    var dfTingInfo = !df_ting_sn.IsNullOrEmpty() ? new ServiceFactory.UserInfo.Ting().GetTingBySn(df_ting_sn) : new ServiceFactory.UserInfo.Ting.TingInfo();

                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("weixin")
                    {
                        title = "微信号",
                        defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(dfTingInfo.tg_user_sn).wechat_username
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("dou_user")
                    {
                        title = "抖音号",
                        defaultValue = dfTingInfo.dou_user
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("yy_name")
                    {
                        title = "运营团队",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(dfTingInfo.yy_user_sn).name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("ting_name")
                    {
                        title = "厅名",
                        defaultValue = dfTingInfo.ting_name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("tg_name")
                    {
                        title = "厅管名称",
                        defaultValue = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(dfTingInfo.tg_user_sn).name
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("phone")
                    {
                        title = "手机号",
                        defaultValue = dfTingInfo.phone
                    });

                    #endregion
                    return formDisplay;
                }
                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                }
                #endregion
            }
            #endregion
        }
    }
}
