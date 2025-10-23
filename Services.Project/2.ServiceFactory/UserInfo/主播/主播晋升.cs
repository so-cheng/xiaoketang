using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.ServiceFactory.UserService;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {

            #region 数据模型
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class PromotionZhuboModel : ModelDb.user_info_promotion_zhubo_apply
            {

                public string user_name
                {
                    get
                    {
                        return new ServiceFactory.UserInfo.Zhubo().GetZhuboInfo(user_info_zb_sn).user_name;
                    }
                }
                public string status_name
                {
                    get
                    {
                        return status.ToEnum<ModelDb.user_info_promotion_zhubo_apply.status_enum>();
                    }
                }
                public string tg_user_name
                {
                    get
                    {
                        if (tg_user_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(tg_user_sn).username;
                    }
                }

                public string ting_name
                {
                    get
                    {
                        if (ting_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Ting().GetTingBySn(ting_sn).ting_name;
                    }
                }

                public string yy_user_name
                {
                    get
                    {
                        if (yy_user_sn.IsNullOrEmpty())
                        {
                            return "";
                        }
                        return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                    }
                }
            }
            #endregion
            //主播晋升
            public class ZbPromotion
            {

                #region 查询
                /// <summary>
                /// 主播晋升列表
                /// </summary>
                /// <param name="reqJson"></param>
                /// <returns></returns>
                public ModelResult.List GetZbPromotionPageList(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                {

                    string where = " 1=1 ";

                    //查询条件
                    if (!reqJson.GetPara("keyword").IsNullOrEmpty())
                    {
                        where += $" AND (user_info_zhubo.user_name like '%{reqJson.GetPara("keyword")}%' OR user_info_promotion_zhubo_apply.dou_user like '%{reqJson.GetPara("keyword")}%')";
                    }
                    if (!reqJson.GetPara("ting_sn").IsNullOrEmpty())
                    {
                        where += $" AND user_info_promotion_zhubo_apply.ting_sn = '{reqJson.GetPara("ting_sn")}'";
                    }
                    if (!reqJson.GetPara("yy_user_sn").IsNullOrEmpty())
                    {
                        where += $" AND user_info_promotion_zhubo_apply.yy_user_sn = '{reqJson.GetPara("yy_user_sn")}'";
                    }
                    //执行查询
                    var filter = new DoMySql.Filter
                    {
                        on = "user_info_promotion_zhubo_apply.user_info_zb_sn = user_info_zhubo.user_sn",
                        where = where + " order by user_info_promotion_zhubo_apply.create_time desc "
                    };


                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_promotion_zhubo_apply, ModelDb.user_info_zhubo, PromotionZhuboModel>(filter, reqJson);
                }
                /// <summary>
                /// 根据ID查询主播晋升信息
                /// </summary>
                /// <param name="apply_sn"></param>
                /// <returns></returns>
                public PromotionZhuboModel GetZbPromotionByID(string apply_sn)
                {
                    string where = " 1=1 ";
                    where += $" AND user_info_promotion_zhubo_apply.apply_sn = '{apply_sn}'";
                    var filter = new DoMySql.Filter
                    {
                        on = "user_info_promotion_zhubo_apply.user_info_zb_sn = user_info_zhubo.user_sn",
                        where = where + " order by user_info_promotion_zhubo_apply.create_time desc "
                    };
                    PromotionZhuboModel promotionZhuboModel =
                   DoMySql.FindEntity<ModelDb.user_info_promotion_zhubo_apply>($"apply_sn = '{apply_sn}'").ToModel<PromotionZhuboModel>();
                    return promotionZhuboModel;
                }
                #endregion

                #region 增删改
                /// <summary>
                /// 提交主播晋升申请
                /// </summary>
                /// <param name="applyInfo"></param>
                public JsonResultAction CommitPromotionZhuboApply(ModelDb.user_info_promotion_zhubo_apply applyInfo)
                {

                    var result = new JsonResultAction();
                    if (applyInfo.username.IsNullOrEmpty()) throw new Exception("登录账号不可为空");
                    if (applyInfo.mobile.IsNullOrEmpty()) throw new Exception("手机号不可为空");
                    if (applyInfo.password.IsNullOrEmpty()) throw new Exception("密码不能为空");
                    if (applyInfo.jjr_name.IsNullOrEmpty()) throw new Exception("经纪人名字不能为空");
                    if (applyInfo.dou_user.IsNullOrEmpty()) throw new Exception("抖音大头号不能为空");
                    var zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn = '{applyInfo.user_info_zb_sn}'");
                    var userbase = DoMySql.FindEntity<ModelDb.user_base>($"user_sn = '{applyInfo.user_info_zb_sn}'");
                    if (applyInfo.username == userbase.username)
                    {
                        throw new Exception("登录账号不能和主播登录账号一致");
                    }

                    //调用抖音接口根据抖音账号查询抖音昵称及抖音作者id（抖音官方主播唯一身份id）
                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = applyInfo.dou_user
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("请输入正确的抖音账号");
                    }
                    applyInfo.dou_uid = dyCheckResult["data"]["anchor_id"].ToNullableString();

                    //查询抖音经纪人是否存在
                    var dyjjrParam = new
                    {
                        dou_username = applyInfo.jjr_name
                    };
                    var dyInfo = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Tg/GetJjrInfo", dyjjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();

                    if (dyInfo["code"].ToNullableString() == "1")
                    {
                        throw new WeicodeException($@"运营经营人:""{dyjjrParam.dou_username}""不存在");
                    }
                    applyInfo.jjr_uid = dyCheckResult["data"]["anchor_id"].ToNullableString();
                    //租户ID
                    applyInfo.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    //申请sn
                    string apply_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                    applyInfo.apply_sn = apply_sn;
                    //运营sn
                    applyInfo.yy_user_sn = zhubo.yy_user_sn;
                    //厅管sn
                    applyInfo.tg_user_sn = zhubo.tg_user_sn;
                    //厅sn
                    applyInfo.ting_sn = zhubo.ting_sn;
                    //状态
                    applyInfo.status = ModelDb.user_info_promotion_zhubo_apply.status_enum.等待运营审批.ToSByte();

                    applyInfo.create_time = DateTime.Now;
                    applyInfo.Insert();

                    //推送微信消息

                    // 推送公众号
                    //new ServiceFactory.Sdk.WeixinSendMsg().Approve(zhubo.yy_user_sn, "有主播晋升申请需要审核", $"http://{new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "manager").host_domain}UserInfo/ZbPromotion/ApplyList", new ServiceFactory.Sdk.WeixinSendMsg.ApproveInfo
                    //{
                    //    person = new ServiceFactory.UserInfo.Tg().GetTgInfoByUsersn(new UserIdentityBag().user_sn).name,
                    //    post_time = DateTime.Now
                    //});

                    return result;
                }
                /// <summary>
                /// 删除申请
                /// </summary>
                /// <param name="apply_sn"></param>
                public void DeletePromotionZhuboApply(string apply_sn)
                {
                    var apply = DoMySql.FindEntity<ModelDb.user_info_promotion_zhubo_apply>($"apply_sn = '{apply_sn}'");
                    if (apply == null || apply.status > ModelDb.user_info_promotion_zhubo_apply.status_enum.等待运营审批.ToSByte())
                    {
                        throw new Exception("审请信息不存在或者已审批，无法删除！");
                    }
                    apply.Delete();
                }

                /// <summary>
                /// 审批主播晋升
                /// </summary>
                /// <param name="user_info_zb_sn"></param>
                /// <param name="status"></param>
                public void PromotionZhuboApproval(string apply_sn, int status)
                {
                    List<string> lSql = new List<string>();
                    var apply = DoMySql.FindEntity<ModelDb.user_info_promotion_zhubo_apply>($"apply_sn = '{apply_sn}'");
                    if (apply == null || apply.status > ModelDb.user_info_promotion_zhubo_apply.status_enum.等待运营审批.ToSByte())
                    {
                        throw new Exception("审请信息不存在或者已审批！");
                    }
                    //同意
                    if (status == ModelDb.user_info_promotion_zhubo_apply.status_enum.同意.ToSByte())
                    {
                        //创建厅管账号
                        var relation_type = ModelEnum.UserRelationTypeEnum.运营邀厅管;
                        string user_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                        var userbase = new user_base()
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            username = apply.username,
                            mobile = apply.mobile,
                            password = apply.password,
                            user_sn = user_sn,
                            user_type_id = new DomainBasic.UserTypeApp().GetInfoByCode("tger").id.ToSByte()
                        };

                        lSql.AddRange(new ServiceFactory.UserService().PostTran(userbase, relation_type));
                        //创建厅管信息
                        var zhuboinfo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn = '{apply.user_info_zb_sn}'");//被晋升主播信息
                        lSql.Add(new ModelDb.user_info_tingguan()
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            zt_user_sn = zhuboinfo.zt_user_sn,
                            yy_user_sn = zhuboinfo.yy_user_sn,
                            tg_user_sn = user_sn,//新创建的厅管账号
                        }.InsertTran());

                        //给厅管下创建直播厅
                        lSql.Add(new ModelDb.user_info_tg
                        {
                            tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                            yy_user_sn = apply.yy_user_sn,
                            ting_sn = UtilityStatic.CommonHelper.CreateUniqueSn(),    // UtilityStatic.CommonHelper.CreateUniqueSn(),
                            tg_user_sn = user_sn,
                            ting_name = apply.username,
                            dou_user = apply.dou_user,
                            dou_UID = apply.dou_uid,
                            jjr_name = apply.jjr_name,
                            jjr_uid = apply.jjr_uid,
                            zt_user_sn = zhuboinfo.zt_user_sn
                        }.InsertTran());
                        //修改晋升申请表状态
                        apply.apply_time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        apply.status = ModelDb.user_info_promotion_zhubo_apply.status_enum.同意.ToSByte();
                        lSql.Add(apply.UpdateTran());
                    }
                    else//拒绝
                    {
                        apply.status = ModelDb.user_info_promotion_zhubo_apply.status_enum.拒绝.ToSByte();
                        lSql.Add(apply.UpdateTran());
                    }
                    DoMySql.ExecuteSqlTran(lSql);

                }
                #endregion
            }
        }
    }
}
