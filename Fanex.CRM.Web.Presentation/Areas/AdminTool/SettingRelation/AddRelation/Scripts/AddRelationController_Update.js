(function () {
    "use strict";
    /* exported registerUpdateFunctions */
    window.registerUpdateFunctions = function ($scope) {
        $scope.updateSelectedMainSetting = function (mainSetting) {
            $scope.getMainSettingRelationsParams.MainSetting = {
                SettingType: mainSetting.SettingType,
                Id: mainSetting.Id
            };
            $scope.safeApply();
            $scope.getMainSettingRelations();
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        $scope.updateOrder = function (rowIndex, order) {
            $scope.mainSettingRelationView[rowIndex].DisplayOrder = order;
            $scope.safeApply();
        };
        $scope.updateIsSub = function (rowIndex, isCheck) {
            $scope.mainSettingRelationView[rowIndex].IsSubSetting = isCheck;
            $scope.safeApply();
        };
    };
})();