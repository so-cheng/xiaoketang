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
        public class VacationService
        {
            /// <summary>
            /// 更改主播请假状态
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction SetVacation(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var req = reqJson.GetPara();
                var p_jixiao_vacation = DoMySql.FindEntity<ModelDb.p_jixiao_vacation>($"zb_user_sn='{req["zb_user_sn"].ToNullableString()}' and c_date='{req["date"].ToNullableString()}'",false);
                if (p_jixiao_vacation.IsNullOrEmpty())
                {
                    new ModelDb.p_jixiao_vacation
                    {
                        tenant_id=new DomainBasic.TenantApp().GetInfo().id,
                        zb_user_sn=req["zb_user_sn"].ToNullableString(),
                        tg_user_sn=new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.厅管邀主播, req["zb_user_sn"].ToNullableString()),
                        new_zb_user_sn= req["new_zb_user_sn"].ToNullableString(),
                        c_date=req["date"].ToNullableString().ToDate(),
                        cause= req["cause"].ToNullableString()
                    }.Insert();
                }

                return result;
            }

            public JsonResultAction EPT(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                
                return result;
            }
        }
    }    
}
