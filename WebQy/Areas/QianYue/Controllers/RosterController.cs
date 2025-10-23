using System.Web.Mvc;
using WeiCode.Services;
using WeiCode.Models;
using Services.Project;

namespace WebProject.Areas.QianYue.Controllers
{
    /// <summary>
    /// 签约名单
    /// </summary>
    public class RosterController : BaseLoginController
    {
        /// <summary>
        /// 签约名单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(PageFactory.List.DtoReq req)
        {
            var pageModel = new PageFactory.List().Get(req);
            pageModel.listDisplay.listOperateItems.Clear();
            pageModel.listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
            {
                actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                {
                    field_paras = "id",
                    url = "Edit"
                },
                text = "编辑",
            });
            return View(pageModel);
        }
        public ActionResult Edit(PageFactory.Post.DtoReq req)
        {
            var pageModel = new PageFactory.Post().Get(req);
            return View(pageModel);
        }

    }
}