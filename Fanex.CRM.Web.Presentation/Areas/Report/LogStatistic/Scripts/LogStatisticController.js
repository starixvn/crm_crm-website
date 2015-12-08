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
    angular.module('log-statistic-application').controller('logStatisticController', ['$scope', 'LogStatisticService', '$uiBlockerService',
    function ($scope, logStatisticService, $uiBlockerService) {
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
            IsUpdateGrid: false,
            IsExpand: false
        };

        $scope.licenseeReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false,
            IsExpand: false
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
            logStatisticService.getAllSetting()
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

            logStatisticService.getMainReport(getReportCriteria)
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
            var getDetailReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: siteType,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
                IsPagingCallBack: false
            };

            logStatisticService.getDetailReport(getDetailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result.IBCReport;
                    $scope.detailReportOptions.IsUpdateGrid = true;

                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportIBCReport = function () {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                SiteType: 0
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistic/ExportReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };

        $scope.exportLicenseeReport = function () {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ProductId: $scope.searchCriteria.ProductIds,
                MainCategoryId: $scope.searchCriteria.CategoryIds,
                CommunicationId: $scope.searchCriteria.CommunicationIds,
                SiteType: 1
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "LogStatistic/ExportReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            registerOptions($scope);
            $scope.initController();
        });
    } ]);
})();