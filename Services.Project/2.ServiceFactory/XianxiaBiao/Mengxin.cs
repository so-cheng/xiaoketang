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

        public class MengxinService
        {
            /// <summary>
            /// 快捷更改萌新
            /// </summary>
            /// <param name="reqJson"></param>
            /// <returns></returns>
            public JsonResultAction FastEdit(JsonRequestAction reqJson)
            {
                var result = new JsonResultAction();
                var lSql = new List<string>();
                var req = reqJson.GetPara();
                var p_mengxin = DoMySql.FindEntity<ModelDb.p_mengxin>($"id='{req["id"].ToNullableString()}'", false);
                if (!p_mengxin.IsNullOrEmpty())
                {
                    p_mengxin.SetValue(req["name"].ToNullableString(), req["value"].ToInt());
                    lSql.Add(p_mengxin.UpdateTran());
                }

                if (req["name"].ToNullableString()=="join_num")
                {
                    var list = DoMySql.FindList<ModelDb.p_mengxin>($"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and date='{p_mengxin.date}' and id!='{p_mengxin.id}'");
                    foreach (var item in list)
                    {
                        item.join_num = req["value"].ToInt();
                        lSql.Add(item.UpdateTran());
                    }
                }
                MysqlHelper.ExecuteSqlTran(lSql);
                return result;
            }
        }
    }    
}
