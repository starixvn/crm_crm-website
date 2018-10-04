(function () {
    "use strict";
    /* global customerManager, 
    hotKeyManager,
    keyboardManager,
    viewResources */
    angular.module('customer-application').controller('customerController', ['$scope', '$http', '$customerService',
    function ($scope,
        $http,
        $customerService) {
        $scope.customer = {
            SiteSetting: {
                Id: 0,
                Name: ""
            },
            Country: {
                Id: 0,
                Name: ""
            },
            FirstName: "",
            LastName: "",
            NickName: "",
            Languages: [],
            PhoneInfo: [],
            MobilePhone: {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            },

            FaxInfo: {
                CountryCode: "",
                AreaCode: "",
                PhoneNumber: ""
            },
            Email: "",
            MSN: "",
            Skype: "",
            PersonalInfoContacts: [],
            BirthDayString: "",
            BankInfo: {
                Address: "",
                SwiftCode: "",
                AccountName: "",
                AccountNumber: ""
            },
            IsVIP: false,
            Question: "",
            Answer: "",
            Remark: "",
            Role: {
                Id: 0,
                Name: ""
            },
            OriginalDangerLevel: 0,
            ABTDangerLevel: 0,
            OtherSportsDangerLevel: 0,
            RefCode: [],
            Remark2: "",
            UplineInfo: [],
            SelectedLanguages: ''
        };
        $scope.TextSettings = {
            Filter: "Search",
            NoItemChecked: "N/A"
        };

        $scope.username = username;

        $scope.oldCustomer = null;
        $scope.isShowCommunicationTool = true;
        $scope.isShowDetail = true;
        $scope.oldLanguages = undefined;
        $scope.isSetFocus = false;
        $scope.searchUplineInfo = function (username) {
            customerManager.updateTextboxUsername(username);
            $scope.getCustomerInfoByUsername(customerManager.getCustomerInfoByUsernameParams());
        };

        $scope.getCustomerInfoByUsername = function (getCustomerInfoParams) {
            $scope.BlockUI();
            $customerService.getCustomerInfoByUsername(getCustomerInfoParams)
        .then(function (data) {
            var customer = data.Result;
            var isHavingData = customerManager.checkCustomerInfoResult(customer);
            if (isHavingData) {
                $scope.customer = customer;
                $scope.oldLanguages = $scope.customer.SelectedLanguages;
                $scope.oldCustomer = angular.copy($scope.customer);
            }
            customerManager.completedGetCustomerInfoByUsername(customer, isHavingData);
        },
        function () {
            $scope.unblockUI();
        });
        };
        $scope.getGuestCustomerInfoByUsername = function (getCustomerInfoParams) {
            $scope.BlockUI();
            $customerService.getCustomerInfoByUsername(getCustomerInfoParams)
        .then(function (data) {
            var customer = data.Result;
            var isHavingData = customerManager.checkGuestCustomerInfoResult(customer);
            if (isHavingData) {
                $scope.customer = customer;
                $scope.oldLanguages = $scope.customer.SelectedLanguages;
                $scope.oldCustomer = angular.copy($scope.customer);
                $scope.changeDirtyFlag($scope);
            }

            customerManager.completedGetCustomerInfoByUsername(customer, isHavingData);
            $scope.unblockUI();
        },
        function () {
            $scope.unblockUI();
        });
        };
        $scope.viewCRMInfo = function () {
            parent.menuManager.getDashboardMenu(parent.dashboardManager.Enums.SubMenuId.Detail);
        };

        $scope.detailLinkClick = function () {
            if (parent.homeManager.CachedData.isChangingData ||
                parent.homeManager.CachedData.isChangingDataLeftPanel) {
                parent.homeManager.showConfirmIgnoreChange($scope.addFeedbackAfterCheckChanging);
                return;
            }

            $scope.addFeedbackAfterCheckChanging();
        };
        $scope.addFeedbackAfterCheckChanging = function () {
            var url = window.rootUrl + 'AddCustomerFeedback/Index?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name
            });
            parent.menuManager.goToURL('centerFr', url);
            parent.menuManager.setActiveClassForLeftPanel('Add Feedback');
        };
        $scope.addFeedback = function () {
            $scope.detailLinkClick();
        };
        $scope.viewLatestFeedback = function () {
            if (parent.homeManager.CachedData.isChangingData ||
                parent.homeManager.CachedData.isChangingDataLeftPanel) {
                parent.homeManager.showConfirmIgnoreChange($scope.viewLatestFeedbackAfterCheckChanging);
                return;
            }

            $scope.viewLatestFeedbackAfterCheckChanging();
        };
        $scope.viewLatestFeedbackAfterCheckChanging = function () {
            var url = '../Feedbacks/LatestFeedback?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name
            });
            parent.menuManager.goToURL('centerFr', url);
            parent.menuManager.setActiveClassForLeftPanel('Latest Feedback');
        };
        $scope.manageFeedback = function () {
            if (parent.homeManager.CachedData.isChangingData ||
                parent.homeManager.CachedData.isChangingDataLeftPanel) {
                parent.homeManager.showConfirmIgnoreChange($scope.manageFeedbackAfterCheckChanging);
                return;
            }

            $scope.manageFeedbackAfterCheckChanging();
        };
        $scope.manageFeedbackAfterCheckChanging = function () {
            var url = '../Feedbacks/ManageFeedback?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name,
                isGuest: $(customerManager.DOMElements.checkboxIsGuest).prop('checked') ? 1 : 0
            });
            parent.menuManager.goToURL('centerFr', url);
            parent.menuManager.setActiveClassForLeftPanel('Manage Feedback');
        };
        $scope.updateCustomerInfo = function () {
            if (parent.homeManager.CachedData.isChangingData === true) {
                parent.homeManager.showConfirmIgnoreChange($scope.updateCustomerInfoAfterPreventUnsavedAction);
                return;
            }

            $scope.updateCustomerInfoAfterPreventUnsavedAction();
        };
        $scope.updateCustomerInfoAfterPreventUnsavedAction = function () {
            $scope.BlockUI();
            var updatedParams = customerManager.getUpdatedCustomerInfoParams($scope.customer);

            $scope.customer.SiteSetting.Id = updatedParams.SiteSettingId;
            $scope.customer.Country.Id = updatedParams.CountryId;
            $scope.oldCustomer = angular.copy($scope.customer);

            $scope.oldLanguages = customerManager.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(",");
            $customerService.updateCustomerInfo(updatedParams)
                .then(function (data) {
                    var updateResult = data.Result;
                    customerManager.ShowUpdateResult(updateResult);
                    parent.homeManager.CachedData.isChangingDataLeftPanel = false;
                    $scope.unblockUI();
                },
                function () {
                    $scope.unblockUI();
                });
        };
        $scope.textBoxKeyUp = function (e) {
            e = (e) ? e : window.event;
            var key = (e.keyCode) ? e.keyCode : e.which;
            if (key === keyboardManager.Enums.KeyCode.Enter) {
                $scope.updateCustomerInfo();
            }

            return;
        };
        $scope.editPersonalInfo = function (textbox) {
            $(textbox).focus();
        };

        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlU = function () {
            $scope.updateCustomerInfo();
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlG = function () {
            customerManager.enableGuestSearch();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };

        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
        };
        $scope.changeDirtyFlag = function ($scope) {
            if ($scope.oldCustomer &&
                    $scope.customer &&
                    ($scope.oldCustomer.SiteSetting.Id !== $scope.customer.SiteSetting.Id ||
                    $scope.oldCustomer.Country.Id !== $scope.customer.Country.Id ||
                    $scope.oldCustomer.FirstName !== $scope.customer.FirstName ||
                    $scope.oldCustomer.LastName !== $scope.customer.LastName ||
                    $scope.oldLanguages !== $scope.customer.SelectedLanguages ||

                    $scope.oldCustomer.PhoneInfo[0].CountryCode !== $scope.customer.PhoneInfo[0].CountryCode ||
                    $scope.oldCustomer.PhoneInfo[0].AreaCode !== $scope.customer.PhoneInfo[0].AreaCode ||
                    $scope.oldCustomer.PhoneInfo[0].PhoneNumber !== $scope.customer.PhoneInfo[0].PhoneNumber ||

                    $scope.oldCustomer.PhoneInfo[1].CountryCode !== $scope.customer.PhoneInfo[1].CountryCode ||
                    $scope.oldCustomer.PhoneInfo[1].AreaCode !== $scope.customer.PhoneInfo[1].AreaCode ||
                    $scope.oldCustomer.PhoneInfo[1].PhoneNumber !== $scope.customer.PhoneInfo[1].PhoneNumber ||

                    $scope.oldCustomer.PhoneInfo[2].CountryCode !== $scope.customer.PhoneInfo[2].CountryCode ||
                    $scope.oldCustomer.PhoneInfo[2].AreaCode !== $scope.customer.PhoneInfo[2].AreaCode ||
                    $scope.oldCustomer.PhoneInfo[2].PhoneNumber !== $scope.customer.PhoneInfo[2].PhoneNumber ||

                    $scope.oldCustomer.MobilePhone.CountryCode !== $scope.customer.MobilePhone.CountryCode ||
                    $scope.oldCustomer.MobilePhone.AreaCode !== $scope.customer.MobilePhone.AreaCode ||
                    $scope.oldCustomer.MobilePhone.PhoneNumber !== $scope.customer.MobilePhone.PhoneNumber ||

                    $scope.oldCustomer.Email !== $scope.customer.Email ||
                    $scope.oldCustomer.MSN !== $scope.customer.MSN ||
                    $scope.oldCustomer.Skype !== $scope.customer.Skype ||

                    $scope.oldCustomer.PersonalInfoContacts[0].Phone !== $scope.customer.PersonalInfoContacts[0].Phone ||
                    $scope.oldCustomer.PersonalInfoContacts[0].Name !== $scope.customer.PersonalInfoContacts[0].Name ||
                    $scope.oldCustomer.PersonalInfoContacts[1].Phone !== $scope.customer.PersonalInfoContacts[1].Phone ||
                    $scope.oldCustomer.PersonalInfoContacts[1].Name !== $scope.customer.PersonalInfoContacts[1].Name ||
                    $scope.oldCustomer.PersonalInfoContacts[2].Phone !== $scope.customer.PersonalInfoContacts[2].Phone ||
                    $scope.oldCustomer.PersonalInfoContacts[2].Name !== $scope.customer.PersonalInfoContacts[2].Name ||
                    $scope.oldCustomer.PersonalInfoContacts[3].Phone !== $scope.customer.PersonalInfoContacts[3].Phone ||
                    $scope.oldCustomer.PersonalInfoContacts[3].Name !== $scope.customer.PersonalInfoContacts[3].Name)) {
                parent.homeManager.CachedData.isChangingDataLeftPanel = true;
                parent.homeManager.CachedData.ignoreConfirmMessageLeftPanel = viewResources.Confirm_Ignore_EditCustomer_ChangesLeftPanel;
            } else {
                parent.homeManager.CachedData.isChangingDataLeftPanel = false;
            }
        };

        angular.element(document).ready(function () {
            $scope.$watch(function ($scope) {
                $scope.changeDirtyFlag($scope);
            });

            $scope.setFocus();
            customerManager.CacheData.controllerScope = $scope;
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "CustomerInfo"
            });
        });
    } ]);
})();