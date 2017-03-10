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
            crumbs: "首页",
            activeLeafNav: { url: "/admin/home/default" }
        },
        methods: {
            firstNavClick: function (nav) {
                if (this.curFirstNav.id == nav.id) return;
                this.curFirstNav = nav;
                if (nav.children.length > 0) {
                    this.openNav = nav.children[0];
                }
            },
            secondNavClick: function (secNav) {
                this.openNav = secNav == this.openNav ? {} : secNav;
            },
            leafNavClick: function (leafNav) {
                this.activeLeafNav = leafNav;
                this.crumbs = this.curFirstNav.name + ">" + this.openNav.name + ">" + this.activeLeafNav.name;
                //设置iframe中实际的href为当前菜单的url
                document.getElementById("page").contentWindow.location.href = leafNav.url;
            }
        },
        created: function () {
            var self = this;
            abp.ajax({
                url: "/api/services/zero/navigation/GetUserNavigations",
                type: "POST",
                success: function (navs) {
                    self.navs = navs;

                    if (self.navs.length > 1) {
                        self.curFirstNav = self.navs[0];
                        if (self.curFirstNav.children.length > 0) {
                            self.openNav = self.curFirstNav.children[0];
                        }
                    }
                }
            });
        }
    });
})