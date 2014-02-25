"use strict";
/* global dynamicReportApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate: true,
initFromDate: true,
weekPickerManager */
dynamicReportApplication.controller('dynamicReportController', ['$scope', '$http', '$dynamicReportService',
function (
    $scope,
    $http,
    $dynamicReportService) {
    $scope.isSetFocus = false;
    $scope.isUpdatingDate = false;
    $scope.crmConstants = crmConstants;

    $scope.reportTypeOptions = null;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.fromWeekOptions = null;
    $scope.toWeekOptions = null;
    $scope.fromMonthOptions = null;
    $scope.toMonthOptions = null;
    $scope.fromYearOptions = null;
    $scope.toYearOptions = null;
    $scope.productOptions = null;
    $scope.siteOptions = null;
    $scope.statusOptions = null;
    $scope.responseOptions = null;
    $scope.searchCriteria = {
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        ReportType: null
    };
    $scope.initController = function () {
        $scope.getAllSetting();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "Report_DynamicReport"
        });

        $scope.initPicker();
    };
    $scope.initPicker = function () {
        $scope.reportTypeOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateReportType
        };

        $scope.$apply();

        $scope.fromDateOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromDate
        };
        $scope.updateFromDatePicker();

        $scope.toDateOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToDate
        };
        $scope.updateToDatePicker();

        weekPickerManager.registerWeekPicker();
        $scope.fromWeekOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromWeek
        };
        $scope.toWeekOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToWeek
        };

        currentDate = new Date(currentDate);
        initFromDate = new Date(initFromDate);
        $scope.fromMonthOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: initFromDate,
            MinDate: new Date(initFromDate.getFullYear() - 10, initFromDate.getMonth(), initFromDate.getDate()),
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
            MaxDate: currentDate,
            MinDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToMonth,
            SelectedYear: currentDate.getFullYear(),
            SelectedMonth: currentDate.getMonth() + 1
        };

        $scope.toMonthOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.fromYearOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: initFromDate,
            MinDate: new Date(initFromDate.getFullYear() - 10, initFromDate.getMonth(), initFromDate.getDate()),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromYear,
            SelectedYear: initFromDate.getFullYear()
        };

        $scope.fromYearOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.toYearOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            MinDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToYear,
            SelectedYear: currentDate.getFullYear()
        };

        $scope.toYearOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedProduct
        };

        $scope.siteOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSite,
            AllText: viewResources.All,
            IsIncludeNA: true
        };

        $scope.statusOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedStatus
        };
        $scope.responseOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedResponse
        };

        $scope.$apply();
    };
    $scope.getAllSetting = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $dynamicReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.reportTypeOptions.DataList = allSetting.PeriodTypeList;
                $scope.productOptions.DataList = allSetting.ProductList;
                $scope.siteOptions.DataList = allSetting.SiteList;
                $scope.statusOptions.DataList = allSetting.StatusList;
                $scope.responseOptions.DataList = allSetting.ResponseList;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.completedLoadFilterInfo = function () {
        $scope.searchCriteria.ReportType = $scope.reportTypeOptions.DataList[0];
        $scope.searchCriteria.Product = $scope.productOptions.DataList[0];
        $scope.searchCriteria.Site = $scope.siteOptions.DataList[0];
        $scope.searchCriteria.Status = $scope.statusOptions.DataList[0];
        $scope.searchCriteria.Response = $scope.responseOptions.DataList[0];

        $scope.unblockUI();
    };
    $scope.updateFromDate = function (selectedDate) {
        $scope.searchCriteria.FromDate = selectedDate;
        $scope.toDateOptions.MinDate = selectedDate;
        $scope.updateToDatePicker(true);
    };
    $scope.updateFromDatePicker = function (isOnApply) {
        $scope.fromDateOptions.IsRerenderPicker = true;
        if (!isOnApply) {
            $scope.$apply();
        }
    };
    $scope.updateToDatePicker = function (isOnApply) {
        $scope.toDateOptions.IsRerenderPicker = true;
        if (!isOnApply) {
            $scope.$apply();
        }
    };
    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.fromDateOptions.MaxDate = selectedDate;
        $scope.updateFromDatePicker(true);
    };
    $scope.updateReportType = function (reportType) {
        $scope.searchCriteria.ReportType = reportType;
        $scope.$apply();
    };
    $scope.updateFromWeek = function (fromDate) {
        $scope.searchCriteria.FromDate = fromDate;
    };
    $scope.updateToWeek = function (fromDate, toDate) {
        $scope.searchCriteria.ToDate = toDate;
    };
    $scope.updateFromMonth = function (month, year) {
        var selectedDate = new Date(year, month - 1, 1);
        var selectedYear = selectedDate.getFullYear();
        var toDate = $scope.searchCriteria.ToDate;
        var maxFromYear = selectedYear + 10;
        var maxFromMonth = 11;
        if (toDate.getFullYear() < maxFromYear) {
            maxFromYear = toDate.getFullYear();
            maxFromMonth = toDate.getMonth();
        }

        $scope.fromMonthOptions = {
            CurrentDate: selectedDate,
            InitDate: initFromDate,
            MaxDate: new Date(maxFromYear, maxFromMonth, 1),
            MinDate: new Date(selectedDate.getFullYear() - 10, selectedDate.getMonth(), 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromMonth,
            SelectedYear: year,
            SelectedMonth: month
        };

        var maxToYear = toDate.getFullYear() + 10;
        var maxToMonth = 11;
        if (currentDate.getFullYear() < maxToYear) {
            maxToYear = currentDate.getFullYear();
            maxToMonth = currentDate.getMonth();
        }

        $scope.toMonthOptions = {
            CurrentDate: toDate,
            InitDate: currentDate,
            MaxDate: new Date(maxToYear, maxToMonth, 1),
            MinDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateToMonth,
            SelectedYear: $scope.searchCriteria.ToDate.getFullYear(),
            SelectedMonth: $scope.searchCriteria.ToDate.getMonth() + 1
        };

        $scope.searchCriteria.FromDate = selectedDate;
        $scope.toMonthOptions.IsRerenderPicker = true;
        $scope.$apply();
    };
    $scope.updateToMonth = function (month, year) {
        var selectedDate = new Date(year, month - 1, 1);
        var selectedYear = selectedDate.getFullYear();
        var maxToYear = selectedYear + 10;
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

        $scope.fromMonthOptions = {
            CurrentDate: fromDate,
            InitDate: currentDate,
            MaxDate: new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate()),
            MinDate: new Date(fromDate.getFullYear() - 10, fromDate.getMonth(), 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromMonth,
            SelectedYear: $scope.searchCriteria.FromDate.getFullYear(),
            SelectedMonth: $scope.searchCriteria.FromDate.getMonth() + 1
        };

        $scope.searchCriteria.ToDate = selectedDate;
        $scope.fromMonthOptions.IsRerenderPicker = true;
        $scope.$apply();
    };
    $scope.updateFromYear = function (fromYear) {
        var selectedDate = new Date(fromYear, 1, 1);
        var selectedYear = fromYear;
        var toDate = $scope.searchCriteria.ToDate;
        var maxFromYear = selectedYear + 10;
        if (toDate.getFullYear() < maxFromYear) {
            maxFromYear = toDate.getFullYear();
        }

        $scope.fromYearOptions = {
            CurrentDate: selectedDate,
            InitDate: initFromDate,
            MaxDate: new Date(maxFromYear, 11, 1),
            MinDate: new Date(selectedDate.getFullYear() - 10, 1, 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromYear,
            SelectedYear: fromYear
        };

        var maxToYear = toDate.getFullYear() + 10;
        if (currentDate.getFullYear() < maxToYear) {
            maxToYear = currentDate.getFullYear();
        }

        $scope.toYearOptions = {
            CurrentDate: toDate,
            InitDate: currentDate,
            MaxDate: new Date(maxToYear, 11, 1),
            MinDate: new Date(selectedDate.getFullYear(), 1, 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateToYear,
            SelectedYear: $scope.searchCriteria.ToDate.getFullYear()
        };

        $scope.searchCriteria.FromDate = selectedDate;
        $scope.toYearOptions.IsRerenderPicker = true;
        $scope.searchCriteria.FromDate = new Date(fromYear, 1, 1);
        $scope.$apply();
    };
    $scope.updateToYear = function (toYear) {
        var selectedDate = new Date(toYear, 1, 1);
        var selectedYear = toYear;
        var maxToYear = selectedYear + 10;
        if (currentDate.getFullYear() < maxToYear) {
            maxToYear = currentDate.getFullYear();
        }

        var fromDate = $scope.searchCriteria.FromDate;
        $scope.toYearOptions = {
            CurrentDate: selectedDate,
            InitDate: currentDate,
            MaxDate: new Date(maxToYear, 11, 1),
            MinDate: new Date(fromDate.getFullYear(), 1, 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateToYear,
            SelectedYear: toYear
        };

        $scope.fromYearOptions = {
            CurrentDate: fromDate,
            InitDate: currentDate,
            MaxDate: new Date(selectedDate.getFullYear(), 11, 1),
            MinDate: new Date(fromDate.getFullYear() - 10, 1, 1),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromYear,
            SelectedYear: $scope.searchCriteria.FromDate.getFullYear()
        };

        $scope.searchCriteria.ToDate = selectedDate;
        $scope.fromYearOptions.IsRerenderPicker = true;
        $scope.searchCriteria.ToDate = new Date(toYear, 1, 1);
        $scope.$apply();
    };

    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };
    $scope.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    angular.element(document).ready(function () {
        $scope.initController();
        //        $scope.setFocus();
    });
} ]);