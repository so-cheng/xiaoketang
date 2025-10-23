using Services.Project;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.ModelDbs.ModelDb;

namespace TaskProject
{
    /// <summary>
    /// 类名(固定命名，不能修改)
    /// </summary>
    public partial class ProjectClass
    {
        /// <summary>
        /// 生成每天工作待办明细
        /// </summary>
        /// <returns></returns>
        public string SetWorkTodoDay()
        {
            string c_date = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            foreach (var item in DoMySql.FindList<ModelDb.p_work_todo_rule>($"s_time <= '{c_date}' and e_time >= '{c_date}'"))
            {
                if (!item.c_rule.Equals("每天"))
                {
                    if (!item.c_rule.Contains(":")) continue;
                    var c_rule = item.c_rule.Substring(0, item.c_rule.IndexOf(":"));
                    switch (c_rule)
                    {
                        case "每周":
                            CultureInfo cultureInfo = new CultureInfo("zh-CN");
                            string week = c_date.ToDate().ToString("dddd", cultureInfo);

                            if (!item.c_rule.Contains(week))
                            {
                                continue;
                            }
                            break;
                        case "每月":
                            var day = $",{c_date.ToDate().Day},";

                            var days = item.c_rule.Substring(item.c_rule.IndexOf(":") + 1);
                            if (!$",{days},".Contains(day))
                            {
                                // 判断月最后一天
                                if (!item.c_rule.Contains("最后一天"))
                                {
                                    continue;
                                }
                                else
                                {
                                    // 获取月最后一天
                                    var last_day = DateTime.Now.ToString("yyyy-MM-01").ToDate().AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                                    if (c_date != last_day)
                                    {
                                        continue;
                                    }
                                }
                            }
                            break;
                    }
                }

                //所属中台发起的待办规则
                if (!string.IsNullOrEmpty(item.zt_sn))
                {
                    foreach (var yy in new ServiceFactory.UserInfo.Zt().ZtGetNextYy(item.zt_sn))
                    {
                        new ModelDb.p_work_todo
                        {
                            zt_sn = item.zt_sn,
                            yy_sn = yy.user_sn,
                            tenant_id = item.tenant_id,
                            create_time = c_date.ToDate(),
                            rule_id = item.id,
                            content = item.content,
                            sort = item.sort
                        }.Insert();
                    }
                }
                //所属运营发起的待办规则
                if (!string.IsNullOrEmpty(item.yy_sn))
                {
                    foreach (var tg in new ServiceFactory.UserInfo.Tg().TgGetNextTg(item.yy_sn))
                    {
                        new ModelDb.p_work_todo
                        {

                            yy_sn = tg.user_sn,
                            tg_sn = tg.user_sn,
                            tenant_id = item.tenant_id,
                            create_time = c_date.ToDate(),
                            rule_id = item.id,
                            content = item.content,
                            sort = item.sort
                        }.Insert();
                    }
                }
            }
            return "生成每天工作待办明细";
        }
    }
}
