"use strict";
/* jshint unused: vars,
camelcase: false */
/* exported registerOptions */
function registerOptions($scope) {
    $scope.subjectOptions = null;
    $scope.isSetFocus = false;
    $scope.messageResultOptions = {
        Message: '',
        IsShowMessage: false,
        IsSuccess: false
    };
    $scope.initDefaultOption = function () {

    };
}