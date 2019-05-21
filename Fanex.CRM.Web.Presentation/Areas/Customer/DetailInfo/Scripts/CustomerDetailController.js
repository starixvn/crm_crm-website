(function () {
    "use strict";
    /* global 
    customerDetail,
    crmConstants,
    viewResources,
    hotKeyManager,
    sharedScriptManager,
    moment */
    /* jshint camelcase: false */
    angular.module('customerdetail-application').controller('customerDetailController', ['$scope', '$http', '$customerDetailService',
    function ($scope, $http, $customerDetailService) {
        $scope.customerDetail = customerDetail;
        $scope.canEditHeadline = canEditHeadline;
        $scope.oldCustomerDetail = angular.copy($scope.customerDetail);
        $scope.siteList = null;
        $scope.countryList = null;
        $scope.provinceList = null;
        $scope.languageList = null;
        $scope.titleList = null;
        $scope.licenseeTypes = [];
        $scope.isCN88 = isCN88;

        $scope.isLoadedAllSettings = false;
        $scope.isSetFocus = false;

        $scope.isShowPersonalInfo = true;
        $scope.isShowHeadlineInfo = true;
        $scope.isShowCommunicationInfo = true;
        $scope.isShowHomeAddressInfo = true;
        $scope.isShowBankInfoInfo = true;
        $scope.selectedLangueList = '';
        $scope.oldSelectedLangueList = '';
        $scope.TextSettings = {
            Filter: "Search",
            NoItemChecked: "N/A"
        };

        $scope.DOMElements = {
            divResultPopUp: null,
            textboxFirstName: null
        };
        $scope.initController = function () {
            $scope.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
            $scope.DOMElements.textboxFirstName = $("#firstName");
        };

        $scope.getAllSettings = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSettings = false;
            $customerDetailService.getAllSettings()
            .then(function (data) {
                $scope.siteList = data.Result.Sites;
                $scope.titleList = data.Result.Titles;
                $scope.countryList = data.Result.Countries;
                $scope.provinceList = data.Result.Provinces;                
                $scope.languageList = data.Result.Languages;
                $scope.licenseeTypes = data.Result.LicenseeTypes;
                $scope.isLoadedAllSettings = true;

                $scope.selectedSite = $scope.customerDetail.SiteSetting;
                $scope.siteDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: data.Result.Sites,
                    childrenMember: "ChildSites"
                };

                $scope.completedLoadDetailInfo();
            },
            function () {
                $scope.isLoadedAllSettings = true;
                $scope.completedLoadDetailInfo();
            });
        };
        $scope.completedLoadDetailInfo = function () {
            if ($scope.isLoadedAllSettings) {
                $scope.unblockUI();
            }
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
        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };

        $scope.updateCustomerInfoDetail = function () {
            if (parent.homeManager.CachedData.isChangingDataLeftPanel === true) {
                parent.homeManager.showConfirmIgnoreChange($scope.updateCustomerInfoDetailAfterPreventUnsavedAction);
                return;
            }

            $scope.updateCustomerInfoDetailAfterPreventUnsavedAction();
        };
        $scope.updateCustomerInfoDetailAfterPreventUnsavedAction = function () {
            var customerDetail = $scope.customerDetail;
            var updatedCustomerInfoDetailParams = {
                CustomerId: customerDetail.Id,
                FirstName: customerDetail.FirstName,
                LastName: customerDetail.LastName,
                LanguageId: $scope.selectedLangueList,
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
                WeChat: customerDetail.WeChat,
                WhatsApp: customerDetail.WhatsApp,
                Line: customerDetail.Line,
                Yahoo: customerDetail.Yahoo,
                QQ: customerDetail.QQ,
                BBM: customerDetail.BBM,
                PersonalInfoContactName1: customerDetail.PersonalInfoContacts[0].Name,
                PersonalInfoContact1: customerDetail.PersonalInfoContacts[0].Phone,
                PersonalInfoContactName2: customerDetail.PersonalInfoContacts[1].Name,
                PersonalInfoContact2: customerDetail.PersonalInfoContacts[1].Phone,
                PersonalInfoContactName3: customerDetail.PersonalInfoContacts[2].Name,
                PersonalInfoContact3: customerDetail.PersonalInfoContacts[2].Phone,
                PersonalInfoContactName4: customerDetail.PersonalInfoContacts[3].Name,
                PersonalInfoContact4: customerDetail.PersonalInfoContacts[3].Phone,
                SiteId: $scope.selectedSite ? $scope.selectedSite.Id : crmConstants.Enums.Site.NotApply,
                HomeAddressLine1: customerDetail.AddressInfo.LineList[0],
                HomeAddressLine2: customerDetail.AddressInfo.LineList[1],
                HomeAddressLine3: customerDetail.AddressInfo.LineList[2],
                HomeAddressCity: customerDetail.AddressInfo.City,
                HomeAddressCountryId: customerDetail.AddressInfo.Country.Id,
                ProvinceID: $scope.isCN88 ? customerDetail.AddressInfo.Province.Id : 0,
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
                BankSwiftCode: customerDetail.BankInfo.SwiftCode,

                TitleId: customerDetail.Title.Id,
                BirthDay: customerDetail.BirthDayString ?
                moment(customerDetail.BirthDayString).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment) :
                '',
                BettingCode: customerDetail.BettingCode[0],
                RefCode2: customerDetail.BettingCode[1],
                IsVIP: customerDetail.IsVIP,
                IsIgnored: customerDetail.IsIgnored,
                LicenseeTypeId: customerDetail.LicenseeType.Id,
                Checker: customerDetail.Checker,
                IntroducedBy: customerDetail.IntroducedBy
            };

            $scope.BlockUI();

            $scope.customerDetail.Country.Id = updatedCustomerInfoDetailParams.HomeAddressCountryId;
            parent.homeManager.CachedData.isChangingData = false;
            $customerDetailService.updateCustomerInfoDetail(updatedCustomerInfoDetailParams)
            .then(function (data) {
                var updateResult = data.Result;
                $scope.showUpdateResult(updateResult);
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };
        $scope.updateSelectedSite = function (siteId) {
            siteId = parseInt(siteId);
            var siteInfo;
            var siteGroupLength = $scope.siteList.length;
            for (var i = 0; i < siteGroupLength; i++) {
                var siteGroup = $scope.siteList[i];
                var siteLength = siteGroup.ChildSites.length;
                for (var j = 0; j < siteLength; j++) {
                    var site = siteGroup.ChildSites[j];
                    if (site.Id === siteId) {
                        siteInfo = site;
                        break;
                    }
                }

                if (siteInfo) {
                    break;
                }
            }

            if (siteInfo) {
                $scope.customerDetail.SiteSetting = siteInfo;
            } else {
                $scope.customerDetail.SiteSetting = {
                    Id: crmConstants.Enums.Site.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.updateSelectedCountry = function (countryInfoId) {
            countryInfoId = parseInt(countryInfoId);
            var countryInfo;
            var countryListLength = $scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = $scope.countryList[i];
                if (country.Id === countryInfoId) {
                    countryInfo = country;
                    break;
                }
            }

            if (countryInfo) {
                $scope.customerDetail.AddressInfo.Country = countryInfo;
            } else {
                $scope.customerDetail.AddressInfo.Country = {
                    Id: crmConstants.Enums.Country.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.updateSelectedProvince = function (provinceInfoId) {
            provinceInfoId = parseInt(provinceInfoId);
            var provinceInfo;
            var provinceListLength = $scope.provinceList.length;
            for (var i = 0; i < provinceListLength; i++) {
                var province = $scope.provinceList[i];
                if (province.Id === provinceInfoId) {
                    provinceInfo = province;
                    break;
                }
            }

            if (provinceInfo) {
                $scope.customerDetail.AddressInfo.Province = provinceInfo;
            } else {
                $scope.customerDetail.AddressInfo.Province = {
                    Id: crmConstants.Enums.Country.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.updateSelectedCountryBank = function (countryInfoId) {
            countryInfoId = parseInt(countryInfoId);
            var countryInfo;
            var countryListLength = $scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = $scope.countryList[i];
                if (country.Id === countryInfoId) {
                    countryInfo = country;
                    break;
                }
            }

            if (countryInfo) {
                $scope.customerDetail.BankInfo.AddressInfo.Country = countryInfo;
            } else {
                $scope.customerDetail.BankInfo.AddressInfo.Country = {
                    Id: crmConstants.Enums.Country.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.showUpdateResult = function (updateResult) {
            updateResult = sharedScriptManager.formatAjaxResult(updateResult);
            $scope.DOMElements.divResultPopUp.removeClass("display-none");
            $scope.DOMElements.divResultPopUp.html(updateResult.Name || updateResult.Message);
            $scope.DOMElements.divResultPopUp.dialog({
                width: 250,
                height: 150,
                zIndex: 500,
                resizable: false,
                modal: true,
                // position: 'center',
                title: viewResources.Alert,
                close: function (event, ui) {
                    /* jshint unused: false */
                    $scope.DOMElements.divResultPopUp.dialog("destroy");
                    parent.$(document).append('<div id="div-resultpopup" class="selector display-none"></div>');
                    $scope.DOMElements.divResultPopUp = parent.$("#div-resultpopup");
                    $scope.DOMElements.textboxFirstName.focus();

                    if (updateResult.Id === crmConstants.Enums.Error.None &&
                    $scope.shouldReloadLeftPanel()) {
                        parent.leftPanelManager.loadLeftPanel($scope.customerDetail.Name, false, true);
                    }

                    $scope.oldSelectedLangueList = $scope.selectedLangueList;
                    $scope.oldCustomerDetail = angular.copy($scope.customerDetail);
                    $scope.updateIsChangeCustomerInfoFlag();
                    $scope.setFocus();
                    $scope.$apply();
                },
                buttons: [{
                    text: viewResources.OK,
                    click: function () {
                        $scope.DOMElements.divResultPopUp.dialog("close");
                    }
                }]
            });

            $scope.setFocus();
            if (parseInt(updateResult.Id) === crmConstants.Enums.Error.None) {
                setTimeout(function () {
                    $scope.DOMElements.divResultPopUp.dialog("close");
                }, 3000);
            }
        };
        $scope.shouldReloadLeftPanel = function () {
            if ($scope.customerDetail &&
            $scope.oldCustomerDetail &&
            ($scope.oldCustomerDetail.FirstName !== $scope.customerDetail.FirstName ||
             $scope.oldCustomerDetail.LastName !== $scope.customerDetail.LastName ||
             $scope.oldCustomerDetail.Country.Id !== $scope.customerDetail.Country.Id ||
             $scope.oldSelectedLangueList !== $scope.selectedLangueList ||

            $scope.oldCustomerDetail.PhoneInfo[0].CountryCode !== $scope.customerDetail.PhoneInfo[0].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[0].AreaCode !== $scope.customerDetail.PhoneInfo[0].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[0].PhoneNumber !== $scope.customerDetail.PhoneInfo[0].PhoneNumber ||

            $scope.oldCustomerDetail.PhoneInfo[1].CountryCode !== $scope.customerDetail.PhoneInfo[1].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[1].AreaCode !== $scope.customerDetail.PhoneInfo[1].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[1].PhoneNumber !== $scope.customerDetail.PhoneInfo[1].PhoneNumber ||

            $scope.oldCustomerDetail.PhoneInfo[2].CountryCode !== $scope.customerDetail.PhoneInfo[2].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[2].AreaCode !== $scope.customerDetail.PhoneInfo[2].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[2].PhoneNumber !== $scope.customerDetail.PhoneInfo[2].PhoneNumber ||

            $scope.oldCustomerDetail.MobilePhone.CountryCode !== $scope.customerDetail.MobilePhone.CountryCode ||
            $scope.oldCustomerDetail.MobilePhone.AreaCode !== $scope.customerDetail.MobilePhone.AreaCode ||
            $scope.oldCustomerDetail.MobilePhone.PhoneNumber !== $scope.customerDetail.MobilePhone.PhoneNumber ||

            $scope.oldCustomerDetail.Email !== $scope.customerDetail.Email ||
            $scope.oldCustomerDetail.MSN !== $scope.customerDetail.MSN ||
            $scope.oldCustomerDetail.Skype !== $scope.customerDetail.Skype ||

            $scope.oldCustomerDetail.PersonalInfoContacts[0].Name !== $scope.customerDetail.PersonalInfoContacts[0].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[0].Phone !== $scope.customerDetail.PersonalInfoContacts[0].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[1].Name !== $scope.customerDetail.PersonalInfoContacts[1].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[1].Phone !== $scope.customerDetail.PersonalInfoContacts[1].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[2].Name !== $scope.customerDetail.PersonalInfoContacts[2].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[2].Phone !== $scope.customerDetail.PersonalInfoContacts[2].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[3].Name !== $scope.customerDetail.PersonalInfoContacts[3].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[3].Phone !== $scope.customerDetail.PersonalInfoContacts[3].Phone ||
            $scope.oldCustomerDetail.SiteSetting.Id !== $scope.customerDetail.SiteSetting.Id)) {
                return true;
            }

            return false;
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateCustomerInfoDetail();
        };
        $scope.updateSelectedLanguage = function (selectedLangueList) {
            $scope.selectedLangueList = selectedLangueList;
        };
        $scope.updateOldSelectedLanguage = function (selectedLangueList) {
            $scope.oldSelectedLangueList = selectedLangueList;
        };
        $scope.updateIsChangeCustomerInfoFlag = function () {
            if ($scope.customerDetail &&
            $scope.oldCustomerDetail &&
            ($scope.oldCustomerDetail.FirstName !== $scope.customerDetail.FirstName ||
             $scope.oldCustomerDetail.LastName !== $scope.customerDetail.LastName ||
             $scope.oldSelectedLangueList !== $scope.selectedLangueList ||

            $scope.oldCustomerDetail.PhoneInfo[0].CountryCode !== $scope.customerDetail.PhoneInfo[0].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[0].AreaCode !== $scope.customerDetail.PhoneInfo[0].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[0].PhoneNumber !== $scope.customerDetail.PhoneInfo[0].PhoneNumber ||

            $scope.oldCustomerDetail.PhoneInfo[1].CountryCode !== $scope.customerDetail.PhoneInfo[1].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[1].AreaCode !== $scope.customerDetail.PhoneInfo[1].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[1].PhoneNumber !== $scope.customerDetail.PhoneInfo[1].PhoneNumber ||

            $scope.oldCustomerDetail.PhoneInfo[2].CountryCode !== $scope.customerDetail.PhoneInfo[2].CountryCode ||
            $scope.oldCustomerDetail.PhoneInfo[2].AreaCode !== $scope.customerDetail.PhoneInfo[2].AreaCode ||
            $scope.oldCustomerDetail.PhoneInfo[2].PhoneNumber !== $scope.customerDetail.PhoneInfo[2].PhoneNumber ||

            $scope.oldCustomerDetail.Passport.Name !== $scope.customerDetail.Passport.Name ||
            $scope.oldCustomerDetail.MobilePhone.CountryCode !== $scope.customerDetail.MobilePhone.CountryCode ||
            $scope.oldCustomerDetail.MobilePhone.AreaCode !== $scope.customerDetail.MobilePhone.AreaCode ||
            $scope.oldCustomerDetail.MobilePhone.PhoneNumber !== $scope.customerDetail.MobilePhone.PhoneNumber ||
            $scope.oldCustomerDetail.FaxInfo.CountryCode !== $scope.customerDetail.FaxInfo.CountryCode ||
            $scope.oldCustomerDetail.FaxInfo.AreaCode !== $scope.customerDetail.FaxInfo.AreaCode ||
            $scope.oldCustomerDetail.FaxInfo.PhoneNumber !== $scope.customerDetail.FaxInfo.PhoneNumber ||

            $scope.oldCustomerDetail.Email !== $scope.customerDetail.Email ||
            $scope.oldCustomerDetail.MSN !== $scope.customerDetail.MSN ||
            $scope.oldCustomerDetail.Skype !== $scope.customerDetail.Skype ||
            $scope.oldCustomerDetail.WeChat !== $scope.customerDetail.WeChat ||
            $scope.oldCustomerDetail.WhatsApp !== $scope.customerDetail.WhatsApp ||
            $scope.oldCustomerDetail.Line !== $scope.customerDetail.Line ||
            $scope.oldCustomerDetail.Yahoo !== $scope.customerDetail.Yahoo ||
            $scope.oldCustomerDetail.QQ !== $scope.customerDetail.QQ ||
            $scope.oldCustomerDetail.BBM !== $scope.customerDetail.BBM ||

            $scope.oldCustomerDetail.PersonalInfoContacts[0].Name !== $scope.customerDetail.PersonalInfoContacts[0].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[0].Phone !== $scope.customerDetail.PersonalInfoContacts[0].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[1].Name !== $scope.customerDetail.PersonalInfoContacts[1].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[1].Phone !== $scope.customerDetail.PersonalInfoContacts[1].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[2].Name !== $scope.customerDetail.PersonalInfoContacts[2].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[2].Phone !== $scope.customerDetail.PersonalInfoContacts[2].Phone ||
            $scope.oldCustomerDetail.PersonalInfoContacts[3].Name !== $scope.customerDetail.PersonalInfoContacts[3].Name ||
            $scope.oldCustomerDetail.PersonalInfoContacts[3].Phone !== $scope.customerDetail.PersonalInfoContacts[3].Phone ||

            $scope.oldCustomerDetail.SiteSetting.Id !== $scope.customerDetail.SiteSetting.Id ||
            $scope.oldCustomerDetail.AddressInfo.LineList[0] !== $scope.customerDetail.AddressInfo.LineList[0] ||
            $scope.oldCustomerDetail.AddressInfo.LineList[1] !== $scope.customerDetail.AddressInfo.LineList[1] ||
            $scope.oldCustomerDetail.AddressInfo.LineList[2] !== $scope.customerDetail.AddressInfo.LineList[2] ||
            $scope.oldCustomerDetail.AddressInfo.City !== $scope.customerDetail.AddressInfo.City ||
            $scope.oldCustomerDetail.AddressInfo.Country.Id !== $scope.customerDetail.AddressInfo.Country.Id ||
            $scope.oldCustomerDetail.AddressInfo.PostCode !== $scope.customerDetail.AddressInfo.PostCode ||
            $scope.oldCustomerDetail.AddressInfo.State !== $scope.customerDetail.AddressInfo.State ||

            $scope.oldCustomerDetail.BankInfo.HolderName !== $scope.customerDetail.BankInfo.HolderName ||
            $scope.oldCustomerDetail.BankInfo.AccountNumber !== $scope.customerDetail.BankInfo.AccountNumber ||
            $scope.oldCustomerDetail.BankInfo.Name !== $scope.customerDetail.BankInfo.Name ||
            $scope.oldCustomerDetail.BankInfo.SwiftCode !== $scope.customerDetail.BankInfo.SwiftCode ||

            $scope.oldCustomerDetail.BankInfo.AddressInfo.LineList[0] !== $scope.customerDetail.BankInfo.AddressInfo.LineList[0] ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.LineList[1] !== $scope.customerDetail.BankInfo.AddressInfo.LineList[1] ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.LineList[2] !== $scope.customerDetail.BankInfo.AddressInfo.LineList[2] ||

            $scope.oldCustomerDetail.BankInfo.AddressInfo.City !== $scope.customerDetail.BankInfo.AddressInfo.City ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.Country.Id !== $scope.customerDetail.BankInfo.AddressInfo.Country.Id ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.PostCode !== $scope.customerDetail.BankInfo.AddressInfo.PostCode ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.State !== $scope.customerDetail.BankInfo.AddressInfo.State ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.Name !== $scope.customerDetail.BankInfo.AddressInfo.Name ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.HolderName !== $scope.customerDetail.BankInfo.AddressInfo.HolderName ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.AccountNumber !== $scope.customerDetail.BankInfo.AddressInfo.AccountNumber ||
            $scope.oldCustomerDetail.BankInfo.AddressInfo.SwiftCode !== $scope.customerDetail.BankInfo.AddressInfo.SwiftCode ||
            !sharedScriptManager.datesEqual(new Date($scope.oldCustomerDetail.BirthDayString), new Date($scope.customerDetail.BirthDayString)) ||
            $scope.oldCustomerDetail.IsVIP !== $scope.customerDetail.IsVIP ||
            $scope.oldCustomerDetail.IsIgnored !== $scope.customerDetail.IsIgnored ||
            $scope.oldCustomerDetail.Checker !== $scope.customerDetail.Checker ||
            $scope.oldCustomerDetail.BettingCode[0] !== $scope.customerDetail.BettingCode[0] ||
            $scope.oldCustomerDetail.BettingCode[1] !== $scope.customerDetail.BettingCode[1] ||
            $scope.oldCustomerDetail.Title.Id !== $scope.customerDetail.Title.Id ||
            $scope.oldCustomerDetail.LicenseeType.Id !== $scope.customerDetail.LicenseeType.Id)) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_EditCustomer_ChangesTabDetail;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }
        };
        $scope.updateSelectedDate = function (selectedDate) {
            $scope.customerDetail.BirthDayString = selectedDate;
        };
        $scope.updateSelectedTitle = function (titleId) {
            titleId = parseInt(titleId);
            var titleInfo;
            var titleListLength = $scope.titleList.length;
            for (var i = 0; i < titleListLength; i++) {
                var title = $scope.titleList[i];
                if (title.Id === titleId) {
                    titleInfo = title;
                    break;
                }
            }

            if (titleInfo) {
                $scope.customerDetail.Title = titleInfo;
            } else {
                $scope.customerDetail.Title = {
                    Id: crmConstants.Enums.Title.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.setFocus = function () {
            $scope.isSetFocus = true;
        };
        angular.element(document).ready(function () {
            $scope.initController();
            $scope.setFocus();
            $scope.$apply();
            $scope.getAllSettings();

            $scope.$watch(function ($scope) {
                $scope.updateIsChangeCustomerInfoFlag();
            });
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "CustomerInfoDetail"
            });
        });
    } ]);
})();
