"use strict";
/* exported registerUpdateFunctions */
function registerUpdateFunctions($scope) {
    $scope.updateSelectedSubject = function (subject) {
        $scope.searchTemplateParams.Subject = subject;
        $scope.safeApply();
    };
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
}