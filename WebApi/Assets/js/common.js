//弹出iframe页面(标题，网址，宽度，高度，是否垂直居中)
function win_pop_iframe(title, url, area_width, area_height, win_layer) {
    var _area_width = "950";
    var _area_height = "520";
    if (area_width !== undefined) {
        _area_width = area_width;
    }
    if (area_height !== undefined) {
        _area_height = area_height;
    }

    //如果浏览器分辨率小于950，则宽度为屏幕宽度
    if (document.body.clientWidth < 950) _area_width = document.body.clientWidth;

    var _offset_top = $(window).height() - _area_height;
    if (_area_height !== "520" && win_layer === undefined) _offset_top = 520 - _area_height;

    layui.use('layer', function () {
        var layer = layui.layer;
        layer.open({
            type: 2,
            title: title,
            area: [_area_width + "px", _area_height + "px"],
            fix: true,
            shade: [0.5, '#000'],
            offset: [_offset_top / 2 + 'px', ''],
            content: url
        });
    });
}

function openIframe(title, url, area_width, area_height, win_layer) {
    return win_pop_iframe(title, url, area_width, area_height, win_layer);
}

//弹出页面层(标题，元素id，数据url=用于作为模板时读取动态数据，宽度，高度，是否垂直居中)
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
    if (data_url !== undefined) {
        layer.open({
            type: 1,
            title: title,
            area: [_area_width + "px", _area_height + "px"],
            fix: true,
            shade: [0.5, '#000'],
            offset: [_offset_top / 2 + 'px', ''],
            content: module_html
        });
    } else {
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
                            offset: [_offset_top / 2 + 'px', ''],
                            content: module_html
                        });
                    });
                });
            }
        });
    }
}

function openLayer(title, module_id, data_url, area_width, area_height, win_layer) {
    return win_pop_layer(title, module_id, data_url, area_width, area_height, win_layer);
}

//提示框(刷新父框架),msg=提示信息；type=类型（0=成功；1=失败）；url=跳转地址（为空则为刷新）
function layerMsg_P(msg, type, url) {
    var mType = "1";
    if (type === 1) mType = "5";
    if (type === 0) mType = "6";
    layui.use('layer', function () {
        var layer = layui.layer;
        if (type === 1) {
            layer.msg(msg, { icon: mType });
        } else {
            layer.msg(msg, { icon: mType, time: 1000 }, function () {
                if (url) {
                    location.href = url;
                } else {
                    parent.location.reload();
                }     
            });
        }
    });
}

//提示框(刷新本页面)
function layerMsg_R(msg, type, url) {
    var mType = "1";
    if (type === 1) mType = "5";
    if (type === 0) mType = "6";
    layui.use('layer', function () {
        var layer = layui.layer;
        if (type === 1) {
            layer.msg(msg, { icon: mType });
        } else {
            layer.msg(msg, { icon: mType, time: 1000 }, function () {
                if (url) {
                    location.href = url;
                } else {
                    location.reload();
                }                
            });
        }
    });
}

//提示框(跳转到指定地址)
function layerMsg(msg, type, url) {
    var mType = "1";
    if (type === 1) mType = "5";    //失败的图标
    if (type === 0) mType = "6";    //成功的图标
    layui.use('layer', function () {
        var layer = layui.layer;
        switch (type) {
            case 1:
                layer.msg(msg, { icon: mType });
                break;
            case 0:
                if (url === undefined) {
                    layer.msg(msg, { icon: mType });
                    break;
                } else {
                    layer.msg(msg, { icon: mType, time: 500 }, function () {
                        location.href = url;
                    });
                }                
                break;
        }
    });
}

//提示框(跳转网址,POST数据，标题)
function layDel(url, data, title, callback) {
    layui.use('layer', function () {
        var layer = layui.layer;
        layer.confirm('是否确认' + title + '？', { icon: 3, title: title }, function (index) {
            layer.close(index);
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                dataType: "json",
                beforeSend: function () {
                    // 禁用按钮防止重复提交
                    $(".layui-layer-btn0").addClass("layui-disabled");
                    $(".layui-layer-btn0").css("pointer-events", "none").prop("disabled", true);
                },
                async: false,
                success: function (data) {
                    $(".layui-layer-btn0").removeClass("layui-disabled");
                    $(".layui-layer-btn0").css("pointer-events", "auto").prop("disabled", false);
					
					if (typeof callback === "function") {
						callback();
					}else{
						layerMsg_R(data.msg, data.code);
					}                    
                }
            });
        });
    });
}

function actionConfirm(url, data, title, callback) {
    return layDel(url, data, title, callback);
}

//form表单提交form-submit
$(function () {
    $(".btn-submit").attr("lay-submit", "");
    $(".btn-submit").attr("lay-filter", "*");
    $(".btn-submit").attr("type", "button");
});

layui.use(['form', 'layer'], function () {
    var form = layui.form;
    form.on('submit(*)', function (data) {
        var that = $(this);
        var form_id = that.attr("form-id");
        if (form_id === "" || form_id === undefined) form_id = "layui-form";

        var url = $("." + form_id).attr("action");
        if (url === "" || url === undefined) url = this.url;

        var return_reload = that.attr("return-reload");
        if (return_reload === undefined) return_reload = "";
        var return_url = that.attr("return-url");
        if (return_url === undefined) return_url = "";

        var confirm_text = that.attr("confirm-text");
        if (confirm_text !== undefined) {
            var layer = layui.layer;
            layer.confirm(confirm_text, function (index) {
                layer.close(index);
                commonAjax(that, form_id, url, return_url, return_reload);
            });
        } else {
            commonAjax(that, form_id, url, return_url, return_reload);
        }
        return false;
    });
});

function commonAjax(that, form_id, url, return_url, return_reload) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify($("." + form_id).serializeObjComplex()),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            // 禁用按钮防止重复提交
            that.addClass("layui-btn-disabled");
            that.attr({ disabled: "disabled" });
        },
        success: function (data) {
            if (data.code === 1) {
                that.removeClass("layui-btn-disabled");
                that.removeAttr("disabled");
            }
            switch (return_reload) {
                case "parent":
                    if (return_url === "") {
                        layerMsg_P(data.msg, data.code);
                    } else {
                        layerMsg_P(data.msg, data.code, return_url);
                    }
                    break;
                default:
                    if (return_reload !== "") {
                        layerMsg_R(data.msg, data.code, return_reload);
                    } else {
                        if (return_url === "") {
                            layerMsg_R(data.msg, data.code);
                        } else {
                            layerMsg_R(data.msg, data.code, return_url);
                        }
                    }
                    //alert("return-reload属性值设置有误"); 
                    break;
            }
        }
    });
}

//图片预览
$(".agpt-img-view").each(function () {
    $(this).click(function () {
        var src = $(this).attr("src");
        var w = $(this)[0].naturalWidth < parent.innerWidth - 50 ? $(this)[0].naturalWidth : parent.innerWidth - 50;
        var h = $(this)[0].naturalHeight < parent.innerHeight - 50 ? $(this)[0].naturalHeight : parent.innerHeight - 50;
        //页面层
        layer.open({
            type: 1,
            title: false,
            area: [w + 'px', h + 'px'], //宽高
            content: '<div align="center"><img style="height:auto; max-width:100%; display:block;" src="' + src + '"/></div>'
        });
    });
    $(this).mouseover(function () {
        layer.tips('查看原图', $(this), {
            time: 1000
        });
    });
});

//自定义数据列表
function ThisPageList() {
    this.ajax_url = "/Home/IndexListData";
    this.search_form = "search_form";
    this.list_container = "list_container";
    this.list_pager = "list_pager";
    this.form_page = "form_page";
    this.form_pageSize = "form_pageSize";
    this.event_page = false;

    this.init = function () {
        var that = this;
        layui.use(['laytpl', 'laypage'], function () {
            var laytpl = layui.laytpl;
            var laypage = layui.laypage;
            $.ajax({
                type: "POST",
                url: that.ajax_url,
                data: $("#" + that.search_form).serializeArray(),
                dataType: "json",
                async: false,
                success: function (res) {
                    var getTpl = list_tpl.innerHTML
                        , view = document.getElementById(that.list_container);
                    laytpl(getTpl).render(res.data, function (html) {
                        view.innerHTML = html;
                    });

                    //完整功能
                    if (!that.event_page) {
                        laypage.render({
                            elem: that.list_pager
                            , count: res.count
                            , limit: $("#" + that.form_pageSize).val()
                            , layout: ['count', 'prev', 'page', 'next', 'limit', 'refresh', 'skip']
                            , jump: function (obj) {
                                if (that.event_page) {
                                    $("#" + that.form_page).val(obj.curr);
                                    $("#" + that.form_pageSize).val(obj.limit);
                                    that.init();
                                }
                            }
                        });
                        that.event_page = true;
                    }
                }
            });
        });
    };
}

$.fn.serializeObjComplex = function () {
    var o = {};
    var form_data = this.serializeArray();
    $.each(form_data, function () {
        var this_names = this.name.split(".");
        for (var i = 0; i < this_names.length; i++) {
            var this_name_key = "";
            var this_name_value = "";
            for (var j = 0; j < this_names.length; j++) {
                eval("if(!o." + this_name_key + this_names[j] + ")o." + this_name_key + this_names[j] + " = {}");
                if (j < this_names.length - 1) this_name_key += this_names[j] + ".";
                this_name_value += this_names[j] + ".";
            }
            this_name_value = this_name_value.substring(0, this_name_value.length - 1);
            if (i === this_names.length - 1) {
                var tmp = eval("o." + this_name_value);
                var this_value = this.value.replace(/[\n\r]/g, '');

                if (JSON.stringify(tmp) === "{}") {
                    eval("o." + this_name_value + " = '" + this_value + "'");
                } else {
                    if (eval("o." + this_name_value) instanceof Array === false) {
                        var tmp_value = eval("o." + this_name_value);
                        eval("o." + this_name_value + " = new Array();");
                        eval("o." + this_name_value + ".push('" + tmp_value + "')");
                    }
                    eval("o." + this_name_value + ".push('" + this_value + "')");
                }
            }
        }
    });
	//将l_名下对象重新组织	
	o = reStructureObject(o, "", o);
    return o;
};

function reStructureObject(obj, names, o){	
	for(let key in obj){
		names += "." + key;		
		if(key.indexOf("l_") == 0){
			var tmp = [];
			var obj_key = obj[key];			
			
			for(var len = 0; len < obj_key[Object.keys(obj_key)[0]].length; len++){
				console.log(len)
				var _obj = {};
				for(let _key in obj_key){
					_obj[_key] = obj_key[_key][len];
				}
				tmp.push(_obj);
			}
			
			console.log(tmp)
			eval("o" + names + " = tmp;");
		}
		//console.log(obj[key] + "==" + key.indexOf("l_"))
		if (typeof (obj[key]) === "object") {
			o = reStructureObject(obj[key], names, o);
		}else{
			names = names.substr(0, names.lastIndexOf("."))
		}
	}	
	return o;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

//移动端自动适配
if (!IsPC) $(".layui-table").width(1300);

function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
        "SymbianOS", "Windows Phone",
        "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}

