(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Report_WeeklyStatisticsReport"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.getWeeklyStatisticsReport();
            $scope.safeApply();
        };
    };
})();