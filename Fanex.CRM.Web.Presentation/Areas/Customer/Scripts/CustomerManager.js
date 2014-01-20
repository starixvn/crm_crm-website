"use strict";

var CustomerManager = function () {
    var self = this;
    self.DOMElements = {
        confirmSearchByGuestDialog: null,
        centerFrame: null,
        divContent: null,
        checkboxIsGuest: null,
        textboxUsername: null
    };

    self.CacheData = {
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null,
            IsSearch: 0
        },

        ViewResources: {
            GetCustomerByName_NoData: "There is no information",
            OK: "OK",
            SearchByGuest_DialogTitle: "Search By Guest"
        },

        leftPanelBlankPage: '../Home/BlankPage',

        getUplineInfoParams: {
            CustomerId: null
        },
        isConfirmSearchByGuest: false
    };

    self.initPage = function () {
        self.cacheDOMElements();
        var scope = angular.element($(".customerinfo-container")).scope();
        scope.$apply(function () {
            var getCustomerInfoParams = self.getCustomerInfoByUsernameParamsFromURL();
            if (getCustomerInfoParams.Username === null ||
                getCustomerInfoParams.Username === '' &&
                getCustomerInfoParams.Username === "null") {
                return;
            }

            scope.getCustomerInfoByUsername(getCustomerInfoParams);
        });
    };
    self.cacheDOMElements = function () {
        self.DOMElements.confirmSearchByGuestDialog = $("#divPopup");
        self.DOMElements.centerFrame = parent.$("#centerFr");
        self.DOMElements.divContent = $("#divContent");
        self.DOMElements.checkboxIsGuest = parent.$("#isGuest");
        self.DOMElements.textboxUsername = parent.$("#txtuserName");
    };

    self.getCustomerInfoByUsernameParamsFromURL = function () {
        var getCustomerInfoParams = {
            Username: sharedScriptManager.urlDecode(sharedScriptManager.getURLParam("username")),
            IsGuest: sharedScriptManager.getURLParam("isguest"),
            IsSearch: sharedScriptManager.getURLParam("issearch")
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        return getCustomerInfoParams;
    };
    self.getCustomerInfoByUsernameParams = function () {
        var getCustomerInfoParams = {
            Username: sharedScriptManager.urlDecode(sharedScriptManager.getURLParam("username")),
            IsGuest: self.DOMElements.checkboxIsGuest.attr("checked"),
            IsSearch: sharedScriptManager.getURLParam("issearch")
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        return getCustomerInfoParams;
    };

    self.checkCustomerInfoResult = function (customer) {
        if (customer == null || customer === "") {
            self.goToBlankPage();
        }

        if ((customer == null || customer === "") &&
            self.CacheData.getCustomerInfoParams.IsGuest === "false") {
            self.showConfirmSearchByGuestDialog();
            return false;
        }

        if (customer == null || customer === "") {
            self.showGetCustomerInfoByUsername_NoInfo();
            return false;
        }

        self.DOMElements.divContent.removeClass("display-none");
        return true;
    };

    self.checkGuestCustomerInfoResult = function (customer) {
        if (customer == null || customer === "") {
            self.goToBlankPage();
            self.showGetCustomerInfoByUsername_NoInfo();
            return false;
        }

        self.DOMElements.divContent.removeClass("display-none");
        return true;
    };

    self.goToBlankPage = function () {
        try {
            self.DOMElements.centerFrame.attr({
                src: self.CacheData.leftPanelBlankPage
            });

            self.DOMElements.divContent.addClass("display-none");
        } catch (e) {
        }
    };

    self.showGetCustomerInfoByUsername_NoInfo = function () {
        //        showAlertMessage("errmsg", self.CacheData.ViewResources.GetCustomerByName_NoData, 1);
        alert(self.CacheData.ViewResources.GetCustomerByName_NoData);
        self.goToBlankPage();
    };

    self.showConfirmSearchByGuestDialog = function () {
        self.DOMElements.confirmSearchByGuestDialog.dialog({
            width: 250,
            height: 150,
            zIndex: 500,
            position: [2, 2],
            resizable: false,

            title: self.CacheData.ViewResources.SearchByGuest_DialogTitle,
            close: function (event, ui) {
                /*jshint unused: vars */
                if (!self.CacheData.isConfirmSearchByGuest) {
                    self.showGetCustomerInfoByUsername_NoInfo();
                }

                self.CacheData.isConfirmSearchByGuest = false;
            },
            buttons: [{
                text: self.CacheData.ViewResources.OK,
                click: function () {
                    self.getGuestCustomerInfoByUsername();
                    self.CacheData.isConfirmSearchByGuest = true;
                    $(this).dialog("close");
                }
            }]
        });
    };

    self.getGuestCustomerInfoByUsername = function () {
        self.DOMElements.checkboxIsGuest.attr("checked", true);
        var scope = angular.element($(".customerinfo-container")).scope();
        scope.$apply(function () {
            scope.getGuestCustomerInfoByUsername(self.getCustomerInfoByUsernameParams());
        });
    };

    self.updateCustomerSuggestion = function (customer) {
        parent.BlockUI();
        var updateParam = {
            UserId: accId,
            CustomerId: customer.Id,
            Username: customer.Name
        };

        sharedScriptManager.CallAjax(self,
            window.rootUrl + "CustomerSetting/UpdateCustomerSuggestion",
            updateParam,
            null,
            null,
            parent.unblockUI);
    };
    self.comletedGetCustomerInfoByUsername = function (customer, isHavingData) {
        if (!isHavingData) {
            return;
        }

        self.updateCustomerSuggestion(customer);
        if (parseInt(self.CacheData.getCustomerInfoParams.IsSearch) === 1) {
            try {
                var latestFeedbackURL = '../Feedback/CustomerLastestFeedback?' + $.param({
                    custid: customer.Id,
                    custName: customer.Name
                });
                self.DOMElements.centerFrame.attr({
                    src: latestFeedbackURL
                });
            } catch (e) {
            }
        }
    };
    self.searchUplineInfo = function (username) {
        self.DOMElements.textboxUsername.val(username);
    };
};

var customerManager = new CustomerManager();
angular.element(document).ready(function () {
    customerManager.initPage();
});