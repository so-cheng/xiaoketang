using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Areas.Zhenduan.Controllers
{
    /// <summary>
    /// 数据分析
    /// </summary>
    public class AnalyseController : BaseLoginController
    {
        public ActionResult List(PageFactory.ZhenduanList.DtoReq req)
        {
            var pageModel = new PageFactory.ZhenduanList().Get(req);
            pageModel.listDisplay.listData.attachFilterSql = $"tg_user_sn='{new UserIdentityBag().user_sn}'";
            return View(pageModel);
        }

        public ActionResult Post(PageFactory.ZhenduanPost.DtoReq req)
        {
            var pageModel = new PageFactory.ZhenduanPost().Get(req);
            return View(pageModel);
        }

        public ActionResult Edit(PageFactory.ZhenduanEdit.DtoReq req)
        {
            var pageModel = new PageFactory.ZhenduanEdit().Get(req);
            return View(pageModel);
        }

        #region 诊断页面
        /// <summary>
        /// 从系统获取
        /// </summary>
        /// <param name="tg_user_sn"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public ActionResult Index(string tg_user_sn = "", string dateRange = "")
        {
            if (dateRange.IsNullOrEmpty())
            {
                dateRange = DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            string s_date = dateRange.Substring(0, dateRange.IndexOf('~') - 1);
            string e_date = dateRange.Substring(dateRange.IndexOf('~') + 1);
            var p_zhenduan_day = DoMySql.FindField<ModelDb.p_zhenduan_day>("SUM(join_num),SUM(live_hour),SUM(watch_minutes*join_num),SUM(exposure_num),SUM(exposure_times),SUM(gift_num),SUM(gift_times),SUM(gift_new),SUM(amount),SUM(comment)", $"tg_user_sn='{tg_user_sn}' and live_date>='{s_date}' and  live_date>='{e_date}'");
            decimal[] array = new decimal[p_zhenduan_day.Length];
            for (int i = 0; i < p_zhenduan_day.Length; i++)
            {
                array[i] = p_zhenduan_day[i].ToDecimal();
            }
            var list = GetZhenduan(array);

            ViewBag.avg_score = (list[0].score + list[1].score + list[2].score) / 3;
            ViewBag.obj = list.ToJson();
            return View();
        }

        /// <summary>
        /// 从抖音后台获取
        /// </summary>
        /// <returns></returns>
        public ActionResult Auto()
        {
            var user_base = new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn);
            if (user_base.attach3.IsNullOrEmpty())
            {
                ViewBag.url = "Auto";
                return View("CompleteInfo");
            }
            decimal[] array = new ServiceFactory.ZhenduanApiDataService().Get(DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"), user_base.attach3);
            var list = GetZhenduan(array);
            ViewBag.avg_score = (list[0].score + list[1].score + list[2].score) / 3;
            ViewBag.obj = list.ToJson();
            return View("Index");
        }

        /// <summary>
        /// 导出数据页面
        /// </summary>  
        /// <param name="tg_user_sn"></param>
        /// <returns></returns> 
        public ActionResult ExportInfo(string tg_user_sn = "")
        {
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }

            ViewBag.tg_user_sn = tg_user_sn;
            return View();
        }

        public ActionResult ExportToExcel(string account_id)
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = "诊断数据.xls";
            string SheetName = "诊断数据";

            AppLibrary.WriteExcel.Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            AppLibrary.WriteExcel.Cells cells = sheet.Cells;
            //第一行表头
            cells.Add(1, 1, "进直播间人数");
            cells.Add(1, 2, "进直播间人数");
            cells.Add(1, 3, "直播时长");
            cells.Add(1, 4, "总观看时长");
            cells.Add(1, 5, "曝光人数");
            cells.Add(1, 6, "曝光次数");
            cells.Add(1, 7, "打赏人数");
            cells.Add(1, 8, "打赏次数");
            cells.Add(1, 9, "新人打赏人数");
            cells.Add(1, 10, "音浪");
            cells.Add(1, 11, "互动人数");
            cells.Add(1, 12, "人均观看时长");

            int sort = 1;
            for (DateTime date = DateTime.Today; date >= DateTime.Today.AddDays(-31); date = date.AddDays(-1))
            {
                sort++;
                decimal[] array = new ServiceFactory.ZhenduanApiDataService().Get(date.ToString("yyyy-MM-dd"), date.ToString("yyyy-MM-dd"), account_id);
                cells.Add(sort, 1, date.ToString("yyyy-MM-dd"));
                cells.Add(sort, 2, array[0].ToString());
                cells.Add(sort, 3, array[1].ToString());
                cells.Add(sort, 4, array[2].ToString());
                cells.Add(sort, 5, array[3].ToString());
                cells.Add(sort, 6, array[4].ToString());
                cells.Add(sort, 7, array[5].ToString());
                cells.Add(sort, 8, array[6].ToString());
                cells.Add(sort, 9, array[7].ToString());
                cells.Add(sort, 10, array[8].ToString());
                cells.Add(sort, 11, array[9].ToString());
                cells.Add(sort, 12, array[10].ToString());
                if (sort > 32) { break; }
            }

            doc.Send();
            Response.Flush();
            Response.End();

            return Index();
        }

        #endregion

        #region 完善厅管信息
        /// <summary>
        /// 完善厅管的直播平台账号id
        /// </summary>
        /// <returns></returns>
        public ActionResult CompleteInfo(string tg_user_sn = "")
        {
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            ViewBag.tg_user_sn = tg_user_sn;
            return View();
        }

        /// <summary>
        /// 修改厅管的直播平台账号id
        /// </summary>
        /// <returns></returns>
        public ActionResult EditInfo()
        {
            ViewBag.url = "Auto";
            return View("CompleteInfo");
        }

        /// <summary>
        /// 提交厅管的账号id信息
        /// </summary>
        /// <returns></returns>
        public JsonResult InfoPost(string data, string tg_user_sn = "")
        {
            var info = new JsonResultAction();
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            var user_base = new DomainBasic.UserApp().GetInfoByUserSn(tg_user_sn);
            user_base.attach3 = data;
            try
            {
                if (user_base.Update() <= 0) { throw new Exception("提交失败"); }
            }
            catch (Exception e)
            {
                info.msg = e.Message;
                info.code = 1;
            }
            return Json(info);
        }

        /// <summary>
        /// 获取tg抖音账号数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tg_user_sn"></param>
        /// <returns></returns>
        public JsonResult GetInfo(string data, string tg_user_sn = "")
        {
            var info = new JsonResultAction();
            //var arr = new ServiceFactory.ZhenduanApiDataService().Get();
            return Json(info);
        }
        #endregion

        #region 诊断打分

        private List<Zhenduan> GetZhenduan(decimal[] array)
        {
            /*
            array[0]:进直播间人数
            array[1]:直播时长
            array[2]:人均观看时长*进直播间人数
            array[3]:曝光人数
            array[4]:曝光次数
            array[5]:打赏人数
            array[6]:打赏次数
            array[7]:新人打赏人数
            array[8]:音浪
            array[9]:评论人数（互动人数）
            array[10]:人均观看时长
            */
            var list = new List<Zhenduan>();
            if (array.IsNullOrEmpty())
            {

            }
            #region 主播分值影响维度
            //新人打赏占比=新人打赏人数/打赏人数
            double new_gift = array[7].ToDouble() / array[5].ToDouble();
            int new_gift_score = 0;
            if (0 <= new_gift && new_gift < 0.10)
            {
                new_gift_score = 10;
            }
            if (0.10 <= new_gift && new_gift < 0.20)
            {
                new_gift_score = 15;
            }
            if (0.20 <= new_gift && new_gift < 0.30)
            {
                new_gift_score = 20;
            }
            if (0.30 <= new_gift && new_gift < 0.40)
            {
                new_gift_score = 25;
            }
            if (0.40 <= new_gift && new_gift <= 0.50)
            {
                new_gift_score = 30;
            }
            if (0.50 < new_gift && new_gift <= 0.60)
            {
                new_gift_score = 25;
            }
            if (0.60 < new_gift && new_gift <= 0.70)
            {
                new_gift_score = 20;
            }
            if (0.70 < new_gift && new_gift <= 0.80)
            {
                new_gift_score = 15;
            }
            if (0.80 < new_gift && new_gift <= 0.90)
            {
                new_gift_score = 10;
            }

            //总音浪值/总流量=流量价值
            double amount_value = array[8].ToDouble() / array[0].ToDouble();
            int amount_value_score = 0;
            if (0 <= amount_value && amount_value < 5)
            {
                amount_value_score = 10;
            }
            if (5 <= amount_value && amount_value < 10)
            {
                amount_value_score = 20;
            }
            if (10 <= amount_value && amount_value < 15)
            {
                amount_value_score = 30;
            }
            if (15 <= amount_value && amount_value < 20)
            {
                amount_value_score = 40;
            }
            if (20 <= amount_value && amount_value <= 30)
            {
                amount_value_score = 50;
            }
            if (30 < amount_value && amount_value <= 35)
            {
                amount_value_score = 40;
            }
            if (35 < amount_value && amount_value <= 40)
            {
                amount_value_score = 30;
            }
            if (40 < amount_value && amount_value <= 45)
            {
                amount_value_score = 20;
            }
            if (45 < amount_value && amount_value <= 50)
            {
                amount_value_score = 10;
            }

            //重复曝光率=曝光次数/曝光人数
            double exposure_rate = array[4].ToDouble() / array[3].ToDouble();
            int exposure_rate_score = 0;
            if (1.0 < exposure_rate && exposure_rate < 1.5)
            {
                exposure_rate_score = 8;
            }
            if (1.5 <= exposure_rate && exposure_rate <= 2.0)
            {
                exposure_rate_score = 10;
            }
            if (2.0 < exposure_rate && exposure_rate <= 2.5)
            {
                exposure_rate_score = 8;
            }
            if (2.5 < exposure_rate && exposure_rate <= 3.0)
            {
                exposure_rate_score = 6;
            }
            if (3.0 < exposure_rate && exposure_rate <= 3.5)
            {
                exposure_rate_score = 4;
            }
            if (3.5 < exposure_rate && exposure_rate <= 4.0)
            {
                exposure_rate_score = 2;
            }
            if (4.0 < exposure_rate && exposure_rate <= 4.5)
            {
                exposure_rate_score = 0;
            }
            if (4.5 < exposure_rate && exposure_rate < 5.0)
            {
                exposure_rate_score = 0;
            }


            //停留时间
            double watch_minutes = array[10].ToDouble() / 60 / 7;
            int watch_minutes_score = 0;
            if (4.5 < watch_minutes)
            {
                watch_minutes_score = 0;
            }
            if (4.0 < watch_minutes && watch_minutes <= 4.5)
            {
                watch_minutes_score = 0;
            }
            if (3.5 < watch_minutes && watch_minutes <= 4.0)
            {
                watch_minutes_score = 5;
            }
            if (3.0 < watch_minutes && watch_minutes <= 3.5)
            {
                watch_minutes_score = 10;
            }
            if (2.5 < watch_minutes && watch_minutes <= 3.0)
            {
                watch_minutes_score = 15;
            }
            if (1.8 <= watch_minutes && watch_minutes <= 2.5)
            {
                watch_minutes_score = 20;
            }
            if (1.3 <= watch_minutes && watch_minutes < 1.8)
            {
                watch_minutes_score = 15;
            }
            if (0.8 <= watch_minutes && watch_minutes < 1.3)
            {
                watch_minutes_score = 10;
            }
            if (0.3 <= watch_minutes && watch_minutes < 0.8)
            {
                watch_minutes_score = 5;
            }
            if (0 <= watch_minutes && watch_minutes < 0.3)
            {
                watch_minutes_score = 0;
            }


            int zhenduan1 = new_gift_score + amount_value_score + exposure_rate_score + watch_minutes_score;
            #endregion

            #region 流量分值影响维度
            //流量分值=进直播间人数/有效开播时长
            double amount_point = array[0].ToDouble() / array[1].ToDouble();
            int amount_point_score = 0;
            if (350 < amount_point)
            {
                amount_point_score = 80;
            }
            if (250 < amount_point && amount_point <= 350)
            {
                amount_point_score = 90;
            }
            if (150 <= amount_point && amount_point <= 250)
            {
                amount_point_score = 100;
            }
            if (100 <= amount_point && amount_point < 150)
            {
                amount_point_score = 80;
            }
            if (50 <= amount_point && amount_point < 100)
            {
                amount_point_score = 50;
            }
            if (0 <= amount_point && amount_point < 50)
            {
                amount_point_score = 10;
            }
            int zhenduan2 = amount_point_score;
            #endregion

            #region MyRegion
            //进直播间转化率=打赏人数/进直播间人数
            double join_rate = array[5].ToDouble() / array[0].ToDouble();
            int join_rate_score = 0;
            if (30 <= join_rate && join_rate <= 40)
            {
                join_rate_score = 60;
            }
            if (join_rate < 30)
            {
                join_rate_score = 10 * (int)(join_rate / 5);
            }
            if (join_rate > 40)
            {
                join_rate_score = 60 - 10 * (int)((join_rate - 40) / 5);
            }
            if (join_rate_score < 0)
            {
                join_rate_score = 0;
            }



            //互动率=互动人数/进直播间人数
            double hudong_rate = array[9].ToDouble() / array[0].ToDouble();
            int hudong_rate_score = 0;
            if (12 < hudong_rate && hudong_rate <= 14)
            {
                hudong_rate_score = 4;
            }
            if (10 < hudong_rate && hudong_rate <= 12)
            {
                hudong_rate_score = 6;
            }
            if (8 < hudong_rate && hudong_rate <= 10)
            {
                hudong_rate_score = 8;
            }
            if (6 <= hudong_rate && hudong_rate <= 8)
            {
                hudong_rate_score = 10;
            }
            if (4 <= hudong_rate && hudong_rate < 6)
            {
                hudong_rate_score = 8;
            }
            if (2 <= hudong_rate && hudong_rate < 4)
            {
                hudong_rate_score = 6;
            }
            if (0 <= hudong_rate && hudong_rate < 2)
            {
                hudong_rate_score = 4;
            }

            //打赏频次=打赏次数/打赏人数
            double gift_rate = array[6].ToDouble() / array[5].ToDouble();
            int gift_rate_score = 0;
            if (gift_rate >= 4)
            {
                gift_rate_score = 10;
            }
            if (3 <= gift_rate && gift_rate < 4)
            {
                gift_rate_score = 6;
            }
            if (2 <= gift_rate && gift_rate < 3)
            {
                gift_rate_score = 5;
            }
            if (1 <= gift_rate && gift_rate < 2)
            {
                gift_rate_score = 4;
            }
            if (0 <= gift_rate && gift_rate < 1)
            {
                gift_rate_score = 2;
            }
            if (gift_rate == 0)
            {
                gift_rate_score = 0;
            }

            int zhenduan3 = join_rate_score + watch_minutes_score + hudong_rate_score + gift_rate_score;
            #endregion

            list.Add(new Zhenduan
            {
                title = "主播分值影响维度",
                score = zhenduan1,
                child = new List<Zhenduan_child>
                {
                    new Zhenduan_child
                    {
                        title="新人打赏人数占比",
                        status=(40 <= new_gift && new_gift <= 50)?1:3,
                        text=(40 <= new_gift && new_gift <= 50)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="总音浪值",
                        status=(20 <= amount_value && amount_value <= 30)?1:3,
                        text=(20 <= amount_value && amount_value <= 30)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="重复曝光率",
                        status=(1.5 <= exposure_rate && exposure_rate <= 2.0)?1:3,
                        text=(1.5 <= exposure_rate && exposure_rate <= 2.0)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="停留时间",
                        status=(1.8 <= watch_minutes && watch_minutes <= 2.5)?1:3,
                        text=(1.8 <= watch_minutes && watch_minutes <= 2.5)?"合格":"不合格"
                    }
                }
            });
            list.Add(new Zhenduan
            {
                title = "流量分值影响维度",
                score = zhenduan2,
                child = new List<Zhenduan_child>
                {
                    new Zhenduan_child
                    {
                        title="流量分值影响维度",
                        status=(150 <= amount_point && amount_point <= 250)?1:3,
                        text=(150 <= amount_point && amount_point <= 250)?"合格":"不合格"
                    }
                }
            });
            list.Add(new Zhenduan
            {
                title = "直播间用户分值影响因素",
                score = zhenduan3,
                child = new List<Zhenduan_child>
                {
                    new Zhenduan_child
                    {
                        title="进直播间转化率",
                        status=(30 <= join_rate && join_rate <= 40)?1:3,
                        text=(30 <= join_rate && join_rate <= 40)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="停留时间",
                        status=(1.8 <= watch_minutes && watch_minutes <= 2.5)?1:3,
                        text=(1.8 <= watch_minutes && watch_minutes <= 2.5)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="互动率",
                        status=(6 <= hudong_rate && hudong_rate <= 8)?1:3,
                        text=(6 <= hudong_rate && hudong_rate <= 8)?"合格":"不合格"
                    },
                    new Zhenduan_child
                    {
                        title="打赏频率",
                        status=(gift_rate >= 4)?1:3,
                        text=(gift_rate >= 4)?"合格":"不合格"
                    }
                }
            });
            return list;
        }
        #endregion

        #region 分数数据模型
        public class Zhenduan
        {
            public string title { get; set; }
            public int score { get; set; }
            public List<Zhenduan_child> child { get; set; }
        }

        public class Zhenduan_child
        {
            public string title { get; set; }
            public int status { get; set; }
            public string text { get; set; }
        }
        #endregion

    }
}