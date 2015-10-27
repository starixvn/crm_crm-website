(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* exported registerUpdateFunctions */
    function registerUpdateFunctions($scope) {
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
    }
})();