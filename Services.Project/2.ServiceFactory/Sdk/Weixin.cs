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
        public partial class Sdk
        {
            public class WeixinSendMsg
            {
                /// <summary>
                /// OA审批提醒
                /// </summary>
                public void Approve(string user_sn, string content, string url, ApproveInfo approveInfo)
                {
                    string open_id = new DomainBasic.UserApp().GetInfoByUserSn(user_sn).attach4;
                    var para = new
                    {
                        thing1 = approveInfo.person,
                        time2 = approveInfo.post_time
                    };
                    new PlatformSdk.WeixinMP().SendTemplateMessage("GNLsQPuQDHNGjSw9qfPfJcKh3K9jpeDWLkrj7UV3cbU", open_id, content, url, para.ToJson());
                }
                public class ApproveInfo
                {
                    /// <summary>
                    /// 申请人
                    /// </summary>
                    public string person { get; set; }
                    /// <summary>
                    /// 提交时间
                    /// </summary>
                    public DateTime post_time { get; set; }
                }

                /// <summary>
                /// 报名成功提醒
                /// </summary>
                /// <param name="user_sn"></param>
                /// <param name="content">暂时不传</param>
                /// <param name="url"></param>
                /// <param name="applyInfo"></param>
                public void Apply(string user_sn, string content, string url, ApplyInfo applyInfo)
                {
                    string open_id = new DomainBasic.UserApp().GetInfoByUserSn(user_sn).attach4;
                    var para = new
                    {
                        thing2 = applyInfo.theme,
                        time3 = applyInfo.post_time,
                        thing8 = applyInfo.person
                    };
                    new PlatformSdk.WeixinMP().SendTemplateMessage("YJYRqtUvu_TRltYBdlNNcZyUQjLxkVlb8uZw0MZoDQY", open_id, content, url, para.ToJson());
                }
                public class ApplyInfo
                {
                    /// <summary>
                    /// 主题
                    /// </summary>
                    public string theme { get; set; }
                    /// <summary>
                    /// 提交时间
                    /// </summary>
                    public DateTime post_time { get; set; }
                    /// <summary>
                    /// 申请人
                    /// </summary>
                    public string person { get; set; }
                }

                /// <summary>
                /// 工单撤销提醒
                /// </summary>
                /// <param name="user_sn"></param>
                /// <param name="content">暂时不传</param>
                /// <param name="url"></param>
                /// <param name="workOrderCancelInfo"></param>
                public void WorkOrderCancel(string user_sn, string content, string url, WorkOrderCancelInfo workOrderCancelInfo)
                {
                    string open_id = new DomainBasic.UserApp().GetInfoByUserSn(user_sn).attach4;
                    var para = new
                    {
                        thing5 = workOrderCancelInfo.name,
                        time3 = workOrderCancelInfo.cancel_time,
                        character_string4 = workOrderCancelInfo.number
                    };
                    new PlatformSdk.WeixinMP().SendTemplateMessage("hGMzMQ9t3UCQi-TgeAnnu-YOb-ioFdJHHwF-r1sQvk8", open_id, content, url, para.ToJson());
                }
                public class WorkOrderCancelInfo
                {
                    /// <summary>
                    /// 工单名称
                    /// </summary>
                    public string name { get; set; }
                    /// <summary>
                    /// 撤销时间
                    /// </summary>
                    public DateTime cancel_time { get; set; }
                    /// <summary>
                    /// 工单编号
                    /// </summary>
                    public string number { get; set; }
                }
            }
        }
    }
}