(function () {
    "use strict";
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedParentSetting = function (parentSetting) {
            $scope.insertSettingParams.NewSetting.ParentId = parentSetting.Id;
            $scope.safeApply();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
    };
})();