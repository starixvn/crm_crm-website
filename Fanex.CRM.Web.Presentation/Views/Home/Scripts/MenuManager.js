"use strict";
/* global crmConstants */

var MenuManager = function () {
    var self = this;
    self.Enums = {
    };

    self.DOMElements = {
        IconCollapseExpandLeftPanel: null,
        LeftContent: null,
        RightContent: null
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
        if (menuManager.CachedData.isShowingLeftPanel) {
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
        $("#" + frameId).attr({
            src: url
        });
    };
};

var menuManager = new MenuManager();