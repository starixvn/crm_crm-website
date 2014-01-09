"use strict";
/*global accUrlDefault, trimAllSpace, BlockUI, 
head, userId, gup, userSearch, isGuestSearch */

/* exported accid, timerId, autosearch,
isGuest, gotoURL, KeyPress, downup, onBlur,
onFocus, udpateOnlineStatus */

var accid = "sa"; //use for write and read cookie
var timerId = "";
var isonauto = false;
var showleft = false;
var fullheight, contentheight, fullwidth, contentwidth;
var bannerbox = 40; //40
var mainBox = 74; //74
var leftcontent = 0;
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

function changeiconleftright(showlefticon) {
    var btnleftright = $("#btnleftright");
    if (showlefticon) {
        btnleftright.removeClass("leftright");
        btnleftright.addClass("rightleft");
    }
    else {
        btnleftright.removeClass("rightleft");
        btnleftright.addClass("leftright");
    }
}

function leftright() {
    var leftpanel = $('.leftcontent');
    var rightpanel = $('.rightcontent');

    if (showleft) {
        leftcontent = 0;
        contentwidth = fullwidth - leftcontent;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", contentwidth - 2 + "px");
        rightpanel.css("left", leftcontent + "px");
        leftpanel.hide();
    }
    else {
        leftcontent = 273;
        contentwidth = fullwidth - leftcontent;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", contentwidth - 2 + "px");
        rightpanel.css("left", leftcontent + "px");
        leftpanel.show();
    }
    showleft = !showleft;
    changeiconleftright(showleft);
}

function preResize() {
    fullheight = $(".fullsize").height();
    fullwidth = $(".fullsize").width();
    contentheight = fullheight - bannerbox - mainBox;
    contentwidth = fullwidth - leftcontent;
    $(".contentbox").css("height", contentheight + "px");
    $(".rightcontent").css("width", contentwidth - 2 + "px");
    $(".rightcontent").css("height", contentheight + "px");

    $('#centerFr').css("height", contentheight - 2 + "px");
    $('#leftPanel').css("height", contentheight - 2 + "px");
}

function submitSearch(username) {

    if (username != null) {
        $('#txtuserName').val(username);
        userName = username;
    }
    else {
        userName = escape(jQuery.trim($('#txtuserName').val()));
    }

    var isGuest = $("#isGuest").attr("checked");
    if (username !== "") {
        if (userName.length > 50) {
            //$("#divErrorPoup").empty();
            $("#divErrorPoup").html('Max length of Username is 50!');
            $("#divErrorPoup").dialog({ width: 200, height: 100, zIndex: 500, resizable: false, modal: true,
                position: [5, 108], //
                title: 'Alert',
                close: function (event, ui) {
                    /*jshint unused: vars */
                },
                buttons: [{
                    id: "button-maxlength",
                    text: "OK",
                    click: function () {
                        //top.location.href = accUrlDefault;                     
                        $(this).dialog("close");
                    }
                }]
            });

            return false;
        }

        if (userName.toLowerCase() === 'search...' ||
            trimAllSpace($('#txtuserName').val()).length <= 0) {
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

        try {
            /*jshint newcap: false */
            BlockUI();
        } catch (e) {
        }

        $('#leftPanel').attr({ src: '../Customer/CRMInfo?username=' + userName + '&isguest=' + isGuest + '&issearch=1' });
        if (!showleft) {
            leftright();
        }
    }
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

        submitSearch();
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

head.ready(function () {
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

    $("#btnleftright").mouseover(function () {
        $(this).attr('class', $(this).attr('class').replace("rightleft", "rightleft_over").replace("leftright", "leftright_over"));
    });

    $("#btnleftright").mouseout(function () {
        $(this).attr('class', '');
        if (showleft) {
            $(this).addClass("rightleft");
        }
        else {
            $(this).addClass("leftright");
        }
    });

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
    userName = "";
    var username = gup("username");
    if (username != null && username !== "") {
        submitSearch(username);
        leftright();
    } else if (userSearch != null && userSearch !== "") {
        if (isGuestSearch === 'True') {
            $("#isGuest").attr("checked", true);
        }

        submitSearch(userSearch);
        if (!showleft) {
            leftright();
        }
    }
});