(function () {
    "use strict";
    /* global hotKeyManager,
    sharedScriptManager,
    casinoWeeklyResources,
    fromDate,
    toDate */
    /* jshint camelcase: false */
    angular.module('report-casinoweekly-application').controller('casinoweeklyController', ['$scope', 'CasinoWeeklyService', '$uiBlockerService',
    function ($scope, casinoWeeklyService, $uiBlockerService) {
        var _lastSubmitData;

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
            _lastSubmitData = {
                fromDate: $scope.fromDate,
                toDate: $scope.toDate
            };
            casinoWeeklyService.getCasinoWeeklySummary(_lastSubmitData)
                .then(function (data) {
                    $scope.weeklySummary = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
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
                _lastSubmitData.fromDate, _lastSubmitData.toDate, page = page || 0, size || sharedScriptManager.cachedData.defaultItemsPerPage)
                .then(function (data) {
                    $scope.weeklyDetail = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportCasinoWeeklySummaryExcel = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoWeeklySummary',
                Params: _lastSubmitData
            };

            $scope.$apply();
        };

        $scope.exportCasinoWeeklyDetail = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoWeeklyDetail',
                Params: _lastSubmitData
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