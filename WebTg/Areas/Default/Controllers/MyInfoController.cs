using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Domain;
using WeiCode.Models;
using Services.Project;
using WeiCode.ModelDbs;
using static WeiCode.Utility.UtilityStatic;

namespace WebProject.Areas.Default.Controllers
{
    public class MyInfoController : BaseLoginController
    {
        [HttpGet]
        public ActionResult Post()
        {
            var req = new PageFactory.UserCreate.DtoReq();
            req.user_type = "tger";
            req.id = new UserIdentityBag().id;
            var pageModel = new PageFactory.ManagerCreateTg().Get(req);
            pageModel.formDisplay.formItems.Find(item => item.name == "yy_user_sn").isDisplay = false;
            pageModel.formDisplay.formItems.Find(item => item.name == "f_user_sn").isDisplay = false;

            //对指定元素进行排序，index默认值为100
            //pageModel.formDisplay.formItems.Find(item => item.name == "f_user_sn").index = 101;

            /*
             var index = pageModel.formDisplay.formItems.FindIndex(item => item.name == "f_user_sn");

            //将指定元素替换为一个新元素
            pageModel.formDisplay.formItems[index] = new ModelBasic.EmtInput("emt1")
            {
                title = "新元素title"
            };

            //在指定元素后插入一个元素
            pageModel.formDisplay.formItems.Insert(index, new ModelBasic.EmtInput("new_emt2")
                                                            {
                                                                title = "新元素title2"
                                                            });
            */

            return View(pageModel);
        }
    }
}