'use strict';
/* global summaryReportApplication,
sharedScriptManager,
hotKeyManager,
summaryReportResources,
minReportDate,
currentDate */
/* jshint camelcase: false */
summaryReportApplication.controller('summaryReportController', ['$scope', 'SummaryReportService', '$uiBlockerService', '$resultMessageService',
function ($scope, summaryReportService, $uiBlockerService, $resultMessageService) {
    $scope.years = [];
    $scope.year = 0;
    $scope.categoryOptions = {};
    $scope.selectedCategoryIds = '';
    $scope.defaultFocusOptions = null;
    $scope.summaryReport;

    $scope.initController = function () {
        $uiBlockerService.blockUI();

        $scope.years = $scope.getYears();
        $scope.$apply();
        if ($scope.years.length > 0) {
            $scope.year = $scope.years[$scope.years.length - 1];
        }

        $scope.getCategories();
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

    $scope.getCategories = function () {
        $scope.categoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCategory,
            AllText: summaryReportResources.AllCategories,
            MaxHeight: 450,
            NoItemText: summaryReportResources.Symbol_NotApply,
            NoneSelectedText: summaryReportResources.No_category_found,
            NumberOfItemShowShortText: 0/*,
            SubOptionsName: 'ChildSettings',
            NumberOfSelectedLevel: 2*/
        };

        summaryReportService.getCategories()
            .then(function (data) {
                $scope.categoryOptions.DataList = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $uiBlockerService.unblockUI();
            });
    };

    $scope.updateSelectedCategory = function (categoryIdList) {
        $scope.selectedCategoryIds = categoryIdList;
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
        summaryReportService.getSummaryReport($scope.year.id, $scope.selectedCategoryIds)
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
        $scope.initController();
    });
} ]);    