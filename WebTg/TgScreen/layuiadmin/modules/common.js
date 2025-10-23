; layui.define(function (e) {
    var i = (layui.$, layui.layer, layui.laytpl, layui.setter, layui.view, layui.admin);
    i.events.logout = function() {
        $.cookie('token', null);
        location.href = "/login";
    }, e("common", {})
});