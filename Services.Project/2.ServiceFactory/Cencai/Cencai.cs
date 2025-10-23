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
        public partial class Cencai 
        {
            public class CencaiService
            {

                public CencaiService()
                {
                    
                }


                /// <summary>
                /// 编辑主播成才表
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public static JsonResultAction Edit(JsonRequestAction reqJson)
                {
                    var result = new JsonResultAction();
                    
                    var p_cencai = reqJson.GetPara().ToModel<ModelDb.p_cencai>();

                    var user_info_zb =DoMySql.FindEntityById<ModelDb.user_info_zb>(reqJson.GetPara("id").ToInt());
                    user_info_zb.mbti = p_cencai.MBTI;
                    user_info_zb.education = p_cencai.education.ToSByte();
                    user_info_zb.Update();
                    return result;
                }


                /// <summary>
                /// 获取成才合格人数
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <param name="c_date"></param>
                /// <param name="round"></param>
                /// <returns></returns>
                public int getCencaiQualifyNum(string yy_user_sn,string tg_user_sn,string c_date,int round)
                {
                    string where = $"round='{round}'";
                    if (!yy_user_sn.IsNullOrEmpty())
                    {
                        where += $" and tger_sn in ({new ServiceFactory.YyInfoService().YyGetNextTgForSql(yy_user_sn)})";
                    }
                    if (!tg_user_sn.IsNullOrEmpty()) 
                    {
                        where += $" and tger_sn = '{tg_user_sn}'";
                    }
                    var list = DoMySql.FindList<ModelDb.p_cencai>($"{where} order by zber_sn,round");
                    int count = 0;
                    foreach (var item in list)
                    {
                        if (c_date.ToDate() < item.c_date || item.is_finished == (int)ModelDb.p_cencai.is_finished_enum.已结束) { continue; }
                        var p_jixiao_days = DoMySql.FindList<ModelDb.p_jixiao_day>($"zb_user_sn='{item.zber_sn}' and c_date between '{item.c_date.ToDateString( ConvertExt.DateFormate.yyyy_MM_dd)}' and '{c_date.ToDateTime().ToString("yyyy-MM-dd")}'");
                        var sum_yl = p_jixiao_days.Sum(p => (int)p.amount);
                        var live_days = UtilityStatic.DateHelper.DateDiff(c_date.ToDate(), item.c_date,UtilityStatic.DateHelper.DiffType.days) + 1;
                        var target_progress = item.target_yl * 1.0 / item.days * live_days;
                        var yl_progress = item.target_yl == 0 ? 0 : Math.Round(sum_yl * 1.0 / (int)item.target_yl * 100, 2);

                        if(sum_yl >= target_progress && yl_progress != 0) { count++; }
                    }

                    
                    return count;
                }

                /// <summary>
                /// 获取成才合格率
                /// </summary>
                /// <param name="tg_user_sn"></param>
                /// <param name="c_date"></param>
                /// <param name="round"></param>
                /// <returns></returns>
                public double getCencaiQualifyRate(string yy_user_sn,string tg_user_sn, string c_date, int round)
                {
                    string where = $"round='{round}'";
                    if (!yy_user_sn.IsNullOrEmpty())
                    {
                        where += $" and tger_sn in ({new ServiceFactory.YyInfoService().YyGetNextTgForSql(yy_user_sn)})";
                    }
                    if (!tg_user_sn.IsNullOrEmpty())
                    {
                        where += $" and tger_sn = '{tg_user_sn}'";
                    }
                    var list = DoMySql.FindList<ModelDb.p_cencai>($"{where} order by zber_sn,round");
                    if (list.Count==0) { return 0; }
                    return (100 * getCencaiQualifyNum(yy_user_sn, tg_user_sn, c_date, round).ToDouble() / list.FindAll(x=>x.is_finished != (int)ModelDb.p_cencai.is_finished_enum.已结束 && c_date.ToDate() >= x.c_date).Count).ToFixed(2);
                }
            }
        }
    }    

}
