using Services.Project;
using System.Linq;
using System.Web.Mvc;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.PCrm.Controllers
{
    public class PCrmController : BaseLoginController
    {
        #region 用户列表
        public ActionResult CrmList()
        {
            var req = new PageFactory.CrmList.Req();
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn = '{new UserIdentityBag().user_sn}' and status = {ModelDb.p_crm_customer.status_enum.正常.ToSByte()}";
            return View(pageModel);
        }
        #endregion

        #region 流失回收列表
        /// <summary>
        /// 流失回收列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Recover()
        {
            var req = new PageFactory.CrmList.Req();
            var pageModel = new PageFactory.CrmList().Get(req);
            pageModel.listFilter.disabled = true;
            pageModel.listDisplay.isOpenCheckBox = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "recover").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "edit").FirstOrDefault().disabled = true;
            pageModel.listDisplay.listOperateItems.Where(x => x.name == "del").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
            pageModel.listDisplay.listData.attachFilterSql = $"zb_user_sn = '{new UserIdentityBag().user_sn}' and status = {ModelDb.p_crm_customer.status_enum.逻辑删除.ToSByte()}";
            return View(pageModel);
        }
        #endregion

        #region 新建/编辑用户信息
        /// <summary>
        /// 登记/编辑用户页面
        /// </summary>
        [HttpGet]
        public ActionResult CrmPost(int id = 0)
        {
            var req = new PageFactory.CrmPost.Req();
            req.p_Crm_Customer = new ModelDb.p_crm_customer();
            req.p_Crm_Customer.id = id;
            var pageModel = new PageFactory.CrmPost().Get(req);

            var returnUrl = "/PCrm/Info/List";
            if (id > 0) returnUrl = $"/PCrm/Info/info?id={id}&";
            pageModel.postedReturn.returnType = ModelBasic.PagePost.PostedReturn.ReturnType.当前窗口跳转URL;
            pageModel.postedReturn.returnUrl = returnUrl;

            pageModel.buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("b1")
            {
                text = "提交记录",
                mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                {
                    url = "/PCrm/PCrm/CrmList",
                }
            });
            return View(pageModel);
        }
        #endregion
    }
}