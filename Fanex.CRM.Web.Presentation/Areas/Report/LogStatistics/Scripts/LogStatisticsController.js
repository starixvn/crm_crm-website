(function () {
    'use strict';
    /* global casinoDailyApplication,
    hotKeyManager,
    sharedScriptManager,
    crmConstants,
    moment,
    currentDate */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('log-statistics-application').controller('logStatisticsController', ['$scope', 'LogStatisticsService', '$uiBlockerService',
    function ($scope, logStatisticsService, $uiBlockerService) {
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

        $scope.detailReportCriteria = {
            FromDate: new Date(currentDate),
            ToDate: new Date(currentDate),
            PageIndex: 1,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            IsPagingCallBack: false
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
                    Id: 'LogStatistic'
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
                    AllText: viewResources.All,
                    MinWidth: 200,
                    MaxHeight: 600,
                    NoItemText: viewResources.Symbol_NotApply,
                    NoneSelectedText: viewResources.Symbol_NotApply,
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
                    AllText: viewResources.All,
                    MaxHeight: 450,
                    NoItemText: viewResources.Symbol_NotApply,
                    NoneSelectedText: viewResources.Symbol_NotApply,
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
                    AllText: viewResources.All,
                    MaxHeight: 450,
                    NoItemText: viewResources.Symbol_NotApply,
                    NoneSelectedText: viewResources.Symbol_NotApply,
                    NumberOfItemShowShortText: 0
                };
            }
            $scope.updateSelectedCommunicationTool = function (communicationIdList) {
                $scope.searchCriteria.CommunicationIds = communicationIdList;
            };

            $scope.initDatePicker();
            $scope.initProductMultiSelectPicker();
            $scope.initMainCategoryMultiSelectPickers();
            $scope.initCommunicationMultiSelectPickers();
        };

        $scope.getAllSetting = function () {
            var params = {};
            logStatisticsService.getAllSetting()
                    .then(function (data) {
                        $scope.communicationToolOptions.DataList = data.Result.CommunicationToolList;
                        $scope.categoryOptions.DataList = data.Result.MainCategoryList;
                        $scope.productOptions.DataList = data.Result.ProductList;
                    });
        };

        $scope.getLogStatisticReport = function () {
            $uiBlockerService.blockUI();
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds
            };

            logStatisticsService.getMainReport(getReportCriteria)
                    .then(function (data) {
                        $scope.ibcReportOptions.DataList = data.Result.IBCReport;
                        $scope.ibcReportOptions.IsUpdateGrid = true;

                        $scope.licenseeReportOptions.DataList = data.Result.LicenseeReport;
                        $scope.licenseeReportOptions.IsUpdateGrid = true;

                        $uiBlockerService.unblockUI();
                    },
                    function (data) {
                        $uiBlockerService.unblockUI();
                    });
        };

        $scope.toggleExpansionForIBCReport = function () {
            $uiBlockerService.blockUI();
            $scope.$apply(function () {
                $scope.ibcReportOptions.IsExpand = !$scope.ibcReportOptions.IsExpand;
                $scope.ibcReportOptions.IsUpdateGrid = true;
            });
            $uiBlockerService.unblockUI();
        };

        $scope.toggleExpansionForLicenseeReport = function () {
            $uiBlockerService.blockUI();
            $scope.$apply(function () {
                $scope.licenseeReportOptions.IsExpand = !$scope.licenseeReportOptions.IsExpand;
                $scope.licenseeReportOptions.IsUpdateGrid = true;
            });
            $uiBlockerService.unblockUI();
        };

        $scope.viewDetail = function (siteType) {
            $uiBlockerService.blockUI();
            $scope.searchCriteria.SiteType = siteType;
            $scope.detailReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: siteType,
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
                IsPagingCallBack: false
            };

            logStatisticsService.getDetailReport($scope.detailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result;
                    $scope.detailReportOptions.IsUpdateGrid = true;

                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.pagingReportDetail = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.detailReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: $scope.searchCriteria.SiteType,
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                PageIndex: pageIndex + 1,
                PageSize: pageSize,
                IsPagingCallBack: true
            };

            logStatisticsService.getDetailReport($scope.detailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportReport = function (reportType) {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                SiteType: reportType
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistics/ExportReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };

        $scope.exportReportDetail = function () {
            var reportDetailCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: $scope.searchCriteria.SiteType,
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                IsPagingCallBack: false,
                IsGetFullReport: true
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistics/ExportReportDetail",
                Params: reportDetailCriteria
            };

            $scope.$apply();
        }

        angular.element(document).ready(function () {
            registerOptions($scope);
            $scope.initController();
        });
    } ]);
})();