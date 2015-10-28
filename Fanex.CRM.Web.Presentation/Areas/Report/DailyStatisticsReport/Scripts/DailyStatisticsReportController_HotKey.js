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
                Id: "Report_DailyStatisticsReport"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getDailyStatisticsReport();
            $scope.safeApply();
        };
    }
})();