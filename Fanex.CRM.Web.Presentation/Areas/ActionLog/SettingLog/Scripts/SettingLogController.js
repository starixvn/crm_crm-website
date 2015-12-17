(function () {
    "use strict";
    /* global 
    hotKeyManager,
    sharedScriptManager,
    settingLogResources,
    currentDate,
    crmConstants,
    initFromDate,
    moment */
    angular.module('settinglog-application').controller('settingLogController', ['$scope', '$settingLogService', function (
    $scope,
    $settingLogService) {
        $scope.searchCriteria = {
            FromDate: initFromDate,
            ToDate: new Date(currentDate),
            MinToDate: new Date(initFromDate),
            MaxFromDate: new Date(currentDate),
            Username: '',
            Action: null
        };
        $scope.totalItems = 0;
        $scope.settingLogs = [];
        $scope.actions = [];
        $scope.isUpdatingDate = false;
        $scope.isSearched = false;

        $scope.initController = function () {
            $scope.getActions();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'SettingLogs'
            });
        };

        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.searchSettingLogs();
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
            $settingLogService.getActions()
            .then(function (data) {
                $scope.actions = data.Result;
                $scope.actions.unshift({ Id: crmConstants.Enums.Action.All, Name: settingLogResources.All });
                $scope.searchCriteria.Action = $scope.actions[0];
                $scope.unblockUI();
            },
            function () {
                $scope.unblockUI();
            });
        };

        $scope.searchSettingLogs = function (pageIndex, itemsPerPage) {
            $scope.blockUI();
            $scope.searchCriteria.PageIndex = pageIndex ? pageIndex : 0;
            $scope.searchCriteria.ItemsPerPage = itemsPerPage ? itemsPerPage : sharedScriptManager.cachedData.defaultItemsPerPage;
            var searchLogsCriteria = {
                Username: $scope.searchCriteria.Username,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ActionId: $scope.searchCriteria.Action.Id,
                PageIndex: $scope.searchCriteria.PageIndex,
                ItemsPerPage: $scope.searchCriteria.ItemsPerPage
            };
            $settingLogService.getSettingLogs(searchLogsCriteria)
            .then(function (data) {
                var result = data.Result;
                $scope.totalItems = result.TotalItems;
                $scope.settingLogs = result.SettingLogs;
                $scope.isSearched = true;
                $scope.unblockUI();
            },
            function () {
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

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.blockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();