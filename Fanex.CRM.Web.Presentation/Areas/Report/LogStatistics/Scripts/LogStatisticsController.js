"use strict";
/* global crmConstants,
sharedScriptManager, hotKeyManager,
currentDate,
moment, 
viewResources, currentDate, firstDayOfWeek */
/* jshint camelcase: false,
unused: vars */
(function () {
    angular.module('log-statistics-application').controller('logStatisticsController', ['$scope', 'LogStatisticsService', '$uiBlockerService', '$timeout',
    function ($scope, logStatisticsService, $uiBlockerService, $timeout) {
        $scope.allSettings = null;

        $scope.errorMessage = '';

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
        $scope.cachedSearchCriteria = {
            FromDate: '',
            ToDate: '',
            ProductIds: '',
            CategoryIds: '',
            CommunicationIds: ''
        };

        $scope.detailReportCriteria = {
            FromDate: '',
            ToDate: '',
            PageIndex: 1,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };

        $scope.ibcReportOptions = {
            Type: 0,
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false,
            IsExpand: false
        };

        $scope.licenseeReportOptions = {
            Type: 1,
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false,
            IsExpand: false
        };

        $scope.detailReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };

        $scope.downloadFileOptions = null;

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
        };
        $scope.initDatePicker = function () {
            $scope.fromDateOptions = {
                CurrentDate: firstDayOfWeek,
                InitDate: firstDayOfWeek,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: function (selectedDate) {
                    $scope.searchCriteria.FromDate = moment(selectedDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment);
                    
                    if ($scope.toDateOptions) {
                        var maxToDate = new Date(selectedDate.getTime());
                        maxToDate.setMonth(selectedDate.getMonth() + 12);
                        maxToDate.setDate(selectedDate.getDate() - 1);
                        $scope.toDateOptions.MinDate = selectedDate;
                        $scope.toDateOptions.MaxDate = (maxToDate < currentDate) ? maxToDate : currentDate;
                        $scope.updateToDatePicker(true);
                    }
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
                },
                IsAllowNullDate: true,
                YearRange: sharedScriptManager.cachedData.yearRangeDatePicker
            };
            $scope.updateToDatePicker();
        };
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'Report_LogStatistics'
            });
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

        $scope.initProductMultiSelectPicker = function () {
            $scope.productOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedProduct,
                AllText: viewResources.All,
                MinWidth: 200,
                MaxHeight: 600,
                NoItemText: viewResources.Symbol_NotApply,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 2
            };
        };
        $scope.updateSelectedProduct = function (productIdList) {
            $scope.searchCriteria.ProductIds = productIdList;
        };

        $scope.initMainCategoryMultiSelectPickers = function () {
            $scope.categoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCategory,
                AllText: viewResources.All,
                MaxHeight: 450,
                NoItemText: viewResources.Symbol_NotApply,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 0
            };
        };
        $scope.updateSelectedCategory = function (categoryIdList) {
            $scope.searchCriteria.CategoryIds = categoryIdList;
        };

        $scope.initCommunicationMultiSelectPickers = function () {
            $scope.communicationToolOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCommunicationTool,
                AllText: viewResources.All,
                MaxHeight: 450,
                NoItemText: viewResources.Symbol_NotApply,
                NoneSelectedText: viewResources.Symbol_NotApply,
                NumberOfItemShowShortText: 0
            };
        };
        $scope.updateSelectedCommunicationTool = function (communicationIdList) {
            $scope.searchCriteria.CommunicationIds = communicationIdList;
        };

        $scope.validateData = function () {
            if ($scope.searchCriteria.ProductIds === '') {
                $scope.errorMessage = viewResources.ProductFilterRequired;
            }
            else if ($scope.searchCriteria.CategoryIds === '') {
                $scope.errorMessage = viewResources.MainCategoryFilterRequired;
            }
            else if ($scope.searchCriteria.CommunicationIds === '') {
                $scope.errorMessage = viewResources.CommunicationFilterRequired;
            }
            else {
                $scope.errorMessage = '';
            }
        };

        $scope.getAllSetting = function () {
            var params = {};
            logStatisticsService.getAllSetting(params)
                    .then(function (data) {
                        $scope.communicationToolOptions.DataList = data.Result.CommunicationToolList;
                        $scope.categoryOptions.DataList = data.Result.MainCategoryList;
                        $scope.productOptions.DataList = data.Result.ProductList;
                    });
        };

        $scope.getLogStatisticReport = function () {
            $scope.validateData();
            if ($scope.errorMessage !== '') {
                return;
            }            

            $uiBlockerService.blockUI();
            $scope.cachedSearchCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds
            };

            logStatisticsService.getMainReport($scope.cachedSearchCriteria)
                    .then(function (data) {
                        if (data.Result.Success) {
                            $scope.ibcReportOptions.DataList = data.Result.IBCReport;
                            $scope.ibcReportOptions.IsUpdateGrid = true;

                            $scope.licenseeReportOptions.DataList = data.Result.LicenseeReport;
                            $scope.licenseeReportOptions.IsUpdateGrid = true;
                        }

                        $uiBlockerService.unblockUI();
                    },
                    function () {
                        $uiBlockerService.unblockUI();
                    });
        };

        $scope.toggleExpansionForIBCReport = function () {
            $uiBlockerService.blockUI();

            setTimeout(function () {
                $scope.ibcReportOptions.IsExpand = !$scope.ibcReportOptions.IsExpand;
                $scope.ibcReportOptions.IsUpdateGrid = true;
                $scope.$apply();
                $uiBlockerService.unblockUI();
            }, 100);
        };

        $scope.toggleExpansionForLicenseeReport = function () {
            $uiBlockerService.blockUI();

            setTimeout(function () {
                $scope.licenseeReportOptions.IsExpand = !$scope.licenseeReportOptions.IsExpand;
                $scope.licenseeReportOptions.IsUpdateGrid = true;
                $scope.$apply();
                $uiBlockerService.unblockUI();
            }, 100);           
        };

        $scope.viewDetail = function (siteType) {
            $uiBlockerService.blockUI();
            $scope.cachedSearchCriteria.SiteType = siteType;
            $scope.detailReportCriteria = {
                FromDate: $scope.cachedSearchCriteria.FromDate,
                ToDate: $scope.cachedSearchCriteria.ToDate,
                SiteType: siteType,
                ProductId: $scope.cachedSearchCriteria.ProductId,
                MainCategoryId: $scope.cachedSearchCriteria.MainCategoryId,
                CommunicationId: $scope.cachedSearchCriteria.CommunicationId,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
            };

            logStatisticsService.getDetailReport($scope.detailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result.Report;
                    $scope.detailReportOptions.mainCategory = data.Result.MainCategories;
                    $scope.detailReportOptions.CommunicationToolList = data.Result.CommunicationToolList;
                    $scope.detailReportOptions.ExtensionList = data.Result.ExtensionList;
                    $scope.detailReportOptions.ProductList = data.Result.ProductList;
                    $scope.detailReportOptions.SubProductList = data.Result.SubProductList;
                    $scope.detailReportOptions.IsUpdateGrid = true;

                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.pagingReportDetail = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();

            $scope.detailReportCriteria.PageIndex = pageIndex + 1;
            $scope.detailReportCriteria.PageSize = pageSize;

            logStatisticsService.getDetailReport($scope.detailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result.Report;
                    $scope.detailReportOptions.mainCategory = data.Result.MainCategories;
                    $scope.detailReportOptions.CommunicationToolList = data.Result.CommunicationToolList;
                    $scope.detailReportOptions.ExtensionList = data.Result.ExtensionList;
                    $scope.detailReportOptions.ProductList = data.Result.ProductList;
                    $scope.detailReportOptions.SubProductList = data.Result.SubProductList;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportReport = function (reportType) {
            $scope.cachedSearchCriteria.SiteType = reportType;

            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistics/ExportReport",
                Params: $scope.cachedSearchCriteria
            };

            $scope.$apply();
        };

        $scope.exportReportDetail = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistics/ExportReportDetail",
                Params: $scope.detailReportCriteria
            };

            $scope.$apply();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getLogStatisticReport();
        };

        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            $scope.initController();
            $scope.registerHotKey();
        });
    } ]);
})();