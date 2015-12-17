(function () {
    "use strict";
    /* global currentDate */
    /* exported registerOptions */
    window.registerOptions = function ($scope) {
        $scope.fromDateOptions = null;
        $scope.toDateOptions = null;
        $scope.defaultFocusOptions = null;
        $scope.downloadFileOptions = null;
        $scope.isUpdatingDate = false;
        $scope.ibcReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };
        $scope.licenseeReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };
        $scope.detailReportOptions = {
            DataList: null,
            IsResize: false,
            IsUpdateGrid: false
        };

        $scope.initDefaultOption = function () {
            $scope.fromDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MaxDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateFromDate,
                CompleteRender: $scope.completeRenderFromDate
            };

            $scope.toDateOptions = {
                CurrentDate: currentDate,
                InitDate: currentDate,
                MinDate: currentDate,
                IsRerenderPicker: false,
                OnChangeDateEvent: $scope.updateToDate,
                CompleteRender: $scope.completeRenderToDate
            };

            $scope.updateFromDatePicker();
            $scope.updateToDatePicker();

            $scope.defaultFocusOptions = {
                IsSetFocus: false,
                OnChangeEvent: $scope.completeSetFocus
            };
        };
    };
})();