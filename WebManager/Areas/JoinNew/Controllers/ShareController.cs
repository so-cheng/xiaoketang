using Services.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Services;
using WeiCode.Utility;

namespace WebProject.Areas.JoinNew.Controllers
{
    public class ShareController : BaseLoginController
    {
        #region 等待入库
        public ActionResult WaitPut()
        {
            var req = new PageFactory.JoinNew.WaitPut.DtoReq();
            var pageModel = new PageFactory.JoinNew.WaitPut().Get(req);
            pageModel.listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("supplement_time")
            {
                index = 1,
                text = "分配时间",
                width = "160",
            });
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.isOpenNumbers = true;
            pageModel.listDisplay.listBatchItems.Where(x => x.name == "group").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $"tg_need_id > 0 AND status = '{ModelDb.p_join_new_info.status_enum.等待入库.ToInt()}'";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 已经流失
        public ActionResult ZBQuited()
        {
            var req = new PageFactory.JoinNew.Stu_MxList.DtoReq();
            var pageModel = new PageFactory.JoinNew.Stu_MxList().Get(req);
            pageModel.listDisplay.isHideOperate = true;
            pageModel.listDisplay.listData.attachFilterSql = $"( status = '{ModelDb.p_join_new_info.status_enum.逻辑删除.ToInt()}')";
            pageModel.listDisplay.listItems.Where(x => x.field == "mx_sn").FirstOrDefault().disabled = true;
            return View(pageModel);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var req = new PageFactory.JoinNew.Stu_MxPost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.Stu_MxPost().Get(req);
            pageModel.formDisplay.formItems.Find(x => x.name == "term").displayStatus = WeiCode.Models.EmtModelBase.DisplayStatus.编辑;
            return View(pageModel);
        }

        [HttpPost]
        public JsonResult Edit(ModelDb.user_info_zb p_join_new_info)
        {
            var info = new JsonResultAction();
            List<string> lSql = new List<string>();

            try
            {
                if (!DoMySql.FindEntity<ModelDb.user_info_zb>($"id='{p_join_new_info.id}'", false).tg_user_sn.IsNullOrEmpty())
                {
                    throw new Exception("已分配直播厅,禁止修改");
                }
                if (p_join_new_info.dou_username.IsNullOrEmpty()) throw new WeicodeException("请填写抖音账号");
                if (p_join_new_info.dou_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写抖音昵称");
                if (p_join_new_info.wechat_username.IsNullOrEmpty()) throw new WeicodeException("请填写微信账号");
                if (p_join_new_info.wechat_nickname.IsNullOrEmpty()) throw new WeicodeException("请填写微信昵称");
                if (!DoMySql.FindEntity<ModelDb.user_info_zb>($"wechat_username='{p_join_new_info.wechat_username}' and tui_status!=2 and id!='{p_join_new_info.id}'", false).IsNullOrEmpty())
                {
                    throw new WeicodeException("微信账号已存在");
                }
                if (p_join_new_info.zb_sex.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写性别");
                }
                if (p_join_new_info.age <= 0 || p_join_new_info.age.ToString().IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写年龄");
                }
                if (p_join_new_info.job.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写现实工作");
                }
                if (p_join_new_info.province.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写所在省份");
                }
                if (p_join_new_info.city.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写所在城市");
                }
                if (p_join_new_info.sessions.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写接档时间");
                }
                if (p_join_new_info.full_or_part.IsNullOrEmpty())
                {
                    throw new WeicodeException("请填写兼职/全职");
                }
                if (p_join_new_info.full_or_part == "兼职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 1)
                    {
                        throw new Exception("兼职主播请选择1个接档时间");
                    }
                }
                if (p_join_new_info.full_or_part == "全职")
                {
                    if (p_join_new_info.sessions.Split(',').Length != 2)
                    {
                        throw new Exception("全职主播请选择2个接档时间");
                    }
                }

                p_join_new_info.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                if (p_join_new_info.term.IsNullOrEmpty())
                {
                    var p_mengxin = DoMySql.FindEntity<ModelDb.p_mengxin>($"user_sn='{new UserIdentityBag().user_sn}' order by date desc", false);
                    p_join_new_info.term = p_mengxin.term.IsNullOrEmpty() ? "-" : p_mengxin.term;
                }
                if (p_join_new_info.zb_level.IsNullOrEmpty())
                {
                    p_join_new_info.zb_level = "-";
                }
                lSql.Add(p_join_new_info.UpdateTran());
                DoMySql.ExecuteSqlTran(lSql);
            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }
        #endregion

        #region 标记错误
        public ActionResult Mark(string ids)
        {
            var req = new PageFactory.JoinNew.MarkPost.DtoReq();
            req.ids = ids;
            var pageModel = new PageFactory.JoinNew.MarkPost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 入库失败
        /// <summary>
        /// 账号管理端入库失败主播展示
        /// </summary>
        /// <returns></returns>
        public ActionResult StorageFailed()
        {
            var req = new PageFactory.JoinNew.StorageFailed.DtoReq();
            var pageModel = new PageFactory.JoinNew.StorageFailed().Get(req);
            pageModel.listDisplay.isOpenCheckBox = true;
            pageModel.listDisplay.listData.attachFilterSql = $" status = '{ModelDb.p_join_new_info.status_enum.入库失败.ToInt()}'";
            return View(pageModel);
        }

        /// <summary>
        /// 查看萌新上传记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UploadRecords(int id)
        {
            var req = new PageFactory.JoinNew.UploadRecords.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.UploadRecords().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 入库失败退回操作页面
        /// <summary>
        /// 入库失败退回操作页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StorageFailedBack(string id)
        {
            var req = new PageFactory.JoinNew.StorageFailedBack.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.StorageFailedBack().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 流失操作页面
        /// <summary>
        /// 流失
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CausePost(string id)
        {
            var req = new PageFactory.JoinNew.Stu_CausePost.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.Stu_CausePost().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 培训名单
        public ActionResult Search(PageFactory.JoinNew.Search.DtoReq req)
        {
            var pageModel = new PageFactory.JoinNew.Search().Get(req);
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Edit").disabled = true;
            pageModel.listDisplay.listOperateItems.Find(x => x.name == "Transform").disabled = false;
            return View(pageModel);
        }
        #endregion

        #region 转移
        /// <summary>
        /// 转移
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Transforms(int id)
        {
            var req = new PageFactory.JoinNew.Transforms.DtoReq();
            req.id = id;
            var pageModel = new PageFactory.JoinNew.Transforms().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 退回主播
        /// <summary>
        /// 退回主播
        /// </summary>
        /// <returns></returns>
        public ActionResult Back()
        {
            var req = new PageFactory.JoinNew.Back.DtoReq();
            var pageModel = new PageFactory.JoinNew.Back().Get(req);
            return View(pageModel);
        }
        #endregion

        #region 分配日志
        /// <summary>
        /// 分配日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Log(int id = 0)
        {
            var req = new PageFactory.JoinNew.ShareLog.DtoReq();
            var pageModel = new PageFactory.JoinNew.ShareLog().Get(req);
            if (id > 0)
            {
                pageModel.listDisplay.listData.attachFilterSql = $"user_info_zb_id = {id}";
            }
            return View(pageModel);
        }
        #endregion
    }
}