"use strict";
/* global communicationApplication, 
communicationLogs, 
hotKeyManager,
viewResources */
communicationApplication.controller('communicationController', ['$scope', '$http', '$communicationService',
function (
    $scope,
    $http,
    $communicationService) {
    $scope.logs = communicationLogs ? communicationLogs.Logs : null;
    $scope.customerId = communicationLogs.CustomerId;
    $scope.initController = function () {
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs"
        });
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlI = function () {
        $scope.insertCommunicationLog();
    };
    $scope.getLogsByCustomerId = function () {
        $scope.BlockUI();
        var getLogByCustomerIdParams = {
            CustomerId: $scope.customerId
        };

        $communicationService.getLogsByCustomerId(getLogByCustomerIdParams)
            .then(function (data) {
                $scope.logs = data.Result.Logs;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.isLoadedAllSite = true;
                $scope.unblockUI();
            });
    };
    $scope.closeEditView = function () {
        if (parent.shouldReloadLogs) {
            $scope.getLogsByCustomerId();
        }
    };
    $scope.closeInsertView = function () {
        if (parent.shouldReloadLogs) {
            $scope.getLogsByCustomerId();
        }
    };
    $scope.editCommunicationLog = function (logIndex) {
        parent.shouldReloadLogs = false;
        var url = window.rootUrl + "CommunicationLog/OpenEditView?" + $.param({
            logRunningNumber: $scope.logs[logIndex].RunningNumber
        });
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
            $scope.closeEditView);
    };
    $scope.insertCommunicationLog = function () {
        parent.shouldReloadLogs = false;
        parent.OpenPopup(
            window.rootUrl + "CommunicationLog/OpenInsertView?" + $.param({ customerId: $scope.customerId }),
            viewResources.Insert_Communication_Log,
            $(parent.document).width() - 300,
            $(parent.document).height() - 40,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeInsertView);
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
}]);