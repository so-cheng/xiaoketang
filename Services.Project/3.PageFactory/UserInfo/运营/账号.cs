using System;
using System.Collections.Generic;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class Yy_AccountEdit
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = EditAction,
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
                    var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                    #region 表单元素

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtImageSelect("attach1")
                    {
                        title = "头像",
                        defaultValue = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(user_base.user_sn).img_url,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "昵称",
                        isRequired = true,
                        defaultValue = user_base.name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                    {
                        title = "手机号",
                        isRequired = true,
                        defaultValue = user_base.mobile
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("password")
                    {
                        title = "设置密码",
                        placeholder = "不填代表不修改密码"
                    });

                    #endregion 表单元素
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public string user_sn { get; set; }
                    public string relation_type { get; set; } = "";
                    public string user_type { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction EditAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var user_base = req.data_json.ToModel<ServiceFactory.UserService.user_base>();
                    var yyInfo = new ServiceFactory.UserInfo.Yy().GetInfoById(user_base.id);
                    if (user_base.name.IsNullOrEmpty())
                    {
                        user_base.name = user_base.username;
                    }
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");

                    user_base.Update();

                    result.data = new
                    {
                        user_sn = user_base.user_sn
                    };
                    return result;
                }
                #endregion
            }
        }
    }
}
