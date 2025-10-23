using Services.Project;
using System.Linq;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.UserInfo.Controllers
{
    public class ZhuboController : BaseLoginController
    {
        #region 主播名单
        public ActionResult OnJobList()
        {
            var req = new PageFactory.UserInfo.OnJobList.DtoReq();
            var pageModel = new PageFactory.UserInfo.OnJobList().Get(req);          
            pageModel.listDisplay.listData.attachFilterSql = $"zt_user_sn = '{new UserIdentityBag().user_sn}'";
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    url = "Post",
                    field_paras = "id"
                },
                style = "",
                text = "编辑",
                name = "Post"
            });
            return View(pageModel);
        }

        public ActionResult Post(int id)
        {
            var req = new PageFactory.UserInfo.OnJobPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.UserInfo.OnJobPost().Get(req);
            return View(pageModel);
        }
        #endregion
    }
}