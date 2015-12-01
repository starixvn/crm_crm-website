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
    $scope.categoryOptions = null;
    $scope.communicationToolOptions = null;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.searchCriteria = {
        FromDate: '',
        ToDate: '',
        ProductIds: '',
        CategoryIds: '',
        CommunicationIds: ''
    };

    $scope.ibcReportOptions = {
        DataList: null,
        IsResize: false,
        IsUpdateGrid: false
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
        $scope.initMainCategoryMultiSelectPickers();
        $scope.initCommunicationMultiSelectPickers();
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
            NoItemText: casinoDailyEnhancementResources.Symbol_NotApply,
            NoneSelectedText: casinoDailyEnhancementResources.Symbol_NotApply,
            NumberOfItemShowShortText: 2
        };
    }
    $scope.updateSelectedProduct = function (productIdList) {
        $scope.searchCriteria.ProductIds = productIdList;
    };

    $scope.initMainCategoryMultiSelectPickers = function () {
        $scope.categoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCategory,
            AllText: casinoDailyEnhancementResources.All,
            MaxHeight: 450,
            NoItemText: casinoDailyEnhancementResources.Symbol_NotApply,
            NoneSelectedText: casinoDailyEnhancementResources.Symbol_NotApply,
            NumberOfItemShowShortText: 0
        };
    }
    $scope.updateSelectedCategory = function (categoryIdList) {
        $scope.searchCriteria.CategoryIds = categoryIdList;
    };

    $scope.initCommunicationMultiSelectPickers = function () {
        $scope.communicationToolOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCommunicationTool,
            AllText: casinoDailyEnhancementResources.All,
            MaxHeight: 450,
            NoItemText: casinoDailyEnhancementResources.Symbol_NotApply,
            NoneSelectedText: casinoDailyEnhancementResources.Symbol_NotApply,
            NumberOfItemShowShortText: 0
        };
    }
    $scope.updateSelectedCommunicationTool = function (communicationIdList) {
        $scope.searchCriteria.CommunicationIds = communicationIdList;
    };

    $scope.getAllSetting = function () {
        var params = {};
        casinoDailyService.getAllSetting()
                .then(function (data) {
                    $scope.communicationToolOptions.DataList = data.Result.CommunicationToolList;
                    $scope.categoryOptions.DataList = data.Result.MainCategoryList;
                    $scope.productOptions.DataList = data.Result.ProductList;
                });
    };

    $scope.getCasinoDailyEnhancementStatisticsReport = function () {
        $uiBlockerService.blockUI();
        var getReportCriteria = {
            FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
            ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
            ProductId: $scope.searchCriteria.ProductIds,
            MainCategoryId: $scope.searchCriteria.CategoryIds,
            CommunicationId: $scope.searchCriteria.CommunicationIds
        };

        casinoDailyService.getCasinoDailyEnhancementMainReport(getReportCriteria)
                .then(function (data) {                 
                    $scope.ibcReportOptions.DataList = data.Result.IBCReport;
                    $scope.ibcReportOptions.IsUpdateGrid = true;

                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
    };

    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);