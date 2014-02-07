"use strict";
/* global hotKeyManager,
keyboardManager,
leftPanelManager
*/
/* exported shouldReloadLogs */
var shouldReloadLogs = false;
var HomeManager = function () {
    var self = this;
    self.DOMElements = {
        TextboxUsername: null,
        ButtonSearch: null,
        ResultMessage: null,
        CheckboxIsGuest: null
    };

    self.CachedData = {
        /* jshint camelcase: false */
        ViewResources: {
            DefaultSearchCustomer: "Search...",
            Error_SearchCustomer_Empty: 'Please input username!',
            OK: "OK",
            Alert: 'Alert'
        },

        username: null,
        currentCustomer: null
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.bindEvents();

        self.CachedData.username = self.DOMElements.TextboxUsername.val();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: self,
            Id: "Home"
        });
    };

    self.cacheDOMElements = function () {
        self.DOMElements.TextboxUsername = $("#txtuserName");
        self.DOMElements.ButtonSearch = $("#btnSendMsg");
        self.DOMElements.ResultMessage = $("#div-resultpopup");
        self.DOMElements.CheckboxIsGuest = $("#isGuest");
    };

    self.bindEvents = function () {
        //        self.DOMElements.TextboxUsername.unbind("focus");
        self.DOMElements.TextboxUsername.focus(self.TextboxUsernameFocus);

        //        self.DOMElements.TextboxUsername.unbind("blur");
        self.DOMElements.TextboxUsername.blur(self.TextboxUsernameBlur);

        //        self.DOMElements.TextboxUsername.unbind("keyup");
        self.DOMElements.TextboxUsername.keyup(self.TextboxUsernameKeyUp);

        self.DOMElements.ButtonSearch.unbind("click");
        self.DOMElements.ButtonSearch.click(self.ButtonSearchClick);

        self.DOMElements.CheckboxIsGuest.unbind("keyup");
        self.DOMElements.CheckboxIsGuest.keyup(self.CheckboxIsGuestKeyUp);
    };

    self.TextboxUsernameFocus = function () {
        if (self.DOMElements.TextboxUsername.val() === self.CachedData.ViewResources.DefaultSearchCustomer) {
            self.DOMElements.TextboxUsername.val('');
        }
    };
    self.TextboxUsernameBlur = function () {
        var inputValue;
        var oldName = self.CachedData.username;
        var newName = self.DOMElements.TextboxUsername.val();

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
            inputValue = self.CachedData.ViewResources.DefaultSearchCustomer;
        }

        self.DOMElements.TextboxUsername.val($.URLDecode(inputValue));
    };

    self.TextboxUsernameKeyUp = function (e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key === keyboardManager.Enums.KeyCode.Enter) {
            var isValid = self.validDateTextboxUsername();
            self.TextboxUsernameBlur();
            if (isValid) {
                self.CachedData.username = self.DOMElements.TextboxUsername.val();
                leftPanelManager.loadLeftPanel(null, false);
            }
        }

        return;
    };

    self.ButtonSearchClick = function () {
        self.TextboxUsernameBlur();
        if (self.validDateTextboxUsername()) {
            self.CachedData.username = self.DOMElements.TextboxUsername.val();
            leftPanelManager.loadLeftPanel(null, false);
        }
    };
    self.validDateTextboxUsername = function () {
        var username = self.DOMElements.TextboxUsername.val();
        if (username === self.CachedData.ViewResources.DefaultSearchCustomer ||
            $.trim(username).length <= 0) {
            self.TextboxUsernameBlur();
            self.DOMElements.ResultMessage.removeClass("display-none");
            self.DOMElements.ResultMessage.html(self.CachedData.ViewResources.Error_SearchCustomer_Empty);
            self.DOMElements.ResultMessage.dialog({
                width: 200,
                height: 150,
                zIndex: 500,
                resizable: false,
                modal: true,
                position: [2, 90],
                title: self.CachedData.ViewResources.Alert,
                close: function (event, ui) {
                    /*jshint unused: vars */
                    self.DOMElements.ResultMessage.dialog("destroy");
                    $(document).find(".ui-widget-overlay").remove();
                    $(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                    self.DOMElements.divResultPopUp = $("#div-resultpopup");
                },
                buttons: [{
                    text: self.CachedData.ViewResources.OK,
                    click: function () {
                        self.DOMElements.ResultMessage.dialog("close");
                        self.DOMElements.TextboxUsername.focus();
                    }
                }]
            });

            return false;
        }

        return true;
    };
    self.CheckboxIsGuestKeyUp = function (e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key === keyboardManager.Enums.KeyCode.Enter) {
            var isValid = self.validDateTextboxUsername();
            if (isValid) {
                self.CachedData.username = self.DOMElements.TextboxUsername.val();
                leftPanelManager.loadLeftPanel(null, false);
            }
        }
    };
    self.invokeHotKey_CtrlG = function () {
        self.DOMElements.CheckboxIsGuest.attr("checked", "checked");
    };
    self.invokeHotKey_CtrlD = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.Detail);
        }
    };
    self.invokeHotKey_CtrlC = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.CommunicationLog);
        }
    };
};

var homeManager = new HomeManager();

$(document).ready(function () {
    homeManager.initPage();
});