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
using static Services.Project.PageFactory.DangBiao;
using System.Reflection;
using Newtonsoft.Json;

namespace WebProject.Areas.DangBiao.Controllers
{
    public class DangBiaoController : BaseLoginController
    {
        #region 新增档表
        /// <summary>
        /// 新增空白档表
        /// </summary>
        /// <returns></returns>
        public ActionResult Table(string ting_sn)
        {
            if (ting_sn.IsNullOrEmpty())
            {
                ting_sn = new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter() 
                {
                    attachUserType=new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                        UserSn = new UserIdentityBag().user_sn
                    },
                    status = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.Status.正常,
                })[0].ting_sn;
            }
            
            ViewBag.ting_sn = ting_sn;
            return View();
        }

        /// <summary>
        /// 保存档表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Table(TableReqDto req)
        {
            var info = new JsonResultAction();
            try
            {
                
                //1.数据校验处理
                //1.1 时间不能为空
                if (req.c_date.IsNullOrEmpty()) throw new WeicodeException("时间范围不能为空!");
                if (req.ting_sn.IsNullOrEmpty()) throw new Exception("请选择所属厅"); 
                //2.保存数据到数据库
                var lSql = new List<string>();
                var tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                var db_sn = UtilityStatic.CommonHelper.CreateUniqueSn();
                var tg_user_sn = new UserIdentityBag().user_sn;
                var s_date = UtilityStatic.CommonHelper.DateRangeFormat(req.c_date).date_range_s.ToDateTime();
                var e_date = UtilityStatic.CommonHelper.DateRangeFormat(req.c_date).date_range_e.ToDateTime();
                //1.3处理备注信息
                var memo = "";
                //foreach (var item in req.memo_item)
                //{
                //    memo += item + "@";
                //}

                //memo = memo.Substring(0, memo.Length - 1);
                memo += req.memo_1 + "@";
                memo += req.memo_2 + "@";
                memo += req.memo_3 + "@";
                memo += req.memo_4 + "@";
                memo += req.memo_5 + "@";
                memo += req.memo_6 + "@";
                memo += req.memo_7 + "@";
                memo += req.memo_8;

                if (!req.table_id.IsNullOrEmpty() && req.table_id.ToInt() != 0)
                {
                    //2.1修改档表
                    var table = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"id = {req.table_id.ToInt()}");
                    lSql.Add(new ModelDb.p_jixiao_dangbiao
                    {
                        s_date = s_date,
                        e_date = e_date,
                        dang_wei_role = req.dang_wei_role,
                        note = req.note,
                    }.UpdateTran($"id = {req.table_id.ToInt()}"));
                    lSql.Add(new ModelDb.p_jixiao_dangbiao_item
                    {
                        zb_0 = String.Join(",", req.zb_0),
                        zb_1 = String.Join(",", req.zb_1),
                        zb_2 = String.Join(",", req.zb_2),
                        zb_3 = String.Join(",", req.zb_3),
                        zb_4 = String.Join(",", req.zb_4),
                        zb_5 = String.Join(",", req.zb_5),
                        zb_6 = String.Join(",", req.zb_6),
                        zb_7 = String.Join(",", req.zb_7),
                        zb_8 = String.Join(",", req.zb_8),
                        zb_9 = String.Join(",", req.zb_9),
                        zb_10 = String.Join(",", req.zb_10),
                        zb_11 = String.Join(",", req.zb_11),
                        zb_12 = String.Join(",", req.zb_12),
                        zb_13 = String.Join(",", req.zb_13),
                        zb_14 = String.Join(",", req.zb_14),
                        zb_15 = String.Join(",", req.zb_15),
                        zb_16 = String.Join(",", req.zb_16),
                        zb_17 = String.Join(",", req.zb_17),
                        zb_18 = String.Join(",", req.zb_18),
                        zb_19 = String.Join(",", req.zb_19),
                        zb_20 = String.Join(",", req.zb_20),
                        zb_21 = String.Join(",", req.zb_21),
                        zb_22 = String.Join(",", req.zb_22),
                        zb_23 = String.Join(",", req.zb_23),
                        memo = memo,
                        zbsr_color_db=req.zbys
                    }.UpdateTran($"db_sn = '{table.db_sn}'"));
                }
                else
                {
                    //校验是否已经有档表
                    if (!DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"tg_user_sn = '{tg_user_sn}' and s_date <= '{e_date}' and e_date >= '{s_date}'", false).IsNullOrEmpty()) throw new WeicodeException($"您在 {s_date} - {e_date} 内已有档表,无法创建新的档表!");

                    //2.2新增档表
                    lSql.Add(new ModelDb.p_jixiao_dangbiao
                    {
                        tenant_id = tenant_id,
                        db_sn = db_sn,
                        tg_user_sn = tg_user_sn,
                        ting_sn = req.ting_sn,
                        note = req.note,
                        s_date = s_date,
                        e_date = e_date,
                        dang_wei_role = req.dang_wei_role,
                    }.InsertTran());
                    lSql.Add(new ModelDb.p_jixiao_dangbiao_item
                    {
                        tenant_id = tenant_id,
                        db_sn = db_sn,
                        tg_user_sn = tg_user_sn,
                        zb_0 = String.Join(",", req.zb_0),
                        zb_1 = String.Join(",", req.zb_1),
                        zb_2 = String.Join(",", req.zb_2),
                        zb_3 = String.Join(",", req.zb_3),
                        zb_4 = String.Join(",", req.zb_4),
                        zb_5 = String.Join(",", req.zb_5),
                        zb_6 = String.Join(",", req.zb_6),
                        zb_7 = String.Join(",", req.zb_7),
                        zb_8 = String.Join(",", req.zb_8),
                        zb_9 = String.Join(",", req.zb_9),
                        zb_10 = String.Join(",", req.zb_10),
                        zb_11 = String.Join(",", req.zb_11),
                        zb_12 = String.Join(",", req.zb_12),
                        zb_13 = String.Join(",", req.zb_13),
                        zb_14 = String.Join(",", req.zb_14),
                        zb_15 = String.Join(",", req.zb_15),
                        zb_16 = String.Join(",", req.zb_16),
                        zb_17 = String.Join(",", req.zb_17),
                        zb_18 = String.Join(",", req.zb_18),
                        zb_19 = String.Join(",", req.zb_19),
                        zb_20 = String.Join(",", req.zb_20),
                        zb_21 = String.Join(",", req.zb_21),
                        zb_22 = String.Join(",", req.zb_22),
                        zb_23 = String.Join(",", req.zb_23),
                        memo = memo,
                        zbsr_color_db = string.Join(",", req.zbys)
                    }.InsertTran());
                }
                DoMySql.ExecuteSqlTran(lSql);

            }
            catch (Exception ex)
            {
                info.code = 1;
                info.msg = ex.Message;
            }
            return Json(info);
        }

        /// <summary>
        /// 新增模板档表
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTableByTemplate(string ting_sn)
        {
            if (ting_sn.IsNullOrEmpty())
            {
                ting_sn = new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                        UserSn = new UserIdentityBag().user_sn
                    },
                    status = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.Status.正常,
                })[0].ting_sn;
            }
            ViewBag.ting_sn = ting_sn;
            var p_jixiao_dangbiao = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"tg_user_sn = '{new UserIdentityBag().user_sn}' order by create_time desc", false);
            var tableItem = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"db_sn = '{p_jixiao_dangbiao.db_sn}'", false);
            if (p_jixiao_dangbiao.IsNullOrEmpty()) throw new WeicodeException("不存在档表!");
            var vo = new TableVO
            {
                op_type = 0,
                table_id = 0,
                note = p_jixiao_dangbiao.note,
                dang_wei_role = p_jixiao_dangbiao.dang_wei_role,
                tg_user_sn=p_jixiao_dangbiao.tg_user_sn,
                zb_0 = tableItem.zb_0.Split(','),
                zb_1 = tableItem.zb_1.Split(','),
                zb_2 = tableItem.zb_2.Split(','),
                zb_3 = tableItem.zb_3.Split(','),
                zb_4 = tableItem.zb_4.Split(','),
                zb_5 = tableItem.zb_5.Split(','),
                zb_6 = tableItem.zb_6.Split(','),
                zb_7 = tableItem.zb_7.Split(','),
                zb_8 = tableItem.zb_8.Split(','),
                zb_9 = tableItem.zb_9.Split(','),
                zb_10 = tableItem.zb_10.Split(','),
                zb_11 = tableItem.zb_11.Split(','),
                zb_12 = tableItem.zb_12.Split(','),
                zb_13 = tableItem.zb_13.Split(','),
                zb_14 = tableItem.zb_14.Split(','),
                zb_15 = tableItem.zb_15.Split(','),
                zb_16 = tableItem.zb_16.Split(','),
                zb_17 = tableItem.zb_17.Split(','),
                zb_18 = tableItem.zb_18.Split(','),
                zb_19 = tableItem.zb_19.Split(','),
                zb_20 = tableItem.zb_20.Split(','),
                zb_21 = tableItem.zb_21.Split(','),
                zb_22 = tableItem.zb_22.Split(','),
                zb_23 = tableItem.zb_23.Split(','),
            };

            //2.获取主播时长明细数据
            var dictionary = GetZBHourDetails(vo);

            //3. 整理VO
            vo.items = new List<ZBItem>(dictionary.Values);
            ViewBag.db_id = p_jixiao_dangbiao.id;
            ViewBag.ting_name = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_dangbiao.ting_sn).ting_name;
            return View(vo);

        }

        /// <summary>
        /// 判断档表内容是否全为空
        /// </summary>
        /// <param name="req">req</param>
        private bool IsAllTableItemEmpty(TableReqDto req)
        {
            Type type = typeof(TableReqDto);
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var property in propertyInfos)
            {
                if (property.PropertyType == typeof(string[]) && property.CanRead)
                {
                    var values = property.GetValue(req) as string[];
                    foreach (var item in values)
                    {
                        if (!item.IsNullOrEmpty()) return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #region 查看档表记录
        [HttpGet]
        public ActionResult ShowTableList()
        {
            var req = new PageFactory.DangBiao.TableList.DtoReq();
            req.relation_type = ModelEnum.UserRelationTypeEnum.厅管邀厅管;
            var pageModel = new PageFactory.DangBiao.TableList().Get(req);
            //pageModel.listFilter.disabled = true;
            pageModel.listDisplay.listItems.Where(x => x.field == "tg_username").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "ting_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listData.attachFilterSql = $" (tg_user_sn='{new UserIdentityBag().user_sn}' or tg_user_sn in {new DomainUserBasic.UserRelationApp().GetNextAllUsersForSql(ModelEnum.UserRelationTypeEnum.厅管邀厅管, new UserIdentityBag().user_sn)})";
            return View(pageModel);
        }
        #endregion

        #region 根据db_sn查看档表
        [HttpGet]
        public ActionResult ShowTableById(int id = 0, int op_type = 0)//查看op=0;修改op=1
        {
            //1.获取档表的数据
            var p_jixiao_dangbiao = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"id = {id}", false);
            var tableItem = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"db_sn = '{p_jixiao_dangbiao.db_sn}'", false);
            if (p_jixiao_dangbiao.IsNullOrEmpty() || op_type != 0 && op_type != 1) throw new WeicodeException("档表不存在!");
            var vo = new TableVO
            {
                tg_user_sn = p_jixiao_dangbiao.tg_user_sn,
                op_type = op_type,
                table_id = id,
                c_date = p_jixiao_dangbiao.s_date.ToDateString(ConvertExt.DateFormate.yyyy_MM_dd) + " ~ " + p_jixiao_dangbiao.e_date.ToDateString(ConvertExt.DateFormate.yyyy_MM_dd),
                note = p_jixiao_dangbiao.note,
                dang_wei_role = p_jixiao_dangbiao.dang_wei_role,
                zb_0 = tableItem.zb_0.Split(','),
                zb_1 = tableItem.zb_1.Split(','),
                zb_2 = tableItem.zb_2.Split(','),
                zb_3 = tableItem.zb_3.Split(','),
                zb_4 = tableItem.zb_4.Split(','),
                zb_5 = tableItem.zb_5.Split(','),
                zb_6 = tableItem.zb_6.Split(','),
                zb_7 = tableItem.zb_7.Split(','),
                zb_8 = tableItem.zb_8.Split(','),
                zb_9 = tableItem.zb_9.Split(','),
                zb_10 = tableItem.zb_10.Split(','),
                zb_11 = tableItem.zb_11.Split(','),
                zb_12 = tableItem.zb_12.Split(','),
                zb_13 = tableItem.zb_13.Split(','),
                zb_14 = tableItem.zb_14.Split(','),
                zb_15 = tableItem.zb_15.Split(','),
                zb_16 = tableItem.zb_16.Split(','),
                zb_17 = tableItem.zb_17.Split(','),
                zb_18 = tableItem.zb_18.Split(','),
                zb_19 = tableItem.zb_19.Split(','),
                zb_20 = tableItem.zb_20.Split(','),
                zb_21 = tableItem.zb_21.Split(','),
                zb_22 = tableItem.zb_22.Split(','),
                zb_23 = tableItem.zb_23.Split(','),
            };
            //2.获取主播时长明细数据
            var dictionary = GetZBHourDetails(vo);

            //3. 整理VO
            vo.items = new List<ZBItem>(dictionary.Values);
            ViewBag.db_sn = p_jixiao_dangbiao.db_sn;
            ViewBag.db_id = id;
            ViewBag.ting_name = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_dangbiao.ting_sn).ting_name;
            return View(vo);
        }

        /// <summary>
        /// 获取主播时长明细数据
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        private Dictionary<string, ZBItem> GetZBHourDetails(TableVO vo)
        {
            //1 获取当前厅管下的主播
            var zb_users = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, vo.tg_user_sn);
            var dictionary = new Dictionary<string, ZBItem>();
            foreach (var zb in zb_users)
            {
                if (!dictionary.ContainsKey(zb.user_sn))
                {
                    var item = new ZBItem()
                    {
                        user_sn = zb.user_sn,
                        username = zb.username,
                        name = zb.name,
                        attach2 = zb.attach2
                    };
                    dictionary[zb.user_sn] = item;
                }
            }

            //2 获取每个主播的时长
            Type type = typeof(TableReqDto);
            PropertyInfo[] propertyInfos = type.GetProperties();
            try
            {
                foreach (var property in propertyInfos)
                {
                    if (property.PropertyType == typeof(string[]) && property.CanRead && property.Name.Contains("zb"))
                    {
                        var row = property.GetValue(vo) as string[];
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (dictionary.ContainsKey(row[i]))
                            {
                                if (i < 3) dictionary[row[i]].zfp_hours++;
                                else dictionary[row[i]].gw_hours++;
                            }
                        }
                    }
                }
            }
            catch { }
            return dictionary;
        }
        #endregion

        #region 修改档表
        [HttpGet]
        public ActionResult UpdateTableById(string ting_sn,int id = 0, int op_type = 1)//查看op=0;修改op=1
        {
            if (ting_sn.IsNullOrEmpty())
            {
                ting_sn = new ServiceFactory.UserInfo.Ting().GetBaseInfos(new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter()
                {
                    attachUserType = new ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType()
                    {
                        userType = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.AttachUserType.UserType.厅管,
                        UserSn = new UserIdentityBag().user_sn
                    },
                    status = ServiceFactory.UserInfo.Ting.TgBaseInfoFilter.Status.正常,
                })[0].ting_sn;
            }

            ViewBag.ting_sn = ting_sn;
            //1.获取档表的数据
            var p_jixiao_dangbiao = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"id = {id}", false);
            var tableItem = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"db_sn = '{p_jixiao_dangbiao.db_sn}'", false);
            if (p_jixiao_dangbiao.IsNullOrEmpty() || op_type != 0 && op_type != 1) throw new WeicodeException("档表不存在!");
            var vo = new TableVO
            {
                op_type = op_type,
                tg_user_sn = p_jixiao_dangbiao.tg_user_sn,
                table_id = p_jixiao_dangbiao.id,
                c_date = p_jixiao_dangbiao.s_date.ToDateString(ConvertExt.DateFormate.yyyy_MM_dd) + " ~ " + p_jixiao_dangbiao.e_date.ToDateString(ConvertExt.DateFormate.yyyy_MM_dd),
                note = p_jixiao_dangbiao.note,
                dang_wei_role = p_jixiao_dangbiao.dang_wei_role,
                zb_0 = tableItem.zb_0.Split(','),
                zb_1 = tableItem.zb_1.Split(','),
                zb_2 = tableItem.zb_2.Split(','),
                zb_3 = tableItem.zb_3.Split(','),
                zb_4 = tableItem.zb_4.Split(','),
                zb_5 = tableItem.zb_5.Split(','),
                zb_6 = tableItem.zb_6.Split(','),
                zb_7 = tableItem.zb_7.Split(','),
                zb_8 = tableItem.zb_8.Split(','),
                zb_9 = tableItem.zb_9.Split(','),
                zb_10 = tableItem.zb_10.Split(','),
                zb_11 = tableItem.zb_11.Split(','),
                zb_12 = tableItem.zb_12.Split(','),
                zb_13 = tableItem.zb_13.Split(','),
                zb_14 = tableItem.zb_14.Split(','),
                zb_15 = tableItem.zb_15.Split(','),
                zb_16 = tableItem.zb_16.Split(','),
                zb_17 = tableItem.zb_17.Split(','),
                zb_18 = tableItem.zb_18.Split(','),
                zb_19 = tableItem.zb_19.Split(','),
                zb_20 = tableItem.zb_20.Split(','),
                zb_21 = tableItem.zb_21.Split(','),
                zb_22 = tableItem.zb_22.Split(','),
                zb_23 = tableItem.zb_23.Split(','),
            };

            //2.获取主播时长明细数据
            var dictionary = GetZBHourDetails(vo);

            //3. 整理VO
            vo.items = new List<ZBItem>(dictionary.Values);
            ViewBag.db_sn = p_jixiao_dangbiao.db_sn;
            ViewBag.db_id = id;
            ViewBag.ting_name = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_dangbiao.ting_sn).ting_name;
            return View(vo);
        }
        #endregion
        #region 当前档表
        public ActionResult ShowCurrentTable(string tg_user_sn)
        {
            if (tg_user_sn.IsNullOrEmpty())
            {
                tg_user_sn = new UserIdentityBag().user_sn;
            }
            string yy_user_sn = new DomainUserBasic.UserRelationApp().GetParentUserSn(ModelEnum.UserRelationTypeEnum.运营邀厅管, tg_user_sn);
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new ServiceFactory.UserInfo.Yy().YyGetNextTg(yy_user_sn);
                //var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
                if (list.Count > 0)
                {
                    tg_user_sn = list[0].user_sn;
                }
            }
            ViewBag.yy_user_sn = yy_user_sn;
            ViewBag.tg_user_sn = tg_user_sn;

            //1.根据当前的时间查找档表
            var p_jixiao_dangbiao = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"s_date <= '{DateTime.Today.Date}' and e_date >= '{DateTime.Today.Date}' and tg_user_sn = '{tg_user_sn}'", false);
            //if (p_jixiao_dangbiao.IsNullOrEmpty())
            //{
            //    return Content("当前时间不存在档表!");
            //}
            var tableItem = DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao_item>($"db_sn = '{p_jixiao_dangbiao.db_sn}'", false);
            if (tableItem.IsNullOrEmpty())
            {
                tableItem = new ModelDb.p_jixiao_dangbiao_item();
            }
            var vo = new TableVO();

            if (!p_jixiao_dangbiao.IsNullOrEmpty())
            {
                vo = new TableVO
                {
                    op_type = 0,
                    tg_user_sn = tg_user_sn,
                    table_id = p_jixiao_dangbiao.id,
                    c_date = p_jixiao_dangbiao.s_date.ToDateString() + " ~ " + p_jixiao_dangbiao.e_date.ToDateString(),
                    note = p_jixiao_dangbiao.note,
                    dang_wei_role = p_jixiao_dangbiao.dang_wei_role,
                    zb_0 = tableItem.zb_0.Split(','),
                    zb_1 = tableItem.zb_1.Split(','),
                    zb_2 = tableItem.zb_2.Split(','),
                    zb_3 = tableItem.zb_3.Split(','),
                    zb_4 = tableItem.zb_4.Split(','),
                    zb_5 = tableItem.zb_5.Split(','),
                    zb_6 = tableItem.zb_6.Split(','),
                    zb_7 = tableItem.zb_7.Split(','),
                    zb_8 = tableItem.zb_8.Split(','),
                    zb_9 = tableItem.zb_9.Split(','),
                    zb_10 = tableItem.zb_10.Split(','),
                    zb_11 = tableItem.zb_11.Split(','),
                    zb_12 = tableItem.zb_12.Split(','),
                    zb_13 = tableItem.zb_13.Split(','),
                    zb_14 = tableItem.zb_14.Split(','),
                    zb_15 = tableItem.zb_15.Split(','),
                    zb_16 = tableItem.zb_16.Split(','),
                    zb_17 = tableItem.zb_17.Split(','),
                    zb_18 = tableItem.zb_18.Split(','),
                    zb_19 = tableItem.zb_19.Split(','),
                    zb_20 = tableItem.zb_20.Split(','),
                    zb_21 = tableItem.zb_21.Split(','),
                    zb_22 = tableItem.zb_22.Split(','),
                    zb_23 = tableItem.zb_23.Split(','),
                };

                //2.获取主播时长明细数据
                var dictionary = GetZBHourDetails(vo);

                //3. 整理VO
                vo.items = new List<ZBItem>(dictionary.Values);
                ViewBag.db_sn = p_jixiao_dangbiao.db_sn;
                ViewBag.db_id = p_jixiao_dangbiao.id;
                ViewBag.ting_name = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_dangbiao.ting_sn).ting_name;
            }
            return View(vo);
        }

        
        #endregion
    }
}