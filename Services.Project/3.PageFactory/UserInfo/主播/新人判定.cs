using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiCode.DataBase;
using WeiCode.Domain;
using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Utility;
using WeiCode.Services;

using static WeiCode.Models.ModelBasic;

namespace Services.Project
{
    public partial class PageFactory
    {
        public partial class UserInfo
        {
            public partial class XinRenPanding
            {
                #region 
                /// <summary>
                /// 新人判定规则设置
                /// </summary>
                public class List
                {
                    #region DefaultView
                    /// <summary>
                    /// 获取页面数据模型
                    /// </summary>
                    /// <returns></returns>
                    public ModelBasic.PageList Get(DtoReq req)
                    {
                        var pageModel = new ModelBasic.PageList("pagelist");
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
                        return listFilter;
                    }

                    /// <summary>
                    /// 设置扩展的按钮组
                    /// </summary>
                    /// <param name="req"></param>
                    /// <returns></returns>
                    public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                    {
                        var buttonGroup = new ModelBasic.EmtButtonGroup("create");
                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("Post")
                        {
                            title = "Post",
                            text = "创建账号",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = "Post"
                            }
                        });

                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LogList")
                        {
                            text = "操作日志",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = "LogList"
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
                        var listDisplay = new ModelBasic.CtlListDisplay();
                        listDisplay.operateWidth = "220";
                        listDisplay.isOpenNumbers = true;
                        listDisplay.listData = new ModelBasic.CtlListDisplay.ListData
                        {
                            funcGetListData = GetListData
                        };
                        #region 1.显示列                  
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("yy_user_sn_test")
                        {
                            text = "所属运营",
                            width = "150",
                            minWidth = "150"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("in_days")
                        {
                            text = "创建账号几天内算新人",
                            width = "150",
                            minWidth = "100"
                        });
                        listDisplay.listItems.Add(new ModelBasic.EmtModel.ListItem("create_time_text")
                        {
                            text = "创建时间",
                            width = "150",
                            minWidth = "150"
                        });

                        #endregion

                        #region 3.操作列
                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            actionEvent = ModelBasic.EmtModel.ListOperateItem.ActionEvent.弹出窗口,
                            eventOpenLayer = new ModelBasic.EmtModel.ListOperateItem.EventOpenLayer
                            {
                                url = "Post",
                                field_paras = "id"
                            },
                            text = "编辑",
                            name = "Post"
                        });

                        listDisplay.listOperateItems.Add(new ModelBasic.EmtModel.ListOperateItem
                        {
                            name = "Del",
                            style = "",
                            text = "删除",
                            title = "删除",
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
                    public class DtoReq
                    {
                    }
                    #endregion
                    #region ListData
                    /// <summary>
                    /// 菜品表data查询
                    /// </summary>
                    /// <returns></returns>
                    public ModelResult.List GetListData(ModelBasic.CtlListDisplay.ListData.Req reqJson)
                    {
                        string where = $"1=1";
                        //执行查询
                        var filter = new DoMySql.Filter
                        {
                            where = where,
                            orderby = " id desc"
                        };
                        return new ModelBasic.CtlListDisplay.ListData().getList<ModelDb.user_info_yy_newer, ItemDataModel>(filter, reqJson);
                    }
                    /// <summary>
                    /// 自定义筛选参数（自定义数据，与属性对应）
                    /// </summary>
                    public class DtoReqListData : ModelBasic.PageList.ListData.Req
                    {
                        /// <summary>
                        /// 
                        /// </summary>
                        public string name { get; set; }
                        public string id { get; set; }
                        public string status { get; set; }
                        public string parent_id { get; set; }

                    }
                    /// <summary>
                    /// 数据项模型
                    /// </summary>
                    public class ItemDataModel : ModelDb.user_info_yy_newer
                    {
                        public string create_time_text
                        {
                            get

                            {
                                return create_time.ToDate().ToString("yyyy-MM-dd");
                            }
                        }
                        public string yy_user_sn_test
                        {
                            get
                            {
                                return new ServiceFactory.UserInfo.Yy().GetInfoByUserSn(yy_user_sn).username;
                            }
                        }
                    }
                    #endregion
                    #region 批量删除操作
                    /// <summary>
                    /// 链接提交处理的回调函数
                    /// </summary>
                    /// <param name="req">回调函数提交统一的封装对象</param>
                    /// <returns></returns>
                    public JsonResultAction DeletesAction(JsonRequestAction req)
                    {
                        var result = new JsonResultAction();
                        var user_info_yy_newer = req.data_json.ToModel<ModelDb.user_info_yy_newer>();
                        user_info_yy_newer.Delete();
                        return result;
                    }
                    #endregion
                }
                /// <summary>
                /// 
                /// </summary>
                public class Post
                {
                    #region DefaultView
                    public ModelBasic.PagePost Get(DtoReq req)
                    {
                        var pageModel = new ModelBasic.PagePost("");
                        pageModel.postedReturn = new ModelBasic.PagePost.PostedReturn
                        {
                            returnType = ModelBasic.PagePost.PostedReturn.ReturnType.刷新父窗口
                        };

                        pageModel.buttonGroup = GetButtonGroup(req);
                        pageModel.formDisplay = GetFormDisplay(pageModel, req);
                        pageModel.eventCsAction = new ModelBasic.PagePost.EventCsAction
                        {
                            func = PostAction
                        };
                        return pageModel;
                    }

                    /// <summary>
                    /// 设置扩展的按钮组
                    /// </summary>
                    /// <param name="req"></param>
                    /// <returns></returns>
                    public ModelBasic.EmtButtonGroup GetButtonGroup(DtoReq req = null)
                    {
                        var buttonGroup = new ModelBasic.EmtButtonGroup("EmtButtonGroup");
                        buttonGroup.buttonItems.Add(new ModelBasic.EmtModel.ButtonItem("LogList")
                        {
                            text = "操作日志",
                            mode = ModelBasic.EmtModel.ButtonItem.Mode.页面弹框按钮,
                            eventOpenLayer = new ModelBasic.EmtModel.ButtonItem.EventOpenLayer
                            {
                                url = "LogList"
                            }
                        });
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
                        var user_info_yy_newer = DoMySql.FindEntityById<ModelDb.user_info_yy_newer>(req.id);
                        if (user_info_yy_newer.in_days.IsNullOrEmpty()) user_info_yy_newer.in_days = 5;

                        #region 表单元素
                        formDisplay.formItems.Add(new ModelBasic.EmtHidden("id")
                        {
                            defaultValue = user_info_yy_newer.id.ToNullableString()
                        });
                        var yy_options = new ServiceFactory.UserInfo.Yy().GetAllYyForKv();

                        formDisplay.formItems.Add(new ModelBasic.EmtSelect("yy_user_sn")
                        {
                            title = "运营账号",
                            options = yy_options,
                            defaultValue = user_info_yy_newer.yy_user_sn
                        });
                        formDisplay.formItems.Add(new ModelBasic.EmtInput("in_days")
                        {
                            title = "注册后数日内为新人",
                            defaultValue = user_info_yy_newer.in_days.ToString(),
                            colLength = 6,
                        });

                        #endregion
                        return formDisplay;
                    }
                    public class DtoReq
                    {
                        public int id { get; set; }
                    }
                    #endregion
                    #region 
                    /// <summary>
                    /// 处理提交判定规则
                    /// </summary>
                    /// <param name="req">回调函数提交参数统一的封装对象</param>
                    /// <returns></returns>
                    public JsonResultAction PostAction(JsonRequestAction req)
                    {
                        var result = new JsonResultAction();
                        try
                        {
                            var user_info_yy_newer = req.data_json.ToModel<ModelDb.user_info_yy_newer>();   //user_info_yy_newer实体
                            var yy_user_sn = user_info_yy_newer.yy_user_sn;  //所属运营sn

                            // 检查yy_user_sn是否已存在
                            var existingData = DoMySql.FindEntity<ModelDb.user_info_yy_newer>($"yy_user_sn='{yy_user_sn}'", false);
                            if (!existingData.IsNullOrEmpty() && user_info_yy_newer.id == 0) throw new WeicodeException("所选运营已有数据，不允许新增");

                            if (new DomainBasic.UserTypeApp().GetInfo().sys_code == ModelEnum.UserTypeEnum.yyer.ToString())
                            {
                                user_info_yy_newer.yy_user_sn = new UserIdentityBag().user_sn;
                            }

                            user_info_yy_newer.tenant_id = new DomainBasic.TenantApp().GetInfo().id;
                            user_info_yy_newer.InsertOrUpdate();

                            new DomainBasic.SystemBizLogApp().Write("主播规则", ModelDb.sys_biz_log.log_type_enum.产品模块.ToSByte(), new UserIdentityBag().user_sn, $"创建/编辑规则，原注册后数日内为新人：{user_info_yy_newer.in_days}天");
                        }
                        catch (Exception e)
                        {
                            result.code = 1;
                            result.msg = e.Message;
                        }

                        return result;
                    }
                    /// <summary>
                    /// 定义表单模型
                    /// </summary>
                    public class DtoReqData : ModelDb.user_info_yy_newer
                    {

                    }
                    #endregion
                }

                #endregion
            }
        }
    }
}
