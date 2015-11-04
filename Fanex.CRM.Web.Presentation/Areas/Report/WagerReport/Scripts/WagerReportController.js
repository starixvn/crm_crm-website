(function () {
    "use strict";
    /* global sharedScriptManager,
    hotKeyManager,
    minReportDate,
    currentDate */
    /* jshint camelcase: false */
    angular.module('report-wagerreport-application')
        .controller('wagerReportController', ['$scope', 'WagerReportService', '$uiBlockerService', '$resultMessageService',
function ($scope, wagerReportService, $uiBlockerService, $resultMessageService) {
    $scope.years = [];
    $scope.year = 0;
    $scope.defaultFocusOptions = null;
    $scope.wagerReport;

    $scope.initController = function () {
        $scope.years = $scope.getYears();
        $scope.$apply();

        if ($scope.years.length > 0) {
            $scope.year = $scope.years[$scope.years.length - 1];
        }

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
        $uiBlockerService.blockUI();
        wagerReportService.getWagerReport($scope.year.id)
            .then(function (data) {
                $scope.wagerReport = data.Result;
                $uiBlockerService.unblockUI();
            },
            function (data) {
                /* jshint unused: vars */
                sharedScriptManager.handleErrorMessage(data.Result, $resultMessageService);
                $uiBlockerService.unblockUI();
            });
    };

    $scope.exportWagerReportExcel = function () {
        $scope.downloadFileOptions = {
            URL: window.rootUrl + 'WagerReport/ExportWagerReportExcel',
            Params: { year: $scope.year.id }
        };

        $scope.$apply();
    };

    angular.element(document).ready(function () {
        $uiBlockerService.blockUI();
        $uiBlockerService.unblockUI();
        $scope.initController();
    });
} ]);
})();