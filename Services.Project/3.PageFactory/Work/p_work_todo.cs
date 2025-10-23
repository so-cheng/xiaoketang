using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.ModelDbs;

namespace Services.Project._3.PageFactory.Work
{
    public class p_work_todo : ModelDb.p_work_todo
    {
        private string _t_date_time;
        /// <summary>
        /// 剩余待办时间
        /// </summary>
        public string t_date_time
        {
            get
            {
                TimeSpan? difference = e_date_time != null ? e_date_time - DateTime.Now : null;
                if (difference?.TotalMinutes > 0 && difference?.TotalMinutes < 120)
                {
                    _t_date_time = difference?.Hours + ":" + difference?.Minutes + ":" + difference?.Seconds;
                }
                else
                {
                    _t_date_time = "";
                }
                return _t_date_time;
            }
            set
            {

                _t_date_time = value;

            }
        }
    }
}
