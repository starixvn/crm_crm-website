(function () {
    "use strict";
    /* global hotKeyManager,
    sharedScriptManager,
    casinoWeeklyResources,
    fromDate,
    toDate */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('report-casinoweekly-application').controller('casinoweeklyController', ['$scope', 'CasinoWeeklyService', '$uiBlockerService',
    function ($scope, casinoWeeklyService, $uiBlockerService) {
        $scope.fromDate = fromDate;
        $scope.toDate = toDate;
        $scope.weeklySummary;
        $scope.weeklySummaryChart;
        $scope.weeklyDetail = null;
        $scope.downloadFileOptions = null;
        $scope.showSummary = true;

        $scope.initController = function () {
            $uiBlockerService.blockUI();
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'CasinoWeeklyStatistics'
            });
            $uiBlockerService.unblockUI();
        };

        $scope.getCasinoWeeklySummary = function () {
            $uiBlockerService.blockUI();
            $scope.showSummary = true;
            $scope.weeklyDetail = null;
            casinoWeeklyService.getCasinoWeeklySummary($scope.fromDate, $scope.toDate)
                .then(function (data) {
                    $scope.weeklySummary = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.viewCasinoWeeklySummaryChart = function () {
            if (!$scope.weeklySummaryChart) {
                return;
            }

            var crmChart = parent.crmChart;
            crmChart.title = $scope.weeklySummary.Title;
            var chartOption = crmChart.newopts;
            chartOption = $.extend(chartOption, {
                chartType: crmChart.chartType.bar,
                xAxisLabel: '',
                yAxisLabel: '',
                yAxisUnit: casinoWeeklyResources.Feedbacks,
                yAxisFontSize: 8,
                scaleFontSize: 12,
                inGraphDataFontSize: 9,
                inGraphDataRotate: 0,
                legend: false
            });

            crmChart.viewChart($scope.weeklySummaryChart, chartOption);
        };

        $scope.getCasinoWeeklyDetail = function (page, size) {
            $scope.showSummary = false;
            $uiBlockerService.blockUI();
            casinoWeeklyService.getCasinoWeeklyDetail(
                $scope.fromDate, $scope.toDate, page = page || 0, size || sharedScriptManager.cachedData.defaultItemsPerPage)
                .then(function (data) {
                    $scope.weeklyDetail = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportCasinoWeeklySummaryExcel = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoWeeklySummary',
                Params: { fromDate: $scope.fromDate, toDate: $scope.toDate }
            };

            $scope.$apply();
        };

        $scope.exportCasinoWeeklyDetail = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoWeeklyDetail',
                Params: { fromDate: $scope.fromDate, toDate: $scope.toDate }
            };

            $scope.$apply();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getCasinoWeeklySummary();
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();