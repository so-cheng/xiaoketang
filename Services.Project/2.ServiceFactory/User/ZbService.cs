using System;
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
        public class ZbService
        {

            /// <summary>
            /// 获取主播请假天数
            /// </summary>
            /// <param name="zb_user_sn"></param>
            /// <param name="date_s"></param>
            /// <param name="date_e"></param>
            /// <returns></returns>
            public int GetZbVacation(string zb_user_sn, DateTime s_date, DateTime e_date)
            {
                var p_jixiao_vacation = DoMySql.FindList<ModelDb.p_jixiao_vacation>($"zb_user_sn = '{zb_user_sn}' and c_date >= '{s_date.ToString("yyyy-MM-dd")}' and c_date < '{e_date.AddDays(1).ToString("yyyy-MM-dd")}'");
                return p_jixiao_vacation.Count;
            }

            /// <summary>
            /// 获取所属厅管
            /// </summary>
            /// <param name="zb_user_sn"></param>
            /// <returns></returns>
            public ModelDb.user_base GetParentTg(string zb_user_sn)
            {
                var tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, zb_user_sn);
                return new DomainBasic.UserApp().GetUserByUserSn(tg_user_sn).ToModel<ModelDb.user_base>();
            }


        }
    }    
}
