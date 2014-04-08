'use strict';
/* global summaryReportApplication,
sharedScriptManager,
hotKeyManager,
minReportDate,
currentDate */
/* jshint camelcase: false */
summaryReportApplication.controller('summaryReportController', ['$scope', 'SummaryReportService', '$uiBlockerService', '$resultMessageService',
function ($scope, summaryReportService, $uiBlockerService, $resultMessageService) {
    $scope.years = [];
    $scope.year = 0;
    $scope.defaultFocusOptions = null;
    $scope.summaryReport;

    $scope.initController = function () {
        $scope.years = $scope.getYears();
        $scope.$apply();

        $scope.year = $scope.years[0];
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'SummaryReport'
        });

        $scope.defaultFocusOptions = {
            IsSetFocus: false,
            OnChangeEvent: $scope.completeSetFocus
        };
        $scope.setFocus();
    };

    $scope.getYears = function () {
        var years = [];
        var minYear = minReportDate.getFullYear();
        var currentYear = currentDate.getFullYear();
        for (var i = minYear; i <= currentYear; i++) {
            years.push({ id: i, value: i });
        }

        return years;
    };


    $scope.invokeHotKey_CtrlS = function () {
        $scope.getSummaryReport();
    };

    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
        });
    };

    $scope.completeSetFocus = function () {
        $scope.defaultFocusOptions.IsSetFocus = false;
    };

    $scope.getSummaryReport = function () {
        $uiBlockerService.blockUI();
        summaryReportService.getSummaryReport($scope.year.id)
            .then(function (data) {
                $scope.summaryReport = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                $uiBlockerService.unblockUI();
            });
    };

    $scope.exportSummaryReportExcel = function () {
        $scope.downloadFileOptions = {
            URL: window.rootUrl + 'SummaryReport/ExportSummaryReportExcel',
            Params: { year: $scope.year.id }
        };

        $scope.$apply();
    };

    angular.element(document).ready(function () {
        $uiBlockerService.blockUI();
        $uiBlockerService.unblockUI();
        $scope.initController();
    });
} ]);    