using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Utility;
using WeiCode.Domain;
using System.Linq;
using WeiCode.Services;
using System.Data;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        /// <summary>
        /// 签约名单列表
        /// </summary>
        public class List
        {
            public ModelBasic.PageList Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PageList("PageList");
                pageModel.listFilter = GetListFilter(req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.listDisplay = GetListDisplay(req);

                return pageModel;
            }

            public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new ModelBasic.CtlListFilter();
                listFilter.formItems.Add(new ModelBasic.EmtInput("dou_account")
                {
                    width = "120px",
                    placeholder = "抖音号",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("wx_account")
                {
                    width = "120px",
                    placeholder = "微信名",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("real_name")
                {
                    width = "120px",
                    placeholder = "姓名",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("mobile_last_four")
                {
                    width = "120px",
                    placeholder = "手机号",
                });
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("qy_date_range")
                {
                    placeholder = "签约日期范围",
                    mold = EmtTimeSelect.Mold.date_range,
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
                listDisplay.operateWidth = "70";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("qy_date")
                {
                    text = "签约日期",
                    dateFormat = "yyyy-MM-dd",
                    width = "110",
                    minWidth = "110"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("jjr_name")
                {
                    text = "经纪人",
                    width = "110",
                    minWidth = "110"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("wx_account")
                {
                    text = "微信名",
                    width = "150",
                    minWidth = "150"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_account")
                {
                    text = "抖音号",
                    width = "150",
                    minWidth = "150"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("dou_nickname")
                {
                    text = "抖音昵称",
                    width = "150",
                    minWidth = "150"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                {
                    text = "兼职/全职",
                    width = "100",
                    minWidth = "100"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions_name")
                {
                    text = "时间档",
                    width = "220",
                    minWidth = "220"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sex")
                {
                    text = "性别",
                    width = "60",
                    minWidth = "60"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("is_hudong_name")
                {
                    text = "互动",
                    width = "60",
                    minWidth = "60"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("real_name")
                {
                    text = "姓名",
                    width = "80",
                    minWidth = "80"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile_last_four")
                {
                    text = "手机号后四位",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("term")
                {
                    text = "培训群",
                    width = "100",
                    minWidth = "100"
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
                #endregion
                return listDisplay;
            }

            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                string where = $"qy_user_sn = '{new UserIdentityBag().user_sn}'";

                var req = reqJson.GetPara();
                if (!req["dou_account"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and dou_account like '%{req["dou_account"]}%'";
                }
                if (!req["wx_account"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and wx_account like '%{req["wx_account"]}%'";
                }
                if (!req["real_name"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and real_name like '%{req["real_name"]}%'";
                }
                if (!req["mobile_last_four"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and mobile_last_four = '{req["mobile_last_four"]}'";
                }
                if (!reqJson.GetPara("qy_date_range").ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(reqJson.GetPara("qy_date_range").ToNullableString(), 0);
                    where += $" and qy_date >= '{dateRange.date_range_s}' and qy_date <= '{dateRange.date_range_e.ToDate().AddDays(1).AddSeconds(-1)}'";
                }

                var filter = new DoMySql.Filter
                {
                    where = where,
                    orderby = "order by qy_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_qianyue_roster, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {

            }
            public class ItemDataModel : ModelDb.p_qianyue_roster
            {
                /// <summary>
                /// 互动
                /// </summary>
                public string is_hudong_name
                {
                    get
                    {
                        return "互动";
                    }
                }
                public string sessions_name
                {
                    get
                    {
                        var result = this.sessions.Split(',')
                                         .Select(e => new DomainBasic.DictionaryApp().GetKeyFromValue(ModelEnum.DictCategory.档位时段.ToString(), e))
                                         .ToArray();
                        return string.Join(";", result);
                    }
                }
            }
        }

        /// <summary>
        /// 签约名单编辑
        /// </summary>
        public class Post
        {
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                {
                    returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                };
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = GetButtonGroup(req);

                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                    attachPara = new Dictionary<string, object>
                    {
                        {"id",req.id },
                    }
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
                var p_qianyue_roster = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_qianyue_roster>($"id = {req.id}", false);
                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtInput("qy_date")
                {
                    title = "签约日期",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.qy_date.ToDate().ToString("yyyy-MM-dd"),
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("jjr_name")
                {
                    title = "经纪人",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.jjr_name,
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_account")
                {
                    title = "抖音号",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.dou_account,
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_nickname")
                {
                    title = "抖音昵称",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.dou_nickname,
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("wx_account")
                {
                    title = "微信名",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.wx_account
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("real_name")
                {
                    title = "姓名",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.real_name
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("sex")
                {
                    title = "性别",
                    colLength = 6,
                    options = new Dictionary<string, string>()
                    {
                        {"未知", "未知"},
                        {"男", "男"},
                        {"女", "女"}
                    },
                    defaultValue = p_qianyue_roster.sex
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("full_or_part")
                {
                    title = "兼职/全职",
                    colLength = 6,
                    options = new Dictionary<string, string>()
                    {
                        {"兼职", "兼职"},
                        {"全职", "全职"}
                    },
                    defaultValue = p_qianyue_roster.full_or_part
                });
                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("sessions")
                {
                    title = "接档时间",
                    colLength = 6,
                    bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                    defaultValue = p_qianyue_roster.sessions
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile_last_four")
                {
                    title = "手机号后四位",
                    colLength = 6,
                    defaultValue = p_qianyue_roster.mobile_last_four
                });
                formDisplay.formItems.Add(new EmtSelect("term")
                {
                    title = "培训群",
                    colLength = 6,
                    options = DoMySql.FindKvList<ModelDb.p_mengxin>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} order by id desc", "term,term"),
                    defaultValue = p_qianyue_roster.term
                });

                #endregion
                return formDisplay;
            }

            #region 异步请求处理

            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_qianyue_roster = req.data_json.ToModel<DtoReqData>();

                p_qianyue_roster.ToModel<ModelDb.p_qianyue_roster>().Update($"id = {p_qianyue_roster.id}");
                //更新对象容器数据
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.p_qianyue_roster
            {

            }

            #endregion 

            public class DtoReq : ModelBasic.PagePost.Req
            {
                public int id { get; set; }
            }
        }
    }
}
