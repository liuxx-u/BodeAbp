(function ($) {
	$.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.ajax = function (url, data, func) {
        var loadIndex = layer.load(2, { time: 10 * 1000 });

        var header = {};
        if ($.bode.auth.isAuth()) {
            header["Authorization"] = "Bearer " + $.bode.auth.getToken();
        }
        $.ajax({
            type: "post",
            url: url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            header: header,
            success: function (data) {
                layer.close(loadIndex);

                if (data.success) {
                    func(data.result);
                }
                else if (data.unAuthorizedRequest) {
                    $.bode.auth.clearToken();
                    location.href = "/Admin/Accout/Login";
                }
                else {
                    layer.msg(data.error.message);
                }
            }
        });
	}
})(jQuery);