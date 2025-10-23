using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.ModelDbs;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class TargetService
        {
            /// <summary>
            /// 更改主播的日目标
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction DayTargetAVG(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var date = req["date"].ToNullableString().ToDate();
                var p_jixiao_target = DoMySql.FindEntity<ModelDb.p_jixiao_target>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);
                if (req["type"].ToNullableString() == "amount")
                {
                    var p_jixiao_target_day = DoMySql.FindEntity<ModelDb.p_jixiao_target_day>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_target_day.IsNullOrEmpty() && !p_jixiao_target.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_target_day.GetValue($"amount_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var amount_avg = left_day <= 0 ? 0 : (p_jixiao_target.amount - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        amount_avg = amount_avg < 0 ? 0 : amount_avg;
                        var target_day = new Dictionary<string, string>();
                        target_day.Add("id", p_jixiao_target_day.id.ToString());

                        //修改当天目标值
                        target_day.Add($"amount_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_day.Add($"amount_{i}", amount_avg.ToString());
                        }
                        target_day.ToModel<ModelDb.p_jixiao_target_day>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "new")
                {
                    var p_jixiao_target_new = DoMySql.FindEntity<ModelDb.p_jixiao_target_new>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_target_new.IsNullOrEmpty() && !p_jixiao_target.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_target_new.GetValue($"new_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var new_avg = left_day <= 0 ? 0 : (p_jixiao_target.new_num - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        new_avg = new_avg < 0 ? 0 : new_avg;
                        var target_new = new Dictionary<string, string>();
                        target_new.Add("id", p_jixiao_target_new.id.ToString());

                        //修改当天目标值
                        target_new.Add($"new_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_new.Add($"new_{i}", new_avg.ToString());
                        }
                        target_new.ToModel<ModelDb.p_jixiao_target_new>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "contact")
                {
                    var p_jixiao_target_contact = DoMySql.FindEntity<ModelDb.p_jixiao_target_contact>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_target_contact.IsNullOrEmpty() && !p_jixiao_target.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_target_contact.GetValue($"contact_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var contact_avg = left_day <= 0 ? 0 : (p_jixiao_target.contact_num - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        contact_avg = contact_avg < 0 ? 0 : contact_avg;
                        var target_contact = new Dictionary<string, string>();
                        target_contact.Add("id", p_jixiao_target_contact.id.ToString());

                        //修改当天目标值
                        target_contact.Add($"contact_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_contact.Add($"contact_{i}", contact_avg.ToString());
                        }
                        target_contact.ToModel<ModelDb.p_jixiao_target_contact>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "num2")
                {
                    var p_jixiao_target_num2 = DoMySql.FindEntity<ModelDb.p_jixiao_target_num2>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_target_num2.IsNullOrEmpty() && !p_jixiao_target.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_target_num2.GetValue($"num2_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var num2_avg = left_day <= 0 ? 0 : (p_jixiao_target.num_2 - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        num2_avg = num2_avg < 0 ? 0 : num2_avg;
                        var target_day = new Dictionary<string, string>();
                        target_day.Add("id", p_jixiao_target_num2.id.ToString());

                        //修改当天目标值
                        target_day.Add($"num2_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_day.Add($"num2_{i}", num2_avg.ToString());
                        }
                        target_day.ToModel<ModelDb.p_jixiao_target_num2>().Update();
                    }
                }


                return result;
            }


            /// <summary>
            /// 更改主播的日目标
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction TgDayTargetAVG(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var date = req["date"].ToNullableString().ToDate();
                var p_jixiao_target_tg = DoMySql.FindEntity<ModelDb.p_jixiao_target_tg>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);
                if (req["type"].ToNullableString() == "amount")
                {
                    var p_jixiao_tgtarget_amount = DoMySql.FindEntity<ModelDb.p_jixiao_tgtarget_amount>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_tgtarget_amount.IsNullOrEmpty() && !p_jixiao_target_tg.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_tgtarget_amount.GetValue($"amount_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var amount_avg = left_day <= 0 ? 0 : (p_jixiao_target_tg.amount - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        amount_avg = amount_avg < 0 ? 0 : amount_avg;
                        var target_day = new Dictionary<string, string>();
                        target_day.Add("id", p_jixiao_tgtarget_amount.id.ToString());

                        //修改当天目标值
                        target_day.Add($"amount_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_day.Add($"amount_{i}", amount_avg.ToString());
                        }
                        target_day.ToModel<ModelDb.p_jixiao_tgtarget_amount>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "new")
                {
                    var p_jixiao_tgtarget_new = DoMySql.FindEntity<ModelDb.p_jixiao_tgtarget_new>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_tgtarget_new.IsNullOrEmpty() && !p_jixiao_target_tg.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_tgtarget_new.GetValue($"new_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var new_avg = left_day <= 0 ? 0 : (p_jixiao_target_tg.new_num - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        new_avg = new_avg < 0 ? 0 : new_avg;
                        var target_new = new Dictionary<string, string>();
                        target_new.Add("id", p_jixiao_tgtarget_new.id.ToString());

                        //修改当天目标值
                        target_new.Add($"new_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_new.Add($"new_{i}", new_avg.ToString());
                        }
                        target_new.ToModel<ModelDb.p_jixiao_tgtarget_new>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "contact")
                {
                    var p_jixiao_tgtarget_contact = DoMySql.FindEntity<ModelDb.p_jixiao_tgtarget_contact>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_tgtarget_contact.IsNullOrEmpty() && !p_jixiao_target_tg.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_tgtarget_contact.GetValue($"contact_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var contact_avg = left_day <= 0 ? 0 : (p_jixiao_target_tg.contact_num - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        contact_avg = contact_avg < 0 ? 0 : contact_avg;
                        var target_contact = new Dictionary<string, string>();
                        target_contact.Add("id", p_jixiao_tgtarget_contact.id.ToString());

                        //修改当天目标值
                        target_contact.Add($"contact_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_contact.Add($"contact_{i}", contact_avg.ToString());
                        }
                        target_contact.ToModel<ModelDb.p_jixiao_target_contact>().Update();
                    }
                }

                if (req["type"].ToNullableString() == "num2")
                {
                    var p_jixiao_tgtarget_num2 = DoMySql.FindEntity<ModelDb.p_jixiao_tgtarget_num2>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and yearmonth='{date.ToString("yyyy-MM")}'", false);

                    if (!p_jixiao_tgtarget_num2.IsNullOrEmpty() && !p_jixiao_target_tg.IsNullOrEmpty() && !req["value"].ToNullableString().IsNullOrEmpty())
                    {
                        #region 求和修改日期以前已经结算的目标
                        decimal sum_pass = 0;
                        for (int i = 1; i < date.Day; i++)
                        {
                            sum_pass += p_jixiao_tgtarget_num2.GetValue($"num2_{i}").ToDecimal();
                        }
                        #endregion

                        //修改当天数据之后当月剩余天数
                        var left_day = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

                        //求剩余日期每天平均目标值
                        var num2_avg = left_day <= 0 ? 0 : (p_jixiao_target_tg.num_2 - sum_pass - req["value"].ToNullableString().ToDecimal()) / left_day;
                        num2_avg = num2_avg < 0 ? 0 : num2_avg;
                        var target_day = new Dictionary<string, string>();
                        target_day.Add("id", p_jixiao_tgtarget_num2.id.ToString());

                        //修改当天目标值
                        target_day.Add($"num2_{date.Day}", req["value"].ToNullableString());

                        //为当天之后的每天目标赋值
                        for (int i = date.Day + 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
                        {
                            target_day.Add($"num2_{i}", num2_avg.ToString());
                        }
                        target_day.ToModel<ModelDb.p_jixiao_target_num2>().Update();
                    }
                }
                return result;
            }

            public JsonResultAction EPT(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                
                return result;
            }
        }
    }    
}
