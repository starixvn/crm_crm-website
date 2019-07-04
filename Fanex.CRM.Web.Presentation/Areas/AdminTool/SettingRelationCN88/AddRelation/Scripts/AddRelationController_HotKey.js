(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_SettingRelation_AddRelation"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getMainSettingRelations();
            $scope.safeApply();
        };
        $scope.invokeHotKey_CtrlU = function () {
            $scope.addSettingRelations();
            $scope.safeApply();
        };
    };
})();