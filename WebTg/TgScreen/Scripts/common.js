function win_pop_iframe(title, url, area_width, area_height, win_layer) {
    var _area_width = "950";
    var _area_height = "520";
    if (area_width !== undefined) {
        _area_width = area_width;
    }
    if (area_height !== undefined) {
        _area_height = area_height;
    }

    var _offset_top = $(window).height() - _area_height;
    if (_area_height !== "520" && win_layer === undefined) {
        _offset_top = 520 - _area_height;
    }

    layui.use('layer', function () {
        var layer = layui.layer;
        layer.open({
            type: 2,
            title: title,
            area: [_area_width + "px", _area_height + "px"],
            fix: true,
            shade: [0.5, '#000'],
            offset: ['100px', ''],
            content: url
        });
    });
}

//弹出页面层(标题，元素id，数据url，宽度，高度，是否垂直居中)
function win_pop_layer(title, module_id, data_url, area_width, area_height, win_layer) {
    var _area_width = "950";
    var _area_height = "520";
    if (area_width !== undefined) {
        _area_width = area_width;
    }
    if (area_height !== undefined) {
        _area_height = area_height;
    }

    var _offset_top = $(window).height() - _area_height;
    if (_area_height !== "520" && win_layer === undefined) {
        _offset_top = 520 - _area_height;
    }

    var module_html = $("#" + module_id).html();
    $.ajax({
        type: "POST",
        url: data_url,
        data: "",
        dataType: "json",
        async: false,
        success: function (data) {
            layui.use(['laytpl', 'layer'], function () {
                var laytpl = layui.laytpl;
                //直接解析字符
                laytpl(module_html).render({
                    data: data,
                    module_id: module_id
                }, function (string) {
                    module_html = string;
                    var layer = layui.layer;
                    layer.open({
                        type: 1,
                        title: title,
                        area: [_area_width + "px", _area_height + "px"],
                        fix: true,
                        shade: [0.5, '#000'],
                        offset: 'auto',
                        content: module_html
                    });
                });
            }); 
        }
    });
}

function layerMsg_P(msg, type) {
    var mType = "1";
    if (type == "1") mType = "5";
    if (type == "0") mType = "6";
    layui.use('layer', function () {
        var layer = layui.layer;
        if (type == "1") {
            layer.msg(msg, { icon: mType });
        } else {
            layer.msg(msg, { icon: mType, time: 1000 }, function () {
                parent.location.reload();
            });
        }
    });
}

function layerMsg_R(msg, type) {
    var mType = "1";
    if (type == "1") mType = "5";
    if (type == "0") mType = "6";
    layui.use('layer', function () {
        var layer = layui.layer;
        if (type == "1") {
            layer.msg(msg, { icon: mType });
        } else {
            layer.msg(msg, { icon: mType, time: 1000 }, function () {
                location.reload();
            });
        }
    });
}

function layerMsg(msg, type, url) {
    var mType = "1";
    if (type == "1") mType = "5";
    if (type == "0") mType = "6";
    layui.use('layer', function () {
        var layer = layui.layer;
        if (type == "1") {
            layer.msg(msg, { icon: mType });
        } else {
            layer.msg(msg, { icon: mType, time: 500 }, function () {
                location.href = url;
            });
        }
    });
}

function layDel(url, data, title) {
    layui.use('layer', function () {
        var layer = layui.layer;
        layer.confirm('是否确认' + title + '？', { icon: 3, title: title }, function (index) {
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                dataType: "json",
                async: false,
                success: function (data) {
                    layerMsg_R(data.msg, data.code);
                }
            });
        });
    });
}

function returnTip(data) {
    layerMsg_P(data.msg, data.code);
}

//form表单提交form-submit  
$(document).delegate(".btn-submit", "click", function () {
    $.ajax({
        type: "POST",
        url: $("." + $(this).attr("form-id")).attr("action"),
        data: $("." + $(this).attr("form-id")).serialize(),
        dataType: "json",
        success: function (data) {
            layerMsg_P(data.message, data.state);
            if (data.state == 0) {
                setTimeout("location.reload();", 1000);
            }
        }
    });
});