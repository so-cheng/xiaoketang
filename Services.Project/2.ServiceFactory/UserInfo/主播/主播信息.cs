using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;
using static Services.Project.PageFactory.UserInfo;
using static Services.Project.PageFactory.UserInfo.ReplaceDouUserName;
using static Services.Project.ServiceFactory.Sdk.WeixinSendMsg;
using static Services.Project.ServiceFactory.UserInfo;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class UserInfo
        {
            public class Zhubo
            {
                #region 单个查询
                /// <summary>
                /// 获取主播信息
                /// </summary>
                /// <param name="user_sn"></param>
                /// <returns></returns>
                public ZbBaseInfo GetZhuboInfo(string user_sn)
                {
                    var zbInfo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{user_sn}'").ToModel<ZbBaseInfo>();
                    if (zbInfo.IsNullOrEmpty())
                    {
                        return new ZbBaseInfo();
                    }
                    return zbInfo;
                }

                /// <summary>
                /// 主播信息实体类
                /// </summary>
                public class ZhuboInfo1 : ModelDb.user_info_zhubo
                {
                    public ZhuboInfo1()
                    {
                        _color = new DomainBasic.DictionaryApp().GetDescriptionFromValue("职务", this.position);
                    }
                    /// <summary>
                    /// 登录账号
                    /// </summary>
                    public string username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.user_sn).username;
                        }
                    }

                    public string name
                    {
                        get
                        {
                            return this.user_name;
                        }
                    }

                    public bool is_newer
                    {
                        get
                        {
                            //主播所属运营配置的新人判定天数
                            int? newer_days = 5;
                            var user_info_yy_newer = DoMySql.FindEntity<ModelDb.user_info_yy_newer>($"yy_user_sn = '{this.yy_user_sn}'", false);
                            if (!user_info_yy_newer.IsNullOrEmpty())
                            {
                                newer_days = user_info_yy_newer.in_days;
                            }
                            //新人:账号创建日期>当前日期-运营配置的新人天数
                            return this.create_time.ToDate() > DateTime.Today.AddDays(-1 * newer_days.ToDouble()).ToDate();
                        }
                    }

                    private string _color;

                    // <summary>
                    /// 职务对应颜色
                    /// </summary>
                    public string color
                    {
                        get
                        {
                            return _color;
                        }
                    }

                    public string db_color { get; set; }
                }
                #endregion

                #region 统合查询

                /// <summary>
                /// 获取主播的sql语句where
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                private string GetBaseInfosForWhere(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string where = $"tenant_id = '{new DomainBasic.TenantApp().GetInfo().id}'";

                    switch (zbBaseInfoFilter.status)
                    {
                        case ZbBaseInfoFilter.Status.全部:
                            break;
                        case ZbBaseInfoFilter.Status.正常:
                            where += $" and status = '{ModelDb.user_info_zhubo.status_enum.正常.ToInt()}'";
                            break;
                        case ZbBaseInfoFilter.Status.待开账号:
                            where += $" and status = '{ModelDb.user_info_zhubo.status_enum.待开账号.ToInt()}'";
                            break;
                        default:
                            break;
                    }

                    switch (zbBaseInfoFilter.attachUserType.userType)
                    {
                        case ZbBaseInfoFilter.AttachUserType.UserType.基地:
                            where += $" and zt_user_sn = '{zbBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case ZbBaseInfoFilter.AttachUserType.UserType.运营:
                            where += $" and yy_user_sn = '{zbBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case ZbBaseInfoFilter.AttachUserType.UserType.厅管:
                            where += $" and tg_user_sn = '{zbBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case ZbBaseInfoFilter.AttachUserType.UserType.厅:
                            where += $" and ting_sn = '{zbBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case ZbBaseInfoFilter.AttachUserType.UserType.主播:
                            where += $" and user_sn = '{zbBaseInfoFilter.attachUserType.UserSn}'";
                            break;
                        case ZbBaseInfoFilter.AttachUserType.UserType.当前登录账号:
                            where += $" and user_sn = '{new UserIdentityBag().user_sn}'";
                            break;
                        default:
                            break;
                    }

                    if (!zbBaseInfoFilter.attachWhere.IsNullOrEmpty())
                    {
                        where += $" and ({zbBaseInfoFilter.attachWhere})";
                    }

                    return where;
                }

                /// <summary>
                /// 获取主播usersn的sql语句
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public string GetBaseInfosForSql(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string where = GetBaseInfosForWhere(zbBaseInfoFilter);
                    return $"(select user_sn from user_info_zhubo where {where})";
                }


                /*/// <summary>
                /// 获取单个主播基础信息
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public ZbBaseInfo GetBaseInfo(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string sql = GetBaseInfosForWhere(zbBaseInfoFilter);
                    return DoMySql.FindEntity<ModelDb.user_info_zhubo>(sql, false).ToModel<ZbBaseInfo>();
                }*/

                /// <summary>
                /// 获取多个主播基础信息
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ZbBaseInfo> GetBaseInfos(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string sql = GetBaseInfosForWhere(zbBaseInfoFilter);
                    return DoMySql.FindList<ModelDb.user_info_zhubo, ZbBaseInfo>(sql);
                }

                /// <summary>
                /// 获取主播信息字典结构
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public Dictionary<string, string> GetBaseInfosForKv(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    string sql = GetBaseInfosForWhere(zbBaseInfoFilter);
                    var ZhuboInfo = DoMySql.FindKvList<ModelDb.user_info_zhubo>(sql, "user_name, user_sn");
                    return ZhuboInfo;
                }

                /// <summary>
                /// 获取主播信息选项结构
                /// </summary>
                /// <param name="zbBaseInfoFilter"></param>
                /// <returns></returns>
                public List<ModelDoBasic.Option> GetBaseInfosForOption(ZbBaseInfoFilter zbBaseInfoFilter)
                {
                    var options = new List<ModelDoBasic.Option>();

                    foreach (var item in GetBaseInfos(zbBaseInfoFilter))
                    {
                        options.Add(new ModelDoBasic.Option()
                        {
                            text = item.name,
                            value = item.user_sn,
                        });
                    }
                    return options;
                }
                /// <summary>
                /// 过滤条件
                /// </summary>
                public class ZbBaseInfoFilter
                {
                    /// <summary>
                    /// 自定义附加where条件
                    /// </summary>
                    public string attachWhere { get; set; }
                    public Status status { get; set; } = ZbBaseInfoFilter.Status.正常;
                    public enum Status
                    {
                        全部,
                        正常,
                        待开账号
                    }

                    /// <summary>
                    /// 根据上级用户类型筛选
                    /// </summary>
                    public AttachUserType attachUserType { get; set; } = new AttachUserType();
                    public class AttachUserType
                    {
                        public UserType userType { get; set; } = UserType.全部;
                        public enum UserType
                        {
                            全部,
                            基地,
                            运营,
                            厅管,
                            厅,
                            主播,
                            当前登录账号
                        }
                        public string UserSn { get; set; }
                    }
                }
                /// <summary>
                /// 主播信息实体类
                /// </summary>
                public class ZbBaseInfo : ModelDb.user_info_zhubo
                {
                    public ZbBaseInfo()
                    {
                        _color = new DomainBasic.DictionaryApp().GetDescriptionFromValue("职务", this.position);
                    }

                    public string name
                    {
                        get
                        {
                            return user_name;
                        }
                    }

                    private string _color;

                    // <summary>
                    /// 职务对应颜色
                    /// </summary>
                    public string color
                    {
                        get
                        {
                            return _color;
                        }
                    }

                    public bool is_newer
                    {
                        get
                        {
                            //主播所属运营配置的新人判定天数
                            int? newer_days = 5;
                            var user_info_yy_newer = DoMySql.FindEntity<ModelDb.user_info_yy_newer>($"yy_user_sn = '{this.yy_user_sn}'", false);
                            if (!user_info_yy_newer.IsNullOrEmpty())
                            {
                                newer_days = user_info_yy_newer.in_days;
                            }
                            //新人:账号创建日期>当前日期-运营配置的新人天数
                            return this.create_time.ToDate() > DateTime.Today.AddDays(-1 * newer_days.ToDouble()).ToDate();
                        }
                    }

                    /// <summary>
                    /// 登录账号
                    /// </summary>
                    public string username
                    {
                        get
                        {
                            return new DomainBasic.UserApp().GetInfoByUserSn(this.user_sn).username;
                        }
                    }
                    public string db_color { get; set; }
                }
                #endregion

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
                /// 判断主播某日期是否请假
                /// </summary>
                /// <param name="zb_user_sn"></param>
                /// <returns></returns>
                public bool isZbVacation(string zb_user_sn, DateTime date)
                {
                    var vacation = GetZbVacation(zb_user_sn, date, date);
                    return vacation > 0;
                }

                /// <summary>
                /// 删除主播(设置主播状态为逻辑删除)
                /// </summary>
                /// <param name="zb_user_sn"></param>
                public void DeleteZb(string zb_user_sn)
                {

                    var user_base = new DomainBasic.UserApp().GetInfoByUserSn(zb_user_sn);
                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{zb_user_sn}'", false);
                    var lSql = new List<string>();

                    if (!user_base.IsNullOrEmpty() && user_base.user_type_id == new DomainBasic.UserTypeApp().GetInfoByCode("zber").id)
                    {
                        user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                        lSql.Add(user_base.UpdateTran());
                    }

                    if (!user_info_zhubo.IsNullOrEmpty())
                    {
                        user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.逻辑删除.ToSByte();
                        lSql.Add(user_info_zhubo.UpdateTran());
                    }

                    MysqlHelper.ExecuteSqlTran(lSql);

                }
                /// <summary>
                /// 召回主播
                /// </summary>
                /// <param name="zb_user_sn"></param>
                public void RecallZb(string zb_user_sn)
                {
                    List<string> lSql = new List<string>();
                    var user_base = new DomainBasic.UserApp().GetInfoByUserSn(zb_user_sn);
                    var user_info_zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{zb_user_sn}'", false);

                    //判断主播和主播账号是否存在
                    if (user_base == null)
                    {
                        throw new Exception("主播账号不存在！");
                    }
                    if (user_info_zhubo == null)
                    {
                        throw new Exception("主播信息不存在！");
                    }
                    //判断主播的直播厅状态

                    var ting = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn = '{user_info_zhubo.ting_sn}'");
                    if (ting.status == null || ting.status != 0)
                    {
                        throw new Exception("主播所属直播厅异常！");
                    }
                    //修改主播状态为正常
                    user_info_zhubo.status = ModelDb.user_info_zhubo.status_enum.正常.ToSByte();
                    lSql.Add(user_info_zhubo.UpdateTran());
                    //添加主播召回日志
                    lSql.Add(AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum.召回, "召回主播", user_info_zhubo));
                    //修改账号状态
                    user_base.status = ModelDb.user_base.status_enum.正常.ToSByte();
                    lSql.Add(user_base.UpdateTran());
                    DoMySql.ExecuteSqlTran(lSql);
                }

                /// <summary>
                /// 更换接档号
                /// </summary>
                /// <param name="zhubo"></param>
                /// <exception cref="Exception"></exception>
                public void ReplaceDouUserName(ReplaceDouUserName.DtoReq zhubo)
                {

                    var zhuo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'", false);
                    var ting = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn='{zhuo.ting_sn}'", false);
                    if (zhubo.dou_username.IsNullOrEmpty())
                    {
                        throw new Exception($"抖音号必填!");
                    }
                    if (zhubo.mobile.IsNullOrEmpty())
                    {
                        throw new Exception($"手机后四位必填!");
                    }
                    if (zhubo.name.IsNullOrEmpty())
                    {
                        throw new Exception($"真实姓名必填！");
                    }
                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = zhubo.dou_username.ToNullableString()
                    };
                    dynamic dyCheckResult = VerificationDoUser(zhubo.dou_username.ToNullableString());

                    if (ting == null)
                    {
                        throw new Exception("当前主播没有经纪人!");
                    }
                    //调用签约接口
                    QianYue(dyCheckResult.anchor_id, ting.jjr_uid, zhubo.name.ToNullableString(), zhubo.mobile.ToNullableString());
                    //更改主播抖音号   
                    zhuo.dou_username = zhubo.dou_username;
                    zhuo.anchor_id = dyCheckResult["data"]["anchor_id"].ToNullableString();
                    zhuo.Update();
                }
                /// <summary>
                /// 绑定运营经纪人
                /// </summary>
                /// <param name="jjr_username"></param>
                public void Bindingjjr(string jjr_username)
                {
                    var zhuo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_sn='{new UserIdentityBag().user_sn}'", false);
                    var ting = DoMySql.FindEntity<ModelDb.user_info_tg>($"ting_sn='{zhuo.ting_sn}'", false);
                    if (jjr_username.IsNullOrEmpty())
                    {
                        throw new Exception($"经纪人名称不能为空!");
                    }

                    var dyCheckResult = VerificationDoUser(zhuo.dou_username.ToNullableString());

                    //查询经纪人信息
                    var dyjjrParam = new dyQianyueParam()
                    {
                        jjr_name = jjr_username,
                    };
                    var dyqianyueapi = VerificationJjr(jjr_username);

                    //分配运营经纪人

                    SetJjr(zhuo.anchor_id, dyqianyueapi.anchor_id);

                    ting.jjr_uid = dyqianyueapi["data"]["anchor_id"].ToNullableString();
                    ting.jjr_name = jjr_username;
                    ting.Update();
                }

                /// <summary>
                /// 验证主播
                /// </summary>
                /// <param name="dou_username"></param>
                /// <returns></returns>
                /// <exception cref="Exception"></exception>
                public dynamic VerificationDoUser(string dou_username)
                {
                    dynamic dynamic = null;

                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy = new ServiceFactory.UserInfo.Yy().GetInfoById(new UserIdentityBag().id);
                            if (yy.user_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;

                        case ModelEnum.UserTypeEnum.tger:
                            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (tg.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        case ModelEnum.UserTypeEnum.zber:
                            var zhubo = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (zhubo.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        default:
                            break;
                    }
                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = dou_username.ToNullableString()
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetInfo", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    //暂时取消校验
                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("抖音号输入错误!");
                    }
                    dynamic.anchor_id = dyCheckResult["data"]["anchor_id"].ToNullableString();
                    return dynamic;
                }
                /// <summary>
                /// 查询验证抖音经纪人
                /// </summary>
                /// <param name="jjr_name"></param>
                /// <returns></returns>
                /// <exception cref="Exception"></exception>
                public dynamic VerificationJjr(string jjr_name)
                {
                    dynamic dynamic = null;
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy = new ServiceFactory.UserInfo.Yy().GetInfoById(new UserIdentityBag().id);
                            if (yy.user_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;

                        case ModelEnum.UserTypeEnum.tger:
                            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (tg.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        case ModelEnum.UserTypeEnum.zber:
                            var zhubo = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (zhubo.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        default:
                            break;
                    }
                    var dyjjrParam = new dyQianyueParam()
                    {
                        jjr_name = jjr_name,
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Tg/GetJjrInfo", dyjjrParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    //暂时取消校验
                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("没有找到此运营经纪人!");
                    }
                    dynamic.anchor_id = dyCheckResult["data"]["anchor_id"].ToNullableString();

                    return dynamic;
                }
                /// <summary>
                /// 分配运营经纪人
                /// </summary>
                /// <param name="anchor_id"></param>
                /// <param name="broker_id"></param>
                /// <exception cref="Exception"></exception>
                public void SetJjr(string anchor_id, string broker_id)
                {
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy = new ServiceFactory.UserInfo.Yy().GetInfoById(new UserIdentityBag().id);
                            if (yy.user_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;

                        case ModelEnum.UserTypeEnum.tger:
                            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (tg.yy_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;
                        case ModelEnum.UserTypeEnum.zber:
                            var zhubo = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (zhubo.yy_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                    var dyjjrpostParam = new dyQianyueParam()
                    {
                        anchor_id = anchor_id,
                        broker_id = broker_id
                    };
                    var dysetjjrapi = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/SetTg", dyjjrpostParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    if (dysetjjrapi["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("运营经纪人分配失败，请联系管理员!");
                    }
                }
                /// <summary>
                /// 通过抖音号获取经纪人信息
                /// </summary>
                /// <param name="dou_username"></param>
                /// <returns></returns>
                /// <exception cref="Exception"></exception>
                public dynamic GetBrokerByAnchorId(string dou_username)
                {
                    dynamic dynamic = null;
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy = new ServiceFactory.UserInfo.Yy().GetInfoById(new UserIdentityBag().id);
                            if (yy.user_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;

                        case ModelEnum.UserTypeEnum.tger:
                            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (tg.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        case ModelEnum.UserTypeEnum.zber:
                            var zhubo = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (zhubo.yy_sn == "20240926155218103-35597493")
                            {
                                return dynamic;
                            }
                            break;
                        default:
                            break;
                    }
                    var dyParam = new ServiceFactory.JoinNew.dyCheckParam()
                    {
                        dou_username = dou_username.ToNullableString()
                    };
                    var dyCheckResult = UtilityStatic.HttpHelper.HttpPost("http://api.douyinxkt.cn/UserInfo/Zb/GetBrokerByAnchorId", dyParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    //暂时取消校验
                    if (dyCheckResult["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("抖音号输入错误!");
                    }
                    dynamic.agent_name = dyCheckResult["data"]["agent_name"].ToNullableString();
                    dynamic.agent_id = dyCheckResult["data"]["agent_id"].ToNullableString();

                    return dynamic;

                }


                /// <summary>
                /// 签约
                /// </summary>
                /// <param name="anchor_id"></param>
                /// <param name="broker_id"></param>
                /// <param name="name"></param>
                /// <param name="mobile"></param>
                /// <exception cref="Exception"></exception>
                public void QianYue(string anchor_id, string broker_id, string name, string mobile)
                {
                    switch (new ServiceFactory.UserInfo().GetUserType())
                    {
                        case ModelEnum.UserTypeEnum.yyer:
                            var yy = new ServiceFactory.UserInfo.Yy().GetInfoById(new UserIdentityBag().id);
                            if (yy.user_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;

                        case ModelEnum.UserTypeEnum.tger:
                            var tg = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (tg.yy_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;
                        case ModelEnum.UserTypeEnum.zber:
                            var zhubo = new ServiceFactory.UserInfo.Tg().GetInfoById(new UserIdentityBag().id);
                            if (zhubo.yy_sn == "20240926155218103-35597493")
                            {
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                    //调用签约接口
                    var dyqianyueParam = new dyQianyueParam()
                    {
                        anchor_id = anchor_id,
                        jjranchor_id = broker_id,
                        real_name = name,
                        last_four_number = mobile
                    };
                    var dyqianyueapi = UtilityStatic.HttpHelper.HttpPost("http://api.xiaoketang.com/UserInfo/Tg/InnviteAnchor", dyqianyueParam.ToJson(), new UtilityStatic.HttpHelper.HttpPostReq
                    {
                        contentType = UtilityStatic.HttpHelper.HttpPostReq.ContentType.PayLoad
                    }).ToJObject();
                    if (dyqianyueapi["code"].ToNullableString().Equals("1"))
                    {
                        throw new Exception("抖音签约失败，请联系管理员!");
                    }


                }

                #region 日志
                /// <summary>
                /// 添加主播日志
                /// </summary>
                /// <param name="c_type">操作类型</param>
                /// <param name="content">日志内容</param>
                /// <param name="user_info_zb_sn"></param>
                /// <returns></returns>
                public string AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum c_type, string content, string user_info_zb_sn)
                {
                    var user_Info_Zhubo = DoMySql.FindEntity<ModelDb.user_info_zhubo>($"user_info_zb_sn = '{user_info_zb_sn}'");
                    return AddZhuboLog(c_type, content, user_Info_Zhubo);
                }

                /// <summary>
                /// 添加主播日志
                /// </summary>
                /// <param name="c_type">操作类型</param>
                /// <param name="content">日志内容</param>
                /// <param name="user_Info_Zhubo">主播实体</param>
                /// <returns></returns>
                public string AddZhuboLog(ModelDb.user_info_zhubo_log.c_type_enum c_type, string content, ModelDb.user_info_zhubo user_Info_Zhubo)
                {
                    return new ModelDb.user_info_zhubo_log()
                    {
                        tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                        c_type = c_type.ToSByte(),
                        zb_status = user_Info_Zhubo.status.ToSByte(),
                        user_info_zb_sn = user_Info_Zhubo.user_info_zb_sn,
                        content = content,
                        user_type_id = new DomainBasic.UserApp().GetInfoByUserSn(new UserIdentityBag().user_sn).user_type_id,
                        user_sn = new UserIdentityBag().user_sn,
                    }.InsertTran();
                }
                #endregion
            }
        }
    }
}
