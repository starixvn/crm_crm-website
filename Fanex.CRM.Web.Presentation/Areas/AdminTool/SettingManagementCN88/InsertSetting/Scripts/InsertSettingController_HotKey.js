(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_SettingManagement_InsertSetting"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.insertSetting();
            $scope.safeApply();
        };
    };
})();