(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_SettingRelation_ViewList"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getSettingRelations();
            $scope.safeApply();
        };
    }
})();