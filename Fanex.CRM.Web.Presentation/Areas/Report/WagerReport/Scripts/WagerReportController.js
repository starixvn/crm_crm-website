'use strict';
/* global wagerReportApplication,
hotKeyManager,
minReportDate,
currentDate */
/* jshint camelcase: false */
wagerReportApplication.controller('wagerReportController', ['$scope', 'WagerReportService',
function ($scope, wagerReportService) {
    $scope.years = [];
    $scope.year = 0;
    $scope.isSearched = false;
    $scope.defaultFocusOptions = null;
    $scope.wagerReport;

    $scope.initController = function () {
        $scope.years = $scope.getYears();
        $scope.$apply();

        $scope.year = $scope.years[0];
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: 'WagerReport'
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
        $scope.getWagerReport();
    };

    $scope.setFocus = function () {
        $scope.$apply(function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
        });
    };

    $scope.completeSetFocus = function () {
        $scope.defaultFocusOptions.IsSetFocus = false;
    };

    $scope.getWagerReport = function () {
        $scope.blockUI();
        wagerReportService.getWagerReport($scope.year.id)
            .then(function (data) {
                $scope.wagerReport = data.Result;
                $scope.isSearched = true;
                $scope.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                $scope.isSearched = true;
                $scope.unblockUI();
            });
    };

    /* block and unblock UI functions */
    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };

    $scope.blockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };

    angular.element(document).ready(function () {
        $scope.initController();
    });
} ]);    