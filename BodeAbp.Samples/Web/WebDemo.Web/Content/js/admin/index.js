$(".menu>li").on("click",function () {
    $(this).siblings().find("ul").slideUp();
    $(this).find("ul").slideDown();
});

$(".submenu>li").on("click", function () {
    $(".submenu>li.active").removeClass("active");
    $(this).addClass("active");
});


$(window).resize(function(e) {
    $(".main-container").height($(window).height() - $(".header").height() - $(".footer").height());
    $(".wrap").height($(".main-container").height());
    $(".menu").css("minHeight", $(".sidebar").height() - $(".sidebar-header").height() - 1);
    $("#page").height($(window).height() - $("#hd").height() - $("#ft").height() - 12);
    $(".content").width($(window).width() - $(".sidebar").width());
}).resize();

$(".menu>li").css({ "borderColor": "#dbe9f1" });
$(".menu>.current").prev().css({ "borderColor": "#7ac47f" });
$(".menu").on("click", "li", function (e) {
	var aurl = $(this).find("a").attr("date-src");
	$("#page").attr("src", aurl);
	$(".menu>li").css({ "borderColor": "#dbe9f1" });
	$(".menu>.current").prev().css({ "borderColor": "#7ac47f" });
	return false;
});