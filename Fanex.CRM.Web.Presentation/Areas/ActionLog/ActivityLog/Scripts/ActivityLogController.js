"use strict";
/* global activityLogApplication, 
customerId, 
hotKeyManager,
sharedScriptManager,
activityLogResources,
currentDate,
crmConstants,
initFromDate,
moment */
activityLogApplication.controller('activityLogController', ['$scope', '$activityLogService', function (
    $scope,
    $activityLogService) {
    $scope.searchCriteria = {
        CustomerId: customerId,
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        MinToDate: new Date(initFromDate),
        MaxFromDate: new Date(currentDate),
        Username: '',
        Action: null
    };
    $scope.totalItems = 0;
    $scope.activityLogs = [];
    $scope.actions = [];
    $scope.isUpdatingDate = false;
    $scope.isSearched = false;

    $scope.initController = function () {
        $scope.getActions();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'ActivityLogs'
        });
    };

    /* jshint camelcase: false */
    $scope.invokeHotKey_CtrlS = function () {
        $scope.searchActivityLogs();
    };

    /* for datetime picker: fromdate and todate */
    $scope.updateFromDate = function (selectedDate) {
        $scope.searchCriteria.FromDate = selectedDate;
        $scope.searchCriteria.MinToDate = selectedDate;
    };

    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.searchCriteria.MaxFromDate = selectedDate;
    };

    $scope.getActions = function () {
        $scope.blockUI();
        $activityLogService.getActions()
            .then(function (data) {
                $scope.actions = data.Result;
                $scope.actions.unshift({ Id: crmConstants.Enums.Action.All, Name: activityLogResources.All });
                $scope.searchCriteria.Action = $scope.actions[0];
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.unblockUI();
            });
    };

    $scope.searchActivityLogs = function (pageIndex, itemsPerPage) {
        $scope.blockUI();
        $scope.searchCriteria.PageIndex = pageIndex ? pageIndex : 0;
        $scope.searchCriteria.ItemsPerPage = itemsPerPage ? itemsPerPage : sharedScriptManager.cachedData.defaultItemsPerPage;
        var searchLogsCriteria = {
            CustomerId: $scope.searchCriteria.CustomerId,
            Username: $scope.searchCriteria.Username,
            FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
            ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
            ActionId: $scope.searchCriteria.Action.Id,
            PageIndex: $scope.searchCriteria.PageIndex,
            ItemsPerPage: $scope.searchCriteria.ItemsPerPage
        };
        $activityLogService.getActivityLogs(searchLogsCriteria)
            .then(function (data) {
                var result = data.Result;
                $scope.totalItems = result.TotalItems;
                $scope.activityLogs = result.ActivityLogs;
                $scope.isSearched = true;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.isSearched = true;
                $scope.unblockUI();
            });
    };

    /* block and unblock UI functions */
    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };
    $scope.blockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };

    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);