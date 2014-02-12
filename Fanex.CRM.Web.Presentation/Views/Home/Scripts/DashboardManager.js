"use strict";
/* global homeManager, menuManager,
hotKeyManager */
/* exported dashboardManager */
var dashboardManager;
var DashboardManager = function () {
    var self = this;

    self.Enums = {
        SubMenuId: {
            Main: 'liMain',
            Detail: 'liDetail',
            CommunicationLog: 'liCommnunicationLog',
            SpeciaRemark: 'liSpecialRemark',
            Redemption: 'liRedemption'
        }
    };

    self.CachedData = {
        currentSubMenu: null,
        subMenuTabIndex: {
            Tab1: self.Enums.SubMenuId.Main,
            Tab2: self.Enums.SubMenuId.Detail,
            Tab3: self.Enums.SubMenuId.CommunicationLog,
            Tab4: self.Enums.SubMenuId.SpeciaRemark,
            Tab5: self.Enums.SubMenuId.Redemption
        },
        currentMenuTabIndex: 1,
        maxMenuTabIndex: 5,
        subMenuId: null
    };

    self.initPage = function () {
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: self,
            Id: "Dashboard"
        });
    };

    self.dashboardMenuClickAfterCheckChanging = function () {
        var subMenuId = self.CachedData.subMenuId;
        if (subMenuId !== self.CachedData.currentSubMenu) {
            var customer = homeManager.CachedData.currentCustomer;
            var subMenuIdEnum = self.Enums.SubMenuId;
            self.CachedData.currentSubMenu = subMenuId;
            switch (subMenuId) {
                case subMenuIdEnum.Main:
                    menuManager.goToURL('centerFr', window.rootUrl + 'Dashboard/Index?' + $.param({
                        customerId: customer.Id
                    }));
                    self.CachedData.currentMenuTabIndex = 1;
                    break;
                case subMenuIdEnum.Detail:
                    menuManager.goToURL('centerFr', window.rootUrl + 'CustomerSettingDetail/Index?' + $.param({
                        customerId: customer.Id
                    }));
                    self.CachedData.currentMenuTabIndex = 2;
                    break;
                case subMenuIdEnum.CommunicationLog:
                    menuManager.goToURL('centerFr', window.rootUrl + 'CommunicationLog/Index?' + $.param({
                        customerId: customer.Id
                    }));
                    self.CachedData.currentMenuTabIndex = 3;
                    break;
                case subMenuIdEnum.SpeciaRemark:
                    menuManager.goToURL('centerFr', window.rootUrl + 'Remark/Index?' + $.param({
                        customerId: customer.Id
                    }));
                    self.CachedData.currentMenuTabIndex = 4;
                    break;
                case subMenuIdEnum.Redemption:
                    menuManager.goToURL('centerFr', window.rootUrl + 'Redemption/Index?' + $.param({
                        customerId: customer.Id
                    }));
                    self.CachedData.currentMenuTabIndex = 5;
                    break;
                default:
                    break;
            }

            menuManager.setActiveClassForDasboarSubMenu(subMenuId);
        }
    };

    self.dashboardMenuClick = function (subMenuId) {
        self.CachedData.subMenuId = subMenuId;
        if (homeManager.CachedData.isChangingData) {
            homeManager.showConfirmIgnoreChange(self.dashboardMenuClickAfterCheckChanging);
            return;
        }

        self.dashboardMenuClickAfterCheckChanging();
    };
    self.moveToPreviousTab = function () {
        self.CachedData.currentMenuTabIndex--;
        if (self.CachedData.currentMenuTabIndex < 1) {
            self.CachedData.currentMenuTabIndex = self.CachedData.maxMenuTabIndex;
        }

        var subMenuId = self.CachedData.subMenuTabIndex["Tab" + self.CachedData.currentMenuTabIndex];
        if ($("#" + subMenuId).length === 0 &&
            subMenuId !== self.CachedData.currentSubMenu) {
            subMenuId = self.moveToPreviousTab();
        }

        return subMenuId;
    };
    self.moveToNextTab = function () {
        self.CachedData.currentMenuTabIndex++;
        if (self.CachedData.maxMenuTabIndex < self.CachedData.currentMenuTabIndex) {
            self.CachedData.currentMenuTabIndex = 1;
        }

        var subMenuId = self.CachedData.subMenuTabIndex["Tab" + self.CachedData.currentMenuTabIndex];
        if ($("#" + subMenuId).length === 0 &&
            subMenuId !== self.CachedData.currentSubMenu) {
            subMenuId = self.moveToNextTab();
        }

        return subMenuId;
    };

    /* jshint camelcase: false */
    self.invokeHotKey_CtrlComma = function () {
        var subMenuId = self.moveToPreviousTab();
        dashboardManager.dashboardMenuClick(subMenuId);
    };
    /* jshint camelcase: false */
    self.invokeHotKey_CtrlPeriod = function () {
        var subMenuId = self.moveToNextTab();
        dashboardManager.dashboardMenuClick(subMenuId);
    };
};

dashboardManager = new DashboardManager();
$(document).ready(function () {
    dashboardManager.initPage();
});