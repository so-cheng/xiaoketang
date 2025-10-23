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
        public partial class UserInfo
        {
            public class User
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

                /// <summary>
                /// 厅管端创建
                /// </summary>
                /// <param name="dtoReq"></param>
                /// <param name="relation_type"></param>
                /// <returns></returns>
                public List<string> ManagerPostTg(user_base dtoReq, string yy_yser_sn)
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
                    return lSql;
                }

                public List<string> ManagerPostZb(user_base dtoReq, string tg_user_sn)
                {
                    List<string> lSql = new List<string>();
                    if (dtoReq.name.IsNullOrEmpty())
                    {
                        dtoReq.name = dtoReq.username;
                    }
                    lSql.Add(new DomainBasic.UserApp().Create(dtoReq.ToModel<ModelDbBasic.user_base>(), true));
                    lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(ModelEnum.UserRelationTypeEnum.厅管邀主播, tg_user_sn, dtoReq.user_sn));
                    return lSql;
                }

                /// <summary>
                /// 创建user_info_zb
                /// </summary>
                /// <param name="dtoReq"></param>
                /// <returns></returns>
                public bool ManagerPostUserInfoZb(ModelDb.user_info_zb user_info_zb)
                {
                    List<string> lSql = new List<string>();

                    user_info_zb.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    if (user_info_zb.user_info_zb_sn == null || user_info_zb.user_info_zb_sn == "")
                    {
                        user_info_zb.user_info_zb_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    }
                    if (user_info_zb.yy_user_sn == null || user_info_zb.yy_user_sn == "")
                    {
                        user_info_zb.yy_user_sn = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(user_info_zb.tg_user_sn).yy_sn;
                    }

                    switch (user_info_zb.position)
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                            user_info_zb.level = "A";
                            break;
                        case "6":
                            user_info_zb.level = "B";
                            break;
                        case "7":
                            user_info_zb.level = "C";
                            break;
                        default:
                            break;
                    }

                    lSql.Add(user_info_zb.InsertOrUpdateTran($"id = '{user_info_zb.id}'"));
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
                    public string dou_username { get; set; }
                    public string f_user_sn { get; set; }
                    public string user_type { get; set; }
                    public int? user_info_zb_id { get; set; }
                }
                #endregion

                #region 用户修改
                public List<string> UserUpdate(JsonRequestAction req, ServiceFactory.UserInfo.User.user_base user_base, Enum modelEnum)
                {
                    //1.校验信息
                    if (user_base.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (user_base.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");

                    var lSql = new List<string>();
                    //2.修改厅管基本信息
                    var dto = new ModelDbBasic.user_base();
                    dto.id = user_base.id;
                    dto.name = req.GetPara("name");
                    dto.username = req.GetPara("username");
                    dto.mobile = req.GetPara("mobile");
                    dto.attach1 = req.GetPara("attach1");
                    dto.attach3 = user_base.attach3;
                    if (new DomainBasic.UserTypeApp().GetInfo().id == new DomainBasic.UserTypeApp().GetInfoByCode("tger").id)
                    {
                        dto.attach2 = req.GetPara("attach2");
                    }
                    if (!req.GetPara("password").IsNullOrEmpty()) dto.password = req.GetPara("password");
                    lSql.AddRange(new DomainBasic.UserApp().SetUserInfoByEntityTran(dto));
                    //3.修改上级厅管
                    var parent_tg_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(modelEnum, dto.user_sn);
                    if (parent_tg_user_sn != user_base.f_user_sn)
                    {//若上级厅管更改
                     //3.1.如果原来有上级厅管则删除原来的上级厅管
                        if (!parent_tg_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().UnBindTran(modelEnum, parent_tg_user_sn, dto.user_sn));
                        //3.2.如果有新的上级厅管则添加新的上级厅管
                        if (!user_base.f_user_sn.IsNullOrEmpty()) lSql.AddRange(new DomainUserBasic.UserRelationApp().BindTran(modelEnum, user_base.f_user_sn, dto.user_sn));
                    }
                    return lSql;
                }
                #endregion
            }

            /// <summary>
            /// 获取当前用户端的类型
            /// </summary>
            /// <returns></returns>
            public ModelEnum.UserTypeEnum GetUserType()
            {
                return (ModelEnum.UserTypeEnum)new DomainBasic.UserTypeApp().GetInfo().id;
            }
        }

    }

}
