"use strict";
/*global ddsmoothmenu, BlockUI */
/* exported closedSubmenu */

ddsmoothmenu.init({
    mainmenuid: "smoothmenu1", //menu DIV id
    orientation: 'h', //Horizontal or vertical menu: Set to "h" or "v"
    classname: 'ddsmoothmenu', //class added to menu's outer DIV
    //customtheme: ["#1c5a80", "#18374a"],
    contentsource: "markup" //"markup" or ["container_id", "path_to_menu_file"]
});

$(".menu_0").mouseover(function () {
    $(this).addClass("current");
}).mouseout(function () {
    $(this).removeClass("current");
});

function closedSubmenu(id, isNullUrl) {
    var mainmenu = $("#smoothmenu1>ul");
    mainmenu.find("ul").css({ display: 'none', visibility: 'visible' });
    $("#smoothmenu1>ul>li").removeClass('menuselectedleft');
    $("#smoothmenu1>ul>li>a").removeClass('menuselected');

    $("#" + id + "").addClass('menuselectedleft');
    $("#" + id + ">a").addClass('menuselected');

    if (isNullUrl === 0) {
        try {
            /*jshint newcap: false */
            BlockUI();
        } catch (e) {
        }
    }
}
