(function () {
    "use strict";
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedMainSetting = function (mainSetting) {
            $scope.getSettingRelationsParams.MainSetting = {                
                SettingType: mainSetting.SettingNameEn,
                SettingName: mainSetting.SettingName
            };
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    };
})();