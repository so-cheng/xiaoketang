using Services.Project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static WeiCode.Models.ModelBasic;
namespace WebProject.Areas.LiveRoom.Controllers
{
    /// <summary>
    /// 直播间管理
    /// </summary>
    public class ManageController : BaseLoginController
    {
        /// <summary>
        /// 直播间列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var result = new JsonResultAction();
            try
            {
                var tradeProductInfos = new List<PageFactory.LiveRoom.LiveRoomList.ItemDataModel>();

                string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}' and iszhibo=1";
                //中台看自己的,管理端看全部
                switch ((ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id)
                {
                    case ModelEnum.UserTypeEnum.jder:
                        where += $" and zt_user_sn = '{new UserIdentityBag().user_sn}'";
                        break;
                    case ModelEnum.UserTypeEnum.manager:
                        break;
                    default:
                        where += $" and 1!=1";
                        break;
                }
                List<ModelDb.p_liveroom> qrlist = DoMySql.FindList<ModelDb.p_liveroom>(where);
                foreach (var item in qrlist)
                {
                    tradeProductInfos.Add(new Services.Project.PageFactory.LiveRoom.LiveRoomList.ItemDataModel(item));
                }
                result.data = tradeProductInfos;
            }
            catch (Exception e)
            {
                result.code = 1;
                result.msg = e.Message;
                return Json(result);
            }
            return Json(result);
        }
        /// <summary>
        /// 直播间列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 直播间平面图
        /// </summary>
        /// <returns></returns>

        public ActionResult Plan()
        {
            return View();
        }
        /// <summary>
        /// 平面图标注
        /// </summary>
        /// <returns></returns>
        public ActionResult BiaoZhu()
        {
            return View();
        }

        public JsonResult BiaoZhuPost(List<ModelDb.p_liveroom_fix> areas)
        {
            var pageModel = new PageFactory.LiveRoom.LiveRoomPost().InsertLiveRoomFix(areas);
            return Json(pageModel);
        }

        public JsonResult GetFix()
        {
            var pageModel = new PageFactory.LiveRoom.LiveRoomPost().GetLiveRoomFix();
            return Json(pageModel);
        }

        /// <summary>
        /// 直播间使用情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Use()
        {
            return View();
        }
        /// <summary>
        /// 操作日志
        /// </summary>
        public ActionResult Log()
        {
            var req = new PageFactory.LiveRoom.LogList.DtoReq();
            var pageModel = new PageFactory.LiveRoom.LogList().Get(req);
            return View(pageModel);
        }

        /// <summary>
        /// 直播间使用率统计
        /// </summary>
        /// <returns></returns>
        public ActionResult UseRate(string c_date)
        {
            ///默认为近7天
            if (c_date.IsNullOrEmpty())
            {
                c_date = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + " ~ " + DateTime.Today.ToString("yyyy-MM-dd");
            }
            ViewBag.c_date = c_date;

            //解析开始结束时间
            var date_s = c_date.Split('~');
            DateTime f_date = date_s[0].Trim().ToDate();
            DateTime t_date = date_s[1].Trim().ToDate();

            //补全时间段内日期
            List<string> dateArray = new List<string>();
            for (DateTime date = f_date; date <= t_date; date = date.AddDays(1))
            {
                dateArray.Add(date.ToString("yyyy-MM-dd"));
            }
            ViewBag.dateArray = dateArray;

            List<decimal> useNum = new List<decimal>(); //已使用数量
            List<decimal> noUseNum = new List<decimal>(); // 未使用数量


            var list = DoMySql.FindList<ModelDb.p_liveroom_userate>($" zt_user_sn='{new UserIdentityBag().user_sn}' and create_time between '{f_date}' and '{t_date}'");

            foreach (var item in dateArray)
            {
                var result = list.Where(t => t.create_time == item.ToDate());
                var totalCount = result.Count();
                var useCount = result.Where(t => t.status == 1).Count();
                var userate = 0M;
                if (totalCount != 0)
                {
                    userate = (decimal)useCount / totalCount;
                    userate = Convert.ToDecimal(userate.ToString("F2"));
                    useNum.Add(userate * 100);
                    noUseNum.Add((1 - userate) * 100);
                }
                else
                {
                    useNum.Add(0M);
                    noUseNum.Add(0M);
                }
            }
            var count = DoMySql.FindField<ModelDb.p_liveroom>("COUNT(id)", $" zt_user_sn='{new UserIdentityBag().user_sn}'");
            ViewBag.totalNum = count[0].ToInt();
            ViewBag.useNum = useNum;
            ViewBag.noUseNum = noUseNum;
            return View();
        }


        public ActionResult QRCode(string id)
        {
            // 构建详情页URL
            var baseDomain = $"{Request.Url.Scheme}://{Request.Url.Host}";

            string detailUrl = $"{baseDomain}/liveroom/roominfo/detail?liveRoomId={id}";
            // 生成二维码
            byte[] qrCodeBytes = UtilityStatic.QRcode.GenerateQRCode(detailUrl);

            // 返回图片
            return File(qrCodeBytes, "image/png");
        }
    }
}