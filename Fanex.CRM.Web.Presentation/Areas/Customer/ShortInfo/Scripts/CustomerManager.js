(function () {
    "use strict";
    /* global customerManager, SettingManager, sharedScriptManager, accId, crmConstants, hotKeyManager */
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
                IsFromURL: 1,
                IsReloadLeftPanel: 0
            },

            /* jshint camelcase: false */
            
            ViewResources: {
                GetCustomerByName_NoData: viewResources.NoInformation,
                OK: viewResources.OK,
                Cancel: viewResources.Cancel,
                SearchByGuest_DialogTitle: viewResources.SearchByGuest,
                Select: viewResources.Select,
                Symbol_NotApply: 'N/A',
                All_Languages: viewResources.AllLanguages,
                No_languages_found: viewResources.NoLanguage,
                Alert: viewResources.Alert,
                CustomerInfo_ConfirmSearchByGuest: viewResources.ConfirmSearchByGuest
            },

            leftPanelBlankPage: '../Home/BlankPage',

            getUplineInfoParams: {
                CustomerId: null
            },
            isConfirmSearchByGuest: false,
            currentCustomer: null,

            isLoadedSite: false,
            isLoadedInactiveSite: false,
            isLoadedCountry: false,
            isLoadedProvince: false,
            isLoadedLanguage: false,
            controllerScope: null
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
            self.DOMElements.dropdownProvince = $("#customerinfo-provinceoptions");
            self.DOMElements.dropdownLanguage = $("#customerinfo-languageoptions");
            self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
            self.DOMElements.autocompleteBox = parent.$("#Autocomplete_1");
        };
        self.getCustomerInfoByUsername = function () {            
            var scope = angular.element("#divContent").scope();
            scope.$apply(function () {
                var getCustomerInfoParams = self.getCustomerInfoByUsernameParamsFromURL(scope.username);
                if (getCustomerInfoParams.Username === null ||
                getCustomerInfoParams.Username === '' &&
                getCustomerInfoParams.Username === "null") {
                    return;
                }
                
                scope.getCustomerInfoByUsername(getCustomerInfoParams);
            });
        };
        self.getCustomerInfoByUsernameParamsFromURL = function (username) {
            var decodedUsername = username ? (new DOMParser).parseFromString(username, 'text/html').body.textContent : null;
            var getCustomerInfoParams = {
                Username: decodedUsername || $.trim(sharedScriptManager.urlDecode(sharedScriptManager.getURLParam("username"))),
                IsGuest: sharedScriptManager.getURLParam("isguest"),
                IsSearch: sharedScriptManager.getURLParam("issearch"),
                IsFromURL: sharedScriptManager.getURLParam("isfromurl"),
                IsReloadLeftPanel: sharedScriptManager.getURLParam("isreloadleftpanel")
            };

            self.CacheData.getCustomerInfoParams = getCustomerInfoParams;
            return getCustomerInfoParams;
        };
        self.getCustomerInfoByUsernameParams = function () {
            var getCustomerInfoParams = {
                Username: $.trim(self.DOMElements.textboxUsername.val()),
                IsGuest: self.DOMElements.checkboxIsGuest.attr("checked") ? true : false,
                IsSearch: sharedScriptManager.getURLParam("issearch"),
                IsFromURL: 0,
                IsReloadLeftPanel: 0
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
                parent.menuManager.emptyDashboard();
                self.showGetCustomerInfoByUsername_NoInfo();
                return false;
            }

            self.DOMElements.divContent.removeClass("display-none");
            return true;
        };
        self.checkGuestCustomerInfoResult = function (customer) {
            if (customer == null || customer === "") {
                self.goToBlankPage();
                parent.menuManager.emptyDashboard();
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
            self.DOMElements.textboxUsername.prop('disabled', true);
            parent.menuManager.hideLeftPanel();
            self.DOMElements.divResultPopUp.removeClass("display-none");
            self.DOMElements.divResultPopUp.html(self.CacheData.ViewResources.GetCustomerByName_NoData);
            self.DOMElements.divResultPopUp.dialog({
                width: 250,
                height: 150,
                zIndex: 500,
                resizable: false,
                modal: true,
                position: { my: "left+2 top+80", at: "left top" },
                title: self.CacheData.ViewResources.Alert,
                close: function (event, ui) {
                    /* jshint unused: false */
                    self.DOMElements.divResultPopUp.dialog("destroy");
                    parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                    self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                    self.DOMElements.textboxUsername.prop('disabled', false);
                },
                buttons: [{
                    text: self.CacheData.ViewResources.OK,
                    value: self.CacheData.ViewResources.OK,
                    click: function (event) {
                        event.stopPropagation();
                        self.DOMElements.divResultPopUp.dialog("close");
                    }
                }]
            });
        };
        self.showConfirmSearchByGuestDialog = function () {
            self.DOMElements.autocompleteBox.css("display", "none");

            self.DOMElements.divResultPopUp.html(self.CacheData.ViewResources.CustomerInfo_ConfirmSearchByGuest);
            self.DOMElements.divResultPopUp.removeClass("display-none");
            self.DOMElements.divResultPopUp.dialog({
                width: 280,
                height: 150,
                zIndex: 500,
                position: { my: "left+2 top+80", at: "left top" },
                resizable: false,
                modal: true,
                title: self.CacheData.ViewResources.SearchByGuest_DialogTitle,
                close: function (event, ui) {
                    /* jshint unused: false */
                    self.DOMElements.divResultPopUp.dialog("destroy");
                    parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');

                    self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                    if (!self.CacheData.isConfirmSearchByGuest) {
                        parent.menuManager.emptyDashboard();
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
                },
            {
                text: self.CacheData.ViewResources.Cancel,
                click: function () {
                    self.DOMElements.divResultPopUp.dialog('close');
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
                self.unblockUI();
                return;
            }           
            self.getAllSite();
            self.getAllSitesLicenseeInactive();
            self.getAllCountry();
            self.getAllProvince();
            self.getAllLanguage();
            parent.menuManager.showLeftPanel();
            self.updateCustomerSuggestion(customer);
            if (parseInt(self.CacheData.getCustomerInfoParams.IsSearch) === 1 &&
            parseInt(self.CacheData.getCustomerInfoParams.IsReloadLeftPanel) !== 1) {
                try {
                    parent.menuManager.getDashboardMenu(parent.dashboardManager.Enums.SubMenuId.Main);
                } catch (e) {
                }
            }
            // set txtuserName to current Username.
            if (customer.Name !== self.DOMElements.textboxUsername.val()) {
                self.DOMElements.textboxUsername.val(customer.Name);
            }
        };
        self.completedLoadLeftPanel = function () {
            if (self.CacheData.isLoadedSite &&
            self.CacheData.isLoadedInactiveSite &&
            self.CacheData.isLoadedCountry &&
            self.CacheData.isLoadedProvince &&                
            self.CacheData.isLoadedLanguage) {
                self.unblockUI();
            }
        };

        self.getAllSitesLicenseeInactive = function () {
            self.CacheData.isLoadedInactiveSite = false;
            sharedScriptManager.CallAjax(self,
                window.rootUrl + "Setting/AllSitesLicenseeInactive",
            null,
            self.renderAllSitesLicenseeInactive,
            null,
            self.completedLoadLeftPanel);
        };

        self.renderAllSitesLicenseeInactive = function (result) {
            var siteLicensee = result;
            self.CacheData.controllerScope.showInactiveSite = self.checkInactiveSite(siteLicensee);
            self.CacheData.controllerScope.$apply();
            self.CacheData.isLoadedInactiveSite = true;
        }; 

        self.checkInactiveSite = function (siteLicensee) {
            for (var index = 0; index < siteLicensee.length; index++) {
                var site = siteLicensee[index];
                if (site.Id === self.CacheData.currentCustomer.SiteSetting.Id) {
                    return true;
                }
            }
            return false;
        };

        self.getAllSite = function () {
            self.CacheData.isLoadedSite = false;
            sharedScriptManager.CallAjax(self,
                window.rootUrl + "Setting/AllSite",
                null,
                self.renderAllSiteResult,
                null,
                self.completedLoadLeftPanel);
        };

        self.renderAllSiteResult = function (result) {
            self.CacheData.controllerScope.selectedSite = self.CacheData.currentCustomer.SiteSetting;
            self.getAllSitesLicenseeInactive();
            self.CacheData.controllerScope.siteDropdownSettings = {
                idMember: "Id",
                nameMember: "Name",
                sourceData: result,
                childrenMember: "ChildSites"
            };          
            self.CacheData.controllerScope.$apply();
            self.CacheData.isLoadedSite = true;
        };   

        self.getAllCountry = function () {
            self.CacheData.isLoadedCountry = false;
            sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/AllCountry",
            null,
            self.renderAllCountryResult,
            null,
            self.completedLoadLeftPanel);
        };

        self.getAllProvince = function () {
            self.CacheData.isLoadedProvince = false;
            sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/AllProvince",
            null,
            self.renderAllProvinceResult,
            null,
            self.completedLoadLeftPanel);
        };

        self.renderAllCountryResult = function (result) {
            var siteOptions = '';
            var scope = angular.element($(".customerinfo-container")).scope();            
            if (!scope.isCN88) {
                siteOptions += '<option value="' + crmConstants.Enums.Country.NotApply + '">' +
                            self.CacheData.ViewResources.Symbol_NotApply +
                        '</option>';
            }
            
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
            self.DOMElements.dropdownCountry.find('option').click(function () {
                self.CacheData.controllerScope.customer.Country.Id = parseInt(customerManager.DOMElements.dropdownCountry.val());
                self.CacheData.controllerScope.$apply();
            });
            self.CacheData.isLoadedCountry = true;
        };

        self.renderAllProvinceResult = function (result) {
            var siteOptions = '';            
            if (result && result.length > 0) {
                var countryLength = result.length;
                for (var i = 0; i < countryLength; i++) {
                    var province = result[i];
                    siteOptions += '<option value="' + province.Id + '"' + '>' +
                                    province.Name +
                                '</option>';
                }
            }

            self.DOMElements.dropdownProvince.empty().html(siteOptions);
            self.DOMElements.dropdownProvince.val(self.CacheData.currentCustomer.Province.Id);
            self.DOMElements.dropdownProvince.find('option').click(function () {
                self.CacheData.controllerScope.customer.Province.Id = parseInt(customerManager.DOMElements.dropdownProvince.val());
                self.CacheData.controllerScope.$apply();
            });
            self.CacheData.isLoadedProvince = true;
        };

        self.getAllLanguage = function () {
            self.CacheData.isLoadedLanguage = false;
            sharedScriptManager.CallAjax(self,
            window.rootUrl + "Setting/AllLanguage",
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
                        text: info.Name,
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
            listData: dropdownLanguage,
            onCheck: function () {
                self.CacheData.controllerScope.customer.SelectedLanguages = self.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(",");
                self.CacheData.controllerScope.$apply();
            }
        });

            self.DOMElements.dropdownLanguage = $("#customerinfo-languageoptions");
            self.CacheData.controllerScope.customer.SelectedLanguages = self.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(",");
            self.CacheData.controllerScope.oldLanguages = self.CacheData.controllerScope.customer.SelectedLanguages;
            self.CacheData.controllerScope.$apply();
            self.DOMElements.dropdownLanguage.keyup(self.CacheData.controllerScope.textBoxKeyUp);
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
                SiteSettingId: self.CacheData.controllerScope.selectedSite ?
                    self.CacheData.controllerScope.selectedSite.Id : crmConstants.Enums.Site.NotApply,
                CountryId: parseInt(self.DOMElements.dropdownCountry.val()),
                ProvinceId: parseInt(self.DOMElements.dropdownProvince.val()),
                FirstName: customer.FirstName,
                LastName: customer.LastName,
                NickName: customer.NickName,
                Languages: self.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(","),

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

                PersonalInfoContacts: customer.PersonalInfoContacts,

                Email: customer.Email,
                MSN: customer.MSN,
                Skype: customer.Skype
            };
        };
        self.ShowUpdateResult = function (updateResult) {
            updateResult = sharedScriptManager.formatAjaxResult(updateResult);
            self.DOMElements.divResultPopUp.removeClass("display-none");
            self.DOMElements.divResultPopUp.html(updateResult.Name || updateResult.Message);
            self.DOMElements.divResultPopUp.dialog({
                width: 250,
                height: 150,
                zIndex: 500,
                resizable: false,
                modal: true,
                position: { my: "left+2 top+80", at: "left top" },
                title: self.CacheData.ViewResources.Alert,
                close: function (event, ui) {
                    /* jshint unused: false */
                    self.DOMElements.divResultPopUp.dialog("destroy");
                    parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                    self.DOMElements.divResultPopUp = parent.$("#div-resultpopup");

                    if (updateResult.Id === crmConstants.Enums.Error.None &&
                    parent.dashboardManager &&
                    parent.dashboardManager.CachedData.currentSubMenu === parent.dashboardManager.Enums.SubMenuId.Detail) {
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

            $(".btn-update").focus();
            if (parseInt(updateResult.Id) === crmConstants.Enums.Error.None) {
                setTimeout(function () {
                    self.DOMElements.divResultPopUp.dialog("close");
                }, 3000);
            }
        };

        self.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        self.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        self.enableGuestSearch = function () {
            self.DOMElements.checkboxIsGuest.attr("checked", "checked");
        };

    };

    window.customerManager = new CustomerManager();
    angular.element(document).ready(function () {
        window.customerManager.initPage();
    });
})();