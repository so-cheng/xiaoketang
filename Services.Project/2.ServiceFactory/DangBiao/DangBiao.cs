using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;
using static Services.Project.PageFactory.DangBiao;
using System.Reflection;
using WeiCode.Domain;
using WeiCode.ModelDbs;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public class DangBiao
        {
            #region 档表
            /// <summary>
            /// 根据id获取档表信息
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public DangBiaoInfo GetInfoById(int id)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"id = {id}", false).ToModel<DangBiaoInfo>();
                return table;
            }
            /// <summary>
            /// 根据sn获取档表信息
            /// </summary>
            /// <param name="sn"></param>
            /// <returns></returns>
            public DangBiaoInfo GetInfoBySn(string sn)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"db_sn = '{sn}'", false).ToModel<DangBiaoInfo>();
                return table;
            }

            /// <summary>
            /// 根据筛选条件获取档表信息
            /// </summary>
            /// <param name="where"></param>
            /// <returns></returns>
            public DangBiaoInfo GetInfoByString(string where)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"{where}", false).ToModel<DangBiaoInfo>();
                return table;
            }
            #endregion

            #region  档表明细
            /// <summary>
            /// 根据id获取档表明细信息
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public DangBiaoItemInfo GetItemInfoById(int id)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"id = {id}", false).ToModel<DangBiaoItemInfo>();
                return table;
            }
            /// <summary>
            /// 根据sn获取档表明细信息
            /// </summary>
            /// <param name="sn"></param>
            /// <returns></returns>
            public DangBiaoItemInfo GetItemInfoBySn(string sn)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"db_sn = '{sn}'", false).ToModel<DangBiaoItemInfo>();
                return table;
            }

            /// <summary>
            /// 根据筛选条件获取档表明细信息
            /// </summary>
            /// <param name="where"></param>
            /// <returns></returns>
            public DangBiaoItemInfo GetItemInfoByString(string where)
            {
                var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"{where}", false).ToModel<DangBiaoItemInfo>();
                return table;
            }
            #endregion

            /// <summary>
            /// 获取主播时长明细数据
            /// </summary>
            /// <param name="vo"></param>
            /// <returns></returns>
            public Dictionary<string, ZBItem> GetZBHourDetails(TableVO vo, bool isTg = false)
            {
                //1 获取当前厅管下的主播
                var zb_users = new List<ServiceFactory.UserInfo.Zhubo.ZbBaseInfo>();
                if (isTg)
                {
                    zb_users = new ServiceFactory.UserInfo.Tg().TgGetNextZb(vo.tg_user_sn);
                }
                else
                {
                    var tgsn = new ServiceFactory.DangBiao().GetInfoById(vo.table_id).tg_user_sn;
                    zb_users = new ServiceFactory.UserInfo.Tg().TgGetNextZb(tgsn);
                }
                var dictionary = new Dictionary<string, ZBItem>();
                foreach (var zb in zb_users)
                {
                    if (!dictionary.ContainsKey(zb.user_sn))
                    {
                        var item = new ZBItem()
                        {
                            user_sn = zb.user_sn,
                            username = zb.username,
                            name = zb.name,
                            attach2 = zb.position
                        };
                        dictionary[zb.user_sn] = item;
                    }
                }

                //2 获取每个主播的时长
                Type type = typeof(TableReqDto);
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (var property in propertyInfos)
                {
                    if (property.PropertyType == typeof(string[]) && property.CanRead)
                    {
                        var row = property.GetValue(vo) as string[];
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (dictionary.ContainsKey(row[i]))
                            {
                                if (i < 3) dictionary[row[i]].zfp_hours++;
                                else dictionary[row[i]].gw_hours++;
                            }
                        }
                    }
                }
                return dictionary;
            }

            public class DangBiaoInfo : ModelDb.p_jixiao_dangbiao
            {

            }
            public class DangBiaoItemInfo : ModelDb.p_jixiao_dangbiao_item
            {

            }
        }
    }
}
