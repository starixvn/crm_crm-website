'use strict';
/* global casinoDailyApplication,
hotKeyManager,
sharedScriptManager,
crmConstants,
moment,
casinoDailyResources,
currentDate */
/* jshint camelcase: false */
/* jshint unused: vars */
casinoDailyEnhancementApplication.controller('casinoDailyEnhancementController', ['$scope', 'CasinoDailyEnhancementService', '$uiBlockerService',
function ($scope, casinoDailyService, $uiBlockerService) {
    $scope.allSettings = null;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.searchCriteria = {
        FromDate: '',
        ToDate: '',
        PageIndex: 0,
        PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
    };

    $scope.initController = function () {
        $uiBlockerService.blockUI();
        $scope.getAllSetting();
        $scope.fromDateOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: function (selectedDate) {
                $scope.searchCriteria.FromDate = moment(selectedDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
                if ($scope.toDateOptions) {
                    $scope.toDateOptions.MinDate = selectedDate && selectedDate !== '' ? selectedDate : null;
                }

                $scope.updateToDatePicker(true);
            },
            IsAllowNullDate: true,
            YearRange: sharedScriptManager.cachedData.yearRangeDatePicker
        };
        $scope.updateFromDatePicker();

        $scope.toDateOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: function (selectedDate) {
                $scope.searchCriteria.ToDate = moment(selectedDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
                $scope.fromDateOptions.MaxDate = selectedDate && selectedDate !== '' ? selectedDate : null;
                $scope.updateFromDatePicker(true);
            },
            IsAllowNullDate: true,
            YearRange: sharedScriptManager.cachedData.yearRangeDatePicker
        };
        $scope.updateToDatePicker();

        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'CasinoDailyEnhancementStatistics'
        });
        $uiBlockerService.unblockUI();
    };

    $scope.updateFromDatePicker = function (isOnApply) {
        $scope.fromDateOptions.IsRerenderPicker = true;
        if (!isOnApply) {
            $scope.$apply();
        }
    };

    $scope.updateToDatePicker = function (isOnApply) {
        if ($scope.toDateOptions) {
            $scope.toDateOptions.IsRerenderPicker = true;
        }

        if (!isOnApply) {
            $scope.$apply();
        }
    };

    $scope.getAllSetting = function () {
        var params = {};
        casinoDailyService.getAllSetting()
                .then(function (data) {
                    $scope.communicationToolOptionDataList = data.Result.CommunicationToolList;
                    $scope.productOptionDataList = data.Result.ProductList;
                    $scope.selected = $scope.productOptionDataList[0];
                    $scope.mainCategoryOptionDataList = data.Result.MainCategoryList;
                });
    };

    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);