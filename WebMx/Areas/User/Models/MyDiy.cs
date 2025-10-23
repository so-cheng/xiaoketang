using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WeiCode.Utility;
using WeiCode.DataBase;
using WeiCode.Services;

using DataBase.Project;
using Services.Project;
using static Component.Models.ModelCvFuncBasic;

namespace DataBase.Core
{
    public class MyDiy
    {
        /// <summary>
        /// 标签页
        /// </summary>
        public PageModel.Controls controls { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public PageModel.Tree tree { get; set; }


    }


    public class TestHtml
    {
        public string title { get; set; } = "";
        public string title1 { get; set; } = "";
    }
}