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

    public partial class PageFactory
    {
        /// <summary>
        /// 编辑/添加列表
        /// </summary>
        public class YyTeamPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("PagePost");
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction
                };
                pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                {
                    returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新当前窗口
                };
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = new ModelBasic.EmtButtonGroup("")
                {

                    buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        /*
                        new EmtModel.ButtonItem
                        {
                            text = "添加直播间",
                            url = "/OutData/Daydata/Post",
                            mode = EmtModel.ButtonItem.Mode.页面弹框按钮
                        }
                        */
                    }
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
                var p_course_video_category = DoMySql.FindEntity<ModelDb.p_course_video_category>($"id = '{req.id}'", false);


                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });

                var options = DoMySql.FindKvList<ModelDb.group_yy>($"","group_name,id");

                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and status=0 and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'"))
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_"+item.id)
                    {
                        title=item.username,
                        options= options,
                        defaultValue=item.attach1
                    });
                }

                

                #endregion
                return formDisplay;
            }

            public class ItemUser : ModelDbBasic.user_base
            {
            }

            public class DtoReq
            {
                /// <summary>
                /// 类型id
                /// </summary>
                public string id { get; set; } = "0";
            }
            #endregion

            #region 异步请求处理

            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var info = new JsonResultAction(null);
                var lSql = new List<string>();
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and status=0"))
                {
                    item.attach1 = req.GetPara("yy_"+item.id).ToString();
                    lSql.Add(item.UpdateTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                return info;
            }

            #endregion
        }
    }
}
