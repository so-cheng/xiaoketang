using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;
using static WeiCode.Models.ModelBasic;

namespace Services.Project
{

    public partial class PageFactory
    {
        public partial class ProjectBasic
        {
            public partial class DataRule
            {

                public class DtoReq
                {
                    public int id { get; set; }

                    public int type_id { get; set; }

                    public string field_key { get; set; }
                }
                /// <summary>
                /// 列表页
                /// </summary>
                public class List
                {

                    #region DefaultView
                    /// <summary>
                    /// 获取页面数据模型
                    /// </summary>
                    /// <returns></returns>
                    public ModelBasic.PageList Get(DtoReq req)
                    {
                        var pageModel = new PageList("pagelist");

                        //查询所有的规则模块
                        ServiceFactory.ProjectBasic.DataRule dataRule = new ServiceFactory.ProjectBasic.DataRule();

                        List<projectbasic_data_rule_type> ruleTypes = dataRule.GetRuleTypeList(req);

                        string top = "";
                        top += $@"<div class=""layui-tab layui-tab-brief"">";
                        top += $@"   <ul class=""layui-tab-title"">";

                        foreach (var item in ruleTypes)
                        {
                            top += $@"       <li {(req.id == item.id ? @"class=""layui-this""" : "")} lay-id=""{item.id}"" onclick=""location.href='?id={item.id}    '"">{item.name}</li>";
                        }
                        top += $@"   </ul>";
                        top += $@"</div>";

                        pageModel.topPartial = new List<ModelBase>
                    {
                        new ModelBasic.EmtHtml("html_top")
                        {
                            Content = top
                        }
                    };
                        pageModel.listDisplay = GetListDisplay(req);
                        return pageModel;
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
                            funcGetListData = GetListData,
                            attachPara = new Dictionary<string, object>() { { "id", req.id } }
                        };
                        #region 1.显示列       

                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("field_name")
                        {
                            text = "字段名称",
                            width = "300",
                            minWidth = "300"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("field_key")
                        {
                            text = "字段",
                            width = "120",
                            minWidth = "120"
                        });

                        #endregion

                        #region 3.操作列
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {

                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                            eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                            {
                                url = $"Edit?type_id={req.id}",
                                field_paras = "field_key",
                                layer_width = "1600",
                            },
                            text = "编辑",
                            name = "Edit",
                        });
                        #endregion
                        return listDisplay;
                    }
                    /// <summary>
                    /// 请求参数对象
                    /// </summary>

                    #endregion

                    #region ListData
                    /// <summary>
                    /// data查询
                    /// </summary>
                    /// <returns></returns>
                    public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                    {
                        ServiceFactory.ProjectBasic.DataRule dataRule = new ServiceFactory.ProjectBasic.DataRule();


                        return dataRule.GetListData(reqJson);
                    }
                    #endregion

                    #region 数据模型
                    public class ItemDataModel : ModelDb.dataanalysis_coredata_ting_rule
                    {

                    }

                    #endregion

                }
            }
        }
    }
}
