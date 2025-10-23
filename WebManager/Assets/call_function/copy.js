


// 复制文本
function copy_text(content) {
    copyText(content, function () {
        layer.msg("复制成功", { icon: "6" });
    });
}

// 复制的方法
function copyText(text, callback) { // text: 要复制的内容， callback: 回调
    var tag = document.createElement('input');
    tag.setAttribute('id', 'cp_hgz_input');
    tag.value = text;
    document.getElementsByTagName('body')[0].appendChild(tag);
    document.getElementById('cp_hgz_input').select();
    document.execCommand('copy');
    document.getElementById('cp_hgz_input').remove();
    if (callback) { callback(text) }
}