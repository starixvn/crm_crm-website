"use strict";
/* global customerApplication,
customerManager, 
hotKeyManager */
customerApplication.controller('customerController', ['$scope', '$http', '$customerService',
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
            PersonalInfoContact: [],
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

    $scope.oldCustomer = null;
        $scope.isShowCommunicationTool = true;
        $scope.isShowDetail = true;
    $scope.oldLanguages = '';
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
                    $scope.oldCustomer = angular.copy($scope.customer);
                }

                customerManager.completedGetCustomerInfoByUsername(customer, isHavingData);
            },
            function (data) {
                /* jshint unused: vars */
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
                    $scope.oldCustomer.SiteSetting.Id = parseInt(customerManager.DOMElements.dropdownSite.val());
                    $scope.oldCustomer = angular.copy($scope.customer);
                }

                customerManager.completedGetCustomerInfoByUsername(customer, isHavingData);
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
        };
        $scope.viewCRMInfo = function () {
            var url = '../Customer/EditCRMInfo?' + $.param({
                custid: $scope.customer.Id
            });
            parent.menuManager.goToURL('centerFr', url);
        };
        $scope.addFeedback = function () {
            var url = '../Feedback/AddFeedback?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name
            });
            parent.menuManager.goToURL('centerFr', url);
        };
        $scope.viewLatestFeedback = function () {
            var url = '../Feedback/CustomerLastestFeedback?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name
            });
            parent.menuManager.goToURL('centerFr', url);

        };
        $scope.manageFeedback = function () {
            var url = '../Feedback/ManageFeedback?' + $.param({
                custid: $scope.customer.Id,
                custName: $scope.customer.Name,
                isGuest: 0
            });
            parent.menuManager.goToURL('centerFr', url);
        };
        $scope.updateCustomerInfo = function () {
            $scope.BlockUI();
            var updatedParams = customerManager.getUpdatedCustomerInfoParams($scope.customer);
        $scope.customer.SiteSetting.Id = updatedParams.SiteSettingId;
        $scope.customer.Country.Id = updatedParams.CountryId;

        $scope.oldCustomer.SiteSetting.Id = updatedParams.SiteSettingId;
        $scope.oldCustomer.Country.Id = updatedParams.CountryId;
        $scope.oldLanguages = customerManager.DOMElements.dropdownLanguage.fanexMultiSelect("getItems", true).join(",");
            $customerService.updateCustomerInfo(updatedParams)
            .then(function (data) {
                var updateResult = data.Result;
                customerManager.ShowUpdateResult(updateResult);
                parent.homeManager.CachedData.isChangingData = false;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
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

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlU = function () {
            $scope.updateCustomerInfo();
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlG = function () {
            customerManager.enableGuestSearch();
        };
        angular.element(document).ready(function () {
        $scope.$watch(function ($scope) {
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

                $scope.oldCustomer.PersonalInfoContact[0] !== $scope.customer.PersonalInfoContact[0] ||
                $scope.oldCustomer.PersonalInfoContact[1] !== $scope.customer.PersonalInfoContact[1] ||
                $scope.oldCustomer.PersonalInfoContact[2] !== $scope.customer.PersonalInfoContact[2] ||
                $scope.oldCustomer.PersonalInfoContact[3] !== $scope.customer.PersonalInfoContact[3])) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_EditCustomer_Changes;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }
        });

        customerManager.CacheData.controllerScope = $scope;
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "CustomerInfo"
            });
        });
    } ]);