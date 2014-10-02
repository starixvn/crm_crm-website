"use strict";
/* jshint unused: vars,
camelcase: false */
/* exported registerUpdateFunctions */
function registerUpdateFunctions($scope) {
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
}