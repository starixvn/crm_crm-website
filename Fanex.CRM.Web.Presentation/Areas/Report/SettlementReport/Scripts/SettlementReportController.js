"use strict";
/* global settlementReportApplication,
currentDate: true,
sharedScriptManager,
hotKeyManager,
minReportDate */
/* jshint unused: vars */
/* jshint camelcase: false */
settlementReportApplication.controller('settlementReportController', [
    '$scope',
    '$http',
    'settlementReportService',
    '$uiBlockerService',
    '$resultMessageService',
    function (
        $scope,
        $http,
        settlementReportService,
        $uiBlockerService,
        $resultMessageService) {

        $scope.fromYearOptions = null;
        $scope.isUpdatingDate = false;
        $scope.defaultFocusOptions = null;
        $scope.settlementReportOptions = {
            DataList: null,
            IsUpdateGrid: false
        };
        $scope.searchCriteria = {
            Year: currentDate.getFullYear(),
            PageIndex: 0,
            PageSize: sharedScriptManager.cachedData.monthPagingDefaultItemsPerPage,
            IsGetFromCached: false,
            IsGetFullReport: false
        };
        $scope.lastSelectedParams = angular.copy($scope.searchCriteria);
        $scope.initController = function () {
            $scope.initPicker();
            $scope.registerHotKey();
            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };
            $scope.setFocus();
        };
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_SettlementReport"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getSettlementReport();
        };
        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.defaultFocusOptions.IsSetFocus = true;
            });
        };
        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };
        $scope.initPicker = function () {
            var minYear = new Date(currentDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
                    currentDate.getMonth(),
                    currentDate.getDate());
            if (minYear < minReportDate) {
                minYear = minReportDate;
            }

            $scope.fromYearOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                MinDate: minYear,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromYear,
                SelectedYear: currentDate.getFullYear()
            };

            $scope.fromYearOptions.IsRerenderPicker = true;
            $scope.$apply();
        };
        $scope.updateFromYear = function (fromYear, isAlreadyApply) {
            var selectedDate = new Date(fromYear, 0, 1);
            var selectedYear = fromYear;
            var maxFromYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var currentYear = currentDate.getFullYear();
            if (currentYear < maxFromYear) {
                maxFromYear = currentYear;
            }

            var minYear = new Date(selectedDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
                    selectedDate.getMonth(),
                    selectedDate.getDate());
            if (minYear < minReportDate) {
                minYear = minReportDate;
            }

            $scope.fromYearOptions = {
                CurrentDate: selectedDate,
                InitDate: selectedDate,
                MaxDate: new Date(maxFromYear, 11, 1),
                MinDate: minYear,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromYear,
                SelectedYear: fromYear
            };

            $scope.searchCriteria.Year = fromYear;
        };
        $scope.getSettlementReport = function () {
            $uiBlockerService.blockUI();
            var getReportCriteria = {
                Year: $scope.searchCriteria.Year,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false,
                IsGetFullReport: false
            };

            $scope.lastSelectedParams = getReportCriteria;
            settlementReportService.getSettlementReport(getReportCriteria)
                .then(function (data) {
                    $scope.settlementReportOptions.DataList = data.Result;
                    $scope.settlementReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.pagingSettlementReport = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;
            settlementReportService.getSettlementReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.settlementReportOptions.DataList = data.Result;
                    $scope.settlementReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "SettlementReport/ExportSettlementReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };
        angular.element(document).ready(function () {
            $uiBlockerService.blockUI();
            $uiBlockerService.unblockUI();
            $scope.initController();
        });
    } ]);