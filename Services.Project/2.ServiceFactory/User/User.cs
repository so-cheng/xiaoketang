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
        /// 用户
        /// </summary>
        public class UserService
        {
            #region 创建
            public bool Post(user_base dtoReq, Enum relation_type)
            {
                //调用创建用户接口
                List<string> lSql = new List<string>();
                if (dtoReq.name.IsNullOrEmpty())
                {
                    dtoReq.name = dtoReq.username;
                }
                if (!dtoReq.f_user_sn.IsNullOrEmpty())
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, dtoReq.f_user_sn, dtoReq.user_sn));
                }
                lSql.Add(new DomainBasic.UserApp().Create(dtoReq.ToModel<ModelDbBasic.user_base>(), true));
                lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(relation_type, new UserIdentityBag().user_sn, dtoReq.user_sn));
                return DoMySql.ExecuteSqlTran(lSql) > 0;
            }

            public List<string> PostTran(user_base dtoReq, Enum relation_type)
            {
                //调用创建用户接口
                List<string> lSql = new List<string>();
                if (dtoReq.name.IsNullOrEmpty())
                {
                    dtoReq.name = dtoReq.username;
                }
                if (!dtoReq.f_user_sn.IsNullOrEmpty())
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, dtoReq.f_user_sn, dtoReq.user_sn));
                }
                lSql.Add(new DomainBasic.UserApp().Create(dtoReq.ToModel<ModelDbBasic.user_base>(), true));
                lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(relation_type, new UserIdentityBag().user_sn, dtoReq.user_sn));
                return lSql;
            }
            /// <summary>
            /// 厅管端创建
            /// </summary>
            /// <param name="dtoReq"></param>
            /// <param name="relation_type"></param>
            /// <returns></returns>
            public bool ManagerPostTg(user_base dtoReq, string yy_yser_sn)
            {
                //调用创建用户接口
                List<string> lSql = new List<string>();
                if (dtoReq.name.IsNullOrEmpty())
                {
                    dtoReq.name = dtoReq.username;
                }
                if (!dtoReq.f_user_sn.IsNullOrEmpty())
                {
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀厅管, dtoReq.f_user_sn, dtoReq.user_sn));
                }
                lSql.Add(new DomainBasic.UserApp().Create(dtoReq.ToModel<ModelDbBasic.user_base>(), true));
                lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_yser_sn, dtoReq.user_sn));
                return DoMySql.ExecuteSqlTran(lSql) > 0;
            }

            public bool ManagerPostZb(user_base dtoReq, string tg_user_sn)
            {
                List<string> lSql = new List<string>();
                if (dtoReq.name.IsNullOrEmpty())
                {
                    dtoReq.name = dtoReq.username;
                }
                lSql.Add(new DomainBasic.UserApp().Create(dtoReq.ToModel<ModelDbBasic.user_base>(), true));
                lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn, dtoReq.user_sn));
                return DoMySql.ExecuteSqlTran(lSql) > 0;
            }

            public JsonResultAction TgInfoEdit(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var lSql = new List<string>();
                var req = reqJson.GetPara();
                var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req["id"].ToInt());

                string[] info = new string[4];
                if (user_base.attach3.Contains("☆"))
                {
                    info = user_base.attach3.Split('☆');
                    info[req["Index"].ToInt()] = req["Value"].ToString();
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == req["Index"].ToInt())
                        {
                            info[i] = req["Value"].ToString();
                        }
                        else
                        {
                            info[i] = "-";
                        }
                    }
                }

                user_base.attach3 = string.Join("☆", info);
                lSql.Add(user_base.UpdateTran());
                MysqlHelper.ExecuteSqlTran(lSql);
                return result;
            }
            public class Attach3Info
            {
                public string dou_username { get; set; }
                public string tg_sex { get; set; }
                public string wechat_username { get; set; }
                public string UID { get; set; }
            }

            public class user_base : ModelDb.user_base
            {
                public string f_user_sn { get; set; }
                public string user_type { get; set; }
                public int? user_info_zb_id { get; set; }
            }
            #endregion
        }
    }
}
