"use strict";
/* global dailyStatisticsReportApplication,
crmConstants,
currentDate,
moment,
sharedScriptManager,
registerUpdateFunctions,
registerOptions,
registerHotkeyFunctions */
/* jshint camelcase: false,
unused: vars */
dailyStatisticsReportApplication.controller('dailyStatisticsReportController',
    ['$scope', '$http', '$uiBlockerService', '$resultMessageService', 'dailyStatisticsReportService',
    function (
        $scope,
        $http,
        $uiBlockerService,
        $resultMessageService,
        dailyStatisticsReportService) {
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
        $scope.getDailyStatisticsReport = function () {
            $uiBlockerService.blockUI();
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment)
            };

            dailyStatisticsReportService.getDailyStatisticsReport(getReportCriteria)
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
        $scope.exportIBCReport = function () {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment)
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DailyStatisticsReport/ExportIbcReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };
        $scope.exportLicenseeReport = function () {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment)
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DailyStatisticsReport/ExportLicenseeReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };
        $scope.viewDetail = function (siteType) {
            $uiBlockerService.blockUI();
            $scope.getDetailReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: siteType,
                PageIndex: 1,
                PageSize: sharedScriptManager.cachedData.defaultItemsPerPage,
                IsPagingCallBack: false
            };

            dailyStatisticsReportService.getDailyStatisticsDetailReport($scope.getDetailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.pagingDetailReport = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.getDetailReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                SiteType: $scope.searchCriteria.SiteType,
                PageIndex: pageIndex + 1,
                PageSize: pageSize,
                IsPagingCallBack: true
            };

            dailyStatisticsReportService.getDailyStatisticsDetailReport($scope.getDetailReportCriteria)
                .then(function (data) {
                    $scope.detailReportOptions.DataList = data.Result;
                    $scope.detailReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportDetailReport = function () {
            var getReportCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                IsPagingCallBack: false,
                IsGetFullReport: true
            };
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DailyStatisticsReport/ExportDetailReport",
                Params: getReportCriteria
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            registerOptions($scope);
            registerUpdateFunctions($scope);
            registerHotkeyFunctions($scope);

            $scope.initController();
            $scope.setFocus();
            $scope.registerHotKey();
        });
    } ]);