"use strict";
/* jshint unused: vars,
camelcase: false */
/* exported registerUpdateFunctions */
function registerUpdateFunctions($scope) {
    $scope.updateSelectedParentSetting = function (parentSetting) {
        $scope.insertSettingParams.NewSetting.ParentId = parentSetting.Id;
        $scope.safeApply();
    };
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
}