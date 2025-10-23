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
    /// 跟进管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 跟进提交页面
        /// </summary>
        public class CustomerFollow
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
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("")
                {
                    title = "详情",
                });
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
                crm_base.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                crm_base.type_id = dtoReqData.type_id;
                crm_base.crm_sn = dtoReqData.crm_sn;
                crm_base.next_time = dtoReqData.next_time;
                crm_base.last_time = DateTime.Now;
                if (dtoReqData.grade_id != null)
                {
                    crm_base.grade_id = dtoReqData.grade_id;
                    lSql.Add(new ModelDb.crm_grade_log
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        o_grade_id = dtoReqData.o_grade_id,
                        crm_sn = dtoReqData.crm_sn,
                        content = dtoReqData.grade_content,
                        n_grade_id = dtoReqData.grade_id,
                        user_sn=new UserIdentityBag().user_sn
                    }.InsertTran());
                    crm_base.grade_id = dtoReqData.grade_id;
                }
                lSql.Add(new ModelDb.crm_log
                {
                    tenant_id= new DomainBasic.TenantApp().GetInfo().id,
                    content = dtoReqData.contact_content,
                    crm_sn= dtoReqData.crm_sn,
                    user_sn = new UserIdentityBag().user_sn
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

    }
}