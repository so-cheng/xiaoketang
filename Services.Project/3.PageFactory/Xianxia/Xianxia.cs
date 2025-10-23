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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {

        #region Boss
        /// <summary>
        /// 
        /// </summary>
        public class XianxiaList
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
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "提交日期",
                    //defaultValue = req.c_date
                });

                //var option = new Dictionary<string,string>();
                //foreach (var item in DoMySql.FindList<ModelDb.xuexi_category>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status=0"))
                //{
                //    option.Add(item.name,item.id.ToString());
                //}
                //listFilter.formItems.Add(new ModelBasic.EmtSelect("category_id")
                //{
                //    placeholder = "类型",
                //    options=option,
                //});
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
                        url = $"Post",
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
                listDisplay.operateWidth = "120";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "日期",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wx_sn_text")
                {
                    text = "外宣联系人",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wokanguo")
                {
                    text = "我看过",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("kanguowo")
                {
                    text = "看过我",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("greet")
                {
                    text = "打招呼",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_greet")
                {
                    text = "牛人新招呼",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("communicate")
                {
                    text = "我沟通",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("exchange")
                {
                    text = "交换电话微信",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_num")
                {
                    text = "已添加微信数",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_num_rate")
                {
                    text = "微信添加率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_appoint")
                {
                    text = "预约面试",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_appoint_rate")
                {
                    text = "预约面试率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview")
                {
                    text = "已面试",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_rate")
                {
                    text = "面试率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("training")
                {
                    text = "培训",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue")
                {
                    text = "签约",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_male")
                {
                    text = "入男厅",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_female")
                {
                    text = "入女厅",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_rate")
                {
                    text = "签约率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("leave_rate")
                {
                    text = "流失率",
                    width = "90",
                    minWidth = "80"
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
                    name = "Post"
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
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
                /// <summary>
                /// 
                /// </summary>
                public FilterForm filterForm { get; set; } = new FilterForm();

                /// <summary>
                /// 筛选参数（自定义）
                /// </summary>
                public class FilterForm
                {
                    /// <summary>
                    /// 关键词
                    /// </summary>
                    public string keyword { get; set; }
                    public string user_sn { get; set; }
                }
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

                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();
                if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange"), 0);
                    where += $" and c_date >= '{dateRange.date_range_s}' and c_date <= '{dateRange.date_range_e}'";
                }
                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_wx_xianxia, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                /// <summary>
                /// 
                /// </summary>
                public string create_time { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_wx_xianxia
            {
                public string c_date_text
                {
                    get
                    {
                        return c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
                public string wx_sn_text
                {
                    get
                    {
                        return new DomainBasic.UserApp().GetInfoByUserSn(this.wx_user_sn).username;
                    }
                }

                public string wechat_num_rate
                {
                    get
                    {
                        return exchange == 0 ? "0" : Math.Round((100 * wechat_num.ToDouble() / exchange).ToDouble(), 2) + "%";
                    }
                }
                public string interview_appoint_rate
                {
                    get
                    {
                        return wechat_num == 0 ? "0" : Math.Round((100 * interview_appoint.ToDouble() / wechat_num).ToDouble(), 2) + "%";
                    }
                }
                public string interview_rate
                {
                    get
                    {
                        return interview_appoint == 0 ? "0" : Math.Round((100 * interview.ToDouble() / interview_appoint).ToDouble(), 2) + "%";
                    }
                }
                public string qianyue_rate
                {
                    get
                    {
                        return interview == 0 ? "0" : Math.Round((100 * qianyue.ToDouble() / interview).ToDouble(), 2) + "%";
                    }
                }
                public string leave_rate
                {
                    get
                    {
                        double rate = Math.Round((1 - (100 * qianyue.ToDouble() / wechat_num)).ToDouble(), 2);
                        if (rate < 0) { rate = 0; }

                        return wechat_num == 0 ? "0" : rate + "%";
                    }
                }
            }
            #endregion

            #region 异步请求处理

            /// <summary>
            /// 批量删除资产
            /// </summary>
            /// <param name="req">回调函数提交统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                var xuexi_base = new ModelDb.p_wx_xianxia();
                xuexi_base.Delete($"id = ({dtoReqData.id})");
                return result;
            }
            public class DtoReqData : ModelDb.p_wx_xianxia
            {
            }
            #endregion
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class XianxiaPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
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
                /*
                 buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("List")
                {
                    title = "List",
                    text = "反馈记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/Service/FeedBack/List",
                    },
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
                var p_wx_xianxia = DoMySql.FindEntityById<ModelDb.p_wx_xianxia>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_wx_xianxia.id.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "日期",
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    defaultValue = p_wx_xianxia.c_date.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("wx_contact")
                {
                    title = "外宣联系人",
                    defaultValue = p_wx_xianxia.wx_user_sn,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("wokanguo")
                {
                    title = "我看过",
                    defaultValue = p_wx_xianxia.wokanguo.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("kanguowo")
                {
                    title = "看过我",
                    defaultValue = p_wx_xianxia.kanguowo.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("greet")
                {
                    title = "打招呼",
                    defaultValue = p_wx_xianxia.greet.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("new_greet")
                {
                    title = "牛人新招呼",
                    defaultValue = p_wx_xianxia.new_greet.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("communicate")
                {
                    title = "我沟通",
                    defaultValue = p_wx_xianxia.communicate.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("exchange")
                {
                    title = "交换电话微信",
                    defaultValue = p_wx_xianxia.exchange.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("wechat_num")
                {
                    title = "已添加微信",
                    defaultValue = p_wx_xianxia.wechat_num.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("interview_appoint")
                {
                    title = "预约面试",
                    defaultValue = p_wx_xianxia.interview_appoint.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("interview")
                {
                    title = "已面试",
                    defaultValue = p_wx_xianxia.interview.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("training")
                {
                    title = "培训",
                    defaultValue = p_wx_xianxia.training.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("qianyue")
                {
                    title = "签约",
                    defaultValue = p_wx_xianxia.qianyue.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_male")
                {
                    title = "入男厅",
                    defaultValue = p_wx_xianxia.ting_male.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_female")
                {
                    title = "入女厅",
                    defaultValue = p_wx_xianxia.ting_female.ToString(),
                    colLength = 6,
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
                var p_wx_xianxia = req.data_json.ToModel<ModelDb.p_wx_xianxia>();
                p_wx_xianxia.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_wx_xianxia.wx_user_sn.IsNullOrEmpty())
                {
                    p_wx_xianxia.wx_user_sn = new UserIdentityBag().user_sn;
                }
                p_wx_xianxia.InsertOrUpdate();

                //更新对象容器数据
                return result;
            }
            #endregion
        }
        #endregion

        #region 抖音
        /// <summary>
        /// 抖音列表
        /// </summary>
        public class DouyinList
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
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange", true)
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "提交日期",
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
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "Post",
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
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "日期",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wx_sn_text")
                {
                    text = "外宣联系人",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("living_session")
                {
                    text = "直播场次",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("living_schedule")
                {
                    text = "直播时间",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("living_visitors")
                {
                    text = "进房人数",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("living_audience")
                {
                    text = "曝光人数",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_followers")
                {
                    text = "新增粉丝数",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wechat_num")
                {
                    text = "已添加微信数",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_appoint")
                {
                    text = "预约面试",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_appoint_rate")
                {
                    text = "预约面试率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview")
                {
                    text = "已面试",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("interview_rate")
                {
                    text = "面试率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("training")
                {
                    text = "培训",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue")
                {
                    text = "签约",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_male")
                {
                    text = "入男厅",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_female")
                {
                    text = "入女厅",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qianyue_rate")
                {
                    text = "签约率",
                    width = "90",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("leave_rate")
                {
                    text = "流失率",
                    width = "90",
                    minWidth = "80"
                });
                #endregion
                #region 2.批量操作列
                listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "批量操作",

                    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量删除",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = DeletesAction,
                             },
                            disabled = true
                        }
                    }
                });
                #endregion
                #region 3.操作列
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "Post",
                        field_paras = "id"
                    },
                    text = "编辑",
                    name = "Post"
                });

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
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
            public class DtoReq
            {
            }
            #endregion
            #region ListData
            /// <summary>
            /// 菜品表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
                if (!reqJson.GetPara("dateRange").IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("dateRange"), 0);
                    where += $" and c_date >= '{dateRange.date_range_s}' and c_date <= '{dateRange.date_range_e}'";
                }
                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_wx_xianxia_douyin, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.ListData.Req
            {
                /// <summary>
                /// 
                /// </summary>
                public string name { get; set; }
                public string id { get; set; }
                public string status { get; set; }
                public string parent_id { get; set; }

            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_wx_xianxia_douyin
            {
                public string c_date_text
                {
                    get
                    {
                        return c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }
                public string wx_sn_text
                {
                    get
                    {
                        return new DomainBasic.UserApp().GetInfoByUserSn(this.wx_user_sn).username;
                    }
                }
               
                public string interview_appoint_rate
                {
                    get
                    {
                        return wechat_num == 0 ? "0" : Math.Round((100 * interview_appoint.ToDouble() / wechat_num).ToDouble(), 2) + "%";
                    }
                }
                public string interview_rate
                {
                    get
                    {
                        return interview_appoint == 0 ? "0" : Math.Round((100 * interview.ToDouble() / interview_appoint).ToDouble(), 2) + "%";
                    }
                }
                public string qianyue_rate
                {
                    get
                    {
                        return interview == 0 ? "0" : Math.Round((100 * qianyue.ToDouble() / interview).ToDouble(), 2) + "%";
                    }
                }
                public string leave_rate
                {
                    get
                    {
                        double rate = Math.Round((1 - (100 * qianyue.ToDouble() / wechat_num)).ToDouble(), 2);
                        if (rate < 0) { rate = 0; }

                        return wechat_num == 0 ? "0" : rate + "%";
                    }
                }
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
                var xuexi_base = new ModelDb.p_wx_xianxia_douyin();
                xuexi_base.Delete($"id = ({dtoReqData.id})");
                return result;
            }
            public class DtoReqData : ModelDb.p_wx_xianxia_douyin
            {
                public string id { get; set; }
            }
            #endregion
        }
        /// <summary>
        /// 新增抖音每日数据
        /// </summary>
        public class DouyinPost
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
                var p_wx_xianxia_douyin = DoMySql.FindEntityById<ModelDb.p_wx_xianxia_douyin>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_wx_xianxia_douyin.id.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "日期",
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    defaultValue = p_wx_xianxia_douyin.c_date.ToDateString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("wx_user_sn")
                {
                    title = "外宣联系人",
                    defaultValue = p_wx_xianxia_douyin.wx_user_sn,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("living_session")
                {
                    title = "直播场次",
                    defaultValue = p_wx_xianxia_douyin.living_session,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("living_schedule")
                {
                    title = "直播时间",
                    defaultValue = p_wx_xianxia_douyin.living_schedule,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("living_visitors")
                {
                    title = "进房人数",
                    defaultValue = p_wx_xianxia_douyin.living_visitors.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("living_audience")
                {
                    title = "曝光人数",
                    defaultValue = p_wx_xianxia_douyin.living_audience.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("new_followers")
                {
                    title = "新增粉丝数",
                    defaultValue = p_wx_xianxia_douyin.new_followers.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("wechat_num")
                {
                    title = "已添加微信",
                    defaultValue = p_wx_xianxia_douyin.wechat_num.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("interview_appoint")
                {
                    title = "预约面试",
                    defaultValue = p_wx_xianxia_douyin.interview_appoint.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("interview")
                {
                    title = "已面试",
                    defaultValue = p_wx_xianxia_douyin.interview.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("training")
                {
                    title = "培训",
                    defaultValue = p_wx_xianxia_douyin.training.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("qianyue")
                {
                    title = "签约",
                    defaultValue = p_wx_xianxia_douyin.qianyue.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_male")
                {
                    title = "入男厅",
                    defaultValue = p_wx_xianxia_douyin.ting_male.ToString(),
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_female")
                {
                    title = "入女厅",
                    defaultValue = p_wx_xianxia_douyin.ting_female.ToString(),
                    colLength = 6,
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }
            }
            #endregion
            #region 新建菜品
            /// <summary>
            /// 表单提交处理的回调函数
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_wx_xianxia_douyin = req.data_json.ToModel<ModelDb.p_wx_xianxia_douyin>();
                p_wx_xianxia_douyin.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_wx_xianxia_douyin.wx_user_sn.IsNullOrEmpty())
                {
                    p_wx_xianxia_douyin.wx_user_sn = new UserIdentityBag().user_sn;
                }
                p_wx_xianxia_douyin.InsertOrUpdate();
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.p_wx_xianxia_douyin
            {
            }
            #endregion
        }

        #endregion
    }
}
