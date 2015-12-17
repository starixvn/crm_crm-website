(function () {
    "use strict";
    /* global sharedScriptManager,
    hotKeyManager,
    summaryReportResources */
    /* jshint camelcase: false */
    angular.module('report-summaryreport-application')
        .controller('summaryReportController', ['$scope', 'SummaryReportService', '$uiBlockerService', '$resultMessageService',
    function ($scope, summaryReportService, $uiBlockerService, $resultMessageService) {
        $scope.years = [];
        $scope.year = 0;
        $scope.siteTypes = [];
        $scope.site = undefined;
        $scope.categoryOptions = {};
        $scope.selectedCategoryIds = '';
        $scope.defaultFocusOptions = null;
        $scope.summaryReport;

        var _lastSubmitParams;

        $scope.initController = function () {
            $uiBlockerService.blockUI();

            $scope.getAllSettings();
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

        $scope.getAllSettings = function () {
            $scope.categoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedCategory,
                AllText: summaryReportResources.AllCategories,
                MaxHeight: 450,
                NoItemText: summaryReportResources.Symbol_NotApply,
                NoneSelectedText: summaryReportResources.No_category_found,
                NumberOfItemShowShortText: 0
            };

            summaryReportService.getAllSettings()
                .then(function (data) {
                    $scope.years = data.Result.YearRange;
                    if ($scope.years.length > 0) {
                        $scope.year = $scope.years[$scope.years.length - 1];
                    }

                    $scope.siteTypes = data.Result.SiteTypes;
                    $scope.site = $scope.siteTypes[0];

                    $scope.categoryOptions.DataList = data.Result.Categories;
                    $uiBlockerService.unblockUI();
                },
                function () {
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
            _lastSubmitParams = {
                year: $scope.year.Id,
                siteType: $scope.site.Id,
                categories: $scope.selectedCategoryIds
            };
            summaryReportService.getSummaryReport(_lastSubmitParams)
                .then(function (data) {
                    $scope.summaryReport = data.Result;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                    $uiBlockerService.unblockUI();
                });
        };

        $scope.exportSummaryReportExcel = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + 'SummaryReport/ExportSummaryReportExcel',
                Params: _lastSubmitParams
            };

            $scope.$apply();
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);
})();