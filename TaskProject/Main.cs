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
        /// 主播三天未提交封禁
        /// </summary>
        /// <returns></returns>
        public string CheckZbReport()
        {
            var lSql = new List<string>();
            foreach (var item in DoMySql.FindList<ModelDb.user_base>($"tenant_id='1' and user_type_id = '{new DomainBasic.UserTypeApp().GetInfoByCode("zber").id}' and status = '0' and create_time<'{DateTime.Today.AddDays(-3)}'"))
            {
                if (DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{item.user_sn}' and create_time>='{DateTime.Now.AddDays(-3)}' and create_time<='{DateTime.Now}'").Count == 0)
                {
                    item.status = 9;
                    item.modify_time = DateTime.Now;
                }
                lSql.Add(item.UpdateTran());
            }
            MysqlHelper.ExecuteSqlTran(lSql);

            return "";
        }

        /// <summary>
        /// 3天未提交日报封厅管账号
        /// </summary>
        /// <returns></returns>
        public string CheckTgReport()
        {
            var lSql = new List<string>();
            foreach (var item in DoMySql.FindList<ModelDb.user_base>($"tenant_id='1' and user_type_id = '{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}' and status = '0'"))
            {
                if (DoMySql.FindList<ModelDb.p_jixiao_day>($"tg_user_sn='{item.user_sn}' and create_time>='{DateTime.Now.AddDays(-6)}' and create_time<='{DateTime.Now.AddDays(-1)}'").Count == 0)
                {
                    item.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                    item.modify_time = DateTime.Now;
                }
                lSql.Add(item.UpdateTran());
            }
            MysqlHelper.ExecuteSqlTran(lSql);

            return "";
        }

        /// <summary>
        /// 根据绩效定时任务统计各厅节奏等级(每日)
        /// </summary>
        /// <returns></returns>
        public string Jiezou()
        {
            //获取执行日期
            DateTime startDate = DateTime.Today.AddDays(-3);//开始日期 Convert.ToDateTime("2025-09-10");  //
            DateTime endDate = DateTime.Today.AddDays(-1);//结束日期 Convert.ToDateTime("2025-10-01");    //

            // 获取日期范围
            List<DateTime> dateList = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                                .Select(days => startDate.AddDays(days))
                                                .ToList();

            var ruleList = DoMySql.FindList<jiezou_detail_rule>($"type = '{jiezou_detail_rule.type_enum.日节奏统计规则.ToSByte()}'");

            // 获取所有活动日期
            var activeDates = DoMySql.FindListBySql<jiezou_huodongri>("select hd_date from jiezou_huodongri").Select(e => e.hd_date).ToList();
            foreach (var dateitem in dateList)
            {
                string today = dateitem.ToString("yyyy-MM-dd");
                try
                {
                    //查询现有厅
                    foreach (var item in new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()))
                    {
                        //按厅抽取人均拉新、人均直播场次、拉新总数、建联总数、二消总数
                        var jixiao = DoMySql.FindListBySql<ServiceFactory.JixiaoTable>($"SELECT avg(new_num) AS new_num_avg,sum(new_num) AS new_num_sum, sum(contact_num) AS contact_num_sum, sum(num_2) AS num_2_sum,sum(amount) as amount_sum,count(*) as zb_num   FROM p_jixiao_day WHERE zb_user_sn IN ( SELECT zb_user_sn FROM p_jixiao_day_session WHERE ting_sn = '{item.ting_sn}' AND c_date = '{today}' AND is_rest != 1 GROUP BY zb_user_sn ) and c_date = '{today}' and is_newer = '{p_jixiao_day.is_newer_enum.否.ToInt()}'");
                        decimal renjun = Convert.ToDecimal(jixiao[0].new_num_avg);
                        decimal amount = Convert.ToDecimal(jixiao[0].amount_sum);
                        decimal new_num_sum = Convert.ToDecimal(jixiao[0].new_num_sum);
                        decimal zb_num = Convert.ToDecimal(jixiao[0].zb_num);
                        //厅下当日开档数量
                        var dangweiObject = DoMySql.FindListBySql<ServiceFactory.JixiaoTable>($"select count(*) as session_count_avg from(SELECT session FROM p_jixiao_day_session WHERE ting_sn = '{item.ting_sn}' AND c_date = '{today}' AND is_rest != 1 group by session) dangwei");
                        decimal dangwei = 0;
                        if (!dangweiObject.IsNullOrEmpty())
                        {
                            dangwei = Convert.ToDecimal(dangweiObject[0].session_count_avg);
                        }
                        //建联率 = （建联数量 / 主播人数*1.5） *100
                        decimal jll;
                        //二消率 = （二消数量 / 主播人数*1.5） *100
                        decimal exl;
                        if (new_num_sum != 0)
                        {
                            jll = (Convert.ToDecimal(jixiao[0].contact_num_sum) / (zb_num * 1.5m) * 100).ToFixed(2);
                            exl = (Convert.ToDecimal(jixiao[0].num_2_sum) / (zb_num * 1.5m) * 100).ToFixed(2);
                        }
                        else
                        {
                            jll = 0;
                            exl = 0;
                        }

                        string jiezoujd = "0.5";
                        foreach (var rule in ruleList)
                        {
                            if (rule != null &&
                               renjun >= rule.new_num_rule &&
                               jll >= rule.contact_num_rule &&
                               exl >= rule.num_2_rule &&
                               dangwei >= rule.dangwei_rule &&
                               amount >= rule.amount_rule)
                            {
                                jiezoujd = rule.key;
                            }
                        }

                        var _jiezou_detail = DoMySql.FindEntity<jiezou_detail>($"ting_sn = '{item.ting_sn}' AND data_time = '{today}'", false);
                        _jiezou_detail.tenant_id = 1;
                        _jiezou_detail.yy_user_sn = item.yy_user_sn;
                        _jiezou_detail.tg_user_sn = item.tg_user_sn;
                        _jiezou_detail.ting_sn = item.ting_sn;
                        _jiezou_detail.step = Convert.ToDecimal(jiezoujd);
                        _jiezou_detail.data_time = DateTime.Parse(today);
                        _jiezou_detail.new_num_avg = renjun;
                        _jiezou_detail.session_count_avg = dangwei;
                        _jiezou_detail.jll = jll;
                        _jiezou_detail.exl = exl;
                        _jiezou_detail.zb_num = zb_num.ToInt();
                        _jiezou_detail.is_new_num_avg = (sbyte?)(renjun >= 1.5m ? 1 : 0);//人均拉新是否达标
                        _jiezou_detail.is_session_count_avg = (sbyte?)(dangwei >= 4 ? 1 : 0);//开档数量是否达标
                        _jiezou_detail.is_jll = (sbyte?)(jll >= 40 ? 1 : 0);//建联率是否达标
                        _jiezou_detail.is_exl = (sbyte?)(exl >= 30 ? 1 : 0);//二消率是否达标                                                                                              
                        _jiezou_detail.is_active = (sbyte?)(activeDates.Contains(DateTime.Parse(today).Date) ? 1 : 0);
                        _jiezou_detail.modify_time = DateTime.Now;

                        // 判断是否满足月度达标条件
                        _jiezou_detail.is_month_pass = (sbyte?)((_jiezou_detail.is_new_num_avg == 1
                            && _jiezou_detail.is_session_count_avg == 1
                            && _jiezou_detail.is_jll == 1
                            && _jiezou_detail.is_exl == 1
                            && _jiezou_detail.is_active == 0) ? 1 : 0);
                        //月度考核过程明细备注new_num_sum
                        _jiezou_detail.month_pass_demo = $"{item.ting_name}直播厅" +
                                                        $"人均拉新: {renjun} (拉新总数{new_num_sum}/主播数{zb_num}，{(_jiezou_detail.is_new_num_avg == 1 ? ModelDb.jiezou_detail.is_active_enum.是.ToSByte() : ModelDb.jiezou_detail.is_active_enum.否.ToSByte())}, 标准≥1.5), " +
                                                        $"开档数量: {dangwei} ({(_jiezou_detail.is_session_count_avg == 1 ? ModelDb.jiezou_detail.is_session_count_avg_enum.是.ToSByte() : ModelDb.jiezou_detail.is_session_count_avg_enum.否.ToSByte())}, 标准≥3), " +
                                                        $"建联率: {jll}% (建联总数{jixiao[0].contact_num_sum}/(主播数{zb_num}×1.5)×100，{(_jiezou_detail.is_jll == 1 ? ModelDb.jiezou_detail.is_jll_enum.是.ToSByte() : ModelDb.jiezou_detail.is_jll_enum.否.ToSByte())}, 标准≥40), " +
                                                        $"二消率: {exl}% (二消总数{jixiao[0].num_2_sum}/(主播数{zb_num}×1.5)×100，{(_jiezou_detail.is_exl == 1 ? ModelDb.jiezou_detail.is_exl_enum.是.ToSByte() : ModelDb.jiezou_detail.is_exl_enum.否.ToSByte())}, 标准≥30)";
                        //插入_jiezou_detail数据
                        _jiezou_detail.InsertOrUpdate($"ting_sn = '{item.ting_sn}' AND data_time = '{DateTime.Parse(today)}'");

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception e)
                {
                    UtilityStatic.TxtLog.Error("发生异常: " + e.Message);
                }
            }

            return $"根据绩效定时任务统计各厅节奏阶段:{dateList.Count}天";
        }

        /// <summary>
        /// 根据绩效定时任务统计各厅节奏阶段_月度统计
        /// </summary>
        /// <returns></returns>
        public string JiezouMouth()
        {
            //获取当前日期的天数，仅每月5号执行
            if (DateTime.Now.Day != 5)
            {
                return "";
            }

            DateTime? s_date = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-01").ToDate();
            DateTime? e_date = DateTime.Today.ToString("yyyy-MM-01").ToDate().AddDays(-1);
            string jiezou_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
            string name = DateTime.Today.AddMonths(-1).ToString("yyyy-MM");
            //次月5日生成上月运营节奏阶段数据
            new ModelDb.jiezou_detail_mouth_term
            {
                tenant_id = 1,
                jiezou_sn = jiezou_sn,
                name = name,
                s_date = s_date,
                e_date = e_date
            }.InsertOrUpdate($"name = '{name}'");

            // 统计达标厅的数量
            int TdabiaoCount = 0;
            // 先判断时间范围内是否有数据
            var hasData = DoMySql.FindListBySql<jiezou_detail>($"SELECT * FROM jiezou_detail WHERE data_time BETWEEN '{s_date}' AND '{e_date}'");
            if (hasData.Count > 0)
            {
                foreach (var item in new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()))
                {
                    try
                    {
                        // 查询该厅的合格数量
                        var hegeCount = DoMySql.FindListBySql<jiezou_detail>($"SELECT * FROM jiezou_detail WHERE ting_sn = '{item.ting_sn}' AND step >= 4.0 AND data_time BETWEEN '{s_date}' AND '{e_date}'AND is_active=0").Count;

                        // 查询该厅的考核数量
                        var kaoheCount = DoMySql.FindListBySql<jiezou_detail>($"SELECT * FROM jiezou_detail WHERE ting_sn = '{item.ting_sn}' AND is_active = 0 AND data_time BETWEEN '{s_date}' AND '{e_date}'").Count;

                        // 判断是否达标
                        int isStandard = 0;
                        if ((kaoheCount > 0) && ((double)hegeCount / kaoheCount > 0.5))
                        {
                            TdabiaoCount++;
                            isStandard = 1;
                        }

                        var ting = new ServiceFactory.UserInfo.Ting().GetTingBySn(item.ting_sn);

                        // 直接执行插入操作
                        var mouthData = new ModelDb.jiezou_detail_mouth
                        {
                            tenant_id = 1,
                            jiezou_sn = jiezou_sn,
                            yy_user_sn = ting.yy_user_sn,
                            yy_name = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(ting.yy_user_sn).name,
                            tg_user_sn = item.tg_user_sn,
                            tg_name = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(item.tg_user_sn).name,
                            ting_sn = item.ting_sn,
                            ting_name = ting.ting_name,
                            is_standard = isStandard.ToString(),
                        };
                        mouthData.Insert();
                    }
                    catch (Exception e)
                    {
                        UtilityStatic.TxtLog.Error($"处理厅[{item.ting_sn}]数据时发生异常: " + e.Message);
                        // 发生异常继续处理下一个厅
                        continue;
                    }
                }
            }
            else
            {
                TdabiaoCount = 0;
            }

            return $"根据绩效定时任务统计各厅月度是否达标4.0:{TdabiaoCount}个厅";
        }

        /// <summary>
        /// 根据主播的类别、兼职全职统计每日厅下主播人数
        /// </summary>
        /// <returns></returns>
        public string UserPropertyCount()
        {

            var lSql = new List<string>();

            //获取执行日期
            var today = DateTime.Today.AddDays(-1);

            //当前所有厅的列表数据
            var tgList = new ServiceFactory.UserInfo.Tg().GetAllTg();
            try
            {
                foreach (var item in tgList)
                {

                    //统计厅下当天的主播分类人数
                    var levelCount = DoMySql.FindListBySql<ZbPropertyCount>($"select level as property,count(*) as coutnum from user_info_zb where tg_user_sn = '{item.user_sn}' and status = 0 and user_sn != '' group by level");
                    int aNum = 0;
                    int bNum = 0;
                    int cNum = 0;
                    foreach (var level in levelCount)
                    {
                        if (level.property == null) continue;
                        if (level.property.Equals("A"))
                        {
                            aNum = level.coutnum.ToInt();
                        }
                        else if (level.property.Equals("B"))
                        {
                            bNum = level.coutnum.ToInt();
                        }
                        else if (level.property.Equals("C"))
                        {
                            cNum = level.coutnum.ToInt();
                        }
                    }
                    var fullOrPartCount = DoMySql.FindListBySql<ZbPropertyCount>($"select full_or_part as property,count(*) as coutnum from user_info_zb where tg_user_sn = '{item.user_sn}' and status = 0 and user_sn != '' group by full_or_part");
                    int fullNum = 0;
                    int partNum = 0;
                    foreach (var fullOrPart in fullOrPartCount)
                    {
                        if (fullOrPart.property == null) continue;
                        if (fullOrPart.property.Equals("全职"))
                        {
                            fullNum = fullOrPart.coutnum.ToInt();
                        }
                        else if (fullOrPart.property.Equals("兼职"))
                        {
                            partNum = fullOrPart.coutnum.ToInt();
                        }
                    }
                    var zbCount = DoMySql.FindListBySql<ZbPropertyCount>($"select count(*) as coutnum from user_info_zb where tg_user_sn = '{item.user_sn}' and status = 0 and user_sn != ''");
                    //查询所属运营
                    string yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(item.user_sn).yy_sn;
                    //插入jiezou_detail_mouth数据
                    lSql.Add(new ModelDb.user_info_property_tj
                    {
                        tenant_id = 1,
                        yy_user_sn = yy_user_sn,
                        tg_user_sn = item.user_sn,
                        data_time = today,
                        count_a = aNum,
                        count_b = bNum,
                        count_c = cNum,
                        zb_sum = zbCount[0].coutnum.ToInt(),
                        count_full = fullNum,
                        count_part = partNum,
                    }.InsertTran());

                }
            }
            catch (Exception e)
            {
                UtilityStatic.TxtLog.Error("发生异常: " + e.Message);
            }
            MysqlHelper.ExecuteSqlTran(lSql);

            return $"当日统计主播属性的厅数量:{tgList.Count}个厅";
        }

        /// <summary>
        /// 根据绩效定时任务统计各主播节奏阶段
        /// </summary>
        /// <returns></returns>
        public string Jiezou_zhubo()
        {

            //获取执行日期
            DateTime startDate = DateTime.Today.AddDays(-2);//开始日期
            DateTime endDate = DateTime.Today.AddDays(-1);//结束日期

            // 获取日期范围
            List<DateTime> dateList = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                                .Select(days => startDate.AddDays(days))
                                                .ToList();
            var ruleList = DoMySql.FindList<ModelDb.jiezou_zhubo_detail_rule>($"1=1");

            foreach (var dateitem in dateList)
            {
                string today = dateitem.ToString("yyyy-MM-dd");

                try
                {
                    //每天数据提交一次事务
                    var lSql = new List<string>();

                    //按主播抽取绩效数据
                    var jixiao = DoMySql.FindListBySql<ServiceFactory.JixiaoTable>($"SELECT * FROM p_jixiao_day WHERE zb_user_sn IN ( SELECT zb_user_sn FROM p_jixiao_day_session WHERE c_date ='{today}' AND is_rest != 1 GROUP BY zb_user_sn ) and c_date = '{today}' and is_newer = '{ModelDb.p_jixiao_day.is_newer_enum.否.ToInt()}'");
                    //遍历每个主播的数据
                    foreach (var item in jixiao)
                    {
                        decimal new_num_avg = item.new_num.ToDecimal() / item.session_count.ToDecimal();//平均每档拉新
                        decimal contact_num = item.contact_num.ToDecimal();//主播当日总建联
                        decimal num_2 = item.num_2.ToDecimal();//主播当日总二消个数
                        decimal amount_2_avg = item.amount_2.ToDecimal() / item.session_count.ToDecimal();//平均每档二消音浪
                        decimal hx_amount_avg = item.hx_amount.ToDecimal() / item.session_count.ToDecimal();//平均每档回消音浪
                        decimal hdpk_amount = item.hdpk_amount.ToDecimal();//主播当日活动的PK总流水


                        string jiezoujd = "1.0";
                        foreach (var rule in ruleList)
                        {
                            if (rule != null &&
                               new_num_avg >= rule.new_num_avg_rule &&
                               contact_num >= rule.contact_num_rule &&
                               num_2 >= rule.num_2_rule &&
                               amount_2_avg >= rule.amount_2_avg_rule &&
                               hdpk_amount >= rule.hdpk_amount_rule)
                            {
                                jiezoujd = rule.key;
                            }
                        }

                        var jiezou_detail_zhubo = DoMySql.FindEntity<ModelDb.jiezou_zhubo_detail>($"zb_user_sn = '{item.zb_user_sn}' AND data_time ='{today}'", false);
                        jiezou_detail_zhubo.tenant_id = 1;
                        jiezou_detail_zhubo.zb_user_sn = item.zb_user_sn;
                        jiezou_detail_zhubo.ting_sn = item.ting_sn;
                        jiezou_detail_zhubo.tg_user_sn = item.tg_user_sn;
                        jiezou_detail_zhubo.yy_user_sn = item.yy_user_sn;
                        jiezou_detail_zhubo.step = Convert.ToDecimal(jiezoujd);
                        jiezou_detail_zhubo.data_time = DateTime.Parse(today);
                        jiezou_detail_zhubo.new_num_avg = new_num_avg;
                        jiezou_detail_zhubo.contact_num = contact_num;
                        jiezou_detail_zhubo.num_2 = num_2;
                        jiezou_detail_zhubo.amount_2_avg = amount_2_avg;
                        jiezou_detail_zhubo.hx_amount_avg = hx_amount_avg;
                        jiezou_detail_zhubo.hdpk_amount = hdpk_amount;
                        jiezou_detail_zhubo.session_count = item.session_count.ToDecimal();
                        //插入jiezou_detail数据
                        lSql.Add(jiezou_detail_zhubo.InsertOrUpdateTran());
                    }
                    MysqlHelper.ExecuteSqlTran(lSql);
                }
                catch (Exception e)
                {
                    UtilityStatic.TxtLog.Error("发生异常: " + e.Message);
                }
            }

            return $"根据绩效定时任务统计各主播节奏阶段:{dateList.Count}天";
        }


        /// <summary>
        /// 主播属性统计实体类
        /// </summary>
        public class ZbPropertyCount : Entity
        {
            public string property { get; set; }
            public string coutnum { get; set; }
        }

    }
}
