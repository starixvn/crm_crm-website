"use strict";
/*global accUrlDefault, trimAllSpace, 
userId, gup, userSearch, isGuestSearch,
leftPanelManager,
menuManager,
homeManager */

/* exported accid, timerId, autosearch,
isGuest, gotoURL, KeyPress, downup, udpateOnlineStatus */

var accid = "sa"; //use for write and read cookie
var timerId = "";
var isonauto = false;
var fullheight, fullwidth;
var mainBox = 74; //74
var autosearch;
var showLogo = true;
var userName = "";
var isGuest;
var underMaintenanceCode = 3;

function openConfirmPopup(errorCode) {
    $("#dialog:ui-dialog").dialog("destroy");

    $("#divErrorPoup").html('Another session has been logged in or your session is expired. Forced to logout!');
    $("#divErrorPoup").dialog({
        width: 480,
        height: 140,
        zIndex: 500,
        resizable: false,
        modal: true,
        title: 'Alert',
        close: function (event, ui) {
            /* jshint unused: false */
            redirectPage(errorCode);
        },
        buttons: [
                {
                    text: "OK",
                    click: function () {
                        redirectPage(errorCode);
                        $(this).dialog("close");
                    }
                }
            ]
    });
    return false;
}

function redirectPage(errorCode) {
    if (errorCode === underMaintenanceCode) {
        top.location.href = umUrl;
    }
    else {
        top.location.href = logoutUrl;
    }
}

function preResize() {
    fullheight = $(".fullsize").height();
    fullwidth = $(".fullsize").width();
}

function gotoURL(frmName, url) {
    $('#' + frmName).attr({ src: url });
}

function KeyPress(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key === 13) {
        if (trimAllSpace($('#txtuserName').val()).length <= 0) {
            $("#divErrorPoup").html('Please input Username.');
            $("#divErrorPoup").dialog({ width: 200, height: 100, zIndex: 500, resizable: false, modal: true,
                position: { my: "left+5 top+108", at: "left top" }, //
                title: 'Alert',
                close: function (event, ui) {
                    /* jshint unused: false */
                },
                buttons: [{
                    text: "OK",
                    click: function () {
                        $(this).dialog("close");
                        $('#txtuserName').focus();
                    }
                }]
            });
            return false;
        }

        leftPanelManager.loadLeftPanel(null, true);
    }
    return;
}

function downup() {
    var logoHidden = $('.mainbox');
    if (showLogo) {
        logoHidden.addClass('hidden');

        $('#btndownup').addClass('downup');
        $('#btndownup').removeClass('updown');
        showLogo = false;
        mainBox = 0;
    }
    else {
        logoHidden.removeClass('hidden');

        $('#btndownup').addClass('updown');
        $('#btndownup').removeClass('downup');
        showLogo = true;
        mainBox = 74;
    }
}

function udpateOnlineStatus() {
    var reCheckIn = function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "UserLogin/CheckUserOnlineStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if(rs.ErrCode > 0) {
                    openConfirmPopup(rs.ErrCode);
                }
                else {
                    setTimeout(reCheckIn, 120000);
                }
            }, error: function () {
                openConfirmPopup();
            }
        });
    };

    reCheckIn();
}

function toggleSelectLanguage() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function selectLanguage(language) {
    window.location = window.rootUrl + "Home/Default?language=" + language;
    toggleSelectLanguage();
}

$(document).ready(function () {
    preResize();
    //Add events to page
    $(window).resize(function () {
        preResize();
    });

    var txtusername = $('#txtuserName');
    txtusername.blur(function () {
        if (!isonauto) {
            $('#Autocomplete_1').css('display', 'none');
        }
        else {
            txtusername.focus();
        }
    });

    txtusername.autocomplete({
        serviceUrl: window.rootUrl + 'CustomerSetting/CustomerAutoSuggestion',
        minChars: 2,
        isCacheSuggestion: false,
        zIndex: 9999,
        width: 185,
        deferRequestBy: 0, //miliseconds
        params: {
            userId: userId,
            isGuest: function () {
                return $('#isGuest').is(':checked');
            },
            rTime: function () {
                return new Date().getMilliseconds();
            }
        },

        //aditional parameters
        closeSuggestion: homeManager.focusSearchTextbox
    });

    $('.autocomplete').mouseover(function () {
        isonauto = true;
    }).mouseout(function () {
        isonauto = false;
    });
    //Add events to page  

    $("#btndownup").mouseover(function () {
        $(this).attr('class', $(this).attr('class').replace("updown", "updown_over").replace("downup", "downup_over"));
    });

    $("#btndownup").mouseout(function () {
        $(this).attr('class', '');
        if (showLogo) {
            $(this).addClass("updown");
        }
        else {
            $(this).addClass("downup");
        }
    });

    //search customer
    menuManager.initPage();
    leftPanelManager.initPage();

    userName = "";
    var username = gup("username");
    if (username != null && username !== "") {
        txtusername.val(username);
        leftPanelManager.loadLeftPanel(username, true);
        menuManager.showHideLeftPanel();
    } else if (userSearch != null && userSearch !== "") {
        if (isGuestSearch === 'True') {
            $("#isGuest").attr("checked", true);
        }

        txtusername.val(userSearch).removeClass("font-style-italic");
        leftPanelManager.loadLeftPanel(userSearch, true);
        menuManager.showLeftPanel();
    }
});
