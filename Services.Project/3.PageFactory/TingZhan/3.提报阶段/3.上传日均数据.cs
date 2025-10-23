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
    /// 日均数据
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            #region 上传日均数据
            /// <summary>
            /// 创建/编辑页面
            /// </summary>
            public class DayPost
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
                        func = PostAction,
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
                    var p_tingzhan = DoMySql.FindEntityById<ModelDb.p_tingzhan>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = p_tingzhan.id.ToNullableString()
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtLabel("c_date")
                    {
                        title = "厅战时间",
                        defaultValue = p_tingzhan.c_date.ToDate().ToString("yyyy-MM-dd")
                    });

                    formDisplay.formItems.Add(new ModelBasic.EmtExcelRead("l_p_tingzhan_target")
                    {
                        title = "",
                        exampleFileUrl = "/UploadFile/file/日均数据模板.xls",
                        colItems = new List<ModelBasic.EmtExcelRead.ColItem>
                        {
                            new ModelBasic.EmtExcelRead.ColItem("dou_username")
                            {
                             title = "抖音账号",
                             edit = "text",
                            },
                            new ModelBasic.EmtExcelRead.ColItem("day_amount")
                            {
                             title = "日均音浪",
                             edit = "text",
                            },
                         },
                        displayStatus = EmtModelBase.DisplayStatus.只读
                    });
                    #endregion
                    return formDisplay;
                }

                public class DtoReq : ModelBasic.PagePost.Req
                {
                    public int id { get; set; }
                }
                #endregion

                #region 异步请求处理
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    var result = new JsonResultAction();
                    int sort = 0;
                    if (dtoReqData.l_p_tingzhan_target.IsNullOrEmpty()) throw new WeicodeException("请上传数据");
                    foreach (var item in dtoReqData.l_p_tingzhan_target)
                    {
                        sort++;

                        var ting_user = DoMySql.FindEntity<ModelDb.user_info_tg>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and dou_user = '{item.dou_username}'", false);
                        if (!ting_user.IsNullOrEmpty())
                        {
                            var p_tingzhan_target = DoMySql.FindEntity<ModelDb.p_tingzhan_target>($"tenant_id = {new DomainBasic.TenantApp().GetInfo().id} and tingzhan_id = {dtoReqData.id} and ting_sn = '{ting_user.ting_sn}'", false);
                            if (!p_tingzhan_target.IsNullOrEmpty())
                            {
                                p_tingzhan_target.day_amount = item.day_amount;
                                lSql.Add(p_tingzhan_target.UpdateTran());
                            }
                        }
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                    //更新对象容器数据
                    return result;
                }
                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData : ModelDb.p_tingzhan_target
                {
                    public List<p_tingzhan_target> l_p_tingzhan_target { get; set; }
                }
                public class p_tingzhan_target : ModelDb.p_tingzhan_target
                {
                    public string dou_username { get; set; }
                }
                #endregion
            }
            #endregion
        }
    }
}
