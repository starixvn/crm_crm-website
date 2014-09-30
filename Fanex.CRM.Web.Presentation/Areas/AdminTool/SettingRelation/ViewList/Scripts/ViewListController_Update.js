"use strict";
/* jshint unused: vars,
camelcase: false */
/* exported registerUpdateFunctions */
function registerUpdateFunctions($scope) {
    $scope.updateSelectedMainSetting = function (mainSetting) {
        $scope.getSettingRelationsParams.MainSetting = {
            Type: mainSetting.Type
        };
        $scope.safeApply();
    };
    $scope.completeSetFocus = function () {
        $scope.isSetFocus = false;
    };
}