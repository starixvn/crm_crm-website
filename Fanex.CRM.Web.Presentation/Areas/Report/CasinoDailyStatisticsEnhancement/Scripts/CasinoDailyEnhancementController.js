'use strict';
/* global casinoDailyApplication,
hotKeyManager,
sharedScriptManager,
crmConstants,
moment,
currentDate */
/* jshint camelcase: false */
/* jshint unused: vars */
casinoDailyEnhancementApplication.controller('casinoDailyEnhancementController', ['$scope', 'CasinoDailyEnhancementService', '$uiBlockerService',
function ($scope, casinoDailyService, $uiBlockerService) {
    $scope.allSettings = null;
    $scope.productOptions = null;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.searchCriteria = {
        FromDate: '',
        ToDate: '',
        ProductIds: '',
        PageIndex: 0,
        PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
    };

    $scope.initController = function () {
        $uiBlockerService.blockUI();

        $scope.initPicker();
        $scope.getAllSetting();

        $uiBlockerService.unblockUI();
    };

    $scope.initPicker = function () {
        $scope.initDatePicker();
        $scope.initProductMultiSelectPicker();
        $scope.initOtherPickers();
    }

    $scope.initDatePicker = function () {
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
    }
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

    $scope.initProductMultiSelectPicker = function () {
        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedProduct,
            AllText: casinoDailyEnhancementResources.All,
            MinWidth: 200,
            MaxHeight: 600,
            NoItemText: "",
            NoneSelectedText: casinoDailyEnhancementResources.Symbol_NotApply,
            NumberOfItemShowShortText: 2
        };
    }
    $scope.updateSelectedProduct = function (productIdList) {
        $scope.searchCriteria.ProductIds = productIdList;
    };

    $scope.initOtherPickers = function () {
        $scope.communicationToolOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCommunicationTool
        };

        $scope.mainCategoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedMainCategory
        };
    }

    $scope.getAllSetting = function () {
        var params = {};
        casinoDailyService.getAllSetting()
                .then(function (data) {
                    $scope.communicationToolOptions.DataList = data.Result.CommunicationToolList;
                    $scope.mainCategoryOptions.DataList = data.Result.MainCategoryList;
                    $scope.productOptions.DataList = data.Result.ProductList;
                });
    };

    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);