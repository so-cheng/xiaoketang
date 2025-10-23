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
    /// 绩效-厅战
    /// </summary>
    public partial class PageFactory
    {
        public partial class TingZhan
        {
            /// <summary>
            /// 厅站数据列表页
            /// </summary>
            public class TingZhanList
            {
                #region DefaultView
                /// <summary>
                /// 获取页面数据模型
                /// </summary>
                /// <returns></returns>
                public ModelBasic.PageList Get(DtoReq req)
                {
                    ItemDataModel.show_detail = req.show_detail;
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

                    listFilter.formItems.Add(new ModelBasic.EmtButton("hide_detail")
                    {
                        defaultValue = "隐藏增幅",
                        disabled = ItemDataModel.show_detail == 0,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = @"
                                       const currentUrl = new URL(window.location.href);
                                       currentUrl.searchParams.set('show_detail', '0');
                                       window.location.href = currentUrl.toString();"
                            }
                        }
                    });
                    listFilter.formItems.Add(new ModelBasic.EmtButton("show_detail")
                    {
                        defaultValue = "显示增幅",
                        disabled = ItemDataModel.show_detail == 1,
                        eventJsChange = new EmtFormBase.EventJsChange
                        {
                            eventJavascript = new EventJavascript
                            {
                                code = @"
                                       const currentUrl = new URL(window.location.href);
                                       currentUrl.searchParams.set('show_detail', '1');
                                       window.location.href = currentUrl.toString();"
                            }
                        }
                    });
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
                        text = "新增厅战",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"Post",
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

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_num")
                    {
                        text = "参加厅战厅数",
                        width = "160",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("old_amount")
                    {
                        text = "厅战前流水",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_text")
                    {
                        text = "厅战总流水",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_day_text")
                    {
                        text = "厅战当天总流水",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2040_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "20:40",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2040", "amount_2040" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2040")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2040")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2040" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2040.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2040.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2040")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2040.set(page.focus.attr('data-amount_2040'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2050_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "20:50",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2050", "amount_2050" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2050")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2050")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2050" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2050.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2050.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2050")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2050.set(page.focus.attr('data-amount_2050'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2100_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:00",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2100", "amount_2100" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2100")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2100")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2100" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2100.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2100.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2100")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2100.set(page.focus.attr('data-amount_2100'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2110_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:10",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2110", "amount_2110" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2110")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2110")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2110" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2110.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2110.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2110")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2110.set(page.focus.attr('data-amount_2110'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2120_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:20",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2120", "amount_2120" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2120")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2120")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2120" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2120.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2120.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2120")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2120.set(page.focus.attr('data-amount_2120'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2130_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:35",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2130", "amount_2130" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2130")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2130")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2130" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2130.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2130.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2130")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2130.set(page.focus.attr('data-amount_2130'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2140_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:40",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2140", "amount_2140" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2140")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2140")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2140" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2140.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2140.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2140")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2140.set(page.focus.attr('data-amount_2140'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2150_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "21:50",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2150", "amount_2150" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2150")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2150")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2150" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2150.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2150.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2150")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2150.set(page.focus.attr('data-amount_2150'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2200_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:00",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2200", "amount_2200" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2200")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2200")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2200" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2200.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2200.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2200")
                                    {
                                        defaultValue = "取消",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2200.set(page.focus.attr('data-amount_2200'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2210_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:10",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2210", "amount_2210" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2210")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2210")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2210" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2210.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2210.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2210")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2210.set(page.focus.attr('data-amount_2210'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2220_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:20",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2220", "amount_2220" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2220")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2220")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2220" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2220.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2220.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2220")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2220.set(page.focus.attr('data-amount_2220'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2230_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:35",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2230", "amount_2230" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2230")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2230")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2230" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2230.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2230.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2230")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2230.set(page.focus.attr('data-amount_2230'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2240_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:40",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2240", "amount_2240" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2240")
                                    {
                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2240")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2240" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2240.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2240.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2240")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2240.set(page.focus.attr('data-amount_2240'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2250_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "22:50",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2250", "amount_2250" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2250")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2250")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2250" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2250.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2250.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2250")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2250.set(page.focus.attr('data-amount_2250'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2300_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:00",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2300", "amount_2300" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2300")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2300")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2300" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2300.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2300.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2300")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2300.set(page.focus.attr('data-amount_2300'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2310_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:10",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2310", "amount_2310" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2310")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2310")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2310" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2310.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2310.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2310")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2310.set(page.focus.attr('data-amount_2310'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2320_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:20",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2320", "amount_2320" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2320")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2320")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2320" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2320.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2320.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2320")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2320.set(page.focus.attr('data-amount_2320'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2330_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:35",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2330", "amount_2330" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2330")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2330")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2330" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2330.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2330.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2330")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2330.set(page.focus.attr('data-amount_2330'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2340_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:40",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2340", "amount_2340" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2340")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2340")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2340" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2340.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2340.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2340")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2340.set(page.focus.attr('data-amount_2340'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2350_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "23:50",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2350", "amount_2350" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2350")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2350")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2350" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2350.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2350.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2350")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2350.set(page.focus.attr('data-amount_2350'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_2400_num")
                    {
                        mode = ModelBasic.EmtModel.ListItem.Mode.便捷浮动层,
                        text = "24:00",
                        width = "140",
                        minWidth = "160",
                        fastLayer = new ModelBasic.EmtModel.ListItem.FastLayer
                        {
                            fieldsData = new Dictionary<string, string>
                        {
                            {"id", "id" },
                            {"amount_2400", "amount_2400" },
                        },
                            emtModelBase = new ModelBasic.EmtGrid("grid")
                            {
                                items = new List<ModelBasic.EmtGrid.Item>
                            {
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=6,
                                    emtModelBase = new ModelBasic.EmtInput($"amount_text_2400")
                                    {

                                        width = "120"
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("button_2400")
                                    {
                                        defaultValue = "提交",
                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventCsAction=new EmtFormBase.EventJsChange.EventCsAction
                                            {
                                                attachPara=new Dictionary<string, object>
                                                {
                                                    {"name","amount_2400" },
                                                    {"id","<%=page.focus.attr('data-id')%>" },
                                                    {"value","<%=page.amount_text_2400.value%>" }
                                                },
                                                resCallJs="page.focus.text(page.amount_text_2400.value);$('.checkbox_div').hide();",
                                                func=new ServiceFactory.TingZhanService().FastEdit
                                            }
                                        }
                                    }
                                },
                                new ModelBasic.EmtGrid.Item
                                {
                                    colLength=3,
                                    emtModelBase = new ModelBasic.EmtButton("cancel_2400")
                                    {
                                        defaultValue = "取消",

                                        eventJsChange = new EmtFormBase.EventJsChange
                                        {
                                            eventJavascript=new EventJavascript
                                            {
                                                code="$('.checkbox_div').hide();"
                                            }
                                        }
                                    }
                                }
                            }
                            },
                            eventJsShow = new EventJsBasic
                            {
                                eventJavascript = new EventJavascript
                                {
                                    code = "page.amount_text_2400.set(page.focus.attr('data-amount_2400'))"
                                }
                            }
                        }
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("total")
                    {
                        text = "当前实际总流水",
                        width = "140",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("average")
                    {
                        text = "当天平均流水",
                        width = "140",
                        minWidth = "160"
                    });
                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("tingzhan_average")
                    {
                        text = "活动平均流水",
                        width = "140",
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
                    public int show_detail { get; set; } = 1;
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

                    string where = $"tenant_id='{new DomainBasic.TenantApp().GetInfo().id}'";
                    if (!req["c_date_range"].ToNullableString().IsNullOrEmpty())
                    {
                        var dateRange = UtilityStatic.CommonHelper.DateRangeFormat(req["c_date_range"].ToNullableString(), 0);
                        where += " AND  c_date >= '" + dateRange.date_range_s + "' AND c_date <= '" + dateRange.date_range_e + "'";
                    }
                    var filter = new DoMySql.Filter
                    {
                        where = where + " order by c_date desc"
                    };
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_tingzhan, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_jixiao_tingzhan
                {
                    public static int show_detail { get; set; } = 1;

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
                            if (show_detail == 1)
                            {
                                return this.amount_2040 == 0 ? "0" : this.amount_2040.ToString() + "w" + "(" + (this.real_amount_2040 - this.old_amount).ToInt().ToString() + "w)";
                            }
                            else
                            {
                                return this.amount_2040 == 0 ? "0" : this.amount_2040.ToString() + "w";
                            }
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
                            if (show_detail == 1)
                            {
                                return this.amount_2050 == 0 ? "0" : this.amount_2050.ToString() + "w" + "(" + (this.real_amount_2050 - this.real_amount_2040).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2050 == 0 ? "0" : this.amount_2050.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2100 == 0 ? "0" : this.amount_2100.ToString() + "w" + "(" + (this.real_amount_2100 - this.real_amount_2050).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2100 == 0 ? "0" : this.amount_2100.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2110 == 0 ? "0" : this.amount_2110.ToString() + "w" + "(" + (this.real_amount_2110 - this.real_amount_2100).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2110 == 0 ? "0" : this.amount_2110.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2120 == 0 ? "0" : this.amount_2120.ToString() + "w" + "(" + (this.real_amount_2120 - this.real_amount_2110).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2120 == 0 ? "0" : this.amount_2120.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2130 == 0 ? "0" : this.amount_2130.ToString() + "w" + "(" + (this.real_amount_2130 - this.real_amount_2120).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2130 == 0 ? "0" : this.amount_2130.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2140 == 0 ? "0" : this.amount_2140.ToString() + "w" + "(" + (this.real_amount_2140 - this.real_amount_2130).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2140 == 0 ? "0" : this.amount_2140.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2150 == 0 ? "0" : this.amount_2150.ToString() + "w" + "(" + (this.real_amount_2150 - this.real_amount_2140).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2150 == 0 ? "0" : this.amount_2150.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2200 == 0 ? "0" : this.amount_2200.ToString() + "w" + "(" + (this.real_amount_2200 - this.real_amount_2150).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2200 == 0 ? "0" : this.amount_2200.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2210 == 0 ? "0" : this.amount_2210.ToString() + "w" + "(" + (this.real_amount_2210 - this.real_amount_2200).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2210 == 0 ? "0" : this.amount_2210.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2220 == 0 ? "0" : this.amount_2220.ToString() + "w" + "(" + (this.real_amount_2220 - this.real_amount_2210).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2220 == 0 ? "0" : this.amount_2220.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2230 == 0 ? "0" : this.amount_2230.ToString() + "w" + "(" + (this.real_amount_2230 - this.real_amount_2220).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2230 == 0 ? "0" : this.amount_2230.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2240 == 0 ? "0" : this.amount_2240.ToString() + "w" + "(" + (this.real_amount_2240 - this.real_amount_2230).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2240 == 0 ? "0" : this.amount_2240.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2250 == 0 ? "0" : this.amount_2250.ToString() + "w" + "(" + (this.real_amount_2250 - this.real_amount_2240).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2250 == 0 ? "0" : this.amount_2250.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2300 == 0 ? "0" : this.amount_2300.ToString() + "w" + "(" + (this.real_amount_2300 - this.real_amount_2250).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2300 == 0 ? "0" : this.amount_2300.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2310 == 0 ? "0" : this.amount_2310.ToString() + "w" + "(" + (this.real_amount_2310 - this.real_amount_2300).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2310 == 0 ? "0" : this.amount_2310.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2320 == 0 ? "0" : this.amount_2320.ToString() + "w" + "(" + (this.real_amount_2320 - this.real_amount_2310).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2320 == 0 ? "0" : this.amount_2320.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2330 == 0 ? "0" : this.amount_2330.ToString() + "w" + "(" + (this.real_amount_2330 - this.real_amount_2320).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2330 == 0 ? "0" : this.amount_2330.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2340 == 0 ? "0" : this.amount_2340.ToString() + "w" + "(" + (this.real_amount_2340 - this.real_amount_2330).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2340 == 0 ? "0" : this.amount_2340.ToString() + "w";
                            }
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
                            if (show_detail == 1)
                            {
                                return this.amount_2350 == 0 ? "0" : this.amount_2350.ToString() + "w" + "(" + (this.real_amount_2350 - this.real_amount_2340).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2350 == 0 ? "0" : this.amount_2350.ToString() + "w";
                            }

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
                            if (show_detail == 1)
                            {
                                return this.amount_2400 == 0 ? "0" : this.amount_2400.ToString() + "w" + "(" + (this.real_amount_2400 - this.real_amount_2350).ToInt().ToString() + "w" + ")";
                            }
                            else
                            {
                                return this.amount_2400 == 0 ? "0" : this.amount_2400.ToString() + "w";
                            }

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

                    public string average
                    {
                        get
                        {
                            return ting_num == 0 ? "0" : (amount_2400.ToDouble() / ting_num.ToDouble()).ToString("0.00") + "w";
                        }
                    }
                    public string tingzhan_average
                    {
                        get
                        {
                            return ting_num == 0 ? "0" : ((this.real_amount_2400 - this.old_amount).ToDouble() / ting_num.ToDouble()).ToString("0.00") + "w";
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
                    var p_jixiao_tingzhan = req.data_json.ToModel<ModelDb.p_jixiao_tingzhan>();
                    var info = new JsonResultAction();
                    var lSql = new List<string>();
                    lSql.Add(p_jixiao_tingzhan.DeleteTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return info;
                }

                #endregion
            }

            /// <summary>
            /// 每日上报绩效数据
            /// </summary>
            public class TingZhanPost
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
                    var p_jixiao_tingzhan = DoMySql.FindEntityById<ModelDb.p_jixiao_tingzhan>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "绩效发生日期",
                        defaultValue = p_jixiao_tingzhan.c_date.ToDateTime().ToString("yyyy-MM-dd"),
                        mold = ModelBasic.EmtTimeSelect.Mold.date
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("old_amount")
                    {
                        title = "厅战前流水",
                        defaultValue = p_jixiao_tingzhan.old_amount.ToNullableString(),
                        colLength = 5
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_num")
                    {
                        title = "参加厅战厅数",
                        defaultValue = p_jixiao_tingzhan.ting_num.ToNullableString(),
                        colLength = 5
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total")
                    {
                        title = "厅战总流水",
                        defaultValue = p_jixiao_tingzhan.amount_total.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total_day")
                    {
                        title = "当天总流水",
                        defaultValue = p_jixiao_tingzhan.amount_total_day.ToNullableString(),
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
                /// 新增厅战数据
                /// </summary>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var p_jixiao_tingzhan = req.data_json.ToModel<ModelDb.p_jixiao_tingzhan>();
                    if (p_jixiao_tingzhan.id == 0)
                    {
                        if (p_jixiao_tingzhan.c_date.IsNullOrEmpty()) throw new Exception("请选择绩效发生日期");
                        if (!DoMySql.FindEntity<ModelDb.p_jixiao_tingzhan>($"c_date='{p_jixiao_tingzhan.c_date}'", false).IsNullOrEmpty()) throw new Exception("当前日期已提交");
                        p_jixiao_tingzhan.user_sn = new UserIdentityBag().user_sn;
                        p_jixiao_tingzhan.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                        p_jixiao_tingzhan.user_type_id = new DomainBasic.UserTypeApp().GetInfo().id;

                        p_jixiao_tingzhan.amount_2030 = 0;
                        p_jixiao_tingzhan.amount_2040 = 0;
                        p_jixiao_tingzhan.amount_2050 = 0;
                        p_jixiao_tingzhan.amount_2100 = 0;
                        p_jixiao_tingzhan.amount_2110 = 0;
                        p_jixiao_tingzhan.amount_2120 = 0;
                        p_jixiao_tingzhan.amount_2130 = 0;
                        p_jixiao_tingzhan.amount_2140 = 0;
                        p_jixiao_tingzhan.amount_2150 = 0;
                        p_jixiao_tingzhan.amount_2200 = 0;
                        p_jixiao_tingzhan.amount_2210 = 0;
                        p_jixiao_tingzhan.amount_2220 = 0;
                        p_jixiao_tingzhan.amount_2230 = 0;
                        p_jixiao_tingzhan.amount_2240 = 0;
                        p_jixiao_tingzhan.amount_2250 = 0;
                        p_jixiao_tingzhan.amount_2300 = 0;
                        p_jixiao_tingzhan.amount_2310 = 0;
                        p_jixiao_tingzhan.amount_2320 = 0;
                        p_jixiao_tingzhan.amount_2330 = 0;
                        p_jixiao_tingzhan.amount_2340 = 0;
                        p_jixiao_tingzhan.amount_2350 = 0;
                        p_jixiao_tingzhan.amount_2400 = 0;
                    }
                    lSql.Add(p_jixiao_tingzhan.InsertOrUpdateTran($"id={p_jixiao_tingzhan.id}"));
                    DoMySql.ExecuteSqlTran(lSql);
                    return result;
                }

                /// <summary>
                /// 定义表单模型
                /// </summary>
                public class DtoReqData
                {
                    public static DtoReqData instance = new DtoReqData();

                    private DtoReqData()
                    {

                    }
                    public int id { get; set; }
                    public string old_c_date { get; set; }
                }
                #endregion
            }

            /// <summary>
            /// 厅站数据列表页
            /// </summary>
            public class YYTingZhanList
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
                        text = "新增厅战",
                        mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                        eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                        {
                            url = $"TingZhanPost",
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
                        text = "厅战前流水",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("ting_num")
                    {
                        text = "参加厅战厅数",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total")
                    {
                        text = "厅战总流水",
                        width = "160",
                        minWidth = "160"
                    });

                    listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("amount_total_day")
                    {
                        text = "厅战当天总流水",
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
                    return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.p_jixiao_tingzhan, ItemDataModel>(filter, reqJson);
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
                public class ItemDataModel : ModelDb.p_jixiao_tingzhan
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
                            var result = this.amount_2040 == 0 ? "0" : this.amount_2040.ToString() + "w" + "(" + (this.real_amount_2040 - this.old_amount).ToString() + "w)";
                            return result;
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
                            return this.amount_2050 == 0 ? "0" : this.amount_2050.ToString() + "w" + "(" + (this.real_amount_2050 - this.real_amount_2040).ToString() + "w" + ")";
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
                            return this.amount_2100 == 0 ? "0" : this.amount_2100.ToString() + "w" + "(" + (this.real_amount_2100 - this.real_amount_2050).ToString() + "w" + ")";
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
                            return this.amount_2110 == 0 ? "0" : this.amount_2110.ToString() + "w" + "(" + (this.real_amount_2110 - this.real_amount_2100).ToString() + "w" + ")";
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
                            return this.amount_2120 == 0 ? "0" : this.amount_2120.ToString() + "w" + "(" + (this.real_amount_2120 - this.real_amount_2110).ToString() + "w" + ")";
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
                            return this.amount_2130 == 0 ? "0" : this.amount_2130.ToString() + "w" + "(" + (this.real_amount_2130 - this.real_amount_2120).ToString() + "w" + ")";
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
                            return this.amount_2140 == 0 ? "0" : this.amount_2140.ToString() + "w" + "(" + (this.real_amount_2140 - this.real_amount_2130).ToString() + "w" + ")";
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
                            return this.amount_2150 == 0 ? "0" : this.amount_2150.ToString() + "w" + "(" + (this.real_amount_2150 - this.real_amount_2140).ToString() + "w" + ")";
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
                            return this.amount_2200 == 0 ? "0" : this.amount_2200.ToString() + "w" + "(" + (this.real_amount_2200 - this.real_amount_2150).ToString() + "w" + ")";
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
                            return this.amount_2210 == 0 ? "0" : this.amount_2210.ToString() + "w" + "(" + (this.real_amount_2210 - this.real_amount_2200).ToString() + "w" + ")";
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
                            return this.amount_2220 == 0 ? "0" : this.amount_2220.ToString() + "w" + "(" + (this.real_amount_2220 - this.real_amount_2210).ToString() + "w" + ")";
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
                            return this.amount_2230 == 0 ? "0" : this.amount_2230.ToString() + "w" + "(" + (this.real_amount_2230 - this.real_amount_2220).ToString() + "w" + ")";
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
                            return this.amount_2240 == 0 ? "0" : this.amount_2240.ToString() + "w" + "(" + (this.real_amount_2240 - this.real_amount_2230).ToString() + "w" + ")";
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
                            return this.amount_2250 == 0 ? "0" : this.amount_2250.ToString() + "w" + "(" + (this.real_amount_2250 - this.real_amount_2240).ToString() + "w" + ")";
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
                            return this.amount_2300 == 0 ? "0" : this.amount_2300.ToString() + "w" + "(" + (this.real_amount_2300 - this.real_amount_2250).ToString() + "w" + ")";
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
                            return this.amount_2310 == 0 ? "0" : this.amount_2310.ToString() + "w" + "(" + (this.real_amount_2310 - this.real_amount_2300).ToString() + "w" + ")";
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
                            return this.amount_2320 == 0 ? "0" : this.amount_2320.ToString() + "w" + "(" + (this.real_amount_2320 - this.real_amount_2310).ToString() + "w" + ")";
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
                            return this.amount_2330 == 0 ? "0" : this.amount_2330.ToString() + "w" + "(" + (this.real_amount_2330 - this.real_amount_2320).ToString() + "w" + ")";
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
                            return this.amount_2340 == 0 ? "0" : this.amount_2340.ToString() + "w" + "(" + (this.real_amount_2340 - this.real_amount_2330).ToString() + "w" + ")";
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
                            return this.amount_2350 == 0 ? "0" : this.amount_2350.ToString() + "w" + "(" + (this.real_amount_2350 - this.real_amount_2340).ToString() + "w" + ")";
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
                            return this.amount_2400 == 0 ? "0" : this.amount_2400.ToString() + "w" + "(" + (this.real_amount_2400 - this.real_amount_2350).ToString() + "w" + ")";
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
                    var p_jixiao_tingzhan = req.data_json.ToModel<ModelDb.p_jixiao_tingzhan>();
                    var info = new JsonResultAction();
                    var lSql = new List<string>();
                    lSql.Add(p_jixiao_tingzhan.DeleteTran());
                    DoMySql.ExecuteSqlTran(lSql);
                    return info;
                }

                #endregion
            }

            /// <summary>
            /// 每日上报绩效数据
            /// </summary>
            public class YYTingZhanPost
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
                    var p_jixiao_tingzhan = DoMySql.FindEntityById<ModelDb.p_jixiao_tingzhan>(req.id);
                    #region 表单元素
                    formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                    {
                        defaultValue = req.id.ToNullableString()
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("c_date")
                    {
                        title = "绩效发生日期",
                        defaultValue = p_jixiao_tingzhan.c_date.ToDateTime().ToString("yyyy-MM-dd"),
                        mold = ModelBasic.EmtTimeSelect.Mold.date
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("old_amount")
                    {
                        title = "厅战前流水",
                        defaultValue = p_jixiao_tingzhan.old_amount.ToNullableString(),
                        colLength = 5
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("ting_num")
                    {
                        title = "参加厅战厅数",
                        defaultValue = p_jixiao_tingzhan.ting_num.ToNullableString(),
                        colLength = 5
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2040")
                    {
                        title = "20:40",
                        defaultValue = p_jixiao_tingzhan.amount_2040.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2050")
                    {
                        title = "20:50",
                        defaultValue = p_jixiao_tingzhan.amount_2050.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2100")
                    {
                        title = "21:00",
                        defaultValue = p_jixiao_tingzhan.amount_2100.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2110")
                    {
                        title = "21:10",
                        defaultValue = p_jixiao_tingzhan.amount_2110.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2120")
                    {
                        title = "21:20",
                        defaultValue = p_jixiao_tingzhan.amount_2120.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2130")
                    {
                        title = "21:35",
                        defaultValue = p_jixiao_tingzhan.amount_2130.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2140")
                    {
                        title = "21:40",
                        defaultValue = p_jixiao_tingzhan.amount_2140.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2150")
                    {
                        title = "21:50",
                        defaultValue = p_jixiao_tingzhan.amount_2150.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2200")
                    {
                        title = "22:00",
                        defaultValue = p_jixiao_tingzhan.amount_2200.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2210")
                    {
                        title = "22:10",
                        defaultValue = p_jixiao_tingzhan.amount_2210.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2220")
                    {
                        title = "22:20",
                        defaultValue = p_jixiao_tingzhan.amount_2220.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2230")
                    {
                        title = "22:35",
                        defaultValue = p_jixiao_tingzhan.amount_2230.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2240")
                    {
                        title = "22:40",
                        defaultValue = p_jixiao_tingzhan.amount_2240.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2250")
                    {
                        title = "22:50",
                        defaultValue = p_jixiao_tingzhan.amount_2250.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2300")
                    {
                        title = "23:00",
                        defaultValue = p_jixiao_tingzhan.amount_2300.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2310")
                    {
                        title = "23:10",
                        defaultValue = p_jixiao_tingzhan.amount_2310.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2320")
                    {
                        title = "23:20",
                        defaultValue = p_jixiao_tingzhan.amount_2320.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2330")
                    {
                        title = "23:35",
                        defaultValue = p_jixiao_tingzhan.amount_2330.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2340")
                    {
                        title = "23:40",
                        defaultValue = p_jixiao_tingzhan.amount_2340.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2350")
                    {
                        title = "23:50",
                        defaultValue = p_jixiao_tingzhan.amount_2350.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_2400")
                    {
                        title = "24:00",
                        defaultValue = p_jixiao_tingzhan.amount_2400.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total")
                    {
                        title = "厅战总流水",
                        defaultValue = p_jixiao_tingzhan.amount_total.ToNullableString(),
                        colLength = 4
                    });
                    formDisplay.formItems.Add(new ModelBasic.EmtInputMoney("amount_total_day")
                    {
                        title = "当天总流水",
                        defaultValue = p_jixiao_tingzhan.amount_total_day.ToNullableString(),
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
                /// 新增厅战数据
                /// </summary>
                public JsonResultAction PostAction(JsonRequestAction req)
                {
                    List<string> lSql = new List<string>();
                    var result = new JsonResultAction();
                    var p_jixiao_tingzhan = req.data_json.ToModel<ModelDb.p_jixiao_tingzhan>();
                    var dtoReqData = req.data_json.ToModel<DtoReqData>();
                    if (p_jixiao_tingzhan.id == 0)
                    {
                        if (p_jixiao_tingzhan.c_date.IsNullOrEmpty()) throw new Exception("请选择绩效发生日期");
                        if (!DoMySql.FindEntity<ModelDb.p_jixiao_tingzhan>($"c_date='{p_jixiao_tingzhan.c_date}'", false).IsNullOrEmpty()) throw new Exception("当前日期已提交");
                    }
                    lSql.Add(p_jixiao_tingzhan.InsertOrUpdateTran($"id={p_jixiao_tingzhan.id}"));
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
}
