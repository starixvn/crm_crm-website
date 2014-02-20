"use strict";
/* global dynamicReportApplication,
hotKeyManager,
viewResources,
crmConstants,
currentDate,
customerId,
initFromDate */
dynamicReportApplication.controller('dynamicReportController', ['$scope', '$http', '$dynamicReportService',
function (
    $scope,
    $http,
    $dynamicReportService) {
    $scope.isSetFocus = false;
    $scope.periodTypeList = [];
    $scope.reportTypeList = [];
    $scope.isUpdatingDate = false;
    $scope.searchCriteria = {
        FromDate: initFromDate,
        ToDate: new Date(currentDate),
        MinToDate: new Date(initFromDate),
        MaxFromDate: new Date(currentDate)
    };
    $scope.initController = function () {
        $scope.getAllSetting();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "Report_DynamicReport"
        });
    };

    $scope.getAllSetting = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $dynamicReportService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.periodTypeList = allSetting.PeriodTypeList;
                $scope.reportTypeList = allSetting.ReportTypeList;
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