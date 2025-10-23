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
    /// Crm类型管理
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public class TypeList
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
                pageModel.listFilter.isExport = false;
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
                return listFilter;
            }
            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("create")
                {
                    title="create",
                    text = "新建类型",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "TypePost",
                        
                    },
                });
                return buttonGroup;
            }
            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 1.显示列
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                {
                    text = "名称",
                    width = "120",
                    minWidth = "120",
                });
                #endregion
                #region 2.批量操作列
                #endregion
                #region 3.操作列
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    style = "",
                    text = "删除"

                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "GradeList",
                        field_paras = "id"
                    },
                    text = "等级管理"

                });
                #endregion
                return listDisplay;
            }
            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
            }
            #endregion
            #region ListData

            /// <summary>
            /// 资产表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                string where = "1=1";
               
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.crm_type, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.crm_type
            {
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 表单提交处理的回调函数
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var crm_type = req.data_json.ToModel<ModelDb.crm_type>();
                var info = new JsonResultAction();
                var lSql = new List<string>();
                lSql.Add(crm_type.DeleteTran());
                DoMySql.ExecuteSqlTran(lSql);
                return info;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>

            #endregion
        }
        /// <summary>
        /// 新增类型
        /// </summary>
        public class TypePost
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
                var crm_type = DoMySql.FindEntityById<ModelDb.crm_type>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                {
                    title = "基本信息",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                {
                    title = "*名称",
                    defaultValue = crm_type.name,
                    colLength = 6
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }

            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增用户
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var crm_type = req.data_json.ToModel<ModelDb.crm_type>();
                crm_type.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                var result = new JsonResultAction();
                if (crm_type.name.IsNullOrEmpty()) throw new WeicodeException("用户昵称不可为空！");
                lSql.Add(crm_type.InsertOrUpdateTran());
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            #endregion
        }
        /// <summary>
        /// 管理等级
        /// </summary>
        public class TypeGrade
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
                pageModel.listFilter.isExport = false;
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
                listFilter.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    placeholder = "parent_id",
                    defaultValue = req.id.ToString()
                });
                return listFilter;
            }
            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("create")
                {
                    title = "create",
                    text = "新建等级",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "CradeCreate?type_id=" + req.id,
                    },
                });
                return buttonGroup;
            }
            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "220";
                listDisplay.isOpenNumbers = true;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                #region 1.显示列
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("name")
                {
                    text = "名称",
                    width = "120",
                    minWidth = "120",
                });
                #endregion
                #region 2.批量操作列
                #endregion
                #region 3.操作列
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    style = "",
                    text = "删除"

                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "CradeCreate",
                        field_paras = "id"
                    },
                    style = "",
                    text = "编辑",
                    name = "Post"

                });
                #endregion
                return listDisplay;
            }
            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq
            {
                public int id { get; set; }
            }
            #endregion
            #region ListData

            /// <summary>
            /// 资产表data查询
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                string where = "1=1";
                var req = reqJson.GetPara();
                if (!req["id"].ToNullableString().IsNullOrEmpty()) where += $" AND (type_id ='{req["id"]}')";
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.crm_grade, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.crm_grade
            {
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 表单提交处理的回调函数
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var crm_grade = req.data_json.ToModel<ModelDb.crm_grade>();
                var info = new JsonResultAction();
                var lSql = new List<string>();
                lSql.Add(crm_grade.DeleteTran());
                DoMySql.ExecuteSqlTran(lSql);
                return info;
            }

            /// <summary>
            /// 定义表单模型
            /// </summary>

            #endregion
        }
        /// <summary>
        /// 设置等级
        /// </summary>
        public class TypeGradeCreate
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
                formDisplay.formItems.Add(new ModelBasic.EmtFieldset("")
                {
                    title = "基本信息",
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id,
                }) ;
                formDisplay.formItems.Add(new ModelBasic.EmtInput("name")
                {
                    title = "*名称",
                    colLength = 6
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("type_id")
                {
                    title = "type_id",
                    defaultValue = req.type_id
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public string id { get; set; } = "0";
                public string type_id { get; set; }

            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 设置用户等级
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var crm_grade = req.data_json.ToModel<ModelDb.crm_grade>();
                crm_grade.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                 var result = new JsonResultAction();
                if (crm_grade.name.IsNullOrEmpty()) throw new WeicodeException("用户昵称不可为空！");
                lSql.Add(crm_grade.InsertOrUpdateTran());
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            #endregion
        }
    }
}