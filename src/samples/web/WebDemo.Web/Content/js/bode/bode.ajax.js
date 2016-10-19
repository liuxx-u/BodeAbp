(function ($) {
    $.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.ajax = function (url, data, func) {
        var loadIndex = layer.load(2, { time: 10 * 1000 });

        var header = {
            timestamp: new Date().getTime(),
            nonce: $.bode.tools.hattedCode()
        };
        //计算签名参数
        header["signature"] = md5(md5(header.timestamp) + md5(header.nonce) + md5(JSON.stringify(data)));

        if ($.bode.auth.isAuth()) {
            header["Authorization"] = "Bearer " + $.bode.auth.getToken();
        }
        $.ajax({
            type: "post",
            url: url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            headers: header,
            success: function (data) {
                layer.close(loadIndex);

                if (data.success) {
                    func(data.result);
                }
                else if (data.unAuthorizedRequest) {
                    //$.bode.auth.clearToken();
                    location.href = "/Admin/Accout/Login";
                }
                else {
                    layer.msg(data.error.message);
                }
            }
        });
    };

    //加载必要的组件
    if (typeof layer === 'undefined') {
        var jspath = "/Content/js/plugs/layer/layer.js";
        $.getScript(jspath).fail(function () { alert("请检查/Content/js/plugs/layer/layer.js的路径是否正确!"); });
    }

    if (typeof md5 === 'undefined') {
        var jspath = "/Content/js/md5.min.js";
        $.getScript(jspath).fail(function () { alert("请检查/Content/js/md5.min.js的路径是否正确!"); });
    }
})(jQuery);