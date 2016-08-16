(function ($) {
    $.bode = $.bode || { version: 1.0, vtime: (new Date().getTime()) };
})(jQuery);

(function ($) {
    $.bode.tree = function (selector, conf) {
        this.tree = $(selector);

        this.conf = conf;
        this.valueField = conf.valueField || "value";
        this.textField = conf.textField || "text";
        this.parentField = conf.parentField || "parentValue";
        this.initialValue = conf.initialValue || "0";
        this.itemSelect = conf.itemSelect || function (d) { };
        this.loadCompleted = conf.loadCompleted || function () { }
        this.itemCancel = conf.itemCancel || function (d) { },
        this.folderSelectEnable = conf.folderSelectEnable || false;
        this.multiSelectEnable = conf.multiSelectEnable || false;
        this.childrenHash = {};
        this.itemHash = {};

        this.selectItems = [];

        this.init = function () {
            //初始化数据源
            if (typeof (this.conf.url) != "undefined") {
                var tree = this;
                $.bode.ajax(this.conf.url, {}, function (d) {
                    tree.conf.source = d;
                    tree.initData();
                    tree.loadCompleted();
                });
            } else {
                this.initData();
                this.loadCompleted();
            }
        };

        this.initData = function () {
            //初始化哈希表
            for (var i = 0, n = this.conf.source.length; i < n; i++) {
                var item = this.conf.source[i];
                var parentIdKey = item[this.parentField];
                if (typeof (this.childrenHash[parentIdKey]) === "undefined") {
                    this.childrenHash[parentIdKey] = [];
                }
                this.childrenHash[parentIdKey].push(item);

                var itemValueKey = item[this.valueField].toString();
                this.itemHash[itemValueKey] = item;
            }

            this.load();
        }

        this.load = function () {
            var level = 0;
            var currentLevel = this.childrenHash[this.initialValue];
            while (currentLevel&&currentLevel.length > 0) {
                var newLevel = [];

                for (var i = 0, n = currentLevel.length; i < n; i++) {
                    var item = currentLevel[i];
                    var container = level === 0 ? this.tree : this.tree.find("div[data-value='" + item[this.parentField] + "']").next(".tree-folder-content");

                    if (typeof (this.childrenHash[item[this.valueField]]) === "undefined" || this.childrenHash[item[this.valueField]].length === 0) {
                        $('<div data-value="' + item[this.valueField] + '" class="tree-item" style="display: block;"><div class="tree-item-name">' + item[this.textField] + '</div></div>').appendTo(container);
                    } else {
                        $('<div class="tree-folder" style="display: block;"><div data-value="' + item[this.valueField] + '" class="tree-folder-header"><i class="fa fa-folder blueberry"></i><div class="tree-folder-name">' + item[this.textField] + '<div class="tree-actions"></div></div></div>' +
                        '<div class="tree-folder-content" style="display: none;"></div><div class="tree-loader" style="display: none;"><div class="tree-loading"><i class="fa fa-rotate-right fa-spin blueberry"></i></div></div></div>').appendTo(container);

                        newLevel = newLevel.concat(this.childrenHash[item[this.valueField]]);
                    }
                }
                currentLevel = newLevel;
                level++;
            }

            this.initEvent();
        };

        this.initEvent = function () {
            var tree = this;

            //点击事件
            var fnSelect = function (oObj) {
                tree.tree.find(".bg-palegreen").removeClass("bg-palegreen");
                oObj.addClass("bg-palegreen");

                var value = oObj.attr("data-value");
                var item = tree.itemHash[value];
                tree.itemSelect(item);
            }

            this.tree.find(".tree-item").click(function () {
                //判断父级文件夹是否展开
                if ($(this).closest(".tree-folder-content").is(":hidden")) {

                }

                //开启多选时不执行itemSelect方法
                if (tree.multiSelectEnable) {
                    if (!$(this).hasClass("bg-palegreen")) {
                        $('<i class="fa fa-check-square-o"></i>').prependTo($(this));
                        $(this).addClass("bg-palegreen");
                    } else {
                        $(this).children("i").remove();
                        $(this).removeClass("bg-palegreen");
                    }
                } else {
                    if (!$(this).hasClass("bg-palegreen")) fnSelect($(this));
                }

                return false;
            });

            this.tree.find(".tree-folder").click(function () {
                if ($(this).children(".tree-folder-content").is(":hidden")) {
                    $(this).children(".tree-folder-header").children("i").removeClass("fa-folder").addClass("fa-folder-open");
                    $(this).children(".tree-folder-content").show();
                } else {
                    $(this).children(".tree-folder-content").hide();
                    $(this).children(".tree-folder-header").children("i").removeClass("fa-folder-open").addClass("fa-folder");
                }

                //如果开启多选，文件夹不能被选中(暂不支持)
                if (tree.folderSelectEnable && !tree.multiSelectEnable && !$(this).children(".tree-folder-header").hasClass("bg-palegreen")) {

                    fnSelect($(this).children(".tree-folder-header"));
                }

                return false;
            });
        };

        this.init();
    }
})(jQuery);