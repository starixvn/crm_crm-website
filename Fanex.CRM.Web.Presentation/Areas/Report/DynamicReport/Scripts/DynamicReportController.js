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

    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;
    $scope.periodTypeOptions = null;
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