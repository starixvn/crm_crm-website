/* global crmConstants, 
sharedScriptManager,
viewResources,
moment,currentDate, fromDateDefault, toDateDefault */
/* jshint camelcase: false */
(function () {
    "use strict";
    angular.module('deleted-feedback-statistics-application')
    .controller('deletedFeedbackStatisticsController', ['$scope', 'DeletedFeedbackStatisticsService', '$uiBlockerService',
    function ($scope, DeletedFeedbackStatisticsService, $uiBlockerService) {
        $scope.reportOptions = {
            DataList: null,
            IsUpdateGrid: false
        };
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;

        $scope.searchCriteria = {
            Handler: '',
            DeletedBy: '',
            FromDate: '',
            ToDate: ''
        };
        $scope.cachedSearchCriteria = {
            Handler: '',
            DeletedBy: '',
            FromDate: '',
            ToDate: ''
        };
        $scope.downloadFileOptions = null;

        $scope.initController = function () {
            $uiBlockerService.blockUI();
            $scope.initPicker();
            $uiBlockerService.unblockUI();
        };

        $scope.initPicker = function () {
            $scope.initDatePicker();
        };

        $scope.initDatePicker = function () {
            $scope.fromDateOptions = {
                CurrentDate: fromDateDefault,
                InitDate: fromDateDefault,
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
                CurrentDate: toDateDefault,
                InitDate: toDateDefault,
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

        $scope.getReport = function () {
            $uiBlockerService.blockUI();
            $scope.cachedSearchCriteria = {
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                Handler: $scope.searchCriteria.Handler,
                DeletedBy: $scope.searchCriteria.DeletedBy
            };

            DeletedFeedbackStatisticsService.getReport($scope.cachedSearchCriteria)
                    .then(function (data) {
                        $scope.reportOptions.DataList = data.Result.ReportData;
                        $scope.reportOptions.mainCategory = data.Result.MainCategories;
                        $scope.reportOptions.IsUpdateGrid = true;

                        $uiBlockerService.unblockUI();
                    },
                    function () {
                        $uiBlockerService.unblockUI();
                    });
        };

        $scope.openDetailFeedbackPopup = function (id) {
            var url = window.rootUrl + "CommunicationLog/ViewDetailById?" + $.param({
                logId: id
            });

            parent.OpenPopup(
            url,
            viewResources.FeedbackDetail,
            $(parent.document).width() - 300,
            $(parent.document).height() - 50,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            { my: "center top+50", at: "center top" },
            null);
        };

        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "DeletedFeedbackStatistics/ExportReport",
                Params: $scope.cachedSearchCriteria
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();