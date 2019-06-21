(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_SettingManagement_ViewList"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getSettingRelations();
            $scope.safeApply();
        };
    };
})();