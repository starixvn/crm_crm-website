(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    function registerHotkeyFunctions($scope) {
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
    }
})();