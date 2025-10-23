$(window).load(function () {
    $(".loading").fadeOut()
})
$(function () {

    test();

    var show = 0;

    var last_real = 0;

    wrap();

  


    function month_left() {
        var date = new Date;
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var month_ = new Date(parseInt(year), parseInt(month), 0);
        var month_count = month_.getDate();
        var current_count = new Date().getDate();
        var i = parseInt(month_count) - parseInt(current_count);
        return i;
    }

    function month_complete_per() {
        var date = new Date;
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var month_ = new Date(parseInt(year), parseInt(month), 0);
        var month_count = month_.getDate();
        var current_count = new Date().getDate();
        return (parseInt(current_count) * 100.0 / parseInt(month_count)).toFixed(2);
    }


    function test() {
        $.ajax(
            {
                type: "POST",
                dataType: "json",
                url: "/BizScreen/Guest/GetLargeData?t_month=" + t_month,
                success: function (result) {
                    var left = month_left();
                    //汇总
                    $("#plan_amount_t").html(Thousandformat(result.data.plan_amount_t));
                    $("#real_amount_t").html(Thousandformat(result.data.real_amount_t));
                    $("#real_amount_d").html(Thousandformat(result.data.real_amount_d));
                    /*
                    if (last_real != result.data.real_amount_d) {
                        $('#real_amount_d').lemCounter({
                            value_to: Thousandformat(result.data.real_amount_d),
                            value_from: last_real
                        });
                        last_real = result.data.real_amount_d;
                    }
                    */
                    zb2(left);
                    zb3(result.data.real_per_t);
                    zb4(month_complete_per());

                    //小组进度
                    var dep = [];
                    var dep_plan = [];
                    var dep_real = [];
                    var dep_plan_per = [];
                    var dep_real_per = [];
                    var dep_plan_share = [];
                    var dep_real_share = [];


                    $.each(result.Data.LdDepList,
                        function (index, item) {
                            dep.push(item.departmentTitle);

                            dep_plan_per.push(100);
                            dep_real_per.push(parseInt(100 * item.real_amount / item.plan_amout));

                            dep_plan.push(item.plan_amout);
                            dep_real.push(item.real_amount);

                            dep_plan_share.push({ value: item.plan_amout, name: item.departmentTitle });
                            dep_real_share.push({ value: item.real_amount, name: item.departmentTitle });
                        });

                    echarts_1(dep_plan_share);
                    echarts_2(dep_real_share);
                    echarts_3(dep, dep_plan, dep_real);

                    echarts_5(dep.reverse(), dep_plan_per.reverse(), dep_real_per.reverse());

                    //实时业绩
                    var dyn_ul_html = "";
                    $("#dyn_ul").html(dyn_ul_html);
                    $.each(result.Data.LdDynSaleList,
                        function (index, item) {
                            if (index < 3) {
                                $("#m" + index).html(item.nickname + "(" + Thousandformat(item.sale) + ")");
                                $("#im" + index).attr('src', item.photo);
                                if ("http://b.crm.91jingyun.com" == item.photo) {
                                    $("#im" + index).attr('src', 'http://b.crm.91jingyun.com/Upload/photo/default.jpg');
                                }
                            }
                            dyn_ul_html +=
                                "<li><p><span>" +
                                item.departmentTitle +
                                "</span><span>" +
                                item.nickname +
                                "</span><span>" +
                                Thousandformat(item.sale) +
                                "</span><span>" +
                                item.sale_at +
                                "</span></p></li>";
                        });
                    $("#dyn_ul").html(dyn_ul_html);
                    if (show == 0) {
                        wrap();
                        show = 1;
                    }

                    //年度业绩
                    var year_month = [];
                    var year_plan = [];
                    var year_real = [];
                    var year_per = [];
                    $.each(result.Data.LdYearList,
                        function (index, item) {
                            year_month.push(item.month);
                            year_plan.push(item.plan);
                            year_real.push(item.real);
                            year_per.push(item.per);
                        });
                    echarts_4(year_month, year_plan, year_real, year_per);

                    //精英
                    var member_rank = "";
                    var style = "style=\"color: white\"";
                    member_rank +=
                        " <tr><th scope=\"col\">排名</th><th scope=\"col\">部门</th><th scope=\"col\">精英</th><th scope=\"col\">销售额</th></tr>";
                    $.each(result.Data.LdEmplyeeList,
                        function (index, item) {
                            var rankid = parseInt(index) + 1;
                            if (index < 6) {
                                if (index > 2) style = "";
                                member_rank +=
                                    "<tr><td><span>" +
                                    rankid +
                                    "</span></td><td " + style + ">" +
                                    item.departmentTitle +
                                    "</td><td " + style + ">" +
                                    item.nickname +
                                    "<br></td><td " + style + ">" + Thousandformat(item.sale_t) + "<br></td></tr>";
                            }
                        });
                    $("#dyn_tr").html(member_rank);

                    //团队
                    var team_rank = "";
                    style = "style=\"color: white\"";
                    team_rank +=
                        "<tr><th scope=\"col\">排名</th><th scope=\"col\">部门</th><th scope=\"col\">目标</th><th scope=\"col\">销售</th></tr>";
                    $.each(result.Data.LdDepSaleList,
                        function (index, item) {
                            var teamid = parseInt(index) + 1;
                            if (index < 10) {
                                if (index > 2) style = "";
                                team_rank +=
                                    "<tr><td><span>" +
                                    teamid +
                                    "</span></td><td  " + style + ">" +
                                    item.departmentTitle +
                                    "</td><td " + style + ">" +
                                    Thousandformat(item.plan_amout) +
                                    "</td><td " + style + ">" +
                                    Thousandformat(item.real_amount) +
                                    "</td></tr>";
                            }
                        });
                    $("#dyn_tr0").html(team_rank);
                },
                error: function (e) {
                    console.log(e.status);
                    console.log(e.responseText);
                }
            });
    }


    function wrap() {
        $('.wrap,.adduser').liMarquee({
            direction: 'up',/*身上滚动*/
            runshort: false,/*内容不足时不滚动*/
            scrollamount: 20/*速度*/
        });
    }


    //计划 ok
    function echarts_1(dep_plan_share) {
        // 基于准备好的dom，初始化echarts实例->计划销售占比
        var myChart = echarts.init(document.getElementById('echart1'));
        option = {
            tooltip: {
                trigger: 'item',
                formatter: "{b} : {c} ({d}%)"
            },
            calculable: true,
            series: [
                {
                    name: ' ',
                    color: ['#62c98d', '#2f89cf', '#4cb9cf', '#53b666', '#62c98d', '#205acf', '#c9c862', '#c98b62', '#c962b9', '#7562c9', '#c96262', '#c25775', '#00b7be'],
                    type: 'pie',
                    radius: [30, 70],
                    center: ['50%', '50%'],
                    roseType: 'radius',
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },

                    lableLine: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },

                    data: dep_plan_share
                },
            ]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //实际 ok
    function echarts_2(dep_real_share) {
        // 基于准备好的dom，初始化echarts实例->实际销售占比
        var myChart = echarts.init(document.getElementById('echart2'));

        option = {
            tooltip: {
                trigger: 'item',
                formatter: "{b} : {c} ({d}%)"
            },
            calculable: true,
            series: [{
                name: '',
                color: ['#62c98d', '#2f89cf', '#4cb9cf', '#53b666', '#62c98d', '#205acf', '#c9c862', '#c98b62', '#c962b9', '#c96262'],
                type: 'pie',
                //起始角度，支持范围[0, 360]
                startAngle: 0,
                //饼图的半径，数组的第一项是内半径，第二项是外半径
                radius: [51, 100],
                //支持设置成百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度
                center: ['50%', '45%'],

                //是否展示成南丁格尔图，通过半径区分数据大小。可选择两种模式：
                // 'radius' 面积展现数据的百分比，半径展现数据的大小。
                //  'area' 所有扇区面积相同，仅通过半径展现数据大小
                roseType: 'area',
                //是否启用防止标签重叠策略，默认开启，圆环图这个例子中需要强制所有标签放在中心位置，可以将该值设为 false。
                avoidLabelOverlap: false,
                label: {
                    normal: {
                        show: true,
                        //  formatter: '{c}辆'
                    },
                    emphasis: {
                        show: true
                    }
                },
                labelLine: {
                    normal: {
                        show: true,
                        length2: 1,
                    },
                    emphasis: {
                        show: true
                    }
                },
                data: dep_real_share
            }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //计划实际
    function echarts_3(dep, dep_plan, dep_real) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('echart3'));

        option = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    lineStyle: {
                        color: '#57617B'
                    }
                }
            },
            legend: {

                //icon: 'vertical',
                data: ['计划', '实际'],
                //align: 'center',
                // right: '35%',
                top: '0',
                textStyle: {
                    color: "#fff"
                },
                // itemWidth: 15,
                // itemHeight: 15,
                itemGap: 20,
            },
            grid: {
                left: '0',
                right: '20',
                top: '10',
                bottom: '20',
                containLabel: true
            },
            xAxis: [{
                type: 'category',
                boundaryGap: false,
                axisLabel: {
                    show: true,
                    textStyle: {
                        color: 'rgba(255,255,255,.6)'
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: 'rgba(255,255,255,.1)'
                    }
                },
                data: dep
            }, {




            }],
            yAxis: [{
                axisLabel: {
                    show: true,
                    textStyle: {
                        color: 'rgba(255,255,255,.6)'
                    }
                },
                axisLine: {
                    lineStyle: {
                        color: 'rgba(255,255,255,.1)'
                    }
                },
                splitLine: {
                    lineStyle: {
                        color: 'rgba(255,255,255,.1)'
                    }
                }
            }],
            series: [{
                name: '计划',
                type: 'line',
                smooth: true,
                symbol: 'circle',
                symbolSize: 5,
                showSymbol: false,
                lineStyle: {
                    normal: {
                        width: 2
                    }
                },
                areaStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgba(24, 163, 64, 0.3)'
                        }, {
                            offset: 0.8,
                            color: 'rgba(24, 163, 64, 0)'
                        }], false),
                        shadowColor: 'rgba(0, 0, 0, 0.1)',
                        shadowBlur: 10
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#cdba00',
                        borderColor: 'rgba(137,189,2,0.27)',
                        borderWidth: 12
                    }
                },
                data: dep_plan
            }, {
                name: '实际',
                type: 'line',
                smooth: true,
                symbol: 'circle',
                symbolSize: 5,
                showSymbol: false,
                lineStyle: {
                    normal: {
                        width: 2
                    }
                },
                areaStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgba(39, 122,206, 0.3)'
                        }, {
                            offset: 0.8,
                            color: 'rgba(39, 122,206, 0)'
                        }], false),
                        shadowColor: 'rgba(0, 0, 0, 0.1)',
                        shadowBlur: 10
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#277ace',
                        borderColor: 'rgba(0,136,212,0.2)',
                        borderWidth: 12
                    }
                },
                data: dep_real
            }]
        };

        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //年度销售 ok
    function echarts_4(year_month, year_plan, year_real, year_per) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('echart4'));
        option = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    lineStyle: {
                        color: '#57617B'
                    }
                }
            },
            "legend": {

                "data": [
                    { "name": "计划" },
                    { "name": "实际" },
                    { "name": "完成率" }
                ],
                "top": "0%",
                "textStyle": {
                    "color": "rgba(255,255,255,0.9)"//图例文字
                }
            },

            "xAxis": [
                {
                    "type": "category",

                    data: year_month,
                    axisLine: { lineStyle: { color: "rgba(255,255,255,.1)" } },
                    axisLabel: {
                        textStyle: { color: "rgba(255,255,255,.6)", fontSize: '14', },
                    },

                },
            ],
            "yAxis": [
                {
                    "type": "value",
                    "name": "金额",
                    "min": 0,
                    "max": 3000000,
                    "interval": 10,
                    "axisLabel": {
                        "show": true,

                    },
                    axisLine: { lineStyle: { color: 'rgba(255,255,255,.4)' } },//左线色

                },
                {
                    "type": "value",
                    "name": "完成率",
                    "show": true,
                    "axisLabel": {
                        "show": true,

                    },
                    axisLine: { lineStyle: { color: 'rgba(255,255,255,.4)' } },//右线色
                    splitLine: { show: true, lineStyle: { color: "#001e94" } },//x轴线
                },
            ],
            "grid": {
                "top": "10%",
                "right": "30",
                "bottom": "30",
                "left": "30",
            },
            "series": [
                {
                    "name": "计划",

                    "type": "bar",
                    "data": year_plan,
                    "barWidth": "auto",
                    "itemStyle": {
                        "normal": {
                            "color": {
                                "type": "linear",
                                "x": 0,
                                "y": 0,
                                "x2": 0,
                                "y2": 1,
                                "colorStops": [
                                    {
                                        "offset": 0,
                                        "color": "#609db8"
                                    },

                                    {
                                        "offset": 1,
                                        "color": "#609db8"
                                    }
                                ],
                                "globalCoord": false
                            }
                        }
                    }
                },
                {
                    "name": "实际",
                    "type": "bar",
                    "data": year_real,
                    "barWidth": "auto",

                    "itemStyle": {
                        "normal": {
                            "color": {
                                "type": "linear",
                                "x": 0,
                                "y": 0,
                                "x2": 0,
                                "y2": 1,
                                "colorStops": [
                                    {
                                        "offset": 0,
                                        "color": "#66b8a7"
                                    },
                                    {
                                        "offset": 1,
                                        "color": "#66b8a7"
                                    }
                                ],
                                "globalCoord": false
                            }
                        }
                    },
                    "barGap": "0"
                },
                {
                    "name": "完成率",
                    "type": "line",
                    "yAxisIndex": 1,

                    "data": year_per,
                    lineStyle: {
                        normal: {
                            width: 2
                        },
                    },
                    "itemStyle": {
                        "normal": {
                            "color": "#cdba00",

                        }
                    },
                    "smooth": true
                }
            ]
        };


        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //ok
    function echarts_5(dep, dep_plan, dep_real) {

        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('echart5'));
        // 颜色
        var lightBlue = {
            type: 'linear',
            x: 0,
            y: 0,
            x2: 0,
            y2: 1,
            colorStops: [{
                offset: 0,
                color: 'rgba(41, 121, 255, 1)'
            }, {
                offset: 1,
                color: 'rgba(0, 192, 255, 1)'
            }],
            globalCoord: false
        }

        var option = {
            tooltip: {
                show: false
            },
            grid: {
                top: '0%',
                left: '65',
                right: '20%',
                bottom: '0%',
            },
            xAxis: {
                min: 0,
                max: 100,
                splitLine: {
                    show: false
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: false
                },
                axisLabel: {
                    show: false
                }
            },
            yAxis: {
                data: dep,
                //offset: 15,
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: false
                },
                axisLabel: {
                    color: 'rgba(255,255,255,.6)',
                    fontSize: 14
                }
            },
            series: [{
                type: 'bar',
                label: {
                    show: true,
                    zlevel: 10000,
                    position: 'right',
                    padding: 10,
                    color: '#49bcf7',
                    fontSize: 14,
                    formatter: '{c}%'

                },
                itemStyle: {
                    color: '#49bcf7'
                },
                barWidth: '15',
                data: dep_real,
                z: 10
            }, {
                type: 'bar',
                barGap: '-100%',
                itemStyle: {
                    color: '#fff',
                    opacity: 0.1
                },
                barWidth: '15',
                data: dep_plan,
                z: 5
            }],
        };
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //部门
    function zb1(department_count) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('zb1'));
        var v1 = 0 //实际
        var v2 = department_count //计划
        var v3 = v1 + v2//总消费 
        option = {
            series: [{

                type: 'pie',
                radius: ['60%', '70%'],
                color: '#49bcf7',
                label: {
                    normal: {
                        position: 'center'
                    }
                },
                data: [{
                    value: v2,
                    name: '',
                    label: {
                        normal: {
                            formatter: v2 + '',
                            textStyle: {
                                fontSize: 20,
                                color: '#fff',
                            }
                        }
                    }
                }, {
                    value: v1,
                    name: '',
                    label: {
                        normal: {
                            formatter: function (params) {
                                return ''
                            },
                            textStyle: {
                                color: '#aaa',
                                fontSize: 6
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(255,255,255,.2)'
                        },
                        emphasis: {
                            color: '#fff'
                        }
                    },
                }]
            }]
        };
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //职员
    function zb2(emplyee_count) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('zb2'));
        var v1 = emplyee_count
        var v2 = 0
        var v3 = v1 + v2//总消费 
        option = {

            //animation: false,
            series: [{
                type: 'pie',
                radius: ['60%', '70%'],
                color: '#cdba00',
                label: {
                    normal: {
                        position: 'center'
                    }
                },
                data: [{
                    value: v1,
                    name: '',
                    label: {
                        normal: {
                            formatter: v1 + '',
                            textStyle: {
                                fontSize: 20,
                                color: '#fff',
                            }
                        }
                    }
                }, {
                    value: v2,
                    name: '',
                    label: {
                        normal: {
                            formatter: function (params) {
                                return ''
                            },
                            textStyle: {
                                color: '#aaa',
                                fontSize: 12
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(255,255,255,.2)'
                        },
                        emphasis: {
                            color: '#fff'
                        }
                    },
                }]
            }]
        };
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    //进度
    function zb3(real_per_t) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('zb3'));
        var v1 = 100 - parseInt(real_per_t)
        var v2 = real_per_t
        var v3 = v1 + v2//总消费 
        option = {
            series: [{
                type: 'pie',
                radius: ['60%', '70%'],
                color: '#62c98d',
                label: {
                    normal: {
                        position: 'center'
                    }
                },
                data: [{
                    value: v2,
                    name: '',
                    label: {
                        normal: {
                            formatter: v2 + '%',
                            textStyle: {
                                fontSize: 20,
                                color: '#fff',
                            }
                        }
                    }
                }, {
                    value: v1,
                    name: '',
                    label: {
                        normal: {
                            formatter: function (params) {
                                return ''
                            },
                            textStyle: {
                                color: '#aaa',
                                fontSize: 12
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(255,255,255,.2)'
                        },
                        emphasis: {
                            color: '#fff'
                        }
                    },
                }]
            }]
        };
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }

    function zb4(left) {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('zb4'));
        var v1 = 100 - parseInt(left)
        var v2 = left
        var v3 = v1 + v2//总消费 
        option = {
            series: [{
                type: 'pie',
                radius: ['60%', '70%'],
                color: '#62c98d',
                label: {
                    normal: {
                        position: 'center'
                    }
                },
                data: [{
                    value: v2,
                    name: '',
                    label: {
                        normal: {
                            formatter: v2 + '',
                            textStyle: {
                                fontSize: 20,
                                color: '#fff',
                            }
                        }
                    }
                }, {
                    value: v1,
                    name: '',
                    label: {
                        normal: {
                            formatter: function (params) {
                                return ''
                            },
                            textStyle: {
                                color: '#aaa',
                                fontSize: 12
                            }
                        }
                    },
                    itemStyle: {
                        normal: {
                            color: 'rgba(255,255,255,.2)'
                        },
                        emphasis: {
                            color: '#fff'
                        }
                    },
                }]
            }]
        };
        myChart.setOption(option);
        window.addEventListener("resize", function () {
            myChart.resize();
        });
    }


    //function fetchData() {
    //    $.ajax({
    //        url: "/Sysconfig/FetchSysNote",
    //        type: 'post',
    //        dataType: "json",
    //        success: function (data) {
    //            if (data.Status == "success") {
    //                showImg(data.Data.photo, data.Data.nickname, data.Data.money);
    //            }
    //        }
    //    });
    //}

    //function fetch_note(nickname, img, mesg, money) {
    //    var content =
    //        '<div class=\"layadmin-homepage-panel layadmin-homepage-shadow\" style=\"opacity: 0.9;\" ><div class=\"layui-card text-center\"><div class=\"layui-card-body\"><div class=\"layadmin-homepage-pad-ver\"><img class=\"layadmin-homepage-pad-img\" src=\"' +
    //        img +
    //        '\" width=\"96\" height=\"96\"></div><h4 class=\"layadmin-homepage-font\">' +
    //        nickname +
    //        '</h4><p class=\"layadmin-homepage-min-font\" style=\"color:#393D49\">' +
    //        mesg +
    //        '</p></div></div></div><h1 style=\"float:right;font-weight:blood;color: #ed405d;\">' +
    //        money +
    //        '</h1>';
    //    layui.layer.open({
    //        type: 1,
    //        anim: 6,
    //        title: false,
    //        offset: 'auto',
    //        time: 5200,
    //        closeBtn: false,
    //        area: ['400px', '280px'],
    //        content: content,
    //        success: function (layero, index) {
    //            var msg = new SpeechSynthesisUtterance(mesg);

    //            msg.rate = 1.2;

    //            window.speechSynthesis.speak(msg);
    //        },
    //        yes: function () {
    //            layer.closeAll();
    //        }
    //    });
    //}



})

















