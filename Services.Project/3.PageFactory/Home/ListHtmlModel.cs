using System;
using System.Collections.Generic;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using WeiCode.ModelDbs;
using WeiCode.Models;
using WeiCode.Domain;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static WeiCode.Models.ModelBasic;
using WeiCode.Modular;

namespace Services.Project
{
    public partial class PageFactory
    {
        /// <summary>
        /// 自定义模板列表
        /// </summary>
        public class ListHtml1
        {
            #region DefaultView
            /// <summary>
            /// 获取页面数据模型
            /// </summary>
            /// <returns></returns>
            public ModelBasic.PageListHtml Get(DtoReq req)
            {
                var pageModel = new ModelBasic.PageListHtml("listname");

                pageModel.listFilter = GetListFilter(req);
                string content = "";
                foreach (var item in DoMySql.FindList<ModelDb.sys_notice_category>($""))
                {
                    content += $@"<a href=""ListHtml?category_id={item.id}"" class=""layui-btn {(item.id==req.category_id ? "layui-btn-primary layui-border-blue" : "layui-btn-primary")}"">{item.name}</a>"+"\r\n";
                }
                pageModel.topPartial = new List<ModelBase>
                {
                    new ModelBasic.EmtHtml("html_top")
                    {
                        Content = content,
                    },
                };
                pageModel.tempItemHtml = new CptAttrModelBase.TempItemHtml
                {
                    
                    htmlCode = $@"
                                {{{{# var redDot = ''; }}}}
                                {{{{# var titleColor = 'grey'; }}}}
                                {{{{# if(item.is_read == 0){{redDot = '•';titleColor = 'black';}} }}}}
                                <div class=""layui-card-header"" style=""font-weight: bold;color:{{{{titleColor}}}};"">{{{{item.title}}}}<span style=""color: red; font-size: 24px; margin-left: 5px;"">{{{{redDot}}}}</span></div>
                                <div class=""layui-card-body"">
                                    接收人:{{{{item.username}}}}<br>
                                    消息内容:{{{{item.content}}}}<br>
                                    <a href=""ReadNotice?notice_id={{{{item.id}}}}"" style=""color:deepskyblue;"">{{{{item.link_text}}}}</a>
                                </div>
                                ",
                    eventItemClick = new CptAttrModelBase.TempItemHtml.EventItemClick
                    {
                        placeText = "点击数据项占位符",
                        paras = new Dictionary<string, string>
                        {
                            {"value","组件名称{{ item.content1 }}"}
                        },
                        callJs = @"parent.page_post.content.set(value.replace(/┕┙/g, '\r\n'));page.close();",
                        
                    },
                };
                
                pageModel.listData = new PageListHtml.ListData
                {
                    func = GetListData,
                    attachPara = new Dictionary<string, object>
                    {
                        {"category_id",req.category_id}
                    },
                };


                return pageModel;
            }
            /// <summary>
            /// 设置列表筛选表单的元素
            /// </summary>
            /// <param name="req"></param>
            /// <returns></returns>
            public CtlListFilter GetListFilter(DtoReq req)
            {
                var listFilter = new CtlListFilter(req);
                listFilter.formItems.Add(new EmtSelect("is_read")
                {
                    options=new Dictionary<string, string>() 
                    {
                        {"未读",ModelDb.sys_notice.is_read_enum.未读.ToSByte().ToString()},
                        {"已读",ModelDb.sys_notice.is_read_enum.已读.ToSByte().ToString()}
                    },
                    width = "60px",
                    placeholder = "未读/已读"
                });

                return listFilter;
            }
            /// <summary>
            /// 请求参数对象
            /// </summary>
            public class DtoReq : PageList.Req
            {
                /// <summary>
                /// 关键词
                /// </summary>
                public string keyword { get; set; }
                public int category_id { get; set; }
            }
            #endregion

            #region 异步请求处理
            /// <summary>
            /// data数据
            /// </summary>
            /// <returns></returns>
            public ModelResult.List GetListData(PageListHtml.ListData.Req reqJson)
            {
                var req = reqJson.GetPara();
                var category_id = req["category_id"].ToInt();
                string where = $"category_id = '{category_id}' and user_sn = '{new UserIdentityBag().user_sn}'";

                if (!req["is_read"].IsNullOrEmpty())
                {
                    where += $" and is_read = '{req["is_read"].IsNullOrEmpty()}'";
                }

                //执行查询
                var filter = new DoMySql.Filter
                {
                    where = where,
                    orderby = "is_read,id desc",
                };
                return new PageListHtml.ListData().getList<ModelDb.sys_notice, ItemDataModel>(filter, reqJson);
            }
            /// <summary>
            /// 数据项模型
            /// </summary>
            public class ItemDataModel : ModelDb.sys_notice
            {
                public string username
                {
                    get
                    {
                        return "主播A";
                    }
                }

                public object content2
                {
                    get
                    {
                        return new
                        {
                            a = "1111aaaa",
                            b = "2"
                        };
                    }
                }
            }
            #endregion
        }
    }
}
