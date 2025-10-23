using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.UserInfo;
using static Services.Project.ServiceFactory.UserService;
using static WeiCode.ModelDbs.ModelDb;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class ProjectBasic
        {

            #region 数据模型

            #endregion

            public class DataRule
            {

                #region 查询
                /// <summary>
                /// 返回所有规则模块
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public List<projectbasic_data_rule_type> GetRuleTypeList(PageFactory.ProjectBasic.DataRule.DtoReq reqJson)
                {
                    string where = " 1=1 ";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where
                    };
                    return DoMySql.FindList<projectbasic_data_rule_type>(where);
                }
                /// <summary>
                /// 获取模块下规则
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {
                    var req = reqJson.GetPara();
                    string where = $" 1=1 ";
                    if (!string.IsNullOrEmpty(req["id"].ToString()))
                    {
                        where += $" AND type_id = '{req["id"].ToString()}'";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " id desc"
                    };
                    var data = new ModelBasic.CtlListDisplay.ListData().getList<projectbasic_data_rule, projectbasic_data_rule>(filter, reqJson);
                    List<projectbasic_data_rule> list = data.data as List<projectbasic_data_rule>;
                    data.data = list.GroupBy(a => a.field_key).Select(b => b.First()).ToList();
                    return data;
                }
                /// <summary>
                /// 根据type_id和field_key获取规则列表
                /// </summary>
                /// <param name="type_id"></param>
                /// <param name="field_key"></param>
                /// <returns></returns>
                public List<projectbasic_data_rule> GetRuleList(string type_id, string field_key)
                {
                    string where = " 1=1 ";
                    where += $" AND  field_key = '{field_key}' AND type_id = '{type_id}'";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " id desc"
                    };
                    return DoMySql.FindList<projectbasic_data_rule>(filter);
                }
                /// <summary>
                /// 更具模块名称查询规则id
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public projectbasic_data_rule_type GetRuleTypeId(string name)
                {
                    return DoMySql.FindEntity<projectbasic_data_rule_type>($" name = '{name}'");
                }
                public List<projectbasic_data_rule> GetRuleList(string type_id)
                {
                    string where = " 1=1 ";
                    where += $" AND type_id = '{type_id}'";
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        where = where,
                        orderby = " id desc"
                    };
                    return DoMySql.FindList<projectbasic_data_rule>(filter);
                }
                #endregion

                #region 增删改
                /// <summary>
                /// 删除规则
                /// </summary>
                /// <param name="id"></param>
                public void DeleteRule(int id)
                {
                    if (id == 0)
                    {
                        throw new Exception("请选择正确的规则！");
                    }

                    //查询规则是否存在

                    var rule = DoMySql.FindEntity<projectbasic_data_rule>($"id = '{id}'");

                    if (rule == null)
                    {
                        throw new Exception("没有找到此规则!");
                    }
                    //至少保留一个规则

                    var rules = DoMySql.FindList<projectbasic_data_rule>($" type_id = '{rule.type_id}' AND field_key = '{rule.field_key}'");
                    if (rules.Count < 2)
                    {
                        throw new Exception("至少保留一个规则!");
                    }

                    rule.Delete();
                }
                /// <summary>
                /// 新增或者修改规则
                /// </summary>
                /// <param name="rule"></param>
                public void AddOrEditRule(projectbasic_data_rule rule)
                {
                    if (rule.min_value == null)
                    {
                        throw new Exception("最小值不能为空！");
                    }
                    if (rule.max_value == null)
                    {
                        throw new Exception("最大值不能为空！");
                    }
                    if (rule.max_value < rule.min_value)
                    {
                        throw new Exception("最大值不能小于最小值！");
                    }
                    //修改rule
                    if (rule.id > 0)
                    {
                        rule.Update();
                    }
                    else
                    {
                        rule.create_time = DateTime.Now;
                        rule.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        rule.Insert();
                    }
                }

                #endregion


                #region 过滤数据

                /// <summary>
                /// 规则数据转换
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="type_name"></param>
                /// <param name="list"></param>
                /// <returns></returns>
                public List<T> GetRuleData<T>(string type_name, List<T> listData) where T : projectbasic_data_rule, new()
                {
                    //查询规则模块
                    var type = GetRuleTypeId(type_name);
                    //查询规则
                    var rules = GetRuleList(type.id.ToString());
                    T t = new T();
                    //遍历数据
                    foreach (var data in listData)
                    {
                        //反射获取属性
                        foreach (var pro in data.GetType().GetProperties())
                        {
                            //匹配属性和规则字段
                            var rule = rules.Where(a => a.field_key == pro.Name).ToList();

                            if (rule != null)
                            {
                                foreach (var item in rule)
                                {
                                    if (item.field_key == pro.Name)
                                    {

                                        var value = pro.GetValue(data);
                                        if (value.ToDecimal() > item.min_value && value.ToDecimal() < item.max_value)
                                        {
                                            data.font_color = item.font_color;
                                            data.font_size = item.font_size;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return listData;
                }


                #endregion
            }
        }
    }
}
