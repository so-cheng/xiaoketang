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
        public partial class JixiaoDay
        {
            public class QingJiaService
            {
                /// <summary>
                /// 获取主播请假的列表
                /// </summary>
                /// <param name="zb_user_sn"></param>
                /// <param name="c_date"></param>
                /// <returns></returns>
                public List<ModelDb.p_jixiao_qingjia> GetListByUserSn(string zb_user_sn)
                {
                    var p_jixiao_qingjia = DoMySql.FindList<ModelDb.p_jixiao_qingjia>($" zb_user_sn = '{zb_user_sn}'");
                    return p_jixiao_qingjia;
                }


                /// <summary>
                /// 获取主播是否请假
                /// </summary>
                /// <param name="zb_user_sn"></param>
                /// <param name="c_date"></param>
                /// <returns></returns>
                public bool GetZbByUserDate(string zb_user_sn,DateTime date )
                {
                    var p_jixiao_qingjia = DoMySql.FindEntity<ModelDb.p_jixiao_qingjia>($" zb_user_sn = '{zb_user_sn}' and s_date <= '{date.ToString("yyyy-MM-dd")}' and e_date >= '{date.ToString("yyyy-MM-dd")}'and status={(sbyte)ModelDb.p_jixiao_qingjia.status_enum.审批同意}", false);
                    if (p_jixiao_qingjia.IsNullOrEmpty()) return false;

                    return true;
                }


                /// <summary>
                /// 厅中有哪些主播请假
                /// </summary>
                /// <param name="zb_user_sn"></param>
                /// <param name="c_date"></param>
                /// <returns></returns>
                public List<ModelDb.p_jixiao_qingjia> GetListByTing(string ting_sn)
                {
                    var p_jixiao_qingjia = DoMySql.FindList<ModelDb.p_jixiao_qingjia>($" ting_sn = '{ting_sn}'");
                    return p_jixiao_qingjia;
                }
            }
            
        }
        
    }
}
