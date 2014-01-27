"use strict";
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
        ViewResources: {
            Home_Index_ErrorUsernameMaxLength: 'Max length of username is 50!',
            Home_Index_ErrorUsernameEmpty: 'Please input username!',
            OK: 'OK'
        },

        DefaultUsernameSearch: 'search...',
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null
        }
    };

    self.initPage = function () {
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
            Username: username != null ? username : escape(jQuery.trim(self.DOMElements.textboxUsername.val())),
            IsGuest: self.DOMElements.checkboxIsGuest.attr("checked") ? true : false
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        var isValid = self.validateGetCustomerInfoParams(getCustomerInfoParams);
        if (!isValid) {
            return;
        }

        BlockUI();
        var sourceURL = '../CustomerSetting/GetShortCustomerInfo?' + $.param({
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
            height: 100,
            zIndex: 500,
            resizable: false,
            modal: true,
            position: [5, 108],
            title: 'Alert',
            close: function (event, ui) {
                /*jshint unused: vars */
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