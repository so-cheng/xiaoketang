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
        /// <summary>
        /// 厅管相关信息
        /// </summary>
        public class TgInfoService
        {

            /// <summary>
            /// 获取厅管信息
            /// </summary>
            /// <param name="user_sn"></param>
            /// <returns></returns>
            public TgInfo GetTgInfo(string user_sn)
            {
                var tgInfo = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_sn}' and user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("tger").id}'",false).ToModel<TgInfo>();
                if (tgInfo.IsNullOrEmpty()) 
                {
                    return new TgInfo();
                }
                try
                {
                    var info = tgInfo.attach3.Split('☆');
                    tgInfo.dou_username = info[0];
                    tgInfo.tg_sex = info[1];
                    tgInfo.wechat_username = info[2];
                    tgInfo.UID = info[3];
                    tgInfo.QRCode = info[3];
                }
                catch
                {

                }
                return tgInfo;
            }

            public class TgInfo : ModelDb.user_base
            {
                /// <summary>
                /// 抖音大头号
                /// </summary>
                public string dou_username { get; set; }
                /// <summary>
                /// 男女厅
                /// </summary>
                public string tg_sex { get; set; }
                /// <summary>
                /// 管理微信号
                /// </summary>
                public string wechat_username { get; set; }
                /// <summary>
                /// UID
                /// </summary>
                public string UID { get; set; }

                /// <summary>
                /// 目前在开档
                /// </summary>
                public string dangwei { get; set; }

                /// <summary>
                /// 二维码
                /// </summary>
                public string QRCode { get; set; }
            }

            /// <summary>
            /// 厅管获取下级厅管
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public List<ModelDbBasic.user_base> TgGetNextTg(string tg_user_sn)
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀厅管, tg_user_sn);
                return list;
            }

            /// <summary>
            /// 厅管获取下级主播
            /// </summary>
            /// <param name="yy_user_sn"></param>
            /// <returns></returns>
            public List<ModelDbBasic.user_base> TgGetNextZb(string tg_user_sn)
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn);
                return list;
            }
        }

        /// <summary>
        /// 运营相关信息
        /// </summary>
        public class YyInfoService
        {
            /// <summary>
            /// 获取运营名下的请假主播
            /// </summary>
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

            public Dictionary<string,string> YyGetNextTgForKv(string yy_user_sn)
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsersForKv(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                return list;
            }

            public string YyGetNextTgForSql(string yy_user_sn)
            {
                return new DomainUserBasic.UserRelationApp().GetNextUserSnForSql(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
            }

            public Dictionary<string, string> GetAllYyForKv()
            {
                var Dic = new Dictionary<string, string>();

                var list = GetAllYyForList();
                foreach (var item in list)
                {
                    Dic.Add(item.username,item.user_sn);
                }
                return Dic;
            }

            public List<ModelDbBasic.user_base> GetAllYyForList()
            {
                return new DomainBasic.UserApp().GetInfosByEntityWhere(new DomainBasic.UserApp.EntityWhere
                {
                    userTypeEnum = ModelEnum.UserTypeEnum.yyer
                });
            }
        }

        /// <summary>
        /// 主播相关信息
        /// </summary>
        public class ZbInfoService
        {

            public ModelDbBasic.user_base GetZbInfo(string user_sn)
            {
                return new DomainBasic.UserApp().GetInfoByUserSn(user_sn);
            }

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

            /// <summary>
            /// 判断主播某日期是否请假
            /// </summary>
            /// <param name="zb_user_sn"></param>
            /// <returns></returns>
            public bool isZbVacation(string zb_user_sn, DateTime date)
            {
                var vacation = GetZbVacation(zb_user_sn, date, date);
                return vacation > 0;
            }

        }
    }    
}
