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
        public partial class Join
        {
            /// <summary>
            /// 萌新排序
            /// </summary>
            public class MengxinSortService
            {
                #region 设置主播排序
                /// <summary>
                /// 设置主播的排序
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                /// <returns></returns>
                public ModelDb.user_info_zb SetZbSort(int user_info_zb_id)
                {
                    /***
                     * 排序规则:
                     * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                     */
                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return SetZbSort(zb);
                }

                public ModelDb.user_info_zb SetZbSort(ModelDb.user_info_zb zb)
                {
                    /***
                    * 排序规则:
                    * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                    */
                    var NewTerm = DoMySql.FindEntity<ModelDb.p_mengxin>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' order by create_time desc");
                    if (zb.is_change == ModelDb.user_info_zb.is_change_enum.换厅.ToInt())
                    {
                        return SetChangeZbSortForEntity(zb);
                    }
                    if (zb.is_fast == ModelDb.user_info_zb.is_fast_enum.加急.ToInt())
                    {
                        return SetFastZbSortForEntity(zb);
                    }
                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }

                    if (zb.zb_level == "C" || zb.zb_level == "D")
                    {
                        return ResetSort(zb);
                    }

                    if (zb.zb_level == "A" || zb.zb_level == "B")
                    {
                        if (zb.term == NewTerm.term)
                        {
                            return SetNewZbSortForEntity(zb);
                        }
                        else
                        {
                            return SetOldZbSortForEntity(zb);
                        }
                    }

                    return zb;

                }
                #endregion

                #region 设置换厅
                /// <summary>
                /// 设置加急主播的排序
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                /// <returns></returns>
                public ModelDb.user_info_zb SetChangeZbSortForEntity(int user_info_zb_id)
                {
                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return SetFastZbSortForEntity(zb);
                }
                private ModelDb.user_info_zb SetChangeZbSortForEntity(ModelDb.user_info_zb zb)
                {
                    /***
                     * 排序规则:
                     * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                     */
                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }
                    zb.supplement_sort = 0;
                    return zb;
                }
                #endregion

                #region 设置加急
                /// <summary>
                /// 设置加急主播的排序
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                /// <returns></returns>
                public ModelDb.user_info_zb SetFastZbSortForEntity(int user_info_zb_id)
                {
                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return SetFastZbSortForEntity(zb);
                }
                private ModelDb.user_info_zb SetFastZbSortForEntity(ModelDb.user_info_zb zb)
                {
                    /***
                     * 排序规则:
                     * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                     */
                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }
                    var NewTerm = DoMySql.FindEntity<ModelDb.p_mengxin>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' order by create_time desc");
                    if (zb.term == NewTerm.term)
                    {
                        throw new Exception("禁止加急");
                    }
                    if (zb.zb_level == "A")
                    {
                        zb.supplement_sort = 2;
                    }
                    if (zb.zb_level == "B")
                    {
                        zb.supplement_sort = 4;
                    }
                    return zb;
                }
                #endregion

                #region 设置新一期
                /// <summary>
                /// 设置新一期主播排序
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                /// <returns></returns>
                public ModelDb.user_info_zb SetNewZbSortForEntity(int user_info_zb_id)
                {

                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return SetNewZbSortForEntity(zb);
                }
                private ModelDb.user_info_zb SetNewZbSortForEntity(ModelDb.user_info_zb zb)
                {
                    /***
                     * 排序规则:
                     * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                     */
                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }
                    if (zb.zb_level == "A")
                    {
                        zb.supplement_sort = 1;
                    }
                    if (zb.zb_level == "B")
                    {
                        zb.supplement_sort = 3;
                    }
                    return zb;
                }
                #endregion

                #region 设置往期
                /// <summary>
                /// 设置新一期主播排序
                /// </summary>
                /// <param name="user_info_zb_id"></param>
                /// <returns></returns>
                public ModelDb.user_info_zb SetOldZbSortForEntity(int user_info_zb_id)
                {
                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return SetOldZbSortForEntity(zb);
                }
                private ModelDb.user_info_zb SetOldZbSortForEntity(ModelDb.user_info_zb zb)
                {
                    /***
                     * 排序规则:
                     * 换厅0 > 最新期的A级1 > 往期加急A级2 > 最新期的B级3 > 往期加急B级4 > 往期A级5 > 往期B级6
                     */

                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }
                    if (zb.zb_level == "A")
                    {
                        zb.supplement_sort = 5;
                    }
                    if (zb.zb_level == "B")
                    {
                        zb.supplement_sort = 6;
                    }
                    return zb;
                }
                #endregion

                #region 重置排序
                public ModelDb.user_info_zb ResetSort(int user_info_zb_id)
                {
                    var zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{user_info_zb_id}'", false);
                    return ResetSort(zb);

                }
                private ModelDb.user_info_zb ResetSort(ModelDb.user_info_zb zb)
                {
                    if (zb.id <= 0)
                    {
                        throw new Exception("主播不存在");
                    }
                    zb.supplement_sort = 100;
                    return zb;
                }
                #endregion

            }
        }
    }
}
