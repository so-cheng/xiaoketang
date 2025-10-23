using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Services;
using WeiCode.Utility;

namespace Services.Project
{
    public partial class ServiceFactory
    {
        public partial class JoinNew
        {
            /// <summary>
            /// 快捷更改置顶次数
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction FastEditPinToTopTimes(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();

                var user_info_yunying = DoMySql.FindEntity<ModelDb.user_info_yunying>($"id='{req["id"].ToNullableString()}'", false);

                if (!user_info_yunying.IsNullOrEmpty())
                {
                    user_info_yunying.SetValue(req["name"].ToNullableString(), req["value"].ToInt());
                    user_info_yunying.Update();
                }
                return result;
            }

            /// <summary>
            /// 补人置顶（定义补人权重1000为置顶）
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction PinToTopAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_join_apply = DoMySql.FindEntityById<ModelDb.p_join_apply>(req.GetPara()["id"].ToNullableString().ToInt());

                // 校验补人申请状态
                if (p_join_apply.status != ModelDb.p_join_apply.status_enum.等待外宣补人.ToSByte()) throw new WeicodeException("当前状态不可置顶，请刷新页面");

                // 校验是否置顶过
                if (p_join_apply.weight.Equals(1000)) throw new WeicodeException("不可重复置顶");

                // 校验剩余置顶次数
                int? pintotop_times = 0;
                var user_info_yunying = DoMySql.FindEntity<ModelDb.user_info_yunying>($"yy_user_sn = '{new UserIdentityBag().user_sn}'", false);
                if (!user_info_yunying.IsNullOrEmpty())
                {
                    pintotop_times = user_info_yunying.join_pintotop_times;
                }
                if (pintotop_times <= 0) throw new WeicodeException("置顶剩余次数不足");

                // 置顶操作
                List<string> lSql = new List<string>();
                p_join_apply.weight = 1000;
                lSql.Add(p_join_apply.UpdateTran());

                user_info_yunying.join_pintotop_times = pintotop_times - 1;
                lSql.Add(user_info_yunying.UpdateTran());
                DoMySql.ExecuteSqlTran(lSql);
                // 日志
                AddApplyLog(p_join_apply.apply_sn, ModelDb.p_join_apply_log.c_type_enum.置顶);
                return result;
            }
        }
    }
}
