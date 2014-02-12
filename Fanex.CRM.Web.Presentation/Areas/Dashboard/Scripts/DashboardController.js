"use strict";
/* global dashboardApplication,
customerId,
hotKeyManager */
dashboardApplication.controller('dashboardController', function ($scope,
    $http,
    $dashboardService) {
    $scope.customerId = customerId;
    $scope.customerDashboardInfo = null;
    $scope.initController = function () {
        $scope.getCustomerDashboardInfo();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "Dashboard_MainTab"
        });
    };

    $scope.getNumberOfVerifiedInfo = function () {
        if ($scope.customerDashboardInfo) {
            return ($scope.customerDashboardInfo.IsVerifyEmail ? 1 : 0) +
            ($scope.customerDashboardInfo.IsVerifyPassport ? 1 : 0) +
            ($scope.customerDashboardInfo.IsVerifyBirthday ? 1 : 0) +
            ($scope.customerDashboardInfo.IsVerifySecretCode ? 1 : 0) +
            ($scope.customerDashboardInfo.IsVerifyBetCredit ? 1 : 0);
        }

        return 0;
    };

    $scope.getCustomerDashboardInfo = function () {
        $scope.BlockUI();
        var getCustomerDashboardInfoCriteria = {
            CustomerId: $scope.customerId
        };
        $dashboardService.getCustomerDashboardInfo(getCustomerDashboardInfoCriteria)
            .then(function (data) {
                $scope.customerDashboardInfo = data.Result;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
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
    angular.element(document).ready(function () {
        $scope.initController();
    });
});