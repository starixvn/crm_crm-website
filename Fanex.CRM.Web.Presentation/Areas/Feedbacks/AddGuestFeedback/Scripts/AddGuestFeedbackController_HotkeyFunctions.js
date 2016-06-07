(function () {
    "use strict";
    /* jshint camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    window.registerHotkeyFunctions = function ($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Feedback_AddGuestFeedback"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.addFeedback();
            $scope.safeApply();
        };
        $scope.invokeHotKey_CtrlE = function () {
            $scope.relatedCustomerResultDirective.verifyUsernames($scope.customersInput);
        };
    };
})();