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
    /// 用户管理
    /// </summary>
    public partial class PageFactory
    {

        /// <summary>
        /// 转移用户
        /// </summary>
        public class CustomerMoves
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
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("ids")
                {
                    defaultValue = req.ids.ToNullableString()
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                {
                    title = "转移主播",
                    colLength = 6,
                    autoComplete = new ModelBasic.EmtInput.AutoComplete
                    {
                        funcGetData = GetData,
                    }
                });
                #endregion
                return formDisplay;
            }

            #region 自动获取主播账号全名
            public ModelBasic.EmtInput.AutoComplete.Res GetData(ModelBasic.EmtInput.AutoComplete.Req req)
            {
                var info = new ModelBasic.EmtInput.AutoComplete.Res();
                
                try
                {
                    info.data = DoMySql.FindList<ModelDbBasic.user_base, ItemUser>(new DoMySql.Filter
                    {
                        fields = "username",
                        where = $"username like '%{req.keyword}%' and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("zber").id} and tenant_id ={new DomainBasic.TenantApp().GetInfo().id} and user_sn != '{new UserIdentityBag().user_sn}' and status='{ModelDb.user_base.status_enum.正常.ToInt()}'"
                    });
                }
                catch (Exception ex)
                {
                    info.code = 1;
                }
                return info;
            }
            public class ItemUser : ModelDbBasic.user_base
            {
                public string username { get; set; }
            }
            #endregion

            /// <summary>
            /// 获取厅管筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req["yy_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 获取主播筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetZhubo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new Dictionary<string, string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            public class DtoReq
            { /// <summary>
              /// 附加额外参数
              /// </summary>
                public Enum relation_type { get; set; } = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                public string ids { get; set; }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 转移用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"username='{dtoReqData.username}' and user_type_id = {new DomainBasic.UserTypeApp().GetInfoByCode("zber").id} and tenant_id ={new DomainBasic.TenantApp().GetInfo().id}");
                if(user_base.user_sn==new UserIdentityBag().user_sn|| user_base.status!=0||user_base.tenant_id!=new DomainBasic.TenantApp().GetInfo().id)
                {
                    throw new WeicodeException("目标主播账号无法转移");
                }


                var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn);
                lSql.Add(new ModelDb.p_crm_customer
                {
                    tg_user_sn = tg_user_sn,
                    zb_user_sn = user_base.user_sn,
                }.UpdateTran($"id in ({dtoReqData.ids})"));

                foreach (var item in DoMySql.FindList<ModelDb.p_crm_customer>($"id in ({dtoReqData.ids})"))
                {
                    lSql.Add(new ModelDb.crm_base
                    {
                        user_sn = user_base.user_sn
                    }.UpdateTran($"crm_sn = '{item.crm_sn}'"));
                }
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                public string ids { get; set; }
                /// <summary>
                /// 转移主播用户名
                /// </summary>
                public string username {get;set;}
            }
            #endregion
        }
    }
}