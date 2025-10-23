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
        public class HelpService
        {
            /// <summary>
            /// 获取链接
            /// </summary>
            /// <returns></returns>
            public string GetLink(string code)
            {
                var host_domain = new DomainBasic.TenantDomainApp().GetInfoByTenantId(new DomainBasic.TenantApp().GetInfo().id, "zber").host_domain;
                return "http://" + host_domain + $"/Help/Center/Detail?code={code}";
            }
        }
    }
}