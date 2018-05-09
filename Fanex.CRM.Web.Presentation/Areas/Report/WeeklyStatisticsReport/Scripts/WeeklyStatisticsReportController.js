(function () {
    "use strict";
    /* global crmConstants,
    currentDate,
    moment,
    sharedScriptManager,
    registerUpdateFunctions,
    registerOptions,
    registerHotkeyFunctions */
    /* jshint camelcase: false,
    unused: vars */
    angular.module('report-weeklystatistics-application').controller('weeklyStatisticsReportController',
    ['$scope', '$uiBlockerService', 'weeklyStatisticsReportService',
    function (
        $scope,
        $uiBlockerService,
        weeklyStatisticsReportService) {
        var _lastSubmitData;
        $scope.searchCriteria = {
            FromDate: new Date(currentDate),
            ToDate: new Date(currentDate)
        };
        $scope.getDetailReportCriteria = {
            FromDate: new Date(currentDate),
            ToDate: new Date(currentDate),
            PageIndex: 1,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
            IsPagingCallBack: false
        };
        $scope.oldSearchCriteria = null;
        $scope.initController = function () {
            sharedScriptManager.safeApply($scope);
            $scope.initDefaultOption();

            $scope.fromDateOptions.CompleteRender = $scope.completeRenderFromDate;
            $scope.fromDateOptions.CompleteRender = $scope.completeRenderToDate;
        };
        $scope.getWeeklyStatisticsReport = function () {
            $uiBlockerService.blockUI();
            _lastSubmitData = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment)
            };

            weeklyStatisticsReportService.getWeeklyStatisticsReport(_lastSubmitData)
                .then(function (data) {
                    $scope.ibcReportOptions.DataList = data.Result.IBCReport;
                    $scope.ibcReportOptions.IsUpdateGrid = true;

                    $scope.licenseeReportOptions.DataList = data.Result.LicenseeReport;
                    $scope.licenseeReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportIBCReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "WeeklyStatisticsReport/ExportIbcReport",
                Params: _lastSubmitData
            };

            $scope.$apply();
        };
        $scope.exportLicenseeReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "WeeklyStatisticsReport/ExportLicenseeReport",
                Params: _lastSubmitData
            };

            $scope.$apply();
        };
        $scope.viewDetail = function (siteType) {
            $uiBlockerService.blockUI();
            $scope.searchCriteria.SiteType = siteType;
            $scope.getDetailReportCriteria = {
                FromDate: _lastSubmitData.FromDate,
                ToDate: _lastSubmitData.ToDate,
                SiteType: siteType,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
                IsPagingCallBack: false
            };

            weeklyStatisticsReportService.getWeeklyStatisticsDetailReport($scope.getDetailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result.Report;
                    $scope.mainCategory = data.Result.MainCategory;
                    $scope.CommunicationToolList = data.Result.CommunicationToolList;
                    $scope.ExtensionList = data.Result.ExtensionList;
                    $scope.ProductList = data.Result.ProductList;
                    $scope.SubProductList = data.Result.SubProductList;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.pagingDetailReport = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.getDetailReportCriteria = {
                FromDate: _lastSubmitData.FromDate,
                ToDate: _lastSubmitData.ToDate,
                SiteType: $scope.searchCriteria.SiteType,
                PageIndex: pageIndex + 1,
                PageSize: pageSize,
                IsPagingCallBack: true
            };

            weeklyStatisticsReportService.getWeeklyStatisticsDetailReport($scope.getDetailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportDetailReport = function () {
            var getReportCriteria = {
                FromDate: _lastSubmitData.FromDate,
                ToDate: _lastSubmitData.ToDate,
                SiteType: $scope.getDetailReportCriteria.SiteType,
                IsPagingCallBack: false,
                IsGetFullReport: true
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "WeeklyStatisticsReport/ExportDetailReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            $uiBlockerService.blockUI();
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);

            $scope.initController();
            $scope.setFocus();
            $scope.registerHotKey();
            $uiBlockerService.unblockUI();
        });
    } ]);
})();