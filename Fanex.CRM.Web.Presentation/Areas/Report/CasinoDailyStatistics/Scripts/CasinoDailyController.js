(function () {
    "use strict";
    /* global hotKeyManager,
    sharedScriptManager,
    crmConstants,
    moment,
    casinoDailyResources,
    currentDate */
    /* jshint camelcase: false */
    angular.module('report-casinodaily-application').controller('casinoDailyController', ['$scope', 'CasinoDailyService', '$uiBlockerService',
    function ($scope, casinoDailyService, $uiBlockerService) {
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.searchCriteria = {
            FromDate: '',
            ToDate: '',
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };
        $scope.dailySummary;
        $scope.dailySummaryChart;
        $scope.dailyDetail = null;
        $scope.downloadFileOptions = null;
        $scope.showSummary = true;

        $scope.initController = function () {
            $uiBlockerService.blockUI();
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
                Id: 'CasinoDailyStatistics'
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

        $scope.getCasinoDailySummary = function () {
            $uiBlockerService.blockUI();
            $scope.showSummary = true;
            $scope.dailyDetail = null;
            casinoDailyService.getCasinoDailySummary($scope.searchCriteria.FromDate, $scope.searchCriteria.ToDate)
                .then(function (data) {
                    $scope.dailySummary = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.getCasinoDailyDetail = function (page, size) {
            $scope.showSummary = false;
            $uiBlockerService.blockUI();
            casinoDailyService.getCasinoDailyDetail($scope.searchCriteria.FromDate, $scope.searchCriteria.ToDate,
            page = page || 0, size || sharedScriptManager.cachedData.defaultItemsPerPage)
        .then(function (data) {
            $scope.dailyDetail = data.Result;
            $uiBlockerService.unblockUI();
        },
        function () {
            $uiBlockerService.unblockUI();
        });
        };

        $scope.viewCasinoDailySummaryChart = function () {
            if (!$scope.dailySummaryChart) {
                return;
            }

            var crmChart = parent.crmChart;
            crmChart.title = $scope.dailySummary.Title;
            var chartOption = $.extend({}, crmChart.newopts, {
                yAxisUnit: casinoDailyResources.Feedbacks,
                chartType: crmChart.chartType.pie
            });

            crmChart.viewChart($scope.dailySummaryChart, chartOption);
        };

        $scope.exportCasinoDailySummary = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoDailySummary',
                Params: { fromDate: $scope.searchCriteria.FromDate, toDate: $scope.searchCriteria.ToDate }
            };

            $scope.$apply();
        };

        $scope.exportCasinoDailyDetail = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'CasinoReport/ExportCasinoDailyDetail',
                Params: { fromDate: $scope.searchCriteria.FromDate, toDate: $scope.searchCriteria.ToDate }
            };

            $scope.$apply();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getCasinoDailySummary();
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();