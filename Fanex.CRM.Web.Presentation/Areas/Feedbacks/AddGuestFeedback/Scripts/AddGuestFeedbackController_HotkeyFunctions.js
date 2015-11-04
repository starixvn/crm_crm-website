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
                Id: "Feedback_AddGuestFeedback"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.addFeedback();
            $scope.safeApply();
        };
        $scope.invokeHotKey_CtrlE = function () {
            if ($scope.addGuestFeedbackParams.RelatedCustomerId === '' ||
                $scope.addGuestFeedbackParams.RelatedCustomerId === null ||
                $scope.addGuestFeedbackParams.RelatedCustomerId === undefined) {
                return;
            }

            $scope.verifyUserName();
        };
    };
})();