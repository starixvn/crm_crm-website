"use strict";
/* global hotKeyManager,
keyboardManager,
leftPanelManager,
menuManager */
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
            DefaultSearchCustomer: "Phone or Username or Nickname",
            Error_SearchCustomer_Empty: 'Please input username!',
            OK: "OK",
            Cancel: "Cancel",
            Alert: 'Alert',
            Confirm_Ignore_Changes: 'Confirm Ignore Changes'
        },

        username: null,
        currentCustomer: null,
        isIgnoreChange: false,

        // Prevent unsaved escape
        isChangingData: false,
        ignoreConfirmMessage: '',
        isChangingDataLeftPanel: false,
        ignoreConfirmMessageLeftPanel: '',

        isIgnoreChangePopUp: false,
        isChangingDataPopUp: false,
        ignoreConfirmMessagePopUp: '',

        shareInfoBetweenPopup: {
            ActionResult: null,
            ShouldReloadView: false,
            PopupController: null
        }
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
        self.DOMElements.ButtonSearch = $("#buttonsearch");
        self.DOMElements.ResultMessage = $("#div-resultpopup");
        self.DOMElements.CheckboxIsGuest = $("#isGuest");
        self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
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
            self.TextboxUsernameBlur();
            if (self.CachedData.isChangingData || self.CachedData.isChangingDataLeftPanel) {
                self.showConfirmIgnoreChange(self.loadLeftPanel);
            } else {
                self.loadLeftPanel();
            }
        }

        return;
    };

    self.showConfirmIgnoreChange = function (callbackFunction) {
        self.DOMElements.divResultPopUp.html(self.CachedData.isChangingDataLeftPanel ?
            self.CachedData.ignoreConfirmMessageLeftPanel :
            self.CachedData.ignoreConfirmMessage);
        self.DOMElements.divResultPopUp.removeClass("display-none");
        self.DOMElements.divResultPopUp.dialog({
            width: 450,
            height: 180,
            zIndex: 500,
            position: ['center', 120],
            resizable: false,
            modal: true,
            title: self.CachedData.ViewResources.Confirm_Ignore_Changes,
            close: function (event, ui) {
                /*jshint unused: vars */
                self.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
            },
            buttons: [{
                text: self.CachedData.ViewResources.OK,
                click: function () {
                    self.CachedData.isIgnoreChange = true;
                    self.CachedData.isChangingData = false;
                    self.CachedData.isChangingDataLeftPanel = false;
                    self.DOMElements.divResultPopUp.dialog("close");
                    callbackFunction();
                }
            },
            {
                text: self.CachedData.ViewResources.Cancel,
                click: function () {
                    self.DOMElements.divResultPopUp.dialog('close');
                }
            }]
        });
    };
    self.showConfirmIgnoreChangePopUp = function (ignoreFunction,
        cancelIgnoreFunction) {
        self.DOMElements.divResultPopUp.html(self.CachedData.ignoreConfirmMessagePopUp);
        self.DOMElements.divResultPopUp.removeClass("display-none");
        self.DOMElements.divResultPopUp.dialog({
            width: 450,
            height: 180,
            zIndex: 500,
            position: ['center', 120],
            resizable: false,
            modal: true,
            title: self.CachedData.ViewResources.Confirm_Ignore_Changes,
            close: function (event, ui) {
                /*jshint unused: vars */
                self.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                if (cancelIgnoreFunction &&
                    typeof (cancelIgnoreFunction) === "function") {
                    cancelIgnoreFunction();
                }
            },
            buttons: [{
                text: self.CachedData.ViewResources.OK,
                click: function () {
                    self.CachedData.isIgnoreChangePopUp = true;
                    self.CachedData.isChangingDataPopUp = false;
                    self.CachedData.isChangingDataPopUp = false;
                    self.DOMElements.divResultPopUp.dialog("close");
                    if (ignoreFunction &&
                        typeof (ignoreFunction) === "function") {
                        ignoreFunction();
                    }
                }
            },
            {
                text: self.CachedData.ViewResources.Cancel,
                click: function () {
                    self.DOMElements.divResultPopUp.dialog('close');
                }
            }]
        });
    };

    self.ButtonSearchClick = function () {
        self.TextboxUsernameBlur();
        if (self.CachedData.isChangingData || self.CachedData.isChangingDataLeftPanel) {
            self.showConfirmIgnoreChange(self.loadLeftPanel);
        } else {
            self.loadLeftPanel();
        }
    };

    self.loadLeftPanel = function () {
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
            if (self.CachedData.isChangingData || self.CachedData.isChangingDataLeftPanel) {
                self.showConfirmIgnoreChange(self.loadLeftPanel);
            } else {
                self.loadLeftPanel();
            }
        }
    };
    self.focusSearchTextbox = function () {
        self.DOMElements.TextboxUsername.focus();
    };
    self.invokeHotKey_CtrlG = function () {
        self.DOMElements.CheckboxIsGuest.attr("checked", "checked");
    };
    self.invokeHotKey_CtrlD = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.Detail);
        }
    };
    self.invokeHotKey_CtrlL = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.CommunicationLog);
        }
    };
    self.invokeHotKey_CtrlR = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.Redemption);
        }
    };
    self.invokeHotKey_CtrlK = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.SpeciaRemark);
        }
    };
    self.invokeHotKey_CtrlH = function () {
        menuManager.getDashboardMenu(parent.dashboardManager.Enums.SubMenuId.Main);
    };
    self.invokeHotKey_CtrlM = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.Main);
        }
    };
    self.invokeHotKey_CtrlT = function () {
        if (parent.dashboardManager) {
            parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.ActivityLog);
        }
    };
    self.invokeHotKey_CtrlF = function () {
        self.DOMElements.TextboxUsername.focus();
    };
};

var homeManager = new HomeManager();
$(document).ready(function () {
    homeManager.initPage();
});