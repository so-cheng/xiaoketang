using System;
using System.Collections.Generic;
using System.Linq;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Modular;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        #region 更新上级用户关系
        public class UpdateSuperiorUserRelation
        {
            #region DefaultView
            public PagePost Get(DtoReq req)
            {
                #region 设置tab页
                var pageModel = new PagePost("post");
                string htmlTop = @"<div class=""layui-tab"" style=""width:100%;"">
                                        <ul class=""layui-tab-title"">";

                foreach (var item in new DomainUserBasic.UserRelationTypeApp().GetUser_Relation_Types())
                {
                    htmlTop += $@"<li class=""{(req.type_id.Equals(item.id) ? "layui-this" : "")}"" onclick=""location.href='?type_id={item.id}'"">{item.cname}</li>";
                }
                htmlTop += @"</ul>
                        </div>";
                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = htmlTop
                    }
                };
                #endregion
                #region 设置页面模型
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.jsFileNames.Add(PageModelBase.JsFileName.通知消息);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                    attachPara = new Dictionary<string, object>
                    {
                        {"type_id", req.type_id}
                    }
                };
                return pageModel;
                #endregion
            }
            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
            {
                if (req.type_id.IsNullOrEmpty()) throw new WeicodeException("请选择Tab页");
                //1.获取当前tab的所有上级用户
                List<ModelDoBasic.Option> users = new DomainBasic.UserApp().GetUsersByUserTypeIdForOption(new DomainUserBasic.UserRelationTypeApp().GetInfoById(req.type_id).f_user_type_id);

                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                if ((ModelEnum.UserRelationTypeEnum)req.type_id == ModelEnum.UserRelationTypeEnum.厅管邀厅管)
                {
                    formDisplay.formItems.Add(new EmtInput("username")
                    {
                        title = "用户名",
                        placeholder = "请输入用户名",
                        colLength = 6,
                        eventJsChanges = new List<EmtFormBase.EventJsChange>
                        {
                            new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    func = GetParentUser,
                                    resCallJs = $@"(res.data == null) ? { new WuiPage.Notify().Info("没有上级用户!")} : {new WuiPage("post.superior_username").Set(new WuiPageJs.Code().Run("res.data"))}",
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "type_id", req.type_id },
                                        { "username", $"{new WuiPageJs("post.username").Value}"},
                                    }
                                }
                            },
                            new EmtFormBase.EventJsChange
                            {
                                eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                                {
                                    func = GetOtherTgUserInCurrentYY,
                                    resCallJs = $@"{new ModelBasic.EmtSelect.Js("post.user_sn_selected").options(@"JSON.parse(res.data)")}",
                                    attachPara = new Dictionary<string, object>
                                    {
                                        { "username", $"{new WuiPageJs("post.username").Value}"},
                                    }
                                }
                            },
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelect("user_sn_selected")
                    {
                        title = "选择新的上级用户",
                        options = new Dictionary<string, string>(),
                        colLength = 6
                    });
                }
                else
                {
                    formDisplay.formItems.Add(new EmtInput("username")
                    {
                        title = "用户名",
                        placeholder = "请输入用户名",
                        colLength = 6,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                func = GetParentUser,
                                resCallJs = $@"(res.data == null) ? { new WuiPage.Notify().Info("没有上级用户!")} : {new WuiPage("post.superior_username").Set(new WuiPageJs.Code().Run("res.data"))}",
                                attachPara = new Dictionary<string, object>
                                {
                                    { "type_id", req.type_id },
                                    { "username", $"{new WuiPageJs("post.username").Value}"},
                                }
                            }
                        }
                    });
                    formDisplay.formItems.Add(new EmtSelectFull("user_sn_selected")
                    {
                        title = "选择新的上级用户",
                        options = users,
                        colLength = 6
                    });
                }

                formDisplay.formItems.Add(new ModelBasic.EmtLabel("superior_username")
                {
                    title = "当前上级用户",
                    colLength = 6,
                });

                #endregion
                return formDisplay;
            }

            /// <summary>
            /// 获取厅管所属运营下的厅管
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public JsonResultAction GetOtherTgUserInCurrentYY(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                //1.获取req中的信息
                var username = req.GetPara("username");

                //2.校验tg是否存在
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空");
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {ModelEnum.UserTypeEnum.tger.ToInt()}");
                if (users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");

                //3.获取tg所在的运营
                var superior_yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, users[0].user_sn);

                //4.获取运营下的厅管
                var option = new Dictionary<string, string>();
                option.Add("解除绑定关系", "-");
                foreach (var item in new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, superior_yy_user_sn))
                {
                    option.Add(item.username, item.user_sn);
                }
                result.data = option.ToJson();
                return result;
            }

            /// <summary>
            /// 获取上级用户
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public JsonResultAction GetParentUser(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                //1.获取req中的信息
                var username = req.GetPara("username");
                var type_id = req.GetPara("type_id").ToInt();
                int t_user_type_id = new DomainUserBasic.UserRelationTypeApp().GetInfoById(type_id).t_user_type_id;

                //2.校验用户名是否存在
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空");
                if (type_id < 1) throw new WeicodeException("请选择tab页");
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {t_user_type_id}");
                if (users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");

                //3.获取上级用户
                //3.1运营邀请厅管：判断厅管是否有上级厅管，有则不能修改
                if ((ModelEnum.UserRelationTypeEnum)type_id == ModelEnum.UserRelationTypeEnum.运营邀厅管)
                {
                    var superior_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, users[0].user_sn);
                    if (!superior_tg_user_sn.IsNullOrEmpty()) throw new WeicodeException($"用户 '{username}' 存在上级厅管，请先解除上级厅管!");
                }
                var superior_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn((ModelEnum.UserRelationTypeEnum)type_id, users[0].user_sn);
                if (superior_user_sn.IsNullOrEmpty()) throw new WeicodeException($"用户 '{username}' 不存在上级，请绑定上级用户!");
                result.data = new DomainBasic.UserApp().GetInfoByUserSn(superior_user_sn).username;
                return result;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                /// <summary>
                /// 类型id
                /// </summary>
                public int type_id { get; set; }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 更新上级用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                //1.数据校验
                //1.1 校验数据是否存在
                var username = req.GetPara("username");
                var type_id = req.GetPara("type_id").ToInt();
                var new_superior_user_sn = req.GetPara("user_sn_selected");
                var superior_username = req.GetPara("superior_username");
                if (type_id <= 0) throw new WeicodeException("type_id不能为空!");
                if (username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空!");
                if (new_superior_user_sn.IsNullOrEmpty()) throw new WeicodeException("请选择新的上级用户!");

                var user_relation_type = new DomainUserBasic.UserRelationTypeApp().GetInfoById(type_id);
                //1.2判断用户是否存在
                int t_user_type_id = user_relation_type.t_user_type_id;
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {t_user_type_id}");
                if (users.Count < 1) throw new WeicodeException($"用户 '{username}' 不存在");
                var t_user_sn = users[0].user_sn;

                //1.3判断上级用户是否存在
                int f_user_type_id = user_relation_type.f_user_type_id;
                //todo：根据用户名和用户类型查找用户
                var superior_users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{superior_username}' and user_type_id = {f_user_type_id}");


                List<string> lSql = new List<string>();
                if (superior_users.Count == 1)//上级用户存在
                {
                    //2 解绑用户关系
                    var superior_user_sn = superior_users[0].user_sn;
                    lSql = new DomainUserBasic.UserRelationApp().UnBindTran((ModelEnum.UserRelationTypeEnum)type_id, superior_user_sn, t_user_sn);
                }

                //3 绑定用户关系
                if (new_superior_user_sn != "-")
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran((ModelEnum.UserRelationTypeEnum)type_id, new_superior_user_sn, t_user_sn));
                }
                DoMySql.ExecuteSqlTran(lSql);

                return new JsonResultAction();
            }
            #endregion
        }
        #endregion

        #region 多厅转运营
        public class TingsMoveYYer
        {
            #region DefaultView
            public PagePost Get(DtoReq req)
            {
                #region 设置页面模型
                var pageModel = new PagePost("post");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.jsFileNames.Add(PageModelBase.JsFileName.通知消息);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
                #endregion
            }
            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private CtlFormDisplay GetFormDisplay(PagePost pageModel, DtoReq req = null)
            {
                var formDisplay = pageModel.formDisplay;
                #region 表单元素
                formDisplay.formItems.Add(new EmtInput("tg_username")
                {
                    title = "厅管用户名",
                    placeholder = "请输入厅管用户名",
                    colLength = 6,
                    eventJsChanges = new List<EmtFormBase.EventJsChange>
                    {
                        new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                func = GetParentUser,
                                resCallJs = $@"(res.data == null) ? { new WuiPage.Notify().Info("没有上级厅管!")} : {new WuiPage("post.cur_super_tger").Set(new WuiPageJs.Code().Run("res.data"))}",
                                attachPara = new Dictionary<string, object>
                                {
                                    { "type_id", (int)ModelEnum.UserRelationTypeEnum.厅管邀厅管 },
                                    { "tg_username", $"{new WuiPageJs("post.tg_username").Value}"},
                                }
                            }
                        },
                        new EmtFormBase.EventJsChange
                        {
                            eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                            {
                                func = GetParentUser,
                                resCallJs = $@"(res.data == null) ? { new WuiPage.Notify().Info("没有上级运营!")} : {new WuiPage("post.cur_super_yyer").Set(new WuiPageJs.Code().Run("res.data"))}",
                                attachPara = new Dictionary<string, object>
                                {
                                    { "type_id", (int)ModelEnum.UserRelationTypeEnum.运营邀厅管 },
                                    { "tg_username", $"{new WuiPageJs("post.tg_username").Value}"},
                                }
                            }
                        },
                    }
                });
                formDisplay.formItems.Add(new EmtSelect("new_yyer")
                {
                    title = "选择新的运营",
                    options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                { "yy_user_sn","<%=page.new_yyer.value%>"}
                            },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("new_super_tger").options(@"JSON.parse(res.data)")};"
                        }
                    },
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtLabel("cur_super_tger")
                {
                    title = "当前上级厅管",
                    colLength = 6,
                });
                formDisplay.formItems.Add(new EmtSelect("new_super_tger")
                {
                    title = "选择上级厅管(可选)",
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtLabel("cur_super_yyer")
                {
                    title = "当前所属运营",
                    colLength = 6,
                });
                #endregion
                return formDisplay;
            }
            /// <summary>
            /// 获取厅管筛选项
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction GetTinGuan(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                result.data = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req["yy_user_sn"].ToNullableString()).ToJson();
                return result;
            }

            /// <summary>
            /// 获取上级用户
            /// </summary>
            /// <param name="arg"></param>
            /// <returns></returns>
            public JsonResultAction GetParentUser(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                //1.获取req中的信息
                var username = req.GetPara("tg_username");
                var type_id = req.GetPara("type_id").ToInt();

                //2.校验厅管用户名是否存在
                if (username.IsNullOrEmpty()) throw new WeicodeException("厅管用户名不能为空");
                if (type_id < 1) throw new WeicodeException("转移类型不存在");
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{username}' and user_type_id = {(int)ModelEnum.UserTypeEnum.tger}");
                if (users.Count < 1) throw new WeicodeException($"厅管 '{username}' 不存在");

                //3.获取上级用户
                var superior_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn((ModelEnum.UserRelationTypeEnum)type_id, users[0].user_sn);
                if (superior_user_sn.IsNullOrEmpty() && (ModelEnum.UserRelationTypeEnum)type_id == ModelEnum.UserRelationTypeEnum.运营邀厅管) throw new WeicodeException($"用户 '{username}' 不存在上级运营!");
                if (superior_user_sn.IsNullOrEmpty() && (ModelEnum.UserRelationTypeEnum)type_id == ModelEnum.UserRelationTypeEnum.厅管邀厅管) throw new WeicodeException($"用户 '{username}' 不存在上级厅管!");
                result.data = new DomainBasic.UserApp().GetInfoByUserSn(superior_user_sn).username;

                return result;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                /// <summary>
                /// 类型id
                /// </summary>
                public int type_id { get; set; }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 更新上级用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                //1.数据校验
                //1.1 校验数据是否存在
                var tg_username = req.GetPara("tg_username");   //厅管用户名
                var new_yyer_sn = req.GetPara("new_yyer");      //新的所属运营
                var new_super_tger_sn = req.GetPara("new_super_tger");  //新的上级厅管

                if (tg_username.IsNullOrEmpty()) throw new WeicodeException("用户名不能为空!");
                if (new_yyer_sn.IsNullOrEmpty()) throw new WeicodeException("请选择新的运营!");

                //1.2判断厅管是否存在
                var users = new DomainBasic.UserApp().GetInfosByWhere($"username = '{tg_username}' and user_type_id = {(int)ModelEnum.UserTypeEnum.tger}");
                if (users.Count < 1) throw new WeicodeException($"厅管 '{tg_username}' 不存在");
                var cur_tg_sn = users[0].user_sn;

                //2.若有上级厅管则解绑
                var cur_sup_tg_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀厅管, cur_tg_sn);
                if (!cur_sup_tg_sn.IsNullOrEmpty())
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, cur_sup_tg_sn, cur_tg_sn));
                }

                //3.将厅管以及所有下级厅管 转移到new_yyer_sn
                var cur_yyer_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, cur_tg_sn);
                List<string> tger_sns = new DomainUserBasic.UserRelationApp().GetNextAllUsersForOption(ModelEnum.UserRelationTypeEnum.厅管邀厅管, cur_tg_sn,"").Select(p=>p.value).ToList();
                tger_sns.Add(cur_tg_sn);
                foreach (var tg_sn in tger_sns)
                {
                    //3.1解绑当前yy
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(ModelEnum.UserRelationTypeEnum.运营邀厅管, cur_yyer_sn, tg_sn));
                    //3.2绑定new_yyer_sn
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.运营邀厅管, new_yyer_sn, tg_sn));
                }

                //4.若选择了上级厅管，则绑定新的厅管
                if (!new_super_tger_sn.IsNullOrEmpty())
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new_super_tger_sn, cur_tg_sn));
                }

                DoMySql.ExecuteSqlTran(lSql);
                return new JsonResultAction();
            }
            #endregion
        }
        #endregion
    }
}
