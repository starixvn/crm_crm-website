"use strict";
/* global statisticReportApplication,
initFromDate: true,
currentDate: true,
sharedScriptManager,
hotKeyManager,
crmConstants,
minReportDate,
moment */
/* jshint unused: vars */
/* jshint camelcase: false */
statisticReportApplication.controller('statisticReportController', [
    '$scope',
    '$http',
    'statisticReportService',
    '$uiBlockerService',
    '$resultMessageService',
    function (
        $scope,
        $http,
        statisticReportService,
        $uiBlockerService,
        $resultMessageService) {
        $scope.isShowFilter = true;
        $scope.crmConstants = crmConstants;
        $scope.statisticReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };
        $scope.reportTypeOptions = null;
        $scope.isUpdatingDate = false;
        $scope.mainCategoryOptions = null;
        $scope.subCategoryOptions = null;
        $scope.downloadFileOptions = null;
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.fromMonthOptions = null;
        $scope.toMonthOptions = null;
        $scope.defaultFocusOptions = null;
        $scope.searchCriteria = {
            ReportType: {
                Id: crmConstants.Enums.PeriodType.Daily
            },

            FromDate: new Date(initFromDate),
            ToDate: new Date(currentDate),

            MainCategory: null,
            SubCategory: null,
            PageIndex: 0,
            PageSize: 10,
            IsGetFromCached: false,
            IsGetFullReport: false
        };
        $scope.lastSelectedParams = angular.copy($scope.searchCriteria);

        $scope.initController = function () {
            $scope.initPicker();
            $scope.getAllSetting();
            $scope.registerHotKey();
            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };
            $scope.setFocus();
        };
        $scope.showHideFilterBox = function () {
            $scope.isShowFilter = !$scope.isShowFilter;
            $scope.statisticReportOptions.IsUpdateGrid = true;
            $scope.statisticReportOptions.IsResize = true;
        };
        $scope.getAllSetting = function () {
            $uiBlockerService.blockUI();
            statisticReportService.getAllSetting()
                .then(function (data) {
                    var allSetting = data.Result;
                    $scope.completedLoadFilterInfo(allSetting);
                },
                function (data) {
                    $scope.completedLoadFilterInfo(null);
                });
        };
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_StatisticReport"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getStatisticReport();
        };
        $scope.completedLoadFilterInfo = function (allSetting) {
            if (allSetting) {
                $scope.reportTypeOptions.DataList = allSetting.PeriodTypeList;
                $scope.mainCategoryOptions.DataList = allSetting.MainCategoryList;
                $scope.subCategoryOptions.DataList = $scope.mainCategoryOptions.DataList[0].ChildSettings;
                $scope.searchCriteria.ReportType = $scope.reportTypeOptions.DataList[0];
                $scope.searchCriteria.MainCategory = $scope.mainCategoryOptions.DataList[0];
                $scope.searchCriteria.SubCategory = $scope.subCategoryOptions.DataList[0];
            }

            $uiBlockerService.unblockUI();
        };
        $scope.initPicker = function () {
            $scope.reportTypeOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateReportType
            };
            $scope.$apply();

            currentDate = new Date(currentDate);
            initFromDate = new Date(initFromDate);

            var beginYearDropdown = new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
                    initFromDate.getMonth(),
                    initFromDate.getDate());
            $scope.fromMonthOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: initFromDate.getFullYear(),
                SelectedMonth: initFromDate.getMonth() + 1
            };
            $scope.fromMonthOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.toMonthOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: currentDate.getFullYear(),
                SelectedMonth: currentDate.getMonth() + 1
            };
            $scope.toMonthOptions.IsRerenderPicker = true;
            $scope.$apply();

            $scope.mainCategoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedMainCategory
            };
            $scope.subCategoryOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedSubCategory
            };
            $scope.$apply();

            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: initFromDate,
                MinDate: minReportDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromDate,
                IsSetDate: false
            };

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MinDate: minReportDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToDate,
                IsSetDate: false
            };

            $scope.updateFromDatePicker();
            $scope.updateToDatePicker();
        };
        $scope.updateFromDatePicker = function (isOnApply) {
            //            $scope.fromDateOptions.IsRerenderPicker = true;
            if (!isOnApply) {
                $scope.$apply(function () {
                    $scope.fromDateOptions.IsRerenderPicker = true;
                });
            }
        };
        $scope.updateToDatePicker = function (isOnApply) {
            if (!isOnApply) {
                $scope.$apply(function () {
                    $scope.toDateOptions.IsRerenderPicker = true;
                });
            }
        };
        $scope.updateFromDate = function (selectedDate, isAlreadyApply) {
            selectedDate = minReportDate < selectedDate ? selectedDate : minReportDate;
            $scope.searchCriteria.FromDate = selectedDate;
            if ($scope.searchCriteria.ToDate < selectedDate) {
                $scope.toDateOptions.IsSetDate = true;
                $scope.toDateOptions.InitDate = selectedDate;
                $scope.updateToDatePicker(isAlreadyApply);
            } else if (sharedScriptManager.getEndDateOfMonth(selectedDate) < $scope.searchCriteria.ToDate) {
                $scope.toDateOptions.IsSetDate = true;
                $scope.toDateOptions.InitDate = sharedScriptManager.getEndDateOfMonth(selectedDate);
                $scope.updateToDatePicker(isAlreadyApply);
            }

            $scope.updateFromDatePicker(isAlreadyApply);
        };
        $scope.updateToDate = function (selectedDate, isAlreadyApply) {
            selectedDate = minReportDate < selectedDate ? selectedDate : minReportDate;
            $scope.searchCriteria.ToDate = selectedDate;
            if (selectedDate < $scope.searchCriteria.FromDate) {
                $scope.fromDateOptions.IsSetDate = true;
                $scope.fromDateOptions.InitDate = selectedDate;
                $scope.updateFromDatePicker(isAlreadyApply);
            } else if ($scope.searchCriteria.FromDate < sharedScriptManager.getStartDateOfMonth(selectedDate)) {
                $scope.fromDateOptions.IsSetDate = true;
                $scope.fromDateOptions.InitDate = sharedScriptManager.getStartDateOfMonth(selectedDate);
                $scope.updateFromDatePicker(isAlreadyApply);
            }

            $scope.updateToDatePicker(isAlreadyApply);
        };
        $scope.updateReportType = function (reportType) {
            $scope.searchCriteria.ReportType = reportType;
            var reportTypeId = parseInt(reportType.Id);

            $scope.searchCriteria.FromDate = new Date(initFromDate);
            $scope.searchCriteria.ToDate = new Date(currentDate);
            switch (reportTypeId) {
                case crmConstants.Enums.PeriodType.Daily:
                    $scope.fromDateOptions.IsRerenderPicker = true;
                    $scope.toDateOptions.IsRerenderPicker = true;
                    break;
                case crmConstants.Enums.PeriodType.Monthly:
                    $scope.fromMonthOptions.IsRerenderPicker = true;
                    $scope.toMonthOptions.IsRerenderPicker = true;
                    break;
            }

            $scope.$apply();
        };
        $scope.updateSelectedMainCategory = function (mainCategory) {
            $scope.searchCriteria.MainCategory = mainCategory;
            $scope.subCategoryOptions.DataList = mainCategory.ChildSettings;
            $scope.searchCriteria.SubCategory = mainCategory.ChildSettings[0];
            $scope.$apply();
        };
        $scope.updateSelectedSubCategory = function (subCategory) {
            $scope.searchCriteria.SubCategory = subCategory;
            $scope.$apply();
        };
        $scope.updateFromMonth = function (month, year, isAlreadyApply) {
            if (isNaN(year)) {
                return;
            }

            var selectedDate = new Date(year, month - 1, 1);
            var selectedYear = selectedDate.getFullYear();

            var toDate = $scope.searchCriteria.ToDate;
            var maxFromYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxFromMonth = 11;
            if (toDate.getFullYear() < maxFromYear) {
                maxFromYear = toDate.getFullYear();
                maxFromMonth = toDate.getMonth();
            }

            var beginYearDropdown = new Date(selectedDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, selectedDate.getMonth(), 1);
            $scope.fromMonthOptions = {
                CurrentDate: selectedDate,
                InitDate: initFromDate,
                MaxDate: new Date(maxFromYear, maxFromMonth, 1),
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: year,
                SelectedMonth: month
            };

            var maxToYear = toDate.getFullYear() + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxToMonth = 11;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
                maxToMonth = currentDate.getMonth();
            }

            var selectToDate = toDate;
            if (selectedYear !== toDate.getFullYear()) {
                selectToDate = sharedScriptManager.getEndDateOfYear(selectedYear);
            }

            $scope.toMonthOptions = {
                CurrentDate: selectToDate,
                InitDate: currentDate,
                MinDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
                MaxDate: new Date(maxToYear, maxToMonth, 1),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: selectToDate.getFullYear(),
                SelectedMonth: selectToDate.getMonth() + 1
            };

            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toMonthOptions.IsRerenderPicker = true;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };
        $scope.updateToMonth = function (month, year, isAlreadyApply) {
            if (isNaN(year)) {
                return;
            }

            var selectedDate = new Date(year, month - 1, 1);
            var selectedYear = selectedDate.getFullYear();
            var maxToYear = selectedYear + sharedScriptManager.cachedData.numberOfYearPerSelection;
            var maxToMonth = 11;
            if (currentDate.getFullYear() < maxToYear) {
                maxToYear = currentDate.getFullYear();
                maxToMonth = currentDate.getMonth();
            }

            var fromDate = $scope.searchCriteria.FromDate;
            $scope.toMonthOptions = {
                CurrentDate: selectedDate,
                InitDate: currentDate,
                MaxDate: new Date(maxToYear, maxToMonth, 1),
                MinDate: new Date(fromDate.getFullYear(), fromDate.getMonth(), fromDate.getDate()),
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateToMonth,
                SelectedYear: year,
                SelectedMonth: month
            };

            var beginYearDropdown = new Date(fromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection, fromDate.getMonth(), 1);
            var selectFromDate = fromDate;
            if (selectedYear !== fromDate.getFullYear()) {
                selectFromDate = sharedScriptManager.getStartDateOfYear(selectedYear);
            }

            $scope.fromMonthOptions = {
                CurrentDate: selectFromDate,
                InitDate: currentDate,
                MaxDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
                MinDate: beginYearDropdown < new Date(minReportDate) ? new Date(minReportDate) : beginYearDropdown,
                IsRerenderPicker: false,
                OnChangeEvent: $scope.updateFromMonth,
                SelectedYear: selectFromDate.getFullYear(),
                SelectedMonth: selectFromDate.getMonth() + 1
            };

            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromMonthOptions.IsRerenderPicker = true;
            if (!isAlreadyApply) {
                $scope.$apply();
            }
        };

        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };
        $scope.setFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
            $scope.$apply();
        };
        $scope.getStatisticReport = function () {
            $uiBlockerService.blockUI();
            var getReportCriteria = {
                ReportType: $scope.searchCriteria.ReportType.Id,
                FromDate: moment($scope.searchCriteria.FromDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),
                ToDate: moment($scope.searchCriteria.ToDate).format(crmConstants.Enums.FormatConstant.ShortDate_String_Moment),

                MainCategoryId: $scope.searchCriteria.MainCategory.Id,
                SubCategoryId: $scope.searchCriteria.SubCategory.Id,
                PageIndex: $scope.searchCriteria.PageIndex,
                PageSize: $scope.searchCriteria.PageSize,
                IsGetFromCached: false,
                IsGetFullReport: false
            };

            $scope.lastSelectedParams = getReportCriteria;
            statisticReportService.getStatisticReport(getReportCriteria)
                .then(function (data) {
                    $scope.statisticReportOptions.DataList = data.Result;
                    $scope.statisticReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.pagingStatisticReport = function (pageIndex, pageSize) {
            $uiBlockerService.blockUI();
            $scope.lastSelectedParams.PageIndex = pageIndex;
            $scope.lastSelectedParams.PageSize = pageSize;
            $scope.lastSelectedParams.IsGetFromCached = true;
            statisticReportService.getStatisticReport($scope.lastSelectedParams)
                .then(function (data) {
                    $scope.statisticReportOptions.DataList = data.Result;
                    $scope.statisticReportOptions.IsUpdateGrid = true;
                    $uiBlockerService.unblockUI();
                },
                function (data) {
                    sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                    $uiBlockerService.unblockUI();
                });
        };
        $scope.exportReport = function () {
            $scope.downloadFileOptions = {
                URL: window.rootUrl + "StatisticReport/ExportStatisticReport",
                Params: $scope.lastSelectedParams
            };

            $scope.$apply();
        };
        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.defaultFocusOptions.IsSetFocus = true;
            });
        };

        angular.element(document).ready(function () {
            $scope.initController();
        });
    } ]);