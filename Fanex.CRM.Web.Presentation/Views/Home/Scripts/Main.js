"use strict";
/*global accUrlDefault, trimAllSpace, BlockUI, 
head, userId, gup, userSearch, isGuestSearch */

/* exported accid, timerId, autosearch,
isGuest, gotoURL, KeyPress, downup, onBlur,
onFocus, udpateOnlineStatus */

var accid = "sa"; //use for write and read cookie
var timerId = "";
var isonauto = false;
var fullheight, fullwidth;
var bannerbox = 40; //40
var mainBox = 74; //74
var autosearch;
var showLogo = true;
var userName = "";
var isGuest;

function openConfirmPopup() {
    $("#dialog:ui-dialog").dialog("destroy");

    $("#divErrorPoup").html('Another session has been logged in or your session is expired. Forced to logout!');
    $("#divErrorPoup").dialog({
        width: 480,
        height: 140,
        zIndex: 500,
        resizable: false,
        modal: true,
        close: function (event, ui) {
            /*jshint unused: vars */
            top.location.href = accUrlDefault;
        },
        buttons: [
                {
                    text: "OK",
                    click: function () {
                        top.location.href = accUrlDefault;
                        $(this).dialog("close");
                    }
                }
            ]
    });
    return false;
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
                position: [5, 108], //
                title: 'Alert',
                close: function (event, ui) {
                    /*jshint unused: vars */
                },
                buttons: [{
                    text: "OK",
                    click: function () {
                        //top.location.href = accUrlDefault;  

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

function onBlur() {
    var inputValue = 'Search...';
    var oldName = userName;
    var newName = $('#txtuserName').val();

    if (oldName !== '' && newName !== '') {
        inputValue = newName;
    }
    else if (oldName !== '' && newName === '') {
        inputValue = oldName;
    }
    else if (newName !== '') {
        inputValue = newName;
    }
    else {
        inputValue = 'Search...';
    }

    $('#txtuserName').val($.URLDecode(inputValue));
}

function onFocus() {
    if ($('#txtuserName').val() === 'Search...') {
        $('#txtuserName').val('');
    }
}

function udpateOnlineStatus() {

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Account/CheckUserOnlineStatus",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            if (rs != null && rs.ErrCode != null) {
                if (rs.ErrCode > 0) {
                    openConfirmPopup();
                }

                return false;
            }
            else {
                openConfirmPopup();
                return false;
            }
        }, error: function () {
            openConfirmPopup();
            return false;
        }
    });
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
        serviceUrl: window.rootUrl + 'Customer/CustomerAutoSuggestion',
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
        }

        //aditional parameters
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
        $('#txtuserName').val(username);
        leftPanelManager.loadLeftPanel(username, true);
        menuManager.showHideLeftPanel();
    } else if (userSearch != null && userSearch !== "") {
        if (isGuestSearch === 'True') {
            $("#isGuest").attr("checked", true);
        }

        $('#txtuserName').val(userSearch);
        leftPanelManager.loadLeftPanel(userSearch, true);
        menuManager.showLeftPanel();
    }
});
