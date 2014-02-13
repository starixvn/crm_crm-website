"use strict";
/* global dashboardApplication,
customerId,
hotKeyManager,
viewResources */
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

    $scope.closeEditRedemptionLogView = function () {
        if (parent.shouldReloadLogs) {
            $scope.getCustomerDashboardInfo();
        }
    };
    $scope.editRedemptionLog = function (logId) {
        parent.shouldReloadLogs = false;
        /* jshint camelcase: false */
        parent.OpenPopup(
            window.rootUrl + "Redemption/OpenEditView?" + $.param({
                logId: logId
            }),
            viewResources.Edit_Redemption_Log,
            $(parent.document).width() - 600,
            500,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeEditRedemptionLogView,
            ['center', 120]);
    };
    $scope.closeEditCommunicationLogView = function () {
        if (parent.shouldReloadLogs) {
            $scope.getCustomerDashboardInfo();
        }
    };
    $scope.editCommunicationLog = function (logId) {
        parent.shouldReloadLogs = false;
        var url = window.rootUrl + "CommunicationLog/OpenEditView?" + $.param({
            logRunningNumber: logId
        });
        /* jshint camelcase: false */
        parent.OpenPopup(
            url,
            viewResources.Edit_Communication_Log,
            $(parent.document).width() - 300,
            $(parent.document).height() - 40,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeEditCommunicationLogView);
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