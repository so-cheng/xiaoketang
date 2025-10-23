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
        public class JiezouService
        {
            /// <summary>
            /// 编辑节奏表
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction Edit(JsonRequestAction reqJson)
            {
                var lSql = new List<string>();
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var jiezou_item = DoMySql.FindEntity<ModelDb.jiezou_item>($"tg_user_sn='{req["tg_user_sn"]}' and jiezou_sn='{req["jiezou_sn"]}' and status=0", false);
                if (req["progress"].ToSByte() == 2 && !req["plan"].IsNullOrEmpty() && req["plan"].ToDate() > DateTime.Today)
                {
                    throw new Exception("完成时间不能超过今天");
                }

                DateTime date = UtilityStatic.CommonHelper.GetDbNullTime();

                if (req["step"].ToNullableString()=="1")
                {
                    
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_1 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间
                        jiezou_item.time_plan_2 = date;
                        jiezou_item.time_plan_3 = date;
                        jiezou_item.time_plan_4 = date;
                        jiezou_item.time_plan_5 = date;
                        jiezou_item.time_plan_6 = date;
                        jiezou_item.time_plan_7 = date;

                        //修改所有阶段的完成进度
                        jiezou_item.progress_1 = req["progress"].ToSByte();
                        jiezou_item.progress_2 = 0;
                        jiezou_item.progress_3 = 0;
                        jiezou_item.progress_4 = 0;
                        jiezou_item.progress_5 = 0;
                        jiezou_item.progress_6 = 0;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_1 != 0)
                    {
                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_1 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_1 = req["plan"].ToDate();

                        }
                    }

                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "2")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_2 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_1 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_1 = DateTime.Today;
                        }
                        
                        jiezou_item.time_plan_3 = date;
                        jiezou_item.time_plan_4 = date;
                        jiezou_item.time_plan_5 = date;
                        jiezou_item.time_plan_6 = date;
                        jiezou_item.time_plan_7 = date;

                        //修改所有阶段的完成进度
                        jiezou_item.progress_2 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_3 = 0;
                        jiezou_item.progress_4 = 0;
                        jiezou_item.progress_5 = 0;
                        jiezou_item.progress_6 = 0;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_2 != 0)
                    {
                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_2 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_2 = req["plan"].ToDate();
                        }
                    }

                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "3")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_3 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_2 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_2 = DateTime.Today;
                        }
                        
                        jiezou_item.time_plan_4 = date;
                        jiezou_item.time_plan_5 = date;
                        jiezou_item.time_plan_6 = date;
                        jiezou_item.time_plan_7 = date;

                        //修改所有阶段的完成进度
                        jiezou_item.progress_3 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_2 = 2;
                        jiezou_item.progress_4 = 0;
                        jiezou_item.progress_5 = 0;
                        jiezou_item.progress_6 = 0;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_3 != 0)
                    {
                        
                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_3 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_3 = req["plan"].ToDate();
                        }
                    }
                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "4")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_4 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_3 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_3 = DateTime.Today;
                        }
                        
                        jiezou_item.time_plan_5 = date;
                        jiezou_item.time_plan_6 = date;
                        jiezou_item.time_plan_7 = date;

                        //修改所有阶段的完成进度
                        jiezou_item.progress_4 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_2 = 2;
                        jiezou_item.progress_3 = 2;
                        jiezou_item.progress_5 = 0;
                        jiezou_item.progress_6 = 0;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_4 != 0)
                    {
                        
                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_4 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_4 = req["plan"].ToDate();
                        }
                    }
                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "5")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_5 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_4 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_4 = DateTime.Today;
                        }
                        jiezou_item.time_plan_6 = date;
                        jiezou_item.time_plan_7 = date;


                        //修改所有阶段的完成进度
                        jiezou_item.progress_5 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_2 = 2;
                        jiezou_item.progress_3 = 2;
                        jiezou_item.progress_4 = 2;
                        jiezou_item.progress_6 = 0;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_5 != 0)
                    {
                        
                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_5 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_5 = req["plan"].ToDate();
                        }
                    }
                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "6")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_6 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_6 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_6 = DateTime.Today;
                        }
                        jiezou_item.time_plan_7 = date;

                        //修改所有阶段的完成进度
                        jiezou_item.progress_6 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_2 = 2;
                        jiezou_item.progress_3 = 2;
                        jiezou_item.progress_4 = 2;
                        jiezou_item.progress_5 = 2;
                        jiezou_item.progress_7 = 0;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_6 != 0)
                    {

                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_6 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_6 = req["plan"].ToDate();
                        }
                    }
                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }

                if (req["step"].ToNullableString() == "7")
                {
                    //用户修改了完成进度，并且没有将进度设置为空
                    if (jiezou_item.progress_7 != req["progress"].ToSByte() && req["progress"].ToSByte() != 0)
                    {
                        //修改其他阶段时间

                        if (jiezou_item.progress_7 != 2)
                        {//如果上一阶段为进行中或空，将上一阶段周期时间变成今日
                            jiezou_item.time_plan_7 = DateTime.Today;
                        }


                        //修改所有阶段的完成进度
                        jiezou_item.progress_7 = req["progress"].ToSByte();
                        jiezou_item.progress_1 = 2;
                        jiezou_item.progress_2 = 2;
                        jiezou_item.progress_3 = 2;
                        jiezou_item.progress_4 = 2;
                        jiezou_item.progress_5 = 2;
                        jiezou_item.progress_6 = 2;
                    }

                    //如果完成进度不为空
                    if (jiezou_item.progress_5 != 0)
                    {

                        if (req["plan"].IsNullOrEmpty())
                        {
                            jiezou_item.time_plan_5 = date;
                        }
                        else
                        {
                            jiezou_item.time_plan_5 = req["plan"].ToDate();
                        }
                    }
                    ResetStep(jiezou_item);
                    lSql.Add(jiezou_item.UpdateTran());
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                
                return result;
            }

            /// <summary>
            /// 设置开始时间
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction SetTime(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var jiezou_item = DoMySql.FindEntity<ModelDb.jiezou_item>($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and jiezou_sn='{req["jiezou_sn"].ToNullableString()}' and status=0", false);

                if (!req["date"].ToNullableString().IsNullOrEmpty())
                {
                    jiezou_item.start_date = req["date"].ToString().ToDate();
                    jiezou_item.InsertOrUpdate($"tg_user_sn='{req["tg_user_sn"].ToNullableString()}' and jiezou_sn='{req["jiezou_sn"].ToNullableString()}' and status=0");
                }
                return result;
            }

            /// <summary>
            /// 重置节奏表当前步骤
            /// </summary>
            /// <param name="jiezou_item"></param>
            private void ResetStep(ModelDb.jiezou_item jiezou_item)
            {
                jiezou_item.step = 0;
                if (jiezou_item.progress_1 == ModelDb.jiezou_item.progress_1_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)0.5;
                }
                if (jiezou_item.progress_2 == ModelDb.jiezou_item.progress_2_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)1;
                }
                if (jiezou_item.progress_3 == ModelDb.jiezou_item.progress_3_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)2;
                }
                if (jiezou_item.progress_4 == ModelDb.jiezou_item.progress_4_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)3;
                }
                if (jiezou_item.progress_5 == ModelDb.jiezou_item.progress_5_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)4;
                }
                if (jiezou_item.progress_6 == ModelDb.jiezou_item.progress_6_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)5;
                }
                if (jiezou_item.progress_7 == ModelDb.jiezou_item.progress_7_enum.进行中.ToSByte())
                {
                    jiezou_item.step = (decimal?)6;
                }
                if (jiezou_item.progress_1 == 2 && jiezou_item.progress_2 == 2 && jiezou_item.progress_3 == 2 && jiezou_item.progress_4 == 2 && jiezou_item.progress_5 == 2 && jiezou_item.progress_6 == 2 && jiezou_item.progress_7 == 2)
                {
                    jiezou_item.step = 7;
                }
            }

            public List<JiezouQNS> GetQNS(int step,string keyword="")
            {
                var list = new List<JiezouQNS>();
                
                var Question = DoMySql.FindList<ModelDb.qns_questions>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and step='{step}' order by create_time,sort");

                int sort = 0;
                foreach (var q in Question)
                {
                    var Solution = DoMySql.FindList<ModelDb.qns_solutions>($" qns_sn='{q.qns_sn}'");
                    if ((!keyword.IsNullOrEmpty()) && (Solution.FindAll(x => x.solution.Contains(keyword)).Count == 0) &&(!q.question.Contains(keyword)))
                    {
                        continue;
                    }
                    sort++;
                    int solutioinSort = 0;
                    
                    
                    foreach (var s in Solution)
                    {
                        solutioinSort++;
                        list.Add(new JiezouQNS 
                        {
                            Question=q.question,
                            Solution=s.solution,
                            StepRowspan = sort==1 && solutioinSort == 1,
                            QuestionRowspan = solutioinSort == 1,
                            qns_sn = q.qns_sn
                        });
                    }
                    if (Solution.Count == 0)
                    {
                        list.Add(new JiezouQNS
                        {
                            Question = q.question,
                            Solution = "暂无数据",
                            StepRowspan = sort==1,
                            QuestionRowspan = true,
                            qns_sn = q.qns_sn
                        });
                    }
                    solutioinSort = 0;
                }
                return list;
            }

            public class JiezouQNS
            {
                public string Question { get; set; }
                public string Solution { get; set; }
                public bool StepRowspan { get; set; }
                public bool QuestionRowspan { get; set; }
                public string qns_sn { get; set; }
            }
        }
        public class JiezouResult : ModelDb.jiezou_detail
        {
            public object name { get; set; }
            public object username { get; set; }
            public object ting_name { get; set; }
            public object is_standard { get; set; }
        }

        public class JiezouZhuboResult : ModelDb.jiezou_zhubo_detail
        {
            public object name { get; set; }
            public object ting_name { get; set; }
        }
    }    
}
