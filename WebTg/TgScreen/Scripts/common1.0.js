function ChangeDateFormat(val) {
    if (val != null) {
        var ret = "";
        var date = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
        if (!(date.getFullYear() < 1000)) {
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minu = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            ret = date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minu;
        }
        return ret;
    }
    return "";
}

function getParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

function addQQ(qq) {
    // window.open("tencent://message/?uin=" + qq +"&Site=&Menu=yes");
    window.open("https://wpa.qq.com/msgrd?v=3&uin=" + qq + "&site=qq&menu=yes");
}


function tip(data) {
    layer.msg(data);
}

function sale_platform() {
    top.layui.index.openTabsPage("/Customer/MyCustomer", "电话销售平台");
}

function sale_dayReport() {
    top.layui.index.openTabsPage("/Sale/DayReport", "销售日报提交");
}

function question() {
    top.layui.index.openTabsPage("/Docu/Question", "异议处理");
}

function help() {
    top.layui.index.openTabsPage("/Docu/Index", "文档查询");
}

function fetch() {
    top.layui.index.openTabsPage("/Pool/Index", "公海客户池");
}

Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function formatDate(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? '0' + m : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    return y + '-' + m + '-' + d;
};  


//千分显示数值，正则方式
function Thousandformat(num) {
    var reg = /\d{1,3}(?=(\d{3})+$)/g;
    return (num + '').replace(reg, '$&,');
}

//百分比
function GetPercent(num, total) {
    num = parseFloat(num);
    total = parseFloat(total);
    if (isNaN(num) || isNaN(total)) {
        return "-";
    }
    return total <= 0 ? "0%" : (Math.round(num / total * 10000) / 100.00) + "%";
}

//随机数
function random(lower, upper) {
    return Math.floor(Math.random() * (upper - lower)) + lower;
}

