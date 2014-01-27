
customerDetailApplication.controller('customerDetailController', function ($scope,
    $http,
    $customerDetailService) {
    $scope.customerDetail = customerDetail;
    $scope.siteList = null;
    $scope.countryList = null;
    $scope.languageList = null;
    $scope.isLoadedAllSite = false;
    $scope.isLoadedAllCountry = false;
    $scope.isLoadedAllLanguage = false;
    $scope.isShowPersonalInfo = true;
    $scope.isShowHomeAddressInfo = true;
    $scope.isShowBankInfoInfo = true;

    $scope.DOMElements = {
        divResultPopUp: null,
        textboxFirstName: null,
        dropdownLanguage: null
    };
    $scope.initController = function () {
        $scope.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
        $scope.DOMElements.textboxFirstName = $("#firstName");
        $scope.DOMElements.dropdownLanguage = $("#customerinfodetail-languageoptions");
    };

    $scope.getAllSite = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $customerDetailService.getAllSite()
            .then(function (data) {
                $scope.siteList = data.Result;
                $scope.isLoadedAllSite = true;
                $scope.completedLoadDetailInfo();
            },
            function (data) {
                $scope.isLoadedAllSite = true;
                $scope.completedLoadDetailInfo();
            });
    };
    $scope.completedLoadDetailInfo = function () {
        if ($scope.isLoadedAllSite &&
            $scope.isLoadedAllCountry &&
            $scope.isLoadedAllLanguage) {
            $scope.unblockUI();
        }
    };
    $scope.getAllCountry = function () {
        $scope.BlockUI();
        $scope.isLoadedAllCountry = false;
        $customerDetailService.getAllCountry()
            .then(function (data) {
                $scope.countryList = data.Result;
                $scope.isLoadedAllCountry = true;
                $scope.completedLoadDetailInfo();
            },
            function (data) {
                $scope.isLoadedAllCountry = true;
                $scope.completedLoadDetailInfo();
            });
    };
    $scope.getAllLanguage = function () {
        $scope.BlockUI();
        $scope.isLoadedAllLanguage = false;
        $customerDetailService.getAllLanguage()
            .then(function (data) {
                $scope.languageList = data.Result;
                $scope.isLoadedAllLanguage = true;
                $scope.renderAllLanguageResult(data.Result);
                $scope.completedLoadDetailInfo();
            },
            function (data) {
                $scope.isLoadedAllLanguage = true;
                $scope.completedLoadDetailInfo();
            });
    };
    $scope.renderAllLanguageResult = function (result) {
        $scope.DOMElements.dropdownLanguage.empty();
        var dropdownLanguage = [];
        var prefixId = 'customerinfo-languageoptions-';
        var isCheckAll = true;

        if (result && result.length > 0) {
            var optionLength = result.length;
            for (var i = 0; i < optionLength; i++) {
                var info = result[i];
                var isChecked = $scope.isCustomerKnowLanguage(info.Id);
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

        $scope.DOMElements.dropdownLanguage.fanexMultiSelect(
                {
                    minWidth: 177,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: isCheckAll,
                    showFooter: false,
                    showTooltip: false,
                    checkAllText: viewResources.All_Languages,
                    isUsedCustomCheckAllText: true,
                    customCheckAllText: viewResources.All_Languages,
                    noItemText: viewResources.No_languages_found,
                    noneSelectedText: viewResources.Symbol_NotApply,
                    selectedList: 3,
                    listData: dropdownLanguage
                });

        $scope.DOMElements.dropdownLanguage = $("#customerinfodetail-languageoptions");
        $scope.isLoadedAllLanguage = true;
    };
    $scope.isCustomerKnowLanguage = function (languageId) {
        var result = false;
        var languageLength = $scope.customerDetail.Languages.length;
        for (var i = 0; i < languageLength; i++) {
            var language = $scope.customerDetail.Languages[i];
            if (languageId === language.Id) {
                result = true;
                break;
            }
        }

        return result;
    };
    $scope.getCustomerInfoDetailById = function () {
    };
    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };
    $scope.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    $scope.updateCustomerInfoDetail = function () {
        var customerDetail = $scope.customerDetail;
        var updatedCustomerInfoDetailParams = {
            CustomerId: customerDetail.Id,
            FirstName: customerDetail.FirstName,
            LastName: customerDetail.LastName,
            LanguageId: $scope.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(","),
            Phone1PhoneNumber: customerDetail.PhoneInfo[0].PhoneNumber,
            Phone1CountryCode: customerDetail.PhoneInfo[0].CountryCode,
            Phone1AreaCode: customerDetail.PhoneInfo[0].AreaCode,
            Phone2PhoneNumber: customerDetail.PhoneInfo[1].PhoneNumber,
            Phone2CountryCode: customerDetail.PhoneInfo[1].CountryCode,
            Phone2AreaCode: customerDetail.PhoneInfo[1].AreaCode,
            Phone3PhoneNumber: customerDetail.PhoneInfo[2].PhoneNumber,
            Phone3CountryCode: customerDetail.PhoneInfo[2].CountryCode,
            Phone3AreaCode: customerDetail.PhoneInfo[2].AreaCode,
            PassportNumber: customerDetail.Passport.Name,
            MobilePhonePhoneNumber: customerDetail.MobilePhone.PhoneNumber,
            MobilePhoneCountryCode: customerDetail.MobilePhone.CountryCode,
            MobilePhoneAreaCode: customerDetail.MobilePhone.AreaCode,
            FaxNumber: customerDetail.FaxInfo.FaxNumber,
            FaxCountryCode: customerDetail.FaxInfo.CountryCode,
            FaxAreaCode: customerDetail.FaxInfo.AreaCode,
            Email: customerDetail.Email,
            Skype: customerDetail.Skype,
            MSN: customerDetail.MSN,
            PersonalInfoContact1: customerDetail.PersonalInfoContact[0],
            PersonalInfoContact2: customerDetail.PersonalInfoContact[1],
            PersonalInfoContact3: customerDetail.PersonalInfoContact[2],
            PersonalInfoContact4: customerDetail.PersonalInfoContact[3],
            SiteId: customerDetail.SiteSetting.Id,
            HomeAddressLine1: customerDetail.AddressInfo.LineList[0],
            HomeAddressLine2: customerDetail.AddressInfo.LineList[1],
            HomeAddressLine3: customerDetail.AddressInfo.LineList[2],
            HomeAddressCity: customerDetail.AddressInfo.City,
            HomeAddressCountryId: customerDetail.AddressInfo.Country.Id,
            HomeAddressPostCode: customerDetail.AddressInfo.PostCode,
            HomeAddressState: customerDetail.AddressInfo.State,
            BankAddressLine1: customerDetail.BankInfo.AddressInfo.LineList[0],
            BankAddressLine2: customerDetail.BankInfo.AddressInfo.LineList[1],
            BankAddressLine3: customerDetail.BankInfo.AddressInfo.LineList[2],
            BankAddressCity: customerDetail.BankInfo.AddressInfo.City,
            BankAddressPostCode: customerDetail.BankInfo.AddressInfo.PostCode,
            BankAddressState: customerDetail.BankInfo.AddressInfo.State,
            BankAddressCountryId: customerDetail.BankInfo.AddressInfo.Country.Id,
            BankName: customerDetail.BankInfo.Name,
            BankHolderName: customerDetail.BankInfo.HolderName,
            BankAccountNumber: customerDetail.BankInfo.AccountNumber,
            BankSwiftCode: customerDetail.BankInfo.SwiftCode
        };

        $scope.BlockUI();
        $customerDetailService.updateCustomerInfoDetail(updatedCustomerInfoDetailParams)
            .then(function (data) {
                var updateResult = data.Result;
                $scope.showUpdateResult(updateResult);
                $scope.unblockUI();
            },
            function (data) {
                $scope.unblockUI();
            });
    };
    $scope.updateSelectedSite = function (site) {
        if (site == undefined || site == null) {
            $scope.customerDetail.SiteSetting = {
                Id: crmConstants.Enums.Site.NotApply
            };

            return;
        }

        $scope.customerDetail.SiteSetting = site;
    };
    $scope.updateSelectedCountry = function (countryInfo) {
        $scope.customerDetail.Country = countryInfo;
    };
    $scope.updateSelectedCountryBank = function (countryInfo) {
        $scope.customerDetail.BankInfo.AddressInfo.Country = countryInfo;
    };
    $scope.showUpdateResult = function (updateResult) {
        $scope.DOMElements.divResultPopUp.removeClass("display-none");
        $scope.DOMElements.divResultPopUp.html(updateResult.Name);
        $scope.DOMElements.divResultPopUp.dialog({
            width: 250,
            height: 150,
            zIndex: 500,
            resizable: false,
            modal: true,
            position: 'center',
            title: viewResources.Alert,
            close: function (event, ui) {
                /*jshint unused: vars */
                $scope.DOMElements.divResultPopUp.dialog("destroy");
                parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                $scope.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                $scope.DOMElements.textboxFirstName.focus();

                if (updateResult.Id === crmConstants.Enums.Error.None) {
                    parent.leftPanelManager.loadLeftPanel($scope.customerDetail.Name, false, true);
                }
            },
            buttons: [{
                text: viewResources.OK,
                click: function () {
                    $scope.DOMElements.divResultPopUp.dialog("close");
                }
            }]
        });
    };
    $scope.invokeHotKey_CtrlS = function () {
        $scope.updateCustomerInfoDetail();
    };
    angular.element(document).ready(function () {
        $scope.initController();
        $scope.getAllSite();
        $scope.getAllCountry();
        $scope.getAllLanguage();
        hotKeyManager.CachedData.ActiveDocumentManager.push($scope);
    });
});
