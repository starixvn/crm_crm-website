"use strict";
/* global redemptionViewLogApplication, 
customerId, 
hotKeyManager,
viewResources,
currentDate,
crmConstants */
redemptionViewLogApplication.controller('redemptionViewLogController', function ($scope,
    $http,
    $redemptionViewLogService) {
    $scope.customerId = customerId;
    $scope.searchCriteria = {
        CustomerId: customerId,
        FromDate: new Date(currentDate),
        ToDate: new Date(currentDate),
        Status: null,
        MinToDate: new Date(currentDate),
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
        $scope.searchCriteria.Status = $scope.statusList[0];
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