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
        public class CustomerService
        {
            /// <summary>
            /// 计算资料完整度
            /// </summary>
            /// <param name="customer_id"></param>
            /// <returns></returns>
            public decimal GetInfoCompleted(int customer_id)
            {
                int completed = 0;// 资料完整度：16项
                var customer = DoMySql.FindEntityById<ModelDb.p_crm_customer>(customer_id);

                if (!customer.nick.IsNullOrEmpty()) completed++;
                if (!customer.dou_user.IsNullOrEmpty() && !customer.dou_user.Equals("-")) completed++;
                if (!customer.user_grade.IsNullOrEmpty()) completed++;
                if (!customer.address.IsNullOrEmpty() && !customer.address.Equals("-")) completed++;
                if (!customer.know_type.IsNullOrEmpty()) completed++;
                if (!customer.user_job.IsNullOrEmpty() && !customer.user_job.Equals("-")) completed++;
                if (!customer.user_life.IsNullOrEmpty()) completed++;
                if (!customer.emo_status.IsNullOrEmpty() && !customer.emo_status.Equals("-")) completed++;
                if (!customer.user_like.IsNullOrEmpty() && !customer.user_like.Equals("-")) completed++;
                if (!customer.user_birthday.IsNullOrEmpty() && !customer.user_birthday.Equals("-")) completed++;
                if (!customer.first_time.IsNullOrEmpty()) completed++;
                if (!customer.user_level.IsNullOrEmpty() && !customer.user_level.Equals("-")) completed++;
                if (!customer.mbti.IsNullOrEmpty()) completed++;
                if (!customer.has_wechat.IsNullOrEmpty() && !customer.has_wechat.Equals("-")) completed++;
                if (!customer.first_consum.IsNullOrEmpty() && !customer.first_consum.Equals("-")) completed++;
                if (!customer.contact_time.IsNullOrEmpty()) completed++;

                return Math.Round((completed.ToDecimal() / 16) * 100);
            }
        }
    }

}
