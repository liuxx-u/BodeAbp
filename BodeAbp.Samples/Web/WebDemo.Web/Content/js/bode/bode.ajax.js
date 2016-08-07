(function ($) {
	$.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
	$.bode.ajax = function (url, data, func) {
		$.ajax({
			type: "post",
			url: url,
			data: JSON.stringify(data),
			contentType: "application/json; charset=utf-8",
			success: function (data) {
				if (data.success) {
					func(data);
				}
				else if (data.unAuthorizedRequest) {
					//身份过期
				}
				else {
					layer.msg("服务器错误...");
				}
			}
		});
	}
})(jQuery);