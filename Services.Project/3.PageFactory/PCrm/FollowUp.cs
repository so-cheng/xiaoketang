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
    /// 用户等级管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 设置用户等级
        /// </summary>
        public class FollowUpPost
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
                var crm_base = DoMySql.FindEntity<ModelDb.crm_base>($"crm_sn='{req.crm_sn}'", false);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                {
                    title = "基本信息",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("crm_sn")
                {
                    title = "客户编号",
                    defaultValue = req.crm_sn,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("o_grade_id")
                {
                    defaultValue = crm_base.grade_id.ToString(),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("type_id")
                {
                    defaultValue = req.type_id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("grade")
                {
                    title = "当前等级",
                    defaultValue = DoMySql.FindEntity<ModelDb.crm_grade>($"id='{crm_base.grade_id}'", false).name,
                    displayStatus = EmtModelBase.DisplayStatus.只读,
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelectFull("grade_id")
                {
                    options = DoMySql.FindListBySql<ModelDoBasic.Option>($"select id as value,name as text from crm_grade where type_id='{req.type_id}'"),
                    title = "更新等级",
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("contact_content")
                {
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    title = "联系情况备注",
                    colLength = 7
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTextarea("grade_content")
                {
                    mode = ModelBasic.EmtTextarea.Mode.TextArea,
                    title = "等级变更备注",
                    colLength = 7
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("next_time")
                {
                    title = "下次联系时间",
                    colLength = 6
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }
                public int type_id { get; set; }
                public string crm_sn { get; set; }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 设置等级
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var crm_base = new ModelDb.crm_base();
                crm_base.type_id = dtoReqData.type_id;
                crm_base.crm_sn = dtoReqData.crm_sn;
                crm_base.next_time = dtoReqData.next_time;
                crm_base.last_time = DateTime.Now;
                crm_base.grade_id = dtoReqData.o_grade_id;
                crm_base.user_sn = new UserIdentityBag().user_sn;
                if (dtoReqData.grade_id != null)
                {
                    lSql.Add(new ModelDb.crm_grade_log
                    {
                        o_grade_id = dtoReqData.o_grade_id,
                        crm_sn = dtoReqData.crm_sn,
                        user_sn= new UserIdentityBag().user_sn,
                        content = dtoReqData.grade_content,
                        n_grade_id = dtoReqData.grade_id
                    }.InsertTran());
                    crm_base.grade_id = dtoReqData.grade_id;
                }
                lSql.Add(new ModelDb.crm_log
                {
                    content = dtoReqData.contact_content,
                    crm_sn = dtoReqData.crm_sn,
                    user_sn = new UserIdentityBag().user_sn,
                }.InsertTran());
                lSql.Add(crm_base.InsertOrUpdateTran($"crm_sn='{dtoReqData.crm_sn}'"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            public class DtoReqData : ModelDb.crm_base
            {
                public int? o_grade_id { get; set; }
                public string contact_content { get; set; }
                public string grade_content { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 跟进记录列表
        /// </summary>
        public class FollowUpList
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
                pageModel.listFilter.isExport = false;
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
                
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("c_date_range")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "选择日期",
                });
                listFilter.formItems.Add(new ModelBasic.EmtInput("content")
                {
                    placeholder = "联系情况搜索"
                });
                return listFilter;
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
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.isHideOperate = true;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 1.显示列
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zb_name")
                {
                    text = "所属用户",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                {
                    text = "用户昵称",
                    width = "120",
                    minWidth = "120",
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("content")
                {
                    text = "联系情况备注",
                    width = "280",
                    minWidth = "280",
                });
                #endregion
                #region 2.批量操作列
                #endregion
                #region 3.操作列
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
            /// 资产表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = "1=1";

                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  create_time >= '" + dateRange.date_range_s + "' AND create_time <= '" + dateRange.date_range_e + "'";
                }
                if (!req["content"].ToNullableString().IsNullOrEmpty()) where += $" AND (content like '%{req["content"]}%')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.crm_log, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.ListData.Req
            {


            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.crm_log
            {
                public string name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.p_crm_customer>($"crm_sn = '{this.crm_sn}'", false).nick;
                    }
                }
                public string zb_name
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{this.user_sn}'", false).name;
                    }
                }
            }
            #endregion
            #region 异步请求处理

            #endregion
        }
    }
}