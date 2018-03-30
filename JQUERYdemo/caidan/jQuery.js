$(document).ready(function () {
    $(".main > a").next("ul").hide();//使得加载页面看不见子菜单 选择器含义：选择main类下儿子一代中的<a>标记
    $(".hmain > a").next("ul").hide();//使得加载页面看不见子菜单
    $(".main > a").click(function () {
        var ulNode = $(this).next("ul");
        if (ulNode.css("display") == "none") {
            ulNode.slideDown();
            $(':focus').css("background-image", "url('001.jpg')");//改变背景图片
        } else {
            ulNode.slideUp();
            $(':focus').css("background-image", "url('002.jpg')");//改变背景图片
        }
    });
    $(".hmain").hover(function () {
        $(this).children("ul").stop();//停止以前未执行的动画，直接执行下面动画
        $(this).children("ul").slideDown();
        $(this).children("a").css("background-image", "url('001.jpg')");//改变背景图片
    }, function () {
        $(this).children("ul").stop();//停止以前未执行的动画，直接执行下面动画
        $(this).children("ul").slideUp();
        $(this).children("a").css("background-image", "url('002.jpg')");//改变背景图片
    })
})