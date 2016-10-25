(function ($) {
    $.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.tools = {
        url: {
            encode: function (url) {
                return encodeURIComponent(url);
            },
            decode: function (url) {
                return decodeURIComponent(url);
            }
        },
        input: {
            formatDiscount: function (oObj) {
                if (!oObj) return;
                oObj.keyup(function () {
                    var reg = $(this).val().match(/\d+\.?\d{0,2}/);
                    var txt = '';
                    if (reg != null) {
                        txt = reg[0];
                    }
                    $(this).val(txt);
                }).change(function () {
                    $(this).keypress();
                    var v = $(this).val();
                    if (/\.$/.test(v)) {
                        $(this).val(v.substr(0, v.length - 1));
                    }
                });
            },
            formatTime: function (oObj, showTime) {
                if (!oObj) return;
                var minView = showTime ? 1 : 2;
                var format = showTime ? "yyyy-MM-dd hh:ii" : "yyyy-MM-dd";
                oObj.datetimepicker({
                    minView: minView,
                    todayBtn: 1,
                    language: 'zh-CN',
                    format: format,
                    weekStart: 1,
                    autoclose: 1
                });
            }
        },
        array: {
            valueToText: function (value, array, defaultText) {
                var text = defaultText == undefined ? value : defaultText;
                $.each(array, function () {
                    if (this.id != undefined && this.id === value) {
                        text = this.text;
                        return false;
                    }
                    if (this.id != undefined && this.id === value) {
                        text = this.text;
                        return false;
                    }
                    return true;
                });
                return text;
            },
            expandAndToString: function (array, separator) {
                var result = "";
                if (!separator) {
                    separator = ",";
                }
                $.each(array, function (index, item) {
                    result = result + item.toString() + separator;
                });
                return result.substring(0, result.length - separator.length);
            }
        },
        timeFormat: function (time, formatStr) {
            if (!(time instanceof Date)) {
                return time;
            }
            if (!Date.prototype.format) {
                Date.prototype.format = function (format) {
                    var o = {
                        "M+": this.getMonth() + 1, //month
                        "d+": this.getDate(), //day
                        "h+": this.getHours(), //hour
                        "m+": this.getMinutes(), //minute
                        "s+": this.getSeconds(), //second
                        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
                        "S": this.getMilliseconds() //millisecond
                    }
                    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
                    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
                    for (var k in o) if (new RegExp("(" + k + ")").test(format))
                        format = format.replace(RegExp.$1,
                        RegExp.$1.length == 1 ? o[k] :
                        ("00" + o[k]).substr(("" + o[k]).length));
                    return format;
                }
            }
            return time.format(formatStr);
        },
        hattedCode: function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        },
        toCamelCase: function (str) {
            if (!str) return str;
            if (str.Length == 1) return str.toLowerCase();
            return str[0].toLowerCase() + str.substring(1);
        }
    };

    if (typeof store === 'undefined') {
        var jspath = "/Content/js/store.min.js";
        $.getScript(jspath).done(function () {
            $.bode.store = store;
        }).fail(function () { alert("请检查/Content/js/store.min.js的路径是否正确!"); });
    }
    else {
        $.bode.store = store;
    }

    $.bode.auth = {
        login: function (token) {
            $.bode.store.set("BodeAbpAuthToken", token);
        },
        getToken: function () {
            return $.bode.store.get("BodeAbpAuthToken");
        },
        isAuth: function () {
            var token = $.bode.store.get("BodeAbpAuthToken");
            return typeof (token) !== "undefined" && token != null && token.length > 0;
        },
        clearToken: function () {
            $.bode.store.remove("BodeAbpAuthToken");
        }
    };
})(jQuery);