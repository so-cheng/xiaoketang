using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.Services;

using Services.Project;
using WeiCode.ModelDbs;

namespace WebProject.Controllers
{
    public class HomeController : BaseController
    {
        public ContentResult Index()
        {
            Response.Redirect(new DomainBasic.UserTypeApp().GetDefaultPage());
            return Content("");
        }

        public ContentResult DouGetTotal()
        {
            return Content("");
        }

        public ContentResult DouGetTingData()
        {
            return Content("");
        }
        public ContentResult JiezouMouth()
        {
            new TaskProject.ProjectClass().JiezouMouth();
            return Content("");
        }


        /// <summary>
        /// 同步user_info_zb的信息到user_info_zhubo
        /// </summary>
        /// <returns></returns>
        public ContentResult InsertZhuboFromZb()
        {
            var list = DoMySql.FindList<ModelDb.user_info_zb>("user_sn not in (SELECT user_sn from user_info_zhubo) and user_sn != '' LIMIT 100");
            var lSql = new List<string>();
            foreach (var user_info_zb in list)
            {
                var user_info_zhubo = user_info_zb.ToModel<ModelDb.user_info_zhubo>();
                user_info_zhubo.user_name = user_info_zb.name;

                if (user_info_zb.status == ModelDb.user_info_zb.status_enum.正常.ToSByte())
                {
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                }
                if (user_info_zb.status == ModelDb.user_info_zb.status_enum.已流失.ToSByte())
                {
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.已离职.ToSByte();
                }
                if (user_info_zb.status == ModelDb.user_info_zb.status_enum.逻辑删除.ToSByte())
                {
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                }
                if (user_info_zb.user_sn.IsNullOrEmpty())
                {
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.待开账号.ToSByte();
                }
                lSql.Add(user_info_zhubo.InsertOrUpdateTran($"user_info_zb_sn = '{user_info_zhubo.user_info_zb_sn}'"));
            }
            MysqlHelper.ExecuteSqlTran(lSql);
            return Content(list.Count.ToString());
        }

        /// <summary>
        /// 根据user_base同步user_info_zhubo
        /// </summary>
        /// <returns></returns>
        public ContentResult InsertZhuboFromUserBase()
        {
            var lSql = new List<string>();
            var list = DoMySql.FindList<ModelDb.user_base>($"user_type_id = 11 and user_sn not in (SELECT user_sn from user_info_zhubo WHERE user_sn != '') LIMIT 10");// and user_sn = '20250803200554694-1445391725'

            foreach (var user_base in list)
            {
                string tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, user_base.user_sn);
                string ting_sn = tg_user_sn;
                string yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).yy_sn;
                string zt_user_sn = new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).zt_user_sn;
                sbyte? status = 1;
                switch (user_base.status)
                {
                    case 0:
                        status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                        break;
                    case 9:
                        status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                        break;
                    default:
                        break;
                }

                string level = "";
                switch (user_base.attach2)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        level = "A";
                        break;
                    case "6":
                        level = "B";
                        break;
                    case "7":
                        level = "C";
                        break;
                    default:
                        break;
                }

                lSql.Add(new ModelDb.user_info_zhubo()
                {
                    tenant_id = user_base.tenant_id,
                    user_info_zb_sn = UtilityStatic.CommonHelper.CreateUniqueSn(),
                    user_sn = user_base.user_sn,
                    user_name = user_base.name,
                    mobile = user_base.mobile,
                    tg_user_sn = tg_user_sn,
                    ting_sn = ting_sn,
                    yy_user_sn = yy_user_sn,
                    zt_user_sn = zt_user_sn,
                    full_or_part = user_base.attach1,
                    position = user_base.attach2,
                    level = level,
                    openid = user_base.attach4,
                    status = status,
                }.InsertTran());
            }
            MysqlHelper.ExecuteSqlTran(lSql);
            return Content(list.Count.ToString());
        }

        /// <summary>
        /// 同步user_info_zhubo的职位和评级
        /// </summary>
        /// <returns></returns>
        public ContentResult UpdateZhubo()
        {
            var zhubos = DoMySql.FindList<ModelDb.user_info_zhubo>($" user_sn != '' and (position = '' or (position != '' and level = '')) limit 10"); // and user_sn = '20250311235701503-310936241'

            foreach (var zhubo in zhubos)
            {
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{zhubo.user_sn}'");
                if (user_base.attach2.IsNullOrEmpty()) {
                    zhubo.position = "1";
                }
                else
                {
                    zhubo.position = user_base.attach2;
                }
                if (zhubo.full_or_part.IsNullOrEmpty())
                {
                    if (user_base.attach1.IsNullOrEmpty())
                    {
                        zhubo.full_or_part = "兼职";
                    }
                    else
                    {
                        zhubo.full_or_part = user_base.attach1;
                    }
                }

                switch (zhubo.position)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        zhubo.level = "A";
                        break;
                    case "6":
                        zhubo.level = "B";
                        break;
                    case "7":
                        zhubo.level = "C";
                        break;
                    default:
                        break;
                }
                zhubo.Update();
            }
            return Content(zhubos.Count.ToString());
        }

        /// <summary>
        /// user_info_tg 所属运营不同步
        /// </summary>
        /// <returns></returns>
        public string UpdateTgInfoRelation()
        {
            var l_ting = DoMySql.FindListBySql<user_info_tg>($@"SELECT
	                                                                user_info_tg.*,
	                                                                f_user_sn 
                                                                FROM
	                                                                user_info_tg
	                                                                LEFT JOIN user_relation ON user_info_tg.tg_user_sn = user_relation.t_user_sn 
                                                                WHERE
	                                                                user_info_tg.tenant_id = 1 
	                                                                AND user_relation.f_user_type_id = 12 
	                                                                AND user_info_tg.yy_user_sn != user_relation.f_user_sn");

            foreach (var ting in l_ting)
            {
                ting.yy_user_sn = ting.f_user_sn;
                ting.ToModel<ModelDb.user_info_tg>().Update();
            }
            return $"{l_ting.Count}";
        }
        public class user_info_tg : ModelDb.user_info_tg
        {
            /// <summary>
            /// 实际所属运营
            /// </summary>
            public string f_user_sn { get; set; }
        }

        /// <summary>
        /// 主播user_info_zhubo 所属厅管与运营不同步
        /// </summary>
        /// <returns></returns>
        public string UpdateZhuboInfoRelation()
        {
            var l_zhubo_tg = DoMySql.FindListBySql<user_info_zhubo>($@"SELECT
	                                                                        user_info_zhubo.*,
	                                                                        f_user_sn 
                                                                        FROM
	                                                                        user_info_zhubo
	                                                                        LEFT JOIN user_relation ON user_info_zhubo.user_sn = user_relation.t_user_sn 
                                                                        WHERE
	                                                                        user_info_zhubo.tenant_id = 1 
	                                                                        AND user_sn != '' 
	                                                                        AND user_relation.f_user_type_id = 10 
	                                                                        AND user_info_zhubo.tg_user_sn != user_relation.f_user_sn");

            foreach (var zhubo in l_zhubo_tg)
            {
                zhubo.tg_user_sn = zhubo.f_user_sn;
                zhubo.ting_sn = zhubo.f_user_sn;
                zhubo.ToModel<ModelDb.user_info_zhubo>().Update();
            }

            var l_zhubo_yy = DoMySql.FindListBySql<user_info_zhubo>($@"SELECT
	                                                                        user_info_zhubo.*,
	                                                                        f_user_sn 
                                                                        FROM
	                                                                        user_info_zhubo
	                                                                        LEFT JOIN user_relation ON user_info_zhubo.tg_user_sn = user_relation.t_user_sn 
                                                                        WHERE
	                                                                        user_info_zhubo.tenant_id = 1 
	                                                                        AND user_sn != '' 
	                                                                        AND user_relation.f_user_type_id = 12 
	                                                                        AND user_info_zhubo.yy_user_sn != user_relation.f_user_sn");

            foreach (var zhubo in l_zhubo_yy)
            {
                zhubo.yy_user_sn = zhubo.f_user_sn;
                zhubo.ToModel<ModelDb.user_info_zhubo>().Update();
            }

            return $"{l_zhubo_tg.Count} = {l_zhubo_yy.Count}";
        }

        /// <summary>
        /// 重置主播状态(与zb同步)
        /// </summary>
        /// <returns></returns>
        public string ResetZhuboStatus()
        {
            var list = DoMySql.FindList<ModelDb.user_info_zhubo>($"user_sn != ''");
            foreach (var item in list)
            {
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{item.user_sn}'", false);
                if (user_base.IsNullOrEmpty()) continue;
                switch (user_base.status)
                {
                    case (sbyte?)ModelDb.user_base.status_enum.正常:
                        item.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                        break;
                    case (sbyte?)ModelDb.user_base.status_enum.逻辑删除:
                        item.status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                        break;
                    default:
                        break;
                }
                item.Update();
            }
            return "";
        }

        public class user_info_zhubo : ModelDb.user_info_zhubo
        {
            /// <summary>
            /// 实际所属运营
            /// </summary>
            public string f_user_sn { get; set; }
        }
    }
}