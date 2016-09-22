(function ($) {
    $.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.wizard = function (selector, conf) {
        this.obj = $(selector);
        this.conf = conf;

        this.currentStep = 1;
        this.stepNums = this.obj.find("li").length;
        this.btPre = $('#' + this.obj.attr("id") + '-actions').find("button.btn-prev");
        this.btNext = $('#' + this.obj.attr("id") + '-actions').find("button.btn-next");

        this.setState = function () {
            //设置上一步按钮状态
            this.btPre.attr("disabled", this.currentStep === 1);

            //设置下一步按钮状态
            var nextText = this.currentStep === this.stepNums ? '保存' : '下一步';
            this.btNext.html(nextText + '<i class="fa fa-angle-right"></i>');

            //设置当前活动页
            this.obj.find("li.active").removeClass("active");
            this.obj.find('li:eq(' + (this.currentStep - 1) + ')').addClass("active");
            this.obj.find('li:eq(' + (this.currentStep - 1) + ')').removeClass("complete");

            this.obj.find('li:lt(' + (this.currentStep - 1) + ')').addClass("complete");
            this.obj.find('li:gt(' + (this.currentStep - 1) + ')').removeClass("complete");

            $(this.obj.attr("data-target")).find(".active").removeClass("active");
            $(this.obj.find("li.active").attr("data-target")).addClass("active");
        };
        this.previous = function () {
            if (typeof (this.conf.onPreClick) === "function" && !this.conf.onPreClick()) return;

            this.currentStep--;
            this.setState();
        };
        this.next = function () {
            if (this.currentStep === this.stepNums) {
                this.conf.onFinish();
                return;
            }
            if (typeof (this.conf.onNextClick) === "function" && !this.conf.onNextClick()) return;

            this.currentStep++;
            this.setState();
        };

        this.btPre.on("click", $.proxy(this.previous, this));
        this.btNext.on("click", $.proxy(this.next, this));
        //初始化上一步按钮
        this.btPre.attr("disabled", this.currentStep === 1);
    }
})(window.jQuery);