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

namespace WebProject.Areas.DangBiao.Controllers
{
    public class DangBiaoController : BaseLoginController
    {
        #region 查看档表记录
        [HttpGet]
        public ActionResult ShowTableList()
        {
            var req = new PageFactory.DangBiao.TableList.DtoReq();
            var pageModel = new PageFactory.DangBiao.TableList().Get(req);
            pageModel.listFilter.formItems.Where(x => x.name == "yy_user_sn").FirstOrDefault().disabled = false;
            pageModel.listFilter.formItems.Where(x => x.name == "ting_sn").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listItems.Where(x => x.field == "tg_username").FirstOrDefault().disabled = false;
            pageModel.listDisplay.listOperateItems.Where(x => x.text == "编辑").FirstOrDefault().disabled = true;
            pageModel.buttonGroup.disabled = true;
            pageModel.buttonGroup.buttonItems.Clear();
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
                op_type = op_type,
                table_id = id,
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
            ViewBag.db_id = id;
            ViewBag.tg_username = new DomainBasic.UserApp().GetInfoByUserSn(p_jixiao_dangbiao.tg_user_sn).username;
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
            var zb_users = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.厅管邀主播, DomainBasicStatic.DoMySql.FindEntity<ModelDb.p_jixiao_dangbiao>($"id = {vo.table_id}", false).tg_user_sn);
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
            return dictionary;
        }
        #endregion

        #region 当前档表
        public ActionResult ShowCurrentTable(string yy_user_sn, string tg_user_sn)
        {
            if (yy_user_sn.IsNullOrEmpty())
            {
                yy_user_sn = DoMySql.FindEntity<ModelDb.user_base>($"user_type_id='{new DomainBasic.UserTypeApp().GetInfoByCode("yyer").id}' and tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'").user_sn;
            }
            if (tg_user_sn.IsNullOrEmpty())
            {
                var list = new DomainUserBasic.UserRelationApp().GetNextUsers(ModelEnum.UserRelationTypeEnum.运营邀厅管, yy_user_sn);
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
                ViewBag.db_sn = p_jixiao_dangbiao.db_sn;
                ViewBag.db_id = p_jixiao_dangbiao.id;
                ViewBag.ting_name = new ServiceFactory.UserInfo.Ting().GetTingBySn(p_jixiao_dangbiao.ting_sn).ting_name;
                //3. 整理VO
                vo.items = new List<ZBItem>(dictionary.Values);
            }
            return View(vo);
        }
        #endregion
    }
}