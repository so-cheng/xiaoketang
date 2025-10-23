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
        public class YyService
        {
            /// <summary>
            /// 获取运营名下某天的请假主播
            /// </summary>
            /// <param name="yy_user_sn">运营sn</param>
            /// <param name="date">指定日期</param>
            /// <returns></returns>
            public List<ModelDb.user_base> GetZbVacation(string yy_user_sn, DateTime date)
            {
                var tg_list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);

                var list = new List<ModelDb.user_base>();
                foreach (var tg in tg_list)
                {
                    list.AddRange(DoMySql.FindList<ModelDb.user_base>($"user_sn in (select zb_user_sn from p_jixiao_vacation where c_date = '{date.ToString("yyyy-MM-dd")}' and user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀主播,tg.user_sn)}) and status = '{ModelDb.user_base.status_enum.正常}'"));
                }
                return list;
            }

            /// <summary>
            /// 运营获取下级厅管
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public List<ModelDbBasic.user_base> YyGetNextTg(string yy_user_sn)
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                return list;
            }
            /// <summary>
            /// 运营获取下级厅管Sql
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public string YyGetNextTgForSql(string yy_user_sn)
            {
                return new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
            }

        }
    }    
}
