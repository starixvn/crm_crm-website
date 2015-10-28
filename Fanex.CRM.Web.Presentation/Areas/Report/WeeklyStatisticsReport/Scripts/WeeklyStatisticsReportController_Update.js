(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* exported registerUpdateFunctions */
    function registerUpdateFunctions($scope) {
        $scope.updateFromDatePicker = function () {
            $scope.fromDateOptions.IsRerenderPicker = true;
            $scope.safeApply();
        };
        $scope.updateToDatePicker = function () {
            $scope.toDateOptions.IsRerenderPicker = true;
            $scope.safeApply();
        };
        $scope.setFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = true;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.defaultFocusOptions.IsSetFocus = false;
        };
        $scope.updateFromDate = function (selectedDate) {
            $scope.searchCriteria.FromDate = selectedDate;
            $scope.toDateOptions.MinDate = selectedDate;
            $scope.updateToDatePicker();
            $scope.completeRenderToDate();
        };
        $scope.updateToDate = function (selectedDate) {
            $scope.searchCriteria.ToDate = selectedDate;
            $scope.fromDateOptions.MaxDate = selectedDate;
            $scope.updateFromDatePicker();
            $scope.completeRenderFromDate();
        };
        $scope.completeRenderFromDate = function () {
            $scope.fromDateOptions.IsRerenderPicker = false;
            $scope.safeApply();
        };
        $scope.completeRenderToDate = function () {
            $scope.toDateOptions.IsRerenderPicker = false;
            $scope.safeApply();
        };
    }
})();