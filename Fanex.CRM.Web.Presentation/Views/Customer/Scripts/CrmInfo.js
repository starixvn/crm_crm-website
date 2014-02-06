"use strict";
/*global showAlertMessage, obj, Set_Cookie, gup, Get_Cookie, sharedScriptManager */
/* exported result, custid, roleid, crmInfo, GetCustId, leftTab2Click, collapseInfo */

var result = "";
var custid;
var roleid;
var crmInfo;

var cusidSearch;
var userNameSearch = '';

function openConfirmPopup() {
    $("#divPopup").dialog({
        width: 250,
        height: 150,
        zIndex: 500,
        position: [2, 2],
        resizable: false,

        close: function (event, ui) {
            /*jshint unused: vars */
            showAlertMessage("errmsg", "There is no information", 1);
            try {
                parent.gotoURL('centerFr', '../Home/BlankPage');
            } catch (e) {
            }
        },
        buttons: [
                {
                    text: "OK",
                    click: function () {
                        crmInfo.autoSearchGuestCustomer();
                        $(this).dialog("close");
                    }
                }
            ]
    });
}

function updateCustomerSearch() {
    //Update CustSearch
    try { parent.BlockUI(); } catch (e) { }
}

function GetCustId() {
    return obj('custid').value;
}

function loadUpline(custId) {
    var data = {
        'custId': custId
    };

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Customer/SelListUpline", //SelResult
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (listUpline) {
            /* jshint unused: vars */
        }
    });
}

function leftTab2Click(item, contentId) {
    $("#left_tab2 .current").removeClass("current");
    $("#" + item.id).addClass("current");

    for (var i = 1; i <= 2; i++) {
        $("#box_05_content_" + i).addClass("hidden");
    }
    $("#" + contentId).removeClass("hidden");
}

function collapseInfo() {
    var curr = $("#btnupdownCustInfo");
    var obj = $("#custInfoDetal");
    if (obj.hasClass("hidden")) {
        obj.removeClass("hidden");
        /*jshint camelcase: false */
        /*jshint newcap: false */
        Set_Cookie("coUserInfo", "show", 7, "/", "", "");
        curr.attr("title", "Hide Detail");
        curr.html('<img src="../Views/Customer/Styles/Images/icon_down.gif" class="imgDownUp" />');
    }
    else {
        obj.addClass("hidden");
        Set_Cookie("coUserInfo", "hidden", 7, "/", "", "");
        curr.html('<img src="../Views/Customer/Styles/Images/icon_up.gif" class="imgDownUp" />');
        curr.attr("title", "Show Detail");
    }
}

crmInfo = {
    isGuest: 0,
    userName: '',
    divContent: '',
    isSearch: 0,
    loadCrmInfo: function () {
        crmInfo.userName = sharedScriptManager.urlDecode(gup("username"));
        crmInfo.isGuest = gup("isguest");
        crmInfo.divContent = $("#divContent");
        crmInfo.isSearch = gup("issearch");

        if (crmInfo.userName !== null &&
            crmInfo.userName !== '' &&
            crmInfo.userName !== 'null') {
            var inputData = '{"userName":"' + crmInfo.userName + '","isGuest":' + crmInfo.isGuest + '}';
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Customer/GetCustomerInfoByUserName",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: inputData,
                success: function (customer) {
                    if (customer != null) {
                        loadUpline(customer.Id);
                        $("#box_02_headertext").html(customer.UserName);
                        crmInfo.divContent.css("display", "block");
                        $("#errmsg").html("");
                        cusidSearch = customer.Id;
                        userNameSearch = customer.UserName;
                        updateCustomerSearch();
                        if (parseInt(crmInfo.isSearch) === 1) {
                            try {
                                parent.gotoURL('centerFr',
                                    '../Feedback/CustomerLastestFeedback?custid=' + customer.Id +
                                    '&custName=' + customer.UserName);
                            } catch (e) {
                            }
                        }
                    }
                    else if (crmInfo.isGuest === "false") {
                        openConfirmPopup();
                    } else {
                        showAlertMessage("errmsg", "There is no information", 1);
                        try { parent.gotoURL('centerFr', '../Home/BlankPage'); } catch (e) { }

                    }
                },
                error: function () { },
                complete: function () { }
            });
        }
    },
    autoSearchGuestCustomer: function () {

        try { parent.$("#isGuest").attr("checked", true); } catch (e) { }
        if (crmInfo.userName != null && crmInfo.userName !== '' && crmInfo.userName !== 'null') {
            var inputData = '{"userName":"' + crmInfo.userName + '","isGuest":' + true + '}';
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Customer/GetCustomerInfoByUserName",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: inputData,
                success: function (customer) {
                    if (customer != null) {
                        loadUpline(customer.Id);
                        $("#box_02_headertext").html(customer.UserName);
                        $("#tmplCustInfo").tmpl(customer).appendTo("#tbodyCustInfo");
                        $("#tmplDetail").tmpl(customer).appendTo("#divDetail");
                        crmInfo.divContent.css("display", "block");
                        $("#errmsg").html("");

                        if (parseInt(crmInfo.isSearch) === 1) {
                            cusidSearch = customer.Id;
                            userNameSearch = customer.UserName;
                            updateCustomerSearch();
                            try {
                                parent.gotoURL('centerFr',
                                    '../Feedback/CustomerLastestFeedback?custid=' + customer.Id +
                                    '&custName=' + customer.UserName);
                            } catch (e) {
                            }
                        }
                    }
                    else {
                        showAlertMessage("errmsg", "There is no information", 1);

                        try { parent.gotoURL('centerFr', '../Home/BlankPage'); } catch (e) { }
                    }
                },
                error: function () { },
                complete: function () { }
            });
        }
    }
};

$(document).ready(
    function init() {

        $('.button_left02').click(function () {
            $('.button_left02_active').removeClass("button_left02_active");
            $(this).addClass("button_left02_active");
            //Reset main menu
            if (parent.resetMenu) {
                parent.resetMenu();
            }
        });

        //Johnny Add: User info hide or show is store in cookie.
        /*jshint camelcase: false */
        /*jshint newcap: false */
        var className = Get_Cookie("coUserInfo");
        var curr = $("#btnupdownCustInfo");
        var tmp = $("#custInfoDetal");
        if (className === "show") {
            tmp.removeClass("hidden");
            curr.attr("title", "Hide Detail");
            curr.html('<img src="../Views/Customer/Styles/Images/icon_down.gif" class="imgDownUp">');
        }

        //Load data
        crmInfo.loadCrmInfo();
        try { parent.jQuery.unblockUI(); } catch (e) { }

    }
);