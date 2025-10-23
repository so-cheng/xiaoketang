using System;
using System.Linq;
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
    public partial class PageFactory
    {
        /// <summary>
        /// 成才
        /// </summary>
        public partial class Cencai
        {
            /// <summary>
            /// 创建成才表单
            /// </summary>
            public class CencaiPost
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
                    var p_cencai = DoMySql.FindEntityById<ModelDb.p_cencai>(req.id);
                    string c_date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!p_cencai.IsNullOrEmpty())
                    {
                        p_cencai.c_date.ToDateTime().ToString("yyyy-MM-dd");
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("tger_sn")
                    {
                        defaultValue = req.tg_user_sn,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("round")
                    {
                        title = "轮次",
                        defaultValue = req.round.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtSelect("yy_user_sn")
                    {
                        title = "选择运营",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        defaultValue = req.yy_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "window.location.href='/cencai/zb/post?yy_user_sn='+page_post.yy_user_sn.value",
                            }
                        },
                    });
                    formDisplay.formItems.Add(new EmtSelect("tg_user_sn")
                    {
                        title = "选择厅管",
                        options = new ServiceFactory.UserInfo.Tg().GetTreeOptionDic(req.yy_user_sn),
                        defaultValue = req.tg_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = "window.location.href='/cencai/zb/post?yy_user_sn='+page_post.yy_user_sn.value+'&tg_user_sn='+page_post.tg_user_sn.value",
                            }
                        },
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("zbers")
                    {
                        title = "主播",
                        defaultValue = req.zb_user_sn,
                        bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.厅管邀主播, req.tg_user_sn),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("teacher")
                    {
                        title = "师父名字",
                        defaultValue = p_cencai.teacher,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "开始日期",
                        defaultValue = c_date,
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("days")
                    {
                        title = "考核天数",
                        defaultValue = p_cencai.days.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_lx")
                    {
                        title = "拉新目标",
                        defaultValue = p_cencai.target_lx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_erx")
                    {
                        title = "二消目标",
                        defaultValue = p_cencai.target_erx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_jl")
                    {
                        title = "建联目标",
                        defaultValue = p_cencai.target_jl.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_yl")
                    {
                        title = "音浪目标",
                        defaultValue = p_cencai.target_yl.ToNullableString(),
                        colLength = 12
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// p_cencai_id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 厅管user_sn
                    /// </summary>
                    public string tg_user_sn { get; set; }

                    /// <summary>
                    /// 运营sn
                    /// </summary>
                    public string yy_user_sn { get; set; }

                    /// <summary>
                    /// 主播sn
                    /// </summary>
                    public string zb_user_sn { get; set; }

                    /// <summary>
                    /// 轮次
                    /// </summary>
                    public int round { get; set; } = 1;
                }
                #endregion
                #region 新增成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();

                    var p_cencai = req.data_json.ToModel<ModelDb.p_cencai>();

                    var zbers = req.GetPara("zbers").Split(',').ToList();
                    if (p_cencai.tger_sn.IsNullOrEmpty()) throw new WeicodeException("请选择厅管!");
                    if (req.GetPara("zbers").IsNullOrEmpty()) throw new WeicodeException("请选择主播!");
                    p_cencai.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_cencai.is_finished = (int)ModelDb.p_cencai.is_finished_enum.未结束;
                    foreach (var user_sn in zbers)
                    {
                        p_cencai.zber_sn = user_sn;
                        lSql.Add(p_cencai.InsertOrUpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai
                {
                    //多个主播user_sn
                    public string zbers { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 编辑成才表单
            /// </summary>
            public class CencaiEdit
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
                    var p_cencai = DoMySql.FindEntityById<ModelDb.p_cencai>(req.id);

                    var p_jixiao_days = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{p_cencai.zber_sn}' and c_date between '{p_cencai.c_date.ToDateString( ConvertExt.DateFormate.yyyy_MM_dd)}' and '{req.end_date.ToDate().ToDateString( ConvertExt.DateFormate.yyyy_MM_dd)}'");

                    string c_date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!p_cencai.IsNullOrEmpty())
                    {
                        c_date = p_cencai.c_date.ToDateTime().ToString("yyyy-MM-dd");
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("zb_user_sn")
                    {
                        defaultValue = req.zb_user_sn.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("round")
                    {
                        title = "轮次",
                        defaultValue = p_cencai.round.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("teacher")
                    {
                        title = "师父名字",
                        defaultValue = p_cencai.teacher,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "开始日期",
                        defaultValue = c_date,
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("days")
                    {
                        title = "考核天数",
                        defaultValue = p_cencai.days.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_lx")
                    {
                        title = "拉新目标",
                        defaultValue = p_cencai.target_lx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_erx")
                    {
                        title = "二消目标",
                        defaultValue = p_cencai.target_erx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_jl")
                    {
                        title = "建联目标",
                        defaultValue = p_cencai.target_jl.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_yl")
                    {
                        title = "音浪目标",
                        defaultValue = p_cencai.target_yl.ToNullableString(),
                        colLength = 12
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHtml("Label")
                    {
                        Content = "统计周期内的每日数据总和;"+
                                        "</br>如果与实际数据不符，则通过此处手动修改;"+
                                        "</br>产生的差额被计入前一天的每日数据中。"
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInput("new_amount")
                    {
                        title = "拉新流水",
                        defaultValue = p_jixiao_days.Sum(p => (int)p.new_num).ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("new_amount_old")
                    {
                        defaultValue = p_jixiao_days.Sum(p => (int)p.new_num).ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("amount_2")
                    {
                        title = "二消流水",
                        defaultValue = p_jixiao_days.Sum(p => (int)p.amount_2).ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("amount_2_old")
                    {
                        defaultValue = p_jixiao_days.Sum(p => (int)p.amount_2).ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("old_amount")
                    {
                        title = "老用户流水",
                        defaultValue = p_jixiao_days.Sum(p => (int)p.old_amount).ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("old_amount_old")
                    {
                        defaultValue = p_jixiao_days.Sum(p => (int)p.old_amount).ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("activity_amount")
                    {
                        title = "活动流水",
                        defaultValue = p_jixiao_days.Sum(p => (int)p.amount).ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("activity_amount_old")
                    {
                        defaultValue = p_jixiao_days.Sum(p => (int)p.amount).ToNullableString(),
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// p_cencai_id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 厅管user_sn
                    /// </summary>
                    public string tg_user_sn { get; set; }

                    /// <summary>
                    /// 运营sn
                    /// </summary>
                    public string yy_user_sn { get; set; }

                    /// <summary>
                    /// 主播sn
                    /// </summary>
                    public string zb_user_sn { get; set; }

                    /// <summary>
                    /// 轮次
                    /// </summary>
                    public int round { get; set; } = 1;
                    
                    /// <summary>
                    /// 统计截止日期 
                    /// </summary>
                    public string end_date { get; set; }
                }
                #endregion
                #region 新增成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai = req.data_json.ToModel<ModelDb.p_cencai>();
                    

                    var amount_2 = req.GetPara("amount_2").ToInt();
                    var new_amount = req.GetPara("new_amount").ToInt();
                    var old_amount = req.GetPara("old_amount").ToInt();
                    var activity_amount = req.GetPara("activity_amount").ToInt();

                    var amount_2_old = req.GetPara("amount_2_old").ToInt();
                    var new_amount_old = req.GetPara("new_amount_old").ToInt();
                    var old_amount_old = req.GetPara("old_amount_old").ToInt();
                    var activity_amount_old = req.GetPara("activity_amount_old").ToInt();


                    //如果修改了合计值，并且昨日未提交绩效
                    var p_jixiao_day = DoMySql.FindEntity<ModelDb.p_jixiao_day>($"zb_user_sn='{req.GetPara("zb_user_sn")}' and c_date='{DateTime.Today.AddDays(-1).ToDateString(ConvertExt.DateFormate.yyyy_MM_dd)}'", false);
                    if (p_jixiao_day.id == 0 && (amount_2 != amount_2_old || new_amount!= new_amount_old || old_amount!= old_amount_old || activity_amount!= activity_amount_old))
                    {
                        throw new Exception($"主播:{new ServiceFactory.ZbInfoService().GetZbInfo(req.GetPara("zb_user_sn")).username}缺失{DateTime.Today.AddDays(-1).ToDateString(ConvertExt.DateFormate.yyyy_MM_dd)}的提报数据，请先补全数据");
                    }


                    //计算新的值，并赋值给昨天的数据
                    p_jixiao_day.new_num = p_jixiao_day.new_num + new_amount - new_amount_old;
                    p_jixiao_day.amount_2 = p_jixiao_day.amount_2 + amount_2 - amount_2_old;
                    p_jixiao_day.old_amount = p_jixiao_day.old_amount+ old_amount - old_amount_old;
                    p_jixiao_day.amount = p_jixiao_day.amount + activity_amount - activity_amount_old;

                    //如果是新增，初始设置状态为未结束
                    if (p_cencai.id==0)
                    {
                        p_cencai.is_finished = (int)ModelDb.p_cencai.is_finished_enum.未结束;
                    }

                    lSql.Add(p_cencai.InsertOrUpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai
                {
                    //多个主播user_sn
                    public string zbers { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 成才表单新轮次
            /// </summary>
            public class CencaiNewRoundEdit
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
                    var old_p_cencai = DoMySql.FindEntityById<ModelDb.p_cencai>(req.id);
                    string c_date = DateTime.Now.ToString("yyyy-MM-dd");
                    if (!old_p_cencai.IsNullOrEmpty())
                    {
                        c_date = old_p_cencai.c_date.ToDateTime().ToString("yyyy-MM-dd");
                    }
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("round")
                    {
                        title = "轮次",
                        defaultValue = req.round.ToString(),
                    });
                    formDisplay.formItems.Add(new EmtHidden("tger_sn")
                    {
                        title = "厅管",
                        defaultValue = old_p_cencai.tger_sn,
                    });
                    formDisplay.formItems.Add(new EmtHidden("zber_sn")
                    {
                        title = "主播",
                        defaultValue = old_p_cencai.zber_sn,
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("teacher")
                    {
                        title = "师父名字",
                        defaultValue = old_p_cencai.teacher,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "开始日期",
                        defaultValue = c_date,
                        mold = ModelBasic.EmtTimeSelect.Mold.date,
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("days")
                    {
                        title = "考核天数",
                        defaultValue = old_p_cencai.days.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_lx")
                    {
                        title = "拉新目标",
                        defaultValue = old_p_cencai.target_lx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_erx")
                    {
                        title = "二消目标",
                        defaultValue = old_p_cencai.target_erx.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_jl")
                    {
                        title = "建联目标",
                        defaultValue = old_p_cencai.target_jl.ToNullableString(),
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("target_yl")
                    {
                        title = "音浪目标",
                        defaultValue = old_p_cencai.target_yl.ToNullableString(),
                        colLength = 12
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// p_cencai_id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 厅管user_sn
                    /// </summary>
                    public string tg_user_sn { get; set; }

                    /// <summary>
                    /// 运营sn
                    /// </summary>
                    public string yy_user_sn { get; set; }

                    /// <summary>
                    /// 主播sn
                    /// </summary>
                    public string zb_user_sn { get; set; }

                    /// <summary>
                    /// 轮次
                    /// </summary>
                    public int round { get; set; }
                }
                #endregion
                #region 新增轮次成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai = req.data_json.ToModel<ModelDb.p_cencai>();

                    //1.旧轮次结束
                    var old_p_cencai = DoMySql.FindEntityById<ModelDb.p_cencai>(p_cencai.id);
                    if (old_p_cencai.id > 0)
                    {
                        old_p_cencai.is_finished = (byte)ModelDb.p_cencai.is_finished_enum.已结束;
                        lSql.Add(old_p_cencai.UpdateTran());
                    }
                    else
                    {
                        throw new Exception("当前轮次已不存在");
                    }
                    //2.新轮次添加

                    var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{p_cencai.zber_sn}'",false);

                    p_cencai.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_cencai.is_finished = (int)ModelDb.p_cencai.is_finished_enum.未结束;
                    p_cencai.join_date = user_info_zb.qun_time;
                    p_cencai.MBTI = user_info_zb.mbti;
                    p_cencai.zb_status = user_info_zb.status;
                    lSql.Add(p_cencai.InsertTran());

                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai
                {

                }
                #endregion
            }

            /// <summary>
            /// 编辑成才PK表单
            /// </summary>
            public class CencaiPKPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.buttonGroup = GetButtonGroup(pageModel, req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction
                    };
                    return pageModel;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(PagePost page, DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_cencai_pk = DoMySql.FindEntity<ModelDb.p_cencai_pk>($"id = '{req.id}'", false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_cencai_pk.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("u_type")
                    {
                        defaultValue = req.u_type.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_sn")
                    {
                        defaultValue = req.user_sn.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                    {
                        title = "pk名称",
                        defaultValue = p_cencai_pk.name,
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// p_cencai_pk_sn
                    /// </summary>
                    public string pk_sn { get; set; }

                    /// <summary>
                    /// p_cencai_pk_id
                    /// </summary>
                    public int id { get; set; }

                    /// <summary>
                    /// 创建人类型
                    /// </summary>
                    public int u_type { get; set; }

                    /// <summary>
                    /// 创建人
                    /// </summary>
                    public string user_sn { get; set; }
                }

                #endregion
                #region 新增成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai_pk = req.data_json.ToModel<ModelDb.p_cencai_pk>();

                    if (p_cencai_pk.id == 0)
                    {
                        p_cencai_pk.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_cencai_pk.pk_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    }
                    p_cencai_pk.InsertOrUpdate();

                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai_pk
                {
                }
                #endregion
            }

            /// <summary>
            /// 成才PK列表
            /// </summary>
            public class CencaiPKList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("user_sn")
                    {
                        placeholder = "选择创建人",
                        defaultValue = req.user_sn,
                        options = DoMySql.FindKvList<ModelDb.p_cencai_pk>(fields: "user_sn,user_sn"),
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "新增PK",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = "Post",
                        }
                    });
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
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = false;
                    listDisplay.isOpenCheckBox = false;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    /*
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("id")
                    {
                        text = "id",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("pk_sn")
                    {
                        text = "pk_sn",
                    });*/
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                    {
                        text = "pk名称",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("creater")
                    {
                        text = "创建人",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "180",
                        minWidth = "180",
                    });
                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量删除",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = DeletesAction,
                             },
                        }
                    }
                    });
                    #endregion
                    #region 3.操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "编辑",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            field_paras = "id",
                            url = "Post"
                        },
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "主播列表",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            field_paras = "pk_sn",
                            url = "ItemList"
                        },
                    });
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                        style = "",
                        text = "查看数据",
                        eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                        {
                            field_paras = "pk_sn",
                            url = "PKDataList"
                        },
                    });
                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    /// <summary>
                    /// cencai_pk_sn
                    /// </summary>
                    public string pk_sn { get; set; }

                    /// <summary>
                    /// 创建人
                    /// </summary>
                    public string user_sn { get; set; }

                    /// <summary>
                    /// 创建人类型
                    /// </summary>
                    public int u_type { get; set; }
                }
                #endregion
                #region ListData

                /// <summary>
                /// 菜品表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"1=1";
                    if (!req["user_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (user_sn = '{req["user_sn"]}')";
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_cencai_pk, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public string id { get; set; }
                    public string status { get; set; }
                    public string parent_id { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_cencai_pk
                {
                    public string creater
                    {
                        get
                        {
                            var user_sn = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_cencai_pk>($"pk_sn = '{this.pk_sn}'", false).user_sn;
                            return new DomainBasic.UserApp().GetInfoByUserSn(user_sn).username;
                        }
                    }
                    public string creater_utype
                    {
                        get
                        {
                            var utype = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_cencai_pk>($"pk_sn = '{this.pk_sn}'", false).u_type;
                            return utype != null ? ((ModelDb.p_cencai_pk.u_type_enum)utype).ToString() : "";
                        }
                    }
                }
                #endregion
                #region 删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai_pk_item = req.data_json.ToModel<ModelDb.p_cencai_pk_item>();
                    lSql.Add(p_cencai_pk_item.DeleteTran($"id in ({p_cencai_pk_item.id})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                #endregion
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai_pk_item = new ModelDb.p_cencai_pk_item();
                    lSql.Add(p_cencai_pk_item.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_cencai_pk_item
                {
                    public string ids { get; set; }
                }
                #endregion
            }


            /// <summary>
            /// 成才PKItem列表
            /// </summary>
            public class CencaiPKItemList
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
                    listFilter.formItems.Add(new ModelBasic.EmtSelect("yyer_sn")
                    {
                        placeholder = "选择运营账号",
                        defaultValue = req.yyer_sn,
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtHidden("pk_sn")
                    {
                        defaultValue = req.pk_sn,
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
                    buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("post")
                    {
                        text = "添加",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"ItemPost?pk_sn={req.pk_sn}",
                        },
                    });
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
                    listDisplay.operateWidth = "220";
                    listDisplay.isOpenNumbers = true;
                    listDisplay.isOpenCheckBox = true;
                    listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                    {
                        funcGetListData = GetListData
                    };
                    #region 1.显示列
                    /*
                     listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("id")
                    {
                        text = "id",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("pk_sn")
                    {
                        text = "pk_sn",
                    });
                     */
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yyer_username")
                    {
                        text = "运营",
                        width = "180",
                        minWidth = "180",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tger_username")
                    {
                        text = "厅管",
                        width = "280",
                        minWidth = "280",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("zber_username")
                    {
                        text = "主播",
                        width = "120",
                        minWidth = "120",
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time")
                    {
                        text = "创建时间",
                        width = "120",
                        minWidth = "120",
                    });
                    #endregion
                    #region 2.批量操作列
                    listDisplay.listBatchItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                    {
                        text = "批量操作",
                        buttonItems = new List<ModelBasic.EmtModel.ButtonItem>
                    {
                        new ModelBasic.EmtModel.ButtonItem("")
                        {
                            text = "批量删除",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.请求处理_回调cs函数,
                            eventCsAction = new ModelBasic.EmtModel.ButtonItem.EventCsAction
                            {
                                func = DeletesAction,
                             },
                        }
                    }
                    });
                    #endregion
                    #region 3.操作列
                    listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                    {
                        actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                        style = "",
                        text = "删除",
                        eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                        {
                            func = DelAction,
                            field_paras = "id",
                        },
                    });

                    #endregion
                    return listDisplay;
                }
                /// <summary>
                /// 请求参数对象
                /// </summary>
                public class DtoReq
                {
                    /// <summary>
                    /// cencai_pk_sn
                    /// </summary>
                    public string pk_sn { get; set; }

                    /// <summary>
                    /// 运营sn
                    /// </summary>
                    public string yyer_sn { get; set; }
                }
                #endregion
                #region ListData

                /// <summary>
                /// 菜品表data查询
                /// </summary>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $"1=1";
                    if (!req["yyer_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (yyer_sn = '{req["yyer_sn"]}')";
                    if (!req["pk_sn"].ToNullableString().IsNullOrEmpty()) where += $" AND (pk_sn = '{req["pk_sn"]}')";
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by id desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_cencai_pk_item, ItemDataModel>(filter, reqJson);
                }
                /// <summary>
                /// 自定义筛选参数（自定义数据，与属性对应）
                /// </summary>
                public class DtoReqListData : ModelBasic.PageList.ListData.Req
                {
                    /// <summary>
                    /// 
                    /// </summary>
                    public string name { get; set; }
                    public string id { get; set; }
                    public string status { get; set; }
                    public string parent_id { get; set; }

                }
                /// <summary>
                /// 数据项模型
                /// </summary>
                public class ItemDataModel : ModelDb.p_cencai_pk_item
                {
                    public string yyer_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.yyer_sn).username;
                        }
                    }
                    public string tger_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.tger_sn).username;
                        }
                    }
                    public string zber_username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.zber_sn).username;
                        }
                    }
                }
                #endregion
                #region 删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DelAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai_pk_item = req.data_json.ToModel<ModelDb.p_cencai_pk_item>();
                    lSql.Add(p_cencai_pk_item.DeleteTran($"id in ({p_cencai_pk_item.id})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                #endregion
                #region 批量删除操作
                /// <summary>
                /// 链接提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction DeletesAction(JsonRequestAction req)
                {
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var p_cencai_pk_item = new ModelDb.p_cencai_pk_item();
                    lSql.Add(p_cencai_pk_item.DeleteTran($"id in ({dtoReqData.ids})"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                public class DtoReqData : ModelDb.p_cencai_pk_item
                {
                    public string ids { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 编辑成才Item表单
            /// </summary>
            public class CencaiPKItemPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.buttonGroup = GetButtonGroup(pageModel, req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction
                    };
                    return pageModel;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(PagePost page, DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    //var p_cencai_pk = DoMySql.FindEntityById<ModelDb.p_cencai_pk>(req.pk_sn);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("pk_sn")
                    {
                        defaultValue = req.pk_sn.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new EmtSelect("yyer_sn")
                    {
                        title = "运营",
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'", "username,user_sn"),
                        defaultValue = req.yy_user_sn,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = $"window.location.href='/cencai/pk/itempost?pk_sn={req.pk_sn}&yy_user_sn='+page_post.yyer_sn.value",
                            }
                        },
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("tger_sn")
                    {
                        title = "厅管",
                        defaultValue = req.tg_user_sn,
                        options = DoMySql.FindKvList<ModelDb.user_base>($"user_sn in {new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, req.yy_user_sn)}", "username,user_sn"),
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = $"window.location.href='/cencai/pk/itempost?pk_sn={req.pk_sn}&yy_user_sn=' + page_post.yyer_sn.value + '&tg_user_sn=' + page_post.tger_sn.value",
                            }
                        },
                        colLength = 12
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("zbers")
                    {
                        title = "主播",
                        defaultValue = "",
                        bindOptions = new DomainUserBasic.UserRelationApp().GetNextUsersForOption(ModelEnum.UserRelationTypeEnum.厅管邀主播, req.tg_user_sn),
                        colLength = 12
                    });
                    #endregion
                    return formDisplay;
                }
                public class DtoReq
                {
                    /// <summary>
                    /// p_cencai_pk_sn
                    /// </summary>
                    public string pk_sn { get; set; }

                    /// <summary>
                    /// 厅管sn
                    /// </summary>
                    public string tg_user_sn { get; set; }

                    /// <summary>
                    /// 运营sn
                    /// </summary>
                    public string yy_user_sn { get; set; }
                }

                #endregion
                #region 新增成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    List<string> lSql = new List<string>();
                    var dto = req.data_json.ToModel<DtoReqData>();

                    //1.校验
                    if (dto.yyer_sn.IsNullOrEmpty()) throw new WeicodeException("请选择运营!");
                    if (dto.tger_sn.IsNullOrEmpty()) throw new WeicodeException("请选择厅管!");
                    if (dto.zbers.IsNullOrEmpty()) throw new WeicodeException("请选择主播!");
                    if (dto.pk_sn.IsNullOrEmpty()) throw new WeicodeException("pk_sn不存在!");

                    //2.添加p_cencai_pk_item
                    var zbers = req.GetPara("zbers").Split(',').ToList();
                    var p_cencai_pk_item = dto.ToModel<ModelDb.p_cencai_pk_item>();
                    p_cencai_pk_item.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    foreach (var zb in zbers)
                    {
                        p_cencai_pk_item.zber_sn = zb;
                        lSql.Add(p_cencai_pk_item.InsertOrUpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai_pk_item
                {
                    //多个主播user_sn
                    public string zbers { get; set; }

                    public string yyer_sn { get; set; }

                    public string tger_sn { get; set; }
                }
                #endregion
            }


            /// <summary>
            /// 编辑成才Item表单
            /// </summary>
            public class SettingPost
            {
                #region DefaultView
                public ModelBasic.PagePost Get(DtoReq req)
                {
                    var pageModel = new ModelBasic.PagePost("post");
                    pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                    {
                        returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                    };
                    pageModel.buttonGroup = GetButtonGroup(pageModel, req);
                    pageModel.formDisplay = GetFormDisplay(pageModel, req);
                    pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                    {
                        func = PostAction
                    };
                    return pageModel;
                }

                /// <summary>
                /// 设置扩展的按钮组
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.EmtButtonGroup GetButtonGroup(PagePost page, DtoReq req = null)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
                    return buttonGroup;
                }
                /// <summary>
                /// 配置表单元素控件
                /// </summary>
                /// <param name="req"></param>
                /// <returns></returns>
                public ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
                {
                    var formDisplay = pageModel.formDisplay;
                    var p_cencai_setting = DoMySql.FindEntity<ModelDb.p_cencai_setting>($"user_sn='{req.user_sn}'",false);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_cencai_setting.id.ToNullableString(),
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("teacher")
                    {
                        title="显示师父",
                        options=new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id>0 ? p_cencai_setting.teacher.ToInt().ToNullableString(): "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("zber_name")
                    {
                        title = "显示昵称",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.zber_name.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("join_date")
                    {
                        title = "显示入职日期",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.join_date.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("role")
                    {
                        title = "显示角色",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.role.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("MBTI")
                    {
                        title = "显示MBTI",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.MBTI.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("age")
                    {
                        title = "显示年龄",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.age.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("education")
                    {
                        title = "显示学历",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.education.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("zb_status")
                    {
                        title = "显示状态",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.zb_status.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("round")
                    {
                        title = "显示考核阶段",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.round.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("days")
                    {
                        title = "显示考核天数",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.days.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("live_days_total")
                    {
                        title = "显示总直播天数",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.live_days_total.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("vacation_days")
                    {
                        title = "显示请假天数",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.vacation_days.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("live_days")
                    {
                        title = "显示实际直播天数",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.live_days.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("new_num")
                    {
                        title = "显示拉新",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.new_num.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("amount_2")
                    {
                        title = "显示二消",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.amount_2.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("contact_num")
                    {
                        title = "显示建联",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.contact_num.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("new_num_avg")
                    {
                        title = "显示平均拉新数",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.new_num_avg.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("amount_2_rate")
                    {
                        title = "显示二消率",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.amount_2_rate.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("contact_num_rate")
                    {
                        title = "显示建联率",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.contact_num_rate.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("time_progress")
                    {
                        title = "显示时间进度",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.time_progress.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("new_2_amount")
                    {
                        title = "显示拉新二消流水",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.new_2_amount.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("old_amount")
                    {
                        title = "显示老用户流水",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.old_amount.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("activity_amount")
                    {
                        title = "显示活动流水",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.activity_amount.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("amount")
                    {
                        title = "显示流水累计",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.amount.ToInt().ToNullableString() : "1",
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtRadio("amount_progress")
                    {
                        title = "显示流水进度",
                        options = new Dictionary<string, string>
                        {
                            {"是","1"},
                            {"否","0"},
                        },
                        defaultValue = p_cencai_setting.id > 0 ? p_cencai_setting.amount_progress.ToInt().ToNullableString() : "1",
                    });
                    
                    #endregion
                    return formDisplay;
                }
                public class DtoReq:ModelDb.p_cencai_setting
                {

                }

                #endregion
                #region 新增成才表单
                /// <summary>
                /// 表单提交处理的回调函数
                /// </summary>
                /// <param name="req">回调函数提交参数统一的封装对象</param>
                /// <returns></returns>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    //todo:继续完成成才表字段设置
                    var result = new JsonResultAction();

                    var dto = req.data_json.ToModel<ModelDb.p_cencai_setting>();
                    dto.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    dto.user_sn = new UserIdentityBag().user_sn;
                    //1.校验
                    //2.更新/插入
                    dto.InsertOrUpdate($"user_sn='{dto.user_sn}'");
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_cencai_setting
                {
                    //多个主播user_sn
                    public string zbers { get; set; }

                    public string yyer_sn { get; set; }

                    public string tger_sn { get; set; }
                }
                #endregion
            }
        }
        
    }
}
