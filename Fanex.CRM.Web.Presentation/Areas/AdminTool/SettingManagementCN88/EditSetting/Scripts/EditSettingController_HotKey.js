(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "AdminTool_SettingManagement_EditSetting"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.editSetting();
            $scope.safeApply();
        };
    };
})();