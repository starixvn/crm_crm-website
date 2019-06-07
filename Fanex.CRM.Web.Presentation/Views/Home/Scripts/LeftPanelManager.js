"use strict";
/* global BlockUI,
menuManager,
fixBugManager */
/* exported leftPanelManager */
var LeftPanelManager = function () {
    var self = this;
    self.DOMElements = {
        textboxUsername: null,
        checkboxIsGuest: null,
        resultPopup: null,
        leftPanel: null
    };

    self.CacheData = {
        MaxUsernameLength: 50,
        /* jshint camelcase: false */
        ViewResources: {
            Home_Index_ErrorUsernameMaxLength: viewResources.ErrorUsernameMaxLength,
            Home_Index_ErrorUsernameEmpty: viewResources.ErrorUsernameEmpty,
            OK: 'OK'
        },

        DefaultUsernameSearch: 'search...',
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null
        }
    };

    self.initPage = function () {
        fixBugManager.fixJqueryParamMethod();
        self.cacheDOMElements();
    };

    self.cacheDOMElements = function () {
        self.DOMElements.textboxUsername = $('#txtuserName');
        self.DOMElements.checkboxIsGuest = $('#isGuest');
        self.DOMElements.resultPopup = $('#divErrorPoup');
        self.DOMElements.leftPanel = $('#leftPanel');
    };

    self.loadLeftPanel = function (username, isFromURL, isReloadLeftPanel) {
        var getCustomerInfoParams = {
            Username: username != null ? username : jQuery.trim(self.DOMElements.textboxUsername.val()).replace(/<|>|#|&/g, ''),
            IsGuest: self.DOMElements.checkboxIsGuest.attr("checked") ? true : false
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        var isValid = self.validateGetCustomerInfoParams(getCustomerInfoParams);
        if (!isValid) {
            return;
        }

        /* jshint newcap: false */
        BlockUI();
        var sourceURL = '../CustomerSetting/ShortCustomerInfo?' + $.param({
            username: getCustomerInfoParams.Username,
            isguest: getCustomerInfoParams.IsGuest,
            issearch: 1,
            isfromurl: isFromURL ? 1 : 0,
            isreloadleftpanel: isReloadLeftPanel ? 1 : 0
        });

        self.DOMElements.leftPanel.attr({
            src: sourceURL
        });

        menuManager.showLeftPanel();
    };

    self.validateGetCustomerInfoParams = function (getCustomerInfoParams) {
        if (getCustomerInfoParams.Username !== "" &&
            getCustomerInfoParams.Username.length > self.CacheData.MaxUsernameLength) {
            self.DOMElements.resultPopup.html(self.CacheData.ViewResources.Home_Index_ErrorUsernameMaxLength);
            self.showValidateGetParamsResult();
            return false;
        }

        if (getCustomerInfoParams.Username !== "" &&
            (getCustomerInfoParams.Username.toLowerCase() === self.CacheData.DefaultUsernameSearch ||
            $.trim(getCustomerInfoParams.Username) === "")) {
            self.DOMElements.resultPopup.html(self.CacheData.ViewResources.Home_Index_ErrorUsernameEmpty);
            self.showValidateGetParamsResult();
            return false;
        }

        return true;
    };

    self.showValidateGetParamsResult = function () {
        self.DOMElements.resultPopup.dialog({
            width: 200,
            height: 150,
            zIndex: 500,
            resizable: false,
            modal: true,
            position: { my: "left+5 top+108", at: "left top" },
            title: 'Alert',
            close: function (event, ui) {
                /* jshint unused: false */
            },
            buttons: [{
                id: "button-maxlength",
                text: self.CacheData.ViewResources.OK,
                click: function () {
                    $(this).dialog("close");
                    self.DOMElements.textboxUsername.focus();
                }
            }]
        });
    };
};

var leftPanelManager = new LeftPanelManager();