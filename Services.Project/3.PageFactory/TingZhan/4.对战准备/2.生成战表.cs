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
    /// 生成战表
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 修改对战厅
            /// <summary>
            /// 修改对战厅编辑页面
            /// </summary>
            public class MateChangeTgPost
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
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
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
                    #region 表单元素
                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {req.id}", false);

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("type")
                    {
                        defaultValue = req.type.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn")
                    {
                        title = "直播厅",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                        {
                            attachWhere = $"ting_sn not in (select ting_sn1 from p_tingzhan_mate where tingzhan_id = {p_tingzhan_mate.tingzhan_id}) and ting_sn not in (select ting_sn2 from p_tingzhan_mate where tingzhan_id = {p_tingzhan_mate.tingzhan_id})"
                        })
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                    public int type { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var reqData = req.data_json.ToModel<DtoReqData>();
                    List<string> lSql = new List<string>();

                    // 获取厅信息
                    var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn);

                    // 更新对战厅信息
                    var p_tingzhan_mate = DoMySql.FindEntity<ModelDb.p_tingzhan_mate>($"id = {reqData.id} and tenant_id = {new DomainBasic.TenantApp().GetInfo().id}");
                    switch (reqData.type)
                    {
                        case 1:
                            p_tingzhan_mate.tg_user_sn1 = ting.tg_user_sn;
                            p_tingzhan_mate.ting_sn1 = reqData.ting_sn;
                            break;
                        case 2:
                            p_tingzhan_mate.tg_user_sn2 = ting.tg_user_sn;
                            p_tingzhan_mate.ting_sn2 = reqData.ting_sn;
                            break;
                    }

                    lSql.Add(p_tingzhan_mate.UpdateTran());

                    // 添加厅目标
                    lSql = new ServiceFactory.TingZhanService().AddTargetForSql(p_tingzhan_mate.tingzhan_id, reqData.ting_sn, p_tingzhan_mate.amont, lSql);

                    DoMySql.ExecuteSqlTran(lSql);

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_tingzhan_mate
                {
                    /// <summary>
                    /// 1 左ting_sn1，2 右ting_sn2
                    /// </summary>
                    public int type { get; set; }
                    public string ting_sn { get; set; }
                }

                #endregion
            }
            #endregion

            #region 新增对战关系
            /// <summary>
            /// 新增对战关系
            /// </summary>
            public class AddMatePost
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
                public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
                {
                    var buttonGroup = new ModelBasic.EmtButtonGroup("");
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
                    #region 表单元素
                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn1")
                    {
                        title = "直播厅A",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                        {
                            attachWhere = $"ting_sn not in (select ting_sn1 from p_tingzhan_mate where tingzhan_id = {p_tingzhan.id}) and ting_sn not in (select ting_sn2 from p_tingzhan_mate where tingzhan_id = {p_tingzhan.id})"
                        })
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtSelect("ting_sn2")
                    {
                        title = "直播厅B",
                        options = new ServiceFactory.UserInfo.Ting().GetBaseInfosForKv(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter
                        {
                            attachWhere = $"ting_sn not in (select ting_sn1 from p_tingzhan_mate where tingzhan_id = {p_tingzhan.id}) and ting_sn not in (select ting_sn2 from p_tingzhan_mate where tingzhan_id = {p_tingzhan.id})"
                        })
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amont")
                    {
                        title = "目标音浪",
                        placeholder = "单位(万)"
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {

                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    var result = new JsonResultAction();
                    var reqData = req.data_json.ToModel<DtoReqData>();
                    if (reqData.amont.IsNullOrEmpty()) throw new Exception("请输入目标音浪");
                    List<string> lSql = new List<string>();

                    var p_tingzhan = new ServiceFactory.TingZhanService().getNewTingzhan();

                    var p_tingzhan_mate = new ModelDb.p_tingzhan_mate()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        tingzhan_id = p_tingzhan.id,
                        tg_user_sn1 = new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn1).tg_user_sn,
                        ting_sn1 = reqData.ting_sn1,
                        tg_user_sn2 = new ServiceFactory.UserInfo.Ting().GetTingBySn(reqData.ting_sn2).tg_user_sn,
                        ting_sn2 = reqData.ting_sn2,
                        amont = reqData.amont,
                    };

                    lSql.Add(p_tingzhan_mate.InsertTran());

                    // 添加厅目标
                    lSql = new ServiceFactory.TingZhanService().AddTargetForSql(p_tingzhan.id, reqData.ting_sn1, reqData.amont, lSql);
                    lSql = new ServiceFactory.TingZhanService().AddTargetForSql(p_tingzhan.id, reqData.ting_sn2, reqData.amont, lSql);

                    DoMySql.ExecuteSqlTran(lSql);

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_tingzhan_mate
                {

                }

                #endregion
            }
            #endregion
        }
    }
}
