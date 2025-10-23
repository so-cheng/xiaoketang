using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.Models;
using WeiCode.Domain;
using WeiCode.ModelDbs;

namespace Services.Project
{
    /// <summary>
    /// 绩效-跨房
    /// </summary>
    public partial class PageFactory
    {
        /// <summary>
        /// 跨房数据列表页
        /// </summary>
        public class KuafangList
        {
            #region DefaultView
            /// <summary>
            /// 获取页面数据模型
            /// </summary>
            /// <returns></returns>
            public ModelBasic.PageList Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PageList("PageList");

                pageModel.listFilter = GetListFilter(req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.listDisplay = GetListDisplay(req);
                return pageModel;
            }

            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new ModelBasic.CtlListFilter();
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "选择绩效发生日期",
                });
                return listFilter;
            }
            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "上报音浪",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = $"KuafangPost",
                    }
                });
                return buttonGroup;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "160";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "绩效发生日期",
                    width = "160",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_num")
                {
                    text = "参加跨房厅数",
                    width = "160",
                    minWidth = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_amount")
                {
                    text = "跨房前流水",
                    width = "160",
                    minWidth  = "160"
                });
                
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_text")
                {
                    text = "跨房总流水",
                    width = "160",
                    minWidth = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_day_text")
                {
                    text = "跨房当天总流水",
                    width = "160",
                    minWidth = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2040_num")
                {
                    text = "20:40",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2050_num")
                {
                    text = "20:50",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2100_num")
                {
                    text = "21:00",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2110_num")
                {
                    text = "21:10",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2120_num")
                {
                    text = "21:20",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2130_num")
                {
                    text = "21:35",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2140_num")
                {
                    text = "21:40",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2150_num")
                {
                    text = "21:50",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2200_num")
                {
                    text = "22:00",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2210_num")
                {
                    text = "22:10",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2220_num")
                {
                    text = "22:20",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2230_num")
                {
                    text = "22:35",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2240_num")
                {
                    text = "22:40",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2250_num")
                {
                    text = "22:50",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2300_num")
                {
                    text = "23:00",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2310_num")
                {
                    text = "23:10",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2320_num")
                {
                    text = "23:20",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2330_num")
                {
                    text = "23:35",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2340_num")
                {
                    text = "23:40",
                    width = "140",
                    minWidth  = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2350_num")
                {
                    text = "23:50",
                    width = "140",
                    minWidth  = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2400_num")
                {
                    text = "24:00",
                    width = "140",
                    minWidth  = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("total")
                {
                    text = "当前实际总流水",
                    width = "140",
                    minWidth  = "160"
                });
                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name="edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "Edit",
                        field_paras = "id"
                    },
                    text = "编辑"
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    style = "",
                    text = "删除"

                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
            }
            #endregion
            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();

                //var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{new UserIdentityBag().user_sn}'");

                string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}' and user_sn='{new UserIdentityBag().user_sn}'";
                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                }
                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_kuafang, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                /// <summary>
                /// 
                /// </summary>
                public string keyword { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_kuafang
            {

                

                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }

                public decimal? real_amount_2040
                {
                    get
                    {
                        if (this.amount_2040 == 0)
                        {
                            return this.old_amount;
                        }
                        else
                        {
                            return this.amount_2040;
                        }
                    }
                }
                public string amount_2040_num
                {
                    get
                    {
                        return this.amount_2040==0?"":this.amount_2040.ToString()+"w" + "(" + (this.real_amount_2040 - this.old_amount).ToInt().ToString() + "w)";
                    }
                }

                public decimal? real_amount_2050
                {
                    get
                    {
                        if (this.amount_2050 == 0)
                        {
                            return this.real_amount_2040;
                        }
                        else
                        {
                            return this.amount_2050;
                        }
                    }
                }
                public string amount_2050_num
                {
                    get
                    {
                        return this.amount_2050 == 0 ? "" : this.amount_2050.ToString() + "w" + "(" + (this.real_amount_2050 - this.real_amount_2040).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2100
                {
                    get
                    {
                        if (this.amount_2100 == 0)
                        {
                            return this.real_amount_2050;
                        }
                        else
                        {
                            return this.amount_2100;
                        }
                    }
                }
                public string amount_2100_num
                {
                    get
                    {
                        return this.amount_2100 == 0 ? "" : this.amount_2100.ToString() + "w" + "(" + (this.real_amount_2100 - this.real_amount_2050).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2110
                {
                    get
                    {
                        if (this.amount_2110 == 0)
                        {
                            return this.real_amount_2100;
                        }
                        else
                        {
                            return this.amount_2110;
                        }
                    }
                }
                public string amount_2110_num
                {
                    get
                    {
                        return this.amount_2110 == 0 ? "" : this.amount_2110.ToString() + "w" + "(" + (this.real_amount_2110 - this.real_amount_2100).ToInt().ToString() + "w" + ")";
                    }
                }


                public decimal? real_amount_2120
                {
                    get
                    {
                        if (this.amount_2120 == 0)
                        {
                            return this.real_amount_2110;
                        }
                        else
                        {
                            return this.amount_2120;
                        }
                    }
                }
                public string amount_2120_num
                {
                    get
                    {
                        return this.amount_2120 == 0 ? "" : this.amount_2120.ToString() + "w" + "(" + (this.real_amount_2120 - this.real_amount_2110).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2130
                {
                    get
                    {
                        if (this.amount_2130 == 0)
                        {
                            return this.real_amount_2120;
                        }
                        else
                        {
                            return this.amount_2130;
                        }
                    }
                }
                public string amount_2130_num
                {
                    get
                    {
                        return this.amount_2130 == 0 ? "" : this.amount_2130.ToString() + "w" + "(" + (this.real_amount_2130 - this.real_amount_2120).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2140
                {
                    get
                    {
                        if (this.amount_2140 == 0)
                        {
                            return this.real_amount_2130;
                        }
                        else
                        {
                            return this.amount_2140;
                        }
                    }
                }
                public string amount_2140_num
                {
                    get
                    {
                        return this.amount_2140 == 0 ? "" : this.amount_2140.ToString() + "w" + "(" + (this.real_amount_2140 - this.real_amount_2130).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2150
                {
                    get
                    {
                        if (this.amount_2150 == 0)
                        {
                            return this.real_amount_2140;
                        }
                        else
                        {
                            return this.amount_2150;
                        }
                    }
                }
                public string amount_2150_num
                {
                    get
                    {
                        return this.amount_2150 == 0 ? "" : this.amount_2150.ToString() + "w" + "(" + (this.real_amount_2150 - this.real_amount_2140).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2200
                {
                    get
                    {
                        if (this.amount_2200 == 0)
                        {
                            return this.real_amount_2150;
                        }
                        else
                        {
                            return this.amount_2200;
                        }
                    }
                }
                public string amount_2200_num
                {
                    get
                    {
                        return this.amount_2200 == 0 ? "" : this.amount_2200.ToString() + "w" + "(" + (this.real_amount_2200 - this.real_amount_2150).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2210
                {
                    get
                    {
                        if (this.amount_2210 == 0)
                        {
                            return this.real_amount_2200;
                        }
                        else
                        {
                            return this.amount_2210;
                        }
                    }
                }
                public string amount_2210_num
                {
                    get
                    {
                        return this.amount_2210 == 0 ? "" : this.amount_2210.ToString() + "w" + "(" + (this.real_amount_2210 - this.real_amount_2200).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2220
                {
                    get
                    {
                        if (this.amount_2220 == 0)
                        {
                            return this.real_amount_2210;
                        }
                        else
                        {
                            return this.amount_2220;
                        }
                    }
                }
                public string amount_2220_num
                {
                    get
                    {
                        return this.amount_2220 == 0 ? "" : this.amount_2220.ToString() + "w" + "(" + (this.real_amount_2220 - this.real_amount_2210).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2230
                {
                    get
                    {
                        if (this.amount_2230 == 0)
                        {
                            return this.real_amount_2220;
                        }
                        else
                        {
                            return this.amount_2230;
                        }
                    }
                }
                public string amount_2230_num
                {
                    get
                    {
                        return this.amount_2230 == 0 ? "" : this.amount_2230.ToString() + "w" + "(" + (this.real_amount_2230 - this.real_amount_2220).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2240
                {
                    get
                    {
                        if (this.amount_2240 == 0)
                        {
                            return this.real_amount_2230;
                        }
                        else
                        {
                            return this.amount_2240;
                        }
                    }
                }
                public string amount_2240_num
                {
                    get
                    {
                        return this.amount_2240 == 0 ? "" : this.amount_2240.ToString() + "w" + "(" + (this.real_amount_2240 - this.real_amount_2230).ToInt().ToString() + "w" + ")";
                    }
                }
                public decimal? real_amount_2250
                {
                    get
                    {
                        if (this.amount_2250 == 0)
                        {
                            return this.real_amount_2240;
                        }
                        else
                        {
                            return this.amount_2250;
                        }
                    }
                }
                public string amount_2250_num
                {
                    get
                    {
                        return this.amount_2250 == 0 ? "" : this.amount_2250.ToString() + "w" + "(" + (this.real_amount_2250 - this.real_amount_2240).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2300
                {
                    get
                    {
                        if (this.amount_2300 == 0)
                        {
                            return this.real_amount_2250;
                        }
                        else
                        {
                            return this.amount_2300;
                        }
                    }
                }
                public string amount_2300_num
                {
                    get
                    {
                        return this.amount_2300 == 0 ? "" : this.amount_2300.ToString() + "w" + "(" + (this.real_amount_2300 - this.real_amount_2250).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2310
                {
                    get
                    {
                        if (this.amount_2310 == 0)
                        {
                            return this.real_amount_2300;
                        }
                        else
                        {
                            return this.amount_2310;
                        }
                    }
                }
                public string amount_2310_num
                {
                    get
                    {
                        return this.amount_2310 == 0 ? "" : this.amount_2310.ToString() + "w" + "(" + (this.real_amount_2310 - this.real_amount_2300).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2320
                {
                    get
                    {
                        if (this.amount_2320 == 0)
                        {
                            return this.real_amount_2310;
                        }
                        else
                        {
                            return this.amount_2320;
                        }
                    }
                }
                public string amount_2320_num
                {
                    get
                    {
                        return this.amount_2320 == 0 ? "" : this.amount_2320.ToString() + "w" + "(" + (this.real_amount_2320 - this.real_amount_2310).ToInt().ToString() + "w" + ")";
                    }
                }
                public decimal? real_amount_2330
                {
                    get
                    {
                        if (this.amount_2330 == 0)
                        {
                            return this.real_amount_2320;
                        }
                        else
                        {
                            return this.amount_2330;
                        }
                    }
                }
                public string amount_2330_num
                {
                    get
                    {
                        return this.amount_2330 == 0 ? "" : this.amount_2330.ToString() + "w" + "(" + (this.real_amount_2330 - this.real_amount_2320).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2340
                {
                    get
                    {
                        if (this.amount_2340 == 0)
                        {
                            return this.real_amount_2330;
                        }
                        else
                        {
                            return this.amount_2340;
                        }
                    }
                }
                public string amount_2340_num
                {
                    get
                    {
                        return this.amount_2340 == 0 ? "" : this.amount_2340.ToString() + "w" + "(" + (this.real_amount_2340 - this.real_amount_2330).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2350
                {
                    get
                    {
                        if (this.amount_2350 == 0)
                        {
                            return this.real_amount_2340;
                        }
                        else
                        {
                            return this.amount_2350;
                        }
                    }
                }
                public string amount_2350_num
                {
                    get
                    {
                        return this.amount_2350 == 0 ? "" : this.amount_2350.ToString() + "w" + "(" + (this.real_amount_2350 - this.real_amount_2340).ToInt().ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2400
                {
                    get
                    {
                        if (this.amount_2400 == 0)
                        {
                            return this.real_amount_2350;
                        }
                        else
                        {
                            return this.amount_2400;
                        }
                    }
                }
                public string amount_2400_num
                {
                    get
                    {
                        return this.amount_2400 == 0 ? "" : this.amount_2400.ToString() + "w" + "(" + (this.real_amount_2400 - this.real_amount_2350).ToInt().ToString() + "w" + ")";
                    }
                }
                public string total
                {
                    get
                    {
                        return (this.real_amount_2400 - this.old_amount).ToString() + "w";
                    }
                }

                public string amount_total_text
                {
                    get
                    {
                        return (real_amount_2400 - old_amount).ToString();
                    }
                }

                public string amount_total_day_text
                {
                    get
                    {
                        return real_amount_2400.ToString();
                    }
                }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 表单提交处理的回调函数
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var p_jixiao_kuafang = req.data_json.ToModel<ModelDb.p_jixiao_kuafang>();
                var info = new JsonResultAction();
                var lSql = new List<string>();
                lSql.Add(p_jixiao_kuafang.DeleteTran());
                DoMySql.ExecuteSqlTran(lSql);
                return info;
            }

            #endregion
        }
        /// <summary>
        /// 每日上报绩效数据
        /// </summary>
        public class KuafangPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");

                return buttonGroup;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var p_jixiao_kuafang = DoMySql.FindEntityById<ModelDb.p_jixiao_kuafang>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "绩效发生日期",
                    defaultValue = p_jixiao_kuafang.c_date.ToDateTime().ToString("yyyy-MM-dd"),
                    mold = ModelBasic.EmtTimeSelect.Mold.date
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("old_amount")
                {
                    title = "跨房前流水",
                    defaultValue = p_jixiao_kuafang.old_amount.ToNullableString(),
                    colLength = 5
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_num")
                {
                    title = "参加跨房厅数",
                    defaultValue = p_jixiao_kuafang.ting_num.ToNullableString(),
                    colLength = 5
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2040")
                {
                    title = "20:40",
                    defaultValue = p_jixiao_kuafang.amount_2040.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2050")
                {
                    title = "20:50",
                    defaultValue = p_jixiao_kuafang.amount_2050.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2100")
                {
                    title = "21:00",
                    defaultValue = p_jixiao_kuafang.amount_2100.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2110")
                {
                    title =  "21:10",
                    defaultValue = p_jixiao_kuafang.amount_2110.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2120")
                {
                    title = "21:20",
                    defaultValue = p_jixiao_kuafang.amount_2120.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2130")
                {
                    title = "21:35",
                    defaultValue = p_jixiao_kuafang.amount_2130.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2140")
                {
                    title ="21:40",
                    defaultValue = p_jixiao_kuafang.amount_2140.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2150")
                {
                    title ="21:50",
                    defaultValue = p_jixiao_kuafang.amount_2150.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2200")
                {
                    title = "22:00",
                    defaultValue = p_jixiao_kuafang.amount_2200.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2210")
                {
                    title = "22:10",
                    defaultValue = p_jixiao_kuafang.amount_2210.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2220")
                {
                    title = "22:20",
                    defaultValue = p_jixiao_kuafang.amount_2220.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2230")
                {
                    title = "22:35",
                    defaultValue = p_jixiao_kuafang.amount_2230.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2240")
                {
                    title ="22:40",
                    defaultValue = p_jixiao_kuafang.amount_2240.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2250")
                {
                    title = "22:50",
                    defaultValue = p_jixiao_kuafang.amount_2250.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2300")
                {
                    title ="23:00",
                    defaultValue = p_jixiao_kuafang.amount_2300.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2310")
                {
                    title = "23:10",
                    defaultValue = p_jixiao_kuafang.amount_2310.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2320")
                {
                    title = "23:20",
                    defaultValue = p_jixiao_kuafang.amount_2320.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2330")
                {
                    title = "23:35",
                    defaultValue = p_jixiao_kuafang.amount_2330.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2340")
                {
                    title = "23:40",
                    defaultValue = p_jixiao_kuafang.amount_2340.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2350")
                {
                    title = "23:50",
                    defaultValue = p_jixiao_kuafang.amount_2350.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2400")
                {
                    title = "24:00",
                    defaultValue = p_jixiao_kuafang.amount_2400.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total")
                {
                    title = "跨房总流水",
                    defaultValue = p_jixiao_kuafang.amount_total.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total_day")
                {
                    title = "当天总流水",
                    defaultValue = p_jixiao_kuafang.amount_total_day.ToNullableString(),
                    colLength = 4
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }
            }


            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增跨房数据
            /// </summary>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var p_jixiao_kuafang = req.data_json.ToModel<ModelDb.p_jixiao_kuafang>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                if (p_jixiao_kuafang.id==0)
                {
                    if (p_jixiao_kuafang.c_date.IsNullOrEmpty()) throw new Exception("请选择绩效发生日期");
                    if (!DoMySql.FindEntity<ModelDb.p_jixiao_kuafang>($"c_date='{p_jixiao_kuafang.c_date}'", false).IsNullOrEmpty()) throw new Exception("当前日期已提交");
                    p_jixiao_kuafang.user_sn = new UserIdentityBag().user_sn;
                    p_jixiao_kuafang.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                    p_jixiao_kuafang.user_type_id = new DomainBasic.UserTypeApp().GetInfo().id;

                }
                lSql.Add(p_jixiao_kuafang.InsertOrUpdateTran($"id={p_jixiao_kuafang.id}"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                public int id { get; set; }
                public string old_c_date { get; set; }
            }
            #endregion
        }



        /// <summary>
        /// 运营跨房数据列表页
        /// </summary>
        public class YYKuafangList
        {
            #region DefaultView
            /// <summary>
            /// 获取页面数据模型
            /// </summary>
            /// <returns></returns>
            public ModelBasic.PageList Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PageList("PageList");

                pageModel.listFilter = GetListFilter(req);
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.listDisplay = GetListDisplay(req);
                return pageModel;
            }

            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new ModelBasic.CtlListFilter();
                listFilter.formItems.Add(new ModelBasic.EmtTimeSelect("dateRange")
                {
                    mold = ModelBasic.EmtTimeSelect.Mold.date_range,
                    placeholder = "选择绩效发生日期",
                });
                return listFilter;
            }
            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");

                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("")
                {
                    text = "上报音浪",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = $"KuafangPost",
                    }
                });

                return buttonGroup;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                var listDisplay = new ModelBasic.CtlListDisplay();
                listDisplay.operateWidth = "160";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;
                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("c_date_text")
                {
                    text = "绩效发生日期",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_amount")
                {
                    text = "跨房前流水",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_num")
                {
                    text = "参加跨房厅数",
                    width = "160",
                    minWidth = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total")
                {
                    text = "跨房总流水",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_day")
                {
                    text = "跨房当天总流水",
                    width = "160",
                    minWidth = "160"
                });

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2040_num")
                {
                    text = "20:40",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2050_num")
                {
                    text = "20:50",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2100_num")
                {
                    text = "21:00",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2110_num")
                {
                    text = "21:10",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2120_num")
                {
                    text = "21:20",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2130_num")
                {
                    text = "21:35",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2140_num")
                {
                    text = "21:40",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2150_num")
                {
                    text = "21:50",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2200_num")
                {
                    text = "22:00",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2210_num")
                {
                    text = "22:10",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2220_num")
                {
                    text = "22:20",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2230_num")
                {
                    text = "22:35",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2240_num")
                {
                    text = "22:40",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2250_num")
                {
                    text = "22:50",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2300_num")
                {
                    text = "23:00",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2310_num")
                {
                    text = "23:10",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2320_num")
                {
                    text = "23:20",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2330_num")
                {
                    text = "23:35",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2340_num")
                {
                    text = "23:40",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2350_num")
                {
                    text = "23:50",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2400_num")
                {
                    text = "24:00",
                    width = "160",
                    minWidth = "160"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("total")
                {
                    text = "当前实际总流水",
                    width = "160",
                    minWidth = "160"
                });
                #region 操作列按钮


                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "edit",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                    eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                    {
                        url = "Edit",
                        field_paras = "id"
                    },
                    text = "编辑"
                });
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DelAction,
                        field_paras = "id"
                    },
                    style = "",
                    text = "删除"

                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
            }
            #endregion
            #region ListData
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();

                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{new UserIdentityBag().user_sn}'");

                string where = "1=1";
                if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                {
                    var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                    where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                }
                var filter = new DoMySql.Filter
                {
                    where = where + " order by c_date desc"
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_kuafang, ItemDataModel>(filter, reqJson);
            }

            /// <summary>
            /// 自定义筛选参数（自定义数据，与属性对应）
            /// </summary>
            public class DtoReqListData : ModelBasic.PageList.Req
            {
                /// <summary>
                /// 
                /// </summary>
                public string keyword { get; set; }
            }

            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.p_jixiao_kuafang
            {
                public string c_date_text
                {
                    get
                    {
                        return this.c_date.ToDate().ToString("yyyy-MM-dd");
                    }
                }

                public decimal? real_amount_2040
                {
                    get
                    {
                        if (this.amount_2040 == 0)
                        {
                            return this.old_amount;
                        }
                        else
                        {
                            return this.amount_2040;
                        }
                    }
                }
                public string amount_2040_num
                {
                    get
                    {
                        return this.amount_2040 == 0 ? "" : this.amount_2040.ToString() + "w" + "(" + (this.real_amount_2040 - this.old_amount).ToString() + "w)";
                    }
                }

                public decimal? real_amount_2050
                {
                    get
                    {
                        if (this.amount_2050 == 0)
                        {
                            return this.real_amount_2040;
                        }
                        else
                        {
                            return this.amount_2050;
                        }
                    }
                }
                public string amount_2050_num
                {
                    get
                    {
                        return this.amount_2050 == 0 ? "" : this.amount_2050.ToString() + "w" + "(" + (this.real_amount_2050 - this.real_amount_2040).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2100
                {
                    get
                    {
                        if (this.amount_2100 == 0)
                        {
                            return this.real_amount_2050;
                        }
                        else
                        {
                            return this.amount_2100;
                        }
                    }
                }
                public string amount_2100_num
                {
                    get
                    {
                        return this.amount_2100 == 0 ? "" : this.amount_2100.ToString() + "w" + "(" + (this.real_amount_2100 - this.real_amount_2050).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2110
                {
                    get
                    {
                        if (this.amount_2110 == 0)
                        {
                            return this.real_amount_2100;
                        }
                        else
                        {
                            return this.amount_2110;
                        }
                    }
                }
                public string amount_2110_num
                {
                    get
                    {
                        return this.amount_2110 == 0 ? "" : this.amount_2110.ToString() + "w" + "(" + (this.real_amount_2110 - this.real_amount_2100).ToString() + "w" + ")";
                    }
                }


                public decimal? real_amount_2120
                {
                    get
                    {
                        if (this.amount_2120 == 0)
                        {
                            return this.real_amount_2110;
                        }
                        else
                        {
                            return this.amount_2120;
                        }
                    }
                }
                public string amount_2120_num
                {
                    get
                    {
                        return this.amount_2120 == 0 ? "" : this.amount_2120.ToString() + "w" + "(" + (this.real_amount_2120 - this.real_amount_2110).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2130
                {
                    get
                    {
                        if (this.amount_2130 == 0)
                        {
                            return this.real_amount_2120;
                        }
                        else
                        {
                            return this.amount_2130;
                        }
                    }
                }
                public string amount_2130_num
                {
                    get
                    {
                        return this.amount_2130 == 0 ? "" : this.amount_2130.ToString() + "w" + "(" + (this.real_amount_2130 - this.real_amount_2120).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2140
                {
                    get
                    {
                        if (this.amount_2140 == 0)
                        {
                            return this.real_amount_2130;
                        }
                        else
                        {
                            return this.amount_2140;
                        }
                    }
                }
                public string amount_2140_num
                {
                    get
                    {
                        return this.amount_2140 == 0 ? "" : this.amount_2140.ToString() + "w" + "(" + (this.real_amount_2140 - this.real_amount_2130).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2150
                {
                    get
                    {
                        if (this.amount_2150 == 0)
                        {
                            return this.real_amount_2140;
                        }
                        else
                        {
                            return this.amount_2150;
                        }
                    }
                }
                public string amount_2150_num
                {
                    get
                    {
                        return this.amount_2150 == 0 ? "" : this.amount_2150.ToString() + "w" + "(" + (this.real_amount_2150 - this.real_amount_2140).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2200
                {
                    get
                    {
                        if (this.amount_2200 == 0)
                        {
                            return this.real_amount_2150;
                        }
                        else
                        {
                            return this.amount_2200;
                        }
                    }
                }
                public string amount_2200_num
                {
                    get
                    {
                        return this.amount_2200 == 0 ? "" : this.amount_2200.ToString() + "w" + "(" + (this.real_amount_2200 - this.real_amount_2150).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2210
                {
                    get
                    {
                        if (this.amount_2210 == 0)
                        {
                            return this.real_amount_2200;
                        }
                        else
                        {
                            return this.amount_2210;
                        }
                    }
                }
                public string amount_2210_num
                {
                    get
                    {
                        return this.amount_2210 == 0 ? "" : this.amount_2210.ToString() + "w" + "(" + (this.real_amount_2210 - this.real_amount_2200).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2220
                {
                    get
                    {
                        if (this.amount_2220 == 0)
                        {
                            return this.real_amount_2210;
                        }
                        else
                        {
                            return this.amount_2220;
                        }
                    }
                }
                public string amount_2220_num
                {
                    get
                    {
                        return this.amount_2220 == 0 ? "" : this.amount_2220.ToString() + "w" + "(" + (this.real_amount_2220 - this.real_amount_2210).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2230
                {
                    get
                    {
                        if (this.amount_2230 == 0)
                        {
                            return this.real_amount_2220;
                        }
                        else
                        {
                            return this.amount_2230;
                        }
                    }
                }
                public string amount_2230_num
                {
                    get
                    {
                        return this.amount_2230 == 0 ? "" : this.amount_2230.ToString() + "w" + "(" + (this.real_amount_2230 - this.real_amount_2220).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2240
                {
                    get
                    {
                        if (this.amount_2240 == 0)
                        {
                            return this.real_amount_2230;
                        }
                        else
                        {
                            return this.amount_2240;
                        }
                    }
                }
                public string amount_2240_num
                {
                    get
                    {
                        return this.amount_2240 == 0 ? "" : this.amount_2240.ToString() + "w" + "(" + (this.real_amount_2240 - this.real_amount_2230).ToString() + "w" + ")";
                    }
                }
                public decimal? real_amount_2250
                {
                    get
                    {
                        if (this.amount_2250 == 0)
                        {
                            return this.real_amount_2240;
                        }
                        else
                        {
                            return this.amount_2250;
                        }
                    }
                }
                public string amount_2250_num
                {
                    get
                    {
                        return this.amount_2250 == 0 ? "" : this.amount_2250.ToString() + "w" + "(" + (this.real_amount_2250 - this.real_amount_2240).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2300
                {
                    get
                    {
                        if (this.amount_2300 == 0)
                        {
                            return this.real_amount_2250;
                        }
                        else
                        {
                            return this.amount_2300;
                        }
                    }
                }
                public string amount_2300_num
                {
                    get
                    {
                        return this.amount_2300 == 0 ? "" : this.amount_2300.ToString() + "w" + "(" + (this.real_amount_2300 - this.real_amount_2250).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2310
                {
                    get
                    {
                        if (this.amount_2310 == 0)
                        {
                            return this.real_amount_2300;
                        }
                        else
                        {
                            return this.amount_2310;
                        }
                    }
                }
                public string amount_2310_num
                {
                    get
                    {
                        return this.amount_2310 == 0 ? "" : this.amount_2310.ToString() + "w" + "(" + (this.real_amount_2310 - this.real_amount_2300).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2320
                {
                    get
                    {
                        if (this.amount_2320 == 0)
                        {
                            return this.real_amount_2310;
                        }
                        else
                        {
                            return this.amount_2320;
                        }
                    }
                }
                public string amount_2320_num
                {
                    get
                    {
                        return this.amount_2320 == 0 ? "" : this.amount_2320.ToString() + "w" + "(" + (this.real_amount_2320 - this.real_amount_2310).ToString() + "w" + ")";
                    }
                }
                public decimal? real_amount_2330
                {
                    get
                    {
                        if (this.amount_2330 == 0)
                        {
                            return this.real_amount_2320;
                        }
                        else
                        {
                            return this.amount_2330;
                        }
                    }
                }
                public string amount_2330_num
                {
                    get
                    {
                        return this.amount_2330 == 0 ? "" : this.amount_2330.ToString() + "w" + "(" + (this.real_amount_2330 - this.real_amount_2320).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2340
                {
                    get
                    {
                        if (this.amount_2340 == 0)
                        {
                            return this.real_amount_2330;
                        }
                        else
                        {
                            return this.amount_2340;
                        }
                    }
                }
                public string amount_2340_num
                {
                    get
                    {
                        return this.amount_2340 == 0 ? "" : this.amount_2340.ToString() + "w" + "(" + (this.real_amount_2340 - this.real_amount_2330).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2350
                {
                    get
                    {
                        if (this.amount_2350 == 0)
                        {
                            return this.real_amount_2340;
                        }
                        else
                        {
                            return this.amount_2350;
                        }
                    }
                }
                public string amount_2350_num
                {
                    get
                    {
                        return this.amount_2350 == 0 ? "" : this.amount_2350.ToString() + "w" + "(" + (this.real_amount_2350 - this.real_amount_2340).ToString() + "w" + ")";
                    }
                }

                public decimal? real_amount_2400
                {
                    get
                    {
                        if (this.amount_2400 == 0)
                        {
                            return this.real_amount_2350;
                        }
                        else
                        {
                            return this.amount_2400;
                        }
                    }
                }
                public string amount_2400_num
                {
                    get
                    {
                        return this.amount_2400 == 0 ? "" : this.amount_2400.ToString() + "w" + "(" + (this.real_amount_2400 - this.real_amount_2350).ToString() + "w" + ")";
                    }
                }
                public string total
                {
                    get
                    {
                        return (this.real_amount_2400 - this.old_amount).ToString() + "w";
                    }
                }
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 表单提交处理的回调函数
            /// </summary>
            /// <param name="req">回调函数提交参数统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DelAction(JsonRequestAction req)
            {
                var p_jixiao_kuafang = req.data_json.ToModel<ModelDb.p_jixiao_kuafang>();
                var info = new JsonResultAction();
                var lSql = new List<string>();
                lSql.Add(p_jixiao_kuafang.DeleteTran());
                DoMySql.ExecuteSqlTran(lSql);
                return info;
            }

            #endregion
        }
        /// <summary>
        /// 每日上报绩效数据
        /// </summary>
        public class YYKuafangPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.buttonGroup = GetButtonGroup(req);
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");

                return buttonGroup;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var p_jixiao_kuafang = DoMySql.FindEntityById<ModelDb.p_jixiao_kuafang>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                {
                    title = "绩效发生日期",
                    defaultValue = p_jixiao_kuafang.c_date.ToDateTime().ToString("yyyy-MM-dd"),
                    mold = ModelBasic.EmtTimeSelect.Mold.date
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("old_amount")
                {
                    title = "跨房前流水",
                    defaultValue = p_jixiao_kuafang.old_amount.ToNullableString(),
                    colLength = 5
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_num")
                {
                    title = "参加跨房厅数",
                    defaultValue = p_jixiao_kuafang.ting_num.ToNullableString(),
                    colLength = 5
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2040")
                {
                    title = "20:40",
                    defaultValue = p_jixiao_kuafang.amount_2040.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2050")
                {
                    title = "20:50",
                    defaultValue = p_jixiao_kuafang.amount_2050.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2100")
                {
                    title = "21:00",
                    defaultValue = p_jixiao_kuafang.amount_2100.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2110")
                {
                    title = "21:10",
                    defaultValue = p_jixiao_kuafang.amount_2110.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2120")
                {
                    title = "21:20",
                    defaultValue = p_jixiao_kuafang.amount_2120.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2130")
                {
                    title = "21:35",
                    defaultValue = p_jixiao_kuafang.amount_2130.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2140")
                {
                    title = "21:40",
                    defaultValue = p_jixiao_kuafang.amount_2140.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2150")
                {
                    title = "21:50",
                    defaultValue = p_jixiao_kuafang.amount_2150.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2200")
                {
                    title = "22:00",
                    defaultValue = p_jixiao_kuafang.amount_2200.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2210")
                {
                    title = "22:10",
                    defaultValue = p_jixiao_kuafang.amount_2210.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2220")
                {
                    title = "22:20",
                    defaultValue = p_jixiao_kuafang.amount_2220.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2230")
                {
                    title = "22:35",
                    defaultValue = p_jixiao_kuafang.amount_2230.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2240")
                {
                    title = "22:40",
                    defaultValue = p_jixiao_kuafang.amount_2240.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2250")
                {
                    title = "22:50",
                    defaultValue = p_jixiao_kuafang.amount_2250.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2300")
                {
                    title = "23:00",
                    defaultValue = p_jixiao_kuafang.amount_2300.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2310")
                {
                    title = "23:10",
                    defaultValue = p_jixiao_kuafang.amount_2310.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2320")
                {
                    title = "23:20",
                    defaultValue = p_jixiao_kuafang.amount_2320.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2330")
                {
                    title = "23:35",
                    defaultValue = p_jixiao_kuafang.amount_2330.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2340")
                {
                    title = "23:40",
                    defaultValue = p_jixiao_kuafang.amount_2340.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2350")
                {
                    title = "23:50",
                    defaultValue = p_jixiao_kuafang.amount_2350.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2400")
                {
                    title = "24:00",
                    defaultValue = p_jixiao_kuafang.amount_2400.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total")
                {
                    title = "跨房总流水",
                    defaultValue = p_jixiao_kuafang.amount_total.ToNullableString(),
                    colLength = 4
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total_day")
                {
                    title = "当天总流水",
                    defaultValue = p_jixiao_kuafang.amount_total_day.ToNullableString(),
                    colLength = 4
                });
                #endregion
                return formDisplay;
            }
            public class DtoReq
            {
                public int id { get; set; }
            }


            #endregion
            #region 异步请求处理
            /// <summary>
            /// 新增跨房数据
            /// </summary>
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                List<string> lSql = new List<string>();
                var result = new JsonResultAction();
                var p_jixiao_kuafang = req.data_json.ToModel<ModelDb.p_jixiao_kuafang>();
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                if (p_jixiao_kuafang.id == 0)
                {
                    if (p_jixiao_kuafang.c_date.IsNullOrEmpty()) throw new Exception("请选择绩效发生日期");
                    if (!DoMySql.FindEntity<ModelDb.p_jixiao_kuafang>($"c_date='{p_jixiao_kuafang.c_date}'", false).IsNullOrEmpty()) throw new Exception("当前日期已提交");
                }
                lSql.Add(p_jixiao_kuafang.InsertOrUpdateTran($"id={p_jixiao_kuafang.id}"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            /// <summary>
            /// 定义表单模型
            /// </summary>
            public class DtoReqData
            {
                public int id { get; set; }
                public string old_c_date { get; set; }
            }
            #endregion
        }
    }

}
