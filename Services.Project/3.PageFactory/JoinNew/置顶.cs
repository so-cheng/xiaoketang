using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    /// <summary>
    /// 置顶模块
    /// </summary>
    public partial class PageFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// （超管）运营置顶次数列表
            /// </summary>
            public class PinToTopList
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
                        width = "110px",
                        title = "运营账号",
                        options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv(),
                        placeholder = "运营账号",
                    });
                    return listFilter;
                }
                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }

                /// <summary>
                /// 设置列表显示的元素
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
                {
                    var listDisplay = new ModelBasic.CtlListDisplay();
                    listDisplay.isHideOperate = true;
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData,
                        pageSize = 50
                    };

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                    {
                        text = "运营账号",
                        width = "130",
                        minWidth = "130"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("join_pintotop_times")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "次数",
                        width = "90",
                        minWidth = "90",
                        sort = true,
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                            {
                                {"id", "id" },
                                {"join_pintotop_times", "join_pintotop_times" },
                            },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"join_pintotop_times_text")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("submit_join_pintotop_times")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"name","join_pintotop_times" },
                                                    {"value","<%=page.join_pintotop_times_text.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.join_pintotop_times_text.value);$('.floatlayer_div').hide();",
                                                func= new ServiceFactory.JoinNew().FastEditPinToTopTimes
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_join_pintotop_times")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.floatlayer_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.join_pintotop_times_text.set(page.focus.attr('data-join_pintotop_times'));"
                                }
                            }
                        }
                    });
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
                    string where = $"1=1";

                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" and yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }

                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = "order by id"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_yunying, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.user_info_yunying
                {
                    public string yy_name
                    {
                        get
                        {
                            return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                        }
                    }
                }
                #endregion
            }
        }
    }
}
