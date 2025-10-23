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
    /// 账号管理
    /// </summary>
    public partial class PageFactory
    {
        #region ZB
        /// <summary>
        /// 查看下属的账号数据
        /// </summary>
        public class ZbInfoList
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

                listFilter.formItems.Add(new ModelBasic.EmtInput("user_sn")
                {
                    width = "100px",
                    placeholder = "主播"
                });
                return listFilter;
            }

            /// <summary>
            /// 设置扩展的按钮组
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
            {
                var buttonGroup = new ModelBasic.EmtButtonGroup("");
                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("InfoPost")
                {
                    title = "InfoPost",
                    text = "新增",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "InfoPost"
                    }
                });

                buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("ExcelPost")
                {
                    title = "ExcelPost",
                    text = "Excel导入",
                    mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                    eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                    {
                        url = "ExcelPost"
                    }
                });
                return buttonGroup;
            }

            /// <summary>
            /// 设置列表显示的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public ModelBasic.CtlListDisplay GetListDisplay(DtoReq req = null)
            {
                var listDisplay = new ModelBasic.CtlListDisplay(req);
                listDisplay.operateWidth = "350";
                listDisplay.isOpenCheckBox = false;
                listDisplay.isOpenNumbers = false;

                listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                {
                    funcGetListData = GetListData
                };

                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("user_sn")
                {
                    text = "所属用户",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("app_id")
                {
                    text = "直播账号id",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("age")
                {
                    text = "年龄",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("marriage")
                {
                    text = "是否已婚",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("child")
                {
                    text = "有无孩子",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sound_card")
                {
                    text = "声卡",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("full_or_part")
                {
                    text = "兼职全职",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("address")
                {
                    text = "地区",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("experience")
                {
                    text = "直播经验",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("job")
                {
                    text = "现实工作",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("revenue")
                {
                    text = "目标收入",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("sessions")
                {
                    text = "接档时间",
                    width = "120",
                    minWidth = "120"
                }); listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("birthday")
                {
                    text = "生日",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("star_sign")
                {
                    text = "星座",
                    width = "120",
                    minWidth = "120"
                }); listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("talent")
                {
                    text = "才艺",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("way")
                {
                    text = "招聘渠道",
                    width = "120",
                    minWidth = "120"
                }); listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mobile")
                {
                    text = "电话号码",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("devices_num")
                {
                    text = "设备数量",
                    width = "120",
                    minWidth = "120"
                });
                listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("mbti")
                {
                    text = "mbti人格",
                    width = "120",
                    minWidth = "120"
                });
                #region 操作列按钮
                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
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

                listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                {
                    name = "Del",
                    style = "",
                    text = "删除",
                    title = "提示说明",
                    actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.请求处理_回调cs函数,
                    eventCsAction = new ModelBasic.EmtModel.ListOperateItem.EventCsAction
                    {
                        func = DeletesAction,
                        field_paras = "id"
                    }
                });
                #endregion
                return listDisplay;
            }

            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : ModelBasic.PageList.Req
            {
                /// <summary>
                /// 
                /// </summary>
                public FilterForm filterForm { get; set; } = new FilterForm();

                /// <summary>
                /// 筛选参数（自定义）
                /// </summary>
                public class FilterForm
                {
                    /// <summary>
                    /// 关键词
                    /// </summary>
                    public string keyword { get; set; }
                }
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
                string where = $"status = '{ModelDb.user_base.status_enum.正常.ToSByte()}'";



                var dtoReqListData = reqJson.data_json.ToModel<DtoReqListData>();

                //查询条件
                if (!dtoReqListData.keyword.IsNullOrEmpty()) where += $" AND (name like '%{dtoReqListData.keyword}%' OR introduce like '%{dtoReqListData.keyword}%')";

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where + " order by id desc "
                };
                return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_base, ItemDataModel>(filter, reqJson);
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
            public class ItemDataModel : ModelDb.user_base
            {
            }
            #endregion
            #region 异步请求处理
            /// <summary>
            /// 批量删除资产
            /// </summary>
            /// <param name="req">回调函数提交统一的封装对象</param>
            /// <returns></returns>
            public JsonResultAction DeletesAction(JsonRequestAction req)
            {
                var dtoReqData = req.data_json.ToModel<DtoReqData>();
                var result = new JsonResultAction();
                List<string> lSql = new List<string>();
                var user_base = new ModelDb.user_base();
                user_base.status = ModelDb.user_base.status_enum.逻辑删除.ToSByte();
                lSql.Add(user_base.UpdateTran($"id = ({dtoReqData.id})"));
                DoMySql.ExecuteSqlTran(lSql);
                return result;
            }
            public class DtoReqData : ModelDb.user_base
            {
                public string id { get; set; }
            }
            #endregion
        }

        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class ZbInfoPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                var user_info_zb = DoMySql.FindEntity<ModelDb.user_info_zb>($"user_sn='{user_base.user_sn}'", false);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = user_info_zb.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_sn")
                {
                    defaultValue = user_base.user_sn
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("username")
                {
                    title = "主播昵称",
                    defaultValue= user_base.username,
                    isRequired = false,
                    displayStatus= EmtModelBase.DisplayStatus.只读,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("app_id")
                {
                    title = "直播账号id",
                    isRequired = false,
                    defaultValue = user_info_zb.app_id,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("join_date")
                {
                    title = "入职时间",
                    isRequired = false,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("age")
                {
                    title = "年龄",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.age.ToNullableString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("marriage")
                {
                    title = "是否已婚",
                    isRequired = false,
                    colLength = 6,
                    options=new Dictionary<string, string>
                    {
                        {"已婚","已婚"},
                        {"未婚","未婚"},
                        {"离异","离异"}
                    },
                    defaultValue = user_info_zb.marriage,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("child")
                {
                    title = "有无孩子",
                    isRequired = false,
                    options= new Dictionary<string, string>
                    {
                        {"有","有"},
                        {"无","无"},
                    },
                    colLength = 6,
                    defaultValue = user_info_zb.child,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("sound_card")
                {
                    title = "声卡",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.sound_card,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("full_or_part")
                {
                    title = "兼职全职",
                    isRequired = false,
                    colLength = 6,
                    displayStatus= EmtModelBase.DisplayStatus.只读,
                    defaultValue = user_base.attach1,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                {
                    title = "地区",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.address,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("experience")
                {
                    title = "直播经验",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.experience,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("job")
                {
                    title = "现实工作",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.job,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("revenue")
                {
                    title = "目标收入",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.revenue,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("sessions")
                {
                    title = "接档时间",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.sessions,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("birthday")
                {
                    title = "生日",
                    isRequired = false,
                    mold= ModelBasic.EmtTimeSelect.Mold.date,
                    colLength = 6,
                    defaultValue = user_info_zb.birthday
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("star_sign")
                {
                    title = "星座",
                    isRequired = false,
                    colLength = 6,
                    options=new DomainBasic.DictionaryApp().GetListForOption("星座"),
                    defaultValue = user_info_zb.star_sign,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("talent")
                {
                    title = "才艺",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.talent,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("way")
                {
                    title = "招聘渠道",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.way,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("mobile")
                {
                    title = "电话号码",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_base.mobile,
                    displayStatus= EmtModelBase.DisplayStatus.只读,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInputNumber("devices_num")
                {
                    title = "设备数量",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.devices_num.ToNullableString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("mbti")
                {
                    title = "mbti人格",
                    isRequired = false,
                    colLength = 6,
                    defaultValue = user_info_zb.mbti,
                    options = new DomainBasic.DictionaryApp().GetListForOption("MBTI"),
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq : ModelBasic.PagePost.Req
            {
                public int id { get; set; }
                public string user_sn { get; set; }
                public string relation_type { get; set; } = "";
                public string user_type { get; set; }
            }
            #endregion
            #region 异步请求处理
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var user_info_zb = req.data_json.ToModel<ModelDb.user_info_zb>();
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_info_zb.user_sn}'");
                if (user_base.IsNullOrEmpty()) throw new Exception($@"主播:""{req.GetPara()["username"]}""不存在");

                user_info_zb.tenant_id=new DomainBasic.TenantApp().GetInfo().id;
                user_info_zb.InsertOrUpdate($"user_sn='{user_info_zb.user_sn}'");
                return result;
            }
            #endregion
        }

        /// <summary>
        /// 主播信息excel导入
        /// </summary>
        public class ZbExcelPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("PagePost");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = req.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("relation_type")
                {
                    defaultValue = req.relation_type.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("user_type")
                {
                    defaultValue = req.user_type.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtExcelRead("l_excel")
                {
                    title = "选择excel表",
                    colItems = new List<ModelBasic.EmtExcelRead.ColItem>
                    {
                        new ModelBasic.EmtExcelRead.ColItem("user_sn")
                        {
                         title = "主播昵称",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("app_id")
                        {
                         title = "直播账号id",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("age")
                        {
                         title = "年龄",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("marriage")
                        {
                         title = "是否已婚",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("child")
                        {
                         title = "有无孩子",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("sound_card")
                        {
                         title = "声卡",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("full_or_part")
                        {
                         title = "兼职全职",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("address")
                        {
                         title = "地区",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("experience")
                        {
                         title = "直播经验",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("job")
                        {
                         title = "现实工作",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("revenue")
                        {
                         title = "目标收入",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("sessions")
                        {
                         title = "接档时间",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("birthday")
                        {
                         title = "生日",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("star_sign")
                        {
                         title = "星座",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("talent")
                        {
                         title = "才艺",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("way")
                        {
                         title = "招聘渠道",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("mobile")
                        {
                         title = "电话号码",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("devices_num")
                        {
                         title = "设备数量",
                         edit = "text",
                        },
                        new ModelBasic.EmtExcelRead.ColItem("mbti")
                        {
                         title = "mbti人格",
                         edit = "text",
                        },
                     },
                    displayStatus = EmtModelBase.DisplayStatus.编辑,
                    exampleFileUrl= "/DownFile/主播信息导入示例.xls",
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq : ModelBasic.PagePost.Req
            {
                public int id { get; set; }
                public string relation_type { get; set; } = "";
                public string user_type { get; set; }
            }
            #endregion
            #region 异步请求处理
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var user_info_excel = req.data_json.ToModel<user_info_excel>();



                return result;
            }


            public class user_info_excel
            {
                public List<ModelDb.user_info_zb> user_info_zbs { get; set; }
            }
            #endregion
        }
        #endregion

        #region TG
        /// <summary>
        /// 创建/编辑页面
        /// </summary>
        public class TgInfoPost
        {
            #region DefaultView
            public ModelBasic.PagePost Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PagePost("post");
                pageModel.formDisplay = GetFormDisplay(pageModel, req);
                pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                {
                    func = PostAction,
                };
                return pageModel;
            }

            /// <summary>
            /// 配置表单元素控件
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            private ModelBasic.CtlFormDisplay GetFormDisplay(ModelBasic.PagePost pageModel, DtoReq req)
            {
                var formDisplay = pageModel.formDisplay;
                var user_base = DoMySql.FindEntityById<ModelDb.user_base>(req.id);
                var user_info_tg = DoMySql.FindEntity<ModelDb.user_info_tg>($"tg_user_sn='{user_base.user_sn}'", false);
                #region 表单元素
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                {
                    defaultValue = user_info_tg.id.ToNullableString()
                });
                formDisplay.formItems.Add(new ModelBasic.EmtHidden("tg_user_sn")
                {
                    defaultValue = user_base.user_sn
                });

                formDisplay.formItems.Add(new ModelBasic.EmtInput("tg_username")
                {
                    title = "厅管账号",
                    defaultValue = user_base.username,
                    displayStatus = EmtModelBase.DisplayStatus.只读,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("mode")
                {
                    title = "模式",
                    options = new DomainBasic.DictionaryApp().GetListForOption("厅模式"),
                    defaultValue = user_info_tg.mode,
                    colLength = 6,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("dou_user")
                {
                    title = "抖音号",
                    isRequired = true,
                    colLength = 6,
                    defaultValue = user_info_tg.dou_user,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("ting_name")
                {
                    title = "厅名",
                    colLength = 6,
                    defaultValue = user_info_tg.ting_name,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("tg_sex")
                {
                    title = "性别",
                    options = new Dictionary<string, string>
                    {
                        {"男","男"},
                        {"女","女"},
                    },
                    colLength = 6,
                    defaultValue = user_info_tg.tg_sex,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("phone")
                {
                    title = "电话",
                    colLength = 6,
                    defaultValue = user_info_tg.phone,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("birthday")
                {
                    title = "出生日期",
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.date,
                    defaultValue = user_info_tg.birthday.ToDateString(ConvertExt.DateFormate.yyyy_MM_dd),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtSelect("mbti")
                {
                    title = "人格mbti",
                    colLength = 6,
                    defaultValue = user_info_tg.mbti,
                    options = new DomainBasic.DictionaryApp().GetListForOption("MBTI"),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("open_ting_time")
                {
                    title = "开厅时间",
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.time,
                    defaultValue = user_info_tg.open_ting_time.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtExt.XmSelect("current_open_dangwei")
                {
                    title = "开厅时间段",
                    colLength = 6,
                    bindOptions = new DomainBasic.DictionaryApp().GetListForOption(ModelEnum.DictCategory.档位时段),
                    defaultValue = user_info_tg.current_open_dangwei,
                });
                formDisplay.formItems.Add(new ModelBasic.EmtTimeSelect("join_party_time")
                {
                    title = "加入公会",
                    colLength = 6,
                    mold = ModelBasic.EmtTimeSelect.Mold.datetime,
                    defaultValue = user_info_tg.join_party_time.ToString(),
                });
                formDisplay.formItems.Add(new ModelBasic.EmtInput("address")
                {
                    title = "地址",
                    colLength = 6,
                    defaultValue = user_info_tg.address,
                });
                #endregion
                return formDisplay;
            }

            public class DtoReq : ModelBasic.PagePost.Req
            {
                public int id { get; set; }
                public string user_sn { get; set; }
                public string relation_type { get; set; } = "";
                public string user_type { get; set; }
            }
            #endregion
            #region 异步请求处理
            public JsonResultAction PostAction(JsonRequestAction req)
            {
                var result = new JsonResultAction();
                var user_info_tg = req.GetPara<ModelDb.user_info_tg>();// data_json.ToModel<ModelDb.user_info_tg>();
                var user_base = DoMySql.FindEntity<ModelDb.user_base>($"user_sn='{user_info_tg.tg_user_sn}'");
                if (user_base.IsNullOrEmpty()) throw new Exception($@"厅管:""{req.GetPara()["tg_username"]}""不存在");

                user_info_tg.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                user_info_tg.InsertOrUpdate($"tg_user_sn='{user_info_tg.tg_user_sn}'");
                return result;
            }
            #endregion
        }
        #endregion
    }
}
