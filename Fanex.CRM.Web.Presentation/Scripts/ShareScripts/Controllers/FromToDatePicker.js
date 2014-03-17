"use strict";
/* global sharedControllerManager: true,
currentDate,
initFromDate */
/* jshint unused: vars */

var sharedControllerManager = sharedControllerManager || {};
sharedControllerManager.registerDatePicker = function ($scope) {
    $scope.isUpdatingDate = false;
    $scope.fromDateOptions = null;
    $scope.toDateOptions = null;

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
    $scope.updateFromDate = function (selectedDate) {
        $scope.searchCriteria.FromDate = selectedDate;
        $scope.toDateOptions.MinDate = selectedDate;
        $scope.updateToDatePicker(true);
    };
    $scope.updateToDate = function (selectedDate) {
        $scope.searchCriteria.ToDate = selectedDate;
        $scope.fromDateOptions.MaxDate = selectedDate;
        $scope.updateFromDatePicker(true);
    };

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
};