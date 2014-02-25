"use strict";
/* global dynamicReportApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate,
initFromDate */
dynamicReportApplication.controller('dynamicReportController', ['$scope', '$http', '$dynamicReportService',
function (
    $scope,
    $http,
    $dynamicReportService) {
    $scope.isSetFocus = false;
    $scope.periodTypeList = [];
    $scope.isUpdatingDate = false;

    $scope.periodTypeOptions = null;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.fromWeekOptions = null;
    $scope.toWeekOptions = null;
    $scope.fromMonthOptions = null;
    $scope.toMonthOptions = null;
    $scope.searchCriteria = {
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        PeriodType: null
    };
    $scope.initController = function () {
        $scope.getAllSetting();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "Report_DynamicReport"
        });

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

        $scope.periodTypeOptions = {
            DataList: $scope.periodTypeList,
            OnChangeEvent: $scope.updatePeriodType
        };

        $scope.$apply();
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
        $scope.fromMonthOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: new Date(currentDate.getFullYear() + 10, currentDate.getMonth(), currentDate.getDate()),
            MinDate: new Date(currentDate.getFullYear() - 10, currentDate.getMonth(), currentDate.getDate()),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromMonth
        };

        $scope.fromMonthOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.toMonthOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: new Date(currentDate.getFullYear() + 10, currentDate.getMonth(), currentDate.getDate()),
            MinDate: new Date(currentDate.getFullYear() - 10, currentDate.getMonth(), currentDate.getDate()),
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromWeek
        };

        $scope.toMonthOptions.IsRerenderPicker = true;
        $scope.$apply();
    };

    $scope.getAllSetting = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $dynamicReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.periodTypeOptions.DataList = allSetting.PeriodTypeList;

                $scope.searchCriteria.PeriodType = $scope.periodTypeOptions.DataList[0];
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.completedLoadFilterInfo = function () {
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
    $scope.updatePeriodType = function (periodType) {
        $scope.searchCriteria.PeriodType = periodType;
    };

    $scope.updateFromWeek = function (fromDate) {
        $scope.searchCriteria.FromDate = fromDate;
    };
    $scope.updateToWeek = function (fromDate, toDate) {
        $scope.searchCriteria.ToDate = toDate;
    };
    $scope.updateFromMonth = function (month, year) {
        var selectedDate = new Date(year, month, 1);
        $scope.fromMonthOptions = {
            CurrentDate: selectedDate,
            InitDate: initFromDate,
            MaxDate: new Date(selectedDate.getFullYear() + 10, selectedDate.getMonth(), selectedDate.getDate()),
            MinDate: new Date(selectedDate.getFullYear() - 10, selectedDate.getMonth(), selectedDate.getDate()),
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromMonth,
            SelectedYear: year,
            SelectedMonth: month
        };

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