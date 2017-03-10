(function ($) {
    $.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.ajax = function (url, data, func) {
        var header = {
            timestamp: new Date().getTime(),
            nonce: $.bode.tools.hattedCode()
        };
        //计算签名参数
        header["signature"] = md5(md5(header.timestamp) + md5(header.nonce) + md5(JSON.stringify(data)));

        abp.ajax({
            type: "post",
            url: url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            headers: header,
            success: function (data) {
                func(data);
            }
        });
    };
})(jQuery);