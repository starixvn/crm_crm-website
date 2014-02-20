"use strict";
/* global communicationApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate,
customerId,
initFromDate */
communicationApplication.controller('communicationController', ['$scope', '$http', '$communicationService',
function (
    $scope,
    $http,
    $communicationService) {
    $scope.isSetFocus = false;
    $scope.logs = null;
    $scope.isSuccess = false;
    $scope.isShowResultMessage = false;
    $scope.resultMessage = '';
    $scope.customerId = customerId;
    $scope.statusList = [];
    $scope.isUpdatingDate = false;
    $scope.isSearched = true;
    $scope.searchCriteria = {
        CustomerId: $scope.customerId,
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        Status: null,
        MinToDate: new Date(initFromDate),
        MaxFromDate: new Date(currentDate),
        Handler: ''
    };

    $scope.initController = function () {
        $scope.getAllStatus();
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
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
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
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
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
            parent.homeManager.showConfirmIgnoreChangePopUp(insertLogController.ignoreChanges,
                insertLogController.setFocus);
            return false;
        }

        return true;
    };
    $scope.beforeCloseEditView = function () {
        if (parent.homeManager.CachedData.isChangingDataPopUp &&
            parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController) {
            var popUpController = parent.homeManager.CachedData.shareInfoBetweenPopup.PopupController;
            parent.homeManager.CachedData.isIgnoreChangePopUp = false;
            parent.homeManager.showConfirmIgnoreChangePopUp(popUpController.ignoreChanges,
                popUpController.setFocus);
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
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.isSetFocus = true;
        });
    };
    $scope.getAllStatus = function () {
        $scope.BlockUI();
        $communicationService.getAllStatus()
            .then(function (data) {
                $scope.statusList = data.Result;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.completedLoadFilterInfo = function () {
        $scope.searchCriteria.Status = {
            Id: crmConstants.Enums.Status.All
        };
        $scope.unblockUI();
        $scope.searchCommunicationLogs();
    };
    $scope.updateSelectedStatus = function (statusId) {
        statusId = parseInt(statusId);
        var statusLength = $scope.statusList.length;
        var selectedStatus = null;
        for (var i = 0; i < statusLength; i++) {
            var status = $scope.statusList[i];
            if (status.Id === statusId) {
                selectedStatus = status;
                break;
            }
        }

        if (selectedStatus) {
            $scope.searchCriteria.Status = selectedStatus;
        }
        else {
            $scope.searchCriteria.Status = {
                Id: crmConstants.Enums.Status.All
            };
        }
    };
    $scope.updateFromDate = function (selectedDate) {
        $scope.searchCriteria.FromDate = selectedDate;
        $scope.searchCriteria.MinToDate = selectedDate;
    };
    $scope.searchCommunicationLogs = function () {
        $scope.BlockUI();
        var searchLogsCriteria = {
            CustomerId: $scope.searchCriteria.CustomerId,
            FromDate: $scope.searchCriteria.FromDate,
            ToDate: $scope.searchCriteria.ToDate,
            StatusId: $scope.searchCriteria.Status.Id,
            Handler: $scope.searchCriteria.Handler
        };

        $communicationService.getLogsByCustomerId(searchLogsCriteria)
            .then(function (data) {
                $scope.logs = data.Result.Logs;
                $scope.isSearched = true;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.isSearched = true;
                $scope.unblockUI();
            });
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.searchCommunicationLogs();
    };
    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.searchCriteria.MaxFromDate = selectedDate;
    };
    angular.element(document).ready(function () {
        $scope.initController();
        $scope.setFocus();
    });
} ]);