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


namespace WebProject.Areas.JixiaoDay.Controllers
{
    public class ShangjiaoController : BaseLoginController
    {
        /// <summary>
        /// 支付页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {          
            return View();
        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {          
            return View();
        }

        public ActionResult Deliver()
        {         
            return View();
        }


        public JsonResult WeixinNotifyUrl(string out_trade_no, string trade_no)
        {
            var info = new JsonResultAction();

            try
            {
                // 先查询支付记录
                var shangjiao = DoMySql.FindEntity<ModelDb.p_jixiao_day_ting_shangjiao>($"id = {out_trade_no}", true);

                // 检查记录是否存在
                if (shangjiao == null) throw new WeicodeException("支付记录不存在");

                // 检查状态
                if (shangjiao.status != (sbyte?)ModelDb.p_jixiao_day_ting_shangjiao.status_enum.未上交)
                {
                    throw new WeicodeException("当前状态无法支付");
                }

                // 修改状态
                shangjiao.status = (sbyte?)ModelDb.p_jixiao_day_ting_shangjiao.status_enum.已上交;

                // 更新记录
                shangjiao.Update();
                info.msg = "支付成功";
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }

            return Json(info);
        }
        
    }
}