"use strict";
/* global redemptionViewLogApplication, 
customerId, 
hotKeyManager,
viewResources,
currentDate,
crmConstants,
initFromDate */
redemptionViewLogApplication.controller('redemptionViewLogController', function ($scope,
    $http,
    $redemptionViewLogService) {
    $scope.customerId = customerId;
    $scope.isSearched = false;
    $scope.searchCriteria = {
        CustomerId: customerId,
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        Status: null,
        MinToDate: new Date(initFromDate),
        MaxFromDate: new Date(currentDate)
    };

    $scope.logs = [];
    $scope.statusList = [];
    $scope.isUpdatingDate = false;

    $scope.initController = function () {
        $scope.getAllStatus();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs"
        });
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlI = function () {
        $scope.insertLog();
    };
    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.searchLogs();
    };

    $scope.getAllStatus = function () {
        $scope.BlockUI();
        $redemptionViewLogService.getAllStatus()
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
    };
    $scope.updateSelectedStatus = function (status) {
        if (status) {
            $scope.searchCriteria.Status = status;
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
    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.searchCriteria.MaxFromDate = selectedDate;
    };
    $scope.searchLogs = function () {
        $scope.BlockUI();
        var searchLogsCriteria = {
            CustomerId: $scope.searchCriteria.CustomerId,
            FromDate: $scope.searchCriteria.FromDate,
            ToDate: $scope.searchCriteria.ToDate,
            StatusId: $scope.searchCriteria.Status.Id
        };
        $redemptionViewLogService.searchLogs(searchLogsCriteria)
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
    $scope.closeInsertView = function () {
        if (parent.shouldReloadLogs && $scope.isSearched) {
            $scope.searchLogs();
        }
    };
    $scope.closeEditView = function () {
        if (parent.shouldReloadLogs) {
            $scope.searchLogs();
        }
    };
    $scope.insertLog = function () {
        parent.shouldReloadLogs = false;
        parent.OpenPopup(
            window.rootUrl + "Redemption/OpenInsertView?" + $.param({ customerId: $scope.customerId }),
            viewResources.Insert_Redemption_Log,
            $(parent.document).width() - 400,
            400,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeInsertView);
    };
    $scope.editLog = function (logId) {
        parent.shouldReloadLogs = false;
        parent.OpenPopup(
            window.rootUrl + "Redemption/OpenEditView?" + $.param({
                logId: logId
            }),
            viewResources.Edit_Redemption_Log,
            $(parent.document).width() - 400,
            400,
            true,
            false,
            parent.window,
            3000,
            null,
            $scope.closeEditView);
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