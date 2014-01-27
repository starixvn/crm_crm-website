"use strict";

var CustomerManager = function () {
    var self = this;
    self.DOMElements = {
        confirmSearchByGuestDialog: null,
        centerFrame: null,
        divContent: null,
        checkboxIsGuest: null,
        textboxUsername: null,
        dropdownSite: null,
        dropdownLanguage: null,
        dropdownLanguageHeaderCheckBox: null,
        optionNotApplyCheckBox: null,
        divResultPopUp: null
    };

    self.CacheData = {
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null,
            IsSearch: 0,
            IsFromURL: 1
        },

        ViewResources: {
            GetCustomerByName_NoData: "There is no information",
            OK: "OK",
            SearchByGuest_DialogTitle: "Search By Guest",
            Select: 'Select',
            Symbol_NotApply: 'N/A',
            All_Languages: 'All Languages',
            No_languages_found: 'No languages found.',
            Alert: 'Alert',
            CustomerInfo_ConfirmSearchByGuest: "Do you want to search guest customer's information?"
        },

        leftPanelBlankPage: '../Home/BlankPage',

        getUplineInfoParams: {
            CustomerId: null
        },
        isConfirmSearchByGuest: false,
        currentCustomer: null,

        isLoadedSite: false,
        isLoadedCountry: false,
        isLoadedLanguage: false
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.getCustomerInfoByUsername();
    };
    self.cacheDOMElements = function () {
        self.DOMElements.centerFrame = parent.$("#centerFr");
        self.DOMElements.divContent = $("#divContent");
        self.DOMElements.checkboxIsGuest = parent.$("#isGuest");
        self.DOMElements.textboxUsername = parent.$("#txtuserName");
        self.DOMElements.dropdownSite = $("#customerinfo-siteoptions");
        self.DOMElements.dropdownCountry = $("#customerinfo-countryoptions");
        self.DOMElements.dropdownLanguage = $("#customerinfo-languageoptions");
        self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
    };
    self.getCustomerInfoByUsername = function () {
        var scope = angular.element("#divContent").scope();
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
    self.getCustomerInfoByUsernameParamsFromURL = function () {
        var getCustomerInfoParams = {
            Username: sharedScriptManager.urlDecode(sharedScriptManager.getURLParam("username")),
            IsGuest: sharedScriptManager.getURLParam("isguest"),
            IsSearch: sharedScriptManager.getURLParam("issearch"),
            IsFromURL: sharedScriptManager.getURLParam("isfromurl")
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        return getCustomerInfoParams;
    };
    self.getCustomerInfoByUsernameParams = function () {
        var getCustomerInfoParams = {
            Username: self.DOMElements.textboxUsername.val(),
            IsGuest: self.DOMElements.checkboxIsGuest.attr("checked") ? true : false,
            IsSearch: sharedScriptManager.getURLParam("issearch"),
            IsFromURL: 0
        };

        self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
        return getCustomerInfoParams;
    };

    self.checkCustomerInfoResult = function (customer) {
        if (customer == null || customer === "") {
            self.goToBlankPage();
        }

        if ((customer == null || customer === "") &&
            (self.CacheData.getCustomerInfoParams.IsGuest === "false" ||
            self.CacheData.getCustomerInfoParams.IsGuest === "") &&
            !parseInt(self.CacheData.getCustomerInfoParams.IsFromURL)) {
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
        self.goToBlankPage();
        parent.menuManager.hideLeftPanel();
        self.DOMElements.divResultPopUp.removeClass("display-none");
        self.DOMElements.divResultPopUp.html(self.CacheData.ViewResources.GetCustomerByName_NoData);
        self.DOMElements.divResultPopUp.dialog({
            width: 250,
            height: 150,
            zIndex: 500,
            resizable: false,
            modal: true,
            position: [2, 80],
            title: self.CacheData.ViewResources.Alert,
            close: function (event, ui) {
                /*jshint unused: vars */
                self.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
            },
            buttons: [{
                text: self.CacheData.ViewResources.OK,
                value: self.CacheData.ViewResources.OK,
                click: function () {
                    self.DOMElements.divResultPopUp.dialog("close");
                }
            }]
        });
    };

    self.showConfirmSearchByGuestDialog = function () {
        self.DOMElements.divResultPopUp.html(self.CacheData.ViewResources.CustomerInfo_ConfirmSearchByGuest);
        self.DOMElements.divResultPopUp.removeClass("display-none");
        self.DOMElements.divResultPopUp.dialog({
            width: 280,
            height: 150,
            zIndex: 500,
            position: [2, 80],
            resizable: false,

            title: self.CacheData.ViewResources.SearchByGuest_DialogTitle,
            close: function (event, ui) {
                /*jshint unused: vars */
                self.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');

                self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                if (!self.CacheData.isConfirmSearchByGuest) {
                    self.showGetCustomerInfoByUsername_NoInfo();
                }

                self.CacheData.isConfirmSearchByGuest = false;
            },
            buttons: [{
                text: self.CacheData.ViewResources.OK,
                click: function () {
                    self.CacheData.isConfirmSearchByGuest = true;
                    self.DOMElements.divResultPopUp.dialog("close");
                    self.getGuestCustomerInfoByUsername();
                }
            }]
        });

        parent.menuManager.hideLeftPanel();
    };

    self.getGuestCustomerInfoByUsername = function () {
        self.DOMElements.checkboxIsGuest.attr("checked", "checked");
        var scope = angular.element($(".customerinfo-container")).scope();
        scope.$apply(function () {
            scope.getGuestCustomerInfoByUsername(self.getCustomerInfoByUsernameParams());
        });
    };

    self.updateCustomerSuggestion = function (customer) {
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
            null);
    };
    self.completedGetCustomerInfoByUsername = function (customer, isHavingData) {
        self.CacheData.currentCustomer = customer;
        parent.homeManager.CachedData.currentCustomer = customer;
        if (!isHavingData) {
            $scope.unblockUI();
            return;
        }

        self.getAllSite();
        self.getAllCountry();
        self.getAllLanguage();
        parent.menuManager.showLeftPanel();
        self.updateCustomerSuggestion(customer);
        if (parseInt(self.CacheData.getCustomerInfoParams.IsSearch) === 1) {
            try {
                parent.menuManager.getDashboardMenu();
                parent.menuManager.goToURL('centerFr', '/Dashboard');
            } catch (e) {
            }
        }
    };
    self.completedLoadLeftPanel = function () {
        if (self.CacheData.isLoadedSite &&
            self.CacheData.isLoadedCountry &&
            self.CacheData.isLoadedLanguage) {
            self.unblockUI();
        }
    };

    self.getAllSite = function () {
        self.CacheData.isLoadedSite = false;
        sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/GetAllSite",
            null,
            self.renderAllSiteResult,
            null,
            self.completedLoadLeftPanel);
    };
    self.renderAllSiteResult = function (result) {
        var siteOptions = '';
        siteOptions += '<option value="' + crmConstants.Enums.Site.NotApply + '">' +
                            self.CacheData.ViewResources.Select +
                        '</option>';
        if (result && result.length > 0) {
            var siteGroupLength = result.length;
            for (var i = 0; i < siteGroupLength; i++) {
                var siteGroup = result[i];
                var childSiteLength = siteGroup.ChildSites.length;
                siteOptions += '<optgroup label="' + siteGroup.Name + '">';
                for (var j = 0; j < childSiteLength; j++) {
                    var site = siteGroup.ChildSites[j];
                    siteOptions += '<option value="' + site.Id + '"' + '>' +
                                        site.Name +
                                    '</option>';
                }

                siteOptions += '</optgroup>';
            }
        }

        self.DOMElements.dropdownSite.empty().html(siteOptions);
        self.DOMElements.dropdownSite.val(self.CacheData.currentCustomer.SiteSetting.Id);
        self.CacheData.isLoadedSite = true;
    };
    self.getAllCountry = function () {
        self.CacheData.isLoadedCountry = false;
        sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/GetAllCountry",
            null,
            self.renderAllCountryResult,
            null,
            self.completedLoadLeftPanel);
    };
    self.renderAllCountryResult = function (result) {
        var siteOptions = '';
        siteOptions += '<option value="' + crmConstants.Enums.Country.NotApply + '">' +
                            self.CacheData.ViewResources.Symbol_NotApply +
                        '</option>';
        if (result && result.length > 0) {
            var countryLength = result.length;
            for (var i = 0; i < countryLength; i++) {
                var country = result[i];
                siteOptions += '<option value="' + country.Id + '"' + '>' +
                                    country.Name +
                                '</option>';
            }
        }

        self.DOMElements.dropdownCountry.empty().html(siteOptions);
        self.DOMElements.dropdownCountry.val(self.CacheData.currentCustomer.Country.Id);
        self.CacheData.isLoadedCountry = true;
    };
    self.getAllLanguage = function () {
        self.CacheData.isLoadedLanguage = false;
        sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/GetAllLanguage",
            null,
            self.renderAllLanguageResult,
            null,
            self.completedLoadLeftPanel);
    };
    self.renderAllLanguageResult = function (result) {
        self.DOMElements.dropdownLanguage.empty();
        var dropdownLanguage = [];
        var prefixId = 'customerinfo-languageoptions-';
        var isCheckAll = true;

        if (result && result.length > 0) {
            var optionLength = result.length;
            for (var i = 0; i < optionLength; i++) {
                var info = result[i];
                var isChecked = self.IsCustomerKnowLanguage(info.Id);
                if (!isChecked) {
                    isCheckAll = false;
                }

                dropdownLanguage.push({
                    id: prefixId + info.Id,
                    value: info.Id,
                    text: info.Value,
                    checked: isChecked,
                    enable: true,
                    cssClass: "",
                    items: []
                });
            }
        }

        self.DOMElements.dropdownLanguage.fanexMultiSelect(
        {
            minWidth: 177,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: isCheckAll,
            showFooter: false,
            showTooltip: false,
            checkAllText: self.CacheData.ViewResources.All_Languages,
            isUsedCustomCheckAllText: true,
            customCheckAllText: self.CacheData.ViewResources.All_Languages,
            noItemText: self.CacheData.ViewResources.No_languages_found,
            noneSelectedText: self.CacheData.ViewResources.Symbol_NotApply,
            selectedList: 3,
            listData: dropdownLanguage
        });

        self.DOMElements.dropdownLanguage = $("#customerinfo-languageoptions");
        self.CacheData.isLoadedLanguage = true;
    };
    self.IsCustomerKnowLanguage = function (languageId) {
        var result = false;
        var languageLength = self.CacheData.currentCustomer.Languages.length;
        for (var i = 0; i < languageLength; i++) {
            var language = self.CacheData.currentCustomer.Languages[i];
            if (languageId === language.Id) {
                result = true;
                break;
            }
        }

        return result;
    };
    self.updateTextboxUsername = function (username) {
        self.DOMElements.textboxUsername.val(username);
    };
    self.getUpdatedCustomerInfoParams = function (customer) {
        return {
            CustomerId: customer.Id,
            SiteSettingId: parseInt(self.DOMElements.dropdownSite.val()),
            CountryId: parseInt(self.DOMElements.dropdownCountry.val()),
            FirstName: customer.FirstName,
            LastName: customer.LastName,
            NickName: customer.NickName,
            Languages: self.DOMElements.dropdownLanguage.fanexMultiSelect("getItems").join(","),

            PhoneInfo1CountryCode: customer.PhoneInfo[0].CountryCode,
            PhoneInfo1AreaCode: customer.PhoneInfo[0].AreaCode,
            PhoneInfo1: customer.PhoneInfo[0].PhoneNumber,

            PhoneInfo2CountryCode: customer.PhoneInfo[1].CountryCode,
            PhoneInfo2AreaCode: customer.PhoneInfo[1].AreaCode,
            PhoneInfo2: customer.PhoneInfo[1].PhoneNumber,

            PhoneInfo3CountryCode: customer.PhoneInfo[2].CountryCode,
            PhoneInfo3AreaCode: customer.PhoneInfo[2].AreaCode,
            PhoneInfo3: customer.PhoneInfo[2].PhoneNumber,

            MobilePhoneCountryCode: customer.MobilePhone.CountryCode,
            MobilePhoneAreaCode: customer.MobilePhone.AreaCode,
            MobilePhone: customer.MobilePhone.PhoneNumber,

            PersonalInfoContact: customer.PersonalInfoContact,

            Email: customer.Email,
            MSN: customer.MSN,
            Skype: customer.Skype
        };
    };
    self.ShowUpdateResult = function (updateResult) {
        self.DOMElements.divResultPopUp.removeClass("display-none");
        self.DOMElements.divResultPopUp.html(updateResult.Name);
        self.DOMElements.divResultPopUp.dialog({
            width: 250,
            height: 150,
            zIndex: 500,
            resizable: false,
            modal: true,
            position: [2, 80],
            title: self.CacheData.ViewResources.Alert,
            close: function (event, ui) {
                /*jshint unused: vars */
                self.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");

                if (updateResult.Id === crmConstants.Enums.Error.None &&
                    parent.dashboardManager &&
                    parent.dashboardManager.CachedData.currentSubMenu == parent.dashboardManager.Enums.SubMenuId.Detail) {
                    parent.dashboardManager.dashboardMenuClick(parent.dashboardManager.Enums.SubMenuId.Detail);
                }
            },
            buttons: [{
                text: self.CacheData.ViewResources.OK,
                click: function () {
                    self.DOMElements.divResultPopUp.dialog("close");
                }
            }]
        });
    };

    self.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };
    self.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    self.enableGuestSearch = function () {
        self.DOMElements.checkboxIsGuest.attr("checked", "checked");
    };
};

var customerManager = new CustomerManager();
angular.element(document).ready(function () {
    customerManager.initPage();
});