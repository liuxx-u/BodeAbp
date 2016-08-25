$(function () {
    $(window).resize(function (e) {
        $(".main-container").height($(window).height() - $(".header").height() - $(".footer").height());
        $(".wrap").height($(".main-container").height());
        $(".menu").css("minHeight", $(".sidebar").height() - $(".sidebar-header").height() - 1);
        $("#page").height($(".main-container").height() - 10);
        $(".content").width($(window).width() - $(".sidebar").width());
    }).resize();

    var vm = new Vue({
        el: "#index-page",
        data: {
            navs: [],
            curFirstNav: {},
            openNav: {},
            activeLeafNav: {url:"/admin/home/table"}
        },
        methods:{
            firstNavClick: function (nav) {
                if (this.curFirstNav.name == nav.name) return;
                this.curFirstNav = nav;
                if (nav.items.length > 0) {
                    this.openNav = nav.items[0];
                }
            },
            secondNavClick: function (secNav) {
                this.openNav = secNav == this.openNav ? {} : secNav;
            },
            leafNavClick: function (leafNav) {
                this.activeLeafNav = leafNav;
            }
        },
        created: function () {
            var self = this;
            $.bode.ajax("/api/services/zero/user/GetUserNavigations", {}, function (navs) {
                for (var i = 0, iLen = navs.length; i < iLen; i++) {
                    if (navs[i].name == "admin") {
                        self.navs = navs[i].items;
                        break;
                    }
                }
                if (self.navs.length > 1) {
                    self.curFirstNav = self.navs[0];
                    if (self.curFirstNav.items.length > 0) {
                        self.openNav = self.curFirstNav.items[0];
                    }
                }
            });
        }
    });
})