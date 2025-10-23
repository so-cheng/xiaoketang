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
    /// 厅管目标
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 设定单个厅管目标
        /// </summary>
        public class TgTargetPostSingle
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
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                /*
                 *  buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "月目标记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/TgManage/target/tgList",
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
                string code = @"if(page_post.amount_target_2.value == ''){ page_post.amount_target_2.set(parseFloat(page_post.amount_target_1.value));}
                                if(page_post.amount_target_3.value == ''){ page_post.amount_target_3.set(parseFloat(page_post.amount_target_1.value));}";

                var p_jixiao_target_tg = DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"id='{req.id}'", false);

                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                {
                    title = "目标月份",
                    isRequired = true,
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    defaultValue = DateTime.Today.ToString("yyyy-MM"),
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                {
                    title = "直播厅",
                    options = new ServiceFactory.UserInfo.Ting().GetTingsKvByTgsn(new UserIdentityBag().user_sn)
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_target_1")
                {
                    title = "音浪-阶段1(万)",
                    defaultValue = p_jixiao_target_tg.amount_target_1.ToString(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventJavascript = new EventJavascript
                        {
                            code = code
                        }
                    }
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_target_2")
                {
                    title = "音浪-阶段2(万)",
                    defaultValue = p_jixiao_target_tg.amount_target_2.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_target_3")
                {
                    title = "音浪-阶段3(万)",
                    defaultValue = p_jixiao_target_tg.amount_target_3.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("new_num")
                {
                    title = "目标拉新",
                    defaultValue = p_jixiao_target_tg.new_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("contact_num")
                {
                    title = "目标建联",
                    defaultValue = p_jixiao_target_tg.contact_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("num_2")
                {
                    title = "目标二消",
                    defaultValue = p_jixiao_target_tg.num_2.ToString(),
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq
            {
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
                var p_jixiao_target_tg = req.data_json.ToModel<ModelDb.p_jixiao_target_tg>();

                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.tger:
                        p_jixiao_target_tg.tg_user_sn = new UserIdentityBag().user_sn;
                        break;
                }
                
                if (p_jixiao_target_tg.ting_sn.IsNullOrEmpty()) throw new WeicodeException("请选择直播厅！");
                if (!DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"ting_sn = '{p_jixiao_target_tg.ting_sn}' and  yearmonth = '{dtoReqData.yearmonth}'", false).IsNullOrEmpty()) throw new WeicodeException($"当前月份已经设定过{dtoReqData.yearmonth}月份目标");
                // 对音浪相关字段的验证
                if (p_jixiao_target_tg.amount_target_1 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_1 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }

                if (p_jixiao_target_tg.amount_target_2 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_2 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }

                if (p_jixiao_target_tg.amount_target_3 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_3 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }

                // 对拉新字段的验证
                if (p_jixiao_target_tg.new_num < 0)
                {
                    throw new WeicodeException("拉新必须为数字！");
                }

                p_jixiao_target_tg.amount = p_jixiao_target_tg.amount_target_1.ToDecimal() + p_jixiao_target_tg.amount_target_2.ToDecimal() + p_jixiao_target_tg.amount_target_3.ToDecimal();
                p_jixiao_target_tg.yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, p_jixiao_target_tg.tg_user_sn);
                p_jixiao_target_tg.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                lSql.Add(p_jixiao_target_tg.ToModel<ModelDb.p_jixiao_target_tg>().InsertOrUpdateTran($"id='{p_jixiao_target_tg.id}'"));

                int daysInMonth = DateTime.DaysInMonth(p_jixiao_target_tg.yearmonth.ToDate().Year, p_jixiao_target_tg.yearmonth.ToDate().Month);
                var avg_amount = Math.Round((p_jixiao_target_tg.amount / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_new = Math.Round((p_jixiao_target_tg.new_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_contact = Math.Round((p_jixiao_target_tg.contact_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_num2 = Math.Round((p_jixiao_target_tg.num_2.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();

                var target_amount = new Dictionary<string, string>();
                target_amount.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_amount.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_amount.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_amount.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_amount.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_new = new Dictionary<string, string>();
                target_new.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_new.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_new.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_new.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_new.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_contact = new Dictionary<string, string>();
                target_contact.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_contact.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_contact.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_contact.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_contact.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_num2 = new Dictionary<string, string>();
                target_num2.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_num2.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_num2.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_num2.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_num2.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                for (int i = 1; i <= daysInMonth; i++)
                {
                    target_amount.Add($"amount_{i}", avg_amount.ToString());
                    target_new.Add($"new_{i}", avg_new.ToString());
                    target_contact.Add($"contact_{i}", avg_contact.ToString());
                    target_num2.Add($"num2_{i}", avg_num2.ToString());
                }
                lSql.Add(target_amount.ToModel<ModelDb.p_jixiao_tgtarget_amount>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_new.ToModel<ModelDb.p_jixiao_tgtarget_new>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_contact.ToModel<ModelDb.p_jixiao_tgtarget_contact>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_num2.ToModel<ModelDb.p_jixiao_tgtarget_num2>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));

                MysqlHelper.ExecuteSqlTran(lSql);

                var result = new JsonResultAction();

                return result;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                /// <summary>
                /// 目标月份
                /// </summary>
                public string yearmonth { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 设定厅管目标
        /// </summary>
        public class TgTargetPost
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
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "月目标记录",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/TgManage/target/tgList",
                    }
                });
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
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                {
                    title = "目标月份",
                    isRequired = true,
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    defaultValue = DateTime.Today.ToString("yyyy-MM"),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                {"yearmonth","<%=page.yearmonth.value%>"}
                            },
                            func = GetUserList,
                            resCallJs = $"{new ModelBasic.EmtTableEdit.Js("l_target").set(@"JSON.parse(res.data)")}"
                        }
                    },
                    colLength = 6
                });

                formDisplay.formItems.Add(new ModelBasic.EmtTableDataEdit("l_target")
                {
                    limit = 20,
                    height = "600px",
                    width = "1300px",
                    title = "月度目标",
                    colItems = new List<ModelBasic.EmtTableDataEdit.ColItem>
                    {
                        new ModelBasic.EmtTableDataEdit.ColItem("name")
                        {
                         title = "所属厅管",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("ting_name")
                        {
                         title = "直播厅",
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount")
                        {
                         title = "目标音浪",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount_target_1")
                        {
                         title = "音浪-阶段1",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount_target_2")
                        {
                         title = "音浪-阶段2",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("amount_target_3")
                        {
                         title = "音浪-阶段3",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("new_num")
                        {
                         title = "目标拉新",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("contact_num")
                        {
                         title = "目标建联",
                         edit = "text",
                         width = "120"
                        },
                        new ModelBasic.EmtTableDataEdit.ColItem("num_2")
                        {
                         title = "目标二消",
                         edit = "text",
                         width = "120"
                        },
                     },
                    defaultValue = DoMySql.FindObjectsBySql(GetDataForSql(DateTime.Today.ToString("yyyy-MM-dd").ToDate().ToString("yyyy-MM"))).ToJson(),
                    displayStatus = EmtModelBase.DisplayStatus.编辑
                });

                #endregion
                return formDisplay;
            }


            public JsonResultAction GetUserList(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                string yy_user_sn = req["yy_user_sn"].ToNullableString();
                if (yy_user_sn.IsNullOrEmpty())
                {
                    yy_user_sn = new UserIdentityBag().user_sn;
                }

                var data = DoMySql.FindObjectsBySql(GetDataForSql(req["yearmonth"].ToNullableString()));
                result.data = data.ToJson();
                return result;
            }

            /// <summary>
            /// 左表:运营下的所有直播厅,右表:已设置的厅目标记录
            /// </summary>
            /// <param name="yearmonth"></param>
            /// <returns></returns>
            public string GetDataForSql(string yearmonth)
            {
                return $@"SELECT
	                    user_info_tg.id AS id,
	                    (select NAME from user_base where user_sn = user_info_tg.tg_user_sn) name,
	                    ting_name,
	                    user_info_tg.ting_sn,
                        amount,
	                    amount_target_1,
	                    amount_target_2,
	                    amount_target_3,
	                    contact_num,
	                    new_num,
	                    num_2 
                    FROM
	                    user_info_tg
	                    LEFT JOIN ( SELECT * FROM p_jixiao_target_tg WHERE yearmonth = '{yearmonth}') AS t1 ON user_info_tg.ting_sn = t1.ting_sn 
                    WHERE
	                    user_info_tg.tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' AND user_info_tg.yy_user_sn = '{new UserIdentityBag().user_sn}' AND user_info_tg.STATUS = {ModelDb.user_info_tg.status_enum.正常.ToInt()}";
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

                if (dtoReqData.yearmonth.IsNullOrEmpty()) throw new WeicodeException("请选择目标月份！");
                foreach (var item in dtoReqData.l_target)
                {
                    /*if (!DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"tg_user_sn = '{item.tg_user_sn}' and  yearmonth = '{dtoReqData.yearmonth}'", false).IsNullOrEmpty()) throw new WeicodeException($"厅管:'{item.name}'已经设定过{dtoReqData.yearmonth}月份目标");*/
                    if (item.IsNullOrEmpty() || (item.amount_target_1.IsNullOrEmpty() && item.amount_target_2.IsNullOrEmpty() && item.amount_target_3.IsNullOrEmpty() && item.new_num.IsNullOrEmpty() && item.contact_num.IsNullOrEmpty() && item.amount_2.IsNullOrEmpty()))
                    {
                        continue;
                    }
                    if (item.amount_target_1 < 0) throw new WeicodeException("音浪必须大于0！");
                    if (item.amount_target_2 < 0) throw new WeicodeException("音浪必须大于0！");
                    if (item.amount_target_3 < 0) throw new WeicodeException("音浪必须大于0！");
                    if (item.new_num < 0) throw new WeicodeException("拉新必须为数字！");
                    // 对音浪相关字段的验证
                    if (item.amount_target_1 < 0)
                    {
                        throw new WeicodeException("音浪必须为数字！");
                    }
                    else if (item.amount_target_1 > 10000)
                    {
                        throw new WeicodeException("音浪数值过大，是否输入错误？");
                    }

                    if (item.amount_target_2 < 0)
                    {
                        throw new WeicodeException("音浪必须为数字！");
                    }
                    else if (item.amount_target_2 > 10000)
                    {
                        throw new WeicodeException("音浪数值过大，是否输入错误？");
                    }

                    if (item.amount_target_3 < 0)
                    {
                        throw new WeicodeException("音浪必须为数字！");
                    }
                    else if (item.amount_target_3 > 10000)
                    {
                        throw new WeicodeException("音浪数值过大，是否输入错误？");
                    }
                    item.amount = item.amount_target_1.ToDecimal() + item.amount_target_2.ToDecimal() + item.amount_target_3.ToDecimal();
                    if(item.amount > 1000)
                    {
                        throw new WeicodeException("目标音浪不得超过1000，是否输入错误？");
                    }

                    item.yearmonth = dtoReqData.yearmonth;
                    item.yy_user_sn = new UserIdentityBag().user_sn;
                    item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    lSql.Add(item.ToModel<ModelDb.p_jixiao_target_tg>().InsertOrUpdateTran($"ting_sn='{item.ting_sn}' and yearmonth='{item.yearmonth}'"));

                    int daysInMonth = DateTime.DaysInMonth(item.yearmonth.ToDate().Year, item.yearmonth.ToDate().Month);
                    var avg_amount = Math.Round((item.amount / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_new = Math.Round((item.new_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_contact = Math.Round((item.contact_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                    var avg_num2 = Math.Round((item.num_2.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();

                    var target_amount = new Dictionary<string, string>();
                    target_amount.Add("tenant_id", item.tenant_id.ToString());
                    target_amount.Add("y_user_sn", item.yy_user_sn);
                    target_amount.Add("tg_user_sn", item.tg_user_sn);
                    target_amount.Add("ting_sn", item.ting_sn);
                    target_amount.Add("yearmonth", item.yearmonth);

                    var target_new = new Dictionary<string, string>();
                    target_new.Add("tenant_id", item.tenant_id.ToString());
                    target_new.Add("yy_user_sn", item.yy_user_sn);
                    target_new.Add("tg_user_sn", item.tg_user_sn);
                    target_new.Add("ting_sn", item.ting_sn);
                    target_new.Add("yearmonth", item.yearmonth);

                    var target_contact = new Dictionary<string, string>();
                    target_contact.Add("tenant_id", item.tenant_id.ToString());
                    target_contact.Add("yy_user_sn", item.yy_user_sn);
                    target_contact.Add("tg_user_sn", item.tg_user_sn);
                    target_contact.Add("ting_sn", item.ting_sn);
                    target_contact.Add("yearmonth", item.yearmonth);

                    var target_num2 = new Dictionary<string, string>();
                    target_num2.Add("tenant_id", item.tenant_id.ToString());
                    target_num2.Add("yy_user_sn", item.yy_user_sn);
                    target_num2.Add("tg_user_sn", item.tg_user_sn);
                    target_num2.Add("ting_sn", item.ting_sn);
                    target_num2.Add("yearmonth", item.yearmonth);

                    for (int i = 1; i <= daysInMonth; i++)
                    {
                        target_amount.Add($"amount_{i}", avg_amount.ToString());
                        target_new.Add($"new_{i}", avg_new.ToString());
                        target_contact.Add($"contact_{i}", avg_contact.ToString());
                        target_num2.Add($"num2_{i}", avg_num2.ToString());
                    }
                    lSql.Add(target_amount.ToModel<ModelDb.p_jixiao_tgtarget_amount>().InsertOrUpdateTran($"ting_sn='{item.ting_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_new.ToModel<ModelDb.p_jixiao_tgtarget_new>().InsertOrUpdateTran($"ting_sn='{item.ting_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_contact.ToModel<ModelDb.p_jixiao_tgtarget_contact>().InsertOrUpdateTran($"ting_sn='{item.ting_sn}' and yearmonth='{item.yearmonth}'"));
                    lSql.Add(target_num2.ToModel<ModelDb.p_jixiao_tgtarget_num2>().InsertOrUpdateTran($"ting_sn='{item.ting_sn}' and yearmonth='{item.yearmonth}'"));
                }
                DoMySql.ExecuteSqlTran(lSql);
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
                public List<p_jixiao_target_tg> l_target { get; set; }

                /// <summary>
                /// 目标月份
                /// </summary>
                public string yearmonth { get; set; }
            }


            public class p_jixiao_target_tg : ModelDb.p_jixiao_target_tg
            {
                /// <summary>
                /// 厅管名字
                /// </summary>
                public string name { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 编辑单个厅管月目标
        /// </summary>
        public class TgTargetEdit
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                {
                    returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                };
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.buttonGroup = GetButtonGroup(req);
                if(req.id > 0)
                {
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction,
                    };
                }
                else
                {
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = new TgTargetPostSingle().PostAction,
                    };
                }
                
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
                /*buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "主播每月目标列表",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "/ZbManage/target/List",
                    }
                });*/
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
                string code = @"if(page_post.amount_target_2.value == ''){ page_post.amount_target_2.set(parseFloat(page_post.amount_target_1.value));}
                                if(page_post.amount_target_3.value == ''){ page_post.amount_target_3.set(parseFloat(page_post.amount_target_1.value));}";

                var p_jixiao_target_tg = DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"id='{req.id}'", false);

                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    title = "id",
                    defaultValue = p_jixiao_target_tg.id.ToString(),
                });
                if(req.id > 0)
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("yearmonth")
                    {
                        title = "目标月份",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = p_jixiao_target_tg.yearmonth,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        title = "直播厅",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_target_tg.ting_sn).ting_name
                    });
                }
                else
                {
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("yearmonth")
                    {
                        title = "目标月份",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = req.yearmonth,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_name")
                    {
                        title = "直播厅",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = new ServiceFactory.UserInfo.Ting().GetTingBySn(req.ting_sn).ting_name
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_sn")
                    {
                        title = "直播厅sn",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = req.ting_sn,
                        isDisplay = false
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("yy_user_sn")
                    {
                        title = "运营sn",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = req.yy_user_sn,
                        isDisplay = false
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("tg_user_sn")
                    {
                        title = "厅管sn",
                        displayStatus = EmtModelBase.DisplayStatus.只读,
                        defaultValue = req.tg_user_sn,
                        isDisplay = false
                    });
                }
                
                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount_target_1")
                {
                    title = "音浪-阶段1",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.amount_target_1.ToString(),
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventJavascript = new EventJavascript
                        {
                            code = code
                        }
                    }
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount_target_2")
                {
                    title = "音浪-阶段2",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.amount_target_2.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("amount_target_3")
                {
                    title = "音浪-阶段3",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.amount_target_3.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("new_num")
                {
                    title = "目标拉新",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.new_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("contact_num")
                {
                    title = "目标建联",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.contact_num.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("num_2")
                {
                    title = "目标二消",
                    isRequired = true,
                    defaultValue = p_jixiao_target_tg.num_2.ToString(),
                });


                #endregion
                return formDisplay;
            }

            public class DtoReq
            {
                /// <summary>
                /// 主播目标记录id
                /// </summary>
                public int id { get; set; }
                public string yearmonth { get; set; }
                public string ting_sn { get; set; }
                public string yy_user_sn { get; set; }
                public string tg_user_sn { get; set; }
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
                var result = new JsonResultAction();
                var p_jixiao_target_tg = DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"id='{req.GetPara()["id"].ToNullableString()}'", false);
                p_jixiao_target_tg.amount_target_1 = req.GetPara()["amount_target_1"].ToNullableString().ToDecimal();
                p_jixiao_target_tg.amount_target_2 = req.GetPara()["amount_target_2"].ToNullableString().ToDecimal();
                p_jixiao_target_tg.amount_target_3 = req.GetPara()["amount_target_3"].ToNullableString().ToDecimal();
                p_jixiao_target_tg.new_num = req.GetPara()["new_num"].ToNullableString().ToInt();
                p_jixiao_target_tg.contact_num = req.GetPara()["contact_num"].ToNullableString().ToInt();
                p_jixiao_target_tg.num_2 = req.GetPara()["num_2"].ToNullableString().ToInt();

                if (p_jixiao_target_tg.IsNullOrEmpty()) throw new WeicodeException("无效的月目标");
                if (p_jixiao_target_tg.amount_target_1 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_1 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }

                if (p_jixiao_target_tg.amount_target_2 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_2 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }

                if (p_jixiao_target_tg.amount_target_3 < 0)
                {
                    throw new WeicodeException("音浪必须为数字！");
                }
                else if (p_jixiao_target_tg.amount_target_3 > 10000)
                {
                    throw new WeicodeException("音浪数值过大，是否输入错误？");
                }
                if (p_jixiao_target_tg.new_num < 0) throw new WeicodeException("拉新必须为数字！");

                p_jixiao_target_tg.amount = p_jixiao_target_tg.amount_target_1.ToDecimal() + p_jixiao_target_tg.amount_target_2.ToDecimal() + p_jixiao_target_tg.amount_target_3.ToDecimal();

                lSql.Add(p_jixiao_target_tg.ToModel<ModelDb.p_jixiao_target_tg>().UpdateTran());

                int daysInMonth = DateTime.DaysInMonth(p_jixiao_target_tg.yearmonth.ToDate().Year, p_jixiao_target_tg.yearmonth.ToDate().Month);
                var avg_amount = Math.Round((p_jixiao_target_tg.amount / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_new = Math.Round((p_jixiao_target_tg.new_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_contact = Math.Round((p_jixiao_target_tg.contact_num.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();
                var avg_num2 = Math.Round((p_jixiao_target_tg.num_2.ToDouble() / daysInMonth).ToDouble(), 2).ToDecimal();

                var target_amount = new Dictionary<string, string>();
                target_amount.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_amount.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_amount.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_amount.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_amount.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_new = new Dictionary<string, string>();
                target_new.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_new.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_new.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_new.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_new.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_contact = new Dictionary<string, string>();
                target_contact.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_contact.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_contact.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_contact.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_contact.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                var target_num2 = new Dictionary<string, string>();
                target_num2.Add("tenant_id", p_jixiao_target_tg.tenant_id.ToString());
                target_num2.Add("yy_user_sn", p_jixiao_target_tg.yy_user_sn);
                target_num2.Add("tg_user_sn", p_jixiao_target_tg.tg_user_sn);
                target_num2.Add("ting_sn", p_jixiao_target_tg.ting_sn);
                target_num2.Add("yearmonth", p_jixiao_target_tg.yearmonth);

                for (int i = 1; i <= daysInMonth; i++)
                {
                    target_amount.Add($"amount_{i}", avg_amount.ToString());
                    target_new.Add($"new_{i}", avg_new.ToString());
                    target_contact.Add($"contact_{i}", avg_contact.ToString());
                    target_num2.Add($"num2_{i}", avg_num2.ToString());
                }
                lSql.Add(target_amount.ToModel<ModelDb.p_jixiao_tgtarget_amount>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_new.ToModel<ModelDb.p_jixiao_tgtarget_new>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_contact.ToModel<ModelDb.p_jixiao_tgtarget_contact>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(target_num2.ToModel<ModelDb.p_jixiao_tgtarget_num2>().InsertOrUpdateTran($"ting_sn='{p_jixiao_target_tg.ting_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {

            }

            #endregion
        }

        /// <summary>
        ///厅管目标
        /// </summary>
        public class TgTargetList
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

                #region
                string date = DateTime.Now.ToString("yyyy-MM");
                if (DateTime.Now.Day >= 25)
                    date = DateTime.Now.AddMonths(1).ToString("yyyy-MM");

                int tings = new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.基地,
                        UserSn = new UserIdentityBag().user_sn
                    }
                }).Count; ;//总厅数
                
                string where = $"yearmonth='{date}'";
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $@" and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn
                            }
                        })}";
                        break;
                }
                int p_jixiao_target_tg = DoMySql.FindList<ModelDb.p_jixiao_target_tg>(where).Count;//本月提报数
                int unreported = tings - p_jixiao_target_tg; //未提报数量

                // 设置头部提示
                string top = "";
                top += $@"<div class=""layui-card"">";
                top += $@"  <div class=""layui-card-body"">";
                top += $@"    <div class=""layui-row layui-col-space15"">";
                // 总厅数
                top += $@"      <div class=""layui-col-md3"">";
                top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                top += $@"          <div class=""layui-flex layui-items-center"">";
                top += $@"            <div class=""layui-icon layui-icon-home"" style=""font-size: 24px; color: #1E9FFF; margin-right: 10px;""></div>";
                top += $@"            <div>";
                top += $@"              {date} 提报统计";
                top += $@"            </div>";
                top += $@"          </div>";
                top += $@"        </div>";
                top += $@"      </div>";
                // 总厅数
                top += $@"      <div class=""layui-col-md3"">";
                top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                top += $@"          <div class=""layui-flex layui-items-center"">";
                top += $@"            <div class=""layui-icon layui-icon-home"" style=""font-size: 24px; color: #1E9FFF; margin-right: 10px;""></div>";
                top += $@"            <div>";
                top += $@"              <div class=""text-muted"">总厅数{tings}</div>";
                top += $@"            </div>";
                top += $@"          </div>";
                top += $@"        </div>";
                top += $@"      </div>";

                // 提报数量
                top += $@"      <div class=""layui-col-md3"">";
                top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                top += $@"          <div class=""layui-flex layui-items-center"">";
                top += $@"            <div class=""layui-icon layui-icon-ok-circle"" style=""font-size: 24px; color: #009688; margin-right: 10px;""></div>";
                top += $@"            <div>";
                top += $@"              <div class=""text-muted"">已提报数量{p_jixiao_target_tg}</div>";
                top += $@"            </div>";
                top += $@"          </div>";
                top += $@"        </div>";
                top += $@"      </div>";

                // 未提报数量及详情
                top += $@"      <div class=""layui-col-md3"">";
                top += $@"        <div class=""layui-bg-gray layui-p-3 rounded"">";
                top += $@"          <div class=""layui-flex layui-items-center justify-content-between"">";
                top += $@"            <div class=""layui-flex layui-items-center"">";
                top += $@"              <div class=""layui-icon layui-icon-info-circle"" style=""font-size: 24px; color: #FF5722; margin-right: 10px;""></div>";
                top += $@"              <div>";
                top += $@"                <div class=""text-muted"">未提报数量{unreported}</div>";           
                top += $@"              </div>";
                top += $@"            </div>";
                top += $@"            <a href=""javascript:win_pop_iframe('未提报名单','/Target/TgTarget/NotReported?date={date}', 800, 1200)"" class=""layui-btn layui-btn-primary layui-btn-sm"" style=""margin-left: 10px;"">";
                top += $@"              查看详情 <i class=""layui-icon layui-icon-right""></i>";
                top += $@"            </a>";
                top += $@"          </div>";
                top += $@"        </div>";
                top += $@"      </div>";
                top += $@"    </div>";
                top += $@"  </div>";
                top += $@"</div>";

                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = top
                    }
                };
                return pageModel;
                #endregion
            }

            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new ModelBasic.CtlListFilter();
                string js = "";
                if (new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id == new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id)
                {
                    js = new ModelBasic.EmtSelect.Js("zb_user_sn").clear() + ";";
                }

                var yy_options = new Dictionary<string, string>();
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        yy_options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn
                            }
                        });
                        break;
                    case ModelEnum.UserTypeEnum.manager:
                        yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                        break;
                }

                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "运营账号",
                    options = yy_options,
                    disabled = true,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                {"yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{js}"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                });

                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    placeholder = "选择月份",
                });
                return listFilter;
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
                var listDisplay = new ModelBasic.CtlListDisplay(req);
                listDisplay.operateWidth = "180";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                {
                    text = "所属运营",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "所属厅管",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                {
                    text = "直播厅",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yearmonth")
                {
                    text = "目标月份",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "目标音浪(万)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_target_1")
                {
                    text = "音浪-阶段1(万)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_target_2")
                {
                    text = "音浪-阶段2(万)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_target_3")
                {
                    text = "音浪-阶段3(万)",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_complete")
                {
                    text = "已完成音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "目标拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_complete")
                {
                    text = "已完成拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2")
                {
                    text = "目标二消",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("num_2_complete")
                {
                    text = "已完成二消",
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
                        url = "TgEdit"
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
                var p_jixiao_target_tg = DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"id='{req.GetPara()["id"].ToNullableString()}'");
                lSql.Add(p_jixiao_target_tg.DeleteTran());

                lSql.Add(new ModelDb.p_jixiao_tgtarget_amount().DeleteTran($"tg_user_sn='{p_jixiao_target_tg.tg_user_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_tgtarget_new().DeleteTran($"tg_user_sn='{p_jixiao_target_tg.tg_user_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_tgtarget_contact().DeleteTran($"tg_user_sn='{p_jixiao_target_tg.tg_user_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));
                lSql.Add(new ModelDb.p_jixiao_tgtarget_num2().DeleteTran($"tg_user_sn='{p_jixiao_target_tg.tg_user_sn}' and yearmonth='{p_jixiao_target_tg.yearmonth}'"));

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
                string where = "1=1";
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                }
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and yy_user_sn = '{req["yy_user_sn"].ToNullableString()}'";
                }
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $@" and yy_user_sn in {new ServiceFactory.UserInfo.Yy().GetYyBaseInfosForSql(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn
                            }
                        })}";
                        break;
                }

                if (!req["yearmonth"].ToNullableString().IsNullOrEmpty()) where += $" AND (yearmonth ='{req["yearmonth"].ToString()}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_target_tg, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.p_jixiao_target_tg
            {
                public string ting_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                public string yy_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                    }
                }
                public string submit_time
                {
                    get
                    {
                        return this.create_time.ToDate().ToString("yyyy-MM");
                    }
                }

                public string amount_complete
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.doudata_day_ting_31>($"ting_sn = '{ting_sn}' and yearmonth = '{yearmonth}'", false).total_income.ToString();
                    }
                }
                public string new_num_complete
                {
                    get
                    {
                        int? count = 0;
                        foreach (var item in DoMySql.FindList<ModelDb.p_jixiao_day>($"tg_user_sn='{this.tg_user_sn}' and c_date >='{this.yearmonth.ToDate().ToString("yyyy-MM-01")}' and c_date<'{this.yearmonth.ToDate().AddMonths(1).ToString("yyyy-MM-01")}'"))
                        {
                            count += item.new_num;
                        }
                        return count.ToString();
                    }
                }
                public string num_2_complete
                {
                    get
                    {
                        int? count = 0;
                        foreach (var item in DoMySql.FindList<ModelDb.p_jixiao_day>($"tg_user_sn='{this.tg_user_sn}' and c_date >='{this.yearmonth.ToDate().ToString("yyyy-MM-01")}' and c_date<'{this.yearmonth.ToDate().AddMonths(1).ToString("yyyy-MM-01")}'"))
                        {
                            count += item.num_2;
                        }
                        return count.ToString();
                    }
                }
            }
            
            #endregion
        }

        /// <summary>
        /// 厅管目标进度
        /// </summary>
        public class ScheduleTgList
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
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("yearmonth")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.month,
                    defaultValue = DateTime.Today.ToString("yyyy-MM"),
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
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "所属厅管",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                {
                    text = "直播厅",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yearmonth")
                {
                    text = "目标月份",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount")
                {
                    text = "目标音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num")
                {
                    text = "目标拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_finish")
                {
                    text = "已完成音浪",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_finish")
                {
                    text = "已完成拉新",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_progress")
                {
                    mode = ModelBasic.EmtModel.ListItem.Mode.进度条,
                    text = "目标音浪进度",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("new_num_progress")
                {
                    mode = ModelBasic.EmtModel.ListItem.Mode.进度条,
                    text = "目标拉新进度",
                    width = "120",
                    minWidth = "120"
                });
                #region 操作列
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
                var req = reqJson.GetPara();
                string where = $"yy_user_sn = '{new UserIdentityBag().user_sn}'";
                if (!req["yearmonth"].ToNullableString().IsNullOrEmpty()) where += $" AND (yearmonth ='{req["yearmonth"]}')";
                var filter = new DoMySql.Filter
                {
                    where = where,
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_target_tg, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                public string yearmonth { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_target_tg
            {
                public string tg_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                    }
                }
                public string ting_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }
                public string amount_progress
                {
                    get
                    {
                        var amount = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(amount)", $"tg_user_sn ='{this.tg_user_sn}' and c_date >= '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        var result = ((amount[0].ToInt() / this.amount).ToDouble() * 100).ToFixed(2) + "%";
                        return result;
                    }
                }
                public string new_num_progress
                {
                    get
                    {
                        var new_num = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(new_num)", $"tg_user_sn ='{this.tg_user_sn}' and c_date >= '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");
                        var result = ((new_num[0].ToDouble() / this.new_num).ToDouble() * 100).ToFixed(2) + "%";
                        return result;
                    }
                }
                public string new_num_finish
                {
                    get
                    {
                        var new_num = DoMySql.FindField<ModelDb.p_jixiao_day>("sum(new_num)", $"tg_user_sn ='{this.tg_user_sn}' and c_date >= '{this.yearmonth + "-01"}' and c_date<'{this.yearmonth.ToDate().AddMonths(1) + "-01"}'");

                        return new_num[0];
                    }
                }
                public string amount_finish
                {
                    get
                    {
                        return DoMySql.FindEntity<ModelDb.doudata_day_ting_31>($"ting_sn = '{this.ting_sn}' and yearmonth = '{this.yearmonth}'", false).total_income.ToString();
                    }
                }
            }

            #endregion
        }

        public class NotReportedList
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
                string js = "";
                if (new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id == new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id)
                {
                    js = new ModelBasic.EmtSelect.Js("zb_user_sn").clear() + ";";
                }

                var yy_options = new Dictionary<string, string>();
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        yy_options = new ServiceFactory.UserInfo.Yy().GetBaseInfosForKv(new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter
                        {
                            attachUserType = new ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType
                            {
                                userType = ServiceFactory.UserInfo.Yy.YyBaseInfoFilter.AttachUserType.UserType.基地,
                                UserSn = new UserIdentityBag().user_sn
                            }
                        });
                        break;
                    case ModelEnum.UserTypeEnum.manager:
                        yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();
                        break;
                }

                listFilter.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                {
                    placeholder = "运营账号",
                    options = yy_options,
                    disabled = true,
                    eventJsChange = new EmtFormBase.EventJsChange
                    {
                        eventCsAction = new EmtFormBase.EventJsChange.EventCsAction
                        {
                            attachPara = new Dictionary<string, object>
                            {
                                {"yy_user_sn","<%=page.yy_user_sn.value%>"}
                            },
                            func = GetTinGuan,
                            resCallJs = $"{new ModelBasic.EmtSelect.Js("tg_user_sn").options(@"JSON.parse(res.data)")};{js}"
                        }
                    }
                });

                listFilter.formItems.Add(new ModelBasic.EmtSelect("tg_user_sn")
                {
                    placeholder = "厅管账号",
                    disabled = true,
                    options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(new UserIdentityBag().user_sn),
                });
                return listFilter;
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
                var listDisplay = new ModelBasic.CtlListDisplay(req);
                listDisplay.operateWidth = "120";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData,
                    attachPara = new Dictionary<string, object>
                    {
                        {"date",req.date.ToString()}
                    }
                };

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_name")
                {
                    text = "所属运营",
                    width = "140",
                    minWidth = "140"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tg_name")
                {
                    text = "所属厅管",
                    width = "140",
                    minWidth = "140"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_name")
                {
                    text = "直播厅",
                    width = "140",
                    minWidth = "140"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_sn")
                {
                    text = "直播厅sn",
                    width = "140",
                    minWidth = "140",
                    mode = ModelBasic.EmtModel.ListItem.Mode.隐藏字段
                });

                #region 操作列按钮

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        field_paras = "ting_sn=ting_sn,tg_user_sn=tg_user_sn",
                        url = "Edit?yearmonth="+ req.date.ToString(),
                        layer_width = "400",
                        layer_height = "600"
                    },
                    text = "提报目标",
                });

                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                public string date { get; set; }
            }

            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                var where = $"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and status = {ModelDb.user_info_tg.status_enum.正常.ToSByte()} and ting_sn not in (select ting_sn from p_jixiao_target_tg where yearmonth='{req["date"]}')";
                if (!req["tg_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and tg_user_sn = '{req["tg_user_sn"].ToNullableString()}'";
                }
                if (!req["yy_user_sn"].ToNullableString().IsNullOrEmpty())
                {
                    where += $" and yy_user_sn = '{req["yy_user_sn"].ToNullableString()}'";
                }
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $@" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where,
                    orderby = "order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_tg, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {

            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.user_info_tg
            {
                public string yy_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).name;
                    }
                }
                public string tg_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).name;
                    }
                }
            }
            #endregion

            #region 异步请求处理

            public class DtoReqData : ModelDb.p_tingzhan
            {
            }
            #endregion
        }
    }

    public class ComponentPartial
    {
        public string GetTopPartial()
        {
            return "";
        }
    }
}
