"use strict";
/* global communicationApplication, 
communicationLogs, 
hotKeyManager,
viewResources,
crmConstants */
communicationApplication.controller('communicationController', ['$scope', '$http', '$communicationService',
function (
    $scope,
    $http,
    $communicationService) {
    $scope.logs = communicationLogs ? communicationLogs.Logs : null;
    $scope.isSuccess = false;
    $scope.isShowResultMessage = false;
    $scope.resultMessage = '';
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
        if (!parent.homeManager.CachedData.shouldReloadView) {
            return;
        }

        var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
        $scope.isShowResultMessage = true;
        $scope.isSuccess = popUpResult.Code === crmConstants.Enums.Error.None;
        $scope.resultMessage = popUpResult.Message;
        window.scroll(1, 1);
        if ($scope.isSuccess) {
            setTimeout(function () {
                $scope.isShowResultMessage = false;
                $scope.$apply();
            }, 3000);
        }

        $scope.$apply();
        $scope.getLogsByCustomerId();
    };
    $scope.closeInsertView = function () {
        if (!parent.homeManager.CachedData.shouldReloadView) {
            return;
        }

        var popUpResult = parent.homeManager.CachedData.shareInfoBetweenPopup;
        $scope.isShowResultMessage = true;
        $scope.isSuccess = popUpResult.Code === crmConstants.Enums.Error.None;
        $scope.resultMessage = popUpResult.Message;
        window.scroll(1, 1);
        if ($scope.isSuccess) {
            setTimeout(function () {
                $scope.isShowResultMessage = false;
                $scope.$apply();
            }, 3000);
        }

        $scope.$apply();
        $scope.getLogsByCustomerId();
    };
    $scope.beforeCloseInsertView = function () {
        if (parent.homeManager.CachedData.isChangingDataPopUp &&
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
            var insertLogController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
            parent.homeManager.CachedData.isIgnoreChangePopUp = false;
            parent.homeManager.showConfirmIgnoreChangePopUp(insertLogController.ignoreChanges);
            return false;
        }

        return true;
    };
    $scope.beforeCloseEditView = function () {
        if (parent.homeManager.CachedData.isChangingDataPopUp &&
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
            var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
            parent.homeManager.CachedData.isIgnoreChangePopUp = false;
            parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges);
            return false;
        }

        return true;
    };

    $scope.editCommunicationLog = function (logIndex) {
        parent.homeManager.CachedData.shouldReloadView = false;
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
            $scope.closeEditView,
            null,
            $scope.beforeCloseEditView);
    };
    $scope.insertCommunicationLog = function () {
        parent.homeManager.CachedData.shouldReloadView = false;
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
            $scope.closeInsertView,
            null,
            $scope.beforeCloseInsertView);
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
        $(document).focus();
    });
} ]);