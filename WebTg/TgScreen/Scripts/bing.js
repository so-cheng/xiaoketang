
/* 搜索 */
var helangSearch = {
    /* 元素集 */
    els: {},
    /* 搜索类型序号 */
    searchIndex: 0,
    /* 火热的搜索列表 */
    hot: {
        /* 颜色 */
        color: ['#ff2c00', '#ff5a00', '#ff8105', '#fd9a15', '#dfad1c', '#6bc211', '#3cc71e', '#3cbe85', '#51b2ef', '#53b0ff', '#ff8105', '#fd9a15', '#dfad1c', '#6bc211', '#3cc71e', '#3cbe85', '#51b2ef',],
        /* 列表 */
        list: [
            '取获公海数据',
            '添加自己的用户',
            '电话销售平台',
            '店铺根据记录查询',
            '更新我的业绩',
            '查看业绩提成及佣金',
            '添加自定义邀约话术',
            '系统异议查询(比喻关于交易、信任等)',
            '系统帮助'
        ],
        link: [
            '/Pool/Index',
            '/Customer/Create',
            '/Customer/MyCustomer',
            '/Customer/List',
            '/Sale/DayReport',
            '/Sale/Rank',
            '/Docu/Create',
            '/Docu/Question',
            '/Sysconfig/Help'
        ]
    },
    /* 初始化 */
    init: function () {
        var _this = this;
        this.els = {
            pickerBtn: $(".picker"),
            pickerList: $(".picker-list"),
            logo: $(".logo"),
            hotList: $(".hot-list"),
            input: $("#search-input"),
            button: $(".search")
        };

        /* 设置热门搜索列表 */
        this.els.hotList.html(function () {
            var str = '';
            $.each(_this.hot.list, function (index, item) {
                str += '<a lay-href="' + _this.hot.link[index] + '">'
                    + '<div class="number" style="color: ' + _this.hot.color[index] + '">' + (index + 1) + '</div>'
                    + '<div>' + item + '</div>'
                    + '</a>';
            });
            return str;
        });

        /* 注册事件 */
        /* 搜索类别选择按钮 */
        this.els.pickerBtn.click(function () {
            if (_this.els.pickerList.is(':hidden')) {
                setTimeout(function () {
                    _this.els.pickerList.show();
                }, 100);
            }
        });
        /* 搜索类别选择列表 */
        this.els.pickerList.on("click", ">li", function () {
            _this.els.logo.css("background-image", ('url(img/' + $(this).data("logo") + ')'));
            _this.searchIndex = $(this).index();
        });
        /* 搜索 输入框 点击*/
        this.els.input.click(function () {
            if (!$(this).val()) {
                setTimeout(function () {
                    _this.els.hotList.show();
                }, 100);
            }
        });
        /* 搜索 输入框 输入*/
        this.els.input.on("input", function () {
            if ($(this).val()) {
                _this.els.hotList.hide();
            }
        });
        /* 搜索按钮 */
        this.els.button.click(function () {
            //alert(_this.els.input.val());
        });
        /* 文档 */
        $(document).click(function () {
            _this.els.pickerList.hide();
            _this.els.hotList.hide();
        });
        /* 搜索按钮 */
    }
};