"use strict";

var CustomerManager = function () {
    var self = this;
    self.DOMElements = {

    };

    self.CacheData = {
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null
        },

        ViewResources: {
            GetCustomerByName_NoData: "There is no information",
            OK: "OK"
        }
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.getCustomerInfoByUsername();
    };
    self.cacheDOMElements = function () {
        self.DOMElements.confirmSearchByGuestDialog = $("#divPopup");
    };

    self.getCustomerInfoByUsername = function () {
        var getCustomerInfoParams = {
            Username: $.URLDecode(gup("username")),
            IsGuest: gup("isguest")
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        if (getCustomerInfoParams.Username === null ||
            getCustomerInfoParams.Username === '' &&
            getCustomerInfoParams.Username === "null") {
            return;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerSetting/GetCustomerInfoByUsername",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(getCustomerInfoParams),
            success: function (customer) {
                self.renderCustomerInfoResult(customer);
            },
            error: function () { },
            complete: function () { }
        });
    };

    self.renderCustomerInfoResult = function (customer) {
        if (customer == null &&
            self.CacheData.getCustomerInfoParams.IsGuest === "false") {
            self.showConfirmSearchByGuestDialog();
            return;
        }

        if (customer == null) {
            self.showGetCustomerInfoByUsername_NoInfo();
            return;
        }

        //        loadUpline(customer.Id);
        //        $("#box_02_headertext").html(customer.UserName);
        //        crmInfo.divContent.css("display", "block");
        //        $("#errmsg").html("");
        //        cusidSearch = customer.Id;
        //        userNameSearch = customer.UserName;
        //        updateCustomerSearch();
        //        if (parseInt(crmInfo.isSearch) === 1) {
        //            try {
        //                parent.gotoURL('centerFr',
        //                                    '../Feedback/CustomerLastestFeedback?custid=' + customer.Id +
        //                                    '&custName=' + customer.UserName);
        //            } catch (e) {
        //            }
        //        }
    };

    self.goToBlankPage = function () {
        try {
            parent.gotoURL('centerFr', '../Home/BlankPage');
        } catch (e) {
        }
    };

    self.showGetCustomerInfoByUsername_NoInfo = function () {
        showAlertMessage("errmsg", self.CacheData.ViewResources.GetCustomerByName_NoData, 1);
        self.goToBlankPage();
    };

    self.showConfirmSearchByGuestDialog = function () {
        self.DOMElements.confirmSearchByGuestDialog.dialog({
            width: 250,
            height: 150,
            zIndex: 500,
            position: [2, 2],
            resizable: false,

            title: "Search By Guest",
            close: function (event, ui) {
                /*jshint unused: vars */
                self.showGetCustomerInfoByUsername_NoInfo();
            },
            buttons: [{
                text: self.CacheData.ViewResources.OK,
                click: function () {
                    crmInfo.autoSearchGuestCustomer();
                    $(this).dialog("close");
                }
            }]
        });
    };
};

var customerManager = new CustomerManager();
$(document).ready(function () {
    customerManager.initPage();
});