"use strict";
/* global customerApplication,
customerManager, 
hotKeyManager */
customerApplication.controller('customerController', function ($scope,
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
        UplineInfo: []
    };

    $scope.isShowCommunicationTool = true;
    $scope.isShowDetail = true;
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
        $customerService.updateCustomerInfo(updatedParams)
            .then(function (data) {
                var updateResult = data.Result;
                customerManager.ShowUpdateResult(updateResult);
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
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
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerInfo"
        });
    });
});