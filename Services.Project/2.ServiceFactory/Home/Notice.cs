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
        /// 消息提醒
        /// </summary>
        public class NoticeService
        {
            /// <summary>
            /// 添加一条消息记录
            /// </summary>
            /// <param name="category">消息类型</param>
            /// <param name="user_sn">消息接收人sn</param>
            /// <param name="title">标题</param>
            /// <param name="content">内容</param>
            /// <param name="link_url">跳转链接</param>
            public void AddNoticeLog(CategoryEnum category,string user_sn,string title,string content,string link_url)
            {
                new ModelDb.sys_notice()
                {
                    tenant_id = new DomainBasic.TenantApp().GetInfo().id,
                    category_id = category.ToSByte(),
                    user_sn = user_sn,
                    title = title,
                    content = content,
                    is_read = 0,
                    link_text = "查看详情",
                    link_url =link_url
                }.Insert();
            }

            /// <summary>
            /// 已读一条记录
            /// </summary>
            public ModelDb.sys_notice ReadNotice(int id)
            {
                var sys_notice = DoMySql.FindEntityById<ModelDb.sys_notice>(id);
                if (sys_notice.IsNullOrEmpty())
                {
                    throw new Exception("消息不存在");
                }
                if (sys_notice.user_sn != new UserIdentityBag().user_sn)
                {
                    throw new Exception("当前用户无权查看");
                }
                sys_notice.is_read = ModelDb.sys_notice.is_read_enum.已读.ToSByte();
                sys_notice.Update();
                return sys_notice;
            }

            public enum CategoryEnum
            {
                审批提醒=1,
                公会消息=2,
                系统更新提示=3,
                数据异常=4,
                活动提醒=5,
                目标待办=6,
                公会任务=7,
            }
        }    
    }    
}
