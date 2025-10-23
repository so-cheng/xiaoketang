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
    /// 绩效目标
    /// </summary>
    public partial class PageFactory
    {

        #region 

        public class ZhenduanList
        {

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

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    placeholder = "选择月份",
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
                /*
                 buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "Post",
                    }
                });
                 */

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
                listDisplay.operateWidth = "220";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "绩效发生日期",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "音浪(火力)",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("live_hour")
                {
                    text = "有效开播时长(小时)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("exposure_num")
                {
                    text = "曝光人数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("exposure_times")
                {
                    text = "曝光次数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("join_num")
                {
                    text = "进直播间人数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("join_times")
                {
                    text = "进直播间次数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("join_rate")
                {
                    text = "进直播间转化率",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("watch_minutes")
                {
                    text = "人均观看时长(分钟)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("gift_num")
                {
                    text = "打赏人数",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("gift_times")
                {
                    text = "打赏次数",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "新增粉丝",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ACU")
                {
                    text = "ACU",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("game_bill")
                {
                    text = "直播-游戏流水(分成前)(元)",
                    width = "120",
                    minWidth = "120"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("game_gain")
                {
                    text = "直播-主播游戏收入(分成后)(元)",
                    width = "120",
                    minWidth = "120"
                });

                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        field_paras = "id",
                        url = "Edit"
                    },
                    text = "编辑",
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    text = "删除",
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 删除绩效目标
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var info = new JsonResultAction();
                var lSql = new List<string>();
                var p_zhenduan_day = DoMySql.FindEntity<ModelDb.p_zhenduan_day>($"id='{req.GetPara()["id"].ToNullableString()}'");
                lSql.Add(p_zhenduan_day.DeleteTran($"id='{p_zhenduan_day.id}'"));
                MysqlHelper.ExecuteSqlTran(lSql);
                return info;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
            }

            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";

                if (!req["c_date"].ToNullableString().IsNullOrEmpty()) where += $" AND (c_date >='{req["c_date"]}' and c_date <'{req["c_date"].ToDate().AddMonths(1)}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by create_time desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_zhenduan_day, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string create_time { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_zhenduan_day
            {

                public string c_date_text
                {
                    get
                    {
                        return this.live_start.ToDate().ToString("yyyy-MM-dd");
                    }
                }
            }
            #endregion
        }




        /// <summary>
        /// 
        /// </summary>
        public class ZhenduanPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "创建记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/jixiao/jiezou/List",
                    }
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
                var jiezou = DoMySql.FindEntity<ModelDb.jiezou>($"id='{req.id}'", false);
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = jiezou.id.ToString()
                });
                var colTtem = new List<EmtExcelRead.ColItem>();
                colTtem.Add(new EmtExcelRead.ColItem("room_title")
                {
                    title = "直播间标题",
                });
                colTtem.Add(new EmtExcelRead.ColItem("room_title")
                {
                    title = "直播开始时间",
                });
                colTtem.Add(new EmtExcelRead.ColItem("room_title")
                {
                    title = "直播结束时间",
                });
                colTtem.Add(new EmtExcelRead.ColItem("live_hour")
                {
                    title = "直播时长",
                });
                colTtem.Add(new EmtExcelRead.ColItem("exposure_num")
                {
                    title = "曝光人数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("exposure_times")
                {
                    title = "曝光次数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("join_num")
                {
                    title = "进直播间人数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("join_times")
                {
                    title = "进直播间次数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("gift_num")
                {
                    title = "打赏人数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("gift_times")
                {
                    title = "打赏次数",
                });
                colTtem.Add(new EmtExcelRead.ColItem("watch_minutes")
                {
                    title = "人均观看时长",
                });
                colTtem.Add(new EmtExcelRead.ColItem("amount")
                {
                    title = "音浪(火力)",
                });
                colTtem.Add(new EmtExcelRead.ColItem("join_rate")
                {
                    title = "进直播间转化率",
                });
                
                
                colTtem.Add(new EmtExcelRead.ColItem("new_num")
                {
                    title = "新增粉丝",
                });
                colTtem.Add(new EmtExcelRead.ColItem("ACU")
                {
                    title = "ACU",
                });
                colTtem.Add(new EmtExcelRead.ColItem("game_bill")
                {
                    title = "直播-游戏流水(分成前)(元)",
                });
                colTtem.Add(new EmtExcelRead.ColItem("game_gain")
                {
                    title = "直播-主播游戏收入(分成后)(元)",
                });

                formDisplay.formItems.Add(new EmtExcelRead("l_excel")
                {
                    title = "选择excel表",
                    colItems = colTtem,
                    placeholder="",
                    displayStatus = EmtModelBase.DisplayStatus.只读
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public int id { get; set; }
            }
            #region 异步请求处理
            /// <summary>
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                foreach (var item in dtoReqData.l_excel)
                {
                    item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    item.tg_user_sn = new UserIdentityBag().user_sn;
                    lSql.Add(item.InsertOrUpdateTran($"tg_user_sn='{item.tg_user_sn}' and live_start='{item.live_start}'"));
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.p_zhenduan_day
            {
                public List<ModelDb.p_zhenduan_day> l_excel { get; set; }
            }

            #endregion
        }




        public class ZhenduanEdit
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "创建记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/jixiao/jiezou/List",
                    }
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
                var p_zhenduan_day = DoMySql.FindEntity<ModelDb.p_zhenduan_day>($"id='{req.id}'", false);
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = p_zhenduan_day.id.ToString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount")
                {
                    title = "音浪(火力)",
                    defaultValue = p_zhenduan_day.amount.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("live_hour")
                {
                    title = "有效开播时长(小时)",
                    defaultValue = p_zhenduan_day.live_hour.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("exposure_num")
                {
                    title = "曝光人数",
                    defaultValue = p_zhenduan_day.exposure_num.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("exposure_times")
                {
                    title = "曝光次数",
                    defaultValue = p_zhenduan_day.exposure_times.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("join_num")
                {
                    title = "进直播间人数",
                    defaultValue = p_zhenduan_day.join_num.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("join_times")
                {
                    title = "进直播间次数",
                    defaultValue = p_zhenduan_day.join_times.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("watch_minutes")
                {
                    title = "人均观看时长(分钟)",
                    defaultValue = p_zhenduan_day.watch_minutes.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("gift_num")
                {
                    title = "打赏人数",
                    defaultValue = p_zhenduan_day.gift_num.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("gift_times")
                {
                    title = "打赏次数",
                    defaultValue = p_zhenduan_day.gift_times.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("new_num")
                {
                    title = "新增粉丝",
                    defaultValue = p_zhenduan_day.new_fans_num.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("ACU")
                {
                    title = "ACU",
                    defaultValue = p_zhenduan_day.ACU.ToString(),
                    colLength = 6
                });

                #endregion
                return formDisplay;
            }

            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public int id { get; set; }
            }
            #region 异步请求处理
            /// <summary>
            /// 导入多个资产
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var p_zhenduan_day = req.data_json.ToModel<ModelDb.p_zhenduan_day>();
                lSql.Add(p_zhenduan_day.UpdateTran());
                MysqlHelper.ExecuteSqlTran(lSql);
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData : ModelDb.p_zhenduan_day
            {
                public List<ModelDb.p_zhenduan_day> l_excel { get; set; }
            }

            #endregion
        }
        #endregion

    }
}
