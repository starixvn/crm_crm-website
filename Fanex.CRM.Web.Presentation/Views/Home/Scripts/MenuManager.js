"use strict";
/* global crmConstants,
leftPanelManager,
homeManager */
var menuManager;
var MenuManager = function () {
    var self = this;
    self.Enums = {
    };

    self.DOMElements = {
        IconCollapseExpandLeftPanel: null,
        LeftContent: null,
        RightContent: null,
        DashboardMenu: null,
        SubMenu: null
    };

    self.CachedData = {
        isShowingLeftPanel: false
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.bindEvents();
        self.DOMElements.IconCollapseExpandLeftPanel.attr("data-currentstatus", crmConstants.Enums.AppearanceStatus.Expand);
        self.showHideLeftPanel();
    };
    self.cacheDOMElements = function () {
        self.DOMElements.IconCollapseExpandLeftPanel = $("#icon-collapse-expand-leftpanel");
        self.DOMElements.LeftContent = $(".leftcontent");
        self.DOMElements.RightContent = $(".rightcontent");
        self.DOMElements.SubMenu = parent.$('#divSubMenu');
    };
    self.bindEvents = function () {
        self.DOMElements.IconCollapseExpandLeftPanel.unbind("click");
        self.DOMElements.IconCollapseExpandLeftPanel.click(function () {
            self.showHideLeftPanel();
        });

        //        self.DOMElements.IconCollapseExpandLeftPanel.tipsy({
        //            html: true,
        //            opacity: 0.9,
        //            gravity: 'ne'
        //        });
    };

    self.showHideLeftPanel = function () {
        var currentStatus = self.DOMElements.IconCollapseExpandLeftPanel.attr("data-currentstatus");
        switch (currentStatus) {
            case crmConstants.Enums.AppearanceStatus.Collapse:
                self.showLeftPanel();
                break;
            case crmConstants.Enums.AppearanceStatus.Expand:
                self.hideLeftPanel();
                break;
        }
    };

    self.showLeftPanel = function () {
        var username = leftPanelManager.CacheData.getCustomerInfoParams.Username;
        if (menuManager.CachedData.isShowingLeftPanel ||
            username === null ||
            username === "") {
            return;
        }

        self.DOMElements.LeftContent.removeClass(crmConstants.Enums.CssClass.DisplayNone);
        self.DOMElements.LeftContent.css({ width: "22%", display: "block" });
        self.DOMElements.RightContent.css({ width: "78%", left: "22%" });
        self.DOMElements.IconCollapseExpandLeftPanel.attr("data-currentstatus", crmConstants.Enums.AppearanceStatus.Expand);
        menuManager.CachedData.isShowingLeftPanel = true;
    };
    self.hideLeftPanel = function () {
        self.DOMElements.LeftContent.addClass(crmConstants.Enums.CssClass.DisplayNone);
        self.DOMElements.LeftContent.css({ width: "0", display: "none" });
        self.DOMElements.RightContent.css({ width: "100%", left: "0" });
        self.DOMElements.IconCollapseExpandLeftPanel.attr("data-currentstatus", crmConstants.Enums.AppearanceStatus.Collapse);
        menuManager.CachedData.isShowingLeftPanel = false;
    };

    self.isShowingLeftPanel = function () {
        return self.DOMElements.IconCollapseExpandLeftPanel.attr("data-currentstatus") === crmConstants.Enums.AppearanceStatus.Expand;
    };

    self.goToURL = function (frameId, url) {
        // reset changing parameters
        parent.homeManager.CachedData.isChangingData = parent.homeManager.CachedData.isChangingDataLeftPanel = false;
        if (parent.dashboardManager) {
            parent.dashboardManager.CachedData.currentSubMenu = '';
            self.CachedData.currentMenuTabIndex = 1;
        }

        $("#" + frameId).attr({ src: url });
    };

    self.getDashboardMenu = function (tabId) {
        if (homeManager.CachedData.currentCustomer &&
            homeManager.CachedData.currentCustomer.Id !== 0) {
            $.ajax({
                url: window.rootUrl + "Home/GettingDashboardMenu",
                type: "GET",
                success: function (response, status, xhr) {
                    /* jshint unused: vars */
                    self.DOMElements.SubMenu.html(response);
                    parent.dashboardManager.dashboardMenuClick(tabId);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    /* jshint unused: vars */
                }
            });
        } else {
            self.remindSearchCustomer();
        }
    };

    self.setActiveClassForDasboarSubMenu = function (subMenuId) {
        $('#ulDashboardMenu li').removeClass('active');
        $('#' + subMenuId).addClass('active');
    };

    self.remindSearchCustomer = function () {
        self.DOMElements.SubMenu.html('');
        menuManager.goToURL('centerFr', window.rootUrl + "Home/SearchCustomerRemind");
    };
    self.emptyDashboard = function () {
        self.DOMElements.SubMenu.html('');
    };

    self.setActiveClassForSubMenu = function () {
        var breadscrumbMenuName = arguments;
        var breadscrumbMenuNameLength = arguments ? breadscrumbMenuName.length : 0;
        var parentMenuName = "";
        var lastMenu = "";
        if (breadscrumbMenuNameLength) {
            lastMenu = breadscrumbMenuName[breadscrumbMenuNameLength - 1];
            for (var i = 0; i < breadscrumbMenuNameLength - 1; i++) {
                parentMenuName += '<li>' + breadscrumbMenuName[i] + '</li>';
            }
        }

        var activeMenu =
            '<ul class="breadcrumbsMenu list-inline list-unstyled">\
                <li><span class="icon-home touchable-element"></span></li>' +
                parentMenuName +
                '<li class="active">' + lastMenu + '</li>\
            </ul>';

        self.DOMElements.SubMenu.html(activeMenu);
        self.DOMElements.SubMenu.find(".breadcrumbsMenu .icon-home").click(function () {
            menuManager.getDashboardMenu(parent.dashboardManager.Enums.SubMenuId.Main);
        });
    };

    self.setActiveClassForLeftPanel = function (menu) {
        var activeMenu =
        '<ul class="breadcrumbsMenu list-inline list-unstyled">\
            <li><span class="icon-home touchable-element"></span></li>\
            <li class="active">' + menu + '</li>\
        </ul>';

        self.DOMElements.SubMenu.html(activeMenu);
        self.DOMElements.SubMenu.find(".breadcrumbsMenu .icon-home").click(function () {
            menuManager.getDashboardMenu(parent.dashboardManager.Enums.SubMenuId.Main);
        });
    };

    self.clearSubmenuInfo = function () {
        parent.$('#divSubMenu').html('');
    };
};

menuManager = new MenuManager();