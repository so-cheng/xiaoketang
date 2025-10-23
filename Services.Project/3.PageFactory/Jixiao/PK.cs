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
    /// 绩效目标
    /// </summary>
    public partial class PageFactory
    {

        #region 主播PK
        /// <summary>
        /// 设定主播目标
        /// </summary>
        public class ZbPKPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("");
                pageModel.actionType = ModelBasic.PagePost.ActionType.Form;
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.actionUrl = "Index";
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
                 * buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "月目标记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/ZbManage/target/List",
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

                formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    title = "直播厅",
                    options=new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "tg_user_sn","<%=page.tg_user_sn.value%>"}
                            },
                            func = GetZhubo,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("zb_user_sn").options(@"JSON.parse(res.data)")};"
                        }
                    },
                    colLength = 6
                });
                
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("zb_user_sn") 
                {
                    title = "主播",
                    options=new Dictionary<string, string>(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "zb_user_sn","<%=page.zb_user_sn.value%>"}
                            },
                            func = GetZbInfo,
                            resCallJs = $"page.l_zbs.addRow(JSON.parse(res.data));"
                        }
                    },
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTableDataEdit("l_zbs")
                {
                    title = "列表",
                    limit=20,
                    height="600px",
                    width="800px",
                    colItems = new List<ModelBasic.EmtTableDataEdit.ColItem>
                    {
                        new ModelBasic.EmtTableDataEdit.ColItem("id")
                        {
                         title = "id",
                         hide=true,
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("name")
                        {
                         title = "主播",
                        },
                    },
                    displayStatus = EmtModelBase.DisplayStatus.编辑,
                    defaultValue = DoMySql.FindObjects<ModelDb.user_base>(new DoMySql.Filter
                    {
                        fields = "id,username",
                        where = $"1=0 and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and status='{ModelDb.user_base.status_enum.正常.ToInt()}'",
                    }).ToJson(),
                });
                var value = new ModelBasic.EmtTableDataEdit.Js("l_zbs");
                
                return formDisplay;
            }
            #endregion
            /// <summary>
            /// 获取厅管名下主播
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetZhubo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var option = new Dictionary<string, string>();
                option.Add("全部主播",req["tg_user_sn"].ToNullableString());
                foreach (var item in DoMySql.FindList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["tg_user_sn"].ToNullableString())}"))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            public JsonResultAction GetZbInfo(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                string user_sn = req["zb_user_sn"].ToNullableString();
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}'", false);
                if(user_base.user_type_id==new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                {
                    result.data = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn).ToJson();
                }
                else
                {
                    result.data = user_base.ToJson();
                }
                return result;
            }



            public class user_base : ModelDb.user_base
            {
                public string zb_user_sn { get; set; }
                public string amount { get; set; }
                public string new_num { get; set; }
                public string amount_2 { get; set; }
                public string num_2 { get; set; }
            }

            public class DtoReq
            {
                /// <summary>
                /// 附加额外参数
                /// </summary>
                public FormData formData { get; set; } = new FormData();
                public class FormData
                {
                    public int id { get; set; }
                }
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
                var result = new JsonResultAction();
                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                /// <summary>
                /// 目标数据集合
                /// </summary>
                public List<l_zbs> l_zbs { get; set; }
            }

            public class l_zbs : ModelDb.user_base
            {
                /// <summary>
                /// 主播名字
                /// </summary>
                public string name { get; set; }
            }
            #endregion
        }

        #endregion
    }
}
