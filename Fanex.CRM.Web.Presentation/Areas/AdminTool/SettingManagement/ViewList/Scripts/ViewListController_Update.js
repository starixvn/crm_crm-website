(function () {
    "use strict";
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedMainSetting = function (mainSetting) {
            var isChange = !$scope.getSettingRelationsParams.MainSetting ||
            $scope.getSettingRelationsParams.MainSetting.SettingType !== mainSetting.SettingType;
            $scope.getSettingRelationsParams.MainSetting = {
                SettingType: mainSetting.SettingType
            };

            $scope.safeApply();
            if (isChange) {
                $scope.getSettingRelations();
            }
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    };
})();