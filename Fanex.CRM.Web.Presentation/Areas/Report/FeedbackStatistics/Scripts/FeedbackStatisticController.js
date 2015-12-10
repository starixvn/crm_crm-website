(function () {
    "use strict";
    /* global hotKeyManager,
    sharedScriptManager,
    crmConstants,
    moment,
    feedbackStatisticResources,
    currentDate,
    previousWeek,
    settings */
    /* jshint camelcase: false */
    angular.module('feedback-statistic-application')
        .controller('feedbackStatisticController', ['$scope', 'FeedbackStatisticService', '$uiBlockerService', 'alertBoxService',
    function ($scope, feedbackStatisticService, $uiBlockerService, alertBoxService) {
        var _lastSubmitData;

        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.searchCriteria = {
            FromDate: '',
            ToDate: '',
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.defaultItemsPerPage
        };

        $scope.setting = null;
        $scope.settings = settings;

        $scope.fromDuration = { Day: '', Hour: '', Minute: '', Second: '' };
        $scope.toDuration = { Day: '', Hour: '', Minute: '', Second: '' };

        $scope.feedbackStatistic = null;
        $scope.feedbackStatisticChart = null;
        $scope.downloadFileOptions = null;

        $scope.initController = function () {
            $uiBlockerService.blockUI();
            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: previousWeek,
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

            $scope.setting = $scope.settings[0];
            $scope.$apply();

            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'FeedbackStatistics'
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

        $scope.getDurationTime = function (duration) {
            return {
                Day: duration.Day.length === 0 ? 0 : parseInt(duration.Day),
                Hour: duration.Hour.length === 0 ? 0 : parseInt(duration.Hour),
                Minute: duration.Minute.length === 0 ? 0 : parseInt(duration.Minute),
                Second: duration.Second.length === 0 ? 0 : parseInt(duration.Second)
            };
        };

        $scope.getDurationTimeString = function (duration) {
            return (duration.Day.length === 0 ? '0' : duration.Day) + ':' +
            (duration.Hour.length === 0 ? '0' : duration.Hour) + ':' +
            (duration.Minute.length === 0 ? '0' : duration.Minute) + ':' +
            (duration.Second.length === 0 ? '0' : duration.Second);
        };

        $scope.validateDuration = function () {
            var fromDuration = $scope.getDurationTime($scope.fromDuration);
            var timeBegin = (fromDuration.Day * 86400) + (fromDuration.Hour * 3600) + (fromDuration.Minute * 60) + fromDuration.Second;

            var toDuration = $scope.getDurationTime($scope.toDuration);
            var timeEnd = (toDuration.Day * 86400) + (toDuration.Hour * 3600) + (toDuration.Minute * 60) + toDuration.Second;

            alertBoxService.hide();
            if ((timeBegin !== 0 && timeEnd === 0) ||
            (timeBegin > timeEnd && timeBegin !== 0 && timeEnd !== 0)) {
                alertBoxService.show({
                    type: crmConstants.Enums.MessageType.Error,
                    message: feedbackStatisticResources.InvalidDuration
                });
                return false;
            }

            return true;
        };

        $scope.getFeedbackStatistic = function (page, size) {
            if (!$scope.validateDuration()) {
                return;
            }

            $uiBlockerService.blockUI();
            _lastSubmitData = {
                SelectionType: $scope.setting.Id,
                FromDate: $scope.searchCriteria.FromDate,
                ToDate: $scope.searchCriteria.ToDate,
                FromDuration: $scope.getDurationTimeString($scope.fromDuration),
                ToDuration: $scope.getDurationTimeString($scope.toDuration),
                PageIndex: page || 0,
                PageSize: size || sharedScriptManager.cachedData.defaultItemsPerPage
            };

            feedbackStatisticService.getFeedbackStatistic(_lastSubmitData)
            .then(function (data) {
                $scope.feedbackStatistic = data.Result;
                if (!$scope.feedbackStatistic || $scope.feedbackStatistic.TotalItems === 0) {
                    alertBoxService.show({
                        type: crmConstants.Enums.MessageType.Error,
                        message: feedbackStatisticResources.NoDataFound
                    });
                }
                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.viewFeedbackStatisticChart = function () {
            if (!$scope.feedbackStatisticChart) {
                return;
            }

            var crmChart = parent.crmChart;
            crmChart.title = $scope.feedbackStatistic.Title;
            var chartOption = crmChart.newopts;
            chartOption = $.extend(chartOption, {
                chartType: crmChart.chartType.bar,
                xAxisLabel: '',
                yAxisLabel: '',
                yAxisUnit: feedbackStatisticResources.Feedbacks,
                yAxisFontSize: 8,
                scaleFontSize: 12,
                inGraphDataFontSize: 9,
                inGraphDataRotate: 0,
                legend: true
            });

            crmChart.viewChart($scope.feedbackStatisticChart, chartOption);
        };

        $scope.exportFeedbackStatistic = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'FeedbackStatisticReport/ExportFeedbackStatistic',
                Params: _lastSubmitData
            };

            $scope.$apply();
        };

        $scope.openFeedbackStatisticDetail = function (mainCategoryId, subCategoryId, valueId, page, size) {
            var criteria = {
                SelectionType: $scope.setting.Id,
                MainCategoryId: mainCategoryId,
                SubCategoryId: subCategoryId,
                ValueId: valueId,
                FromDate: $scope.searchCriteria.FromDate,
                ToDate: $scope.searchCriteria.ToDate,
                FromDuration: $scope.getDurationTimeString($scope.fromDuration),
                ToDuration: $scope.getDurationTimeString($scope.toDuration),
                PageIndex: page || 0,
                PageSize: size || sharedScriptManager.cachedData.defaultItemsPerPage
            };

            var popupWidth = $(parent.document).width() - 100;
            if (mainCategoryId !== 0 || subCategoryId !== 0 || valueId !== 0) {
                popupWidth -= 145;
            }
            parent.OpenPopup(
                window.rootUrl + 'FeedbackStatisticReport/FeedbackStatisticDetail?' + $.param(criteria),
                feedbackStatisticResources.FeedbackStatisticDetailReport,
                popupWidth,
                $(parent.document).height() - 30,
                true,
                false,
                parent.window,
                3000,
                null,
                null,
                null,
                null);
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.getFeedbackStatistic();
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();