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
        /// <summary>
        /// 补人处理
        /// </summary>
        public class JoinService
        {
            /// <summary>
            /// 厅管申请操作日志
            /// </summary>
            /// <param name="need_id"></param>
            /// <param name="e"></param>
            /// <param name="content"></param>
            public void AddTgNeedLog(int need_id,Enum e,string content="")
            {
                var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(need_id,false);
                content = $"操作人'{new UserIdentityBag().username}'在'{DateTime.Now}'{e.ToString()}了'{p_join_need.tg_username}'在'{p_join_need.create_time}'提交的申请";
                new ModelDb.p_join_need_log()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    need_id=need_id,
                    user_sn=new UserIdentityBag().user_sn,
                    content=content,
                    user_type_id=new DomainBasic.UserTypeApp().GetInfo().id,
                    c_type = e.ToSByte(),
                }.Insert();
            }

            /// <summary>
            /// 删除申请
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(req.GetPara()["id"].ToNullableString().ToInt());
                if (p_join_need.supplement_count > 0)
                {
                    throw new Exception("已被分配主播，禁止删除");
                }
                new ServiceFactory.JoinService().AddTgNeedLog(p_join_need.id, ModelDb.p_join_need_log.c_type_enum.删除);
                p_join_need.Delete();
                return result;
            }

            /// <summary>
            /// 取消申请
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public JsonResultAction CancelAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var p_join_need = DoMySql.FindEntityById<ModelDb.p_join_need>(req.GetPara()["id"].ToNullableString().ToInt());

                //如果存在未拉群的主播，禁止取消
                if (!DoMySql.FindEntity<ModelDb.user_info_zb>($"tg_need_id='{p_join_need.id}' and is_qun='{ModelDb.user_info_zb.is_qun_enum.未拉群.ToSByte()}'", false).IsNullOrEmpty())
                {
                    throw new Exception("还有未完成拉群的主播,禁止取消");
                }

                p_join_need.status = ModelDb.p_join_need.status_enum.已取消.ToSByte();
                p_join_need.Update();
                new ServiceFactory.JoinService().AddTgNeedLog(p_join_need.id, ModelDb.p_join_need_log.c_type_enum.取消);
                return result;
            }
        }
    }
}
